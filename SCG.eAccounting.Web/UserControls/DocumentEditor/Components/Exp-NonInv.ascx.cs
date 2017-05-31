using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class Exp_NonInv : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            ModalPopupExtender1.Show();
            this.UpdatePanelGridView.Update();
        }

        protected void ctlSubmit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
            UpdatePanelGridView.Update();
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
            UpdatePanelGridView.Update();
        }
    }
}