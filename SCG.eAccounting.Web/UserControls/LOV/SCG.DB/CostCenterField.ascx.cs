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
    public partial class CostCenterField : BaseUserControl, IEditorUserControl
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
            get
            {
                if (string.IsNullOrEmpty(ctlCostCenterCode.Value) || string.IsNullOrEmpty(ctlDescription.Text))
                {
                    return string.Empty;
                }
                else
                {
                    return ctlCostCenterCode.Value + '-' + ctlDescription.Text;
                }
            }
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

                ctlAutoCostCenter.CompanyId = UIHelper.ParseLong(ctlCompanyID.Value);
            }
        }

        public string CostCenterCode
        {
            get { return ctlCostCenterCode.Value; }
            set { ctlCostCenterCode.Value = value; }
        }
        public string CostCenterId
        {
            get { return ctlCostCenterID.Value; }
            set { ctlCostCenterID.Value = value; }
        }
        public bool ReadOnly
        {
            get
            {
                if (ViewState["CostCenterReadOnly"] != null)
                    return (bool)ViewState["CostCenterReadOnly"];
                return false;
            }
            set { ViewState["CostCenterReadOnly"] = value; }
        }
        public string AccountFieldControlID
        {
            get
            {
                if (ViewState["AccountFieldControlID"] != null)
                    return ViewState["AccountFieldControlID"].ToString();
                return string.Empty;
            }
            set { ViewState["AccountFieldControlID"] = value; }
        }
        public string IOFieldControlID
        {
            get
            {
                if (ViewState["IOFieldControlID"] != null)
                    return ViewState["IOFieldControlID"].ToString();
                return string.Empty;
            }
            set { ViewState["IOFieldControlID"] = value; }
        }
        #endregion

        #region Page_Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            CallOnObjectLookUpCalling();

            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is CostCenterLookUp)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlCostCenterLookup_OnNotifyPopup);
            }
            ctlAutoCostCenter.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlAutoCostCenter_OnObjectLookUpCalling);
            ctlAutoCostCenter.DataBind();
        }
        public void CheckForReadOnly()
        {
            if (!ReadOnly)
            {
                ctlSearch.Visible = true;
                ctlAutoCostCenter.SetAutoCompleteNotReadOnly();
            }
            else
            {
                ctlAutoCostCenter.SetAutoCompleteReadOnly();
                ctlSearch.Visible = false;
            }
        }

        #endregion

        public bool DisplayCaption
        {
            set
            {
                if (value)
                    ctlDescription.Style.Add("display", "inline-block");
                else
                    ctlDescription.Style.Add("display", "none");
            }
        }

        public void SetWidthTextBox(int width)
        {
            ctlAutoCostCenter.setTextBox = width;
        }
        protected void ctlAutoCostCenter_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            // set companyid to autocompletetextbox to find costcenter by companyid
            CallOnObjectLookUpCalling();
            UserControls.LOV.SCG.DB.CostCenterTextBoxAutoComplete CostCenterSearch = sender as UserControls.LOV.SCG.DB.CostCenterTextBoxAutoComplete;
            if (CompanyId != null)
                CostCenterSearch.CompanyId = CompanyId.Value;
        }

        protected void ctlCostCenterLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                IList<DbCostCenter> CostCenter = (IList<DbCostCenter>)args.Data;
                DbCostCenter dbCostCenter = CostCenter.First<DbCostCenter>();
                CostCenterId = dbCostCenter.CostCenterID.ToString();
                ctlAutoCostCenter.CostCenterCode = dbCostCenter.CostCenterCode;
                ctlDescription.Text = dbCostCenter.Description;
                CallOnObjectLookUpReturn(dbCostCenter);
            }
            ctlUpdatePanelCostCenter.Update();
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            CostCenterLookUp ctlCostCenter = LoadPopup<CostCenterLookUp>("~/UserControls/LOV/SCG.DB/CostCenterLookUp.ascx", ctlPopUpUpdatePanel);
            ctlCostCenter.CompanyId = CompanyId;
            ctlCostCenter.Show();
        }
        protected void ctlAutoCostCenter_NotifyPopupResult(object sender, string action, string result)
        {
            DbCostCenter dbCostCenter;
            long costCenterID = 0;

                if (action == "select")
                {
                    dbCostCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(UIHelper.ParseLong(result));
                    costCenterID = dbCostCenter == null ? 0 : dbCostCenter.CostCenterID;
                    this.SetValue(costCenterID);
                    CallOnObjectLookUpReturn(dbCostCenter);
                }
                else if (action == "textchanged")
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        ResetValue();
                    }
                    else
                    {
                        dbCostCenter = ScgDbQueryProvider.DbCostCenterQuery.getDbCostCenterByCostCenterCode(result);
                        costCenterID = dbCostCenter == null ? 0 : dbCostCenter.CostCenterID;
                        this.SetValue(costCenterID);
                        CallOnObjectLookUpReturn(dbCostCenter);
                    }
                }
                ctlUpdatePanelCostCenter.Update();
        }

        #region Public Method

        public void ShowDefault()
        {

        }
        public void BindCostCenterControl(long costCenterId)
        {
            DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(costCenterId);
            if (costCenter != null)
            {
                ctlCostCenterID.Value = costCenter.CostCenterID.ToString();
                ctlCostCenterCode.Value = costCenter.CostCenterCode;
                ctlAutoCostCenter.CostCenterID = costCenter.CostCenterID.ToString();
                ctlAutoCostCenter.CostCenterCode = costCenter.CostCenterCode;
                ctlDescription.Text = costCenter.Description;
            }
            ctlUpdatePanelCostCenter.Update();
        }
        public void SetValue(long costCenterID)
        {
            DbCostCenter costCenter = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(costCenterID);
            if (costCenter != null)
            {
                ctlAutoCostCenter.CostCenterCode = costCenter.CostCenterCode;
                ctlDescription.Text = costCenter.Description;
                ctlCostCenterCode.Value = costCenter.CostCenterCode.ToString();
                ctlCostCenterID.Value = costCenter.CostCenterID.ToString();
                Control ioField = this.NamingContainer.FindControl(IOFieldControlID);
                
                if (ioField != null && ioField is IOAutoCompleteLookup)
                {
                    IOAutoCompleteLookup io = (IOAutoCompleteLookup)ioField;
                    //io.CostCenterId = costCenter.CostCenterID;
                    io.SetCostCenter(costCenter.CostCenterID);
                }
            }
            else
            {
                this.ResetValue();
            }
            ctlUpdatePanelCostCenter.Update();
        }
        public void ResetValue()
        {
            ctlAutoCostCenter.CostCenterCode = string.Empty;
            ctlDescription.Text = string.Empty;
            CostCenterId = string.Empty;
            CostCenterCode = string.Empty;
            //CompanyId = null;
            ctlUpdatePanelCostCenter.Update();
        }

        #endregion
    }
}