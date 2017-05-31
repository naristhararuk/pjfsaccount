using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.Query;
using SCG.eAccounting.DAL;
using SCG.DB.DTO;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL.Implement;
using SS.DB.DTO;
using SS.DB.Query;
using SCG.eAccounting.BLL;
using System.Text;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Reports
{
    public partial class ReimbursementReport : BasePage
    {
        public ISCGDocumentService SCGDocumentService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlPbDropdownList.DataSource = ScgDbQueryProvider.DbPbRateQuery.GetPbList(GetPBCriteria(), null);
                ctlPbDropdownList.DataValueField = "Pbid";
                ctlPbDropdownList.DataTextField = "PbCode";
                ctlPbDropdownList.DataBind();

                //if (!string.IsNullOrEmpty(Request.QueryString["pbId"]))
                //{
                //    this.GeneratePDF();
                //}
                ctlUpdatePanelMaintainRateGrid.Update();
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                ctlUpdatePanelMaintainRateGrid.Update();
            }
        }

        protected void ctlReport_DataBound(object sender, EventArgs e)
        {
            Dbpb dbPb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(Convert.ToInt64(ctlPbDropdownList.SelectedValue));
            DbCurrency dbCurrency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(Convert.ToInt16(dbPb.MainCurrencyID));
            ctlReport.Columns[6].HeaderText = string.Format(GetProgramMessage("ctlMainCurrency"), "(" + dbCurrency.Symbol + ")");
        }

        protected int RequestCount()
        {
            ReimbursementReportValueObj criteria = GetReportCriteria();
            int count = ScgeAccountingQueryProvider.SCGDocumentQuery.CountReportByCriteria(criteria);
            return count;
        }
        protected object RequestData(int startRow, int pageSize, string sortExpression)
        {
            ReimbursementReportValueObj criteria = GetReportCriteria();
            return ScgeAccountingQueryProvider.SCGDocumentQuery.GetReportList(criteria, startRow, pageSize, sortExpression);
        }
        protected void ctlReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlAmount = e.Row.FindControl("ctlAmount") as Label;
                Label ctlMainCurrency = e.Row.FindControl("ctlMainCurrency") as Label;
                Label ctlAmountTHB = e.Row.FindControl("ctlAmountTHB") as Label;

                ReimbursementReportValueObj data = e.Row.DataItem as ReimbursementReportValueObj;
                ctlAmount.ForeColor = data.Amount < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Gray;
                ctlMainCurrency.ForeColor = data.AmountMainCurrency < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Gray;
                ctlAmountTHB.ForeColor = data.AmountTHB < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Gray;
            }
        }
        protected void cltSearchBtn_Click(object sender, ImageClickEventArgs e)
        {
            ctlReport.DataCountAndBind();
            ctlReport.DataBind();
            ctlUpdatePanelMaintainRateGrid.Update();

            SetMarkStateBeforeUpdate();
        }
        public ReimbursementReportValueObj GetReportCriteria()
        {
            ReimbursementReportValueObj report = new ReimbursementReportValueObj();
            report.PBID = Convert.ToInt64(ctlPbDropdownList.SelectedValue);
            //report.PBID = 98;
            report.RequestDateFrom = ctlRequestDateFromPicker.Value;
            report.RequestDateTo = ctlRequestDateToPicker.Value;
            report.PaidDateFrom = ctlPaidDateFromPicker.Value;
            report.PaidDateTo = ctlPaidDateToPicker.Value;
            report.RequestNoFrom = ctlRequestNoFromTextbox.Text;
            report.RequestNoTo = ctlRequestNoToTextbox.Text;
            if (ctlMarkedDocument.Checked && !ctlUnmarkDocument.Checked)
            {
                report.MarkDocument = true;
            }
            else if (!ctlMarkedDocument.Checked && ctlUnmarkDocument.Checked)
            {
                report.MarkDocument = false;
            }
            else
            {
                report.MarkDocument = null;
            }

            string key = ApplicationMode +"reimbursementCriteriaCookie_" + UserAccount.UserID;
            Request.Cookies.Remove(key);
            HttpCookie reimburstCriteriaCookie = Request.Cookies[key];
            if (reimburstCriteriaCookie == null)
            {
                reimburstCriteriaCookie = new HttpCookie(key);
            }

            reimburstCriteriaCookie.Values["PBID"] = report.PBID.ToString();
            reimburstCriteriaCookie.Values["RequestDateFrom"] = report.RequestDateFrom.ToString();
            reimburstCriteriaCookie.Values["RequestDateTo"] = report.RequestDateTo.ToString();
            reimburstCriteriaCookie.Values["PaidDateFrom"] = report.PaidDateFrom.ToString();
            reimburstCriteriaCookie.Values["PaidDateTo"] = report.PaidDateTo.ToString();
            reimburstCriteriaCookie.Values["RequestNoFrom"] = report.RequestNoFrom.ToString();
            reimburstCriteriaCookie.Values["RequestNoTo"] = report.RequestNoTo.ToString();
            reimburstCriteriaCookie.Values["MarkDocument"] = report.MarkDocument.ToString();
            Response.Cookies.Add(reimburstCriteriaCookie);
            return report;
        }
        public VOPB GetPBCriteria()
        {
            VOPB pb = new VOPB();
            pb.LanguageID = UserAccount.CurrentLanguageID;
            pb.UserID = UserAccount.UserID;
            return pb;
        }
        protected void ctlUpdateMarkBtn_Click(object sender, EventArgs e)
        {
            string key = ApplicationMode + "reimbursementCookie_" + UserAccount.UserID;
            Request.Cookies.Remove(key);
            HttpCookie reimburstCookie = Request.Cookies[key];
            if (reimburstCookie == null)
            {
                reimburstCookie = new HttpCookie(key);
            }

            IList<ReimbursementReportValueObj> obj = new List<ReimbursementReportValueObj>();
            string markList = string.Empty;
            string unMarkList = string.Empty;
            string pb = string.Empty;
            VOPB voPb = ScgDbQueryProvider.DbPBQuery.GetDescription(Convert.ToInt64(ctlPbDropdownList.SelectedValue), 2);
            pb = voPb.PBCode + " " + voPb.Description;

            if (Session["MarkStateBeforeUpdate"] != null)
            {
                IList<ReimbursementReportValueObj> markStateList = (IList<ReimbursementReportValueObj>)Session["MarkStateBeforeUpdate"];
                Session.Remove("MarkStateBeforeUpdate");
                int rowNo = 0;

                foreach (GridViewRow item in ctlReport.Rows)
                {
                    if (item.RowType == DataControlRowType.DataRow)
                    {
                        ReimbursementReportValueObj valueObj = new ReimbursementReportValueObj();
                        HiddenField docNo = (HiddenField)ctlReport.Rows[item.RowIndex].FindControl("ctlDocumentID");
                        CheckBox mark = (CheckBox)ctlReport.Rows[item.RowIndex].FindControl("ctlMark");
                        Literal requestNo = (Literal)ctlReport.Rows[item.RowIndex].FindControl("ctlRequestNo");
                        valueObj.DocumentID = Convert.ToInt64(docNo.Value);
                        valueObj.Mark = mark.Checked;
                        if (mark.Checked)
                        {
                            if (!markStateList[rowNo].Mark)
                            {
                                if (!string.IsNullOrEmpty(markList))
                                {
                                    markList += ",";
                                }
                                markList += requestNo.Text;
                            }
                        }
                        else
                        {
                            if (markStateList[rowNo].Mark)
                            {
                                if (!string.IsNullOrEmpty(unMarkList))
                                {
                                    unMarkList += ",";
                                }
                                unMarkList += requestNo.Text;
                            }
                        }
                        obj.Add(valueObj);
                        rowNo++;
                    }
                }
                reimburstCookie.Values["markList"] = markList;
                reimburstCookie.Values["unmarkList"] = unMarkList;
                Response.Cookies.Add(reimburstCookie);
                SCGDocumentService.UpdateMarkDocument(obj);
                ctlReport.DataCountAndBind();
                ctlUpdatePanelMaintainRateGrid.Update();

                SetMarkStateBeforeUpdate();

                StringBuilder scriptBuilder = new StringBuilder("window.open('ReimbursementReportOutput.aspx?");
                scriptBuilder.AppendFormat("pbId={0}", ctlPbDropdownList.SelectedValue);
                scriptBuilder.AppendFormat("&companyId={0}", voPb.CompanyID.Value);
                scriptBuilder.Append("') ;");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, scriptBuilder.ToString(), true);
            }
        }
        protected void ctlPrintAllBtn_Click(object sender, EventArgs e)
        {
            string key = ApplicationMode + "reimbursementCookie_" + UserAccount.UserID;
            Request.Cookies.Remove(key);
            HttpCookie reimburstCookie = Request.Cookies[key];
            if (reimburstCookie == null)
            {
                reimburstCookie = new HttpCookie(key);
            }

            string markList = string.Empty;
            string unMarkList = string.Empty;
            string pb = string.Empty;
            VOPB voPb = ScgDbQueryProvider.DbPBQuery.GetDescription(Convert.ToInt64(ctlPbDropdownList.SelectedValue), 2);
            pb = voPb.PBCode + " " + voPb.Description;
            foreach (GridViewRow item in ctlReport.Rows)
            {
                if (item.RowType == DataControlRowType.DataRow)
                {
                    HiddenField docNo = (HiddenField)ctlReport.Rows[item.RowIndex].FindControl("ctlDocumentID");
                    CheckBox mark = (CheckBox)ctlReport.Rows[item.RowIndex].FindControl("ctlMark");
                    Literal requestNo = (Literal)ctlReport.Rows[item.RowIndex].FindControl("ctlRequestNo");
                    if (!string.IsNullOrEmpty(markList))
                    {
                        markList += ",";
                    }
                    markList += requestNo.Text;

                }
            }
            reimburstCookie.Values["markList"] = markList;
            reimburstCookie.Values["unmarkList"] = unMarkList;
            Response.Cookies.Add(reimburstCookie);

            StringBuilder scriptBuilder = new StringBuilder("window.open('ReimbursementReportOutput.aspx?");
            scriptBuilder.AppendFormat("pbId={0}", ctlPbDropdownList.SelectedValue);
            scriptBuilder.AppendFormat("&companyId={0}", voPb.CompanyID.Value);
            scriptBuilder.Append("') ;");
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), string.Empty, scriptBuilder.ToString(), true);
        }

        private void SetMarkStateBeforeUpdate()
        {
            if (Session["MarkStateBeforeUpdate"] != null)
                Session.Remove("MarkStateBeforeUpdate");

            Session["MarkStateBeforeUpdate"] = (IList<ReimbursementReportValueObj>)ctlReport.DataSource;
        }
    }
}