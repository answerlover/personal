using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class UserList : PageBase
{
    UserBiz biz = new UserBiz();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(this.LoginUser,Role_Function.UserEdit))
        {
            MessageBox.Show(Setting.NoPermissionMsg);

            return;
        }

        if (!IsPostBack)
        {
            BindData();

            InitControls();
        }
    }

    private void BindData()
    {
        QueryFilter filter = new QueryFilter();
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = this.anPager.StartRecordIndex;
        filter.Pager.pageSize = this.anPager.PageSize;

        if (!Common.CheckAuth(this.LoginUser, Role_Function.Administrator))
        {
            filter.CompanyCode = Common.GetLoginUser().CompanyCode;
            filter.NodePath = LoginUser.NodePath;
        }

        List<UserInfo> userlist = biz.GetList(filter);

        userlist.RemoveAll(u => u.ID == this.LoginUser.ID);


        this.ListView1.DataSource = userlist;
        this.ListView1.DataBind();

        this.anPager.RecordCount = filter.Pager.recordCount;
    }

    private void InitControls()
    {
        List<string> AuthList = Common.GetLoginUser().AuthList;

        if (!AuthList.Contains(Role_Function.UserEdit)
    && !AuthList.Contains(Role_Function.Administrator)
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
        if (e.CommandName == "delete")
        {
            if (!Common.CheckAuth(this.LoginUser, Role_Function.Manager))
            {
                MessageBox.Show(Setting.NoPermissionMsg);
                return;
            }

            try
            {
                int userID = int.Parse(e.CommandArgument.ToString());
                biz.Delete(userID);

                MessageBox.Show("删除成功。");
                BindData();
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败。");
                Logger.Log.Error("delete user Exception.", ex);
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!CheckInput())
        {
            return;
        }

        string queryString = "";

        ViewState["strWhere"] = queryString;

        BindData();
    }

    private bool CheckInput()
    {
        return true;
    }

    protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ListViewDataItem currentItem = (ListViewDataItem)e.Item;
            LinkButton lbtnDelete = (LinkButton)currentItem.FindControl("lbtnDelete");
            if (this.LoginUser != null && Common.CheckAuth(this.LoginUser, Role_Function.Manager))
            {
                lbtnDelete.Visible = true;
            }
            else
            {
                lbtnDelete.Visible = false;
            }
        }
    }
}