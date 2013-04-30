using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomerList : PageBase
{
    CustomerBiz biz = new CustomerBiz();
    public bool showEditButton = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        showEditButton = Common.CheckAuth(Role_Function.CustomerEdit);


        if (!IsPostBack)
        {
            InitControls();
            GetQueryFilter();
            BindData();
        }
    }


    private void BindData()
    {
        QueryFilter filter = ViewState["strWhere"] as QueryFilter;
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = this.anPager.StartRecordIndex;
        filter.Pager.pageSize = this.anPager.PageSize;
        filter.Pager.recordCount = 0;

        DataTable dt = biz.GetData(filter);
        this.ListView_Customer.DataSource = dt;
        this.ListView_Customer.DataBind();

        this.anPager.RecordCount = filter.Pager.recordCount;
    }

    private void InitControls()
    {
        QueryFilter filter = new QueryFilter();
        if (!Common.CheckAuth(this.LoginUser, Role_Function.Administrator))
        {
            filter.CompanyCode = LoginUser.CompanyCode;
        }

        if (!Common.CheckAuth(this.LoginUser, Role_Function.Manager))
        {
            filter.NodePath = LoginUser.NodePath;

        }
        List<UserInfo> users = new UserBiz().GetUserTreeList(filter);
        users.Insert(0, new UserInfo() { ID = -1, UserName = "--全部--" });
        users.Insert(1, new UserInfo() { ID = 0, UserName = "--公海--" });


        //if (!Common.CheckAuth(Role_Function.Manager))
        //{
        //    users = users.Where(u => u.ID == LoginUser.ID || u.UserName == "--公海--").ToList();
        //}

        ddlSeller.DataSource = users;
        ddlSeller.DataBind();

        if (!Common.CheckAuth(Role_Function.Manager))
        {
            ddlSeller.SelectedValue = LoginUser.ID.ToString();
        }

        if (Request.QueryString["type"] == "noseller")
        {
            ddlSeller.SelectedValue = "0";
        }

    }

    protected void anPager_PageChanged(object sender, EventArgs e)
    {
        BindData();
    }
    protected void ListView_Customer_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!CheckInput())
        {
            return;
        }
        GetQueryFilter();
        this.anPager.CurrentPageIndex = 1;

        BindData();
    }

    private void GetQueryFilter()
    {
        QueryFilter filter = new QueryFilter();
        if (!Common.CheckAuth(this.LoginUser, Role_Function.Administrator))
        {
            filter.CompanyCode = LoginUser.CompanyCode;
        }

        if (!Common.CheckAuth(this.LoginUser, Role_Function.Manager))
        {
            filter.NodePath = LoginUser.NodePath;

        }

        filter.SellerID = int.Parse(ddlSeller.SelectedValue);

        if (this.txtTraderCode_Begin.Text.Trim().Length > 0)
        {
            filter.TraderCode_Begin = this.txtTraderCode_Begin.Text.Trim();
        }
        if (this.txtTraderCode_End.Text.Trim().Length > 0)
        {
            filter.TraderCode_End = this.txtTraderCode_End.Text.Trim();
        }

        if (txtTraderName.Text.Trim().Length > 0)
        {
            filter.TraderName = txtTraderName.Text.Trim();
        }
        filter.State = this.rblState.SelectedValue;


        ViewState["strWhere"] = filter;
    }


    private bool CheckInput()
    {
        return true;
    }
}