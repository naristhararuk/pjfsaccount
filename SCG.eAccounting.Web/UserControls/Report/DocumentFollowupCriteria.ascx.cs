using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using SS.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class DocumentFollowupCriteria : BaseUserControl
    {
        #region Properies
        public long companyID 
        {   
            get { return UIHelper.ParseLong(ctlCompanyField.CompanyID); }
        }
        public string LocationFrom
        {
            get { return ctlFromLocationField.LocationCode; }
        }
        public string LocationTo
        {
            get { return ctlToLocationField.LocationCode; }
        }
        public string DateFrom 
        {
            get { return ctlFromDateCalendar.DateValue; } 
        }
        public string  DateTo 
        {
            get { return ctlToDateCalendar.DateValue; }
        }
        public long createrID 
        {
            get { return UIHelper.ParseLong(ctlCreatorData.EmployeeID); }
        }
        public long requesterID
        {
            get { return UIHelper.ParseLong(ctlRequesterData.EmployeeID); }
        }
        public long CostCenterID
        {
            get { return UIHelper.ParseLong(ctlCostCenterField.CostCenterId); }
        }
        public long ServiceTeamID
        {
            //get { return UIHelper.ParseLong(ctlServiceTeam.SelectedValue); }
            get { return UIHelper.ParseLong(ctlServiceTeam.SelectedValue); }
        }
        //public int statusValue
        //{
        //    get { return UIHelper.ParseInt(ctlStatus.SelectedValue); } 
        //}
        public bool isPosting
        {
            get 
            {
                if (ctlCheckSerachOnly.Checked)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        //public string DefaultButton
        //{
        //    set { xxxx.DefaultButton = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if   (!IsPostBack)
            {
                bindServiceTeam();
            }
            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectReturn);
        }
        public void bindServiceTeam()
        {
            long? companyId = null;
            if (!string.IsNullOrEmpty(ctlCompanyField.CompanyID))
            {
                companyId = UIHelper.ParseLong(ctlCompanyField.CompanyID);
            }
            IList<TranslatedListItem> translatedList = ScgDbQueryProvider.DbServiceTeamQuery.GetAllServiceTeamListItemByCompanyId(companyId);
            
            ctlServiceTeam.DataTextField = "strSymbol";
            ctlServiceTeam.DataValueField = "strID";
            ctlServiceTeam.DataSource = translatedList;
            ctlServiceTeam.DataBind();
            if (ctlServiceTeam.Items.Count > 0)
            {
                ctlServiceTeam.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            }
            
        }

        protected void ctlCompanyField_OnObjectReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCompany company = (DbCompany)e.ObjectReturn;
                ctlCompanyField.CompanyID = company.CompanyID.ToString();
                ctlFromLocationField.CompanyId = company.CompanyID;
                ctlToLocationField.CompanyId = company.CompanyID;
            }
            else
            {
                ctlToLocationField.ResetValue();
                ctlFromLocationField.ResetValue();
            }
            bindServiceTeam();
            UpdatePanelVendor.Update();
        }
    }
}