using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.Standard.Utilities;
using System.Text;
using SS.SU.DTO.ValueObject;


namespace SCG.eAccounting.Web.UserControls
{
    public partial class PBInfo : BaseUserControl
    {
        public ISuRolepbService SuRolepbService { get; set; }
        //Property for specified Current Role-Managing.
        public short RoleID
        {
            get { return this.ViewState["RoleID"] == null ? (short)0 : (short)this.ViewState["RoleID"]; }
            set { this.ViewState["RoleID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ctlDropDownListPB.PBNameBind();
               // ctlDropDownListPB.PBRoleNameBind(RoleID);
                ctlGridRole.DataCountAndBind();
                ProgramCode = "PbInfo";
                
            }
        }

        public event EventHandler Notify_Ok;
        //public event EventHandler Notify_Cancel;

        #region This pane event
        public void Initialize(short roleID)
        {
            RoleID = roleID;
        }

        public void Hide()
        {
            ctlPanelPBInfo.Visible = false;

        }

        public void Show()
        {
            ctlDropDownListPB.PBRoleNameBind(RoleID);
            ctlPanelPBInfo.Visible = true;
            RefreshGridView();
        }

        private void RefreshGridView()
        {
            ctlGridRole.DataCountAndBind();
            ctlUpdPanelGridView.Update();
        }

        #endregion

        #region Button Event

        protected void Delete_Click(object sender, EventArgs e)
        {
            //list was chcecked.
            //List<SuRolepb> list = new List<SuRolepb>();
            //traversal in GridView
            foreach (GridViewRow row in ctlGridRole.Rows)
            {
                //traversal in a row
                if (((CheckBox)(row.Cells[0].Controls[1])).Checked)
                {
                    int rowIndex = row.RowIndex;
                    short rolePbID = short.Parse(ctlGridRole.DataKeys[rowIndex].Value.ToString());
                    SuRolepb rolePb = new SuRolepb();
                    rolePb.RolePBID = rolePbID;
                    SuRolepbService.DeleteRolepb(rolePb);

                }
            }
            
            RefreshGridView();
            ctlDropDownListPB.PBRoleNameBind(RoleID);
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            //Prepare rolPB DTO for addition.
            SuRolepb rolePb = new SuRolepb();
            rolePb.Active = true;
            rolePb.CreBy = UserAccount.UserID;
            rolePb.PBID = new SCG.DB.DTO.Dbpb();
            rolePb.PBID.Pbid = Convert.ToInt64(ctlDropDownListPB.SelectedValue);
            rolePb.RoleID = new SuRole();
            rolePb.RoleID.RoleID = RoleID;
            rolePb.UpdBy = UserAccount.UserID;
            rolePb.UpdPgm = ProgramCode;

            try
            {
                SuRolepbService.AddRolepb(rolePb);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            finally
            {
                RefreshGridView();
                ctlDropDownListPB.PBRoleNameBind(RoleID);
            }
        }

        protected void ctlButtonClose_Click(object sender, EventArgs e)
        {
            Notify_Ok(sender, e);
            Hide();
        }

        #endregion

        #region GridView
        protected void Grid_DataBound(object sender, EventArgs e)
        {
            if (ctlGridRole.Rows.Count > 0)
            {
                RegisterScriptForGridView();
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SuRole role = new SuRole();
            role.RoleID = RoleID;
            return QueryProvider.SuRolePBQuery.FindPBInfoByRole(role, 0, 0, sortExpression, UserAccount.UserLanguageID);
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");

            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridRole.ClientID + "', '" + ctlGridRole.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion


    }
}