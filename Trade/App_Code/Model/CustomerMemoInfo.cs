using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CustomerMemoInfo 的摘要说明
/// </summary>
public class CustomerMemoInfo
{
    public int ID { get; set; }
    public int CustomerID { get; set; }
    public string Memo { get; set; }
    public DateTime InDate { get; set; }
    public string InUser { get; set; }
}