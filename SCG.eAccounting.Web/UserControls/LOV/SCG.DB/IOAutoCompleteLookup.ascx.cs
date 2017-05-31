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

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class IOAutoCompleteLookup : BaseUserControl, IEditorUserControl
    {
        #region Property
        public string IOID
        {
            get { return ctlIOID.Text; }
            set { ctlIOID.Text = value; }
        }

        public bool DisplayCaption
        {
            set
            {
                if (value)
                    ctlIODescription.Style.Add("display", "inline-block");
                else
                    ctlIODescription.Style.Add("display", "none");
			}
        }

        #endregion Property
        public long? CostCenterId
        {
            get
            {
                if (string.IsNullOrEmpty(ctlCostCenterID.Value))
                    return null;
                return UIHelper.ParseLong(ctlCostCenterID.Value);
            }
            set
            {
                if (value.HasValue)
                    ctlCostCenterID.Value = value.Value.ToString();
                else
                    ctlCostCenterID.Value = string.Empty;

                ctlIOTextBoxAutoComplete.CostCenterId = UIHelper.ParseLong(ctlCostCenterID.Value);
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

                ctlIOTextBoxAutoComplete.CompanyId = UIHelper.ParseLong(ctlCompanyID.Value);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();
            if (ctl is IOLookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlIOLookup_OnNotifyPopup);
            }
            ctlIOTextBoxAutoComplete.BuildAutoCompleteParameter();
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            IOLookup ctlIOLookup = LoadPopup<IOLookup>("~/UserControls/LOV/SCG.DB/IOLookup.ascx", ctlPopUpUpdatePanel);
            ctlIOLookup.CostCenterId = CostCenterId;
            ctlIOLookup.CompanyId = CompanyId;
            ctlIOLookup.Show();
        }
        #region Lookup Event

        protected void ctlIOLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                DbInternalOrder ioInfo = (DbInternalOrder)args.Data;
                ctlIOTextBoxAutoComplete.InternalOrder = ioInfo.IONumber;
                ctlIODescription.Text = ioInfo.IOText;
                IOID = ioInfo.IOID.ToString();
                CallOnObjectLookUpReturn(ioInfo);
            }
            ctlUpdatePanelIO.Update();
        }
        #endregion

        protected void ctlIOTextBoxAutoComplete_NotifyPopupResult(object sender, string action, string result)
        {
            DbInternalOrder io;
            long ioID = 0;

            if (action == "select")
            {
                io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(UIHelper.ParseLong(result));
                ioID = io == null ? 0 : io.IOID;
                this.BindIOControl(ioID);
                CallOnObjectLookUpReturn(io);
            }
            else if (action == "textchanged")
            {
                if (string.IsNullOrEmpty(result))
                {
                    ResetValue();
                }
                else
                {
                    io = ScgDbQueryProvider.DbIOQuery.getDbInternalOrderByIONumber(result);
                    ioID = io == null ? 0 : io.IOID;
                    this.BindIOControl(ioID);
                    CallOnObjectLookUpReturn(io);
                }
            }
        }
        #region Public Method
        public void ShowDefault()
        {

        }
        public void BindIOControl(long ioId)
        {
            IOID = string.Empty;
            DbInternalOrder io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(ioId);
            if (io != null)
            {
                ctlIOTextBoxAutoComplete.InternalOrder = io.IONumber;
                ctlIODescription.Text = io.IOText;
                IOID = io.IOID.ToString();
            }
            else
            {
                ResetValue();
            }
            ctlUpdatePanelIO.Update();
        }
        public void ResetValue()
        {
            ctlIOTextBoxAutoComplete.InternalOrder = string.Empty;
            ctlIODescription.Text = string.Empty;
            IOID = string.Empty;
            CostCenterId = null;
            ctlUpdatePanelIO.Update();
        }
        #endregion
        public void SetCostCenter(long? costcenterID)
        {
            
            CostCenterId = costcenterID;
            ctlUpdatePanelIO.Update();
        }
		#region IEditorUserControl Members
		public bool Display
		{
			set 
			{
				if (value)
				{
					this.ctlContainer.Style["display"] = "inline-block";
				}
				else
				{
					this.ctlContainer.Style["display"] = "none";
				}
			}
		}
		public string Text
		{
            get { return ctlIOTextBoxAutoComplete.InternalOrder + '-' + ctlIODescription.Text; }
		}
		#endregion
	}
}