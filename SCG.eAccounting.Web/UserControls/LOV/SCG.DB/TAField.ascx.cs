using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class TAField : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlTALookup.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlTANoLookup_ObjectLookupCalling);
            ctlTALookup.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlTANoLookup_ObjectLookupReturn);
        }

        protected void ctlTANoLookup_Click(object sender, ImageClickEventArgs e)
        {
            ctlTALookup.Show();
        }
        protected void ctlTANoLookup_ObjectLookupCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            UserControls.LOV.SCG.DB.TALookup lookup = sender as UserControls.LOV.SCG.DB.TALookup;
            //lookup.CompanyID = ;
            //lookup.RequesterID = ;
            
        }
        protected void ctlTANoLookup_ObjectLookupReturn(object sender, ObjectLookUpReturnArgs e)
        {
            TADocumentObj ta = (TADocumentObj)e.ObjectReturn;
            ctlTANoDetail.Text = ta.DocumentNo;
            ctlTANoDetail.CommandArgument = ta.TADocumentID.ToString();
        }
    }
}