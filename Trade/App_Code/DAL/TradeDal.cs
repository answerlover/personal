using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

public class TradeDal
{
    MySqlConnection conn;
    public TradeDal()
    {

    }
    public TradeDal(MySqlConnection conn)
    {
        this.conn = conn;
    }
    #region  成员

    public bool IsExists(TradeTransactionInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(
            @"select ID from TradeTransaction
 where CompanyCode=@CompanyCode 
and DATE_ADD(SettlementDate,INTERVAL -1 DAY) < @SettlementDate
and DATE_ADD(SettlementDate,INTERVAL 1 DAY) > @SettlementDate
and ID != @ID
");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,info.ID),
					MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.String,info.CompanyCode),
					MySqlHelper.CreateParameter("@SettlementDate", MySqlDbType.DateTime,info.SettlementDate)
                                          };
        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value;
    }

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add(TradeTransactionInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("insert into TradeTransaction(");
        strSql.Append(@"
CompanyCode
,TraderCode
,TraderName
,BelongCode
,TradeCount
,TradeFee
,OrganizationFee
,ExchangeFee
,SettlementDate
,InDate
,InUser
)");
        strSql.Append(" values (");
        strSql.Append(@"
@CompanyCode
,@TraderCode
,@TraderName
,@BelongCode
,@TradeCount
,@TradeFee
,@OrganizationFee
,@ExchangeFee
,@SettlementDate
,@InDate
,@InUser
)");

        //  strSql.Append(";select @@IDENTITY;");
        MySqlParameter[] parameters = this.CreateParameters(info);

        if (this.conn != null)
        {
            return Convert.ToInt32(MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters));
        }
        else
        {
            return Convert.ToInt32(MySqlHelper.ExecuteNonQuery(conn, strSql.ToString(), parameters));
        }
    }

    /// <summary>
    /// 更新一条数据
    /// </summary>
    public void Update(TradeTransactionInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"update TradeTransaction set 
ID=@ID
,TradeTransactionNumber=@TradeTransactionNumber
,UnitPrice=@UnitPrice
,Title=@Title
,Description=@Description
,Image=@Image
,Weight=@Weight
,Length=@Length
,Width=@Width
,Height=@Height
,Category=@Category
,ManuFacturer=@ManuFacturer
,ListPrice=@ListPrice
,Color=@Color
,Layout=@Layout
,Availability=@Availability
");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = CreateParameters(info);

        MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    /// <summary>
    /// 删除一条数据
    /// </summary>
    public void Delete(int ID)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("delete from TradeTransaction ");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,ID)};

        MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public TradeTransactionInfo GetInfo(int ID)
    {
        TradeTransactionInfo info = null;

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"select
ID
,CompanyCode
,TraderCode
,TraderName
,BelongCode
,TradeCount
,TradeFee
,OrganizationFee
,ExchangeFee
,SettlementDate
,InDate
,InUser
from TradeTransaction ");
        strSql.Append(" where ID=@ID ");
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

    public List<TradeTransactionInfo> GetList(QueryFilter filter)
    {
        List<TradeTransactionInfo> list = new List<TradeTransactionInfo>();
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"select 
a.ID
,a.CompanyCode
,a.TraderCode
,a.TraderName
,a.BelongCode
,a.TradeCount
,a.TradeFee
,a.OrganizationFee
,a.ExchangeFee
,a.SettlementDate
,a.InDate
,a.InUser
,c.ID AS UserID
,c.UserName
FROM TradeTransaction a
LEFT JOIN Customers b
ON a.TraderCode = b.CustomerNameCode
AND a.CompanyCode = b.CompanyCode
LEFT JOIN m_User c
ON b.RefSeller = c.ID
LEFT JOIN User_Node n
ON a.ID = n.UserID
");
        strSql.Append("  ");
        strSql.Append(" where 1=1 ");

        List<MySqlParameter> parmslist = new List<MySqlParameter>();

        if (filter.CompanyCode != null)
        {
            strSql.Append(" and a.CompanyCode = @CompanyCode");
            parmslist.Add(MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, filter.CompanyCode));
        }
        if (filter.NodePath != null)
        {
            strSql.Append(" and (n.NodePath like CONCAT(@NodePath ,'%') OR b.RefSeller IS NULL)");
            parmslist.Add(MySqlHelper.CreateParameter("@NodePath", MySqlDbType.VarChar, filter.NodePath));
        }

        if (filter.SettlementDate_Begin != null)
        {
            strSql.Append(" and a.SettlementDate >= @SettlementDate_Begin");
            parmslist.Add(MySqlHelper.CreateParameter("@SettlementDate_Begin", MySqlDbType.DateTime, filter.SettlementDate_Begin));
        }
        if (filter.SettlementDate_End != null)
        {
            strSql.Append(" and DATE_ADD(a.SettlementDate,INTERVAL -1 DAY) <  @SettlementDate_End");
            parmslist.Add(MySqlHelper.CreateParameter("@SettlementDate_End", MySqlDbType.DateTime, filter.SettlementDate_End));
        }


        if (filter.SellerID > 0)
        {
            strSql.Append(" and b.RefSeller =@RefSeller");
            parmslist.Add(MySqlHelper.CreateParameter("@RefSeller", MySqlDbType.Int32, filter.SellerID));
        }
        else if (filter.SellerID == 0)
        {
            strSql.Append(" and b.RefSeller IS NULL");
        }


        if (filter.TraderCode_Begin != null)
        {
            strSql.Append(" and a.TraderCode >= @TraderCode_Begin");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderCode_Begin", MySqlDbType.VarChar, filter.TraderCode_Begin));
        }
        if (filter.TraderCode_End != null)
        {
            strSql.Append(" and a.TraderCode <=  @TraderCode_End");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderCode_End", MySqlDbType.VarChar, filter.TraderCode_End));
        }

        if (filter.TraderName != null)
        {
            strSql.Append(" and a.TraderName  like CONCAT('%',@TraderName,'%')");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderName", MySqlDbType.VarChar, filter.TraderName));
        }

        strSql.Append(" order by b.CustomerNameCode");



        //string sql = MySqlHelper.GetStringSql(strSql.ToString(), parmslist);

        //MySqlParameter cntParam = new MySqlParameter("recordCount", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
        //MySqlParameter[] parms = new MySqlParameter[]{
        //         MySqlHelper.CreateParameter("@startRowIndex",MySqlDbType.VarChar,filter.Pager.startRowIndex, ParameterDirection.Input),
        //         MySqlHelper.CreateParameter("@pageSize",MySqlDbType.Int32,filter.Pager.pageSize, ParameterDirection.Input),
        //         cntParam,              
        //         MySqlHelper.CreateParameter("@strSql",MySqlDbType.VarChar,sql, ParameterDirection.Input)};


        string sql = MySqlHelper.WrapPagingSqlString(strSql.ToString(), filter.Pager);

        // using (IDataReader dataReader = MySqlHelper.ExecuteReader(CommandType.StoredProcedure, "DataPageing", parms))
        using (IDataReader dataReader = MySqlHelper.ExecuteReader(CommandType.Text, sql, parmslist.ToArray()))
        {
            while (dataReader.Read())
            {
                TradeTransactionInfo info = ReaderBind(dataReader);
                info.RefUser = new UserInfo();
                if (dataReader["UserID"] != null && dataReader["UserID"] != DBNull.Value)
                {
                    info.RefUser.ID = (Int32)dataReader["UserID"];
                }
                info.RefUser.UserName = dataReader["UserName"].ToString();

                list.Add(info);
            }
        }

        //filter.Pager.recordCount = int.Parse(cntParam.Value.ToString());
        sql = MySqlHelper.WrapCountSqlString(strSql.ToString());
        filter.Pager.recordCount = int.Parse(MySqlHelper.ExecuteScalar(sql, parmslist.ToArray()).ToString());

        return list;
    }


    public DataTable GetSummaryData(QueryFilter filter)
    {
        string sqlHead = @"
SELECT
c.ID,
c.UserName,
SUM(a.TradeCount) TradeCount,
SUM(a.TradeFee) TradeFee,
SUM(a.OrganizationFee) OrganizationFee,
SUM(a.ExchangeFee) ExchangeFee
FROM
TradeTransaction a
LEFT JOIN Customers b
ON a.TraderCode = b.CustomerNameCode
AND a.CompanyCode = b.CompanyCode
LEFT JOIN m_User c
ON b.RefSeller = c.ID
WHERE 1=1
";
        StringBuilder strSql = new StringBuilder();


        List<MySqlParameter> parmslist = new List<MySqlParameter>();

        if (filter.CompanyCode != null)
        {
            strSql.Append(" and a.CompanyCode = @CompanyCode");
            parmslist.Add(MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, filter.CompanyCode));
        }

        if (filter.SettlementDate_Begin != null)
        {
            strSql.Append(" and a.SettlementDate >= @SettlementDate_Begin");
            parmslist.Add(MySqlHelper.CreateParameter("@SettlementDate_Begin", MySqlDbType.DateTime, filter.SettlementDate_Begin));
        }
        if (filter.SettlementDate_End != null)
        {
            strSql.Append(" and DATE_ADD(a.SettlementDate,INTERVAL -1 DAY) <  @SettlementDate_End");
            parmslist.Add(MySqlHelper.CreateParameter("@SettlementDate_End", MySqlDbType.DateTime, filter.SettlementDate_End));
        }


        if (filter.SellerID > 0)
        {
            strSql.Append(" and b.RefSeller =@RefSeller");
            parmslist.Add(MySqlHelper.CreateParameter("@RefSeller", MySqlDbType.Int32, filter.SellerID));
        }
        else if (filter.SellerID == 0)
        {
            strSql.Append(" and b.RefSeller IS NULL");
        }


        if (filter.TraderCode_Begin != null)
        {
            strSql.Append(" and a.TraderCode >= @TraderCode_Begin");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderCode_Begin", MySqlDbType.VarChar, filter.TraderCode_Begin));
        }
        if (filter.TraderCode_End != null)
        {
            strSql.Append(" and a.TraderCode <=  @TraderCode_End");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderCode_End", MySqlDbType.VarChar, filter.TraderCode_End));
        }

        if (filter.TraderName != null)
        {
            strSql.Append(" and a.TraderName  like CONCAT('%',@TraderName,'%')");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderName", MySqlDbType.VarChar, filter.TraderName));
        }

        string sqlfoot = @" GROUP BY c.ID,
