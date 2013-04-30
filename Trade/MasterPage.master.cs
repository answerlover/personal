using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_MasterPage : System.Web.UI.MasterPage
{
    public UserInfo loginUser = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        loginUser = Common.GetLoginUser();

        if (loginUser == null)
        {
            Response.Redirect("~/Login.aspx");
        }

        if (!IsPostBack)
        {
            if (!loginUser.AuthList.Contains(Role_Function.UserEdit))
            {
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    if (TreeView1.Nodes[i].Value == "UserManage")
                    {
                        TreeView1.Nodes.RemoveAt(i);
                    }
                }
            }
            if (!loginUser.AuthList.Contains(Role_Function.Administrator))
            {
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    if (TreeView1.Nodes[i].Value == "CompanyManage")
                    {
                        TreeView1.Nodes.RemoveAt(i);
                    }
                }
            }  
            if (!loginUser.AuthList.Contains(Role_Function.Manager))
            {
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    if (TreeView1.Nodes[i].Value == "AuthManage")
                    {
                        TreeView1.Nodes.RemoveAt(i);
                    }
                }
            }
        }


    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Common.Logout();

        Response.Redirect("Login.aspx");
    }
}
