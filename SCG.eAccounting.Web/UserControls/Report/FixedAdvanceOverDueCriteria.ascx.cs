using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.SU.BLL;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SS.SU.DTO;
using SCG.DB.DTO;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class FixedAdvanceOverDueCriteria : BaseUserControl
    {
        public ISuUserService SuUserService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCompanyField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCompanyField_OnObjectReturn);
        }
        public VOFixedAdvanceOverDueReport BindCriteria()
        {
            VOFixedAdvanceOverDueReport vo = new VOFixedAdvanceOverDueReport();
            vo.CompanyID = UIHelper.ParseLong(ctlCompanyField.CompanyID);
            vo.FromLocationCode = ctlFromLocationField.LocationCode;
            vo.ToLocationCode = ctlToLocationField.LocationCode;
            vo.FromDueDate = ctlFromDueDateCalendar.DateValue;
            vo.ToDueDate = ctlToDueDateCalendar.DateValue;
            vo.FromFixedAdvanceAmount = UIHelper.ParseDouble(ctlFromFixedAdvanceAmount.Text);
            vo.ToFixedAdvanceAmount = UIHelper.ParseDouble(ctlToFixedAdvanceAmount.Text);
            vo.FromOverDue = UIHelper.ParseInt(ctlFromOverdueDay.Text);
            vo.ToOverDue = UIHelper.ParseInt(ctlToOverdueDay.Text);
            vo.FixedAdvanceType = ctlFixedAdvanceType.SelectedValue;
            if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
            {
                IList<SuUser> userList = SuUserService.FindByUserName(ctlRequesterData.UserID);
                if (userList.Count > 0)
                {
                    long userid = userList[0].Userid;
                    vo.RequesterID = userid;
                }
            }

            //vo.LanguageID = UserAccount.CurrentLanguageID;
            return vo;
        }

        protected void ctlCompanyField_OnObjectReturn(object sender, ObjectLookUpReturnArgs e)
        {
            if (e.ObjectReturn != null)
            {
                DbCompany company = (DbCompany)e.ObjectReturn;
                ctlCompanyField.CompanyID = company.CompanyID.ToString();
                ctlFromLocationField.CompanyId = company.CompanyID;
                ctlToLocationField.CompanyId = company.CompanyID;
            }
            else
            {
                ctlToLocationField.ResetValue();
                ctlFromLocationField.ResetValue();
            }
        }
    }
}