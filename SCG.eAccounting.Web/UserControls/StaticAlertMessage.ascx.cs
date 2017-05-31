using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.Security;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;
using SS.SU.DTO.ValueObject;

using SS.SU.Helper;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class StaticAlertMessage : BaseUserControl
    {
        public ISuGlobalTranslateQuery SuGlobalTranslateQuery { get; set; }
        public MessageTypeEnum.AlertMessage MsgType { get; set; }
        private string msgCode;
        public string skinID;
        public string MsgCode
        {
            get { return msgCode; }
            set { msgCode = value; }
        }
        public string Symbol{get;set;}

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show()
        {
            if (MsgType.Equals(MessageTypeEnum.AlertMessage.Information))
            {
                ctlAlertImg.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/ok.png";
                ctlMessageInformation.Text = MessageSource.GetMessage(Symbol);
                ctlMessageInformation.Visible = true;
                ctlMessageError.Visible = false;
            }
            else if (MsgType.Equals(MessageTypeEnum.AlertMessage.Error))
            {
                ctlAlertImg.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/cancel.png";
                ctlMessageError.Text = MessageSource.GetMessage(Symbol);
                ctlMessageError.Visible = true;
                ctlMessageInformation.Visible = false;
            }
            else
            {
                ctlAlertImg.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/alert.png";
                ctlMessageInformation.Text = MessageSource.GetMessage(Symbol);
                ctlMessageInformation.Visible = true;
                ctlMessageError.Visible = false;
            }
        }
    }
}