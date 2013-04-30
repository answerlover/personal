using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// PageBase 的摘要说明
/// </summary>
public class PageBase : Page
{
    private string _prevURL;
    protected String PrevURL
    {
        get
        {
            return _prevURL ?? ViewState["UrlReferrer"] as string;
        }
        set
        {
            _prevURL = value;
        }
    }

    public PageBase()
    {
        this.Load += PageBase_Load;
    }

    protected UserInfo LoginUser = null;

    void PageBase_Load(object sender, EventArgs e)
    {
        this.Title = Setting.SiteTitle;
        LoginUser = Common.GetLoginUser();
        if (!IsPostBack)
        {
            string url = Request.UrlReferrer == null ? "" : Request.UrlReferrer.PathAndQuery;
            if (!url.Contains("Login.aspx"))
            {
                ViewState["UrlReferrer"] = url;
            }
        }
    }
}