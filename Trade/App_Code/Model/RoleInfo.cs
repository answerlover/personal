using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// RoleInfo 的摘要说明
/// </summary>
[Serializable]
public class RoleInfo
{
    public int RoleID { get; set; }
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }
}