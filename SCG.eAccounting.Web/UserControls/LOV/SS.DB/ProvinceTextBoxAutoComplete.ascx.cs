using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SS.Standard.UI;

using SS.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO.ValueObject;
using System.Web.Script.Serialization;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class ProvinceTextBoxAutoComplete : BaseUserControl
    {
        #region <== Property ==>
        public string ProvinceName
        {
            get { return txtProvinceName.Text; }
            set { txtProvinceName.Text = value; }
        }
        public string ProvinceID
        {
            get { return lblProvinceId.Text; }
            set { lblProvinceId.Text = value; }
        }
        public string RegionId
        {
            get { return lblRegionId.Text; }
            set { lblRegionId.Text = value; }
        }
        public string RegionName
        {
            get { return lblRegionName.Text; }
            set { lblRegionName.Text = value; }
        }
        #endregion <== Property ==>

        #region SetRegionID
        public void SetRegionID(string RegionID)
        {
            lblRegionId.Text = RegionID;
        }
        #endregion SetRegionID

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            DBProvinceAutoCompleteParameter parameter = new DBProvinceAutoCompleteParameter();
            parameter.LanguageId    = UserAccount.CurrentLanguageID.ToString();
            parameter.RegionId      = RegionId;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlProvinceAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlProvinceAutoComplete.UseContextKey = true;
        }
        #endregion protected void Page_Load(object sender, EventArgs e)
    }
}