using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class IOLabelLookup : BaseUserControl
    {
        public long IOID
        {
            get
            {
                if (ViewState["IOID"] != null)
                    return (long)ViewState["IOID"];
                return 0;
            }
            set
            {
                ViewState["IOID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlIOLookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlIOLookup_OnObjectLookUpCalling);
            ctlIOLookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlIOLookup_OnObjectLookUpReturn);

        }
        protected void ctlIOLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            //UserControls.LOV.SCG.DB.CostCenterLookUp CostCenterSearch = sender as UserControls.LOV.SCG.DB.CostCenterLookUp;
        }
        protected void ctlIOLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            DbInternalOrder ioInfo = (DbInternalOrder)e.ObjectReturn;
            IOID = ioInfo.IOID;
            ctlIO.Text = String.Format("{0}", ioInfo.IONumber);
            ctlUpdatePanelIOSimple.Update();
        }
        protected void ctlSearchIO_Click(object sender, ImageClickEventArgs e)
        {
            ctlIOLookup.Show();
        }
        public void SetIOValue(long ioId)
        {
            if (ioId > 0)
            {
                IOID = ioId;
                DbInternalOrder cost = ScgDbQueryProvider.DbIOQuery.FindProxyByIdentity(ioId);
                ctlIO.Text = String.Format("{0}", cost.IONumber);
            }
            ctlUpdatePanelIOSimple.Update();
        }
    }
}