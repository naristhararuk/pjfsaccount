using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class test : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void HidePopUp()
        {
            ctltestModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctltestGrid.DataCountAndBind();
            ctlCurrencySetupForm2.Update();
            
            //ctlCurrencySetupForm2.Update();
            ctltestModalPopupExtender.Show();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            //ctlAddEditPopup.ChMode = "New";
            //ctlAddEditPopup.Show();
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            //ctlAddEditPopup.ChMode = "New";
            //ctlAddEditPopup.Show();
        }
        protected void ctlCurrencySetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return null;
          
            //return SsDbQueryProvider.DbCurrencyQuery.FindCurrencyByID(1);
        }
    }
}