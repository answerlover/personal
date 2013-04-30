using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;

/// <summary>
/// UserBiz 的摘要说明
/// </summary>
public class UserBiz
{
    UserDal dal = new UserDal();

    public UserBiz() { }

    public List<UserInfo> GetList(QueryFilter filter)
    {
        return dal.GetList(filter);
    }

    public List<UserInfo> GetUserTreeList(QueryFilter filter)
    {
        List<UserInfo> userlist = this.GetList(filter);
        userlist.Sort((u1, u2) => u1.NodePath.CompareTo(u2.NodePath));
        userlist.ForEach(u => {
            int level = u.NodePath.Count(c => c == ',');
            u.UserName = "".PadLeft(level > 0 ? level - 1 : 0, '-').Replace("-", "--") + " " + u.UserName;        
        });

        return userlist;
    }

    public UserInfo GetInfo(string UserName, string PassWord)
    {
        return dal.GetInfo(UserName, PassWord);
    }

    public UserInfo GetInfo(int ID)
    {
        return dal.GetInfo(ID);
    }

    public int Add(UserInfo info)
    {
        if (dal.IsExists(info))
        {
            throw new BusinessException("该用户已存在。");
        }
        int userid;
        using (TransactionScope ts = new TransactionScope())
        {
            userid = dal.Add(info);

            dal.UpdateUserNode(userid, info.PID);

            ts.Complete();
        }

        return userid;
    }
    public int Update(UserInfo info)
    {
        if (dal.IsExists(info))
        {
            throw new BusinessException("该用户已存在。");
        }
        using (TransactionScope ts = new TransactionScope())
        {
            dal.Update(info);

            ts.Complete();
        }

        return 1;
    }

    public void Delete(int userID)
    {
        using (TransactionScope ts = new TransactionScope())
        {
            if (dal.HasChildUser(userID))
            {
                throw new BusinessException("请先删除下属或者将下属分配给其它公司领导再执行此操作。");
            }
            dal.Delete(userID);

            ts.Complete();
        }
      
    }

    public int UpdateAll(UserInfo info, bool changePWD)
    {
        using (TransactionScope ts = new TransactionScope())
        {
            if (changePWD)
            {
                this.Update(info);
            }

            this.UpdateUserCompany(info.ID, info.CompanyCode);
            this.UpdateUserRole(info.ID, info.Role.RoleID);
            dal.UpdateUserNode(info.ID, info.PID);

            ts.Complete();
        }
        return 1;
    }


    public DataTable GetRoleData()
    {
        return dal.GetRoleData();
    }
    public DataTable GetCompanyData()
    {
        return dal.GetCompanyData();
    }
    public void UpdateUserRole(int UserID, int RoleID)
    {
        dal.UpdateUserRole(UserID, RoleID);
    }
    public int UpdateUserCompany(int UserID, string CompanyCode)
    {
        return dal.UpdateUserCompany(UserID, CompanyCode);
    }

    private string GetNewSubUserNodePath(string nodePath, int nodeValue)
    {
        return nodePath + "," + nodeValue.ToString().PadLeft(4, '0');
    }


}