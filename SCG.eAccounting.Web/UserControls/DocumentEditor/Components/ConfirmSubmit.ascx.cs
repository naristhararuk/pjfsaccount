using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ConfirmSubmit : BaseUserControl
    {
        public bool DisableButton { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ctlOkButton_Click(object sender, EventArgs e)
        {
            NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
            HideConfirm();
        }

        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Cancel, null));
            HideConfirm();
        }

        protected void ctlSaveAndAddAdvance_Click(object sender, EventArgs e)
        {
            NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Add, null));
            HideConfirm();
        }

        public void ShowConfirm()
        {
            ctlConfirmModalPopupExtender.Show();

            if (this.DisableButton)
            {
                ctlPanelConfirmSubmit.Width = Unit.Pixel(550);
                ctlSaveAndAddAdvance.Visible = this.DisableButton;
            }
            else
            {
                ctlPanelConfirmSubmit.Width = Unit.Pixel(300);
                ctlSaveAndAddAdvance.Visible = this.DisableButton;
            }
            ctlUpdatePanelConfirmSubmit.Update();
        }

        public void HideConfirm()
        {
            ctlConfirmModalPopupExtender.Hide();
        }
    }
}