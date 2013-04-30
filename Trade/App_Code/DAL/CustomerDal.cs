using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

public class CustomerDal
{
    #region 成员


    public bool IsExists(CustomerInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(
            @"select ID from Customers
where (CustomerNameCode=@CustomerNameCode OR CellPhoneNumber=@CellPhoneNumber AND CustomerType = @CustomerType)
AND ID != @ID
");
        strSql.Append(@" order by ID ");

        List<MySqlParameter> parmslist = new List<MySqlParameter>();
        parmslist.Add(MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32, info.ID));
        //parmslist.Add(MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, info.CompanyCode));
        parmslist.Add(MySqlHelper.CreateParameter("@CustomerNameCode", MySqlDbType.VarChar, info.CustomerNameCode));
        parmslist.Add(MySqlHelper.CreateParameter("@CellPhoneNumber", MySqlDbType.VarChar, info.CellPhoneNumber));
        parmslist.Add(MySqlHelper.CreateParameter("@CustomerType", MySqlDbType.VarChar, info.CustomerType));

        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parmslist.ToArray());
        return ret != null && ret != DBNull.Value;
    }

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add(CustomerInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"insert into Customers(
    CompanyCode
    ,CustomerNameCode
    ,CustomerName
    ,CustomerType
    ,State
    ,QQ
    ,Email
    ,CellPhoneNumber
    ,ContactPhoneNumber
    ,Address
    ,IDCardNumber
    ,RefSeller
    ,InDate
    ,InUser
)values (
    @CompanyCode
    ,@CustomerNameCode
    ,@CustomerName
    ,@CustomerType
    ,@State
    ,@QQ
    ,@Email
    ,@CellPhoneNumber
    ,@ContactPhoneNumber
    ,@Address
    ,@IDCardNumber
    ,@RefSeller
    ,@InDate
    ,@InUser
)
;
SELECT last_insert_id() AS id;
");
        MySqlParameter[] parameters = CreateParameters(info);

        return MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }

    /// <summary>
    /// 更新一条数据
    /// </summary>
    public int Update(CustomerInfo info)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
