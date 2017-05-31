using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.BLL;
using SS.Standard.UI;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SS.Standard.Utilities;
using SCG.DB.DTO;
using SCG.DB.Query;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ForeignPerdiemRateProfileCountryEditor : BaseUserControl
    {
        public IFnPerdiemProfileCountryService FnPerdiemProfileCountryService { get; set; }
        public long PPFID
        {
            get { return this.ViewState["PerdiemProfileID"] == null ? (long)0 : (long)this.ViewState["PerdiemProfileID"]; }
            set { this.ViewState["PerdiemProfileID"] = value; }
        }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        protected void Page_Load(object sender, EventArgs e)
        {
            ProgramCode = "PerdiemProfileCountryEditor";
        }
        public void ResetValue()
        {
            ctlCountryNameDropdown.SelectedValue = null;
            ctlZoneDropdown.SelectedValue = null;
            ctlUpdatePanel.Update();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            FnPerdiemProfileCountry ppc;

            try
            {
                ppc = new FnPerdiemProfileCountry();
                ppc.PerdiemProfileID = PPFID;
                ppc.CountryID = Helper.UIHelper.ParseShort(ctlCountryNameDropdown.SelectedItem.Value);
                ppc.ZoneID = Helper.UIHelper.ParseShort(ctlZoneDropdown.SelectedItem.Value);
                ppc.UpdBy = UserAccount.UserID;
                ppc.UpdDate = DateTime.Now.Date;
                ppc.UpdPgm = ProgramCode;
                ppc.CreBy = UserAccount.UserID;
                ppc.CreDate = DateTime.Now.Date;
                FnPerdiemProfileCountryService.AddFnPerdiemProfileCountry(ppc);
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
            ctlForeignPerdiemRateProfileCountryEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlForeignPerdiemRateProfileCountryEditorModalPopupExtender.Show();
        }
        public void Initialize(long ppfid)
        {
            PPFID = ppfid;
            IList<CountryLang> countrylist = ScgDbQueryProvider.DbCountryQuery.FindCountry(UserAccount.CurrentLanguageID);
            ctlCountryNameDropdown.DataSource = countrylist;
            ctlCountryNameDropdown.DataTextField = "CountryName";
            ctlCountryNameDropdown.DataValueField = "CountryID";
            ctlCountryNameDropdown.DataBind();
            ctlCountryNameDropdown.Items.Insert(0, new ListItem("Please Select", string.Empty));

            IList<DbZoneResult> zoneList = SsDbQueryProvider.DbZoneQuery.FindZone(UserAccount.CurrentLanguageID);
            ctlZoneDropdown.DataSource = zoneList;
            ctlZoneDropdown.DataTextField = "ZoneName";
            ctlZoneDropdown.DataValueField = "ZoneID";
            ctlZoneDropdown.DataBind();
            ctlZoneDropdown.Items.Insert(0, new ListItem("Please Select", "-1"));
            ResetValue();
        }
    }
}