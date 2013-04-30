using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CustomerDLL 的摘要说明
/// </summary>
public class CustomerBiz
{
    CustomerDal dal = new CustomerDal();

    public DataTable GetData(QueryFilter filter)
    {
        return dal.GetData(filter);
    }

    public CustomerInfo GetInfo(int id)
    {
        return dal.GetInfo(id);
    }

    public int Add(CustomerInfo info)
    {
        if (dal.IsExists(info))
        {
            throw new BusinessException("该客户交易商代码或者手机号码已经存在。");
        }

        return dal.Add(info);
    }
    public int Update(CustomerInfo info)
    {
        if (dal.IsExists(info))
        {
            throw new BusinessException("该客户交易商代码或者手机号码已经存在。");
        }
        return dal.Update(info);
    }

    public int AddMemo(CustomerMemoInfo memo)
    {
        return dal.AddMemo(memo);
    }
    public DataTable GetMemo(QueryFilter filter)
    {
        return dal.GetMemo(filter);
    }
}