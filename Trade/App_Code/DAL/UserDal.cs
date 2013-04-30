using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

public class UserDal
{

    MySqlConnection conn;
    public UserDal()
    {

    }
    public UserDal(MySqlConnection conn)
    {
        this.conn = conn;
    }


    #region 成员

    public bool IsExists(UserInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select ID
,CompanyCode
,UserName
,PassWord
 from m_User 
where UserName=@UserName
AND ID != @ID");
        MySqlParameter[] parameters = CreateParameters(info);
        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value;
    }

    public bool HasChildUser(int ID)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select 1
 from m_User 
where PID = @ID LIMIT 1");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,ID)};
        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value;
    }

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add(UserInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
insert into m_User(
CompanyCode
,UserName
,PassWord
) values (
@CompanyCode
,@UserName
,@PassWord
)");
        strSql.Append(";select @@IDENTITY;");

        MySqlParameter[] parameters = CreateParameters(info);

        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value ? Convert.ToInt32(ret) : 0;
    }

    /// <summary>
    /// 更新一条数据
    /// </summary>
    public int Update(UserInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update m_User set ");
        strSql.Append("UserName=@UserName,");
        strSql.Append("PassWord=@PassWord,");
        strSql.Append("CompanyCode=@CompanyCode");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = CreateParameters(info);

        return MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    public void Delete(int ID)
    {

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"delete from m_User where ID=@ID ;
UPDATE Customers
SET RefSeller = NULL
WHERE RefSeller = @ID;
");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,ID)};

        if (this.conn != null)
        {
            MySqlHelper.ExecuteNonQuery(conn, strSql.ToString(), parameters);
        }
        else
        {
            MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
        }
    }


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public UserInfo GetInfo(int ID)
    {
        UserInfo info = null;

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select a.ID
,a.CompanyCode
,a.UserName
,b.NodePath
,b.PUserID
from m_User a
LEFT JOIN User_Node b
ON a.ID = b.UserID
");
        strSql.Append(" where a.ID=@ID ");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,ID)
                                          };

        using (IDataReader dataReader = MySqlHelper.ExecuteReader(strSql.ToString(), parameters))
        {
            if (dataReader.Read())
            {
                info = ReaderBind(dataReader);
            }
        }

        if (info != null)
        {
            RoleInfo role;

            info.AuthList = GetUserFunctions(info.ID, out role);
            info.Role = role;
        }

        return info;
    }

    /// <summary>
    /// 登陆时调用
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="PassWord"></param>
    /// <returns></returns>
    public UserInfo GetInfo(string UserName, string PassWord)
    {
        UserInfo info = null;

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select a.ID
,a.CompanyCode
,a.UserName
,a.PassWord
,b.CompanyName
,c.NodePath
,c.PUserID
 from m_User a
LEFT JOIN Company b
ON a.CompanyCode = b.CompanyCode
LEFT JOIN User_Node c
ON a.ID = c.UserID

");
        strSql.Append(" where a.UserName=@UserName ");
        strSql.Append(" and a.PassWord=@PassWord ");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@UserName", MySqlDbType.String,UserName),
					MySqlHelper.CreateParameter("@PassWord", MySqlDbType.String,PassWord)
                                          };

        using (IDataReader dataReader = MySqlHelper.ExecuteReader(strSql.ToString(), parameters))
        {
            if (dataReader.Read())
            {
                info = ReaderBind(dataReader);

            }
        }

        if (info != null)
        {
            RoleInfo role;

            info.AuthList = GetUserFunctions(info.ID, out role);
            info.Role = role;
        }
        return info;
    }

    public List<string> GetUserFunctions(int id, out RoleInfo role)
    {
        List<string> functionlist = new List<string>();
        RoleInfo currentRole = null;

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select c.FunctionCode,a.RoleID, a.RoleName
,a.RoleDescription
  from Role a
INNER JOIN User_Role b
ON a.RoleID = b.RoleID
LEFT JOIN Role_Function c
ON a.RoleID = c.RoleID
WHERE b.UserID = @UserID ");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@UserID", MySqlDbType.Int32,id)
                                          };

        using (IDataReader dataReader = MySqlHelper.ExecuteReader(strSql.ToString(), parameters))
        {
            while (dataReader.Read())
            {
                string functionCode = dataReader["FunctionCode"].ToString();
                if (!functionlist.Contains(functionCode))
                {
                    functionlist.Add(functionCode);
                }

                if (currentRole == null)
                {
                    currentRole = new RoleInfo();
                    currentRole.RoleID = Convert.ToInt32(dataReader["RoleID"]);
                    currentRole.RoleName = (string)dataReader["RoleName"];
                    currentRole.RoleDescription = (string)dataReader["RoleDescription"];
                }
            }
        }

        role = currentRole;
        return functionlist;
    }

    public List<UserInfo> GetList(QueryFilter filter)
    {
        List<UserInfo> list = new List<UserInfo>();
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select a.ID
,a.CompanyCode
,a.UserName
,a.PID
,b.CompanyName
,c.NodePath
,c.PUserID
 from m_User a
LEFT JOIN Company b
ON a.CompanyCode = b.CompanyCode
LEFT JOIN User_Node c
ON a.ID = c.UserID
");
        strSql.Append(" where 1=1 ");

        List<MySqlParameter> parmslist = new List<MySqlParameter>();

        if (filter.CompanyCode != null)
        {
            strSql.Append(" and a.CompanyCode = @CompanyCode");
            parmslist.Add(MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, filter.CompanyCode));
        }

        if (filter.NodePath != null)
        {
            strSql.Append(" and c.NodePath like CONCAT(@NodePath ,'%') ");
            parmslist.Add(MySqlHelper.CreateParameter("@NodePath", MySqlDbType.VarChar, filter.NodePath));
        }

        // remove administator
        strSql.Append(" AND a.UserName NOT IN ('admin','SysAdmin','administrator')");
        strSql.Append(" ORDER BY c.NodePath");
        string sql = strSql.ToString();
        //if (filter.Pager != null)
        //{
        //    sql = MySqlHelper.WrapPagingSqlString(strSql.ToString(), filter.Pager);
        //}

        using (IDataReader dataReader = MySqlHelper.ExecuteReader(CommandType.Text, sql, parmslist.ToArray()))
        {
            while (dataReader.Read())
            {
                list.Add(ReaderBind(dataReader));
            }
        }

        if (filter.Pager != null)
        {
            sql = MySqlHelper.WrapCountSqlString(strSql.ToString());
            filter.Pager.recordCount = int.Parse(MySqlHelper.ExecuteScalar(sql, parmslist.ToArray()).ToString());
        }

        return list;
    }


    public DataTable GetRoleData()
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select RoleID
,RoleName
,RoleDescription
 from Role ");
        strSql.Append(" where 1=1 AND RoleName != 'Administrator' ");

        return MySqlHelper.ExecuteTable(strSql.ToString());
    }

    public DataTable GetCompanyData()
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select CompanyCode
,CompanyName
 from Company ");
        strSql.Append(" where 1=1 ");

        return MySqlHelper.ExecuteTable(strSql.ToString());
    }

    #endregion  成员方法

    public void UpdateUserRole(int UserID, int RoleID)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
