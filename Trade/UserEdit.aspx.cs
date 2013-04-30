using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;
using System.Web.Security;
using System.Data;
using System.Text.RegularExpressions;

public partial class UserEdit : PageBase
{
    UserBiz biz = null;
    int id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo loginUser = Common.GetLoginUser();
        if (!Common.CheckAuth(loginUser, Role_Function.UserEdit))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }
        biz = new UserBiz();

        int.TryParse(Request.QueryString["id"], out id);

        this.PrevURL = "UserList.aspx";
        if (!IsPostBack)
        {
            InitContralValue();
            BindData();
        }
    }

    private void BindData()
    {
        if (id <= 0)
        {
            return;
        }
        UserInfo uInfo = biz.GetInfo(id);

        if (uInfo != null)
        {
            if (!Common.CheckAuth(this.LoginUser, Role_Function.Administrator))
            {
                if (!Common.IsLeader(uInfo.NodePath, LoginUser.NodePath))
                {
                    MessageBox.Show(Setting.NoPermissionMsg);
                    this.btnOK.Visible = false;
                    return;
                }
            }

            this.UserName.Text = uInfo.UserName;
            if (uInfo.Role != null)
            {
                this.ddlRole.SelectedValue = uInfo.Role.RoleID.ToString();
            }

            // 只要所选用户不是管理员角色，移除
            if (uInfo.Role == null || !Common.CheckAuth(uInfo, Role_Function.Administrator))
            {
                //this.ddlRole.Items.Remove(this.ddlRole.Items.FindByText("Administrator"));
            }
            else if (Common.CheckAuth(uInfo, Role_Function.Administrator))
            {
                this.ddlRole.Items.Add(new ListItem("系统管理员", "1"));
                this.ddlRole.SelectedIndex = ddlRole.Items.Count - 1;
                this.ddlRole.Enabled = false;
            }

            ddlCompany.SelectedValue = uInfo.CompanyCode;

            ddlUser.Items.Remove(ddlUser.Items.FindByValue(uInfo.ID.ToString()));
            ddlUser.SelectedValue = uInfo.PID.ToString();
        }
        else
        {
            this.ddlRole.SelectedValue = "Seller";
        }
    }

    private void BindData_User()
    {
        QueryFilter filter = new QueryFilter();
        filter.CompanyCode = this.ddlCompany.SelectedValue;
        if (!Common.CheckAuth(this.LoginUser, Role_Function.Administrator))
        {
            filter.NodePath = LoginUser.NodePath;
        }

        List<UserInfo> userlist = biz.GetUserTreeList(filter);
        if (Common.CheckAuth(this.LoginUser, Role_Function.Manager))
        {
            userlist.Insert(0, new UserInfo() { UserName = "--------无---------", ID = 0 });
        }

        this.ddlUser.DataSource = userlist;
        this.ddlUser.DataBind();
    }

    private void InitContralValue()
    {
        UserInfo loginUser = Common.GetLoginUser();

        DataTable dt = biz.GetRoleData();
        DataView datasource = dt.DefaultView;
        datasource.Sort = "RoleID desc";
        ddlRole.DataSource = datasource;
        ddlRole.DataBind();

        DataTable dtCompany = biz.GetCompanyData();

        ddlCompany.DataSource = dtCompany;
        ddlCompany.DataBind();

        if (!Common.CheckAuth(loginUser, Role_Function.Administrator))
        {
            ddlCompany.SelectedValue = loginUser.CompanyCode;
            ddlCompany.Enabled = false;
        }

        BindData_User();

        if (ddlUser.SelectedIndex <= 0)
        {
            ddlUser.SelectedValue = loginUser.ID.ToString();
        }

        if (id <= 0)
        {
            ModeChange(true);
        }
    }

    private void ModeChange(bool isAddMode)
    {
        this.ckbRestPassword.Visible = false;
        this.UserName.ReadOnly = false;
        this.txtPassword.ReadOnly = false;
        this.txtTitle.Text = "新增用户";
    }

    private bool CheckInput()
    {
        if (this.UserName.Text.Trim().Length <= 0)
        {
            MessageBox.Show("请输入用户名。");
            return false;
        }

        if (this.ckbRestPassword.Checked
            && this.txtPassword.Text.Length <= 0)
        {
            MessageBox.Show("请输入密码。");
            return false;
        }

        return true;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.UserEdit))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        if (!CheckInput())
        {
            return;
        }

        try
        {
            UserInfo uinfo = new UserInfo();
            uinfo.ID = id;
            uinfo.CompanyCode = ddlCompany.SelectedValue;
            uinfo.UserName = this.UserName.Text.Trim();
            uinfo.Password = AuthSecurity.Encrypt(this.txtPassword.Text.Trim());
            uinfo.PID = int.Parse(this.ddlUser.SelectedValue);
            uinfo.Role = new RoleInfo();
            uinfo.Role.RoleID = Convert.ToInt32(this.ddlRole.SelectedValue);

            if (id <= 0)
            {
                biz.Add(uinfo);
            }
            else
            {
                biz.UpdateAll(uinfo, this.ckbRestPassword.Checked);
            }

            MessageBox.Show("修改成功。", this.PrevURL);
        }
        catch (BusinessException ex)
        {
            MessageBox.Show(ex.Message);
        }

        catch (Exception ex)
        {
            MessageBox.Show("保存失败。");
            Logger.Log.Error("update user Exception.", ex);
        }
    }
    protected void ckbRestPassword_CheckedChanged(object sender, EventArgs e)
    {
        this.txtPassword.ReadOnly = !this.ckbRestPassword.Checked;
    }
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData_User();
    }


}