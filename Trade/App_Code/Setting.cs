using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///setting 的摘要说明
/// </summary>
public static class Setting
{
    /// <summary>
    /// Item图片上传目录
    /// </summary>
    public const string SettlementUpfilePath = "~/upload/SettlementReport/";
    /// <summary>
    /// 错误日志文件路径
    /// </summary>
    public const string ErrorLogPath = "~/Log/Error.log";
    /// <summary>
    /// 域名
    /// </summary>
    public static readonly string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];

    public static string NoPermissionMsg = "对不起，你没有权限进行该操作。";

    public static string SiteTitle = "渤海商品交易所成都临远投资管理有限公司";
}