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


namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CountryTextBoxAutoComplete : BaseUserControl
    {
        #region Property
        public string CountryName
        {
            get { return txtCountryName.Text; }
            set { txtCountryName.Text = value; }
        }
        public string CountryId
        {
            get { return ctlCountryId.Text; }
            set { ctlCountryId.Text = value; }
        }
        public string LanguageId
        {
            get { return ctlLanguageId.Text; }
            set { ctlLanguageId.Text = value; }
        }
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    ctlCountryAutoComplete.BehaviorID = UniqueID;
            //}
            short countryId = UIHelper.ParseShort(CountryId);
            short langId = UIHelper.ParseShort(LanguageId);

            DbCountryAutoCompleteParameter parameter = new DbCountryAutoCompleteParameter();
            parameter.LanguageId = langId;
            parameter.CountryId = countryId;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlCountryAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlCountryAutoComplete.UseContextKey = true;
        }
    }
}