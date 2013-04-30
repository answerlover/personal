using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Class1 的摘要说明
/// </summary>
public partial class Global : System.Web.HttpApplication
{
    void Application_Start(object sender, EventArgs e)
    {
        // 在应用程序启动时运行的代码

    }

    void Application_End(object sender, EventArgs e)
    {
        //  在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        if (ex is HttpRequestValidationException)
        {
            Response.Write("输入了非法字符【<a href=\"javascript:history.back(0);\">返回</a>】");
        }
        else
        {
            Logger.Log.Error("Application_Error", ex);
            // Response.Redirect("~/505.htm");
        }
        //Server.ClearError();

    }

    void Session_Start(object sender, EventArgs e)
    {


    }

   
    public override void Init()
    {
        base.Init(); 
    }
    
    void Session_End(object sender, EventArgs e)
    {
        // 在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
        // 或 SQLServer，则不引发该事件。

    }


}