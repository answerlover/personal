using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

/// <summary>
///Logger 的摘要说明
/// </summary>
public class Logger
{

    private static log4net.ILog log = null;

    private static object lockHelper = new object();

    public static log4net.ILog Log
    {
        get
        {
            if (log == null)
            {
                lock (lockHelper)
                {
                    if (log == null)
                    {
                        log = log4net.LogManager.GetLogger("logger");
                    }
                }

            }

            return log;
        }

    }

    public static void Test()
    {
        log4net.ILog log = Log;
        log.Error("4343", new Exception("test"));
        log.Debug("hello");
    }
}