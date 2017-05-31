using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using SCG.eAccounting.DTO.ValueObject;

using SS.SU.Query;
using SS.SU.DTO;

namespace SCG.eAccounting.Web.UserControls.Report
{
    public partial class ExpenseStatementCriteria : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public VOExpenseStatementReport BindCriteria()
        {
            VOExpenseStatementReport vo = new VOExpenseStatementReport();
            string empCodeFrom = string.Empty;
            string empCodeTo = string.Empty;
            SuUser suUserFrom =  QueryProvider.SuUserQuery.FindUserByUserName(ctlFromEmployeeID.UserID,false);
            SuUser suUserTo = QueryProvider.SuUserQuery.FindUserByUserName(ctlToEmployeeID.UserID,false);
            if (suUserFrom != null)
                empCodeFrom = suUserFrom.EmployeeCode;
            if (suUserTo != null)
                empCodeTo = suUserTo.EmployeeCode;
            vo.FromExployeeCode = empCodeFrom;
            vo.ToEmployeeCode = empCodeTo;
            vo.FromDueDate = UIHelper.ParseDate(ctlFromDueDate.DateValue);
            vo.ToDueDate = UIHelper.ParseDate(ctlToDueDate.DateValue);
            vo.DocumentStatus = ctlDocumentStatus.Text;
            vo.ShowParam = string.Format("Employee ID : {0} - {1}, Date : {2} - {3}, Document Status : {4}", new object[] { empCodeFrom, empCodeTo, ctlFromDueDate.DateValue,ctlToDueDate.DateValue,ctlDocumentStatus.Text});
            
            return vo;
        }

    }
}