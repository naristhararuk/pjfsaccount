using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SS.Standard.UI;

using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class LocationField : BaseUserControl
    {
        public long locationID 
        { 
            get
            {
                return UIHelper.ParseLong(ctlLocationId.Value);
            } 
        }
        public string LocationCode
        {
            get { return ctlLocation.Text; } 
        }

        public long? CompanyId
        {
            get
            {
                if (ViewState["CompanyID"] != null)
                {
                    return UIHelper.ParseLong(ViewState["CompanyID"].ToString());
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    ViewState["CompanyID"] = value.Value;
                }
                else
                {
                    ViewState["CompanyID"] = null;
                }
            }
        }
        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is LocationUserLookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlLocationLookup_OnNotifyPopup);
            }
        }

        #endregion

        protected void ctlLocationLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.LocationLookup LocationSearch = sender as UserControls.LOV.SCG.DB.LocationLookup;
        }
        protected void ctlLocationLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                DbLocation Location = (DbLocation)args.Data;

                ctlLocation.Text = Location.LocationCode;
                ctlLocationId.Value = Location.LocationID.ToString();
                CallOnObjectLookUpReturn(Location);
            }
            UpdatePanelLocation.Update();
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            LocationUserLookup ctlLocationLookup = LoadPopup<LocationUserLookup>("~/UserControls/LOV/SCG.DB/LocationUserLookup.ascx", ctlPopUpUpdatePanel);
            ctlLocationLookup.CompanyId = CompanyId;
            ctlLocationLookup.Show();
        }

        public void ResetValue()
        {

            ctlLocation.Text = String.Empty;
            ctlLocationId.Value = string.Empty;
            CompanyId = null;
            UpdatePanelLocation.Update();
        }
    }
}