using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SCG.eAccounting.Web.Helper
{
    public class ReportHelper
    {
        public static void FlushReport(Page outputPage, byte[] reportOutput, ReportType rptType)
        {
            if (rptType.Equals(ReportType.PDF))
                FlushReportPDF(outputPage, reportOutput, "Report");
            
        }

        public static void FlushReport(Page outputPage, byte[] reportOutput, ReportType rptType , string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                FlushReportPDF(outputPage, reportOutput, "Report");
            }
            else
            {
                FlushReportPDF(outputPage, reportOutput, fileName);
            }
        }
        public static void FlushReportEXCEL(Page outputPage, byte[] reportOutput, string fileName)
        {
            try
            {
                if (reportOutput != null)
                {
                    MemoryStream m = new MemoryStream(reportOutput);
                    outputPage.Response.Clear();
                    outputPage.Response.ClearHeaders();
                    outputPage.Response.AddHeader("Content-Length", reportOutput.Length.ToString());
                    outputPage.Response.ContentType = "application/xls";
                    outputPage.Response.AddHeader("Expires", "0");
                    outputPage.Response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                    outputPage.Response.AddHeader("Pragma", "public");
                    outputPage.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
                    outputPage.Response.BinaryWrite(reportOutput);
                    outputPage.Response.Flush();
                    outputPage.Response.End();
                }
                else
                {
                    outputPage.Response.Write("Report Error.");
                }
            }
            catch (Exception e)
            {
                outputPage.Response.Write(e.Message);
            }
        }

        public static void FlushReportPDF(Page outputPage, byte[] reportOutput, string fileName)
        {
            try
            {
                if (reportOutput != null)
                {
                    MemoryStream m = new MemoryStream(reportOutput);
                    outputPage.Response.Clear();
                    outputPage.Response.ClearHeaders();
                    outputPage.Response.AddHeader("Content-Length", reportOutput.Length.ToString());
                    outputPage.Response.ContentType = "application/pdf";
                    outputPage.Response.AddHeader("Expires", "0");
                    outputPage.Response.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                    outputPage.Response.AddHeader("Pragma", "public");
                    outputPage.Response.AddHeader("Content-Disposition", "attachment; filename="+ fileName +".pdf");
                    outputPage.Response.BinaryWrite(reportOutput);
                    outputPage.Response.Flush();
                    outputPage.Response.End();
                }
                else
                {
                    outputPage.Response.Write("Report Error.");
                }
            }
            catch (Exception e)
            {
                outputPage.Response.Write(e.Message);
            }
        }

        public enum ReportType
        {
            PDF,
            EXCEL,
            TIF,
            CVS
        }
    }
}
