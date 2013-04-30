using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 查询过滤器
/// </summary>
[Serializable]
public class QueryFilter
{
    public string CompanyCode { get; set; }
    public string TraderCode_Begin { get; set; }
    public string TraderCode_End { get; set; }
    public string TraderName { get; set; }
    public int? SellerID { get; set; }
    public DateTime? SettlementDate_Begin { get; set; }
    public DateTime? SettlementDate_End { get; set; }
    public string State { get; set; }

    public int? CustomerID { get; set; }

    public PagerInfo Pager { get; set; }

    public string NodePath { get; set; }

}

[Serializable]
public class PagerInfo
{
    public int startRowIndex = 1;
    public int pageSize = 30;
    public int recordCount = 0;
}