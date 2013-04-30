using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;

/// <summary>
/// ExcelHelper 的摘要说明
/// </summary>
public class ExcelHelper
{

    public static DataTable TransferToDataTable(string fileName)
    {
        Excel.Application excel = null;
        Excel.Workbooks wbs = null;
        Excel.Workbook wb = null;
        Excel.Worksheet ws = null;
        Excel.Range range1 = null;

        DataTable dt = new DataTable();

        try
        {
            excel = new Excel.Application();
            excel.UserControl = true;
            excel.DisplayAlerts = false;

            excel.Application.Workbooks.Open(fileName);

            wbs = excel.Workbooks;
            wb = wbs[1];
            ws = (Excel.Worksheet)wb.Worksheets[1];


            int rowCount = ws.UsedRange.Rows.Count;
            int colCount = ws.UsedRange.Columns.Count;
            if (rowCount <= 0)
            {
                throw new Exception("文件中没有数据记录");
            }
            if (colCount <= 0)
            {
                throw new Exception("字段个数不对");
            }

            for (int i = 1; i <= rowCount; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 1; j <= colCount; j++)
                {
                    object cellVal = ws.get_Range(ws.Cells[i, j], ws.Cells[i, j]).Value;

                    if (i == 1)
                    {
                        dt.Columns.Add(new DataColumn(cellVal.ToString()));
                    }
                    else
                    {
                        dr[j - 1] = cellVal;
                    }
                }
                if (i > 1)
                {
                    dt.Rows.Add(dr);
                }
            }

        }
        finally
        {
            if (excel != null)
            {
                if (wbs != null)
                {
                    if (wb != null)
                    {
                        if (ws != null)
                        {
                            if (range1 != null)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                                range1 = null;
                            }
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                            ws = null;
                        }
                        wb.Close(false);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                        wb = null;
                    }
                    wbs.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wbs);
                    wbs = null;
                }
                excel.Application.Workbooks.Close();
                excel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
                GC.Collect();
            }
        }

        return dt;
    }

    public static DataTable GetExcelDataByOleDb(string fileName)
    {
        DataTable dt = new DataTable();
        DataTable dtSchema = null;
        string tableName = "";

        string sql = "";
        OleDbConnection conn = new OleDbConnection();
        conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + "; Extended Properties='Excel 8.0;HDR=no;IMEX=0'";
        try
        {//打开连接
            conn.Open();
            dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if(dtSchema.Rows.Count <=0)
            {
                return dt;
            }
            tableName = dtSchema.Rows[0]["TABLE_NAME"].ToString();
            sql = "select * from [" + tableName + "]";

            OleDbCommand olecommand = new OleDbCommand(sql, conn);
            OleDbDataAdapter da = new OleDbDataAdapter(olecommand);
            da.Fill(dt);
        }
        finally
        {
            conn.Close();//关闭数据库
            conn.Dispose();
        }

        return dt;
    }


}