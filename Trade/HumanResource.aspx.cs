using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class UserList : PageBase
{
    UserBiz biz = new UserBiz();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.UserEdit))
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
        UserInfo loginUser = Common.GetLoginUser();

        QueryFilter filter = new QueryFilter();
        filter.Pager = new PagerInfo();
        filter.Pager.startRowIndex = 1;
        filter.Pager.pageSize = int.MaxValue;

        if (!Common.CheckAuth(loginUser, Role_Function.Administrator))
        {
            filter.CompanyCode = Common.GetLoginUser().CompanyCode;
            filter.NodePath = loginUser.NodePath;
        }

        List<UserInfo> list = biz.GetList(filter);

        TreeNode root = new TreeNode("人力资源", "0");
        foreach (UserInfo uInfo in list)
        {
            TreeNode tn = new TreeNode(uInfo.UserName, uInfo.NodePath);
            Regex regex = new Regex("(,\\w+?)$");

            TreeNode parent = FindTreeNodeByPath(root, regex.Replace(uInfo.NodePath, ""));

            if (parent != null)
            {
                parent.ChildNodes.Add(tn);
            }
            else
            {
                root.ChildNodes.Add(tn);
            }
        }

        this.TreeView1.Nodes.Add(root);
    }

    private TreeNode FindTreeNodeByPath(TreeNode root, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        if (root.Value == path)
        {
            return root;
        }

        foreach (TreeNode tn in root.ChildNodes)
        {
            TreeNode findNd = FindTreeNodeByPath(tn, path);
            if (findNd != null)
            {
                return findNd;
            }
        }

        return null;
    }

}