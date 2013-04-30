using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// TradeTransactionInfo 的摘要说明
/// </summary>
public class TradeTransactionInfo
{
    public int ID { get; set; }
    public string CompanyCode { get; set; }
    public string TraderCode { get; set; }
    public string TraderName { get; set; }
    public string BelongCode { get; set; }
    public int TradeCount { get; set; }
    public decimal TradeFee { get; set; }
    public decimal OrganizationFee { get; set; }
    public decimal ExchangeFee { get; set; }
    public DateTime SettlementDate { get; set; }
    public DateTime InDate { get; set; }
    public string InUser { get; set; }

    public UserInfo RefUser { get; set; }
}