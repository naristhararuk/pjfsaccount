using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
using SCG.DB.BLL;
using Spring.Validation;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class HolidayEditor : BaseUserControl
    {
        public IDbHolidayService DbHolidayService { get; set; }

        #region Properties
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public Int32 HolidayId
        {
            get { return this.ViewState["HolidayId"] == null ? (Int32)0 : (Int32)this.ViewState["HolidayId"]; }
            set { this.ViewState["HolidayId"] = value; }
        }
        #endregion

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;

        public void ResetValue()
        {
            ctlDate.DateValue = string.Empty;
            ctlDescription.Text = string.Empty;
            ctlHolidayUpdatePanel.Update();
        }

        public void Initialize(string mode, Int32 holidayid, Int32 holidayProfileId,int year)
        {
            Mode = mode;
            HolidayId = holidayid;
            if (Mode.Equals(FlagEnum.EditFlag) || !holidayid.Equals(0))
            {
                DbHoliday holiday = ScgDbQueryProvider.DbHolidayQuery.FindProxyByIdentity(HolidayId);
                ctlDate.DateValue = UIHelper.BindDate(holiday.Date.ToString());
                ctlHolidayProfileIDHidden.Value = holidayProfileId.ToString();
                ctlDescription.Text = holiday.Description;
                ctlHolidayUpdatePanel.Update();
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
                ctlHolidayProfileIDHidden.Value = holidayProfileId.ToString();
                ctlDate.Value = new DateTime(year, DateTime.Now.Month, DateTime.Now.Day);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void ctlInsert_Click(object sender, ImageClickEventArgs e)
        {
            DbHoliday holiday;
            try
            {
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    holiday = ScgDbQueryProvider.DbHolidayQuery.FindByIdentity(HolidayId);
                }
                else
                {
                    holiday = new DbHoliday();
                }
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                try
                {
                    if (!string.IsNullOrEmpty(ctlDate.DateValue))
                    {
                        holiday.Date = UIHelper.ParseDate(ctlDate.DateValue).Value;
                    }
                }
                catch
                {
                    errors.AddError("HolidayProfile.Error", new Spring.Validation.ErrorMessage("UniqueHolidayProfile"));
                    throw new ServiceValidationException(errors);
                }
                holiday.HolidayProfileId = Int32.Parse(ctlHolidayProfileIDHidden.Value);
                holiday.Description = ctlDescription.Text;
                holiday.CreBy = UserAccount.UserID;
                holiday.CreDate = DateTime.Now;
                holiday.UpdBy = UserAccount.UserID;
                holiday.UpdDate = DateTime.Now;
                holiday.UpdPgm = UserAccount.CurrentLanguageCode;

                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbHolidayService.UpdateHoliday(holiday);
                }
                else
                {
                    DbHolidayService.AddHoliday(holiday);
                }
                Notify_Ok(sender, e);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }

            catch (NullReferenceException)
            {
                //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                //errors.AddError("InternalOrder.Error", new ErrorMessage("CostCenter and Company Require."));
                //ValidationErrors.MergeErrors(errors);
                //return;
            }

        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
            HidePopUp();

        }
        public void HidePopUp()
        {
            ctlHolidayModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlHolidayModalPopupExtender.Show();
        }
    }
}