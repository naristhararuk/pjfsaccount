using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class MaintainPersonalLevelEditor : BaseUserControl
    {
        public ISuUserPersonalLevelService SuUserPersonalLevelService { get; set; }
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public string SUPL
        {
            get { return this.ViewState["PersonalLevel"] == null ? FlagEnum.NewFlag : (string)this.ViewState["PersonalLevel"]; }
            set { this.ViewState["PersonalLevel"] = value; }
        }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void ResetValue()
        {
            ctlPersonalLevel.Text = string.Empty;
            ctlDescription.Text = string.Empty;
            ctlActive.Checked = true;
            ctlOrdinal.Text = string.Empty;
            ctlUpdatePanel.Update();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            SuUserPersonalLevel su;

            try
            {
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    su = QueryProvider.SuUserPersonalLevelQuery.FindByIdentity(SUPL);
                }
                else
                {
                    su = new SuUserPersonalLevel();
                }
                su.PersonalLevel = ctlPersonalLevel.Text;
                su.Description = ctlDescription.Text;
                su.Active = ctlActive.Checked;
                su.Ordinal = ctlOrdinal.Text;
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    SuUserPersonalLevelService.UpdateSuUserPersonalLevel(su);
                }
                else if (Mode.Equals(FlagEnum.NewFlag))
                {
                    SuUserPersonalLevelService.AddSuUserPersonalLevel(su);
                }
                Notify_Ok(sender, e);
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
                ctlUpdatePanel.Update();
            }
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ResetValue();
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
        }
        public void HidePopUp()
        {
            ctlMaintainPersonalLevelEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlMaintainPersonalLevelEditorModalPopupExtender.Show();
        }
        public void Initialize(string mode, string supl)
        {
            Mode = mode;
            SUPL = supl;

            if (Mode.Equals(FlagEnum.EditFlag))
            {
                SuUserPersonalLevel su = QueryProvider.SuUserPersonalLevelQuery.FindByIdentity(SUPL);
                ctlPersonalLevel.Text = su.PersonalLevel;
                ctlDescription.Text = su.Description;
                ctlActive.Checked = su.Active;
                ctlOrdinal.Text = su.Ordinal;
                ctlUpdatePanel.Update();
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
            }

        }
    }
}