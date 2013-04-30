using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// 权限认证
/// </summary>
public class AuthenticationModule : IHttpModule//, IReadOnlySessionState
{
    #region IHttpModule 成员

    public void Dispose()
    {
        //throw new NotImplementedException();
    }

    public void Init(HttpApplication context)
    {
        context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
    }

    void context_AcquireRequestState(object sender, EventArgs e)
    {
        HttpContext context = HttpContext.Current;
        string requestPath = context.Request.AppRelativeCurrentExecutionFilePath.ToLower();
        if (Regex.IsMatch(requestPath,"^~/[a-z]")//StartsWith("~/admin") 
            && !requestPath.StartsWith("~/admin/login.aspx")
            && !requestPath.StartsWith("~/admin/ckeditor")
            && !requestPath.StartsWith("~/admin/ckfinder")
            && !requestPath.StartsWith("~/scripts")
            && !requestPath.StartsWith("~/styles")
            && !requestPath.StartsWith("~/images")
             && !requestPath.Contains("resource.axd")
           && !requestPath.StartsWith("~/login")
           && !requestPath.StartsWith("~/default")
           && !requestPath.StartsWith("~/index.")
           && !requestPath.StartsWith("~/404.")
           && !requestPath.StartsWith("~/500.")
            )
        {
            if (context.Session == null || context.Session["UserName"] == null)
            {
                context.Response.Redirect("~/Login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(context.Request.Url.PathAndQuery));//.AppRelativeCurrentExecutionFilePath
            }
        }
    }

    #endregion
}