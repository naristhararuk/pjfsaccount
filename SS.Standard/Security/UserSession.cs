using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SS.Standard.Data;
using System.Collections.Generic;

/// <summary>
/// Summary description for User
/// </summary>
namespace SS.Standard.Security
{
    [Serializable]
    public class UserSession
    {
        private int _UserID;
        private string _UserName;
        private string[] _UserRoleID;
        private string _SessionID;


        public UserSession(int UserID, string UserName,string EmployeeName ,string UserRoleID, string SessionID, int OrganizationID,string DEFAULT_OrganizationName, int CURRENT_LanguageID,List<UserMenu> userMenu,bool Active,DateTime endDate,DateTime startDate)
        {
            this._UserID            = UserID;
            this._UserName          = UserName;
            this.EmployeeName          = EmployeeName;
           // this.LastName           = LastName;
            this._SessionID         = SessionID;
            this.OrganizationID      = OrganizationID;
            this.DEFAULT_OrganizationName    = DEFAULT_OrganizationName;
            this.CURRENT_LanguageID = 1;// CURRENT_LanguageID;

            this.CURRENT_OrganizationID      = OrganizationID;
            this.CURRENT_OrganizationName    = DEFAULT_OrganizationName;
                                                                                  //  DefaultLanguageId
            this.LanguageID = 1; //int.Parse(Provider.DbParameter.getDbParameter(1,6));
            //this.DEFAULT_LANG_NAME = 

            if (UserRoleID != "" && UserRoleID!=null)
            {
                this._UserRoleID = UserRoleID.Split(',');
            }
            this.UserMenu = userMenu;
            this.Active = Active;

            this.EndDate = endDate;
            this.StartDate = startDate;
        }

        public List<UserMenu> UserMenu { get; set; }
        public int CURRENT_OrganizationID {get;set;}
        public string CURRENT_OrganizationName {get;set;}
        public int? CURRENT_LanguageID {get;set;}
        public string CURRENT_LANG_NAME { get; set; }
        public string EmployeeName {get;set;}
       // public string LastName{get;set;}

        public int OrganizationID { get; set; }
        public string DEFAULT_OrganizationName { get; set; }

        public int LanguageID { get; set; }
        public string DEFAULT_LANG_NAME { get; set; }

        public string[] UserRole { get { return _UserRoleID; } }
        public string SessionID {  get { return _SessionID; }   }
        public string UserName { get { return _UserName; } }
        public int UserID { get { return _UserID; } }
        public bool Active { get; set; }
        public int ChurchID { get; set; }
        public short UserTypeID { get; set; }
        public bool PassChange { get; set; }
        public byte FailTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }




    }
}
