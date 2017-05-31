using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.Query;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO.ValueObject;
using System.Text;
using SS.SU.BLL;
using SS.SU.BLL.Implement;
using SCG.eAccounting.Web.UserControls;
using System.Collections;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ApproverEditor : BaseUserControl
    {
        public ISuUserFavoriteActorService SuUserFavoriteActorService { get; set; }
        public ISuUserService SuUserService { get; set; }
        public long UId
        {
            get { return UIHelper.ParseLong(user.Value); }
            set { user.Value = value.ToString(); }
        }
        public void Initialize(long id)
        {
            ctlApproverEditorGrid.DataCountAndBind();
            ctlApprroverUpdatePanel.Update();
            UId = id;
           
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //long uid = UserAccount.UserID;
            ctlUserProfileLookUp.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlApproverEditorLookup_OnObjectLookUpReturn);
        }
        #region ApproverEditor
        protected void ctlAddApproverEditor_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserProfileLookUp.Show();
        }
        protected void ctlCloseApproverEditor_Click(object sender, ImageClickEventArgs e)
        {
            ctlApprroverUpdatePanel.Update();
            //ctlAddApproverEditor.Visible = false;
            //ctlDeleteApproverEditor.Visible = false;
            //ctlApproverEditorClose.Visible = false;
            HideDetail();
        }
        protected void ctlDeleteApproverEditor_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlApproverEditorGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long userId = UIHelper.ParseLong(ctlApproverEditorGrid.DataKeys[row.RowIndex].Value.ToString());
                        SuUserFavoriteActor ApproverEditor = QueryProvider.SuUserFavoriteActorQuery.FindUserFavoriteByFavoriteId(userId);
                        SuUserFavoriteActorService.DeleteFavorite(ApproverEditor);
                        //SuUserFavoriteActor user = SuUserFavoriteActor.FindProxyByIdentity(organizationId);
                        //SuOrganizationService.Delete(organization);
                    }
                    catch (Exception ex)
                    {
                        string exMessage = ex.Message;
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        errors.AddError("DeleteApproverEditor.Error", new Spring.Validation.ErrorMessage("CannotDelete"));
                        ValidationErrors.MergeErrors(errors);
                    }
                }
            }
            ctlApproverEditorGrid.DataCountAndBind();
            ctlApprroverUpdatePanel.Update();


        }
        #endregion
        protected void ctlApproverEditorGrid_Databound(object sender, EventArgs e)
        {
            //VOUserProfile criteria = GetSuUserCriteria();
            if (ctlApproverEditorGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }

        }
        public SuUser GetUser()
        {
            return QueryProvider.SuUserQuery.FindByIdentity(UId);
        }
        protected Object RequestDataApproverEditor(int startRow, int pageSize, string sortExpression)
        {
            return QueryProvider.SuUserFavoriteActorQuery.FindApproverByUserId(UId);
        }

        //public int RequestCount()
        //{
        //    int count = QueryProvider.SuUserFavoriteActorQuery.CountByCriteria();

        //    return count;
        //}

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlApproverEditorGrid.ClientID + "', '" + ctlApproverEditorGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        protected void ctlApproverEditorLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<SuUser> userList = (List<SuUser>)e.ObjectReturn;
            ArrayList userArrList = new ArrayList();

            foreach (GridViewRow row in ctlApproverEditorGrid.Rows)
            {
                Label ctlUserID = (Label)ctlApproverEditorGrid.Rows[row.RowIndex].FindControl("ctlUserID");

                userArrList.Add(ctlUserID.Text);
            }

            SuUser user = GetUser();
            foreach (SuUser u in userList)
            {
                if (!userArrList.Contains(u.UserName))
                {
                    SuUserFavoriteActor ApproverEditor = new SuUserFavoriteActor();
                    ApproverEditor.Active = true;
                    ApproverEditor.User = user;
                    ApproverEditor.ActorType = "1"; //1 = ApproverEditor
                    ApproverEditor.CreBy = UserAccount.UserID;
                    ApproverEditor.CreDate = DateTime.Now;
                    //ApproverEditor.RowVersion = u.RowVersion;
                    ApproverEditor.UpdBy = UserAccount.UserID;
                    ApproverEditor.UpdDate = DateTime.Now;
                    ApproverEditor.UpdPgm = UserAccount.CurrentProgramCode;
                    ApproverEditor.ActorUserID = u;

                    SuUserFavoriteActorService.AddFavoriteApprover(ApproverEditor);
                }
            }

            ctlApproverEditorGrid.DataCountAndBind();
            ctlApprroverUpdatePanel.Update();
            
        }
        public void ShowDetail()
        {
            ctlApproverEditorGrid.DataCountAndBind();
            ctlFieldSetApproverEditorInfo.Visible = true;
            ctlApprroverUpdatePanel.Update();
        }
        public void HideDetail()
        {
            ctlFieldSetApproverEditorInfo.Visible = false;
            ctlApprroverUpdatePanel.Update();
        }
    }
}