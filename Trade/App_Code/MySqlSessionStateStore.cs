using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.Data;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;

namespace Kimpel.MySqlSessionDll
{
    /// <summary>
    /// Author:         Harry Kimpel
    /// Website:        kimpel.com
    /// Created:        2007-07-09
    /// Version:        v.0.3
    /// Database:       MySql
    /// Ported From:    http://msdn2.microsoft.com/en-US/library/ms178588(VS.80).aspx
    /// Changes:
    ///                 2009-06-23: fixed method RemoveItem () ('DELETE * FROM ...' --> 'DELETE FROM ...'), some formatting
    ///                 2009-11-30: supports MySQL Connector/Net 6.2
    ///                 2009-11-30: added cluster db support
    ///                 2009-11-30: fixed timeout Minutes to TotalMinutes
    ///                 2009-11-30: changed namespace to Kimpel.MySqlSessionDll
    /// 
    /// This session state store provider supports the following schema:
    /// 
    ///     CREATE TABLE sessions
    ///     (
    ///         SessionId       Text(80)  NOT NULL,
    ///         ApplicationName Text(255) NOT NULL,
    ///         Created         Datetime  NOT NULL,
    ///         Expires         Datetime  NOT NULL,
    ///         LockDate        Datetime  NOT NULL,
    ///         LockId          Integer   NOT NULL,
    ///         Timeout         Integer   NOT NULL,
    ///         Locked          YesNo     NOT NULL,
    ///         SessionItems    Memo,
    ///         Flags           Integer   NOT NULL,
    ///             CONSTRAINT PKSessions PRIMARY KEY (SessionId, ApplicationName)
    ///     )
    /// 
    /// This session state store provider does not automatically clean up
    /// expired session item data. It is recommended
    /// that you periodically delete expired session information from the
    /// data store with the following code (where 'conn' is the MySqlConnection
    /// for the session state store provider):
    /// 
    ///     string commandString = "DELETE FROM sessions WHERE Expires < ?Expires";
    ///     MySqlConnection conn = new MySqlConnection(connectionString);
    ///     MySqlCommand cmd = new MySqlCommand(commandString, conn);
    ///     cmd.Parameters.Add("?Expires", MySqlDbType.DateTime).Value = DateTime.Now;
    ///     conn.Open();
    ///     cmd.ExecuteNonQuery();
    ///     conn.Close();
    /// 
    ///  Alternatively, one can set a Scheduled Task for execute the following:
    ///     mysql -u<UserName> -p<Password> -h<DB Host Name> DB_Name -e"DELETE FROM sessions WHERE Expires < NOW();"
    /// </summary>
    public sealed class MySqlSessionStateStore : SessionStateStoreProviderBase
    {
        #region private members
        private SessionStateSection pConfig = null;
        private List<string> pConnectionStringList;
        private string eventSource = "MySqlSessionStateStore";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please contact your administrator.";
        private string pApplicationName;
        #endregion private members

        #region properties
        /// <summary>
        /// If false, exceptions are thrown to the caller. If true,
        /// exceptions are written to the event log.
        /// </summary>
        private bool pWriteExceptionsToEventLog = false;
        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        /// <summary>
        /// The ApplicationName property is used to differentiate sessions
        /// in the data source by application.
        /// </summary>
        public string ApplicationName
        {
            get { return pApplicationName; }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Initialize values from web.config.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "MySqlSessionStateStore";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "GACMySqlSessionDll MySql Session State Store provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            //
            // Initialize the ApplicationName property.
            //
            pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;


            //
            // Get <sessionState> configuration element.
            //
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(ApplicationName);
            pConfig = (SessionStateSection)cfg.GetSection("system.web/sessionState");


            //
            // Initialize connection string.
            //
            //pConnectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];
            pConnectionStringList = new List<string>();
            if (config["connectionStringName"] != null)
                pConnectionStringList.Add(ConfigurationManager.ConnectionStrings[config["connectionStringName"]].ConnectionString);
            if (config["connectionStringList"] != null)
            {
                foreach (string Name in config["connectionStringList"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    pConnectionStringList.Add(ConfigurationManager.ConnectionStrings[Name].ConnectionString);
                }
            }

            //if(pConnectionStringSettings == null ||
            //        pConnectionStringSettings.ConnectionString.Trim() == "")
            if (pConnectionStringList.Count == 0)
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            //connectionString = pConnectionStringSettings.ConnectionString;

            //
            // Initialize WriteExceptionsToEventLog
            //
            pWriteExceptionsToEventLog = false;

            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                {
                    pWriteExceptionsToEventLog = true;
                }
            }
        }

