using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CostCenterLabelLookup : BaseUserControl
    {
        public long CostCenterID
        {
            get
            {
                if (ViewState["CostCenterID"] != null)
                    return (long)ViewState["CostCenterID"];
                return 0;
            }
            set
            {
                ViewState["CostCenterID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCostCenterLookUp.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlCostCenterLookUp_OnObjectLookUpCalling);
            ctlCostCenterLookUp.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCostCenterLookUp_OnObjectLookUpReturn);

        }
        protected void ctlCostCenterLookUp_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            //UserControls.LOV.SCG.DB.CostCenterLookUp CostCenterSearch = sender as UserControls.LOV.SCG.DB.CostCenterLookUp;
        }
        protected void ctlCostCenterLookUp_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            IList<DbCostCenter> CostCenter = (IList<DbCostCenter>)e.ObjectReturn;
            DbCostCenter dbCostCenter = CostCenter.First<DbCostCenter>();
            CostCenterID = dbCostCenter.CostCenterID;
            ctlCostCenter.Text = dbCostCenter.CostCenterCode;
            ctlUpdatePanelCostCenterSimple.Update();
        }
        protected void ctlSearchCostCenter_Click(object sender, ImageClickEventArgs e)
        {
            ctlCostCenterLookUp.Show();
        }
        public void SetCostCenterValue(long costcenterId)
        {
            if (costcenterId > 0)
            {
                CostCenterID = costcenterId;
                DbCostCenter cost = ScgDbQueryProvider.DbCostCenterQuery.FindProxyByIdentity(costcenterId);

                ctlCostCenter.Text = String.Format("{0}", cost.CostCenterCode);
            }
            ctlUpdatePanelCostCenterSimple.Update();
        }
    }
}