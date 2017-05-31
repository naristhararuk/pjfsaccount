using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.IO;
using SCG.eAccounting.BLL;
using SS.Standard.Utilities;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.Report;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using System.Globalization;
using Spring.Validation;

namespace SCG.eAccounting.Web.Forms.Interface.Programs
{
    public partial class ExportPayroll : BasePage
    {
        public IFnExpenseInvoiceService FnExpenseInvoiceService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Cookies[ApplicationMode+"payrollCookie_" + UserAccount.UserID] != null)
                {
                    ctlDropDownMonth.SelectedValue = Server.HtmlEncode(Request.Cookies[ApplicationMode + "payrollCookie_" + UserAccount.UserID]["month"]);
                    ctlTextboxYear.Text = Server.HtmlEncode(Request.Cookies[ApplicationMode + "payrollCookie_" + UserAccount.UserID]["year"]);
                    txtCompanyList.Text = Server.HtmlEncode(Request.Cookies[ApplicationMode + "payrollCookie_" + UserAccount.UserID]["CompanyList"]);
                    rblPersonalLevel.SelectedValue = Server.HtmlEncode(Request.Cookies[ApplicationMode + "payrollCookie_" + UserAccount.UserID]["PersonalLevel"]);
                }
                else
                {
                    rblPersonalLevel.SelectedIndex = 2;
                    SelectPreviouseMonthAndYear();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["exportType"]))
                {
                    if (Request.QueryString["exportType"].Equals("text"))
                    {
                        this.ExportPayrollTextFile();
            }
                    else if (Request.QueryString["exportType"].Equals("pdf"))
                    {
                        this.ExportPayrollPDF();
        }
                }
                UpdatePanelPayrollCriteria.Update();
            }
        }

        private void SelectPreviouseMonthAndYear()
        {
            ctlTextboxYear.Text = DateTime.Now.Year.ToString();
            if ((DateTime.Now.Month - 1) != 0)
                ctlDropDownMonth.SelectedValue = (DateTime.Now.Month - 1).ToString();
            else
            {
                ctlDropDownMonth.SelectedValue = 12.ToString();
            }
        }

        protected void ctlButtonExport_Click(object sender, EventArgs e)
        {
            Request.Cookies.Remove(ApplicationMode + "payrollCookie_" + UserAccount.UserID);
            HttpCookie payrollCookie = Request.Cookies[ApplicationMode + "payrollCookie_" + UserAccount.UserID];
            if (payrollCookie == null)
            {
                payrollCookie = new HttpCookie(ApplicationMode + "payrollCookie_" + UserAccount.UserID);
            }

            payrollCookie.Values["month"] = ctlDropDownMonth.SelectedValue;
            payrollCookie.Values["year"] = ctlTextboxYear.Text;
            payrollCookie.Values["CompanyList"] = txtCompanyList.Text;
            payrollCookie.Values["PersonalLevel"] = rblPersonalLevel.SelectedValue;
            payrollCookie.Expires = DateTime.Now.AddDays(62);
            Response.Cookies.Add(payrollCookie);
            if (ctlTextboxYear.Text.Length != 4)
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Invalid Year."));
            }
            else if (string.IsNullOrEmpty(txtCompanyList.Text))
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Company is required."));
            }
            else
            {
                Response.Redirect("ExportPayroll.aspx?exportType=text");
            }
        }
        protected void ctlPrint_Click(object sender, EventArgs e)
                {
                    Request.Cookies.Remove(ApplicationMode + "payrollCookie_" + UserAccount.UserID);
                    HttpCookie payrollCookie = Request.Cookies[ApplicationMode + "payrollCookie_" + UserAccount.UserID];
            if (payrollCookie == null)
                    {
                        payrollCookie = new HttpCookie(ApplicationMode + "payrollCookie_" + UserAccount.UserID);
                    }

            payrollCookie.Values["month"] = ctlDropDownMonth.SelectedValue;
            payrollCookie.Values["year"] = ctlTextboxYear.Text;
            payrollCookie.Values["CompanyList"] = txtCompanyList.Text;
            payrollCookie.Values["PersonalLevel"] = rblPersonalLevel.SelectedValue;
            payrollCookie.Expires = DateTime.Now.AddDays(62);
            Response.Cookies.Add(payrollCookie);
            if (ctlTextboxYear.Text.Length != 4)
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Invalid Year."));
            }
            else if (string.IsNullOrEmpty(txtCompanyList.Text))
            {
                ValidationErrors.AddError("Export.Error", new ErrorMessage("Company is required."));
            }
            else
            {
                Response.Redirect("ExportPayroll.aspx?exportType=pdf");
            }
        }

        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ctlCompanyTextboxAutoComplete1.CompanyCode))
            {
                if (string.IsNullOrEmpty(txtCompanyList.Text))
                {
                    txtCompanyList.Text += ctlCompanyTextboxAutoComplete1.CompanyCode;
                }
                else
                {
                    txtCompanyList.Text += "," + ctlCompanyTextboxAutoComplete1.CompanyCode;
                }
            }
            ctlCompanyTextboxAutoComplete1.ResetValue();
            UpdatePanelPayrollCriteria.Update();
        }

        private void ExportPayrollPDF()
        {
                int month = int.Parse(ctlDropDownMonth.SelectedValue);
                int year = int.Parse(ctlTextboxYear.Text);
                //Create datetime use to used to parameter values.
                DateTime dt = new DateTime(year, month, 1);
                DateTime endDate = dt.AddDays(14);
                DateTime startDate = dt.AddMonths(-1).AddDays(15);

                List<ReportParameter> rptParam = new List<ReportParameter>();

                ReportParameter paramCompanyCode = new ReportParameter();
                paramCompanyCode.ParamterName = "CompanyCode";
                paramCompanyCode.ParamterValue = txtCompanyList.Text.Equals(string.Empty) ? string.Empty : txtCompanyList.Text;
                rptParam.Add(paramCompanyCode);

                ReportParameter PersonalLevel = new ReportParameter();
                PersonalLevel.ParamterName = "Ordinal";
                PersonalLevel.ParamterValue = rblPersonalLevel.SelectedValue.ToString();
                rptParam.Add(PersonalLevel);

                ReportParameter startdate = new ReportParameter();
                startdate.ParamterName = "startdate";
                startdate.ParamterValue = startDate.ToString("MM/dd/yyyy", new CultureInfo("en-US"));
                rptParam.Add(startdate);


                ReportParameter enddate = new ReportParameter();
                enddate.ParamterName = "enddate";
                enddate.ParamterValue = endDate.ToString("MM/dd/yyyy", new CultureInfo("en-US"));
                rptParam.Add(enddate);


                // find report file name later
                byte[] results = new FilesGenerator().GetByte(ParameterServices.ReportingURL, ParameterServices.ReportUserName, ParameterServices.ReportPassword, ParameterServices.ReportDomainName, ParameterServices.ReportFolderPath, "PayrollReport", rptParam, FilesGenerator.ExportType.PDF);
                ReportHelper.FlushReportPDF(this.Page, results, "PayrollReport_0" + ctlDropDownMonth.SelectedValue + ctlTextboxYear.Text);
            }

        private void ExportPayrollTextFile()
        {
            try
            {
                string content = FnExpenseInvoiceService.ExportFilePayroll(ctlDropDownMonth.Text,
                                    ctlTextboxYear.Text, txtCompanyList.Text, rblPersonalLevel.SelectedValue);
                string month = "";
                month = ctlDropDownMonth.SelectedValue;
                if (month.Length == 1)
            {
                    month = "0" + month;
            }
                string filename = "Payroll" +
                    ctlCompanyTextboxAutoComplete1.CompanyCode +
                    ctlTextboxYear.Text +
                    month +
                    ".txt";
                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/octet-stream";
                Response.Write(content);
                Response.End();

        }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
    }
        }
    }

}
