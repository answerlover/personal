using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// TradeBiz 的摘要说明
/// </summary>
public class TradeBiz
{
    TradeDal dal = new TradeDal();

    /// <summary>
    /// 增加一条数据
    /// </summary>
    public int Add(TradeTransactionInfo info)
    {
        int id = dal.Add(info);

        return id;
    }

    public int Add(List<TradeTransactionInfo> list)
    {
        TradeTransactionInfo firstItem = list.FirstOrDefault();
        if (firstItem != null && dal.IsExists(firstItem))
        {
            throw new BusinessException(string.Format("系统中已经存在结算日期为[{0}]的数据，请删除后再导入。 ", firstItem.SettlementDate.ToString("yyyy-MM-dd")));
        }

        int count = 0;

        using (MySqlConnection conn = MySqlHelper.CreateConnection(MySqlHelper.ConnectionString))
        {
            conn.Open();
            dal = new TradeDal(conn);

            MySqlTransaction ts = conn.BeginTransaction();

            foreach (TradeTransactionInfo info in list)
            {
                count += this.Add(info);
            }


            ts.Commit();
        }
        new CustomerDal().UpdateState();
        return count;
    }

    //public List<TradeTransactionInfo> GetList(int startRowIndex, int maxRows, out int recordCount, string strWhere)
    //{
    //    return dal.GetList(startRowIndex, maxRows, out recordCount, strWhere);
    //}

    public List<TradeTransactionInfo> GetList(QueryFilter filter)
    {
        return dal.GetList(filter);
    }

    public DataTable GetSummaryData(QueryFilter filter)
    {
        DataTable dt = dal.GetSummaryData(filter);
        return dt;
    }

    public List<TradeTransactionInfo> TransferToEntityList(DataTable dt, DateTime settlementDate)
    {
        List<TradeTransactionInfo> list = new List<TradeTransactionInfo>();
        if (dt != null && dt.Columns.Count > 1)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString().Trim() == "合计")
                {
                    continue;
                }
                
                TradeTransactionInfo info = new TradeTransactionInfo();
                info.TraderCode = dr[0].ToString(); //交易商代码
                info.TraderName = dr[1].ToString(); // 交易商名称
                info.BelongCode = dr[2].ToString(); // 隶属
                info.TradeCount = Int32.Parse(dr[3].ToString(), System.Globalization.NumberStyles.AllowThousands); // 成交数量
                info.TradeFee = Convert.ToDecimal(dr[4]); // 交易手续费(元)
                info.OrganizationFee = Convert.ToDecimal(dr[5]); // 授权服务机构手续费
                info.ExchangeFee = Convert.ToDecimal(dr[6]); // 交易所手续费
                info.SettlementDate = settlementDate;
                info.InDate = DateTime.Now;
                info.InUser = Common.GetLoginUser().UserName;

                list.Add(info);
            }
        }


        return list;
    }


    public void Delete(List<int> idlist)
    {
        idlist.ForEach(id => dal.Delete(id));
    }

}