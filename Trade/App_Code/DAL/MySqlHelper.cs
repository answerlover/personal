using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

public class MySqlHelper
{
    public static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

    static MySqlHelper()
    {
        string regPattern = @"PASSWORD=(.*?);";
        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(ConnectionString, regPattern);
        if (match.Success)
        {
            string pwd = AuthSecurity.Decrypt(match.Result("$1"));
            ConnectionString = System.Text.RegularExpressions.Regex.Replace(ConnectionString, regPattern, string.Format("PASSWORD={0};", pwd));
        }
    }

    public static MySqlConnection CreateConnection(string connectionString)
    {
        return new MySqlConnection(connectionString);
    }

    public static int ExecuteNonQuery(string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlConnection conn = new MySqlConnection(ConnectionString);
        MySqlCommand cmd = new MySqlCommand();
        PrepareCommand(cmd, conn, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Dispose();
        conn.Close();
        conn.Dispose();

        return val;
    }

    public static int ExecuteNonQuery(MySqlConnection conn, string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlCommand cmd = new MySqlCommand();
        PrepareCommand(cmd, conn, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Dispose();

        return val;
    }

    public static MySqlDataReader ExecuteReader(string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlConnection conn = new MySqlConnection(ConnectionString);
        MySqlCommand cmd = new MySqlCommand();
        try
        {
            PrepareCommand(cmd, conn, cmdText, cmdParms);
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    public static MySqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlConnection conn = new MySqlConnection(ConnectionString);
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandType = cmdType;
        try
        {
            PrepareCommand(cmd, conn, cmdText, cmdParms);
            MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }
    /// <summary>
    /// 执行SQL命令返回第一行第一列的值
    /// </summary>      
    /// <param name="commandType">命令类型(stored procedure or text)</param>
    /// <param name="commandText">执行的SQL命令</param>
    /// <param name="commandParameters">绑定到命令的参数</param>
    /// <returns>需使用Convert.To{Type}转换成相应类型的对象</returns>
    public static object ExecuteScalar(string cmdText, params MySqlParameter[] commandParameters)
    {
        using (MySqlConnection conn = new MySqlConnection(ConnectionString))
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, conn, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
    }
    public static DataTable ExecuteTable(string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlConnection conn = new MySqlConnection(ConnectionString);
        MySqlCommand cmd = new MySqlCommand();
        try
        {
            PrepareCommand(cmd, conn, cmdText, cmdParms);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataTable dTable = new DataTable();
            ada.Fill(dTable);
            cmd.Parameters.Clear();
            conn.Close();
            return dTable;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }
    public static DataTable ExecuteTable(CommandType cmdType, string cmdText, params MySqlParameter[] cmdParms)
    {
        MySqlConnection conn = new MySqlConnection(ConnectionString);
        MySqlCommand cmd = new MySqlCommand();
        cmd.CommandType = cmdType;
        try
        {
            PrepareCommand(cmd, conn, cmdText, cmdParms);
            MySqlDataAdapter ada = new MySqlDataAdapter(cmd);
            DataTable dTable = new DataTable();
            ada.Fill(dTable);
            cmd.Parameters.Clear();
            conn.Close();
            return dTable;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }
    private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, string cmdText, MySqlParameter[] cmdParms)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;

        if (cmdParms != null)
        {
            foreach (MySqlParameter parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }
    public static MySqlParameter CreateParameter(string name, MySqlDbType type, object value)
    {
        MySqlParameter parm = new MySqlParameter(name, type);
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            parm.Value = DBNull.Value;
        }
        else
        {
            parm.Value = value;
        }
        return parm;
    }
    public static MySqlParameter CreateParameter(string name, MySqlDbType type, object value, ParameterDirection pd)
    {
        MySqlParameter parm = new MySqlParameter(name, type);
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            parm.Value = DBNull.Value;
        }
        else
        {
            parm.Value = value;
        }
        parm.Direction = ParameterDirection.Output;

        return parm;
    }

    /// <summary>
    /// mysql text类型不支持output参数
    /// </summary>
    /// <param name="strSql"></param>
    /// <param name="pager"></param>
    /// <returns></returns>
    public static string WrapPagingSqlString(string strSql, PagerInfo pager)
    {
        string sql = string.Format(@"
{0} LIMIT @startRowIndex,@pageSize;
", strSql);

        return sql.Replace("@startRowIndex", (pager.startRowIndex - 1).ToString()).Replace("@pageSize", pager.pageSize.ToString());
    }

    public static string WrapCountSqlString(string strSql)
    {
        string sql = strSql;
        sql = Regex.Replace(sql, @"select[\s\S]*from", "select COUNT(*) from", RegexOptions.IgnoreCase);
        sql = Regex.Replace(sql, @"order[\s\S]*by[\s\S]*", "", RegexOptions.IgnoreCase);
        return sql;
    }


    /// <summary>
    /// 参数化查询
    /// </summary>
    /// <param name="strSql"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    public static string GetStringSql(string strSql, IEnumerable<System.Data.Common.DbParameter> parms)
    {
        foreach (System.Data.Common.DbParameter parm in parms)
        {
            string replaceString = parm.Value.ToString();
            if (parm.DbType == DbType.String || parm.DbType == DbType.DateTime)
            {
                replaceString = Filter(replaceString);
                strSql = strSql.Replace(parm.ParameterName, string.Format("'{0}'", replaceString));
            }
            else
            {
                strSql = strSql.Replace(parm.ParameterName, replaceString);
            }
        }
        return strSql;
    }

    /// <summary>
    ///SQL注入过滤
    /// </summary>
    /// <param name="InText">要过滤的字符串</param>
    /// <returns>如果参数存在不安全字符，则返回true</returns>
    public static string Filter(string InText)
    {
        string word = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join";
        word += "|'|\"|%|\\-|;|(|)| ";

        Regex reg = new Regex(word, RegexOptions.IgnoreCase);
        return reg.Replace(InText, "");

    }
}