INSERT INTO Customers_Memo
(
CustomerID
,Memo
,InDate
,InUser
)
SELECT @ID, concat(
    IFNULL((
        SELECT b.UserName FROM m_User b
        WHERE a.RefSeller = b.ID
        LIMIT 1
    ),'公海') 
,' => '
,    IFNULL((SELECT UserName from m_User where ID = @RefSeller LIMIT 1),'公海') )
,@InDate
,@InUser
FROM Customers a
WHERE a.ID = @ID
AND IFNULL(@RefSeller,0) != IFNULL(a.RefSeller,0)
;
");
        strSql.Append(@"
update Customers set 
    CompanyCode=@CompanyCode
    ,CustomerNameCode=@CustomerNameCode
    ,CustomerName=@CustomerName
    ,CustomerType=@CustomerType
    ,State=@State
    ,QQ=@QQ
    ,Email=@Email
    ,CellPhoneNumber=@CellPhoneNumber
    ,ContactPhoneNumber=@ContactPhoneNumber
    ,Address=@Address
    ,IDCardNumber=@IDCardNumber
    ,RefSeller=@RefSeller
    ,InDate=@InDate
    ,InUser=@InUser
");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = CreateParameters(info);

        return MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    /// <summary>
    /// 更新客户的状态为活动
    /// </summary>
    public void UpdateState()
    {
         StringBuilder strSql = new StringBuilder();
        strSql.Append(@"
UPDATE Customers a
SET a.State = 'Active'
WHERE a.State !='VIP'
AND EXISTS (
SELECT 1 FROM TradeTransaction b WHERE a.CompanyCode = b.CompanyCode AND a.CustomerNameCode = b.TraderCode LIMIT 1
);");

        MySqlHelper.ExecuteNonQuery(strSql.ToString());
    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    public void Delete(int ID)
    {

        StringBuilder strSql = new StringBuilder();
        strSql.Append("delete from Customers ");
        strSql.Append(" where ID=@ID ");
        MySqlParameter[] parameters = {
					MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,ID)};

        MySqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
    }


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public CustomerInfo GetInfo(int ID)
    {
        CustomerInfo info = null;

        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"select
b.ID
,b.CompanyCode
,b.CustomerNameCode
,b.CustomerName
,b.CustomerType
,b.State
,b.QQ
,b.Email
,b.CellPhoneNumber
,b.ContactPhoneNumber
,b.Address
,b.IDCardNumber
,b.RefSeller
,b.InDate
,b.InUser
,c.ID AS UserID
,c.UserName
FROM Customers b
LEFT JOIN m_User c
ON b.RefSeller = c.ID
");
        strSql.Append(" where b.ID=@ID ");
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

    ///// <summary>
    ///// 获得数据列表
    ///// </summary>
    //public List<CustomerInfo> GetList(string strWhere)
    //{
    //    List<CustomerInfo> list = new List<CustomerInfo>();
    //    StringBuilder strSql = new StringBuilder();
    //    strSql.Append("select ID,UserName,PassWord ");
    //    strSql.Append(" FROM Customers ");
    //    strSql.Append(" where 1=1 ");
    //    strSql.Append(strWhere);
    //    using (IDataReader dataReader = MySqlHelper.ExecuteReader(strSql.ToString()))
    //    {
    //        while (dataReader.Read())
    //        {
    //            list.Add(ReaderBind(dataReader));
    //        }
    //    }
    //    return list;
    //}

    //public List<CustomerInfo> GetList(int startRowIndex, int maxRows, out int recordCount, string strWhere)
    //{
    //    List<CustomerInfo> list = new List<CustomerInfo>();
    //    StringBuilder strSql = new StringBuilder();
    //    strSql.Append("select ID,UserName,PassWord ");
    //    strSql.Append(" FROM Customers ");
    //    strSql.Append(" where 1=1 ");
    //    strSql.Append(strWhere);

    //    MySqlParameter cntParam = new MySqlParameter("recordCount", MySqlDbType.Int32);
    //    cntParam.Direction = ParameterDirection.Output;
    //    MySqlParameter[] parms = new MySqlParameter[]{
    //            MySqlHelper.CreateParameter("startRowIndex",MySqlDbType.VarChar,startRowIndex, ParameterDirection.Input),
    //            MySqlHelper.CreateParameter("pageSize",MySqlDbType.Int32,maxRows, ParameterDirection.Input),
    //            cntParam,              
    //            MySqlHelper.CreateParameter("strSql",MySqlDbType.VarChar,strSql.ToString(), ParameterDirection.Input)};
    //    using (IDataReader dataReader = MySqlHelper.ExecuteReader(CommandType.StoredProcedure, "DataPageing", parms))
    //    {
    //        while (dataReader.Read())
    //        {
    //            list.Add(ReaderBind(dataReader));
    //        }
    //    }

    //    recordCount = int.Parse(cntParam.Value.ToString());

    //    return list;
    //}



    public DataTable GetData(QueryFilter filter)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"select
b.ID
,b.CompanyCode
,b.CustomerNameCode
,b.CustomerName
,b.CustomerType
,b.State
,b.QQ
,b.Email
,b.CellPhoneNumber
,b.ContactPhoneNumber
,b.Address
,b.IDCardNumber
,b.RefSeller
,b.InDate
,b.InUser
,c.ID AS UserID
,c.UserName
FROM Customers b
LEFT JOIN m_User c
ON b.RefSeller = c.ID
LEFT JOIN User_Node n
ON c.ID = n.UserID
");

        strSql.Append(" where 1=1");


        List<MySqlParameter> parmslist = new List<MySqlParameter>();

        if (filter.CompanyCode != null)
        {
            strSql.Append(" and (b.CompanyCode = @CompanyCode  OR b.CompanyCode like CONCAT( @CompanyCode ,'%')  AND  b.RefSeller IS NULL   )");
            parmslist.Add(MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar, filter.CompanyCode));
        }

        if (filter.NodePath != null)
        {
            strSql.Append(" and (n.NodePath like CONCAT(@NodePath ,'%') OR  b.RefSeller IS NULL)");
            parmslist.Add(MySqlHelper.CreateParameter("@NodePath", MySqlDbType.VarChar, filter.NodePath));
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
            strSql.Append(" and b.CustomerNameCode >= @TraderCode_Begin");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderCode_Begin", MySqlDbType.VarChar, filter.TraderCode_Begin));
        }
        if (filter.TraderCode_End != null)
        {
            strSql.Append(" and b.CustomerNameCode <=  @TraderCode_End");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderCode_End", MySqlDbType.VarChar, filter.TraderCode_End));
        }

        if (filter.TraderName != null)
        {
            strSql.Append(" and b.CustomerName  like CONCAT('%',@TraderName,'%')");
            parmslist.Add(MySqlHelper.CreateParameter("@TraderName", MySqlDbType.VarChar, filter.TraderName));
        }

        if (filter.State != "ALL")
        {
            strSql.Append(" and b.State =  @State");
            parmslist.Add(MySqlHelper.CreateParameter("@State", MySqlDbType.VarChar, filter.State));
        }


        strSql.Append(@" order by b.CustomerNameCode");
        // string sql = MySqlHelper.GetStringSql(strSql.ToString(), parmslist);
        //MySqlParameter cntParam = new MySqlParameter("@recordCount", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
        //MySqlParameter[] parms = new MySqlParameter[]{
        //         MySqlHelper.CreateParameter("@startRowIndex",MySqlDbType.VarChar,filter.Pager.startRowIndex, ParameterDirection.Input),
        //         MySqlHelper.CreateParameter("@pageSize",MySqlDbType.Int32,filter.Pager.pageSize, ParameterDirection.Input),
        //         cntParam};
        //parmslist.AddRange(parms);
        //     MySqlHelper.CreateParameter("@strSql",MySqlDbType.VarChar,sql, ParameterDirection.Input
        //DataTable dt = MySqlHelper.ExecuteTable(CommandType.StoredProcedure, "DataPageing", parms);
        // filter.Pager.recordCount = int.Parse(cntParam.Value.ToString());

        string sql = MySqlHelper.WrapPagingSqlString(strSql.ToString(), filter.Pager);
        DataTable dt = MySqlHelper.ExecuteTable(CommandType.Text, sql, parmslist.ToArray());

        sql = MySqlHelper.WrapCountSqlString(strSql.ToString());
        filter.Pager.recordCount = int.Parse(MySqlHelper.ExecuteScalar(sql, parmslist.ToArray()).ToString());
        return dt;

    }

    #endregion  成员方法


    #region  私有方法


    private static MySqlParameter[] CreateParameters(CustomerInfo info)
    {
        int? sellerID = null;
        if (info.RefSeller.ID > 0)
        {
            sellerID = info.RefSeller.ID;
        }

        MySqlParameter[] parameters = {
                    MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,info.ID),
                    MySqlHelper.CreateParameter("@CompanyCode", MySqlDbType.VarChar,info.CompanyCode),
                    MySqlHelper.CreateParameter("@CustomerNameCode", MySqlDbType.VarChar, info.CustomerNameCode),
                    MySqlHelper.CreateParameter("@CustomerName", MySqlDbType.VarChar, info.CustomerName),
                    MySqlHelper.CreateParameter("@CustomerType", MySqlDbType.VarChar, info.CustomerType),
                    MySqlHelper.CreateParameter("@State", MySqlDbType.VarChar, info.State),
                    MySqlHelper.CreateParameter("@QQ", MySqlDbType.VarChar, info.QQ),
                    MySqlHelper.CreateParameter("@Email", MySqlDbType.VarChar, info.Email),
                    MySqlHelper.CreateParameter("@CellPhoneNumber", MySqlDbType.VarChar, info.CellPhoneNumber),
                    MySqlHelper.CreateParameter("@ContactPhoneNumber", MySqlDbType.VarChar, info.ContactPhoneNumber),
                    MySqlHelper.CreateParameter("@Address", MySqlDbType.VarChar, info.Address),
                    MySqlHelper.CreateParameter("@IDCardNumber", MySqlDbType.VarChar, info.IDCardNumber),
                    MySqlHelper.CreateParameter("@RefSeller", MySqlDbType.Int32,sellerID),
                    MySqlHelper.CreateParameter("@InDate", MySqlDbType.DateTime, info.InDate),
                    MySqlHelper.CreateParameter("@InUser", MySqlDbType.VarChar, info.InUser)
            };
        return parameters;

    }

    /// <summary>
    /// 对象实体绑定数据
    /// </summary>
    private CustomerInfo ReaderBind(IDataReader dataReader)
    {
        CustomerInfo info = new CustomerInfo();
        info.ID = (Int32)dataReader["ID"];
        info.CompanyCode = dataReader["CompanyCode"].ToString();
        info.CustomerNameCode = dataReader["CustomerNameCode"].ToString();
        info.CustomerName = dataReader["CustomerName"].ToString();
        info.CustomerType = dataReader["CustomerType"].ToString();
        info.State = dataReader["State"].ToString();
        info.QQ = dataReader["QQ"].ToString();
        info.Email = dataReader["Email"].ToString();
        info.CellPhoneNumber = dataReader["CellPhoneNumber"].ToString();
        info.ContactPhoneNumber = dataReader["ContactPhoneNumber"].ToString();
        info.Address = dataReader["Address"].ToString();
        info.IDCardNumber = dataReader["IDCardNumber"].ToString();
        if (dataReader["InDate"] != null && dataReader["InDate"] != DBNull.Value)
        {
            info.InDate = (DateTime)dataReader["InDate"];
        }
        info.InUser = dataReader["InUser"].ToString();

        info.RefSeller = new UserInfo();
        if (dataReader["UserID"] != DBNull.Value)
        {
            info.RefSeller.ID = (int)dataReader["UserID"];
        }
        info.RefSeller.UserName = dataReader["UserName"].ToString();


        return info;
    }
    #endregion  私有方法


    //public DataTable GetData(string strWhere)
    //{
    //    List<CustomerInfo> list = new List<CustomerInfo>();
    //    StringBuilder strSql = new StringBuilder();
    //    strSql.Append("select ID,UserName,PassWord,Role ");
    //    strSql.Append(" FROM Customers ");
    //    strSql.Append(" where 1=1 ");
    //    strSql.Append(strWhere);

    //    return MySqlHelper.ExecuteTable(strSql.ToString());
    //}


    public DataTable GetMemo(QueryFilter filter)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"select
