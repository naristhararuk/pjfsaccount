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
    public partial class InitiatorEditor : BaseUserControl
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
            ctlInitiatorGrid.DataCountAndBind();
            ctlUpdatePanelInitiator.Update();
            UId = id;
          

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //long uid = UserAccount.UserID;
            ctlInitiatorLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlInitiatorLookup_OnObjectLookUpReturn);
        }
        #region initiator
        protected void ctlAddInitiator_Click(object sender, ImageClickEventArgs e)
        {
            ctlInitiatorLookup.Show();
        }
        protected void ctlDeleteInitiator_Click(object sender, ImageClickEventArgs e)
        {
            ShowAdd.Visible = false;
            foreach (GridViewRow row in ctlInitiatorGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long userId = UIHelper.ParseLong(ctlInitiatorGrid.DataKeys[row.RowIndex].Value.ToString());
                        SuUserFavoriteActor initiator = QueryProvider.SuUserFavoriteActorQuery.FindUserFavoriteByFavoriteId(userId);
                        SuUserFavoriteActorService.DeleteFavorite(initiator);
                        //SuUserFavoriteActor user = SuUserFavoriteActor.FindProxyByIdentity(organizationId);
                        //SuOrganizationService.Delete(organization);
                    }
                    catch (Exception ex)
                    {
                        string exMessage = ex.Message;
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        errors.AddError("DeleteInitiator.Error", new Spring.Validation.ErrorMessage("CannotDelete"));
                        ValidationErrors.MergeErrors(errors);
                    }
                }
            }
            ctlInitiatorGrid.DataCountAndBind();
            ctlUpdatePanelInitiator.Update();

        }
        protected void ctlCloseInitiator_Click(object sender, ImageClickEventArgs e)
        {
            ShowAdd.Visible = false;
            ctlUpdatePanelInitiator.Update();
            //ctlAddApprover.Visible = false;
            //ctlDeleteApprover.Visible = false;
            //ctlApproverClose.Visible = false;
            ctlFieldsetInitiator.Visible = false;
        }

        #endregion
        protected void ctlInitiatorGrid_Databound(object sender, EventArgs e)
        {
            if (ctlInitiatorGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }
        
        public SuUser GetUser()
        {
            return QueryProvider.SuUserQuery.FindByIdentity(UId);
        }
        protected Object RequestDataInitiator(int startRow, int pageSize, string sortExpression)
        {

            return QueryProvider.SuUserFavoriteActorQuery.FindInitiatorByUserId(UId);
        }
        
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlInitiatorGrid.ClientID + "', '" + ctlInitiatorGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        protected void ctlInitiatorLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            ShowAdd.Text = null;
            ShowAdd.Visible = false;
            IList<SuUser> userList = (List<SuUser>)e.ObjectReturn;
            ArrayList groupArrList = new ArrayList();
            //Add Approver to SuUserFavorite
            //.....
            foreach (GridViewRow row in ctlInitiatorGrid.Rows)
            {
                Label ctlInitiator = (Label)ctlInitiatorGrid.Rows[row.RowIndex].FindControl("ctlUserID");

                groupArrList.Add(ctlInitiator.Text);
            }

            SuUser user = GetUser();
            foreach (SuUser u in userList)
            {
                if (!groupArrList.Contains(u.UserName))
                {
                    SuUserFavoriteActor initiator = new SuUserFavoriteActor();
                    initiator.Active = true;
                    initiator.User = user;
                    initiator.ActorType = "2"; //2 = initiator
                    initiator.CreBy = UserAccount.UserID;
                    initiator.CreDate = DateTime.Now;
                    //approver.RowVersion = u.RowVersion;
                    initiator.UpdBy = UserAccount.UserID;
                    initiator.UpdDate = DateTime.Now;
                    initiator.UpdPgm = UserAccount.CurrentProgramCode;
                    initiator.ActorUserID = u;
                    SuUserFavoriteActorService.AddFavoriteInitiator(initiator);
                }
                else
                {
                    ShowAdd.Text += "INITIATOR   '" + u.UserName + "'  HAS BEEN ADDED <br>";
                    ShowAdd.Visible = true;
                
                
                }
        }

            ctlInitiatorGrid.DataCountAndBind();
            ctlUpdatePanelInitiator.Update();
        }
        public void ShowDetail()
        {
            ctlInitiatorGrid.DataCountAndBind();
            ctlFieldsetInitiator.Visible = true;
            ctlUpdatePanelInitiator.Update();
        }
        public void HideDetail()
        {
            ctlFieldsetInitiator.Visible = false;
            ctlUpdatePanelInitiator.Update();
        }
    }
}