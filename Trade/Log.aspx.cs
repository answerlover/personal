using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class Log : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.Administrator))
        {
            MessageBox.Show(Setting.NoPermissionMsg);

            Response.End();
            return;
        }

        using (StreamReader sr = new StreamReader(Server.MapPath(Setting.ErrorLogPath), Encoding.UTF8))
        {
            while (!sr.EndOfStream)
            {
                Response.Write(sr.ReadLine() + "<br/>");
            }
            Response.End();
        }

    }
}