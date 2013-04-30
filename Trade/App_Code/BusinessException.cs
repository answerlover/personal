using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// BusinessException 的摘要说明
/// </summary>
public class BusinessException : Exception
{

    public BusinessException(string message)
        : base(message)
    {
    }

}