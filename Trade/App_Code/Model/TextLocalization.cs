using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// TextLocalization 的摘要说明
/// </summary>
public class TextLocalization
{
    public static string CustomerTypeFormat(object type)
    {
        if (type == null)
        {
            return null;
        }
        return type.ToString().Replace("Personal", "个人").Replace("Company", "公司");
    }

    public static string CustomerStateFormat(object state)
    {
        if (state == null)
        {
            return null;
        }
        return state.ToString().Replace("New", "开户").Replace("Active", "活动").Replace("VIP", "会员");
    }
}