using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SCG.DB.DAL;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class SapInstanceEditor : BaseUserControl
    {
        public IDbSapInstanceService DbSapInstanceService { get; set; }
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public string SAPID
        {
            get { return this.ViewState["Code"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Code"]; }
            set { this.ViewState["Code"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        public int Height
        {
            set { ctlPanelScroll.Height = value; }
        }

        public bool ShowScrollBar
        {
            set
            {
                if (value)
                {
                    ctlPanelScroll.Height = Unit.Pixel(500);
                    ctlPanelScroll.ScrollBars = ScrollBars.Auto;
                }
                else
                {
                    ctlPanelScroll.Height = Unit.Percentage(100);
                    ctlPanelScroll.ScrollBars = ScrollBars.None;
                }
            }
        }
        public void ResetValue()
        {
            ctlSapInstanceCode.Text = string.Empty;
            ctlAliasName.Text = string.Empty;
            ctlSystemID.Text = string.Empty;
            ctlClient.Text = string.Empty;
            ctlUser.Text = string.Empty;
            ctlPassword.Text = string.Empty;
            ctlLanguage.Text = string.Empty;
            ctlSystemNumber.Text = string.Empty;
            ctlMsgServerHost.Text = string.Empty;
            ctlLogonGroup.Text = string.Empty;
            ctlUserCPIC.Text = string.Empty;
            ctlDocTypeExpPostingDM.Text = string.Empty;
            ctlDocTypeExpRmtPostingDM.Text = string.Empty;
            ctlDocTypeExpPostingFR.Text = string.Empty;
            ctlDocTypeExpRmtPostingFR.Text = string.Empty;
            ctlDocTypeExpICPostingFR.Text = string.Empty;
            ctlDocTypeAdvancePostingDM.Text = string.Empty;
            ctlDocTypeAdvancePostingFR.Text = string.Empty;
            ctlDocTypeRmtPosting.Text = string.Empty;
            ctlDocTypeFixedAdvancePosting.Text = string.Empty;
            ctlDocTypeFixedAdvanceReturnPosting.Text = string.Empty;
            ctlUpdatePanel.Update();
        }
        public void Initialize(string mode, string sapid)
        {
            Mode = mode;
            SAPID = sapid;

            if (Mode.Equals(FlagEnum.EditFlag))
            {
                DbSapInstance sa = ScgDbQueryProvider.DbSapInstanceQuery.FindProxyByIdentity(SAPID);
                ctlSapInstanceCode.Enabled = false;
                ctlSapInstanceCode.Text = sa.Code;
                ctlAliasName.Text = sa.AliasName;
                ctlSystemID.Text = sa.SystemID;
                ctlClient.Text = sa.Client.ToString();
                ctlUser.Text = sa.UserName;
                ctlPassword.Text = sa.Password;
                ctlLanguage.Text = sa.Language;
                ctlSystemNumber.Text = sa.SystemNumber.ToString();
                ctlMsgServerHost.Text = sa.MsgServerHost;
                ctlLogonGroup.Text = sa.LogonGroup;
                ctlUserCPIC.Text = sa.UserCPIC;
                ctlDocTypeExpPostingDM.Text = sa.DocTypeExpPostingDM;
                ctlDocTypeExpRmtPostingDM.Text = sa.DocTypeExpRmtPostingDM;
                ctlDocTypeExpPostingFR.Text = sa.DocTypeExpPostingFR;
                ctlDocTypeExpRmtPostingFR.Text = sa.DocTypeExpRmtPostingFR;
                ctlDocTypeExpICPostingFR.Text = sa.DocTypeExpICPostingFR;
                ctlDocTypeAdvancePostingDM.Text = sa.DocTypeAdvancePostingDM;
                ctlDocTypeAdvancePostingFR.Text = sa.DocTypeAdvancePostingFR;
                ctlDocTypeRmtPosting.Text = sa.DocTypeRmtPosting;
                ctlDocTypeFixedAdvancePosting.Text = sa.DocTypeFixedAdvancePosting;
                ctlDocTypeFixedAdvanceReturnPosting.Text = sa.DocTypeFixedAdvanceReturnPosting;
                ctlUpdatePanel.Update();
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ctlSapInstanceCode.Enabled = true;
                ResetValue();
            }

        }
        protected void Add_Click(object sender, ImageClickEventArgs e)
        {
            DbSapInstance sa;

            try
            {
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    sa = ScgDbQueryProvider.DbSapInstanceQuery.FindByIdentity(SAPID);
                }
                else
                {
                    sa = new DbSapInstance();
                }
                sa.Code = ctlSapInstanceCode.Text;
                sa.AliasName = ctlAliasName.Text;
                sa.SystemID = ctlSystemID.Text;
                sa.Client = ctlClient.Text;
                sa.UserName = ctlUser.Text;
                sa.Password = ctlPassword.Text;
                sa.Language = ctlLanguage.Text;
                sa.SystemNumber = ctlSystemNumber.Text;
                sa.MsgServerHost = ctlMsgServerHost.Text;
                sa.LogonGroup = ctlLogonGroup.Text;
                sa.UserCPIC = ctlUserCPIC.Text;
                sa.DocTypeExpPostingDM = ctlDocTypeExpPostingDM.Text;
                sa.DocTypeExpRmtPostingDM = ctlDocTypeExpRmtPostingDM.Text;
                sa.DocTypeExpPostingFR = ctlDocTypeExpPostingFR.Text;
                sa.DocTypeExpRmtPostingFR = ctlDocTypeExpRmtPostingFR.Text;
                sa.DocTypeExpICPostingFR = ctlDocTypeExpICPostingFR.Text;
                sa.DocTypeAdvancePostingDM = ctlDocTypeAdvancePostingDM.Text;
                sa.DocTypeAdvancePostingFR = ctlDocTypeAdvancePostingFR.Text;
                sa.DocTypeRmtPosting = ctlDocTypeRmtPosting.Text;
                sa.DocTypeFixedAdvancePosting = ctlDocTypeFixedAdvancePosting.Text;
                sa.DocTypeFixedAdvanceReturnPosting = ctlDocTypeFixedAdvanceReturnPosting.Text;
                sa.CreBy = UserAccount.UserID;
                sa.CreDate = DateTime.Now;
                sa.UpdBy = UserAccount.UserID;
                sa.UpdDate = DateTime.Now;
                sa.UpdPgm = UserAccount.CurrentLanguageCode;
                //DbSapInstanceService.SaveOrUpdate(sa);
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbSapInstanceService.UpdateSapInstance(sa);
                }
                else if (Mode.Equals(FlagEnum.NewFlag))
                {
                    DbSapInstanceService.AddSapInstance(sa);
                }
                Notify_Ok(sender, e);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanel.Update();
            }
        }
        protected void Cencel_Click(object sender, ImageClickEventArgs e)
        {
            ResetValue();
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
        }
        public void HidePopUp()
        {
            ctlSAPModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlSAPModalPopupExtender.Show();
        }
    }
}