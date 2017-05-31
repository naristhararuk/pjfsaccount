using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SS.DB.Query;
using System.Text;
using SS.DB.DTO;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ServiceTeam : BasePage
    {
        #region Properties
        public IDbServiceTeamService DbServiceTeamService { get; set; }
        public IDbServiceTeamLocationService DbServiceTeamLocationService { get; set; }
        public IDbLocationQuery DbLocationQuery {get; set;}
        #endregion

        #region Page_load
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "ServiceTeam";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlLocationLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlLocationLookup_OnObjectLookUpReturn);
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }       
        #endregion

        #region ServiceTeam Gridview Event
        protected void ctlServiceTeamGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ServiceTeamEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short serviceTeamId = Convert.ToInt16(ctlServiceTeamGrid.DataKeys[rowIndex].Value);
                ctlServiceTeamForm.PageIndex = (ctlServiceTeamGrid.PageIndex * ctlServiceTeamGrid.PageSize) + rowIndex;
                ctlServiceTeamForm.ChangeMode(FormViewMode.Edit);
                IList<DbServiceTeam> serviceTeamList = new List<DbServiceTeam>();
                DbServiceTeam serviceTeam = DbServiceTeamService.FindByIdentity(serviceTeamId);
                serviceTeamList.Add(serviceTeam);
                ctlServiceTeamForm.DataSource = serviceTeamList;
                ctlServiceTeamForm.DataBind();

                ctlUpdatePanelServiceTeamForm.Update();
                ctlServiceTeamModalPopupExtender.Show();
            }
            if (e.CommandName.Equals("ServiceTeamDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long serviceTeamId = UIHelper.ParseLong(ctlServiceTeamGrid.DataKeys[rowIndex].Value.ToString());
                    DbServiceTeam serviceTeam = ScgDbQueryProvider.DbServiceTeamQuery.FindByIdentity(serviceTeamId);
                    DbServiceTeamService.Delete(serviceTeam);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlServiceTeamGrid.DataCountAndBind();
                    }
                }
                ctlServiceTeamGrid.DataCountAndBind();
                ctlUpdatePanelServiceTeamGridview.Update();
            }
            if (e.CommandName.Equals("LocationEdit"))
            {
                
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                ctlServiceTeamGrid.SelectedIndex = rowIndex;
                long serviceTeamId = UIHelper.ParseShort(ctlServiceTeamGrid.DataKeys[rowIndex].Value.ToString());
                ctlServiceTeamIDHidden.Value = serviceTeamId.ToString();

                ctlLocationGrid.DataCountAndBind();

                lblLocationHeader.Visible = true;
                ctlLocationGrid.Visible = true;
                ctlLocationTools.Visible = true;
                ctlAddLocation.Visible = true;
                ctlCloseLocation.Visible = true;
                ctlUpdatePanelServiceTeamGridview.Update();
                ctlUpdatePanelLocationGrid.Update();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbServiceTeam serviceTeam = GetServiceTeamCriteria();

            return ScgDbQueryProvider.DbServiceTeamQuery.GetServiceTeamList(serviceTeam, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            DbServiceTeam serviceTeam = GetServiceTeamCriteria();

            int count = ScgDbQueryProvider.DbServiceTeamQuery.CountServiceTeamByCriteria(serviceTeam);

            return count;
        }
        public DbServiceTeam GetServiceTeamCriteria()
        {
            DbServiceTeam serviceTeam = new DbServiceTeam();
            serviceTeam.ServiceTeamCode = ctlServiceTeamCodeCri.Text;
            serviceTeam.Description = ctlDescriptionCri.Text;
            return serviceTeam;
        }
        #endregion

        #region Button Event
        protected void ctlServiceTeamSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlServiceTeamGrid.DataCountAndBind();
        }

        protected void ctlServiceTeamAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlServiceTeamModalPopupExtender.Show();
            ctlServiceTeamForm.ChangeMode(FormViewMode.Insert);
            ctlUpdatePanelServiceTeamForm.Update();
        }
        #endregion

        #region ServiceTeam Form Event
        protected void ctlServiceTeamForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlServiceTeamForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlServiceTeamGrid.DataCountAndBind();
                CloseServiceTeamPopUp();
            }
        }
        protected void ctlServiceTeamForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlServiceTeamCode = (TextBox)ctlServiceTeamForm.FindControl("ctlServiceTeamCode");
            TextBox ctlDescription = (TextBox)ctlServiceTeamForm.FindControl("ctlDescription");
            CheckBox ctlActiveChk = (CheckBox)ctlServiceTeamForm.FindControl("ctlActiveChk");

            DbServiceTeam serviceTeam = new DbServiceTeam();
            serviceTeam.ServiceTeamCode  = ctlServiceTeamCode.Text;
            serviceTeam.Description  = ctlDescription.Text;
            serviceTeam.Active = ctlActiveChk.Checked;
            serviceTeam.CreBy = UserAccount.UserID;
            serviceTeam.CreDate = DateTime.Now;
            serviceTeam.UpdBy = UserAccount.UserID;
            serviceTeam.UpdDate = DateTime.Now;
            serviceTeam.UpdPgm = ProgramCode;

            try
            {
                DbServiceTeamService.AddServiceTeam(serviceTeam);
                ctlServiceTeamGrid.DataCountAndBind();
                ctlServiceTeamForm.ChangeMode(FormViewMode.ReadOnly);
                CloseServiceTeamPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlServiceTeamForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short serviceTeamId = Convert.ToInt16(ctlServiceTeamForm.DataKey.Value);
            TextBox ctlServiceTeamCode = (TextBox)ctlServiceTeamForm.FindControl("ctlServiceTeamCode");
            TextBox ctlDescription = (TextBox)ctlServiceTeamForm.FindControl("ctlDescription");
            CheckBox ctlActiveChk = (CheckBox)ctlServiceTeamForm.FindControl("ctlActiveChk");

            DbServiceTeam serviceTeam = DbServiceTeamService.FindByIdentity(serviceTeamId);
            serviceTeam.ServiceTeamCode = ctlServiceTeamCode.Text;
            serviceTeam.Description  = ctlDescription.Text;

            serviceTeam.Active = ctlActiveChk.Checked;
            serviceTeam.CreBy = UserAccount.UserID;
            serviceTeam.CreDate = DateTime.Now;
            serviceTeam.UpdBy = UserAccount.UserID;
            serviceTeam.UpdDate = DateTime.Now;
            serviceTeam.UpdPgm = ProgramCode;

            try
            {
                DbServiceTeamService.UpdateServiceTeam(serviceTeam);
                ctlServiceTeamGrid.DataCountAndBind();
                ctlServiceTeamForm.ChangeMode(FormViewMode.ReadOnly);
                CloseServiceTeamPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlServiceTeamForm_DataBound(object sender, EventArgs e)
        {
            if (ctlServiceTeamForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlServiceTeamCode = (TextBox)ctlServiceTeamForm.FindControl("ctlServiceTeamCode");
                ctlServiceTeamCode.Focus();
                CheckBox ctlActiveCheck = (CheckBox)ctlServiceTeamForm.FindControl("ctlActiveChk");

                if (ctlServiceTeamForm.CurrentMode == FormViewMode.Edit)
                {
                    short serviceTeamID = UIHelper.ParseShort(ctlServiceTeamForm.DataKey.Value.ToString());
                    DbServiceTeam serviceTeam = DbServiceTeamService.FindByIdentity(serviceTeamID);
                }
                if (ctlServiceTeamForm.CurrentMode == FormViewMode.Insert)
                {
                    ctlActiveCheck.Checked = true;
                }
            }
        }
        #endregion

        #region Location Gridview Event
        protected void ctlLocationGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlLocationGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                ctlDeleteLocation.Visible = true;
                ctlDeleteLocation.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlLocationGrid.ClientID);
            }
            else
            {
                ctlDeleteLocation.Visible = false;
            }
        }
        public Object RequestData_ctlLocationGrid(int startRow, int pageSize, string sortExpression)
        {
            DbServiceTeam serviceTeam = GetServiceTeamForLocationGrid();

            return ScgDbQueryProvider.DbServiceTeamLocationQuery.GetServiceTeamLocationList(serviceTeam, startRow, pageSize, sortExpression);
        }

        public int RequestCount_ctlLocationGrid()
        {
            DbServiceTeam serviceTeam = GetServiceTeamForLocationGrid();

            int count = ScgDbQueryProvider.DbServiceTeamLocationQuery.CountServiceTeamLocationByServiceTeamID(serviceTeam);

            return count;
        }
        public DbServiceTeam GetServiceTeamForLocationGrid()
        {
            DbServiceTeam serviceTeam = ScgDbQueryProvider.DbServiceTeamQuery.FindByIdentity(Convert.ToInt64 (ctlServiceTeamIDHidden.Value));
            return serviceTeam;
        }
        #endregion

        #region Location Button Event
        protected void ctlLocationLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<DbLocation> locationList = (IList<DbLocation>)e.ObjectReturn;
            IList<DbServiceTeamLocation> serviceTeamLocationList = new List<DbServiceTeamLocation>();
            
            foreach (DbLocation location in locationList)
            {
                Location locate = new Location();
                locate.LocationID = location.LocationID;
                DbServiceTeam serviceTeam = ScgDbQueryProvider.DbServiceTeamQuery.FindByIdentity(Convert.ToInt64(ctlServiceTeamIDHidden.Value));
                DbServiceTeamLocation serviceTeamLocation = GetServiceTeamLocation(serviceTeam, locate);
                serviceTeamLocationList.Add(serviceTeamLocation);
            }
           
            if (serviceTeamLocationList.Count > 0)
            {
                try
                {
                    DbServiceTeamLocationService.AddServiceTeamLocationList(serviceTeamLocationList);
                    ctlLocationGrid.DataCountAndBind();
                }
                catch (ServiceValidationException ex)
                {
                    ValidationErrors.MergeErrors(ex.ValidationErrors);
                }
            }
            ctlUpdatePanelLocationGrid.Update();
        }
        protected void ctlAddLocation_Click(object sender, ImageClickEventArgs e)
        {
            ctlLocationLookup.isMultiple = true;
            ctlLocationLookup.Show();
            //ctlLocationLookup.isMultiple = true;
            //ctlLocationLookup.Show();
        }
        protected void ctlDeleteLocation_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlLocationGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long serviceTeamLocationId = UIHelper.ParseLong(ctlLocationGrid.DataKeys[row.RowIndex]["ServiceTeamLocationID"].ToString());
                        DbServiceTeamLocation serviceTeamLocation = ScgDbQueryProvider.DbServiceTeamLocationQuery.FindByIdentity(serviceTeamLocationId);
                        DbServiceTeamLocationService.Delete(serviceTeamLocation);
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);

                            ctlUpdatePanelLocationGrid.Update();
                        }
                    }
                }
            }
            ctlLocationGrid.DataCountAndBind();
            ctlUpdatePanelLocationGrid.Update();
        }
        protected void ctlCloseLocation_Click(object sender, ImageClickEventArgs e)
        {
            LocationGridViewClose();
        }
        #endregion

        #region Public Function
        public void CloseServiceTeamPopUp()
        {
            ctlServiceTeamModalPopupExtender.Hide();
            ctlServiceTeamForm.ChangeMode(FormViewMode.ReadOnly);
            ctlUpdatePanelServiceTeamGridview.Update();
        }
        public void LocationGridViewClose()
        {
            lblLocationHeader.Visible = false;
            ctlLocationGrid.Visible = false;
            ctlLocationGrid.DataSource = null;
            ctlLocationGrid.DataBind();
            ctlUpdatePanelLocationGrid.Update();
            ctlCloseLocation.Visible = false;
            ctlAddLocation.Visible = false;
            ctlDeleteLocation.Visible = false;
            ctlLocationTools.Visible = false;
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlLocationGrid.ClientID + "', '" + ctlLocationGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        private DbServiceTeamLocation  GetServiceTeamLocation(DbServiceTeam  serviceTeam,Location location)
        {
            DbServiceTeamLocation serviceTeamLocation = new DbServiceTeamLocation();

            serviceTeamLocation.ServiceTeamID = new DbServiceTeam(serviceTeam.ServiceTeamID);
            serviceTeamLocation.LocationID = new DbLocation(location.LocationID.Value);
            serviceTeamLocation.Active = true;
            serviceTeamLocation.CreBy = UserAccount.UserID;
            serviceTeamLocation.CreDate = DateTime.Now;
            serviceTeamLocation.UpdBy = UserAccount.UserID;
            serviceTeamLocation.UpdDate = DateTime.Now;
            serviceTeamLocation.UpdPgm = ProgramCode;

            return serviceTeamLocation;
        }
        #endregion
    }
}
