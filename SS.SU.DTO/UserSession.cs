using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// Summary description for User
/// </summary>
namespace SS.SU.DTO
{
    [Serializable]
    public partial class UserSession
    {

        private long userID;
        private string userName;
        private string employeeName;
 
        private string personalWebUrl;

    

        private short currentUserLanguageID;
        private string currentUserLanguageName;
        private string currentUserLanguageCode;



        private short userLanguageID;
        private string userLanguageName;
        private string userLanguageCode;

   
        public string sessionID;

        private string currentProgramCode;


       private List<UserRoles> userRole;


        private bool isReceiveDocument;
        private bool isVerifyDocument;
        private bool isVerifyPayment;
        private bool isCounterCashier;
        private bool isApproveVerifyPayment;
        private bool isApproveVerifyDocument;
        private bool isAccountant;
        private bool isPayment;
        private bool isAdmin;
        private bool isAllowMultipleApprovePayment;
        private bool isAllowMultipleApproveAccountant;

        public virtual bool IsAuthenticated
        {
            get;
            set;
        }

        public virtual long UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }
        public virtual string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public virtual string EmployeeName
        {
            get { return this.employeeName; }
            set { this.employeeName = value; }
        }
 

        public virtual string CurrentProgramCode
        {
            get { return this.currentProgramCode; }
            set { this.currentProgramCode = value; }
        }
        public virtual short CurrentUserLanguageID
        {
            get { return this.currentUserLanguageID; }
            set { this.currentUserLanguageID = value; }
        }
        public virtual string CurrentUserLanguageName
        {
            get { return this.currentUserLanguageName; }
            set { this.currentUserLanguageName = value; }
        }
        public virtual string CurrentUserLanguageCode
        {
            get { return this.currentUserLanguageCode; }
            set { this.currentUserLanguageCode = value; }
        }



        public virtual short UserLanguageID
        {
            get { return this.userLanguageID; }
            set { this.userLanguageID = value; }
        }
        public virtual string UserLanguageName
        {
            get { return this.userLanguageName; }
            set { this.userLanguageName = value; }
        }
        public virtual string UserLanguageCode
        {
            get { return this.userLanguageCode; }
            set { this.userLanguageCode = value; }
        }

  

        public virtual string SessionID
        {
            get { return this.sessionID; }
            set { this.sessionID = value; }
        }

        public virtual List<UserRoles> UserRole
        {
            get { return this.userRole; }
            set { this.userRole = value; }
        }


       //public virtual List<SuUserTransactionPermission> UserTransactionPermission { get; set; }


       public virtual bool IsReceiveDocument
       {
           get { return this.isReceiveDocument; }
           set { this.isReceiveDocument = value; }
       }

       public virtual bool IsVerifyDocument
       {
           get { return this.isVerifyDocument; }
           set { this.isVerifyDocument = value; }
       }

       public virtual bool IsVerifyPayment
       {
           get { return this.isVerifyPayment; }
           set { this.isVerifyPayment = value; }
       }
       public virtual bool IsCounterCashier
       {
           get { return this.isCounterCashier; }
           set { this.isCounterCashier = value; }
       }
       public virtual bool IsApproveVerifyPayment
       {
           get { return this.isApproveVerifyPayment; }
           set { this.isApproveVerifyPayment = value; }
       }
       public virtual bool IsApproveVerifyDocument
       {
           get { return this.isApproveVerifyDocument; }
           set { this.isApproveVerifyDocument = value; }
       }
       public virtual bool IsAccountant
       {
           get { return this.isAccountant; }
           set { this.isAccountant = value; }
       }
       public virtual bool IsPayment
       {
           get { return this.isPayment; }
           set { this.isPayment = value; }
       }
       public virtual bool IsAdmin
       {
           get { return this.isAdmin; }
           set { this.isAdmin = value; }
       }

       public virtual bool IsAllowMultipleApprovePayment
       {
           get { return this.isAllowMultipleApprovePayment; }
           set { this.isAllowMultipleApprovePayment = value; }
       }

       public virtual bool IsAllowMultipleApproveAccountant
       {
           get { return this.isAllowMultipleApproveAccountant; }
           set { this.isAllowMultipleApproveAccountant = value; }
       }

    }
}
