using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.Query;
using System.Collections;
using SCG.eAccounting.SAP;
using System.Data;
using SCG.eAccounting.SAP.Query;
using SCG.eAccounting.SAP.DTO.ValueObject;

namespace SCG.eAccounting.Web.Forms.SCG.Log
{
    public partial class InterfacePostSAPLog : BasePage
    {
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlPostSAPLogGrid.DataCountAndBind();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            string strType = (ctlStatus.SelectedValue == "A") ? "" : ctlStatus.SelectedValue;
            string strDate = "";
            if (ctlDate.Text != "")
            {
                string[] strDateSub = ctlDate.Text.Split('/');
                strDate = strDateSub[2].ToString() + strDateSub[1].ToString() + strDateSub[0].ToString();
            }
            return BapiQueryProvider.Bapiret2Query.GetBapiret2ByCriteria(new SapLog(), startRow, pageSize, sortExpression, ctlRequestNo.Text, strDate, strType);
        }
        public int RequestCount()
        {
            string strType = (ctlStatus.SelectedValue == "A") ? "" : ctlStatus.SelectedValue;
            string strDate = "";
            if (ctlDate.Text != "")
            {
                string[] strDateSub = ctlDate.Text.Split('/');
                strDate = strDateSub[2].ToString() + strDateSub[1].ToString() + strDateSub[0].ToString();
            }
            return BapiQueryProvider.Bapiret2Query.CountBapiret2ByCriteria(new SapLog(), ctlRequestNo.Text, strDate, strType);
        }

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlPostSAPLogGrid.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        public string applyDateFormat(string inputDate)
        {
            if (!inputDate.Equals(string.Empty))
            {
                DateTime date = DateTime.Parse(inputDate);
                return date.ToString("MM/dd/yyyy");
            }
            else
                return string.Empty;
        }
    }
}
