using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using wandou4;

public partial class Admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = Setting.SiteTitle;
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {
        UserInfo uInfo = Common.Login(this.UserName.Text.Trim(), this.Password.Text.Trim());
        if (uInfo != null)
        {
            Session.Timeout = 30;
            Session["UserName"] = uInfo;
            //FormsAuthentication.RedirectFromLoginPage(this.UserName.Text, this.RememberMe.Checked);

            string ReturnUrl = Request["ReturnUrl"];

            Response.Redirect(String.IsNullOrEmpty(ReturnUrl) || ReturnUrl.Length < 4 ? "TradeTransactionList.aspx" : HttpUtility.UrlDecode(Request["ReturnUrl"]));
        }
        else
        {
            MessageBox.Show("登录失败，请检查你的用户名和密码。");
        }

        //<?xml version="1.0"?>
        //<configuration>

        //  <location path="Register.aspx">
        //    <system.web>
        //      <authorization>
        //        <allow users="*"/>
        //      </authorization>
        //    </system.web>
        //  </location>

        //  <system.web>
        //    <authorization>
        //      <deny users="?"/>
        //    </authorization>
        //  </system.web>

        //</configuration>

    }
}