using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Report;
using Spring.Validation;
using SCG.eAccounting.Web.Helper;
using System.Globalization;
using SS.DB.Query;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;



namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class SummaryExpenseReport : BasePage
    {
        int docTypeID;


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                this.SetRequestType();

                if (Request.Cookies[ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID] != null)
                {
                    ctlDropDownDocType.SelectedValue = Server.HtmlEncode(Request.Cookies[ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID]["docTypeID"]);
                    ctlFromDocDate.DateValue = Server.HtmlEncode(Request.Cookies[ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID]["FromDocDate"]);
                    ctlToDocDate.DateValue = Server.HtmlEncode(Request.Cookies[ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID]["ToDocDate"]);
                    txtCompanyList.Text = Server.HtmlEncode(Request.Cookies[ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID]["Company"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["exportType"]))
                {
                    if (Request.QueryString["exportType"].Equals("excel"))
                    {
                        this.ExportSummaryExpenseReportExcel();
                    }
                }
                UpdatePanelSummaryExpenseReport.Update();
            }
        }

        private void SetRequestType()
        {
            IList<TranslatedListItem> documentTypeList = ScgeAccountingQueryProvider.SCGDocumentQuery.FindDocumentTypeCriteriaNoTA(); 

            var items = from documentTypes in documentTypeList
                        select new TranslatedListItem() { ID = documentTypes.ID, Symbol = GetMessage(string.Concat("DT_", documentTypes.Symbol)) };

            ctlDropDownDocType.DataSource = items.ToList<TranslatedListItem>();
            ctlDropDownDocType.DataTextField = "Symbol";
            ctlDropDownDocType.DataValueField = "ID";
            ctlDropDownDocType.DataBind();

            ctlDropDownDocType.Items.Insert(0, new ListItem(GetMessage("All_Item"),"0"));
        }

        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ctlCompanyTextboxAutoComplete.CompanyCode))
            {
                if (string.IsNullOrEmpty(txtCompanyList.Text))
                {
                    txtCompanyList.Text += ctlCompanyTextboxAutoComplete.CompanyCode;
                }
                else
                {
                    txtCompanyList.Text += "," + ctlCompanyTextboxAutoComplete.CompanyCode;
                }
            }
            ctlCompanyTextboxAutoComplete.ResetValue();
            UpdatePanelSummaryExpenseReport.Update();
        }

        protected void ctlButtonExport_Click(object sender, EventArgs e)
        {
            Request.Cookies.Remove(ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID);
            HttpCookie SummaryExpenseCookie = Request.Cookies[ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID];
            if (SummaryExpenseCookie == null)
            {
                SummaryExpenseCookie = new HttpCookie(ApplicationMode + "SummaryExpenseCookie_" + UserAccount.UserID);
            }


            SummaryExpenseCookie.Values["docTypeID"] = ctlDropDownDocType.SelectedValue;
            SummaryExpenseCookie.Values["FromDocDate"] = ctlFromDocDate.DateValue;
            SummaryExpenseCookie.Values["ToDocDate"] = ctlToDocDate.DateValue;  
            SummaryExpenseCookie.Values["Company"] = txtCompanyList.Text;
            SummaryExpenseCookie.Expires = DateTime.Now.AddDays(62);
            Response.Cookies.Add(SummaryExpenseCookie);
            if (string.IsNullOrEmpty(ctlFromDocDate.Text))
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Please Select Document From Date."));
            }
            else if (string.IsNullOrEmpty(ctlToDocDate.Text))
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Please Select Document To Date."));
            }
            else if (string.IsNullOrEmpty(ctlLabelCompany.Text))
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Please Select Company."));
            }
            else
            {
                Response.Redirect("SummaryExpenseReport.aspx?exportType=excel");
            }

        }

        private void ExportSummaryExpenseReportExcel()
        {
            List<ReportParameter> rptParam = new List<ReportParameter>();

            ReportParameter paramCompanyCode = new ReportParameter();
            paramCompanyCode.ParamterName = "CompanyCode";
            paramCompanyCode.ParamterValue = txtCompanyList.Text;
            rptParam.Add(paramCompanyCode);

            ReportParameter paramdocTypeID = new ReportParameter();
            paramdocTypeID.ParamterName = "docTypeID";
            paramdocTypeID.ParamterValue = ctlDropDownDocType.SelectedValue.ToString();
            rptParam.Add(paramdocTypeID);

            ReportParameter paramFromDocDate = new ReportParameter();
            paramFromDocDate.ParamterName = "FromDocDate";
            paramFromDocDate.ParamterValue = ctlFromDocDate.DateValue;
            rptParam.Add(paramFromDocDate);

            ReportParameter paramToDocDate = new ReportParameter();
            paramToDocDate.ParamterName = "ToDocDate";
            paramToDocDate.ParamterValue = ctlToDocDate.DateValue;
            rptParam.Add(paramToDocDate);

            // find report file name later
            byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "SummaryExpenseReport", rptParam, FilesGenerator.ExportType.EXCEL);
            ReportHelper.FlushReportEXCEL(this.Page, results, "SummeryExpenseReport_"+ DateTime.Today.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
        }

    }
}