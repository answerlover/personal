using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CompanyDLL 的摘要说明
/// </summary>
public class CompanyBiz
{
    CompanyDal dal = new CompanyDal();

    public DataTable GetData(QueryFilter filter)
    {
        return dal.GetData(filter);
    }

    public CompanyInfo GetInfo(int id)
    {
        return dal.GetInfo(id);
    }

    public int Add(CompanyInfo info)
    {
        if (dal.IsExists(info))
        {
            throw new BusinessException("该公司已经存在。");
        }

        return dal.Add(info);
    }
    public int Update(CompanyInfo info)
    {
        if (dal.IsExists(info))
        {
            throw new BusinessException("该公司已经存在。");
        }
        return dal.Update(info);
    }
    
}