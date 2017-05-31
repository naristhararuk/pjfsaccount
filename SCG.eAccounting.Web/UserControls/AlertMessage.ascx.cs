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
    public partial class AlertMessage : BaseUserControl
    {

        public ISuGlobalTranslateQuery SuGlobalTranslateQuery { get; set; }

      
       
        //public Unit PanelShowMessageHeight 
        //{
        //    get
        //    {
        //        return PanelShowMessage.Height;
        //    }
        //    set 
        //    { 
        //        if(value==0)
        //            PanelShowMessage.Height = 350; 
        //        else
        //            PanelShowMessage.Height = value; 
        //    }
        //}

        //public Unit PanelShowMessageWidth
        //{
        //    get
        //    {
        //        return PanelShowMessage.Width;
        //    }
        //    set
        //    {
        //        if (value == null || value==0)
        //            PanelShowMessage.Width = 450;
        //        else
        //            PanelShowMessage.Width = value;
        //    }
        //}
        public MessageTypeEnum.AlertMessage MsgType { get; set; }
        
        private string msgCode;
        public string MsgCode
        {
            get { return msgCode; }
            set { msgCode = value; }
        }

        public string MsgHeader
        {
            get
            {
                return ctlMsgHeaderLabel.Text;
            }
            set
            {
                ctlMsgHeaderLabel.Text = value;
            }
        }

        public string MsgTopic
        {
            get
            {
                return ctlMsgTopicLabel.Text;
            }
            set
            {
                ctlMsgTopicLabel.Text = value;
            }
        }

        public string MsgBody
        {
            get
            {
                return ctlMsgBodyLabel.Text;
            }
            set
            {
                ctlMsgBodyLabel.Text = value;
            }
        }


        //public string CloseOnClientClick
        //{
        //    get { return ctlMsgCloseImageButton.OnClientClick; }
        //    set { ctlMsgCloseImageButton.OnClientClick = value; }
        //}
        //public string OkCloseOnClientClick
        //{
        //    get { return ctlMsgOKCloseImageButton.OnClientClick; }
        //    set { ctlMsgOKCloseImageButton.OnClientClick = value; }
        //}
        //public string OKOnClientClick
        //{
        //    get { return ctlOKImageButton.OnClientClick; }
        //    set { ctlOKImageButton.OnClientClick = value; }
        //}
        //public string CancelOnClientClick
        //{
        //    get { return ctlMsgCancelImageButton.OnClientClick; }
        //    set { ctlMsgCancelImageButton.OnClientClick = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #region Public Method
        public void Show()
        {
           
            GlobalTranslate msg = SuGlobalTranslateQuery.ResolveMessage(msgCode, UserAccount.CurrentLanguageCode.ToString());
            if (MsgType.Equals(MessageTypeEnum.AlertMessage.Confirmation))
            {
                ctlMsgTopicLabel.Text = MessageTypeEnum.AlertMessage.Confirmation.ToString();
                ctlMsgTypeImage.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/MsgConfirmation.gif";

            }
            else if (MsgType.Equals(MessageTypeEnum.AlertMessage.Information))
            {
                ctlMsgTopicLabel.Text = MessageTypeEnum.AlertMessage.Information.ToString();
                ctlMsgCancelImageButton.Visible = false;
                ctlMsgTypeImage.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/MsgInfomaton.gif";


            }
            else if (MsgType.Equals(MessageTypeEnum.AlertMessage.Error))
            {
                ctlMsgTopicLabel.Text = MessageTypeEnum.AlertMessage.Error.ToString();
                ctlMsgCancelImageButton.Visible = false;
                ctlMsgTypeImage.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/MsgError.gif";


            }
            else
            {
                ctlMsgTopicLabel.Text = MessageTypeEnum.AlertMessage.Alert.ToString();
                ctlMsgCancelImageButton.Visible = false;
                ctlMsgTypeImage.ImageUrl = "~/App_Themes/" + this.Page.StyleSheetTheme.ToString() + "/images/MsgWarning.gif";


            }
            ctlMsgHeaderLabel.Text = msgCode;
            if (msg != null)
            {
                ctlMsgBodyLabel.Text = msg.TranslateWord;
            }
            CallOnObjectLookUpCalling();
            this.UpdatePanelMessage.Update();
            this.ctlPanelModalPopupExtender.Show();
        }

        public void Hide()
        {
            this.ctlPanelModalPopupExtender.Hide();
        }
        #endregion

        protected void ctlMsgOKImageButton_Click(object sender, ImageClickEventArgs e)
        {
            CallOnObjectLookUpReturn("OK");
            Hide();
        }

        protected void ctlMsgCancelImageButton_Click(object sender, ImageClickEventArgs e)
        {
            CallOnObjectLookUpReturn("Cancel");
            Hide();
        }

        protected void ctlMsgCloseImageButton_Click(object sender, ImageClickEventArgs e)
        {
            CallOnObjectLookUpReturn("Close");
            Hide();
        }

       
    }
}