using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class CompanyList : PageBase
{
    CompanyBiz biz = new CompanyBiz();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.Administrator))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        if (!IsPostBack)
        {
            InitControls();
            BindData();
        }
    }

    private void BindData()
    {
        QueryFilter filter = new QueryFilter();
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = this.anPager.StartRecordIndex;
        filter.Pager.pageSize = this.anPager.PageSize;


        DataTable dt = biz.GetData(filter);
        this.ListView1.DataSource = dt;
        this.ListView1.DataBind();

        this.anPager.RecordCount = filter.Pager.recordCount;
    }

    private void InitControls()
    {
        List<string> AuthList = Common.GetLoginUser().AuthList;

        if (!AuthList.Contains(Role_Function.Administrator)
    )
        {
            this.btnAdd.Visible = false;
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
        this.anPager.CurrentPageIndex = 1;

        BindData();
    }

    private bool CheckInput()
    {
        return true;
    }
}