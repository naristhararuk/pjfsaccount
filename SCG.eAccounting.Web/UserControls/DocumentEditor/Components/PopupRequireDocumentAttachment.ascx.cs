using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class PopupRequireDocumentAttachment : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlOkButton_Click(object sender, ImageClickEventArgs e)
        {
            NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
            HidePopup();
        }

        public void ShowPopup(string message)
        {
            ctlWarningPopupModalPopupExtender.Show();
            ctlWarningMessage.Text = message;
            ctlUpdatePanelWarningPopup.Update();
        }

        public void HidePopup()
        {
            ctlWarningPopupModalPopupExtender.Hide();
        }
    }
}