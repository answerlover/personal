using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace wandou4
{
    /// <summary>
    /// aspx页面消息弹出框类。   
    /// 注意：若有多个提示，因为方法里的key都为"msg"，所以只显示第一个。
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// 显示一个弹出窗口
        /// </summary>
        /// <param name="message"></param>
        public static void Show(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("alert(\"" + ToScriptMsg(message) + "\"); \n");
            RegScript(sb.ToString());
        }

        /// <summary>
        /// 显示一个弹出窗口，并转向目标页(导航)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url"></param>
        public static void Show(string message, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("alert(\"" + ToScriptMsg(message) + "\"); \n");
            sb.Append("window.location.href=\"" + url.Trim() + "\";\n");

            RegScript(sb.ToString());
        }

        private static void RegScript(string script)
        {
            Page page = HttpContext.Current.Handler as Page;
            page.ClientScript.RegisterStartupScript(typeof(Page), "msg", script, true);

        }

        private static string ToScriptMsg(string message)
        {
            message = message ?? string.Empty;
            return message.Trim().Replace("'", "‘").Replace("\"", "“").Replace(";","；");
        }
    }
}
