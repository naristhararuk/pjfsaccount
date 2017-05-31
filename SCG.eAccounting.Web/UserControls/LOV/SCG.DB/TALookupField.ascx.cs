using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class TALookupField : BaseUserControl
    {
        public string TaDocumentNo
        {
            get { return ctlTANoDetail.Text; }
            set { ctlTANoDetail.Text = value; }
        }
        public long? TaID
        {
            get { return UIHelper.ParseLong(ctlTAID.Text); }
            set { ctlTAID.Text = value.ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BaseUserControl ctl = GetCurrentPopUpControl();

            if (ctl is TALookup)
            {
                ctl.OnPopUpReturn += new PopUpReturnEventHandler(ctlTANoLookup_OnNotifyPopup);
            }
        }

        protected void ctlTANoLookup_Click(object sender, ImageClickEventArgs e)
        {
            TALookup ctlTALookup = LoadPopup<TALookup>("~/UserControls/LOV/SCG.DB/TALookup.ascx", ctlPopUpUpdatePanel);
            ctlTALookup.Show();
        }

        protected void ctlTANoLookup_OnNotifyPopup(object sender, PopUpReturnArgs args)
        {
            if (args.Type.Equals(PopUpReturnType.OK))
            {
                TADocumentObj ta = (TADocumentObj)args.Data;
                ctlTANoDetail.Text = ta.DocumentNo;
                ctlTAID.Text = ta.DocumentID.ToString();
            }
            ctlUpdatePanelTa.Update();
        }
    }
}