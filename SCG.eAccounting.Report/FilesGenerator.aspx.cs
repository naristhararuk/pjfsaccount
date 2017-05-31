using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Web.Services.Protocols;
using System.Collections.Generic;


namespace SCG.eAccounting.Report
{
    public partial class FilesGenerator : System.Web.UI.Page
    {
        private string ReportUserName = string.Empty;
        private string ReportPassword = string.Empty;
        private string ReportDomain = string.Empty;
        public FilesGenerator()
        {
            
            //ReportUserName = System.Configuration.ConfigurationManager.AppSettings["ReportUserName"].ToString();
            //ReportPassword = System.Configuration.ConfigurationManager.AppSettings["ReportPassword"].ToString();
            //ReportDomain = System.Configuration.ConfigurationManager.AppSettings["ReportDomain"].ToString();
        
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Byte[] GetByte(string ReportServerUrl, string ReportUserName, string ReportPassword, string ReportDomain, string ReportPath, string ReportName, List<ReportParameter> paramter, ExportType exportType)
        {
            // Prepare Render arguments
            string historyID = null;
            string deviceInfo = null;
            string format = exportType.ToString();
            Byte[] results;
            string encoding = String.Empty;
            string mimeType = String.Empty;
            string extension = String.Empty;
            ReportExecution2005.Warning[] warnings = null;
            string[] streamIDs = null;
            ReportExecution2005.ParameterValue[] rptParameters = null;

            try
            {
                ReportExecution2005.ReportExecutionService myReport = new ReportExecution2005.ReportExecutionService();
                myReport.Url = ReportServerUrl;
                if (ReportDomain != string.Empty && ReportDomain.Length > 0)
                {
                    myReport.Credentials = new System.Net.NetworkCredential(ReportUserName, ReportPassword, ReportDomain);
                }
                else
                {
                    myReport.Credentials = new System.Net.NetworkCredential(ReportUserName,ReportPassword);
                }
                
                ReportExecution2005.ExecutionInfo ei;
                if (ReportPath != null && ReportPath.Length > 0)
                {
                    ei = myReport.LoadReport("/" + ReportPath + "/" + ReportName, historyID);
                }
                else
                {
                    if (ReportName != null && ReportName.Length > 0)
                    {
                        ei = myReport.LoadReport("/" + ReportName, historyID);
                    }
                    else
                    {
                        return new Byte[0];
                    }
                }

                /* This code for set credentials on runtime
                require [using SCG.eAccounting.Report.ReportExecution2005;]
                if (ei.CredentialsRequired)
                {
                    List<DataSourceCredentials> credentials = new List<DataSourceCredentials>();
                    foreach (DataSourcePrompt dsp in ei.DataSourcePrompts)
                    {
                        DataSourceCredentials cred = new DataSourceCredentials();
                        cred.DataSourceName = dsp.Name;
                        cred.UserName = [Database Username];
                        cred.Password = [Database Password];
                        credentials.Add(cred);
                    }

                    Console.WriteLine("Setting data source credentials...");
                    ei = myReport.SetExecutionCredentials(credentials.ToArray());
                }
                */

                if (paramter != null && paramter.Count > 0)
                {

                    rptParameters = new ReportExecution2005.ParameterValue[paramter.Count];
                    for (int i = 0; i < paramter.Count; i++)
                    {
                        rptParameters[i] = new ReportExecution2005.ParameterValue();
                        rptParameters[i].Name = paramter[i].ParamterName;
                        rptParameters[i].Value = paramter[i].ParamterValue;

                    }
                }


                deviceInfo = "<DeviceInfo>" + "<SimplePageHeaders>True</SimplePageHeaders>" + "</DeviceInfo>";
                //render the PDF
                myReport.SetExecutionParameters(rptParameters, "th-TH");
                results = myReport.Render(format, deviceInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);
            }
            catch (Exception e)
            {
                throw e;
            }
            return results;
        }

        public enum ExportType
        { 
            PDF,
            EXCEL,
            TIF,
            CVS
        }
    }
}
