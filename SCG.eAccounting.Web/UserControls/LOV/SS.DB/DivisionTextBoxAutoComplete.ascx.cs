using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SS.Standard.UI;

using SS.SU.DTO.ValueObject;

using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class DivisionTextBoxAutoComplete : BaseUserControl
    {
		#region Property
		public string DivisionName 
		{ 
			get { return ctlDivisionName.Text; }  
			set { ctlDivisionName.Text = value; } 
		}
		public string DivisionId
		{
			get { return ctlDivisionId.Text; }
			set { ctlDivisionId.Text = value; }
		}
		public string LanguageId
		{
			get { return ctlLanguageId.Text; }
			set { ctlLanguageId.Text = value; }
		}
		public string OrganizationId
		{
			get	{ return ctlOrganizationId.Text; }
			set { ctlOrganizationId.Text = value; }
		}
		#endregion
    
		protected void Page_Load(object sender, EventArgs e)
		{
			short orgId = UIHelper.ParseShort(OrganizationId);
			short langId = UIHelper.ParseShort(LanguageId);
		
			AutoCompleteParameter parameter = new AutoCompleteParameter();
			parameter.LanguageId = langId;
			parameter.OrganizationId = orgId;

			JavaScriptSerializer serializer = new JavaScriptSerializer();
			ctlDivisionAutoComplete.ContextKey = serializer.Serialize(parameter);
			ctlDivisionAutoComplete.UseContextKey = true;
		}
    }
}