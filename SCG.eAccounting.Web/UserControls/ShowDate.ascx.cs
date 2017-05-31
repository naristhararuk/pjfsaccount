using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ShowDate : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtLangID.Value = UserAccount.CurrentLanguageID.ToString();
            
            if (!IsPostBack)
            {
                if (UserAccount.CurrentLanguageID == 1)
                    lblDate.Text = SetDateThai();
                else if (UserAccount.CurrentLanguageID == 2)
                    lblDate.Text = SetDateEng();
            }
        }

        private string SetDateThai()
        {
            int intDay = DateTime.Now.Day;
            int intMonth = DateTime.Now.Month;
            int intYear = DateTime.Now.Year;

            if (intYear < 2500)
                intYear = intYear + 543;

            string strMonth = "";
            if (intMonth == 1)
                strMonth = "มกราคม";
            else if (intMonth == 2)
                strMonth = "กุมภาพันธ์";
            else if (intMonth == 3)
                strMonth = "มีนาคม";
            else if (intMonth == 4)
                strMonth = "เมษายน";
            else if (intMonth == 5)
                strMonth = "พฤษภาคม";
            else if (intMonth == 6)
                strMonth = "มิถุนายน";
            else if (intMonth == 7)
                strMonth = "กรกฎาคม";
            else if (intMonth == 8)
                strMonth = "สิงหาคม";
            else if (intMonth == 9)
                strMonth = "กันยายน";
            else if (intMonth == 10)
                strMonth = "ตุลาคม";
            else if (intMonth == 11)
                strMonth = "พฤศจิกายน";
            else if (intMonth == 12)
                strMonth = "ธันวาคม";

            return intDay.ToString() + " " + strMonth + " " + intYear.ToString();
        }

        private string SetDateEng()
        {
            int intDay = DateTime.Now.Day;
            int intMonth = DateTime.Now.Month;
            int intYear = DateTime.Now.Year;

            if (intYear > 2500)
                intYear = intYear - 543;

            string strMonth = "";
            if (intMonth == 1)
                strMonth = "January";
            else if (intMonth == 2)
                strMonth = "February";
            else if (intMonth == 3)
                strMonth = "March";
            else if (intMonth == 4)
                strMonth = "April";
            else if (intMonth == 5)
                strMonth = "May";
            else if (intMonth == 6)
                strMonth = "June";
            else if (intMonth == 7)
                strMonth = "July";
            else if (intMonth == 8)
                strMonth = "August";
            else if (intMonth == 9)
                strMonth = "September";
            else if (intMonth == 10)
                strMonth = "October";
            else if (intMonth == 11)
                strMonth = "November";
            else if (intMonth == 12)
                strMonth = "December";

            return strMonth + " " + intDay.ToString() + ", " + intYear.ToString();
        }

        private string SetTimeThai()
        {

            return DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
        }

        private string SetTimeEng()
        {
            string strTmp = "";
            int intHour = DateTime.Now.Hour;
            int intTmp = 0;

            if (intHour >= 12)
                strTmp = "P.M.";
            else
                strTmp = "A.M.";

            if (intHour > 12)
                intTmp = intHour - 12;
            else
                intTmp = intHour;

            if (intTmp == 0)
                intTmp = 12;

            return intTmp.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00") + " " + strTmp;
        }
    }
}