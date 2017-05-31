using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using System.Collections.Generic;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.BLL;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using SCG.eAccounting.BLL.WorkFlowService;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.Security;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.Utilities;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Query;
using SCG.eAccounting.SAP.Service;
using log4net;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.ViewPost
{
    public partial class ViewPostTest : BaseUserControl
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(ViewPost));

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewPostEditDate1.OnPopUpReturn += new PopUpReturnEventHandler(ViewPostEditDate1_OnPopUpReturn);
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region <-- Desing Variable -->
        public ISCGDocumentService SCGDocumentService { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }

        public IRemittanceWorkFlowService RemittanceWorkFlowService { get; set; }
        public IAdvanceWorkFlowService AdvanceWorkFlowService { get; set; }
        public IAdvanceWorkFlowService AdvanceForeignWorkFlowService { get; set; }
        public IExpenseWorkFlowService ExpenseWorkFlowService { get; set; }

        public IUserAccount UserAccount { get; set; }
        #endregion <-- Desing Variable -->

        #region <-- PROPERTY -->

        #region public string DocKind
        public string DocKind
        {
            get
            {
                if (ViewState["DocKind"] == null)
                    return "";
                else
                    return ViewState["DocKind"].ToString();
            }
            set
            {
                ViewState["DocKind"] = value;
            }
        }
        #endregion public DocumentKind DocKind

        #region public long DocID
        public long DocID
        {
            get
            {
                if (ViewState["DocID"] == null)
                    return 0;
                else
                    return (long)ViewState["DocID"];
            }
            set
            {
                ViewState["DocID"] = value;
            }
        }
        #endregion public long DocID

        #region private bool ChangeState
        private bool ChangeState
        {
            get
            {
                if (ViewState["ChangeState"] == null)
                    return false;
                else
                    return (bool)ViewState["ChangeState"];
            }

            set
            {
                ViewState["ChangeState"] = value;
            }
        }
        #endregion private bool ChangeState

        #endregion <-- PROPERTY -->

        #region <-- FUNCTION -->

        #region private void Alert(string Message)
        private void Alert(string Message)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DisableDropdown", "alert('" + Message + "');", true);
        }
        #endregion private void Alert(string Message)

        #region private bool CanSimulateOrPostOrReverse()
        private bool CanSimulateOrPostOrReverse()
        {
            return true;

            //SS.Standard.WorkFlow.DTO.WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(this.DocID);
            //bool boolReturn = false;

            //if (workflow == null)
            //{
            //    this.Alert("Don't Found WorkflowID of this DocumentID !!!");
            //}
            //else if (workflow.CurrentState.Name.ToString() != "WaitVerify")
            //{
            //    boolReturn = false;
            //}
            //else if (this.DocKind == DocumentKind.Advance.ToString())
            //{
            //    AvAdvanceDocument avDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(this.DocID);
            //    if (avDoc.AdvanceType == "DM")
            //        boolReturn = AdvanceWorkFlowService.CanVerifyWaitVerify(workflow.WorkFlowID);
            //    else
            //        boolReturn = AdvanceForeignWorkFlowService.CanVerifyWaitVerify(workflow.WorkFlowID);
            //}
            //else if (this.DocKind == DocumentKind.Remittance.ToString())
            //{
            //    boolReturn = RemittanceWorkFlowService.CanReceiveWaitRemittance(workflow.WorkFlowID);
            //}
            //else if (this.DocKind == DocumentKind.Expense.ToString())
            //{
            //    boolReturn = ExpenseWorkFlowService.CanVerifyWaitVerify(workflow.WorkFlowID);
            //}
            //else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
            //{
            //    boolReturn = ExpenseWorkFlowService.CanPayWaitRemittance(workflow.WorkFlowID);
            //}
            //return boolReturn;
        }
        #endregion private bool CanSimulateOrPostOrReverse()

        #region private bool CanApprove()
        private bool CanApprove()
        {
            return true;

            //SS.Standard.WorkFlow.DTO.WorkFlow workflow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(this.DocID);
            //bool boolReturn = false;

            //if (workflow == null)
            //{
            //    this.Alert("Don't Found WorkflowID of this DocumentID !!!");
            //}
            //else if (this.DocKind == DocumentKind.Advance.ToString())
            //{
            //    AvAdvanceDocument avDoc = ScgeAccountingQueryProvider.AvAdvanceDocumentQuery.GetAvAdvanceByDocumentID(this.DocID);
            //    if (avDoc.AdvanceType == "DM")
            //        boolReturn = AdvanceWorkFlowService.CanApproveWaitApproveVerify(workflow.WorkFlowID);
            //    else
            //        boolReturn = AdvanceForeignWorkFlowService.CanApproveWaitApproveVerify(workflow.WorkFlowID);
            //}
            //else if (this.DocKind == DocumentKind.Remittance.ToString())
            //{
            //    boolReturn = RemittanceWorkFlowService.CanReceiveWaitRemittance(workflow.WorkFlowID);
            //}
            //else if (this.DocKind == DocumentKind.Expense.ToString())
            //{
            //    boolReturn = ExpenseWorkFlowService.CanApproveWaitApproveVerify(workflow.WorkFlowID);
            //}
            //else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
            //{
            //    boolReturn = ExpenseWorkFlowService.CanPayWaitRemittance(workflow.WorkFlowID);
            //}
            //return boolReturn;
        }
        #endregion private bool CanApprove()

        #region private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()
        private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()
        {
            SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService PostService;
            if (this.DocKind == DocumentKind.Advance.ToString())
                PostService = new AdvancePostingService();
            else if (this.DocKind == DocumentKind.Remittance.ToString())
                PostService = new RemittancePostingService();
            else if (this.DocKind == DocumentKind.Expense.ToString())
                PostService = new ExpensePostingService();
            else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                PostService = new ExpenseRemittancePostingService();
            else
                PostService = new AdvancePostingService();

            return PostService;
        }
        #endregion private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()

        #region private string GetDocumentPostingStatus()
        private string GetDocumentPostingStatus()
        {
            if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                return docExpense.RemittancePostingStatus;
            }
            else
            {
                SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                return doc.PostingStatus;
            }
        }
        #endregion private string GetDocumentPostingStatus()

        #region private void ShowData()
        private void ShowData()
        {
            #region Query Data
            Hashtable paramete = new Hashtable();
            paramete.Add("@DOCUMENT_ID", DocID.ToString());
            paramete.Add("@DOCUMENT_KIND", DocKind.ToString());

            DataSet dstViewPost = new DBManage().ExecuteQuery("VIEW_POSTING", paramete);
            DataTable dtbHeadM = dstViewPost.Tables[0];
            DataTable dtbHead = dstViewPost.Tables[1];
            DataTable dtbItemM = dstViewPost.Tables[2];
            DataTable dtbItem = dstViewPost.Tables[3];

            // Edit by kookkla
            // For IC Case
            DataTable dtbHeadB2C = dstViewPost.Tables[4];
            DataTable dtbItemB2C = dstViewPost.Tables[5];
            #endregion Query Data

            #region DataTable Temp
            DataTable dtbShow = new DataTable();
            dtbShow.Columns.Add("FIELD01");
            dtbShow.Columns.Add("FIELD02");
            dtbShow.Columns.Add("FIELD03");
            dtbShow.Columns.Add("FIELD04");
            dtbShow.Columns.Add("ExRate");
            dtbShow.Columns.Add("AmountTHB");
            dtbShow.Columns.Add("FIELD05");
            dtbShow.Columns.Add("FIELD06");
            dtbShow.Columns.Add("FIELD07");
            dtbShow.Columns.Add("FIELD08");
            dtbShow.Columns.Add("FIELD09");
            dtbShow.Columns.Add("FIELD10");
            dtbShow.Columns.Add("FIELD11");
            dtbShow.Columns.Add("FIELD12");
            dtbShow.Columns.Add("FIELD13");
            dtbShow.Columns.Add("FIELD14");
            dtbShow.Columns.Add("FIELD15");
            dtbShow.Columns.Add("FIELD16");
            dtbShow.Columns.Add("FIELD17");
            dtbShow.Columns.Add("HEADERTXT");
            

            DataTable dtbShowB2C = new DataTable();
            dtbShowB2C.Columns.Add("FIELD01");
            dtbShowB2C.Columns.Add("FIELD02");
            dtbShowB2C.Columns.Add("FIELD03");
            dtbShowB2C.Columns.Add("FIELD04");
            dtbShowB2C.Columns.Add("FIELD05");
            dtbShowB2C.Columns.Add("FIELD06");
            dtbShowB2C.Columns.Add("FIELD07");
            dtbShowB2C.Columns.Add("FIELD08");
            dtbShowB2C.Columns.Add("FIELD09");
            dtbShowB2C.Columns.Add("FIELD10");
            dtbShowB2C.Columns.Add("FIELD11");
            dtbShowB2C.Columns.Add("FIELD12");
            dtbShowB2C.Columns.Add("FIELD13");
            dtbShowB2C.Columns.Add("FIELD14");
            dtbShowB2C.Columns.Add("FIELD15");
            dtbShowB2C.Columns.Add("FIELD16");
            dtbShowB2C.Columns.Add("FIELD17");
            dtbShowB2C.Columns.Add("HEADERTXT");
            #endregion DataTable Temp

            #region For Head Main
            if (dtbHeadM.Rows.Count > 0)
            {
                lblComCode.Text = dtbHeadM.Rows[0]["COMP_CODE"].ToString();
                lblComName.Text = dtbHeadM.Rows[0]["CompanyName"].ToString();

                DataRow dr = dtbShow.NewRow();
                dr.BeginEdit();
                dr["FIELD01"] = dtbHeadM.Rows[0]["FI_DOC"].ToString();
                dr["FIELD02"] = dtbHeadM.Rows[0]["DOC_DATE"].ToString();
                dr["FIELD03"] = dtbHeadM.Rows[0]["POST_DATE"].ToString();
                dr["FIELD04"] = dtbHeadM.Rows[0]["DOC_TYPE"].ToString();
                dr["FIELD05"] = dtbHeadM.Rows[0]["COMP_CODE"].ToString();
                dr["FIELD06"] = dtbHeadM.Rows[0]["PERIOD"].ToString();
                dr["FIELD07"] = dtbHeadM.Rows[0]["YEAR"].ToString();
                dr["FIELD08"] = dtbHeadM.Rows[0]["BRNCH"].ToString();
                dr["FIELD09"] = dtbHeadM.Rows[0]["Currency"].ToString();
                dr["FIELD10"] = dtbHeadM.Rows[0]["REF_DOC_NO"].ToString();
                dr["HEADERTXT"] = dtbHeadM.Rows[0]["HEADERTXT"].ToString();
                dr.EndEdit();

                dtbShow.Rows.Add(dr);

                for (int i = 0; i < dtbItemM.Rows.Count; i++)
                {
                    DataRow dr1 = dtbShow.NewRow();
                    dr1.BeginEdit();
                    dr1["HEADERTXT"] = dtbItemM.Rows[i]["PK"].ToString();
                    dr1["FIELD10"] = dtbItemM.Rows[i]["Account"].ToString();
                    dr1["FIELD02"] = dtbItemM.Rows[i]["AMT_DOCCUR"].ToString();
                    dr1["FIELD03"] = dtbItemM.Rows[i]["TAX_CODE"].ToString();
                    dr1["FIELD04"] = dtbItemM.Rows[i]["BaseDate"].ToString();
                    dr1["FIELD05"] = dtbItemM.Rows[i]["PMNTTRMS"].ToString();
                    dr1["FIELD06"] = dtbItemM.Rows[i]["PYMT_METH"].ToString();
                    //dr1["FIELD09"] = dtbItemM.Rows[i]["CURRENCY"].ToString();
                    dr1["FIELD09"] = dtbItemM.Rows[i]["CostCenter"].ToString();
                    dr1["FIELD11"] = dtbItemM.Rows[i]["InterOrder"].ToString();
                    dr1["FIELD12"] = dtbItemM.Rows[i]["WHTCode1"].ToString();
                    dr1["FIELD13"] = dtbItemM.Rows[i]["WHTBase1"].ToString();
                    dr1["FIELD14"] = dtbItemM.Rows[i]["WHTCode2"].ToString();
                    dr1["FIELD15"] = dtbItemM.Rows[i]["WHTBase2"].ToString();
                    dr1["FIELD16"] = dtbItemM.Rows[i]["ALLOC_NMBR"].ToString();
                    dr1["FIELD17"] = dtbItemM.Rows[i]["ITEM_TEXT"].ToString();
                    dr1["ExRate"] = UIHelper.BindExchangeRate(dtbItemM.Rows[i]["EXCH_RATE"].ToString());
                    dr1["AmountTHB"] = UIHelper.BindDecimal(dtbItemM.Rows[i]["AmountTHB"].ToString());
                    dr1.EndEdit();

                    dtbShow.Rows.Add(dr1);
                }
            }
            #endregion For Head Main

            #region For Head Item
            for (int i = 0; i < dtbHead.Rows.Count; i++)
            {
                DataRow drWht1 = null;
                DataRow drWht2 = null;

                DataRow dr = dtbShow.NewRow();
                dr.BeginEdit();
                dr["FIELD01"] = dtbHead.Rows[i]["FI_DOC"].ToString();
                dr["FIELD02"] = dtbHead.Rows[i]["DOC_DATE"].ToString();
                dr["FIELD03"] = dtbHead.Rows[i]["POST_DATE"].ToString();
                dr["FIELD04"] = dtbHead.Rows[i]["DOC_TYPE"].ToString();
                dr["FIELD05"] = dtbHead.Rows[i]["COMP_CODE"].ToString();
                dr["FIELD06"] = dtbHead.Rows[i]["PERIOD"].ToString();
                dr["FIELD07"] = dtbHead.Rows[i]["YEAR"].ToString();
                dr["FIELD08"] = dtbHead.Rows[i]["BRNCH"].ToString();
                dr["FIELD09"] = dtbHead.Rows[i]["Currency"].ToString();
                dr["FIELD10"] = dtbHead.Rows[i]["REF_DOC_NO"].ToString();
                dr["HEADERTXT"] = dtbHead.Rows[i]["HEADERTXT"].ToString();
                dr.EndEdit();
                dtbShow.Rows.Add(dr);

                DataRow[] drItemSelect = dtbItem.Select(" DOC_SEQ = '" + dtbHead.Rows[i]["DOC_SEQ"].ToString() + "' ");
                for (int j = 0; j < drItemSelect.Length; j++)
                {
                    DataRow dr1 = dtbShow.NewRow();
                    dr1.BeginEdit();

                    #region คำนวน Amount เมื่อมี WHT
                    double douWHT1 = 0;
                    if (drItemSelect[j]["WHTBase1"].ToString() != "")
                    {
                        string[] str = drItemSelect[j]["WHTBase1"].ToString().Split('|');
                        douWHT1 = 0 - double.Parse(str[1].ToString());
                    }
                    double douWHT2 = 0;
                    if (drItemSelect[j]["WHTBase2"].ToString() != "")
                    {
                        string[] str = drItemSelect[j]["WHTBase2"].ToString().Split('|');
                        douWHT2 = 0 - double.Parse(str[1].ToString());
                    }
                    double douAmt = double.Parse(drItemSelect[j]["AMT_DOCCUR"].ToString().Replace(",", "")) - douWHT1 - douWHT2;
                    #endregion คำนวน Amount เมื่อมี WHT

                    dr1["HEADERTXT"] = drItemSelect[j]["PK"].ToString();
                    dr1["FIELD10"] = drItemSelect[j]["Account"].ToString();
                    dr1["FIELD02"] = douAmt.ToString("###,###,###,###,###,###,##0.00");
                    dr1["FIELD03"] = drItemSelect[j]["TAX_CODE"].ToString();
                    dr1["FIELD04"] = drItemSelect[j]["BaseDate"].ToString();
                    dr1["FIELD05"] = drItemSelect[j]["PMNTTRMS"].ToString();
                    dr1["FIELD06"] = drItemSelect[j]["PYMT_METH"].ToString();
                    //dr1["FIELD09"] = drItemSelect["CURRENCY"].ToString();
                    dr1["FIELD09"] = drItemSelect[j]["CostCenter"].ToString();
                    dr1["FIELD11"] = drItemSelect[j]["InterOrder"].ToString();
                    dr1["FIELD12"] = drItemSelect[j]["WHTCode1"].ToString();
                    //dr1["FIELD13"] = drItemSelect[j]["WHTBase1"].ToString();
                    dr1["FIELD14"] = drItemSelect[j]["WHTCode2"].ToString();
                    //dr1["FIELD15"] = drItemSelect[j]["WHTBase2"].ToString();
                    dr1["FIELD16"] = drItemSelect[j]["ALLOC_NMBR"].ToString();
                    dr1["FIELD17"] = drItemSelect[j]["ITEM_TEXT"].ToString();

                    #region WHT1
                    if (drItemSelect[j]["WHTBase1"].ToString() != "")
                    {
                        string[] str = drItemSelect[j]["WHTBase1"].ToString().Split('|');
                        dr1["FIELD13"] = str[0];

                        double douBase = double.Parse(str[0].ToString());
                        double douNetAmount = 0 - double.Parse(str[1].ToString());

                        drWht1 = dtbShow.NewRow();
                        drWht1.BeginEdit();
                        //drWht1["FIELD02"] = "50";
                        drWht1["HEADERTXT"] = "50";
                        //drWht1["FIELD03"] = PostingConst.WHTAccount;
                        drWht1["FIELD10"] = PostingConst.WHTAccount;
                        //drWht1["FIELD04"] = douNetAmount.ToString("###,###,###,###,###,###,###,###,###,##0.00");
                        drWht1["FIELD02"] = douNetAmount.ToString("###,###,###,###,###,###,###,###,###,##0.00");
                        drWht1.EndEdit();
                    }
                    #endregion WHT1

                    #region WHT2
                    if (drItemSelect[j]["WHTBase2"].ToString() != "")
                    {
                        string[] str = drItemSelect[j]["WHTBase2"].ToString().Split('|');
                        dr1["FIELD15"] = str[0];

                        double douBase = double.Parse(str[0].ToString());
                        double douNetAmount = 0 - double.Parse(str[1].ToString());

                        drWht2 = dtbShow.NewRow();
                        drWht2.BeginEdit();
                        //drWht2["FIELD02"] = "50";
                        drWht2["HEADERTXT"] = "50";
                        //drWht2["FIELD03"] = PostingConst.WHTAccount;
                        drWht2["FIELD10"] = PostingConst.WHTAccount;
                        //drWht2["FIELD04"] = douNetAmount.ToString("###,###,###,###,###,###,###,###,###,##0.00");
                        drWht2["FIELD02"] = douNetAmount.ToString("###,###,###,###,###,###,###,###,###,##0.00");
                        drWht2.EndEdit();
                    }
                    #endregion WHT2

                    dr1["ExRate"] = UIHelper.BindExchangeRate(drItemSelect[j]["EXCH_RATE"].ToString());
                    dr1["AmountTHB"] = UIHelper.BindDecimal(drItemSelect[j]["AmountTHB"].ToString());

                    dr1.EndEdit();
                    dtbShow.Rows.Add(dr1);
                }

                if (drWht1 != null)
                    dtbShow.Rows.Add(drWht1);
                if (drWht2 != null)
                    dtbShow.Rows.Add(drWht2);
            }
            #endregion For Head Item

            if (dtbHeadB2C.Rows.Count <= 0)
                B2C.Visible = false;
            else
            {
                lblComCodeB2C.Text = dtbHeadB2C.Rows[0]["COMP_CODE"].ToString();
                lblComNameB2C.Text = dtbHeadB2C.Rows[0]["CompanyName"].ToString();
                B2C.Visible = true;
            }              

            #region For Head B2C
            for (int i = 0; i < dtbHeadB2C.Rows.Count; i++)
            {
                DataRow dr = dtbShowB2C.NewRow();
                dr.BeginEdit();
                dr["FIELD01"] = dtbHeadB2C.Rows[i]["FI_DOC"].ToString();
                dr["FIELD02"] = dtbHeadB2C.Rows[i]["DOC_DATE"].ToString();
                dr["FIELD03"] = dtbHeadB2C.Rows[i]["POST_DATE"].ToString();
                dr["FIELD04"] = dtbHeadB2C.Rows[i]["DOC_TYPE"].ToString();
                dr["FIELD05"] = dtbHeadB2C.Rows[i]["COMP_CODE"].ToString();
                dr["FIELD06"] = dtbHeadB2C.Rows[i]["PERIOD"].ToString();
                dr["FIELD07"] = dtbHeadB2C.Rows[i]["YEAR"].ToString();
                dr["FIELD08"] = dtbHeadB2C.Rows[i]["BRNCH"].ToString();
                dr["FIELD09"] = dtbHeadB2C.Rows[i]["Currency"].ToString();
                dr["FIELD10"] = dtbHeadB2C.Rows[i]["REF_DOC_NO"].ToString();
                dr["HEADERTXT"] = dtbHeadB2C.Rows[i]["HEADERTXT"].ToString();
                dr.EndEdit();
                dtbShowB2C.Rows.Add(dr);

                DataRow[] drItemSelect = dtbItemB2C.Select(" DOC_SEQ = '" + dtbHeadB2C.Rows[i]["DOC_SEQ"].ToString() + "' ");
                for (int j = 0; j < drItemSelect.Length; j++)
                {
                    DataRow dr1 = dtbShowB2C.NewRow();
                    dr1.BeginEdit();

                    dr1["HEADERTXT"] = drItemSelect[j]["PK"].ToString();
                    dr1["FIELD10"] = drItemSelect[j]["Account"].ToString();
                    dr1["FIELD02"] = drItemSelect[j]["AMT_DOCCUR"].ToString();
                    dr1["FIELD03"] = drItemSelect[j]["TAX_CODE"].ToString();
                    dr1["FIELD04"] = drItemSelect[j]["BaseDate"].ToString();
                    dr1["FIELD05"] = drItemSelect[j]["PMNTTRMS"].ToString();
                    dr1["FIELD07"] = drItemSelect[j]["PYMT_METH"].ToString();
                    //dr1["FIELD09"] = drItemSelect["CURRENCY"].ToString();
                    dr1["FIELD09"] = drItemSelect[j]["CostCenter"].ToString();
                    dr1["FIELD11"] = drItemSelect[j]["InterOrder"].ToString();
                    dr1["FIELD12"] = drItemSelect[j]["WHTCode1"].ToString();
                    //dr1["FIELD13"] = drItemSelect[j]["WHTBase1"].ToString();
                    dr1["FIELD14"] = drItemSelect[j]["WHTCode2"].ToString();
                    //dr1["FIELD15"] = drItemSelect[j]["WHTBase2"].ToString();
                    dr1["FIELD16"] = drItemSelect[j]["ALLOC_NMBR"].ToString();
                    dr1["FIELD17"] = drItemSelect[j]["ITEM_TEXT"].ToString();

                    dr1.EndEdit();
                    dtbShowB2C.Rows.Add(dr1);
                }
            }
            #endregion For Head B2C

            GridViewShow.DataSource = dtbShow;
            GridViewShow.DataBind();

            GridViewShowB2C.DataSource = dtbShowB2C;
            GridViewShowB2C.DataBind();
        }
        #endregion private void ShowData()

        #region private void DisplayButton(string PostingStatus, string Che09Status)
        private void DisplayButton(string PostingStatus, string Che09Status)
        {
            #region Show Status
            if ((string.IsNullOrEmpty(PostingStatus) || PostingStatus == "N") && Che09Status == "N")
                lblStatus.Text = "NEW";
            else if (PostingStatus == "N" && Che09Status == "S")
                lblStatus.Text = "SIMULATED";
            else if (PostingStatus == "PP")
                lblStatus.Text = "PATIAL POST";
            else if (PostingStatus == "P")
                lblStatus.Text = "POSTED";
            else if (PostingStatus == "C")
                lblStatus.Text = "COMPLETED";
            #endregion Show Status

            #region Set Defualt Button

            btnSimulate.Visible = true;
            btnPost.Visible = true;
            btnApprove.Visible = true;
            btnReverse.Visible = true;

            btnSimulate.Enabled = true;
            btnPost.Enabled = true;
            btnApprove.Enabled = true;
            btnReverse.Enabled = true;

            btnSimulate.Visible = CanSimulateOrPostOrReverse();
            btnPost.Visible = CanSimulateOrPostOrReverse();
            btnApprove.Visible = CanApprove();
            btnReverse.Visible = CanSimulateOrPostOrReverse();
            #endregion Set Defualt Button

            #region Set Button
            // For Adv ที่ Import เข้ามา
            
            if ( (string.IsNullOrEmpty(PostingStatus) || PostingStatus == "N") && Che09Status == "N")
            {
                btnSimulate.Enabled = true;
                btnPost.Enabled = false;
                btnApprove.Enabled = false;
                btnReverse.Enabled = false;
            }
            else if (PostingStatus == "N" && Che09Status == "PS")
            {
                btnSimulate.Enabled = true;
                btnPost.Enabled = false;
                btnApprove.Enabled = false;
                btnReverse.Enabled = false;
            }
            else if ((string.IsNullOrEmpty(PostingStatus) || PostingStatus == "N") && Che09Status == "S")
            {
                btnSimulate.Visible = false;
                btnPost.Enabled = true;
                btnApprove.Enabled = false;
                btnReverse.Enabled = false;
            }
            else if (PostingStatus == "P")
            {
                btnSimulate.Visible = false;
                btnPost.Visible = false;
                btnApprove.Enabled = true;
                btnReverse.Enabled = true;
            }
            else if (PostingStatus == "C")
            {
                btnSimulate.Visible = false;
                btnPost.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Enabled = true;
            }
            else if (PostingStatus == "PP")
            {
                btnSimulate.Visible = false;
                btnPost.Enabled = true;
                btnApprove.Enabled = false;
                btnReverse.Enabled = true;
            }

            if (PostingStatus == "C" && string.IsNullOrEmpty(Che09Status))
            {
                btnSimulate.Visible = false;
                btnPost.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;
            }
            #endregion Set Button
        }
        #endregion private void DisplayButton(string PostingStatus, string Che09Status)

        #region protected void GridViewShow_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void GridViewShow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[1].Text.Length==4)
            {
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.BackColor = System.Drawing.Color.Yellow;
                e.Row.ForeColor = System.Drawing.Color.Blue;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[4].Text.Split('.').Length != 3)
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[5].Text.Split('.').Length != 3)
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[6].Text.Split('.').Length != 3)
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;

            if (e.Row.RowType == DataControlRowType.DataRow && UIHelper.ParseDouble(e.Row.Cells[4].Text) < 0)
            {
                e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[4].Text = UIHelper.BindDecimalNumberAccountFormat(e.Row.Cells[4].Text);
            }

            if (e.Row.RowType == DataControlRowType.DataRow && UIHelper.ParseDouble(e.Row.Cells[6].Text) < 0)
            {
                e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[6].Text = UIHelper.BindDecimalNumberAccountFormat(e.Row.Cells[6].Text);
            }
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[3].Text == "PCADVCL")
            //{
            //    e.Row.BackColor = System.Drawing.Color.Gray;
            //}
        }
        #endregion protected void GridViewShow_RowDataBound(object sender, GridViewRowEventArgs e)

        #region public void Initialize(long DocID, DocumentKind DocKind)
        public void Initialize(long DocID, DocumentKind DocKind)
        {
            CallOnObjectLookUpCalling();
            this.DocID = DocID;
            this.DocKind = DocKind.ToString();
        }
        #endregion public void Initialize(long DocID, DocumentKind DocKind)

        #region public bool IsNoExpense()
        public bool IsNoExpense()
        {
            bool isNoExpense = false;
            if (this.DocKind == DocumentKind.Expense.ToString())
            {
                FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                if (docExpense.TotalExpense <= 0)
                    isNoExpense = true;
            }

            return isNoExpense;
        }
        #endregion public bool IsNoExpense()

        #endregion <-- FUNCTION -->

        #region <-- Button Event -->

        #region Simulate(bool isShow)
        private void Simulate(bool isShow)
        {
            if (IsNoExpense())
            {
                IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(this.DocID, this.DocKind);
                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    listBAPIACHE09[i].DocStatus = "S";
                    BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[i]);
                }
                this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
            }
            else
            {
                if (isShow)
                {
                    #region เรียก Show จากหน้าฟอร์ม
                    IList<BAPISimulateReturn> bapiReturn = this.GetPostingService().BAPISimulate(this.DocID, this.DocKind);
                    this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                    this.ViewPostMessage1.Show(bapiReturn);
                    #endregion เรียก Show จากหน้าฟอร์ม
                }
                else
                {
                    #region เรียกแสดงที่หน้า ViewPost
                    if (CanSimulateOrPostOrReverse())
                    {
                        IList<BAPISimulateReturn> bapiReturn = this.GetPostingService().BAPISimulate(this.DocID, this.DocKind);
                        this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                        this.ViewPostMessage1.Show(bapiReturn);
                    }
                    else
                    {
                        this.Alert("ไม่อนุญาติให้ทำการ Simulate");
                    }
                    #endregion เรียกแสดงที่หน้า ViewPost
                }
            }
        }
        #endregion Simulate(bool isShow)

        #region protected void btnSimulate_Click(object sender, EventArgs e)
        protected void btnSimulate_Click(object sender, EventArgs e)
        {
            btnSimulate.Enabled = false;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                Simulate(false);
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("ViewPost.Simulate_Click() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("ViewPost.Simulate_Click() (ex.Message) : " + ex.Message);
                }
                errors.AddError("ViewPost.Error", new Spring.Validation.ErrorMessage(ex.Message.ToString()));
                this.ValidationErrors.MergeErrors(errors);
                UpdatePanelSearchAccount.Update();
            }

            this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
        }
        #endregion protected void btnSimulate_Click(object sender, EventArgs e)

        #region protected void btnPost_Click(object sender, EventArgs e)
        protected void btnPost_Click(object sender, EventArgs e)
        {
            btnPost.Enabled = false;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                if (CanSimulateOrPostOrReverse())
                {
                    if (IsNoExpense())
                    {
                        SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                        doc.PostingStatus = "P";
                        SCGDocumentService.SaveOrUpdate(doc);

                        ChangeState = true;
                        WorkFlowPost();
                        this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                        this.ShowData();
                        this.UpdatePanelSearchAccount.Update();
                    }
                    else
                    {
                        IList<BAPIPostingReturn> bapiReturn = this.GetPostingService().BAPIPosting(this.DocID, this.DocKind);

                        #region Posting Old Code
                        //IList<BAPIPostingReturn> bapiReturn = this.GetPostingService().BAPIPosting(this.DocID, this.DocKind);

                        //if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                        //{
                        //    FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                        //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "P")
                        //    {
                        //        docExpense.RemittancePostingStatus = "P";
                        //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        //    }
                        //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        //    {
                        //        docExpense.RemittancePostingStatus = "PP";
                        //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        //    }
                        //}
                        //else
                        //{
                        //    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                        //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "P")
                        //    {
                        //        doc.PostingStatus = "P";
                        //        SCGDocumentService.SaveOrUpdate(doc);
                        //    }
                        //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        //    {
                        //        doc.PostingStatus = "PP";
                        //        SCGDocumentService.SaveOrUpdate(doc);
                        //    }
                        //}
                        #endregion Posting Old Code

                        #region Call WorkFlow
                        bool isSuccess = true;
                        for (int i = 0; i < bapiReturn.Count; i++)
                        {
                            if (bapiReturn[i].PostingStatus != "S")
                            {
                                isSuccess = false;
                                break;
                            }
                        }

                        if (isSuccess)
                        {
                            ChangeState = true;
                            WorkFlowPost();
                        }
                        #endregion Call WorkFlow

                        this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                        this.ShowData();
                        this.ViewPostMessage1.Show(bapiReturn);
                        this.UpdatePanelSearchAccount.Update();
                    }
                }
                else
                {
                    this.Alert("ไม่อนุญาติให้ทำการ Post");
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                UpdatePanelSearchAccount.Update();
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("ViewPost.Post_Click() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("ViewPost.Post_Click() (ex.Message) : " + ex.Message);
                }
                errors.AddError("ViewPost.Error", new Spring.Validation.ErrorMessage(ex.Message.ToString()));
                this.ValidationErrors.MergeErrors(errors);
                UpdatePanelSearchAccount.Update();
            }

            this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
        }
        #endregion protected void btnPost_Click(object sender, EventArgs e)

        #region protected void btnApprove_Click(object sender, EventArgs e)
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            btnApprove.Enabled = false;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                if (CanApprove())
                {
                    if (IsNoExpense())
                    {
                        SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                        doc.PostingStatus = "C";
                        SCGDocumentService.SaveOrUpdate(doc);

                        ChangeState = true;
                        WorkFlowApprove(Request.QueryString["wfid"].ToString());
                        this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                        this.ShowData();
                        this.UpdatePanelSearchAccount.Update();
                    }
                    else
                    {
                        IList<BAPIApproveReturn> bapiReturn = this.GetPostingService().BAPIApprove(this.DocID, this.DocKind, this.UserAccount.UserID);

                        #region Approve Old Code
                        //IList<BAPIApproveReturn> bapiReturn = this.GetPostingService().BAPIApprove(this.DocID, this.DocKind, this.UserAccount.UserID);

                        //if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                        //{
                        //    FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                        //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                        //    {
                        //        docExpense.RemittancePostingStatus = "C";
                        //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        //    }
                        //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        //    {
                        //        docExpense.RemittancePostingStatus = "PP";
                        //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        //    }
                        //}
                        //else
                        //{
                        //    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                        //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                        //    {
                        //        doc.PostingStatus = "C";
                        //        SCGDocumentService.SaveOrUpdate(doc);
                        //    }
                        //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        //    {
                        //        doc.PostingStatus = "PP";
                        //        SCGDocumentService.SaveOrUpdate(doc);
                        //    }
                        //}
                        #endregion Approve Old Code

                        #region Call WorkFlow
                        bool isSuccess = true;
                        for (int i = 0; i < bapiReturn.Count; i++)
                        {
                            if (bapiReturn[i].ApproveStatus != "S")
                            {
                                isSuccess = false;
                                break;
                            }
                        }

                        if (isSuccess)
                        {
                            ChangeState = true;
                            WorkFlowApprove(Request.QueryString["wfid"].ToString());
                        }
                        #endregion Call WorkFlow

                        this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                        this.ShowData();
                        this.ViewPostMessage1.Show(bapiReturn);
                        this.UpdatePanelSearchAccount.Update();
                    }
                }
                else
                {
                    this.Alert("ไม่อนุญาติให้ทำการ Approve");
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                UpdatePanelSearchAccount.Update();
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("ViewPost.Approve_Click() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("ViewPost.Approve_Click() (ex.Message) : " + ex.Message);
                }
                errors.AddError("ViewPost.Error", new Spring.Validation.ErrorMessage(ex.Message.ToString()));
                this.ValidationErrors.MergeErrors(errors);
                UpdatePanelSearchAccount.Update();
            }

            this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
        }
        #endregion protected void btnApprove_Click(object sender, EventArgs e)


        #region protected void btnReverse_Click(object sender, EventArgs e)
        protected void btnReverse_Click(object sender, EventArgs e)
        {
            btnReverse.Enabled = false;

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
                if (CanSimulateOrPostOrReverse())
                {
                    ViewPostEditDate1.ShowDate(this.DocID, this.DocKind);
                    
                    #region Old Code
                    //#region Reverse
                    //IList<BAPIReverseReturn> bapiReturn = this.GetPostingService().BAPIReverse(this.DocID, this.DocKind);

                    //if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                    //{
                    //    FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                    //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "S")
                    //    {
                    //        docExpense.RemittancePostingStatus = "N";
                    //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                    //    }
                    //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                    //    {
                    //        docExpense.RemittancePostingStatus = "PP";
                    //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                    //    }
                    //}
                    //else
                    //{
                    //    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                    //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "S")
                    //    {
                    //        doc.PostingStatus = "N";
                    //        SCGDocumentService.SaveOrUpdate(doc);
                    //    }
                    //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                    //    {
                    //        doc.PostingStatus = "PP";
                    //        SCGDocumentService.SaveOrUpdate(doc);
                    //    }
                    //}
                    //#endregion Reverse

                    //#region Call WorkFlow
                    //bool isSuccess = true;
                    //for (int i = 0; i < bapiReturn.Count; i++)
                    //{
                    //    if (bapiReturn[i].ReverseStatus != "S")
                    //    {
                    //        isSuccess = false;
                    //        break;
                    //    }
                    //}

                    //if (isSuccess)
                    //{
                    //    ChangeState = true;
                    //    //WorkFlowReverse();
                    //}
                    //#endregion Call WorkFlow

                    //this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                    //this.ShowData();
                    //this.ViewPostMessage1.Show(bapiReturn);
                    #endregion Old Code
                }
                else
                {
                    this.Alert("ไม่อนุญาติให้ทำการ Reverse");
                }
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                UpdatePanelSearchAccount.Update();
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("ViewPost.Reverse_Click() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("ViewPost.Reverse_Click() (ex.Message) : " + ex.Message);
                }
                errors.AddError("ViewPost.Error", new Spring.Validation.ErrorMessage(ex.Message.ToString()));
                this.ValidationErrors.MergeErrors(errors);
                UpdatePanelSearchAccount.Update();
            }

            this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
        }
        #endregion protected void btnReverse_Click(object sender, EventArgs e)

        #region void ViewPostEditDate1_OnPopUpReturn(object sender, PopUpReturnArgs e)
        void ViewPostEditDate1_OnPopUpReturn(object sender, PopUpReturnArgs e)
        {
            if (e.Type.Equals(PopUpReturnType.OK))
            {
                if (IsNoExpense())
                {
                    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                    doc.PostingStatus = "N";
                    SCGDocumentService.SaveOrUpdate(doc);

                    ChangeState = true;
                    this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                    this.ShowData();
                    this.UpdatePanelSearchAccount.Update();
                }
                else
                {
                    IList<BAPIReverseReturn> bapiReturn = this.GetPostingService().BAPIReverse(this.DocID, this.DocKind);

                    #region Reverse Old Code
                    //IList<BAPIReverseReturn> bapiReturn = this.GetPostingService().BAPIReverse(this.DocID, this.DocKind);

                    //if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                    //{
                    //    FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                    //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "S")
                    //    {
                    //        docExpense.RemittancePostingStatus = "N";
                    //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                    //    }
                    //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                    //    {
                    //        docExpense.RemittancePostingStatus = "PP";
                    //        FnExpenseDocumentService.SaveOrUpdate(docExpense);
                    //    }
                    //}
                    //else
                    //{
                    //    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                    //    if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "S")
                    //    {
                    //        doc.PostingStatus = "N";
                    //        SCGDocumentService.SaveOrUpdate(doc);
                    //    }
                    //    else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                    //    {
                    //        doc.PostingStatus = "PP";
                    //        SCGDocumentService.SaveOrUpdate(doc);
                    //    }
                    //}
                    #endregion Reverse Old Code

                    #region Call WorkFlow
                    bool isSuccess = true;
                    for (int i = 0; i < bapiReturn.Count; i++)
                    {
                        if (bapiReturn[i].ReverseStatus != "S")
                        {
                            isSuccess = false;
                            break;
                        }
                    }

                    if (isSuccess)
                    {
                        ChangeState = true;
                        //WorkFlowReverse();
                    }
                    #endregion Call WorkFlow

                    this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                    this.ShowData();
                    this.ViewPostMessage1.Show(bapiReturn);
                    this.UpdatePanelSearchAccount.Update();
                }
            }
        }
        #endregion void ViewPostEditDate1_OnPopUpReturn(object sender, PopUpReturnArgs e)


        #region protected void btnClose_Click(object sender, EventArgs e)
        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender1.Hide();

            if (ChangeState)
            {
                string wfid = Request.QueryString["wfid"].ToString();
                Response.Redirect("SubmitResult.aspx?wfid=" + wfid);
            }
        }
        #endregion protected void btnClose_Click(object sender, EventArgs e)

        #region protected void ctlCloseImageButton_Click(object sender, ImageClickEventArgs e)
        protected void ctlCloseImageButton_Click(object sender, ImageClickEventArgs e)
        {
            this.ModalPopupExtender1.Hide();

            if (ChangeState)
            {
                string wfid = Request.QueryString["wfid"].ToString();
                Response.Redirect("SubmitResult.aspx?wfid=" + wfid);
            }
        }
        #endregion protected void ctlCloseImageButton_Click(object sender, ImageClickEventArgs e)

        #endregion <-- Button Event -->

        #region WorkFlow

        #region public void WorkFlowPost()
        public void WorkFlowPost()
        {
            string wfid = Request.QueryString["wfid"].ToString();

            if (this.DocKind == DocumentKind.Advance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Verify, WorkFlowStatEventNameConst.WaitVerify, WorkFlowTypeName.Advance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Verify, eventData);
            }
            else if (this.DocKind == DocumentKind.Remittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Verify, WorkFlowStatEventNameConst.WaitRemittance, WorkFlowTypeName.Remittance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Verify, eventData);
            }
            else if (this.DocKind == DocumentKind.Expense.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Verify, WorkFlowStatEventNameConst.WaitVerify, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Verify, eventData);
            }
            else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                //พี่หยวยบอกว่า ExpenseRemittance นั้น จะ Pay ตอนกดปุ่ม Approve เท่านั้น
                //IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Pay, WorkFlowStatEventNameConst.WaitRemittance, WorkFlowTypeName.Expense);
                //object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                //WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Pay, eventData);
            }
        }
        #endregion public void WorkFlowPost()

        #region public void WorkFlowApprove()
        public void WorkFlowApprove(string wfid)
        {
            //string wfid = Request.QueryString["wfid"].ToString();

            if (this.DocKind == DocumentKind.Advance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveVerify, WorkFlowTypeName.Advance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Approve, eventData);
            }
            else if (this.DocKind == DocumentKind.Remittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveRemittance, WorkFlowTypeName.Remittance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Approve, eventData);
            }
            else if (this.DocKind == DocumentKind.Expense.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Approve, WorkFlowStatEventNameConst.WaitApproveVerify, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Approve, eventData);
            }
            else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Pay, WorkFlowStatEventNameConst.WaitRemittance, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Pay, eventData);
            }
        }
        #endregion public void WorkFlowApprove()

        #region public void WorkFlowReverse()
        public void WorkFlowReverse()
        {
            string wfid = Request.QueryString["wfid"].ToString();

            if (this.DocKind == DocumentKind.Advance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Verify, WorkFlowStatEventNameConst.WaitVerify, WorkFlowTypeName.Advance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Verify, eventData);
            }
            else if (this.DocKind == DocumentKind.Remittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Verify, WorkFlowStatEventNameConst.WaitRemittance, WorkFlowTypeName.Remittance);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Verify, eventData);
            }
            else if (this.DocKind == DocumentKind.Expense.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Verify, WorkFlowStatEventNameConst.WaitVerify, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Verify, eventData);
            }
            else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                IList<WorkFlowStateEvent> workFlowState = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindWorkFlowStateEvent(WorkFlowEventNameConst.Pay, WorkFlowStatEventNameConst.WaitRemittance, WorkFlowTypeName.Expense);
                object eventData = new SubmitResponse(workFlowState[0].WorkFlowStateEventID);
                WorkFlowService.NotifyEvent(int.Parse(wfid), WorkFlowEventNameConst.Pay, eventData);
            }
        }
        #endregion public void WorkFlowReverse()

        #endregion WorkFlow

        #region public void Show()
        public void Show()
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            try
            {
               

                if (this.DocID == 0 || this.DocKind == "")
                {
                    this.Alert("Please ! ! ! Initialize ViewPost Control.");
                }
                else
                {
                    #region Have DocID Parameter
                    ChangeState = false;

                    SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                    if (doc == null)
                    {
                        this.Alert("Don't found this DocumentID : " + this.DocID + " in Database !!!");
                    }
                    else
                    {
                        #region Have Document
                        lblDocumentNo.Text = doc.DocumentNo;

                        string strDocPosting = "";
                        if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                        {
                            FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                            strDocPosting = docExpense.RemittancePostingStatus;
                        }
                        else
                        {
                            strDocPosting = doc.PostingStatus;
                        }

                        //if (strDocPosting=="C" || strDocPosting=="c")
                        //{ }
                        //else
                        if ( string.IsNullOrEmpty(GetDocumentPostingStatus()) || GetDocumentPostingStatus() == "N")
                        {
                            this.GetPostingService().DeletePostingDataByDocId(this.DocID, this.DocKind);
                            this.GetPostingService().CreatePostData(this.DocID, this.DocKind);
                            Simulate(true);
                        }

                        this.ShowData();
                        this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                        this.ModalPopupExtender1.Show();
                        #endregion Have Document
                    }
                    #endregion Have DocID Parameter
                }
            }
            catch (Exception ex)
            {
                btnSimulate.Visible = false;
                btnPost.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;

                this.ShowData();
                //this.DisplayButton(GetDocumentPostingStatus(), this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind));
                this.ModalPopupExtender1.Show();

                if (logger != null)
                {
                    logger.Error("ViewPost.Show() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("ViewPost.Show() (ex.Message) : " + ex.Message);
                }

                errors.AddError("ViewPost.Error", new Spring.Validation.ErrorMessage(ex.Message.ToString()));
                this.ValidationErrors.MergeErrors(errors);
                UpdatePanelSearchAccount.Update();
            }
        }
        #endregion public void Show()

        #region public void ApprovePosting()
        public void ApprovePosting(string wfid, bool callFromDocView)
        {
            try
            {

                SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                if (this.DocID == 0 || this.DocKind == "")
                    this.Alert("Please ! ! ! Initialize ViewPost Control.");
                else if (doc == null)
                    this.Alert("Don't found this DocumentID : " + this.DocID + " in Database !!!");
                else if (!CanApprove())
                    this.Alert("ไม่อนุญาติให้ทำการ Approve");
                else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                    this.Alert("เอกสารนี้ <" + DocID + "> ได้ทำการ Approve ไปแล้ว");
                else
                {
                    ChangeState = false;

                    #region Approve
                    IList<BAPIApproveReturn> bapiReturn = this.GetPostingService().BAPIApprove(this.DocID, this.DocKind, this.UserAccount.UserID);

                    if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                    {
                        FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                        if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                        {
                            docExpense.RemittancePostingStatus = "C";
                            FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        }
                        else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        {
                            docExpense.RemittancePostingStatus = "PP";
                            FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        }
                    }
                    else
                    {
                        if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                        {
                            doc.PostingStatus = "C";
                            SCGDocumentService.SaveOrUpdate(doc);
                        }
                        else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        {
                            doc.PostingStatus = "PP";
                            SCGDocumentService.SaveOrUpdate(doc);
                        }
                    }
                    #endregion Approve

                    #region Call WorkFlow
                    bool isSuccess = true;
                    for (int i = 0; i < bapiReturn.Count; i++)
                    {
                        if (bapiReturn[i].ApproveStatus != "S")
                        {
                            isSuccess = false;
                            break;
                        }
                    }

                    if (isSuccess)
                    {
                        ChangeState = true;
                        WorkFlowApprove(wfid);
                    }
                    #endregion Call WorkFlow

                    if (ChangeState)
                    {
                        //string wfid = Request.QueryString["wfid"].ToString();
                        if (!callFromDocView)
                            Response.Redirect("SubmitResult.aspx?wfid=" + wfid);
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("ViewPost.ApprovePosting() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("ViewPost.ApprovePosting() (ex.Message) : " + ex.Message);
                }

                throw ex;
            }
        }
        #endregion public void ApprovePosting()

        #region public void ApprovePostingNew()
        public void ApprovePostingNew(string wfid, bool callFromDocView)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                SCGDocument doc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(this.DocID);
                if (this.DocID == 0 || this.DocKind == "")
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Please ! ! ! Initialize ViewPost Control."));
                else if (doc == null)
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Don't found this DocumentID : " + this.DocID + " in Database !!!"));
                else if (!CanApprove())
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ไม่อนุญาติให้ทำการ Approve"));
                else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("เอกสารนี้ <" + DocID + "> ได้ทำการ Approve ไปแล้ว"));
                else
                {
                    ChangeState = false;

                    #region Approve
                    IList<BAPIApproveReturn> bapiReturn = this.GetPostingService().BAPIApprove(this.DocID, this.DocKind, this.UserAccount.UserID);

                    if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                    {
                        FnExpenseDocument docExpense = ScgeAccountingQueryProvider.FnExpenseDocumentQuery.GetExpenseDocumentByDocumentID(this.DocID);
                        if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                        {
                            docExpense.RemittancePostingStatus = "C";
                            FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        }
                        else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        {
                            docExpense.RemittancePostingStatus = "PP";
                            FnExpenseDocumentService.SaveOrUpdate(docExpense);
                        }
                    }
                    else
                    {
                        if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "A")
                        {
                            doc.PostingStatus = "C";
                            SCGDocumentService.SaveOrUpdate(doc);
                        }
                        else if (this.GetPostingService().GetDocumentStatus(this.DocID, this.DocKind) == "PP")
                        {
                            doc.PostingStatus = "PP";
                            SCGDocumentService.SaveOrUpdate(doc);
                        }
                    }
                    #endregion Approve

                    #region Call WorkFlow
                    bool isSuccess = true;
                    for (int i = 0; i < bapiReturn.Count; i++)
                    {
                        if (bapiReturn[i].ApproveStatus != "S")
                        {
                            isSuccess = false;
                            break;
                        }
                    }

                    if (isSuccess)
                    {
                        ChangeState = true;
                        WorkFlowApprove(wfid);
                    }
                    #endregion Call WorkFlow

                    if (ChangeState)
                    {
                        //string wfid = Request.QueryString["wfid"].ToString();
                        if (!callFromDocView)
                            Response.Redirect("SubmitResult.aspx?wfid=" + wfid);
                    }
                    else
                    {
                        for (int i = 0; i < bapiReturn.Count; i++)
                            for (int j = 0; j < bapiReturn[i].ApproveReturn.Count; j++)
                                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage(bapiReturn[i].ApproveReturn[j].Message));

                        throw new ServiceValidationException(errors);
                    }
                }
            }
            catch (ServiceValidationException serviceEx)
            {
                throw serviceEx;
            }
            catch (Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("ViewPost.ApprovePosting() ERROR : " + ex.ToString());
                }

                throw ex;
            }
        }
        #endregion public void ApprovePostingNew()
    }
}