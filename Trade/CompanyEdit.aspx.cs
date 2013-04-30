using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;
using System.Web.Security;
using System.Data;

public partial class CompanyEdit : PageBase
{
    CompanyBiz biz = new CompanyBiz();
    int id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.Administrator))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        int.TryParse(Request.QueryString["id"], out id);

        this.PrevURL = "CompanyList.aspx";
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
        CompanyInfo info = biz.GetInfo(id);
        this.CompanyCode.Text = info.CompanyCode;
        this.CompanyName.Text = info.CompanyName;
    }

    private void InitContralValue()
    {
        if (id > 0)
        {
            this.CompanyCode.ReadOnly = true;
        }
    }

    private bool CheckInput()
    {
        if (this.CompanyCode.Text.Trim().Length <= 0)
        {
            MessageBox.Show("公司代码必填。");
            return false;
        }
        return true;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.Administrator))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        if (!CheckInput())
        {
            return;
        }
        CompanyInfo info = GetInfo();
        try
        {
            if (id > 0)
            {
                biz.Update(info);
            }
            else
            {
                biz.Add(info);
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
            Logger.Log.Error("update Company Exception.", ex);
        }
    }

    private CompanyInfo GetInfo()
    {
        CompanyInfo info = new CompanyInfo();
        info.ID = id;
        info.CompanyCode = this.CompanyCode.Text.Trim();
        info.CompanyName = this.CompanyName.Text.Trim();
        return info;
    }
}