using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.BLL;
using SS.Standard.Utilities;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SS.SU.Query;
using SS.SU.DTO;
using SCG.eAccounting.Web.Forms.SS.DB.Programs;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using SS.DB.DTO;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ForeignPerdiemRateProfileDetailEditor : BaseUserControl
    {
        public IFnPerdiemRateService FnPerdiemRateService { get; set; }
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long PPFID
        {
            get { return this.ViewState["PerdiemProfileID"] == null ? (long)0 : (long)this.ViewState["PerdiemProfileID"]; }
            set { this.ViewState["PerdiemProfileID"] = value; }
        }
        public long RATEID
        {
            get { return this.ViewState["PerdiemRateID"] == null ? (long)0 : (long)this.ViewState["PerdiemRateID"]; }
            set { this.ViewState["PerdiemRateID"] = value; }
        }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        protected void Page_Load(object sender, EventArgs e)
        {
            ProgramCode = "ForeignRateDetailEditor";
        }
        public void ResetValue()
        {
            ctlPersonalLevelDropdown.SelectedValue = null;
            ctlZoneDropdown.SelectedValue = null;
            ctlOfficialRate.Text = string.Empty;
            ctlExtraRate.Text = string.Empty;
            ctlInternationalStaffRate.Text = string.Empty;
            ctlSCGStaffRate.Text = string.Empty;
            ctlActive.Checked = false;
            ctlUpdatePanel.Update();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            FnPerdiemRate fp;

            try
            {
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    fp = ScgeAccountingQueryProvider.FnPerdiemRateQuery.FindByIdentity(RATEID);
                }
                else
                {
                    fp = new FnPerdiemRate();
                }

                fp.PerdiemProfileID = PPFID;
            //    fp.PerdiemRateID = RATEID;
                fp.PersonalLevel = ctlPersonalLevelDropdown.SelectedItem.Value;
                fp.ZoneID = Helper.UIHelper.ParseShort(ctlZoneDropdown.SelectedItem.Value);
                fp.OfficialPerdiemRate = Helper.UIHelper.ParseDouble(ctlOfficialRate.Text);
                fp.ExtraPerdiemRate = Helper.UIHelper.ParseDouble(ctlExtraRate.Text);
                fp.InternationalStaffPerdiemRate = Helper.UIHelper.ParseDouble(ctlInternationalStaffRate.Text);
                fp.SCGStaffPerdiemRate = Helper.UIHelper.ParseDouble(ctlSCGStaffRate.Text);
                fp.Active = ctlActive.Checked;
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    fp.UpdBy = UserAccount.UserID;
                    fp.UpdDate = DateTime.Now.Date;
                    fp.UpdPgm = ProgramCode;
                    FnPerdiemRateService.UpdateFnPerdiemRate(fp);
                }
                else if (Mode.Equals(FlagEnum.NewFlag))
                {
                    fp.UpdBy = UserAccount.UserID;
                    fp.UpdDate = DateTime.Now.Date;
                    fp.UpdPgm = ProgramCode;
                    fp.CreBy = UserAccount.UserID;
                    fp.CreDate = DateTime.Now.Date;
                    FnPerdiemRateService.AddFnPerdiemRate(fp);
                }
                Notify_Ok(sender, e);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanel.Update();
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ResetValue();
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
        }
        public void HidePopUp()
        {
            ctlForeignPerdiemRateProfileDetailEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlForeignPerdiemRateProfileDetailEditorModalPopupExtender.Show();
        }
        public void Initialize(string mode, long rateid, long ppfid)
        {
            Mode = mode;
            RATEID = rateid;
            PPFID = ppfid;
            IList<SuUserPersonalLevel> list = QueryProvider.SuUserPersonalLevelQuery.GetPLList();

            ctlPersonalLevelDropdown.DataSource = list;
            ctlPersonalLevelDropdown.DataTextField = "PersonalLevel";
            ctlPersonalLevelDropdown.DataValueField = "PersonalLevel";
            ctlPersonalLevelDropdown.DataBind();
            ctlPersonalLevelDropdown.Items.Insert(0, new ListItem("Please Select", string.Empty));


            IList<DbZoneResult> zoneList = SsDbQueryProvider.DbZoneQuery.FindZone(UserAccount.CurrentLanguageID);
            ctlZoneDropdown.DataSource = zoneList;
            ctlZoneDropdown.DataTextField = "ZoneName";
            ctlZoneDropdown.DataValueField = "ZoneID";
            ctlZoneDropdown.DataBind();
            ctlZoneDropdown.Items.Insert(0, new ListItem("Please Select", "-1"));
            if (Mode.Equals(FlagEnum.EditFlag))
            {
                FnPerdiemRate fp = ScgeAccountingQueryProvider.FnPerdiemRateQuery.FindByIdentity(RATEID);
                if (fp.PersonalLevel != null || fp.PersonalLevel != String.Empty)
                {
                    ctlPersonalLevelDropdown.SelectedValue = fp.PersonalLevel;
                }
                DbZoneResult zone = SsDbQueryProvider.DbZoneQuery.FindZoneByID(fp.ZoneID, UserAccount.CurrentLanguageID).FirstOrDefault();
                if (zone != null)
                {
                    ctlZoneDropdown.SelectedValue = zone.ZoneID.ToString();
                }

                ctlOfficialRate.Text = fp.OfficialPerdiemRate.HasValue ? fp.OfficialPerdiemRate.Value.ToString("#,##0.00") : string.Empty;
                ctlExtraRate.Text = fp.ExtraPerdiemRate.HasValue ? fp.ExtraPerdiemRate.Value.ToString("#,##0.00") : string.Empty;
                ctlInternationalStaffRate.Text = fp.InternationalStaffPerdiemRate.HasValue ? fp.InternationalStaffPerdiemRate.Value.ToString("#,##0.00"): string.Empty;
                ctlSCGStaffRate.Text = fp.SCGStaffPerdiemRate.HasValue ? fp.SCGStaffPerdiemRate.Value.ToString("#,##0.00") : string.Empty;
                ctlActive.Checked = fp.Active;
                ctlUpdatePanel.Update();
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
            }

        }
    }
}