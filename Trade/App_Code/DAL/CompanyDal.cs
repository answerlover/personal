using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

public class CompanyDal
{
    #region 成员

    public bool IsExists(CompanyInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select CompanyCode
,CompanyName
 from Company 
where( CompanyCode=@CompanyCode
 or CompanyName=@CompanyName
)
and ID!=@ID
");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.String,info.ID),
					MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.String,info.CompanyCode),
					MySqlHelper.CreateParameter("@CompanyName", MySqlDbType.String,info.CompanyName)
                                          };
        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value;
    }

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add(CompanyInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
insert into Company(
CompanyCode
,CompanyName
) values (
@CompanyCode
,@CompanyName
)");
        strSql.Append(";select @@IDENTITY;");

        MySqlParameter[] parameters = CreateParameters(info);

        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value ? Convert.ToInt32(ret) : 0;
    }

    /// <summary>
    /// 更新一条数据
    /// </summary>
    public int Update(CompanyInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update Company set ");
        strSql.Append("CompanyCode=@CompanyCode,");
        strSql.Append("CompanyName=@CompanyName");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = CreateParameters(info);

        return MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public CompanyInfo GetInfo(int ID)
    {
        CompanyInfo info = null;

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select ID
,CompanyCode
,CompanyName
 from Company 
where ID=@ID ");
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

        return info;
    }




    public DataTable GetData(QueryFilter filter)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
select ID
,CompanyCode
,CompanyName
 from Company ");
        strSql.Append(" where 1=1 ");

        List<MySqlParameter> parmslist = new List<MySqlParameter>();

        if (filter.CompanyCode != null)
        {
            strSql.Append(" and (CompanyCode like CONCAT( @CompanyCode ,'%') )");
            parmslist.Add(MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, filter.CompanyCode));
        }

        strSql.Append(" order by CompanyCode ");



        string sql = MySqlHelper.WrapPagingSqlString(strSql.ToString(), filter.Pager);

        DataTable dt = MySqlHelper.ExecuteTable(strSql.ToString(), parmslist.ToArray());

        sql = MySqlHelper.WrapCountSqlString(strSql.ToString());
        filter.Pager.recordCount = int.Parse(MySqlHelper.ExecuteScalar(sql, parmslist.ToArray()).ToString());

        return dt;
    }



    #endregion  成员方法
    



    #region  私有方法


    private static MySqlParameter[] CreateParameters(CompanyInfo info)
    {
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.String,info.ID),
					MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.String,info.CompanyCode),
					MySqlHelper.CreateParameter("@CompanyName", MySqlDbType.String,info.CompanyName)
                                          };
        return parameters;
    }

    /// <summary>
    /// 对象实体绑定数据
    /// </summary>
    private CompanyInfo ReaderBind(IDataReader dataReader)
    {
        CompanyInfo info = new CompanyInfo();
        info.ID = (Int32)dataReader["ID"];
        info.CompanyCode = dataReader["CompanyCode"].ToString();
        info.CompanyName = dataReader["CompanyName"].ToString();

        return info;
    }
    #endregion  私有方法

}
