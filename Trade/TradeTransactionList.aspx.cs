using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class TradeTransactionList : PageBase
{
    TradeBiz biz = new TradeBiz();

    protected bool IsShowDetailFee = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        IsShowDetailFee = Common.CheckAuth(LoginUser, Role_Function.TransactionEdit);
        if (!IsPostBack)
        {
            InitControls();

            GetQueryFilter();
            BindData();
        }
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
        if (Common.CheckAuth(LoginUser, Role_Function.TransactionEdit))
        {
            users.Insert(1, new UserInfo() { ID = 0, UserName = "--公海--" });
        }

        ddlSeller.DataSource = users;
        ddlSeller.DataBind();

        if (!Common.CheckAuth(LoginUser, Role_Function.TransactionEdit))
        {
            this.btnAdd.Visible = false;
            this.btnDelete.Visible = false;

            ddlSeller.SelectedValue = LoginUser.ID.ToString();
            //ddlSeller.Enabled = false;


        }

        ViewPageChange();

    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {

        QueryFilter filter = ViewState["strWhere"] as QueryFilter;
        if (filter == null)
        {
            filter = new QueryFilter();
        }
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = this.anPager.StartRecordIndex;
        filter.Pager.pageSize = this.anPager.PageSize;
        filter.Pager.recordCount = 0;

        List<TradeTransactionInfo> list = biz.GetList(filter);

        this.ListView1.DataSource = list;
        this.ListView1.DataBind();

        this.anPager.RecordCount = filter.Pager.recordCount;
    }

    private void BindData_Summary()
    {
        QueryFilter filter = ViewState["strWhere"] as QueryFilter;
        if (filter == null)
        {
            filter = new QueryFilter();
        }
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = 1;
        filter.Pager.pageSize = int.MaxValue;
        filter.Pager.recordCount = 0;


        DataTable dt = biz.GetSummaryData(filter);
        this.ListView_Summary.DataSource = dt;
        this.ListView_Summary.DataBind();
    }

    protected void anPager_PageChanged(object sender, EventArgs e)
    {
        BindData();
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
            filter.CompanyCode = Common.GetLoginUser().CompanyCode;
            filter.NodePath = LoginUser.NodePath;
        }
        if (this.dpTradeDate_Begin.Text.Trim().Length > 0)
        {
            filter.SettlementDate_Begin = DateTime.Parse(this.dpTradeDate_Begin.Text.Trim());
        }
        if (this.dpTradeDate_End.Text.Trim().Length > 0)
        {
            filter.SettlementDate_End = DateTime.Parse(this.dpTradeDate_End.Text.Trim());

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

        ViewState["strWhere"] = filter;
    }

    private bool CheckInput()
    {
        DateTime pTradeDate_Begin = DateTime.MinValue;
        if (this.dpTradeDate_Begin.Text.Trim().Length > 0 && !DateTime.TryParse(this.dpTradeDate_Begin.Text.Trim(), out pTradeDate_Begin))
        {
            MessageBox.Show("结算日期输入有误。");
            return false;
        }

        DateTime pTradeDate_End = DateTime.MinValue;

        if (this.dpTradeDate_End.Text.Trim().Length > 0 && !DateTime.TryParse(this.dpTradeDate_End.Text.Trim(), out pTradeDate_End))
        {
            MessageBox.Show("结算日期输入有误。");
            return false;
        }

        if (this.dpTradeDate_Begin.Text.Trim().Length > 0 && this.dpTradeDate_End.Text.Trim().Length > 0
           && pTradeDate_Begin > pTradeDate_End
            )
        {
            MessageBox.Show("结算日期起不能大于结算日期止。");
            return false;
        }

        return true;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

        if (!Common.CheckAuth(Role_Function.TransactionEdit))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        List<int> idlist = new List<int>();
        // if()
        foreach (ListViewDataItem item in ListView1.Items)
        {
            CheckBox cbkRow = item.FindControl("cbkRow") as CheckBox;
            if (cbkRow.Checked)
            {
                idlist.Add((int)ListView1.DataKeys[item.DataItemIndex].Value);
            }
        }

        if (idlist.Count <= 0)
        {
            MessageBox.Show("请选择需要删除的记录。");
            return;
        }

        try
        {
            biz.Delete(idlist);

            MessageBox.Show("删除成功。~");
            BindData();
        }
        catch (BusinessException ex)
        {
            MessageBox.Show(ex.Message);
        }

        catch (Exception ex)
        {
            MessageBox.Show("删除失败。");
            Logger.Log.Error("TradeTransaction delete Exception.", ex);
        }

    }
    protected void rblViewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewPageChange();
    }

    private void ViewPageChange()
    {
        if (this.rblViewType.SelectedIndex == 0)
        {
            this.pnl1.Visible = true;
            this.pnl2.Visible = !this.pnl1.Visible;
        }
        else
        {
            this.pnl1.Visible = false;
            this.pnl2.Visible = !this.pnl1.Visible;
        }

        if (this.rblViewType.SelectedIndex == 0)
        {
            BindData_Summary();
        }
    }

}