ID
,CustomerID
,Memo
,InDate
,InUser
FROM Customers_Memo
");

        strSql.Append(" where 1=1");

        List<MySqlParameter> parmslist = new List<MySqlParameter>();

        if (filter.CustomerID != null)
        {
            strSql.Append(" and CustomerID = @CustomerID");
            parmslist.Add(MySqlHelper.CreateParameter("@CustomerID", MySqlDbType.Int32, filter.CustomerID));
        }

        strSql.Append(@"
order by ID ");

        //string sql = MySqlHelper.GetStringSql(strSql.ToString(), parmslist);


        //MySqlParameter cntParam = new MySqlParameter("recordCount", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
        //MySqlParameter[] parms = new MySqlParameter[]{
        //         MySqlHelper.CreateParameter("@startRowIndex",MySqlDbType.VarChar,filter.Pager.startRowIndex, ParameterDirection.Input),
        //         MySqlHelper.CreateParameter("@pageSize",MySqlDbType.Int32,filter.Pager.pageSize, ParameterDirection.Input),
        //         cntParam,              
        //         MySqlHelper.CreateParameter("@strSql",MySqlDbType.VarChar,sql, ParameterDirection.Input)};

        //DataTable dt = MySqlHelper.ExecuteTable(CommandType.StoredProcedure, "DataPageing", parms);

        //filter.Pager.recordCount = int.Parse(cntParam.Value.ToString());

        string sql = MySqlHelper.WrapPagingSqlString(strSql.ToString(), filter.Pager);
        DataTable dt = MySqlHelper.ExecuteTable(CommandType.Text, sql, parmslist.ToArray());

        sql = MySqlHelper.WrapCountSqlString(strSql.ToString());
        filter.Pager.recordCount = int.Parse(MySqlHelper.ExecuteScalar(sql, parmslist.ToArray()).ToString());

        return dt;

    }

    public int AddMemo(CustomerMemoInfo memo)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(@"insert into Customers_Memo(
    CustomerID
    ,Memo
    ,InDate
    ,InUser
)values (
    @CustomerID
    ,@Memo
    ,@InDate
    ,@InUser
)

");
        strSql.Append(";select @@IDENTITY;");

        MySqlParameter[] parameters = CreateParameters(memo);

        object ret = MySqlHelper.ExecuteScalar(strSql.ToString(), parameters);
        return ret != null && ret != DBNull.Value ? Convert.ToInt32(ret) : 0;
    }

    private MySqlParameter[] CreateParameters(CustomerMemoInfo memo)
    {
        MySqlParameter[] parameters = {
                    MySqlHelper.CreateParameter("@ID", MySqlDbType.Int32,memo.ID),
                    MySqlHelper.CreateParameter("@CustomerID", MySqlDbType.Int32,memo.CustomerID),
                    MySqlHelper.CreateParameter("@Memo", MySqlDbType.VarChar, memo.Memo),
                    MySqlHelper.CreateParameter("@InDate", MySqlDbType.DateTime, memo.InDate),
                    MySqlHelper.CreateParameter("@InUser", MySqlDbType.VarChar, memo.InUser)
            };
        return parameters;
    }

}
