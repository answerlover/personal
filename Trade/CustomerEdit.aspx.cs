using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class CustomerEdit : PageBase
{
    CustomerBiz biz = new CustomerBiz();
    int id = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PrevURL = "CustomerList.aspx";

        int.TryParse(Request.QueryString["id"], out id);

        //if (Common.CheckAuth(LoginUser, Role_Function.CustomerEdit))
        //{
        //    MessageBox.Show(Setting.NoPermissionMsg, this.PrevURL);
        //    return;
        //}

        if (!IsPostBack)
        {
            InitControls();

            BindData();
        }
    }

    private void BindData()
    {
        if (id > 0)
        {
            CustomerInfo info = biz.GetInfo(id);
            if (info == null)
            {
                return;
            }

            if (!Common.CheckAuth(LoginUser, Role_Function.Administrator)
            && !(LoginUser.CompanyCode == info.CompanyCode && Common.CheckAuth(LoginUser, Role_Function.CustomerEdit) || info.CompanyCode.StartsWith(LoginUser.CompanyCode) && info.RefSeller.ID == 0
                || this.LoginUser.ID == info.RefSeller.ID)
            )
            {
                MessageBox.Show(Setting.NoPermissionMsg, this.PrevURL);
                return;
            }

            SetDisplayItemInfo(info);
            BindData_Memo();
        }
    }

    private void BindData_Memo()
    {
        QueryFilter filter = null;
        filter = new QueryFilter();
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = this.anPager.StartRecordIndex;
        filter.Pager.pageSize = this.anPager.PageSize;
        filter.Pager.recordCount = 0;


        filter.CustomerID = id;


        DataTable dt = biz.GetMemo(filter);
        this.rptMemo.DataSource = dt;
        this.rptMemo.DataBind();

        this.anPager.RecordCount = filter.Pager.recordCount;
    }


    private void InitControls()
    {
        if (id > 0)
        {
            pnlLog.Visible = true;
        }

        QueryFilter filter = new QueryFilter();
        filter.CompanyCode = LoginUser.CompanyCode;
        if (!Common.CheckAuth(this.LoginUser, Role_Function.Manager))
        {
            filter.NodePath = LoginUser.NodePath;

        }
        List<UserInfo> users = new UserBiz().GetUserTreeList(filter);
        users.Insert(0, new UserInfo() { ID = 0, UserName = "--无--" });

        ddlSeller.DataSource = users;
        ddlSeller.DataBind();

        if (id <= 0) // new add
        {
            UserInfo loginUser = Common.GetLoginUser();
            this.ddlSeller.SelectedValue = loginUser.ID.ToString();
        }

        rblState_SelectedIndexChanged(null, null);

    }
    private void SetDisplayItemInfo(CustomerInfo info)
    {
        this.txtID.Text = info.ID.ToString();
        this.txtCustomerNameCode.Text = info.CustomerNameCode;
        this.txtCustomerName.Text = info.CustomerName;
        this.rblCustomerType.SelectedValue = info.CustomerType;
        this.rblState.SelectedValue = info.State;
        this.txtQQ.Text = info.QQ;
        this.txtEmail.Text = info.Email;
        this.txtCellPhoneNumber.Text = info.CellPhoneNumber;
        this.txtContactPhoneNumber.Text = info.ContactPhoneNumber;
        this.txtIDCardNumber.Text = info.IDCardNumber;
        this.txtAddress.Text = info.Address;
        this.txtInDate.Text = info.InDate.ToString("yyyy-MM-dd HH:mm");
        //this.txtInUser.Text = info.InUser;
        this.ddlSeller.SelectedValue = info.RefSeller.ID.ToString();

        //  UserInfo refSeller = new UserDal().GetInfo(info.RefSeller.ID);

        if (!Common.CheckAuth(this.LoginUser, Role_Function.CustomerEdit) && !Common.CheckAuth(LoginUser, Role_Function.Administrator)
            //&& (refSeller != null && !Common.IsLeader(LoginUser.NodePath, refSeller.NodePath))
            )
        {
            this.ddlSeller.Enabled = false;
        }

        if (!Common.CheckAuth(this.LoginUser, Role_Function.CustomerEdit) && this.LoginUser.ID != info.RefSeller.ID)
        {
            this.btnSave.Visible = false;
            this.btnAddMemo.Visible = false;
        }

    }

    private CustomerInfo GetInfo()
    {
        CustomerInfo info = new CustomerInfo();
        info.ID = id;
        info.CompanyCode = Common.GetLoginUser().CompanyCode;
        info.CustomerNameCode = txtCustomerNameCode.Text;
        info.CustomerName = txtCustomerName.Text;
        info.CustomerType = rblCustomerType.SelectedValue;
        info.State = rblState.SelectedValue;
        info.QQ = txtQQ.Text;
        info.Email = txtEmail.Text;
        info.CellPhoneNumber = txtCellPhoneNumber.Text;
        info.ContactPhoneNumber = txtContactPhoneNumber.Text;
        info.IDCardNumber = txtIDCardNumber.Text.Trim();
        info.Address = txtAddress.Text;
        info.InDate = DateTime.Now;
        info.InUser = Common.GetLoginUser().UserName;

        info.RefSeller = new UserInfo();
        info.RefSeller.ID = Convert.ToInt32(this.ddlSeller.SelectedItem.Value);
        info.RefSeller.UserName = this.ddlSeller.SelectedItem.Text;

        return info;
    }


    private bool CheckInput()
    {
        if (this.txtCustomerNameCode.Text.Trim().Length <= 0)
        {
            MessageBox.Show("交易商代码必填。");

            return false;
        }
        if (this.txtCellPhoneNumber.Text.Trim().Length <= 0)
        {
            MessageBox.Show("手机号码必填。");

            return false;
        }

        if (this.rblState.SelectedValue == "Open" && txtIDCardNumber.Text.Trim().Length <= 0)
        {
            MessageBox.Show("身份证号码必填。");

            return false;
        }

        if (this.rblState.SelectedValue == "New" && this.txtAddress.Text.Trim().Length <= 0)
        {
            MessageBox.Show("联系地址必填。");

            return false;
        }


        return true;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (!CheckInput())
        {
            return;
        }
        CustomerInfo info = GetInfo();

        UserInfo loginUser = Common.GetLoginUser();

        try
        {
            if (id > 0)
            {

                if (!Common.CheckAuth(LoginUser, Role_Function.Administrator)
                && !(LoginUser.CompanyCode == info.CompanyCode && Common.CheckAuth(LoginUser, Role_Function.CustomerEdit) || info.CompanyCode.StartsWith(LoginUser.CompanyCode) && info.RefSeller.ID == 0
                    || this.LoginUser.ID == info.RefSeller.ID)
                )
                {
                    MessageBox.Show(Setting.NoPermissionMsg, this.PrevURL);
                    return;
                }

                biz.Update(info);
            }
            else
            {
                biz.Add(info);
            }

            MessageBox.Show("success~", this.PrevURL);
        }
        catch (BusinessException ex)
        {
            MessageBox.Show(ex.Message);
        }

        catch (Exception ex)
        {
            MessageBox.Show("保持失败。");
            Logger.Log.Error("update customer Exception.", ex);
        }
    }
    protected void btnAddMemo_Click(object sender, EventArgs e)
    {
        UserInfo loginUser = Common.GetLoginUser();

        if (!Common.CheckAuth(loginUser, Role_Function.CustomerEdit)
&& loginUser.ID != Convert.ToInt32(this.ddlSeller.SelectedItem.Value))
        {
            MessageBox.Show(Setting.NoPermissionMsg, this.PrevURL);
            return;
        }

        if (this.txtMemo.Text.Trim().Length <= 0)
        {
            MessageBox.Show("备注不能为空~");

            return;
        }
        CustomerMemoInfo memo = GetMemo();

        biz.AddMemo(memo);
        MessageBox.Show("success~", this.PrevURL);

    }
    private CustomerMemoInfo GetMemo()
    {
        CustomerMemoInfo memo = new CustomerMemoInfo();
        memo.CustomerID = id;
        memo.Memo = this.txtMemo.Text.Trim();
        memo.InDate = DateTime.Now;
        memo.InUser = Common.GetLoginUser().UserName;

        return memo;
    }
    protected void anPager_PageChanged(object sender, EventArgs e)
    {
        BindData_Memo();
    }
    protected void rblState_SelectedIndexChanged(object sender, EventArgs e)
    {
        reqiredIDCardNumber.Visible = this.rblState.SelectedValue == "Open";
        reqiredAddress.Visible = this.rblState.SelectedValue == "New";
    }
}