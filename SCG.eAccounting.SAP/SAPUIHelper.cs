using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SCG.DB.BLL.Implement;
using SS.SU.BLL.Implement;
using SS.SU.DTO;
using SS.DB.Query;

namespace SCG.eAccounting.SAP
{
    public class SAPUIHelper
    {
        #region public static string ConvertDateTimeToDateString(DateTime DateConvert)
        public static string ConvertDateTimeToDateString(DateTime DateConvert)
        {
            int year = DateConvert.Year;
            int month = DateConvert.Month;
            int day = DateConvert.Day;

            if (year > 2500)
                year = year - 534;

            return year.ToString("0000") + month.ToString("00") + day.ToString("00");
        }
        #endregion public static string ConvertDateTimeToDateString(DateTime DateConvert)

        #region public static DateTime ConvertDateStringToDateTime(string strDate)
        public static DateTime ConvertDateStringToDateTime(string strDate)
        {
            int year = int.Parse(strDate.Substring(0, 4));
            int month = int.Parse(strDate.Substring(4, 2));
            int day = int.Parse(strDate.Substring(6, 4));

            DateTime dateReturn = new DateTime(year,month,day);

            return dateReturn;
        }
        #endregion public static DateTime ConvertDateStringToDateTime(string strDate)

        #region public static Employee GetEmployee(long UserID)
        public static Employee GetEmployee(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);

            Employee emp = new Employee();
            if (suUser != null)
            {
                emp.EmployeeID      = suUser.EmployeeCode;
                emp.EmployeeName    = suUser.EmployeeName;
                emp.UserName        = suUser.UserName;
                emp.CostCenterCode = suUser.CostCenterCode;
                emp.VendorCode = suUser.VendorCode;
            }
            else
            {
                emp.EmployeeID = string.Empty;
                emp.EmployeeName = string.Empty;
                emp.UserName = string.Empty;
                emp.CostCenterCode = string.Empty;
                emp.VendorCode = string.Empty;
            }

            if (dbCompany != null)
            {
                emp.CompanyID = dbCompany.CompanyCode;
                emp.CompanyName = dbCompany.CompanyName;
            }
            else
            {
                emp.CompanyID       = "";
                emp.CompanyName     = "";
            }

            return emp;
        }
        #endregion public static Employee GetEmployee(long UserID)

