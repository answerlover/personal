using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wandou4;

public partial class TransactionEdit : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.CheckAuth(Role_Function.TransactionEdit))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }

        this.PrevURL = "TradeTransactionList.aspx";

        if (!IsPostBack)
        {
            InitControls();
        }
    }

    private void InitControls()
    {
        this.dpTradeDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        UserInfo loginUser = Common.GetLoginUser();

        if (!Common.CheckAuth(loginUser, Role_Function.TransactionEdit))
        {
            MessageBox.Show(Setting.NoPermissionMsg);
            return;
        }
        if (!CheckInput())
        {
            return;
        }

        if (fileUploadTrade.HasFile)
        {
            string filepath = Setting.SettlementUpfilePath + loginUser.ID + "_" + System.IO.Path.GetFileNameWithoutExtension(fileUploadTrade.FileName)
                + System.IO.Path.GetExtension(fileUploadTrade.FileName);
            try
            {
                if (!System.IO.Directory.Exists(Setting.SettlementUpfilePath))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(Setting.SettlementUpfilePath));
                }
                fileUploadTrade.SaveAs(Server.MapPath(filepath));

                TradeBiz biz = new TradeBiz();
                DataTable dt = ExcelHelper.GetExcelDataByOleDb(Server.MapPath(filepath));
                List<TradeTransactionInfo> list = biz.TransferToEntityList(dt, DateTime.Parse(this.dpTradeDate.Text));
                list.ForEach(item => item.CompanyCode = Common.GetLoginUser().CompanyCode);

                biz.Add(list);

                MessageBox.Show("success~", this.PrevURL);

            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败。");
                Logger.Log.Error("import excel Exception.", ex);
            }
        }
    }

    private bool CheckInput()
    {
        DateTime temp = DateTime.MinValue;
        if (this.dpTradeDate.Text.Trim().Length <= 0)
        {
            MessageBox.Show("结算日期必填。");

            return false;
        }

        if (!DateTime.TryParse(this.dpTradeDate.Text.Trim(), out temp))
        {
            MessageBox.Show("结算日期输入有误。");

            return false;
        }

        return true;
    }

}