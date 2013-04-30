using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// UserInfo 的摘要说明
/// </summary>
[Serializable]
public class UserInfo
{
    public int ID { get; set; }
    public string CompanyCode { get; set; }
    public string CompanyName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int PID { get; set; }
    public string NodePath { get; set; }
   
    public RoleInfo Role { get; set; }
    public List<string> AuthList = new List<string>();
}