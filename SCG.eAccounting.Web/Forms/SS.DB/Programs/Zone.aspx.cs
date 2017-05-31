using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.Standard.Utilities;
using SCG.eAccounting.Web.Helper;
using SS.DB.BLL;
using System.Text;

namespace SCG.eAccounting.Web.Forms.SS.DB.Programs
{
    public partial class Zone : BasePage
    {
        public IDbZoneService DbZoneService { get; set; }
        public IDbZoneLangService DbZoneLangService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }
 

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlZoneGridView_PageIndexChanged(object sender, EventArgs e)
        {
            ZoneLangGridViewFinish();
        }

        protected void ctlZoneGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("UserEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short zoneId = UIHelper.ParseShort(ctlZoneGridView.DataKeys[rowIndex].Value.ToString());
                ctlZoneForm.PageIndex = (ctlZoneGridView.PageIndex * ctlZoneGridView.PageSize) + rowIndex;
                ctlZoneForm.ChangeMode(FormViewMode.Edit);
                IList<DbZoneResult> zoneList =  SsDbQueryProvider.DbZoneQuery.FindZoneByID(zoneId,UserAccount.CurrentLanguageID);
                
                ctlZoneForm.DataSource = zoneList;
                ctlZoneForm.DataBind();
                ctlZoneGridView.DataCountAndBind();
                ctlZoneFormUpdatePanel.Update();
                ctlZoneModalPopupExtender.Show();
                ZoneLangGridViewFinish();
            }
            if (e.CommandName.Equals("Select"))
            {

                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                short zoneId = UIHelper.ParseShort(ctlZoneGridView.DataKeys[rowIndex].Value.ToString());
                ctlZoneLangGrid.DataSource = DbZoneLangService.FindZoneLangByZoneID(zoneId);
                ctlZoneLangGrid.DataBind();
                ctlZoneLangButton.Visible = true;
                ctlZoneLangGrid.Visible = true;
                ctlZoneLangFds.Visible = true;
                ctlZoneGridView.DataCountAndBind();
                ctlZoneUpdatePanel.Update();
                ctlZoneLangUpdatePanel.Update();
            }
        }

        protected void ctlZoneGridView_DataBound(object sender, EventArgs e)
        {
            if (ctlZoneGridView.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                divButton.Visible = true;
                ctlZoneDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlZoneGridView.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }

        protected void ctlZoneGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return SsDbQueryProvider.DbZoneQuery.GetZoneList(UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            int count = SsDbQueryProvider.DbZoneQuery.CountZoneByCriteria();

            return count;
        }

        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlZoneModalPopupExtender.Show();
            ctlZoneForm.ChangeMode(FormViewMode.Insert);
            ctlZoneGridView.DataCountAndBind();
            ctlZoneFormUpdatePanel.Update();
        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlZoneGridView.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelectChk")).Checked))
                {
                    try
                    {

                        short zoneId = UIHelper.ParseShort(ctlZoneGridView.DataKeys[row.RowIndex].Value.ToString());
                        DbZone zone = SsDbQueryProvider.DbZoneQuery.FindProxyByIdentity(zoneId);
                        DbZoneService.Delete(zone);
                        ZoneLangGridViewFinish();
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                        }
                    }
                }
            }
            ctlZoneGridView.DataCountAndBind();
            ctlZoneUpdatePanel.Update();
        }

        protected void ctlZoneLangGrid_DataBound(object sender, EventArgs e)
        {

        }

        protected void ctlZoneForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel"))
            {
                CloseZonePopUp();
            }
        }

        protected void ctlZoneForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlZoneName = (TextBox)ctlZoneForm.FindControl("ctlZoneName");

            DbZoneLang zoneLang = new DbZoneLang();
            DbZone zone = new DbZone();

            GetZoneInfo(zone,"Insert");
            zoneLang.Zone = zone;
            zoneLang.Language = DbLanguageService.FindByIdentity(UIHelper.ParseShort(UserAccount.CurrentLanguageID.ToString()));
            zoneLang.ZoneName = ctlZoneName.Text;
            zoneLang.CreBy = UserAccount.UserID;
            zoneLang.CreDate = DateTime.Now;
            zoneLang.UpdBy = UserAccount.UserID;
            zoneLang.UpdDate = DateTime.Now;
            zoneLang.UpdPgm = ProgramCode;
            zoneLang.Active = true;

            try
            {
                DbZoneLangService.AddZoneLang(zone,zoneLang);
                ctlZoneGridView.DataCountAndBind();
                ctlZoneForm.ChangeMode(FormViewMode.ReadOnly);
                CloseZonePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }

        protected void ctlZoneForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short zoneId = UIHelper.ParseShort(ctlZoneForm.DataKey["ZoneID"].ToString());
            short zoneLangId = UIHelper.ParseShort(ctlZoneForm.DataKey["ZoneLangID"].ToString());

            DbZone zone = DbZoneService.FindByIdentity(zoneId);
            GetZoneInfo(zone,"Update");
            try
            {
                DbZoneLangService.UpdateZoneLang(zone);
                ctlZoneGridView.DataCountAndBind();
                ctlZoneForm.ChangeMode(FormViewMode.ReadOnly);
                CloseZonePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }

        protected void ctlZoneForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }

        protected void ctlZoneForm_DataBound(object sender, EventArgs e)
        {
            if (ctlZoneForm.CurrentMode == FormViewMode.Insert)
            {
                TextBox ctlZoneName = ctlZoneForm.FindControl("ctlZoneName") as TextBox;
                ctlZoneName.Focus();
            }
        }

        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            IList<DbZoneLang> zoneLangList = new List<DbZoneLang>();

            DbZone zone = new DbZone(UIHelper.ParseShort(ctlZoneGridView.SelectedValue.ToString()));
            foreach (GridViewRow row in ctlZoneLangGrid.Rows)
            {
                TextBox ctlZoneName = (TextBox)ctlZoneLangGrid.Rows[row.RowIndex].FindControl("ctlZoneName");
                TextBox ctlComment = (TextBox)ctlZoneLangGrid.Rows[row.RowIndex].FindControl("ctlComment");
                CheckBox ctlActiveChk = (CheckBox)ctlZoneLangGrid.Rows[row.RowIndex].FindControl("ctlActive");
                if (!string.IsNullOrEmpty(ctlZoneName.Text) || !string.IsNullOrEmpty(ctlComment.Text))
                {
                    DbZoneLang zoneLang = new DbZoneLang();
                    DbLanguage Lang = new DbLanguage(UIHelper.ParseShort(ctlZoneLangGrid.DataKeys[row.RowIndex].Value.ToString()));
                    zoneLang.Language = Lang;
                    zoneLang.Zone = zone;
                    zoneLang.ZoneName = ctlZoneName.Text;
                    zoneLang.Comment = ctlComment.Text;
                    zoneLang.Active = ctlActiveChk.Checked;
                    zoneLang.CreBy = UserAccount.UserID;
                    zoneLang.CreDate = DateTime.Now;
                    zoneLang.UpdBy = UserAccount.UserID;
                    zoneLang.UpdDate = DateTime.Now;
                    zoneLang.UpdPgm = ProgramCode;
                    zoneLangList.Add(zoneLang);
                }
            }
            DbZoneLangService.UpdateZoneLang(zoneLangList);
            ctlMessage.Message = GetMessage("SaveSuccessFully");
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ZoneLangGridViewFinish();
        }


        public void ZoneLangGridViewFinish()
        {
            ctlZoneLangGrid.DataSource = null;
            ctlZoneLangGrid.DataBind();
            ctlZoneLangUpdatePanel.Update();
            ctlZoneLangFds.Visible = false;
            ctlZoneLangButton.Visible = false;
            ctlZoneGridView.SelectedIndex = -1;
        }
        public void CloseZonePopUp()
        {
            ctlZoneModalPopupExtender.Hide();
            ctlZoneUpdatePanel.Update();
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlZoneGridView.ClientID + "', '" + ctlZoneGridView.HeaderRow.FindControl("ctlSelectAllChk").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
       
        public void GetZoneInfo(DbZone zone,string mode)
        {
            TextBox ctlComment = (TextBox)ctlZoneForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlZoneForm.FindControl("ctlActiveChk");

            zone.Comment = ctlComment.Text;
            zone.Active = ctlActiveChk.Checked;
            if (mode.Equals("Insert"))
            {
                zone.CreBy = UserAccount.UserID;
                zone.CreDate = DateTime.Now;
            }
            zone.UpdBy = UserAccount.UserID;
            zone.UpdDate = DateTime.Now;
            zone.UpdPgm = ProgramCode;
        }
    }
}
