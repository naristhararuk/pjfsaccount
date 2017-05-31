using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SS.Standard.Utilities;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ForeignPerdiemRateProfileEditor : BaseUserControl
    {
        public IFnPerdiemProfileService FnPerdiemProfileService { get; set; }
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long PPFID
        {
            get { return this.ViewState["PerdiemProfileID"] == null ? (long)0 : (long)this.ViewState["PerdiemProfileID"]; }
            set { this.ViewState["PerdiemProfileID"] = value; }
        }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        protected void Page_Load(object sender, EventArgs e)
        {
            ProgramCode = "ForeignRateEditor";
        }
        public void ResetValue()
        {
            ctlPerdiemProfileName.Text = string.Empty;
            ctlDescription.Text = string.Empty;
            ctlActive.Checked = false;
            ctlUpdatePanel.Update();
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            FnPerdiemProfile fp;

            try
            {
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    fp = ScgeAccountingQueryProvider.FnPerdiemProfileQuery.FindByIdentity(PPFID);
                }
                else
                {
                    fp = new FnPerdiemProfile();
                }
                fp.PerdiemProfileName = ctlPerdiemProfileName.Text;
                fp.Description = ctlDescription.Text;
                fp.Active = ctlActive.Checked;
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    fp.UpdBy = UserAccount.UserID;
                    fp.UpdDate = DateTime.Now.Date;
                    fp.UpdPgm = ProgramCode;
                    FnPerdiemProfileService.UpdateFnPerdiemProfile(fp);
                }
                else if (Mode.Equals(FlagEnum.NewFlag))
                {
                    fp.UpdBy = UserAccount.UserID;
                    fp.UpdDate = DateTime.Now.Date;
                    fp.UpdPgm = ProgramCode;
                    fp.CreBy = UserAccount.UserID;
                    fp.CreDate = DateTime.Now.Date;
                    FnPerdiemProfileService.AddFnPerdiemProfile(fp);
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
            ctlForeignPerdiemRateProfileEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlForeignPerdiemRateProfileEditorModalPopupExtender.Show();
        }
        public void Initialize(string mode, long ppfid)
        {
            Mode = mode;
            PPFID = ppfid;

            if (Mode.Equals(FlagEnum.EditFlag))
            {
                FnPerdiemProfile fp = ScgeAccountingQueryProvider.FnPerdiemProfileQuery.FindByIdentity(PPFID);
                ctlPerdiemProfileName.Text = fp.PerdiemProfileName;
                ctlDescription.Text = fp.Description;
                ctlActive.Checked = fp.Active;
                ctlUpdatePanel.Update();
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
            }

        }
    }
}