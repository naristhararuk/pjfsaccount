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
using SS.SU.Query;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class FixedAdvanceDuplicateCriteria : BaseUserControl
    {
        public ISuUserService SuUserService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) { SetBuDropDown(); }
        }
        public void SetBuDropDown()
        {
            ctlBUDropdown.DataSource = ScgDbQueryProvider.DbBuQuery.FindBUALL();
            ctlBUDropdown.DataTextField = "BuName";
            ctlBUDropdown.DataValueField = "BuCode";
            ctlBUDropdown.DataBind();
            ctlBUDropdown.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), string.Empty));
        }
        //protected void ctlRequesterData_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //    SuUser user = (SuUser)e.ObjectReturn;
        //    if (user != null)
        //    {
        //        ctlApproverData.ShowDefaultApprover(UIHelper.ParseLong(ctlRequesterData.EmployeeID));
        //    }
        //    ctlUpdatePanelApprover.Update();
        //}
        public VOFixedAdvanceDuplicateReport BindCriteria()
        {
            VOFixedAdvanceDuplicateReport vo = new VOFixedAdvanceDuplicateReport();
            vo.CompanyID = ctlCompanyField.CompanyID;
            vo.FromDate = UIHelper.ParseDate(ctlFromDateCalendar.DateValue);
            vo.ToDate = UIHelper.ParseDate(ctlToDateCalendar.DateValue);
            vo.BuCode = ctlBUDropdown.SelectedValue;

            SuUser suRequester = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.EmployeeID));
            if (suRequester != null)
            {
                vo.RequesterID = suRequester.Userid.ToString();
            }
            if (UserAccount.IsVerifyDocument || UserAccount.IsApproveVerifyDocument || UserAccount.IsReceiveDocument || UserAccount.IsVerifyPayment || UserAccount.IsCounterCashier)
            {
                vo.CurrenUserID = null;
            }
            else
            {
                vo.CurrenUserID = UserAccount.UserID.ToString();
            }
            string paramList = "Company : " + ctlCompanyField.CompanyCode;
            vo.ParameterList = paramList;


            return vo;
        }
        
    }
}