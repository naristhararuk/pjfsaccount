using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;

using SS.SU.DTO;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class SendFollowUpEmail : BaseUserControl
    {
        #region Properties
        public ISCGEmailService SCGEmailService { get; set; }
        public long RequesterID
        { 
            get { return UIHelper.ParseLong(ctlUserID.Text); }
            set { ctlUserID.Text = value.ToString(); }
        }
        public string EmailType 
        {
            get { return ctlEmailType.Text; }
            set { ctlEmailType.Text = value; }
        }
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ctlDocumentID.Text); }
            set { ctlDocumentID.Text = value.ToString(); } 
        }
        public long AdvanceDocumentID
        {
            get { return UIHelper.ParseLong(ctlAdvanceDocumentID.Text);}
            set { ctlAdvanceDocumentID.Text = value.ToString(); }
        }
        public long CreatorID 
        {  
            get { return UIHelper.ParseLong(ctlCreatorID.Text); }
            set { ctlCreatorID.Text = value.ToString(); }
        }
        public string RequestNo
        {
            get { return ctlRequestNo.Text; }
            set { ctlRequestNo.Text = value; }
        }
        #endregion

        #region OnPreRender(EventArgs e)
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            }
        #endregion

        #region Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlUserProfileLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlUserProfileLookup_OnObjectLookUpReturn);
        }
        #endregion

        #region ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)
        protected void ctlUserProfileLookup_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserProfileLookup.isMultiple = true;
            ctlUserProfileLookup.Show();
        }
        #endregion

        #region void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        protected void ctlUserProfileLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<SuUser> userInfo = (IList<SuUser>)e.ObjectReturn;
            string CCEmail = "";
            if (userInfo != null)
            {
                foreach (SuUser user in userInfo)
                {
                    CCEmail += user.Email.ToString()+";";
                }
            }
            if (!string.IsNullOrEmpty(ctlCC.Text) && !ctlCC.Text.EndsWith(";"))
            {
                ctlCC.Text += ";";
            }
            ctlCC.Text += CCEmail;
            ctlUpdatePanelEmailLog.Update();
        }
        #endregion

        #region private method
        public void Show()
        {
            //for show Email To:
            string strTo = string.Empty;
            //if (this.EmailType.Equals(SCG.eAccounting.BLL.EmailType.EM10))
            //{
            //    SuEMailLog suEmailLog = ScgeAccountingQueryProvider.EmailLogQuery.
            //}
            if (this.RequesterID != 0)
            {
                SuUser user = QueryProvider.SuUserQuery.FindProxyByIdentity(this.RequesterID);
                if (user != null)
                    strTo = user.Email + ";";
            }
            if (this.CreatorID != 0)
            {
                SuUser userCreator = QueryProvider.SuUserQuery.FindProxyByIdentity(this.CreatorID);
                if (userCreator != null)
                {
                            strTo += userCreator.Email;
                        }
                    }

            if (this.EmailType == "EM09")
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(this.DocumentID);
                IList<DocumentAttachment> documentAttachments = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(this.DocumentID);
                if (documentAttachments.Count > 0)
                {
                    SuUser receiver = QueryProvider.SuUserQuery.FindByIdentity(document.ReceiverID.Userid);
                    ctlCC.Text = receiver.Email;

                    SuUser approver = QueryProvider.SuUserQuery.FindByIdentity(document.ApproverID.Userid);
                    ctlCC.Text += ";" + approver.Email;
                }
            }

            if (this.EmailType == "EM10")
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(this.DocumentID);
                //IList<DocumentAttachment> documentAttachments = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(this.DocumentID);
                if (document != null)
                {
                    SuUser approver = QueryProvider.SuUserQuery.FindByIdentity(document.ApproverID.Userid);
                    ctlCC.Text += approver.Email;
                }
                
            }
            if (this.EmailType == "EM15")
            {
                SCGDocument document = ScgeAccountingQueryProvider.SCGDocumentQuery.FindByIdentity(this.DocumentID);
                //IList<DocumentAttachment> documentAttachments = ScgeAccountingQueryProvider.DocumentAttachmentQuery.GetDocumentAttachmentByDocumentID(this.DocumentID);
                if (document != null)
                {
                    SuUser approver = QueryProvider.SuUserQuery.FindByIdentity(document.ApproverID.Userid);
                    ctlCC.Text += approver.Email;
                }

            }
            ctlTo.Text = strTo;
            ctlUpdatePanelEmailLog.Update();
            this.ModalPopupExtender1.Show();
        }
        public void Hide()
        {
            //reset control
            ctlCC.Text = string.Empty;
            ctlRemark.Text = string.Empty;

            this.ModalPopupExtender1.Hide();
        }
        #endregion

        #region Event button
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            Hide();
        }
        protected void ctlSend_Click(object sender, ImageClickEventArgs e)
        {
            //เรียก service สำหรับส่ง email
            SS.Standard.WorkFlow.DTO.WorkFlow workFlow = new SS.Standard.WorkFlow.DTO.WorkFlow();
            long workFlowID = 0;
            if (this.RequesterID != 0)
            {
                workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(this.DocumentID);
                if (workFlow != null)
                {
                    try
                    {
                        workFlowID = workFlow.WorkFlowID;
                        if (this.EmailType.Equals("EM09"))
                            SCGEmailService.SendEmailEM09(workFlow.Document.DocumentID, ctlCC.Text, ctlRemark.Text, string.Empty);
                        else if (this.EmailType.Equals("EM10"))
                            SCGEmailService.SendEmailEM10(this.AdvanceDocumentID, UIHelper.ParseLong(ctlUserID.Text), ctlCC.Text, ctlRemark.Text, false);
                        else if (this.EmailType.Equals("EM15"))
                            SCGEmailService.SendEmailEM15(workFlowID, UIHelper.ParseLong(ctlUserID.Text));

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "SendEmailSuccess", "alert('" + GetMessage("Send Complete") + "');", true);
                        Hide();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

        }
        #endregion
    }
}