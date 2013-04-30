using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CustomerInfo 的摘要说明
/// </summary>
public class CustomerInfo
{
    public int ID { get; set; }
    public string CompanyCode { get; set; }
    public string CustomerNameCode { get; set; }
    public string CustomerName { get; set; }
    public string CustomerType { get; set; }
    public string State { get; set; }
    public string QQ { get; set; }
    public string Email { get; set; }
    public string CellPhoneNumber { get; set; }
    public string ContactPhoneNumber { get; set; }
    public string Address { get; set; }
    public string IDCardNumber { get; set; }
    public DateTime InDate { get; set; }
    public string InUser { get; set; }
    public UserInfo RefSeller { get; set; }
}