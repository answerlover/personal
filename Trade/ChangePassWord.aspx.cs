using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;
using System.Web.Security;

public partial class ChangePassWord : PageBase
{
    UserBiz biz = new UserBiz();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PrevURL = "CustomerList.aspx";
    }
    protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
    {
        UserInfo uInfo = this.LoginUser;

        if (null == biz.GetInfo(uInfo.UserName, AuthSecurity.Encrypt(this.CurrentPassword.Text)))
        {
            MessageBox.Show("密码验证失败。");
            return;
        }

        uInfo.Password = AuthSecurity.Encrypt(this.NewPassword.Text);
        biz.Update(uInfo);
        MessageBox.Show("修改成功。", this.PrevURL);
    }
    protected void CancelPushButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(this.PrevURL);
    }
}