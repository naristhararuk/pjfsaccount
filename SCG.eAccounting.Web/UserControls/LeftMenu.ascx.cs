using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.SU.DTO;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class LeftMenu : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            setMenuPermission();
        }
        private void setMenuPermission()
        {


            //Set Permission   Accountant Pane
            if (UserAccount.IsVerifyDocument
                || UserAccount.IsApproveVerifyDocument
                || UserAccount.IsReceiveDocument
                || UserAccount.IsCounterCashier)
            {
                AccountantPane.Visible = true;
                AccountantInbox.Visible = true;
                AccountantSearch.Visible = true;
            }
            else
            {
                AccountantPane.Visible = true;
                AccountantInbox.Visible = true;
                AccountantSearch.Visible = true;

            }

            //Set Permission Payment Pane
            if (UserAccount.IsVerifyPayment
                || UserAccount.IsApproveVerifyPayment
                || UserAccount.IsReceiveDocument)
            {
                PaymentPane.Visible = true;
                PaymentInbox.Visible = true;
                PaymentSearch.Visible = true;
            }
            else
            {
                PaymentPane.Visible = true;
                PaymentInbox.Visible = true;
                PaymentSearch.Visible = true;

            }



            //Set Permission Search Pane
            //if (UserAccount.IsAccountant
            //    || UserAccount.IsPayment)
            //{
            //    SearchPane.Visible = true;
            //}
            //else
            //{
            //    SearchPane.Visible = false;
            //}

        }
    }
}