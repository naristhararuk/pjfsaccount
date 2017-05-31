using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SS.Standard.UI;

using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;


namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class UserTextBoxAutoComplete : BaseUserControl
    {
        #region Property
       
       
        #endregion

        public class GetUser
        {
            public string UserName { get; set; }
            public string Name{ get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //short countryId = UIHelper.ParseShort(CountryId);
            //short langId = UIHelper.ParseShort(LanguageId);

            //DbCountryAutoCompleteParameter parameter = new DbCountryAutoCompleteParameter();
            //parameter.LanguageId = langId;
            //parameter.CountryId = countryId;

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //ctlUserAutoComplete.ContextKey = serializer.Serialize(parameter);
            GetUser parameter = new GetUser();
            parameter.UserName = ctlUserName.Text;
            parameter.Name = txtNameLastName.Text;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlUserAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlUserAutoComplete.UseContextKey = true;
        }
    }
}