        /// <summary>
        /// SessionStateStoreProviderBase members
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// SessionStateProviderBase.SetItemExpireCallback
        /// </summary>
        /// <param name="expireCallback"></param>
        /// <returns></returns>
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }

        /// <summary>
        /// SessionStateProviderBase.SetAndReleaseItemExclusive
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="lockId"></param>
        /// <param name="newItem"></param>
        public override void SetAndReleaseItemExclusive(
                HttpContext context,
                string id,
                SessionStateStoreData item,
                object lockId,
                bool newItem)
        {
            // Serialize the SessionStateItemCollection as a string.
            string sessItems = Serialize((SessionStateItemCollection)item.Items);

            // try each server in the cluster until successful
            for (int iConnectionString = 0; iConnectionString < pConnectionStringList.Count; iConnectionString++)
            {
                string connectionString = pConnectionStringList[iConnectionString];

                MySqlConnection conn = new MySqlConnection(connectionString);
                MySqlCommand cmd;
                MySqlCommand deleteCmd = null;

                if (newItem)
                {
                    // MySqlCommand to clear an existing expired session if it exists.
                    deleteCmd = new MySqlCommand("DELETE FROM sessions " +
                            "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND Expires < ?Expires", conn);
                    deleteCmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    deleteCmd.Parameters.Add
                        ("?ApplicationName", MySqlDbType.VarChar, 255).Value = ApplicationName;
                    deleteCmd.Parameters.Add
                        ("?Expires", MySqlDbType.DateTime).Value = DateTime.Now;

                    // MySqlCommand to insert the new session item.
                    cmd = new MySqlCommand("INSERT INTO sessions " +
                        " (SessionId, ApplicationName, Created, Expires, " +
                        "  LockDate, LockId, Timeout, Locked, SessionItems, Flags) " +
                        " Values(?SessionId, ?ApplicationName, ?Created, ?Expires, " +
                        "  ?LockDate, ?LockId, ?Timeout, ?Locked, ?SessionItems, ?Flags)", conn);
                    cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    cmd.Parameters.Add
                        ("?ApplicationName", MySqlDbType.VarChar, 255).Value = ApplicationName;
                    cmd.Parameters.Add
                        ("?Created", MySqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add
                        ("?Expires", MySqlDbType.DateTime).Value = DateTime.Now.AddMinutes((Double)item.Timeout);
                    cmd.Parameters.Add
                        ("?LockDate", MySqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = 0;
                    cmd.Parameters.Add
                        ("?Timeout", MySqlDbType.Int32).Value = item.Timeout;
                    cmd.Parameters.Add("?Locked", MySqlDbType.Int32).Value = 0;
                    cmd.Parameters.Add
                        ("?SessionItems", MySqlDbType.VarChar, sessItems.Length).Value = sessItems;
                    cmd.Parameters.Add("?Flags", MySqlDbType.Int32).Value = 0;
                }
                else
                {
                    // MySqlCommand to update the existing session item.
                    cmd = new MySqlCommand(
                        "UPDATE sessions SET Expires = ?Expires, SessionItems = ?SessionItems, Locked = ?Locked " +
                        " WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND LockId = ?LockId", conn);
                    cmd.Parameters.Add("?Expires", MySqlDbType.DateTime).Value =
                        DateTime.Now.AddMinutes((Double)item.Timeout);
                    cmd.Parameters.Add("?SessionItems",
                        MySqlDbType.Text, sessItems.Length).Value = sessItems;
                    cmd.Parameters.Add("?Locked", MySqlDbType.Int32).Value = 0;
                    cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                        255).Value = ApplicationName;
                    cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;
                }

                try
                {
                    conn.Open();

                    if (deleteCmd != null)
                        deleteCmd.ExecuteNonQuery();

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                        WriteToEventLog(e, "SetAndReleaseItemExclusive");
                    if (iConnectionString < pConnectionStringList.Count - 1)
                        continue;
                    if (WriteExceptionsToEventLog)
                    {
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                        throw e;
                }
                finally
                {
                    conn.Close();
                }
                break; // No Exception was thrown so must have been successful on this server of the cluster so done.
            }
        }

        /// <summary>
        /// SessionStateProviderBase.GetItem
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="locked"></param>
        /// <param name="lockAge"></param>
        /// <param name="lockId"></param>
        /// <param name="actionFlags"></param>
        /// <returns></returns>
        public override SessionStateStoreData GetItem(
                HttpContext context,
                string id,
                out bool locked,
                out TimeSpan lockAge,
                out object lockId,
                out SessionStateActions actionFlags)
        {
            return GetSessionStoreItem(false, context, id, out locked,
                out lockAge, out lockId, out actionFlags);
        }

        /// <summary>
        /// SessionStateProviderBase.GetItemExclusive
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="locked"></param>
        /// <param name="lockAge"></param>
        /// <param name="lockId"></param>
        /// <param name="actionFlags"></param>
        /// <returns></returns>
        public override SessionStateStoreData GetItemExclusive(
                HttpContext context,
                string id,
                out bool locked,
                out TimeSpan lockAge,
                out object lockId,
                out SessionStateActions actionFlags)
        {
            return GetSessionStoreItem(true, context, id, out locked,
                out lockAge, out lockId, out actionFlags);
        }

        /// <summary>
        /// GetSessionStoreItem is called by both the GetItem and
        /// GetItemExclusive methods. GetSessionStoreItem retrieves the
        /// session data from the data source. If the lockRecord parameter
        /// is true (in the case of GetItemExclusive), then GetSessionStoreItem
        /// locks the record and sets a new LockId and LockDate.
        /// </summary>
        /// <param name="lockRecord"></param>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="locked"></param>
        /// <param name="lockAge"></param>
        /// <param name="lockId"></param>
        /// <param name="actionFlags"></param>
        /// <returns></returns>
        private SessionStateStoreData GetSessionStoreItem(
                bool lockRecord,
                HttpContext context,
                string id,
                out bool locked,
                out TimeSpan lockAge,
                out object lockId,
                out SessionStateActions actionFlags)
        {
            // Initial values for return value and out parameters.
            SessionStateStoreData item = null;
            lockAge = TimeSpan.Zero;
            lockId = null;
            locked = false;
            actionFlags = 0;

            // try each server in the cluster until successful
            for (int iConnectionString = 0; iConnectionString < pConnectionStringList.Count; iConnectionString++)
            {
                string connectionString = pConnectionStringList[iConnectionString];

                // MySql database connection.
                MySqlConnection conn = new MySqlConnection(connectionString);
                // MySqlCommand for database commands.
                MySqlCommand cmd = null;
                // DataReader to read database record.
                MySqlDataReader reader = null;
                // Datetime to check if current session item is expired.
                DateTime expires;
                // String to hold serialized SessionStateItemCollection.
                string serializedItems = "";
                // True if a record is found in the database.
                bool foundRecord = false;
                // True if the returned session item is expired and needs to be deleted.
                bool deleteData = false;
                // Timeout value from the data store.
                int timeout = 0;

                try
                {
                    conn.Open();

                    // lockRecord is true when called from GetItemExclusive and
                    // false when called from GetItem.
                    // Obtain a lock if possible. Ignore the record if it is expired.
                    if (lockRecord)
                    {
                        cmd = new MySqlCommand(
                            "UPDATE sessions SET" +
                            " Locked = ?Locked1, LockDate = ?LockDate " +
                            " WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND Locked = ?Locked2 AND Expires > ?Expires", conn);
                        cmd.Parameters.Add("?Locked1", MySqlDbType.Int32).Value = 1;
                        cmd.Parameters.Add("?LockDate", MySqlDbType.DateTime).Value
                            = DateTime.Now;
                        cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                        cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                            255).Value = ApplicationName;
                        cmd.Parameters.Add("?Locked2", MySqlDbType.Int32).Value = 0;
                        cmd.Parameters.Add
                            ("?Expires", MySqlDbType.DateTime).Value = DateTime.Now;

                        if (cmd.ExecuteNonQuery() == 0)
                            // No record was updated because the record was locked or not found.
                            locked = true;
                        else
                            // The record was updated.

                            locked = false;
                    }

                    // Retrieve the current session item information.
                    cmd = new MySqlCommand(
                        "SELECT Expires, SessionItems, LockId, LockDate, Flags, Timeout " +
                        "  FROM sessions " +
                        "  WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                    cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                    cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                        255).Value = ApplicationName;

                    // Retrieve session item data from the data source.
                    reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                    while (reader.Read())
                    {
                        expires = reader.GetDateTime(0);

                        if (expires < DateTime.Now)
                        {
                            // The record was expired. Mark it as not locked.
                            locked = false;
                            // The session was expired. Mark the data for deletion.
                            deleteData = true;
                        }
                        else
                            foundRecord = true;

                        serializedItems = reader.GetString(1);
                        lockId = reader.GetInt32(2);
                        lockAge = DateTime.Now.Subtract(reader.GetDateTime(3));
                        actionFlags = (SessionStateActions)reader.GetInt32(4);
                        timeout = reader.GetInt32(5);
                    }
                    reader.Close();


                    // If the returned session item is expired,
                    // delete the record from the data source.
                    if (deleteData)
                    {
                        cmd = new MySqlCommand("DELETE FROM sessions " +
                            "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                        cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                        cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                            255).Value = ApplicationName;

                        cmd.ExecuteNonQuery();
                    }

                    // The record was not found. Ensure that locked is false.
                    if (!foundRecord)
                        locked = false;

                    // If the record was found and you obtained a lock, then set
                    // the lockId, clear the actionFlags,
                    // and create the SessionStateStoreItem to return.
                    if (foundRecord && !locked)
                    {
                        lockId = (int)lockId + 1;

                        cmd = new MySqlCommand("UPDATE sessions SET" +
                            " LockId = ?LockId, Flags = 0 " +
                            " WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                        cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;
                        cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                        cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar, 255).Value = ApplicationName;

                        cmd.ExecuteNonQuery();

                        // If the actionFlags parameter is not InitializeItem,
                        // deserialize the stored SessionStateItemCollection.
                        if (actionFlags == SessionStateActions.InitializeItem)
                            item = CreateNewStoreData(context, Convert.ToInt32(pConfig.Timeout.TotalMinutes));
                        else
                            item = Deserialize(context, serializedItems, timeout);
                    }
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                        WriteToEventLog(e, "GetSessionStoreItem");
                    if (iConnectionString < pConnectionStringList.Count - 1)
                        continue;
                    if (WriteExceptionsToEventLog)
                    {
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                        throw e;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    conn.Close();
                }
                break; // No Exception was thrown so must have been successful on this server of the cluster so done.
            }
            return item;
        }

        /// <summary>
        /// Serialize is called by the SetAndReleaseItemExclusive method to
        /// convert the SessionStateItemCollection into a Base64 string to    
        /// be stored in an Access Memo field.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string Serialize(SessionStateItemCollection items)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            if (items != null)
                items.Serialize(writer);

            writer.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// DeSerialize is called by the GetSessionStoreItem method to
        /// convert the Base64 string stored in the Access Memo field to a
        /// SessionStateItemCollection.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serializedItems"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private SessionStateStoreData Deserialize(
                HttpContext context,
                string serializedItems, int timeout)
        {
            MemoryStream ms =
                new MemoryStream(Convert.FromBase64String(serializedItems));

            SessionStateItemCollection sessionItems =
                new SessionStateItemCollection();

            if (ms.Length > 0)
            {
                BinaryReader reader = new BinaryReader(ms);
                sessionItems = SessionStateItemCollection.Deserialize(reader);
            }

            return new SessionStateStoreData(sessionItems,
                SessionStateUtility.GetSessionStaticObjects(context),
                timeout);
        }

        /// <summary>
        /// SessionStateProviderBase.ReleaseItemExclusive
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="lockId"></param>
        public override void ReleaseItemExclusive(
                HttpContext context,
                string id,
                object lockId)
        {
            // try each server in the cluster until successful
            for (int iConnectionString = 0; iConnectionString < pConnectionStringList.Count; iConnectionString++)
            {
                string connectionString = pConnectionStringList[iConnectionString];

                MySqlConnection conn = new MySqlConnection(connectionString);
                MySqlCommand cmd =
                    new MySqlCommand("UPDATE sessions SET Locked = 0, Expires = ?Expires " +
                    "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND LockId = ?LockId", conn);
                cmd.Parameters.Add("?Expires", MySqlDbType.DateTime).Value =
                    DateTime.Now.AddMinutes(pConfig.Timeout.Minutes);
                cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                    255).Value = ApplicationName;
                cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                        WriteToEventLog(e, "ReleaseItemExclusive");
                    if (iConnectionString < pConnectionStringList.Count - 1)
                        continue;
                    if (WriteExceptionsToEventLog)
                    {
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                        throw e;
                }
                finally
                {
                    conn.Close();
                }
                break; // No Exception was thrown so must have been successful on this server of the cluster so done.
            }
        }

        /// <summary>
        /// SessionStateProviderBase.RemoveItem
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="lockId"></param>
        /// <param name="item"></param>
        public override void RemoveItem(
                HttpContext context,
                string id,
                object lockId,
                SessionStateStoreData item)
        {
            // try each server in the cluster until successful
            for (int iConnectionString = 0; iConnectionString < pConnectionStringList.Count; iConnectionString++)
            {
                string connectionString = pConnectionStringList[iConnectionString];

                MySqlConnection conn = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand("DELETE FROM sessions " +
                    "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName AND LockId = ?LockId", conn);
                cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                    255).Value = ApplicationName;
                cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = lockId;

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                        WriteToEventLog(e, "RemoveItem");
                    if (iConnectionString < pConnectionStringList.Count - 1)
                        continue;
                    if (WriteExceptionsToEventLog)
                    {
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                        throw e;
                }
                finally
                {
                    conn.Close();
                }
                break; // No Exception was thrown so must have been successful on this server of the cluster so done.
            }
        }

        /// <summary>
        /// SessionStateProviderBase.CreateUninitializedItem
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="timeout"></param>
        public override void CreateUninitializedItem(
                HttpContext context,
                string id,
                int timeout)
        {
            // try each server in the cluster until successful
            for (int iConnectionString = 0; iConnectionString < pConnectionStringList.Count; iConnectionString++)
            {
                string connectionString = pConnectionStringList[iConnectionString];

                MySqlConnection conn = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand("INSERT INTO sessions " +
                    " (SessionId, ApplicationName, Created, Expires, " +
                    "  LockDate, LockId, Timeout, Locked, SessionItems, Flags) " +
                    " Values(?SessionId, ?ApplicationName, ?Created, ?Expires, " +
                    "  ?LockDate, ?LockId, ?Timeout, ?Locked, ?SessionItems, ?Flags)", conn);
                cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                    255).Value = ApplicationName;
                cmd.Parameters.Add("?Created", MySqlDbType.DateTime).Value
                    = DateTime.Now;
                cmd.Parameters.Add("?Expires", MySqlDbType.DateTime).Value
                    = DateTime.Now.AddMinutes((Double)timeout);
                cmd.Parameters.Add("?LockDate", MySqlDbType.DateTime).Value
                    = DateTime.Now;
                cmd.Parameters.Add("?LockId", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("?Timeout", MySqlDbType.Int32).Value = timeout;
                cmd.Parameters.Add("?Locked", MySqlDbType.Int32).Value = 0;
                cmd.Parameters.Add("?SessionItems", MySqlDbType.VarChar, 0).Value = "";
                cmd.Parameters.Add("?Flags", MySqlDbType.Int32).Value = 1;

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                        WriteToEventLog(e, "CreateUninitializedItem");
                    if (iConnectionString < pConnectionStringList.Count - 1)
                        continue;
                    if (WriteExceptionsToEventLog)
                    {
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                        throw e;
                }
                finally
                {
                    conn.Close();
                }
                break; // No Exception was thrown so must have been successful on this server of the cluster so done.
            }
        }

        /// <summary>
        /// SessionStateProviderBase.CreateNewStoreData
        /// </summary>
        /// <param name="context"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override SessionStateStoreData CreateNewStoreData(
                HttpContext context,
                int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(),
                SessionStateUtility.GetSessionStaticObjects(context),
                timeout);
        }

        /// <summary>
        /// SessionStateProviderBase.ResetItemTimeout
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        public override void ResetItemTimeout(
                HttpContext context,
                string id)
        {
            // try each server in the cluster until successful
            for (int iConnectionString = 0; iConnectionString < pConnectionStringList.Count; iConnectionString++)
            {
                string connectionString = pConnectionStringList[iConnectionString];

                MySqlConnection conn = new MySqlConnection(connectionString);
                MySqlCommand cmd =
                    new MySqlCommand("UPDATE sessions SET Expires = ?Expires " +
                    "WHERE SessionId = ?SessionId AND ApplicationName = ?ApplicationName", conn);
                cmd.Parameters.Add("?Expires", MySqlDbType.DateTime).Value
                    = DateTime.Now.AddMinutes(Convert.ToInt32(pConfig.Timeout.TotalMinutes));
                cmd.Parameters.Add("?SessionId", MySqlDbType.VarChar, 80).Value = id;
                cmd.Parameters.Add("?ApplicationName", MySqlDbType.VarChar,
                    255).Value = ApplicationName;

                try
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                        WriteToEventLog(e, "ResetItemTimeout");
                    if (iConnectionString < pConnectionStringList.Count - 1)
                        continue;
                    if (WriteExceptionsToEventLog)
                    {
                        //WriteToEventLog(e, "ResetItemTimeout");
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                        throw e;
                }
                finally
                {
                    conn.Close();
                }
                break; // No Exception was thrown so must have been successful on this server of the cluster so done.
            }
        }

        /// <summary>
        /// SessionStateProviderBase.InitializeRequest
        /// </summary>
        /// <param name="context"></param>
        public override void InitializeRequest(HttpContext context)
        {
        }

        /// <summary>
        /// SessionStateProviderBase.EndRequest
        /// </summary>
        /// <param name="context"></param>
        public override void EndRequest(HttpContext context)
        {
        }

        /// <summary>
        /// WriteToEventLog
        /// This is a helper function that writes exception detail to the
        /// event log. Exceptions are written to the event log as a security
        /// measure to ensure private database details are not returned to
        /// browser. If a method does not return a status or Boolean
        /// indicating the action succeeded or failed, the caller also
        /// throws a generic exception.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="action"></param>
        private void WriteToEventLog(Exception e, string action)
        {
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message =
                "An exception occurred communicating with the data source.\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message);
        }
        #endregion methods
    }
}