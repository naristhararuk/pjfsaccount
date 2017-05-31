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
using SS.SU.DTO;
using SS.SU.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CompanyField : BaseUserControl, IEditorUserControl
    {
        #region Property
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
        }
        public string CompanyCode
        {
            get { return ctlCompanyAutocomplete.CompanyCode; }
            set { ctlCompanyAutocomplete.CompanyCode = value; }
        }
        public string CompanyID
        {
            get { return ctlCompanyAutocomplete.CompanyID; }
            set { ctlCompanyAutocomplete.CompanyID = value; }
        }
        public string Text
        {
            //get { return String.Format("{0}:{1}-{2}", ctlCompany.Text, ctlCompanyAutocomplete.CompanyCode, ctlCompanyName.Text); }
            get { return String.Format("{0}-{1}", ctlCompanyAutocomplete.CompanyCode, ctlCompanyName.Text); }
        }
        public bool Display
        {
            set
            {
                if (value)
                    ctlContainerTable.Style.Add("display", "inline-block");
                else
                    ctlContainerTable.Style.Add("display", "none");
            }
        }

        public bool? FlagActive
        {
            get { return ctlCompanyAutocomplete.FlagActive; }
            set { ctlCompanyAutocomplete.FlagActive = value; }
        }

        public bool UseEccOnly
        {
            get { return ctlCompanyAutocomplete.UseEccOnly; }
            set { ctlCompanyAutocomplete.UseEccOnly = value; }
        }
        public bool UseExpOnly
        {
            get { return ctlCompanyAutocomplete.UseExpOnly; }
            set { ctlCompanyAutocomplete.UseExpOnly = value; }
        }

        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is CompanyLookUp)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlCompanyLookup_OnNotifyPopup);
            }
            ctlCompanyAutocomplete.DataBind();
        }

        #endregion

        #region Public Method
        public void ChangeMode()
        {
            if ((!string.IsNullOrEmpty(this.Mode)) && (this.Mode.Equals(ModeEnum.ReadWrite)))
            {

                ctlCompanyAutocomplete.SetAutoCompleteNotReadOnly();
                ctlSearch.Visible = true;
            }
            else
            {
                ctlCompanyAutocomplete.SetAutoCompleteReadOnly();
                ctlSearch.Visible = false;
            }
        }
        #endregion

        protected void ctlCompanyLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            DbCompany company = (DbCompany)args.Data;
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                ctlCompanyAutocomplete.CompanyID = company.CompanyID.ToString();
                ctlCompanyAutocomplete.CompanyCode = company.CompanyCode.ToString();
                ctlCompanyName.Text = company.CompanyName.ToString();
                ctlCompanyID.Text = company.CompanyID.ToString();
                CompanyID = CompanyID.ToString();
            }
            ctlUpdatePanelCompany.Update();
            CallOnObjectLookUpReturn(company);
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            CompanyLookUp ctlCompanyLookup = LoadPopup<CompanyLookUp>("~/UserControls/LOV/SCG.DB/CompanyLookUp.ascx", ctlPopUpUpdatePanel);
            ctlCompanyLookup.UseEccOnly = UseEccOnly;
            ctlCompanyLookup.FlagActive = FlagActive;
            ctlCompanyLookup.Show();
        }
        protected void ctlCompanyCode_NotifyPopupResult(object sender, string action, string result)
        {
            DbCompany dbCom;
            long companyID = 0;
            if (action == "select")
            {
                dbCom = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(result));
                companyID = dbCom == null ? 0 : dbCom.CompanyID;
                this.SetValue(companyID);
                CallOnObjectLookUpReturn(dbCom);
            }
            else if (action == "textchanged")
            {
                if (string.IsNullOrEmpty(result))
                {
                    ResetValue();
                    CallOnObjectLookUpReturn(null);
                }
                else
                {
                    bool? useEcc = null;
                    if (UseEccOnly || UseExpOnly)
                    {
                        useEcc = UseExpOnly ? false : true;
                    }

                    dbCom = ScgDbQueryProvider.DbCompanyQuery.GetDbCompanyByCriteria(result, useEcc,FlagActive);

                    companyID = dbCom == null ? 0 : dbCom.CompanyID;
                    this.SetValue(companyID);
                    CallOnObjectLookUpReturn(dbCom);
                }
            }
            ctlUpdatePanelCompany.Update();
        }
        public void ShowDefault()
        {
            SuUser userInfo = QueryProvider.SuUserQuery.FindByIdentity(UserAccount.UserID);
            if (userInfo != null && userInfo.Company != null)
            {
                if (userInfo.Company.UseEcc == UseEccOnly)
                {
                    ctlCompanyAutocomplete.CompanyID = userInfo.Company.CompanyID.ToString();
                    ctlCompanyAutocomplete.CompanyCode = userInfo.Company.CompanyCode;
                    ctlCompanyName.Text = userInfo.Company.CompanyName;
                    ctlCompanyID.Text = userInfo.Company.CompanyID.ToString();
                }
            }
        }
        public void SetValue(long companyID)
        {
            DbCompany company = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(companyID);
            if (company != null)
            {
                ctlCompanyAutocomplete.CompanyID = company.CompanyID.ToString();
                ctlCompanyAutocomplete.CompanyCode = company.CompanyCode;
                ctlCompanyName.Text = company.CompanyName;
                ctlCompanyID.Text = company.CompanyID.ToString();
            }
            else
            {
                this.ResetValue();
            }
        }
        public void ResetValue()
        {
            ctlCompanyAutocomplete.CompanyID = string.Empty;
            ctlCompanyAutocomplete.CompanyCode = string.Empty;
            ctlCompanyName.Text = string.Empty;
            ctlCompanyID.Text = string.Empty;
            ctlUpdatePanelCompany.Update();
        }
        public void SetWidthTextBox(int width)
        {
            ctlCompanyAutocomplete.setTextBox = width;
        }
    }
}