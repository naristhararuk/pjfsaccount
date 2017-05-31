using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.BLL;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.Standard.Utilities;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class CountryEditor : BaseUserControl
    {
        public IDbCountryLangService DbCountryLangService { get; set; }
        public IDbCountryService DbCountryService { get; set; }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;

        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public short CID
        {
            get { return this.ViewState["CID"] == null ? (short)0 : (short)this.ViewState["CID"]; }
            set { this.ViewState["CID"] = value; }
        }

        public void Initialize(string mode, short id)
        {
            Mode = mode.ToString();
            CID = id;
            if (mode.Equals(FlagEnum.EditFlag))
            {
                DbCountry ct = ScgDbQueryProvider.DbCountryQuery.FindByIdentity(CID);
                ctlCountryCode.Text = ct.CountryCode;
                ctlComment.Text = ct.Comment;
                ctlActiveChk.Checked = ct.Active;
                ctlCountryLangGrid.DataCountAndBind();
                UpdatePanelCountryForm.Update();
            }
            else if (mode.ToString() == FlagEnum.NewFlag)
            {
                ResetValue();
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbCountryLangQuery.FindCountryLangByCountryID(CID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            DbCountry ct = new DbCountry();
            if (Mode.Equals(FlagEnum.EditFlag))
            {
                ct.CountryID = CID;
            }

            ct.CountryCode = ctlCountryCode.Text;
            ct.Comment = ctlComment.Text;
            ct.Active = ctlActiveChk.Checked;
            ct.UpdBy = UserAccount.UserID;
            ct.UpdDate = DateTime.Now;
            ct.UpdPgm = UserAccount.CurrentLanguageCode;
            ct.CreDate = DateTime.Now;
            ct.CreBy = UserAccount.UserID;
            try
            {
                // save or update PB
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbCountryService.UpdateCountry(ct);
                }
                else
                {
                   short cId =  DbCountryService.AddCountry(ct);
                   ct.CountryID = cId;
                }
                IList<DbCountryLang> list = new List<DbCountryLang>();

                foreach (GridViewRow row in ctlCountryLangGrid.Rows)
                {
                    short languageId = UIHelper.ParseShort(ctlCountryLangGrid.DataKeys[row.RowIndex]["LanguageID"].ToString());

                    TextBox CountryName = row.FindControl("ctlCountryName") as TextBox;
                    TextBox Comment = (TextBox)row.FindControl("ctlComment") as TextBox;
                    CheckBox Active = (CheckBox)row.FindControl("ctlActive") as CheckBox;

                  
                    DbCountryLang ctLang = new DbCountryLang();
                    ctLang.Active = Active.Checked;
                    ctLang.CreBy = UserAccount.UserID;
                    ctLang.CreDate = DateTime.Now;
                    ctLang.Comment = Comment.Text;
                    ctLang.Language = new DbLanguage(languageId);
                    ctLang.Country = ct;
                    ctLang.UpdBy = UserAccount.UserID;
                    ctLang.UpdDate = DateTime.Now;
                    ctLang.UpdPgm = UserAccount.CurrentLanguageCode;
                    ctLang.CountryName = CountryName.Text;
                    list.Add(ctLang);
                }

                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbCountryLangService.UpdateCountryLang(list);

                }
                if (Mode.Equals(FlagEnum.NewFlag))
                {
                    DbCountryLangService.UpdateCountryLang(list);
                }

                Notify_Ok(sender, e);

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
                UpdatePanelCountryForm.Update();
            }

        }

        protected void ctlCountryLangGrid_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlCountryLangGrid.Rows)
            {
                TextBox ctlCountryName = row.FindControl("ctlCountryName") as TextBox;
                TextBox ctlComment = row.FindControl("ctlComment") as TextBox;
                CheckBox active = row.FindControl("ctlActive") as CheckBox;

                if (string.IsNullOrEmpty(ctlComment.Text))
                {
                    active.Checked = true;
                }
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
            ctlCountryModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlCountryModalPopupExtender.Show();
        }
        public void ResetValue()
        {
            ctlCountryCode.Text = string.Empty;
            ctlComment.Text = string.Empty;
            ctlActiveChk.Checked = true;
            ctlCountryLangGrid.DataCountAndBind();
            UpdatePanelCountryForm.Update();
        }
    }
}