c.UserName
order by c.ID ";



        // summary 暂时不支持分页; 如果需要可以考虑加入到sp中
        string sql = sqlHead + strSql.ToString() + sqlfoot;
        DataTable dt = MySqlHelper.ExecuteTable(CommandType.Text, sql, parmslist.ToArray());

        sqlHead = @" 
SELECT
0 AS ID,
'合计' as UserName,
SUM(TradeCount) TradeCount,
SUM(TradeFee) TradeFee,
SUM(OrganizationFee) OrganizationFee,
SUM(ExchangeFee) ExchangeFee
FROM
TradeTransaction a
LEFT JOIN Customers b
ON a.TraderCode = b.CustomerNameCode
AND a.CompanyCode = b.CompanyCode
LEFT JOIN m_User c
ON b.RefSeller = c.ID
WHERE 1=1
";
        sqlfoot = "";

        sql = sqlHead + strSql.ToString() + sqlfoot;
        DataTable dt2 = MySqlHelper.ExecuteTable(CommandType.Text, sql, parmslist.ToArray());
        dt.Rows.Add(dt2.Rows[0].ItemArray);

        //sql = MySqlHelper.WrapCountSqlString();
        //   filter.Pager.recordCount = int.Parse(MySqlHelper.ExecuteScalar(sql, parmslist.ToArray()).ToString());

        return dt;
    }


    #endregion  成员方法


    #region  私有方法


    private MySqlParameter[] CreateParameters(TradeTransactionInfo info)
    {
        MySqlParameter[] parameters = {
                    MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,info.ID),
                    MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar,info.CompanyCode),
                    MySqlHelper.CreateParameter("@TraderCode", MySqlDbType.VarChar,info.TraderCode),
                    MySqlHelper.CreateParameter("@TraderName", MySqlDbType.String,info.TraderName),
                    MySqlHelper.CreateParameter("@BelongCode", MySqlDbType.VarChar,info.BelongCode),
                    MySqlHelper.CreateParameter("@TradeCount", MySqlDbType.Int32, info.TradeCount),
                    MySqlHelper.CreateParameter("@TradeFee", MySqlDbType.Decimal, info.TradeFee),
                    MySqlHelper.CreateParameter("@OrganizationFee", MySqlDbType.Decimal,info.OrganizationFee),
                    MySqlHelper.CreateParameter("@ExchangeFee", MySqlDbType.Decimal,info.ExchangeFee),
                    MySqlHelper.CreateParameter("@SettlementDate", MySqlDbType.DateTime, info.SettlementDate),
                    MySqlHelper.CreateParameter("@InDate", MySqlDbType.DateTime,info.InDate),
                    MySqlHelper.CreateParameter("@InUser", MySqlDbType.VarChar,info.InUser)
            };
        return parameters;
    }

    /// <summary>
    /// 对象实体绑定数据
    /// </summary>
    private TradeTransactionInfo ReaderBind(IDataReader dataReader)
    {
        TradeTransactionInfo info = new TradeTransactionInfo();
        object temp;
        info.ID = (Int32)dataReader["ID"];
        info.CompanyCode = dataReader["CompanyCode"].ToString();
        info.TraderCode = dataReader["TraderCode"].ToString();
        info.TraderName = dataReader["TraderName"].ToString();
        info.BelongCode = dataReader["BelongCode"].ToString();
        temp = dataReader["TradeCount"];
        if (temp != null && temp != DBNull.Value)
        {
            info.TradeCount = (int)temp;
        }

        temp = dataReader["TradeFee"];
        if (temp != null && temp != DBNull.Value)
        {
            info.TradeFee = (decimal)temp;
        }
        temp = dataReader["OrganizationFee"];
        if (temp != null && temp != DBNull.Value)
        {
            info.OrganizationFee = (decimal)temp;
        }
        temp = dataReader["ExchangeFee"];
        if (temp != null && temp != DBNull.Value)
        {
            info.ExchangeFee = (decimal)temp;
        }

        temp = dataReader["SettlementDate"];
        if (temp != null && temp != DBNull.Value)
        {
            info.SettlementDate = (DateTime)temp;
        }
        temp = dataReader["InDate"];
        if (temp != null && temp != DBNull.Value)
        {
            info.InDate = (DateTime)temp;
        }

        info.InUser = dataReader["InUser"].ToString();


        return info;
    }
    #endregion  私有方法

}
