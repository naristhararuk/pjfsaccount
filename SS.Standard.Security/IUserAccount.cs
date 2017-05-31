using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;

namespace SS.Standard.Security
{
    public interface IUserAccount
    {
        string UserLanguageCode { get; set; }
        string UserLanguageName { get; set; }
        short UserLanguageID { get; set; }
        //short UserDivisionID { get; }
        //short UserOrganizationID { get; }
        Boolean Authentication { get; }
        long UserID { get; }
        void SetLanguage(short l);
        string UserName { get; }
        string EmployeeName { get; set; }
        //string FirstName { get; set; }
        //string LastName { get; set; }
        List<UserRoles> UserRole { get; set; }
        //List<SuUserTransactionPermission> UserTransactionPermission { get; set; }
        short CurrentLanguageID { get; set; }
        string CurrentLanguageCode { get; set; }
        string CurrentLanguageName { get; set; }
        string SessionID { get; }
        string CurrentProgramCode { get; set; }

        bool IsReceiveDocument {get;}
        bool IsVerifyDocument {get;}
        bool IsVerifyPayment {get;}
        bool IsCounterCashier {get;}
        bool IsApproveVerifyPayment {get;}
        bool IsApproveVerifyDocument { get; }
        bool IsAccountant { get; }
        bool IsPayment { get; }
        bool IsAdmin { get; }
        bool IsAllowMultipleApprovePayment { get; }
        bool IsAllowMultipleApproveAccountant { get; }
    }
}
