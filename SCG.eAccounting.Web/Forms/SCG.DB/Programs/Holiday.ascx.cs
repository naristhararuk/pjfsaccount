using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
using SCG.DB.BLL;
using Spring.Validation;
using SCG.eAccounting.Web.UserControls;


namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class Holiday : BaseUserControl
    {
        public IDbHolidayProfileService DbHolidayProfileService { get; set; }
        public IDbHolidayService DbHolidayService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.AdvanceTypeForm.Equals("1"))
            {
                Mode = "DM";
            }
            else
            {
                Mode = "FR";
            }
            ctlHolidayEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlHolidayEditor.Notify_Cancle += new EventHandler(RefreshGridData);
            if (!Page.IsPostBack)
            {
                ctlHolidayProfileGrid.DataCountAndBind();
            }
        }
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        //public void Initialize(string mode)
        //{
        //    Mode = mode;
        //}
        public string AdvanceTypeForm
        {
            get
            {
                if (ViewState["AdvanceTypeForm"] != null)
                    return ViewState["AdvanceTypeForm"].ToString();
                else
                    return string.Empty;
            }
            set { ViewState["AdvanceTypeForm"] = value; }
        }
        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlUpdatePanelHolidayProfileGridview.Update();
            ctlUpdatePanelHolidayGrid.Update();
            ctlHolidayEditor.HidePopUp();
            ctlHolidayGrid.DataCountAndBind();

        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlHolidayProfileGrid.DataCountAndBind();
            HolidayGridViewClose();
        }
        public void CloseHolidayProfilePopUp()
        {
            ModalPopupExtender1.Hide();
            ModalPopupExtender2.Hide();
            ctlHolidayProfileForm.ChangeMode(FormViewMode.ReadOnly);
            ctlUpdatePanelHolidayProfileGridview.Update();
        }
        protected void ctlHolidayProfileForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlHolidayProfileGrid.DataCountAndBind();
                CloseHolidayProfilePopUp();
            }
        }
        protected void ctlHolidayProfileForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            int holidayprofileid = Convert.ToInt32(ctlHolidayProfileForm.DataKey.Value);
            CheckBox ctlApprove = (CheckBox)ctlHolidayProfileForm.FindControl("ctlApprove");
            DbHolidayProfile holidayProfile = DbHolidayProfileService.FindByIdentity(holidayprofileid);
            holidayProfile.IsApprove = ctlApprove.Checked;
            holidayProfile.Type = Mode;
            holidayProfile.CreBy = UserAccount.UserID;
            holidayProfile.CreDate = DateTime.Now;
            holidayProfile.UpdBy = UserAccount.UserID;
            holidayProfile.UpdDate = DateTime.Now;
            holidayProfile.UpdPgm = ProgramCode;
            try
            {
                DbHolidayProfileService.UpdateHolidayProfile(holidayProfile);
                ctlHolidayProfileGrid.DataCountAndBind();
                ctlHolidayProfileForm.ChangeMode(FormViewMode.ReadOnly);
                CloseHolidayProfilePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlHolidayProfileForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlHolidayProfileForm_DataBound(object sender, EventArgs e)
        {
            if (ctlHolidayProfileForm.CurrentMode != FormViewMode.ReadOnly)
            {
                if (ctlHolidayProfileForm.CurrentMode == FormViewMode.Edit)
                {
                    Label ctlYear = ctlHolidayProfileForm.FindControl("ctlYear") as Label;
                    CheckBox ctlApprove = (CheckBox)ctlHolidayProfileForm.FindControl("ctlApprove");
                    int holidayProfileId = UIHelper.ParseInt(ctlHolidayProfileForm.DataKey.Value.ToString());
                    DbHolidayProfile holidayProfile = DbHolidayProfileService.FindByIdentity(holidayProfileId);
                    ctlYear.Text = holidayProfile.Year.ToString();
                    ctlApprove.Checked = holidayProfile.IsApprove;
                }
                if (ctlHolidayProfileForm.CurrentMode == FormViewMode.Insert)
                {
                    DropDownList ctlYear = ctlHolidayProfileForm.FindControl("ctlYear") as DropDownList;
                    CheckBox ctlApprove = (CheckBox)ctlHolidayProfileForm.FindControl("ctlApprove");
                    ctlYear.Focus();
                    IList<int> year = new List<int>();
                    int countfail = 0;
                    for (int i = 0; i < 5 + countfail; i++)
                    {
                        bool checkYear = true;
                        foreach (int years in ScgDbQueryProvider.DbHolidayProfileQuery.GetYear(Mode))
                        {
                            if (years == DateTime.Now.Year + i)
                            {
                                checkYear = false;
                            }

                        }
                        if (checkYear)
                            year.Add(DateTime.Now.Year + i);
                        else
                            countfail++;
                    }
                    ctlYear.DataSource = year;
                    ctlYear.DataBind();
                    ctlApprove.Checked = true;
                }
            }
        }
        protected void ctlHolidayProfileForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            DropDownList ctlYear = (DropDownList)ctlHolidayProfileForm.FindControl("ctlYear");
            CheckBox ctlApprove = (CheckBox)ctlHolidayProfileForm.FindControl("ctlApprove");
            DbHolidayProfile holidayProfile = new DbHolidayProfile();
            holidayProfile.Year = UIHelper.ParseInt(ctlYear.SelectedValue);
            holidayProfile.IsApprove = ctlApprove.Checked;
            holidayProfile.Type = Mode;
            holidayProfile.CreBy = UserAccount.UserID;
            holidayProfile.CreDate = DateTime.Now;
            holidayProfile.UpdBy = UserAccount.UserID;
            holidayProfile.UpdDate = DateTime.Now;
            holidayProfile.UpdPgm = ProgramCode;

            try
            {
                DbHolidayProfileService.AddHolidayProfile(holidayProfile);
                ctlHolidayProfileGrid.DataCountAndBind();
                CloseHolidayProfilePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        public void HolidayGridViewClose()
        {
            ctlHolidayGrid.Visible = false;
            ctlHolidayGrid.DataSource = null;
            ctlHolidayGrid.DataBind();
            ctlUpdatePanelHolidayGrid.Update();
            ctlHolidayTools.Visible = false;
        }
        protected void ctlHolidayProfileGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("HolidayProfileEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                int holidayProfileId = Convert.ToInt32(ctlHolidayProfileGrid.DataKeys[rowIndex].Values["Id"]);
                ctlHolidayProfileForm.PageIndex = (ctlHolidayProfileGrid.PageIndex * ctlHolidayProfileGrid.PageSize) + rowIndex;
                ctlHolidayProfileForm.ChangeMode(FormViewMode.Edit);
                IList<DbHolidayProfile> holidayProfileList = new List<DbHolidayProfile>();
                DbHolidayProfile holidayProfile = DbHolidayProfileService.FindByIdentity(holidayProfileId);
                holidayProfileList.Add(holidayProfile);
                ctlHolidayProfileForm.DataSource = holidayProfileList;
                ctlHolidayProfileForm.DataBind();
                ctlUpdatePanelHolidayProfileForm.Update();
                ModalPopupExtender1.Show();
            }
            if (e.CommandName.Equals("HolidayProfileDetail"))
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int holidayProfileId = Convert.ToInt32(ctlHolidayProfileGrid.DataKeys[rowIndex].Values["Id"]);
                string holidayProfileYear = ((Literal)ctlHolidayProfileGrid.Rows[rowIndex].FindControl("ctlYearLabel")).Text;
                ctlHolidayProfileGrid.SelectedIndex = rowIndex;
                ctlHolidayProfileIDHidden.Value = holidayProfileId.ToString();
                ctlHolidayProfileYearHidden.Value = holidayProfileYear.ToString();
                ctlHolidayGrid.DataCountAndBind();
                ctlHolidayGrid.Visible = true;
                ctlHolidayTools.Visible = true;
                ctlUpdatePanelHolidayProfileGridview.Update();
                ctlUpdatePanelHolidayGrid.Update();
            }
            if (e.CommandName.Equals("HolidayProfileDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    Int32 holidayProfileId = UIHelper.ParseInt(ctlHolidayProfileGrid.DataKeys[rowIndex].Value.ToString());
                    DbHolidayProfile holidayProfile = ScgDbQueryProvider.DbHolidayProfileQuery.FindByIdentity(holidayProfileId);
                    DbHolidayService.DeleteHolidayProfile(holidayProfileId);
                    DbHolidayProfileService.Delete(holidayProfile);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                    }
                    ctlHolidayProfileGrid.DataCountAndBind();
                }
                ctlHolidayGrid.DataCountAndBind();
                ctlHolidayProfileGrid.DataCountAndBind();
                ctlUpdatePanelHolidayProfileGridview.Update();
            }
        }
        protected void ctlHolidayProfileGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlHolidayProfileGrid_DataBound(object sender, EventArgs e)
        {
        }
        protected void ctlHolidayProfileGrid_PageIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Show();
            ctlHolidayProfileForm.ChangeMode(FormViewMode.Insert);
            ctlUpdatePanelHolidayProfileForm.Update();
        }
        protected void ctlCopy_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Show();
            ctlCopyForm.ChangeMode(FormViewMode.Insert);
            ctlUpdatePanelCopyForm.Update();
            ctlCopyForm.DataBind();
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            int? year = UIHelper.ParseInt(ctlYear.Text);
            sortExpression = "Year";
            return ScgDbQueryProvider.DbHolidayProfileQuery.GetHolidayProfile(year, startRow, pageSize, sortExpression, Mode);
        }
        public int RequestCount()
        {
            int? year = UIHelper.ParseInt(ctlYear.Text);
            return ScgDbQueryProvider.DbHolidayProfileQuery.CountHolidayProfile(year, Mode);
        }
        protected void ctlCopyForm_DataBound(object sender, EventArgs e)
        {
            if (ctlCopyForm.CurrentMode != FormViewMode.ReadOnly)
            {
                if (ctlCopyForm.CurrentMode == FormViewMode.Insert)
                {
                    DropDownList ctlYearFrom = ctlCopyForm.FindControl("ctlYearFrom") as DropDownList;
                    DropDownList ctlYearTo = ctlCopyForm.FindControl("ctlYearTo") as DropDownList;
                    ctlYearTo.Focus();
                    IList<int> yearFrom = new List<int>();
                    foreach (int year in ScgDbQueryProvider.DbHolidayProfileQuery.GetYear(Mode))
                        yearFrom.Add(year);
                    IList<int> yearTo = new List<int>();
                    int countfail = 0;
                    for (int i = 0; i <= 4 + countfail; i++)
                    {
                        bool checkYear = true;
                        foreach (int year in ScgDbQueryProvider.DbHolidayProfileQuery.GetYear(Mode))
                        {
                            if (year == DateTime.Now.Year + i)
                            {
                                checkYear = false;
                            }
                        }
                        if (checkYear)
                            yearTo.Add(DateTime.Now.Year + i);
                        else
                            countfail++;
                    }
                    ctlYearFrom.DataSource = yearFrom;
                    ctlYearFrom.DataBind();
                    ctlYearTo.DataSource = yearTo;
                    ctlYearTo.DataBind();
                }
            }
        }
        protected void ctlCopyForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlHolidayProfileGrid.DataCountAndBind();
                CloseHolidayProfilePopUp();
            }
        }
        protected void ctlCopyForm_ItemInserting(object sender, EventArgs e)
        {
            DropDownList ctlYearFrom = (DropDownList)ctlCopyForm.FindControl("ctlYearFrom");
            DropDownList ctlYearTo = (DropDownList)ctlCopyForm.FindControl("ctlYearTo");
            int YearFrom = UIHelper.ParseInt(ctlYearFrom.SelectedValue);
            int YearTo = UIHelper.ParseInt(ctlYearTo.SelectedValue);
            try
            {
                DbHolidayProfileService.Copy(YearFrom, YearTo,Mode);
                ctlHolidayProfileGrid.DataCountAndBind();
                CloseHolidayProfilePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlCopyForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        public Object RequestHolidayData(int startRow, int pageSize, string sortExpression)
        {
            int holidayProfileId = UIHelper.ParseInt(ctlHolidayProfileIDHidden.Value);
            sortExpression = "Date";
            return ScgDbQueryProvider.DbHolidayQuery.GetHoliday(holidayProfileId, startRow, pageSize, sortExpression);
        }
        public int RequestHolidayCount()
        {
            int holidayProfileId = UIHelper.ParseInt(ctlHolidayProfileIDHidden.Value);
            return ScgDbQueryProvider.DbHolidayQuery.CountHoliday(holidayProfileId);
        }
        protected void ctlHolidayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlHolidayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("HolidayDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    Int32 holidayId = UIHelper.ParseInt(ctlHolidayGrid.DataKeys[rowIndex].Value.ToString());
                    DbHoliday holiday = ScgDbQueryProvider.DbHolidayQuery.FindByIdentity(holidayId);
                    DbHolidayService.Delete(holiday);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                    }
                    ctlHolidayGrid.DataCountAndBind();
                }
                ctlHolidayGrid.DataCountAndBind();
                ctlUpdatePanelHolidayProfileGridview.Update();
            }
        }
        protected void ctlHolidayGrid_DataBound(object sender, EventArgs e)
        {

        }
        protected void ctlHolidayGrid_PageIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ctlAddHoliday_Click(object sender, ImageClickEventArgs e)
        {
            int holidayProfileId = UIHelper.ParseInt(ctlHolidayProfileIDHidden.Value);
            int holidayProfileYear = UIHelper.ParseInt(ctlHolidayProfileYearHidden.Value);
            ctlHolidayEditor.Initialize(FlagEnum.NewFlag, 0, holidayProfileId, holidayProfileYear);
            ctlHolidayEditor.ShowPopUp();
            ctlHolidayGrid.DataCountAndBind();
        }
    }
}