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
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class LocationUserField : BaseUserControl, IEditorUserControl
    {
        #region Property

        public bool Display
        {
            set
            {
                if (value)
                    ctlContainer.Style.Add("display", "inline-block");
                else
                    ctlContainer.Style.Add("display", "none");
            }
        }
        public string Text
        {
            get { return ctlLocationName.Value + '-' + ctlDescription.Text; }
        }
        public long? CompanyId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCompanyID.Value))
                    return null;
                return UIHelper.ParseLong(ctlCompanyID.Value);
            }
            set
            {
                if (value.HasValue)
                    ctlCompanyID.Value = value.Value.ToString();
                else
                    ctlCompanyID.Value = string.Empty;
            }
        }
        public long? LocationID
        {
            get
            {
                if (string.IsNullOrEmpty(ctlLocationID.Value))
                    return null;
                return UIHelper.ParseLong(ctlLocationID.Value);
            }
            set
            {
                if (value.HasValue)
                    ctlLocationID.Value = value.Value.ToString();
                else
                    ctlLocationID.Value = string.Empty;
            }
        }

        public string LocationName
        {
            get { return ctlLocationName.Value; }
            set { ctlLocationName.Value = value; }
        }

        public bool ReadOnly
        {
            get
            {
                if (ViewState["LocationReadOnly"] != null)
                    return (bool)ViewState["LocationReadOnly"];
                return false;
            }
            set { ViewState["LocationReadOnly"] = value; }
        }

        public bool DisableCompany
        {
            get { return ViewState["DisableCompany"] == null ? false : (bool)ViewState["DisableCompany"]; }
            set { ViewState["DisableCompany"] = value; }
        }

        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is LocationUserLookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlLlocationLookup_OnNotifyPopup); ;
            }

            ctlLocationTextBoxAutoComplete.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlLocationTextBox_OnObjectLookUpCalling);

            ctlLocationTextBoxAutoComplete.DataBind();
        }
        protected void ctlLocationTextBox_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            // set companyid to autocompletetextbox to find costcenter by companyid
            //CallOnObjectLookUpCalling();
            UserControls.LOV.SCG.DB.LocationTextBoxAutocomplete LocationSearch = sender as UserControls.LOV.SCG.DB.LocationTextBoxAutocomplete;
            if (CompanyId != null)
                LocationSearch.CompanyId = CompanyId.Value;
        }
        public void CheckForReadOnly()
        {
            if (!ReadOnly)
            {
                ctlSearch.Visible = true;
                ctlLocationTextBoxAutoComplete.SetAutoCompleteNotReadOnly();
            }
            else
            {
                ctlLocationTextBoxAutoComplete.SetAutoCompleteReadOnly();
                ctlSearch.Visible = false;
            }
        }
        #endregion

        protected void ctlLlocationLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                DbLocation location = (DbLocation)args.Data;
                LocationID = location.LocationID;
                LocationName = location.LocationName;
                ctlLocationTextBoxAutoComplete.LocationID = location.LocationID;
                ctlLocationTextBoxAutoComplete.LocationCode = location.LocationCode.Trim();
                ctlLocationTextBoxAutoComplete.LocationName = location.LocationName.Trim();
                ctlDescription.Text = location.LocationName.Trim();
                CallOnObjectLookUpReturn(location);
            }
            ctlUpdatePanelLocation.Update();
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            LocationUserLookup ctlLocationLookup = LoadPopup<LocationUserLookup>("~/UserControls/LOV/SCG.DB/LocationUserLookup.ascx", ctlPopUpUpdatePanel);
            //ctlLocationLookup.SelectedCompany = CompanyId == null ? string.Empty : CompanyId.ToString();
            if (CompanyId.HasValue)
                ctlLocationLookup.CompanyId = CompanyId.Value;
            ctlLocationLookup.Show();
        }
        protected void ctlAutoLocation_NotifyPopupResult(object sender, string action, string result)
        {

            IList<DbLocation> iListDbLocation = ScgDbQueryProvider.DbLocationQuery.FindByLocationNameID(UIHelper.ParseLong(result), LocationName);
            DbLocation dbLocation = new DbLocation();
            long locationID = 0;

            if (action == "select")
            {
                dbLocation = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(UIHelper.ParseLong(result));
                locationID = dbLocation == null ? 0 : dbLocation.LocationID;
                this.BindLocationControl(locationID);
            }
            else if (action == "textchanged")
            {
                if (string.IsNullOrEmpty(result))
                {
                    ResetValue();
                }
                else
                {
                    dbLocation = ScgDbQueryProvider.DbLocationQuery.FindDbLocationByLocationCode(result);
                    locationID = dbLocation == null ? 0 : dbLocation.LocationID;
                    this.BindLocationControl(locationID);
                }
            }
            CallOnObjectLookUpReturn(locationID);
        }

        #region Public Method

        public void ShowDefault()
        {

        }
        public void BindLocationControl(long locationId)
        {
            DbLocation locate = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(locationId);
            if (locate != null)
            {
                ctlLocationTextBoxAutoComplete.LocationID = locate.LocationID;
                ctlLocationTextBoxAutoComplete.LocationCode = locate.LocationCode;
                ctlLocationTextBoxAutoComplete.LocationName = locate.LocationName;
                ctlDescription.Text = locate.LocationName;
                ctlLocationID.Value = locate.LocationID.ToString();
            }
            else
            {
                this.ResetValue();
            }
            ctlUpdatePanelLocation.Update();

        }
        public void SetValue(long locationId)
        {
            DbLocation locate = ScgDbQueryProvider.DbLocationQuery.FindByIdentity(locationId);
            ctlLocationName.Value = locate.LocationName;
            ctlLocationID.Value = locate.LocationID.ToString();
            ctlDescription.Text = locate.LocationName;
            ctlLocationTextBoxAutoComplete.LocationID = locate.LocationID;
            ctlLocationTextBoxAutoComplete.LocationName = locate.LocationName;
            ctlLocationTextBoxAutoComplete.LocationCode = locate.LocationCode;

        }
        public void ResetValue()
        {

            ctlDescription.Text = String.Empty;
            LocationID = null;
            LocationName = string.Empty;
            ctlLocationTextBoxAutoComplete.LocationID = null;
            ctlLocationTextBoxAutoComplete.LocationCode = "";
            ctlLocationTextBoxAutoComplete.LocationName = "";
            ctlUpdatePanelLocation.Update();
        }
        public void SetWidthTextBox(int width)
        {
            ctlLocationTextBoxAutoComplete.setTextBox = width;
        }

        #endregion
    }
}