using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.Security;
using SS.Standard.Utilities;

using SS.Standard.WorkFlow;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.EventUserControl;
using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query.Hibernate;

using SS.Standard.UI;
using SCG.eAccounting.BLL;

using SS.Standard.CommunicationService.DTO;
using SS.Standard.CommunicationService.Implement;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web
{
    public partial class ApproveReject : BasePage
    {
        public IWorkFlowResponseTokenQuery WorkFlowResponseTokenQuery { get; set; }
        public IWorkFlowService WorkFlowService { get; set; }

        public IWorkFlowStateEventQuery WorkFlowStateEventQuery { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            string TokenCode = Request.QueryString["TokenCode"] == null ? string.Empty : Request.QueryString["TokenCode"].Trim();
            string From = Request.QueryString["From"] == null ? string.Empty : Request.QueryString["From"].Trim();
            string Type = Request.QueryString["Type"] == null ? string.Empty : Request.QueryString["Type"].Trim();
            int eventID = Request.QueryString["EventID"] == null ? -1 : Int32.Parse(Request.QueryString["EventID"].Trim());

            //1. SignIn by UserName (From)
            string userName = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(From)).UserName;

            UserEngine.SignIn(userName);

            this.Title = "Approval via Email By :" + userName;
            this.ProgramCode = "ApprovalViaEmail";
            UserAccount.CurrentProgramCode = this.ProgramCode;

            //1.1 If cannot SignIn , we not need to call workflow
            if (UserAccount.Authentication)
            {
                try
                {
                    WorkFlowResponseToken token = WorkFlowResponseTokenQuery.GetByTokenCode_WorkFlowStateEventID(TokenCode, eventID);


                    if (token == null)
                    {
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        errors.AddError("General.Error", new Spring.Validation.ErrorMessage(
                            GetMessage("InvalidTokenID")
                            ));
                        throw new ServiceValidationException(errors);
                    }
                    else
                    {
                        string successText = GetMessage("ApproveRejectResultMessage",
                            token.WorkFlow.Document.DocumentNo,
                            WorkFlowStateEventQuery.GetTranslatedEventName(
                                token.WorkFlowStateEvent.WorkFlowStateEventID,
                                UserAccount.CurrentLanguageID));

                        WorkFlowService.NotifyEventFromToken(TokenCode, UserAccount.UserID, eventID, TokenType.Email);

                        ctlResult.Text = successText;
                    }
                }
                catch (ServiceValidationException ex)
                {
                    this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
                finally
                {
                    UserEngine.SignOut(UserAccount.UserID);
                }
            }
            else
            {
                this.ValidationErrors.AddError("General.Error", 
                    new Spring.Validation.ErrorMessage("ApproveRejectResultLoginFail"));
            }
        }
    }
}
