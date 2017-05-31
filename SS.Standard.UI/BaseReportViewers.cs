using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using AjaxControlToolkit;

using SS.Standard.Security;

using SS.DB.DTO;
using SS.DB.Query;

using SS.SU.DTO;
using SS.SU.Query;
using SS.SU.BLL;
using System.Net;
using System.Security.Principal;

namespace SS.Standard.UI
{
    [Serializable]
    public class BaseReportViewers : BaseUserControl
    {
        /// <summary>
        /// กำหนดชื่อ Folder ของรายงาน
        /// </summary>
        public string ReportFolderPath { get; set; }

        /// <summary>
        /// กำหนดชื่อของรายงาน ไม่ต้องมีนามสกุลของไฟล์
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// กำหนดความสูง
        /// </summary>
        public int ReportHeight { get; set; }


        /// <summary>
        /// กำหนดความกว้าง
        /// </summary>
        public int ReportWidth { get; set; }

        /// <summary>
        /// กำหนดประเภทของความสูง
        /// </summary>
        public LayoutHeightType SetLayoutHeightType { get; set; }


        /// <summary>
        /// กำหนดประเภทของความสูง
        /// </summary>
        public bool HideParameterOnForm { get; set; }

        /// <summary>
        /// กำหนดว่าจะให้สามารถส่งเมล์ได้
        /// </summary>
        public bool EnableSendEmail { get; set; }

        public GenerateType GenerateFileType {get;set;}

        /// <summary>
        /// กำหนดประเภทของความกว้าง
        /// </summary>
        public LayoutWidthType SetLayoutWidthType { get; set; }

        /// <summary>
        /// กำหนด URL ของ Report Server
        /// Ex. http://hqsqlsvr/reportserver$SCG
        /// </summary>
        public Uri ReportViewerServerUrl { get; set; }


        /// <summary>
        /// กำหนดค่า parameter สำหรับรายงาน
        /// </summary>
        public List<ReportParameters> Parameters { get; set; }


        public bool DocumentMapCollapsed { get; set; }

        public BaseReportViewers()
        { 
        
        }


        /// <summary>
        /// กำหนดความสูงของหน้า Report Viewers
        /// </summary>
        public enum LayoutHeightType
        {
            Pixel,
            Percentage,
            Point
        }

        /// <summary>
        /// กำหนดความกว้างของหน้า Report Viewers
        /// </summary>
        public enum LayoutWidthType
        {
            Pixel,
            Percentage,
            Point
        }
        public enum GenerateType
        {
            Excel,
            Pdf
        }
    }
    /// <summary>
    /// สำหรับกำหนด parameter สำหรับ Report
    /// </summary>
    public class ReportParameters
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {

        // local variable for network credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {

                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {

            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }

      
    }

}
