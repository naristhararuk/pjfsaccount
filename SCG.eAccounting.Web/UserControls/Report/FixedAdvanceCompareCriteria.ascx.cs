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
using log4net;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.BLL.Implement;
using SS.SU.Query;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class FixedAdvanceCompareCriteria : BaseUserControl
    {
        public ISuUserService SuUserService { get; set; }
        private static ILog logger = log4net.LogManager.GetLogger(typeof(FixedAdvanceCompareCriteria));
        protected void Page_Load(object sender, EventArgs e)
        {
            //ctlRequesterData.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlRequesterData_OnObjectLookUpReturn);
            //ctlToDateCalendar.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlToDate_OnObjectLookUpReturn);

            //if (UserCulture.CultureTypes == "1")
            //{
            //    ctlReportType.Items[0].Text = "แสดงยอดรวมการเบิกและกราฟ";
            //    ctlReportType.Items[1].Text = "แสดงรายละเอียดการเบิกตามประเภทค่าใช้จ่าย";
            //}
            //else
            //{
            //    ctlReportType.Items[0].Text = "แสดงยอดรวมการเบิกและกราฟ";
            //    ctlReportType.Items[1].Text = "แสดงรายละเอียดการเบิกตามประเภทค่าใช้จ่าย";
            //}
            //ctlReportType.Items[1].Value = "";
        }

        public VOFixedAdvanceCompareReport BindCriteria()
        {
            VOFixedAdvanceCompareReport vo = new VOFixedAdvanceCompareReport();
            vo.FromDate = UIHelper.ParseDate(ctlFromDateCalendar.DateValue);
            vo.DocumentNo = ctlDocumentNo.Text;
            vo.ToDate = UIHelper.ParseDate(ctlToDateCalendar.DateValue);
            vo.ReportType = ctlReportType.SelectedValue;

            SuUser suRequester = QueryProvider.SuUserQuery.FindByIdentity(UIHelper.ParseLong(ctlRequesterData.EmployeeID));

            if (suRequester != null)
            {
                vo.RequesterID = suRequester.Userid.ToString();
            }
            if (UserAccount.IsVerifyDocument || UserAccount.IsApproveVerifyDocument || UserAccount.IsReceiveDocument || UserAccount.IsVerifyPayment || UserAccount.IsCounterCashier)
            {
                vo.ApproverID = null; //CuurentUserId
            }
            else
            {
                vo.ApproverID = UserAccount.UserID.ToString(); //CuurentUserId
            }
            //if (!string.IsNullOrEmpty(ctlRequesterData.UserID))
            //{
            //    IList<SuUser> userList = SuUserService.FindByUserName(ctlRequesterData.UserID);
            //    if (userList.Count > 0)
            //    {
            //        long userid = userList[0].Userid;
            //        vo.RequesterID = userid;
            //    }
            //}

            //if (!string.IsNullOrEmpty(ctlApproverData.UserID))
            //{
            //    IList<SuUser> userList = SuUserService.FindByUserName(ctlApproverData.UserID);
            //    if (userList.Count > 0)
            //    {
            //        long userid = userList[0].Userid;
            //        vo.ApproverID = userid;
            //    }
            //}

            //vo.LanguageID = UserAccount.CurrentLanguageID;
            return vo;
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

        //protected void ctlToDate_OnObjectLookUpReturn(object sender, ObjectLookUpCallingEventArgs e)
        //{
        //    DateTime fromDate = (DateTime)UIHelper.ParseDate(ctlFromDateCalendar.DateValue);
        //    DateTime toDate = (DateTime)UIHelper.ParseDate(ctlFromDateCalendar.DateValue);
        //    DateTime setYear = fromDate.AddMonths(12);
        //    if (setYear < toDate)
        //    {
        //        DateMaximumRange.Visible = true;
        //    }
        //    ctlValidateDate.Update();
        //}
    }
}