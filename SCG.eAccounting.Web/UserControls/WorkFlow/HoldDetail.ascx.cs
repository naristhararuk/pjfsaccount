using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.EventUserControl;
using SCG.eAccounting.DTO;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.BLL.Implement;
using SS.SU.BLL;
using SS.SU.Query;

namespace SCG.eAccounting.Web.UserControls.WorkFlow
{
    public partial class HoldDetail : SS.Standard.UI.BaseUserControl, IEventControl
    {
        public int WorkFlowStateEventID { get; set; }
        public IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion
        public void Initialize(long workFlowID)
        {
            Document document = WorkFlowQueryProvider.WorkFlowQuery.GetDocumentByWorkFlowID(workFlowID);
            if (document != null && document.DocumentType !=null)//Advance DocumentType when 1 is Domestic when 5 is Foreign
            {
                string AdvanceProgramCode = "AdvanceForm";
                string ExpenseProgramCode = "ExpenseForm";
                if (document.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceDomesticDocument) || document.DocumentType.DocumentTypeID.Equals(DocumentTypeID.AdvanceForeignDocument))
                {

                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlSubjectLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.Subject.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlPaymentTypeLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.PaymentType.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlRequestDateOfAdvanceLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.RequestDateOfAdvance.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlRequestDateOfRemittanceLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.RequestDateOfRemittance.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlReasonLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.Reason.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlAttachmentLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.Attachment.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(AdvanceProgramCode, "ctlMemoLabel.Text", UserAccount.CurrentLanguageID.ToString()), AdvanceFieldGroup.Memo.ToString()));

                }
                if (document.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseDomesticDocument) || document.DocumentType.DocumentTypeID.Equals(DocumentTypeID.ExpenseForeignDocument))//Expense DocumentType when 3 is Domestic when 7 is Foreign  
                {
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlCostCenterLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.CostCenter.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlAccountCodeLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.AccountCode.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlInternalOrderLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.InternalOrder.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlSubjectLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.Subject.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlReferenceNoLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.ReferenceNo.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlVATLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.VAT.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlWHTaxLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.WHTax.ToString()));
                    ctlLeftIdentify.Items.Add(new ListItem(QueryProvider.SuGlobalTranslateQuery.GetResolveControl(ExpenseProgramCode, "ctlAttachmentLabel.Text", UserAccount.CurrentLanguageID.ToString()), ExpenseFieldGroup.Attachment.ToString()));

                }
            }
        }


        #region even click
        protected void ctlCopyAllToRight_Click(object sender, EventArgs e)
        {
            int _count = ctlLeftIdentify.Items.Count;
            if (_count != 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = ctlLeftIdentify.Items[i].Text;
                    item.Value = ctlLeftIdentify.Items[i].Value;
                    //Add the item to selected list
                    ctlRightIdentify.Items.Add(item);
                }

            }

            //clear Left list
            ctlLeftIdentify.Items.Clear();
        }

        protected void ctlCopyAllToLeft_Click(object sender, EventArgs e)
        {
            int _count = ctlRightIdentify.Items.Count;
            if (_count != 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = ctlRightIdentify.Items[i].Text;
                    item.Value = ctlRightIdentify.Items[i].Value;
                    //Add the item to selected list Box
                    ctlLeftIdentify.Items.Add(item);
                }

            }

            //clear left list 
            ctlRightIdentify.Items.Clear();


        }

        protected void ctlCopyToRight_Click(object sender, EventArgs e)
        {
            //ctlLeftIdentify.selectedIndex > -1 => not selete data in listBox
            if (ctlLeftIdentify.SelectedIndex > -1)
            {
                string _value = ctlLeftIdentify.SelectedItem.Value;
                string _text = ctlLeftIdentify.SelectedItem.Text;
                ListItem item = new ListItem();
                item.Text = _text;
                item.Value = _value;
                ctlRightIdentify.Items.Add(item);
                ctlLeftIdentify.Items.Remove(item);

            }
        }

        protected void ctlCopyToLeft_Click(object sender, EventArgs e)
        {
            //ctlRightIdentify.selectedIndex > -1 => not selete data in listBox
            if (ctlRightIdentify.SelectedIndex > -1)
            {
                string _value = ctlRightIdentify.SelectedItem.Value;
                string _text = ctlRightIdentify.SelectedItem.Text;
                ListItem item = new ListItem();
                item.Text = _text;
                item.Value = _value;
                ctlLeftIdentify.Items.Add(item);
                ctlRightIdentify.Items.Remove(item);

            }

        }
        #endregion


        //public void Initialize(long workFlowID,IList<
        #region IEventControl Members

        public object GetEventData(int workFlowStateEventID)
        {
            HoldDetailResponse returnObj = new HoldDetailResponse();
            returnObj.WorkFlowStateEventID = workFlowStateEventID;
            //foreach (ListItem item in ctlLeftIdentify.Items)
            //{
            //    returnObj.LeftIdentify.Add(item.Value);
            //}
            //foreach (ListItem item in ctlRightIdentify.Items)
            //{
            //    returnObj.RightIdentify.Add(item.Value);
            //}
            foreach (ListItem item in ctlRightIdentify.Items)
            {
                returnObj.HoldFields.Add(item.Value);
            }
            returnObj.Remark = ctlRemark.Text;
            
            return returnObj;
        }

        #endregion
    }
}