        public static string GetFixedPostingAccountReturnCr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "113220";
                fixedadvanceAcc = "111250";
            }
            else
            {
                //fixedadvanceAcc = "111250";
                fixedadvanceAcc = "113220";
            }
            return fixedadvanceAcc;
        }


        public static string GetFixedPostingAccountCr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "222110";
                fixedadvanceAcc = "211200";
            }
            else
            {
                fixedadvanceAcc = "222110";
                //fixedadvanceAcc = "211200";
            }
            return fixedadvanceAcc;
        }

        public static string GetFixedPostingAccountCashonHandDr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "100210";
                fixedadvanceAcc = "100010";
            }
            else
            {
                //fixedadvanceAcc = "100010";
                fixedadvanceAcc = "100210";
            }
            return fixedadvanceAcc;
        }

        public static string GetFixedPostingAccountDr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "113220";
                fixedadvanceAcc = "111250";
            }
            else
            {
                //fixedadvanceAcc = "111250";
                fixedadvanceAcc = "113220";
            }
            return fixedadvanceAcc;
        }


        public static string GetFixedPostingAccountAdjustDr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "113220";
                fixedadvanceAcc = "111250";
            }
            else
            {
                //fixedadvanceAcc = "111250";
                fixedadvanceAcc = "113220";
            }
            return fixedadvanceAcc;
        }

        public static string GetFixedPostingAccountAdjustCr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "113220";
                fixedadvanceAcc = "111250";
            }
            else
            {
                //fixedadvanceAcc = "111250";
                fixedadvanceAcc = "113220";
            }
            return fixedadvanceAcc;
        }


        public static string GetFixedPostingAccountReturnDr(long UserID)
        {
            SuUserService suUserService = new SuUserService();
            SuUser suUser = suUserService.FindByIdentity(UserID);
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany = dbComService.FindByIdentity(suUser.Company.CompanyID);
            string fixedadvanceAcc = "";
            if (dbCompany != null)
            {
                //fixedadvanceAcc = "100210";
                fixedadvanceAcc = "100010";
            }
            else
            {
                //fixedadvanceAcc = "100010";
                fixedadvanceAcc = "100210";
            }
            return fixedadvanceAcc;
        }

        #region public static Company GetCompany(string CompanyID)
        public static Company GetCompany(long CompanyID)
        {
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany =  dbComService.FindByIdentity(CompanyID);
            
            Company com = new Company();
            if (dbCompany != null)
            {
                com.CompanyID   = dbCompany.CompanyCode;
                com.CompanyName = dbCompany.CompanyName;
            }
            else
            {
                com.CompanyID   = "";
                com.CompanyName = "";
            }
            return com;
        }
        #endregion public static Company GetCompany(string CompanyID)

        #region public static Company GetCompanyByCode(string CompanyID)
        public static Company GetCompanyByCode(string CompanyID)
        {
            DbCompanyService dbComService = new DbCompanyService();
            DbCompany dbCompany1 = dbComService.getCompanyByCode(CompanyID);

            DbCompany dbCompany = dbComService.FindByIdentity(dbCompany1.CompanyID);

            Company com = new Company();
            if (dbCompany != null)
            {
                com.CompanyID = dbCompany.CompanyCode;
                com.CompanyName = dbCompany.CompanyName;
            }
            else
            {
                com.CompanyID = "";
                com.CompanyName = "";
            }
            return com;
        }
        #endregion public static Company GetCompanyByCode(string CompanyID)

        public static string GetAccountCodeExpMapping(string accountCode, string expenseGroupType)
        {
            DbAccountService service = new DbAccountService();
            return service.GetAccountCodeExpMapping(accountCode, expenseGroupType);
        }

        #region public static string SubString18(string Text)
        public static string SubString18(string Text)
        {
            if (Text.Length > 18)
                return Text.Substring(0, 18);
            else
                return Text;
        }
        #endregion public static string SubString18(string Text)

        #region public static string SubString50(string Text)
        public static string SubString50(string Text)
        {
            if (Text.Length > 50)
                return Text.Substring(0, 50);
            else
                return Text;
        }
        #endregion public static string SubString50(string Text)

        #region public static string SubString35(string Text)
        public static string SubString35(string Text)
        {
            if (Text.Length > 35)
                return Text.Substring(0, 35);
            else
                return Text;
        }
        #endregion public static string SubString35(string Text)

        #region public static string CutLeft(ref string text, int length)
        public static string CutLeft(ref string text, int length)
        {
            string ret;

            if (length >= text.Length)
            {
                ret = text;
                text = "";
            }
            else
            {
                ret = text.Substring(0, length);
                text = text.Substring(length);

            }
            return ret;
        }
        #endregion public static string CutLeft(ref string text, int length)

        #region public static string SubString(int Length , string Text)
        public static string SubString(int Length , string Text)
        {
            if (Text.Length > Length)
                return Text.Substring(0, Length);
            else
                return Text;
        }
        #endregion public static string SubString(int Length , string Text)

        #region public static string PadLeftString(int Length, string Text)
        public static string PadLeftString(int Length, string Text)
        {
            if (Text.ToString().Trim()!="" && Text.Length>0 && Text.Length<Length)
                return Text.PadLeft(Length, '0');
            else
                return Text;
        }
        #endregion public static string PadLeftString(int Length, string Text)

        public static bool IsValidCostCenterForAccountCode(string accountCode)
        {
            if (!accountCode.StartsWith("00008"))
            { 
                return true;
            }
            return false;
        }

        //For IC

        #region public static string GetAccountCodeOfCostCenterForIC(string CostCenterCode)
        /// <summary>
        /// GetAccountCodeOfCostCenterForIC 
        /// return ParameterServices.BAPI_ICC == โรงงาน
        /// retunr ParameterServices.BAPI_IGA == Office
        /// return '' == ไม่มีค่า
        /// </summary>
        /// <param name="CostCenterCode"></param>
        /// <returns></returns>
        public static string GetAccountCodeOfCostCenterForIC(string CostCenterCode)
        {
            if(CostCenterCode.Length>4)
            {
                string strTmp = CostCenterCode.Substring(3, 1);
                
                if (strTmp.Equals("0"))
                    return ParameterServices.BAPI_IGA;
                else
                    return ParameterServices.BAPI_ICC;
            }
            else
                return "";

        }
        #endregion public static string GetAccountCodeOfCostCenterForIC(string CostCenterCode)

        #region public static string ConvertCompanyCodeForIC(string CompanyCode)
        public static string ConvertCompanyCodeForIC(string CompanyCode)
        {
            if (string.IsNullOrEmpty(CompanyCode))
                return "";
            else
            {
                string strTmp = CompanyCode.Substring(0, 3);
                strTmp += "2";

                return PadLeftString(10, strTmp);
            }
        }
        #endregion public static string ConvertCompanyCodeForIC(string CompanyCode)

        public static string GetWHTCodeExpMapping(string WHTCode)
        {
            DbWithHoldingTaxService service = new DbWithHoldingTaxService();
            return service.GetWHTCodeExpMapping(WHTCode);
        }
    }

    #region public struct Employee
    public struct Employee
    {
        public string EmployeeID;
        public string EmployeeName;
        public string CompanyID;
        public string CompanyName;
        public string CostCenterCode;
        public string UserName;
        public string VendorCode;
    }
    #endregion public struct Employee

    #region public struct Company
    public struct Company
    {
        public string CompanyID;
        public string CompanyName;
    }
    #endregion public struct Company
}
