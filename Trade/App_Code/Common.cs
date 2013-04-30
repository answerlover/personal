using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using wandou4;

/// <summary>
/// Common 的摘要说明
/// </summary>
public class Common
{
    public static UserInfo GetLoginUser()
    {
        if (HttpContext.Current.Session == null)
        {
            return null;
        }
        return (UserInfo)HttpContext.Current.Session["UserName"];
    }

    public static UserInfo Login(string username, string password)
    {
        UserInfo uInfo = new UserBiz().GetInfo(username, AuthSecurity.Encrypt(password));
        HttpContext.Current.Session["UserName"] = uInfo;

        return uInfo;
    }

    public static void Logout()
    {
        HttpContext.Current.Session["UserName"] = null;
    }

    public static bool CheckAuth(string authlist)
    {
        UserInfo loginUser = Common.GetLoginUser();
        return CheckAuth(loginUser, authlist);
    }

    public static bool CheckAuth(UserInfo loginUser, string authlist)
    {
        bool ret = loginUser.AuthList.Contains(authlist);
        return ret;
    }

    public static bool IsLeader(string sourceNodePath, string targetNodePath)
    {
        if (sourceNodePath == null || targetNodePath == null)
        {
            return false;
        }

        return !Regex.Match(targetNodePath, "^" + sourceNodePath + ".+").Success;
    }

}