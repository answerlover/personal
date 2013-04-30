using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class AuthManage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.Manager))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        QueryFilter filter = new QueryFilter();
        filter.CompanyCode = this.LoginUser.CompanyCode;
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = 1;
        filter.Pager.pageSize = int.MaxValue;

        CompanyBiz biz = new CompanyBiz();

        DataTable dt = biz.GetData(filter);
        this.ddlCompany.DataSource = dt;
        this.ddlCompany.DataBind();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.Manager))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        this.LoginUser.CompanyCode = this.ddlCompany.SelectedValue;

        Session["UserName"] = this.LoginUser;

        MessageBox.Show("切换成功，如果需要登录其它公司，请重新登录后再切换。", "UserList.aspx");
    }
    protected void btnAutoGonghai_Click(object sender, EventArgs e)
    {
        // DB版本老，把Job放这里。
        MySqlHelper.ExecuteNonQuery("call UP_AutoGongHai();");
        MessageBox.Show("操作成功。");

    }
}