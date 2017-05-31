using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class LocationEditor : BaseUserControl
    {
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;

        public IDbLocationService DbLocationService { get; set; }
        public IDbLocationLangService DbLocationLangService { get; set; }

        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long LocationID
        {
            get { return this.ViewState["LocationID"] == null ? (long)0 : (long)this.ViewState["LocationID"]; }
            set { this.ViewState["LocationID"] = value; }
        }
        public long CompanyID
        {
            get { return this.ViewState["CompanyID"] == null ? (long)0 : (long)this.ViewState["CompanyID"]; }
            set { this.ViewState["CompanyID"] = value; }
        }

        public void ResetValue()
        {
            ctlLocationCode.Visible = true;
            ctlLocationCode.Text = string.Empty;
            ctlLocationCodeLabelDisplay.Visible = false;
            ctlLocationCodeLabelDisplay.Text = string.Empty;
            ctlDescription.Text = string.Empty;
            ctlActive.Checked = true;
            ctlLocationUpdatePanel.Update();

        }
        public void Initialize(string mode,long locationID,long companyID)
        {
            LocationID = locationID;
            Mode = mode;
            CompanyID = companyID;
            
            IList<PaymentTypeListItem> list = ScgDbQueryProvider.DbPBQuery.GetPbListItem(companyID, UserAccount.UserLanguageID);

            ctlDefaultPBDropdown.DataSource = list;
            ctlDefaultPBDropdown.DataTextField = "Text";
            ctlDefaultPBDropdown.DataValueField = "ID";
            ctlDefaultPBDropdown.DataBind();
            ctlDefaultPBDropdown.Items.Insert(0, new ListItem("Please Select","0"));

            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbLocation location = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(LocationID);
                LocationID = location.LocationID;
                ctlLocationCodeLabelDisplay.Visible = true;
                ctlLocationCodeLabelDisplay.Text = location.LocationCode;
                ctlLocationCode.Text = location.LocationCode;
                ctlLocationCode.Visible = false;
                ctlDescription.Text = location.LocationName;
                ctlCheckBoxAllowImportExpense.Checked = location.IsAllowImportExpense;
                ctlActive.Checked = location.Active;
                ctlLocationUpdatePanel.Update();
                if (location.DefaultPBID.HasValue && location.DefaultPBID.Value > 0)
                {
                    ctlDefaultPBDropdown.SelectedValue = location.DefaultPBID.Value.ToString();
                }

            }
            else if (mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();

            }

        }
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            DbLocation location = new DbLocation();

            if (Mode.Equals(FlagEnum.EditFlag))
            {
                location.LocationID = LocationID;
            }
            DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(CompanyID);
            
            location.LocationName = ctlDescription.Text;
            location.Active = ctlActive.Checked;
            //location.CompanyCode = com;
            location.CompanyID = com;
            location.CreBy = UserAccount.UserID;
            location.CreDate = DateTime.Now;
            location.LocationCode = ctlLocationCode.Text;
            location.UpdBy = UserAccount.UserID;
            location.UpdDate = DateTime.Now;
            location.IsAllowImportExpense = ctlCheckBoxAllowImportExpense.Checked;
            location.UpdPgm = UserAccount.CurrentProgramCode;
            if (UIHelper.ParseLong(ctlDefaultPBDropdown.SelectedValue) > 0)
            {
                location.DefaultPBID = UIHelper.ParseLong(ctlDefaultPBDropdown.SelectedValue);
            }

            try
            {
                // save or update currency
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbLocationService.UpdateLocation(location);
                    DbLocationService.UpdateLocationToExp(location);
                }
                else
                {
                    long locationId = DbLocationService.AddNewLocation(location);
                    location.LocationID = locationId;
                }

                Notify_Ok(sender, e);

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }


        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
            HidePopUp();

        }
        public void HidePopUp()
        {
            ctlLocationModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlLocationModalPopupExtender.Show();

        }
    }
}