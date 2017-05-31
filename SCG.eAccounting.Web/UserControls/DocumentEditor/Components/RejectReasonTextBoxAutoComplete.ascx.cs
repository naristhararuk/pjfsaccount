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


namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class RejectReasonTextBoxAutoComplete : BaseUserControl
    {
        #region Property
        public string ReasonDetail
        {
            get { return txtReasonDetail.Text; }
            set { txtReasonDetail.Text = value; }
        }
        public string ReasonID
        {
            get { return ctlReasonId.Text; }
            set { ctlReasonId.Text = value; }
        }
        public string LanguageId
        {
            get { return ctlLanguageId.Text; }
            set { ctlLanguageId.Text = value; }
        }
        public string DocumentTypeCode
        {
            get { return ctlDocumentTypeCode.Text; }
            set { ctlDocumentTypeCode.Text = value; }
        }
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            short reasonId = UIHelper.ParseShort(ReasonID);
            short langId = UIHelper.ParseShort(LanguageId);

            DbReasonAutoCompleteParameter parameter = new DbReasonAutoCompleteParameter();
            parameter.LanguageId = langId;
            parameter.ReasonID = reasonId;
            parameter.DocumentTypeCode = DocumentTypeCode;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ctlReasonAutoComplete.ContextKey = serializer.Serialize(parameter);
            ctlReasonAutoComplete.UseContextKey = true;
        }
    }
}