INSERT INTO User_Role
(
RoleID
,UserID
)
SELECT @RoleID
,@UserID
FROM User_Role 
WHERE NOT EXISTS (
	SELECT 1 FROM User_Role WHERE UserID = @UserID
) LIMIT 1
;

UPDATE User_Role
SET RoleID = @RoleID
 WHERE UserID = @UserID
AND RoleID != @RoleID
; ");

        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@UserID", MySqlDbType.Int32,UserID),
					MySqlHelper.CreateParameter("@RoleID", MySqlDbType.Int32,RoleID)
                                          };


        MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    public int UpdateUserCompany(int UserID, string CompanyCode)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update m_User set ");
        strSql.Append("CompanyCode=@CompanyCode");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = new MySqlParameter[]{
                    MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,UserID),
                    MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, CompanyCode)
        };

        return MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    public void UpdateUserNode(int UserID, int PID)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
call UP_UpdateUserTree(@UserID,@PID);
");

        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@UserID", MySqlDbType.Int32,UserID),
                    MySqlHelper.CreateParameter("@PID", MySqlDbType.Int32,PID)
        };

        MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }

    #region  私有方法


    private static MySqlParameter[] CreateParameters(UserInfo info)
    {
        MySqlParameter[] parameters = {
                    MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,info.ID),
                    MySqlHelper.CreateParameter("@UserName", MySqlDbType.VarChar,info.UserName),
                    MySqlHelper.CreateParameter("@PassWord", MySqlDbType.VarChar, info.Password),
                    MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, info.CompanyCode)
            };
        return parameters;
    }

    /// <summary>
    /// 对象实体绑定数据
    /// </summary>
    private UserInfo ReaderBind(IDataReader dataReader)
    {
        UserInfo info = new UserInfo();
        info.ID = (Int32)dataReader["ID"];
        info.UserName = dataReader["UserName"].ToString();
        info.Password = "N/A";//dataReader["PassWord"].ToString();
        info.CompanyCode = dataReader["CompanyCode"].ToString();
        info.NodePath = dataReader["NodePath"].ToString();

        if (dataReader["PUserID"] != DBNull.Value)
        {
            info.PID = (Int32)dataReader["PUserID"];
        }


        foreach (DataRow dr in dataReader.GetSchemaTable().Rows)
        {
            if (dr["ColumnName"].ToString() == "CompanyName")
            {
                info.CompanyName = (string)dataReader["CompanyName"];
                break;
            }
        }

        return info;
    }
    #endregion  私有方法

}
