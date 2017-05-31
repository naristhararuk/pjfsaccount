using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.DB.Query;
using Microsoft.Reporting.WebForms;
using System.IO;
using SS.Standard.CommunicationService.DTO;
using SS.Standard.CommunicationService.Implement;
using System.Text;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ReportViewers : BaseReportViewers
    {
        //public ISCGEmailService SCGEmailService { get; set; }
        protected override void OnInit(EventArgs e)
        {
            //ctlMasterReportViewer.EnableSendEmail = this.EnableSendEmail;
            // ctlMasterReportViewer.OnClickButtonReturn += new SSReportViewer.ObjectOnClickButtonReturnEventHandler(ctlMasterReportViewer_OnClickButtonReturn);

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

       

        public void InitializeReport()
        {
            setAuthentication();

            //if (this.ReportFolderPath == null || this.ReportFolderPath.Length == 0)
            //{
                this.ReportFolderPath = ParameterServices.ReportFolderPath;
            //}

            if (this.ReportName == null || this.ReportName.Length == 0)
            {
                throw new Exception("Need report name");
            }

            if (this.ReportHeight == 0)
            {
                this.ReportHeight = 1024;
                this.SetLayoutHeightType = LayoutHeightType.Pixel;
            }


            if (this.ReportWidth == 0)
            {
                this.ReportWidth = 100;
                this.SetLayoutWidthType = LayoutWidthType.Percentage;
            }

            if (this.ReportViewerServerUrl == null)
            {
                this.ReportViewerServerUrl = new Uri(ParameterServices.ReportingViewersURL);
            }


            if (this.ReportFolderPath != null && this.ReportFolderPath.Length > 0)
            {
                ctlMasterReportViewer.ServerReport.ReportPath = "/" + this.ReportFolderPath + "/" + this.ReportName;
            }
            else
            {
                ctlMasterReportViewer.ServerReport.ReportPath = "/" + this.ReportName;
            }


            ctlMasterReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;


            if (this.SetLayoutWidthType == LayoutWidthType.Percentage)
            {
                ctlMasterReportViewer.Width = Unit.Percentage(ReportWidth);
            }

            if (this.SetLayoutHeightType == LayoutHeightType.Percentage)
            {
                ctlMasterReportViewer.Height = Unit.Percentage(ReportHeight);
            }

            if (this.SetLayoutWidthType == LayoutWidthType.Point)
            {
                ctlMasterReportViewer.Width = Unit.Point(ReportWidth);
            }

            if (this.SetLayoutHeightType == LayoutHeightType.Point)
            {
                ctlMasterReportViewer.Height = Unit.Point(ReportHeight);
            }


            if (this.SetLayoutWidthType == LayoutWidthType.Pixel)
            {
                ctlMasterReportViewer.Width = Unit.Pixel(ReportWidth);
            }


            if (this.SetLayoutHeightType == LayoutHeightType.Pixel)
            {
                ctlMasterReportViewer.Height = Unit.Pixel(ReportHeight);
            }


            if (this.ReportViewerServerUrl != null)
            {
                ctlMasterReportViewer.ServerReport.ReportServerUrl = this.ReportViewerServerUrl;
            }



            if (this.ReportViewerServerUrl != null)
            {
                ctlMasterReportViewer.ShowBackButton = true;
                ctlMasterReportViewer.ShowExportControls = true;
                ctlMasterReportViewer.ShowRefreshButton = true;
                ctlMasterReportViewer.SizeToReportContent = true;
                ctlMasterReportViewer.ShowReportBody = true;
                ctlMasterReportViewer.ShowToolBar = true;
                ctlMasterReportViewer.ShowZoomControl = true;
                ctlMasterReportViewer.ShowPromptAreaButton = true;
                ctlMasterReportViewer.ShowPrintButton = true;
                if (HideParameterOnForm)
                {
                    ctlMasterReportViewer.ShowParameterPrompts = false;

                }
                else
                {
                    ctlMasterReportViewer.ShowParameterPrompts = true;


                }
                ctlMasterReportViewer.ShowPageNavigationControls = true;
                ctlMasterReportViewer.ShowFindControls = true;
                ctlMasterReportViewer.ShowDocumentMapButton = true;
            }

        }

       
      
      
        private void DeleteFiles(string tempFiles)
        {
            if (File.Exists(tempFiles))
            {
                File.Delete(tempFiles);
            }
        }
        private string saveRptAs()
        {
            string FilesType = GenerateFileType.ToString();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            // string deviceInfo;

            byte[] bytes = ctlMasterReportViewer.ServerReport.Render(
            FilesType, null, out mimeType, out encoding, out extension,
            out streamids, out warnings);
            string tempFiles = "~/AttachFiles";
            string filesName = "";
            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(tempFiles)))
            {
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(tempFiles));
            }
            string currentDate = "_"+DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString().PadLeft(2,'0')+DateTime.Now.Day.ToString().PadLeft(2,'0')+DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString();
            string path = "";
            if (FilesType.Equals("Excel"))
            {
               path= tempFiles + "/" + HttpContext.Current.Session.SessionID + currentDate + ".xls";
            }
            else
            {
                path = tempFiles + "/" + HttpContext.Current.Session.SessionID + currentDate + ".pdf";

            }

            filesName = System.Web.HttpContext.Current.Server.MapPath(path);
            FileStream stream = File.OpenWrite(filesName);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
           

            return filesName;
            //Response.Buffer = true;
            //Response.Clear();
            //Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", "attachment; filename=sample."+extension);
            //Response.BinaryWrite(bytes);
            //Response.Flush();

            }


        public void DoCancel(object sender, EventArgs e)
        {
          //  ctlSendEmailPopupExtender.Hide();

        }
        public void DoClose(object sender, EventArgs e)
        {
           // ctlSendEmailPopupExtender.Hide();
        }
        public void ShowReport()
        {
            //if (this.EnableSendEmail)
            //{
            //    this.ctlSendEmail.Visible = true;
            //}
            //else
            //{
            //    this.ctlSendEmail.Visible = false;
            //}
            if (this.Parameters != null)
            {
                Microsoft.Reporting.WebForms.ReportParameter[] paramsReport = new Microsoft.Reporting.WebForms.ReportParameter[this.Parameters.Count];
                for (int i = 0; i < this.Parameters.Count; i++)
                {

                    paramsReport[i] = new Microsoft.Reporting.WebForms.ReportParameter(this.Parameters[i].Name, this.Parameters[i].Value);
                    if (this.Parameters[i].Value == null)
                    {
                        //Response.Write(this.Parameters[i].Name + "<br>");
                        continue;
                    }
                    

                }
                ctlMasterReportViewer.ServerReport.SetParameters(paramsReport);
            }

            ctlMasterReportViewer.ServerReport.Refresh();

            ctlMasterReportViewer.ShowToolBar = true;
            //ctlMasterReportViewer.ShowParameterPrompts = true;

           
            ctlMasterReportViewer.Visible = true;
        }

        private void setAuthentication()
        {
            IReportServerCredentials irsc = new CustomReportCredentials(ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName);
            ctlMasterReportViewer.ServerReport.ReportServerCredentials = irsc;

        }

        private IList<ReportParameters> GetReportCriteriaParameters()
        {
            IList<ReportParameters> returnParameters = new List<ReportParameters>();

            ReportParameterInfoCollection reportParameters = ctlMasterReportViewer.ServerReport.GetParameters();
            foreach (ReportParameterInfo reportParameter in reportParameters)
            {
                if (!reportParameter.Name.StartsWith("Display"))
                    returnParameters.Add(new ReportParameters() { Name = reportParameter.Name });
            }
            return returnParameters;
        }

        public void SetReportParameters(object reportCriteria)
        {
            IList<ReportParameters> reportCriteriaParameters = GetReportCriteriaParameters();
            foreach (ReportParameters reportCriteriaParameter in reportCriteriaParameters)
            {
                System.Reflection.PropertyInfo property = reportCriteria.GetType().GetProperty(reportCriteriaParameter.Name);
                if (property == null) continue;
                if (property.GetValue(reportCriteria, null) == null) continue;

                reportCriteriaParameter.Value = property.GetValue(reportCriteria, null).ToString();
            }

            this.Parameters = new List<ReportParameters>();
            this.Parameters.AddRange(reportCriteriaParameters);
        }

       
        
    }
}


