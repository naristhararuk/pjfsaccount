using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.Standard.Utilities;
using SS.DB.DTO.ValueObject;


namespace SS.DB.Query
{
    public class ParameterServices
    {
        //hash pairing parameter name and value.
        private static Dictionary<string, string> hash = null;

        private static Dictionary<string, string> GetData()
        {
            if (hash == null)
                hash = LoadData();
            return hash;
        }

        private static Dictionary<string, string> LoadData()
        {
            // query all dbparameter from DB and put into Hash table
            Dictionary<string, string> innerHash = new Dictionary<string, string>();


            IList<HashBox> listHashing = SsDbQueryProvider.DbParameterQuery.GetAllNameAndValueParameter();
            foreach (HashBox box in listHashing)
            {
                innerHash.Add(box.Key, box.Value);
            }

            return innerHash;
        }

        #region finished 1 st lot
        public static string SMTPMailServer
        {
            get
            {
                return GetData()["SMTPMailServer"];
            }
        }

        /// <summary>
        /// Return Port value for send email through SMTP Server
        /// </summary>
        public static string SMTPPort
        {
            get
            {
                string _value = GetData()["SMTPPort"];
                if (String.IsNullOrEmpty(_value))
                {
                    return "25";
                }
                else
                {
                    return _value;
                }
            }
        }

        /// <summary>
        /// Return User Account value for send email through SMTP Server
        /// </summary>
        public static string SMTPMailUser
        {
            get
            {
                return GetData()["SMTPMailUser"];
            }
        }

        /// <summary>
        /// Return Password value for send email through SMTP Server
        /// </summary>
        public static string SMTPMailPassword
        {
            get
            {
                return EnableDecryptPassword ? PasswordEngine.Decrypt(GetData()["SMTPMailPassword"]) : GetData()["SMTPMailPassword"];
            }
        }

        /// <summary>
        /// Return Email address of Administrator
        /// </summary>
        public static string AdminEmailAddress
        {
            get
            {
                return GetData()["AdminEmailAddress"];
            }
        }

        /// <summary>
        /// Return name of Administrator Name
        /// </summary>
        public static string AdminName
        {
            get
            {
                return GetData()["AdminName"];
            }
        }

        /// <summary>
        /// Return name of Minute for Session Timeout
        /// </summary>
        public static int ApplicationTimeout
        {
            get
            {
                string strApplicationTimeout = GetData()["ApplicationTimeout"];
                try
                {
                    int iApplicationTimeout = int.Parse(strApplicationTimeout);
                    return iApplicationTimeout;
                }
                catch (Exception)
                {
                    return 0;
                }


            }
        }

        /// <summary>
        /// Return Images Files path for the Announcement picture files Upload
        /// </summary>
        public static string AnnouncementUploadFilePath
        {
            get
            {
                return GetData()["AnnouncementUploadFilePath"];
            }
        }

        /// <summary>
        /// Return Images Files path for Icon files
        /// </summary>
        public static string IconUploadFilePath
        {
            get
            {
                return GetData()["IconUploadFilePath"];
            }
        }

        /// <summary>
        /// Return Images Files path for the  Announcement Group Images file Upload
        /// </summary>
        public static string AnnouncementGoupUploadFilePath
        {
            get
            {
                return GetData()["AnnouncementGoupUploadFilePath"];
            }
        }

        /// <summary>
        /// Return Images Files path for Flag image
        /// </summary>
        public static string FlagUploadFilePath
        {
            get
            {
                return GetData()["FlagUploadFilePath"];
            }
        }

        /// <summary>
        /// Return DefaultLanguage ID
        /// </summary>
        public static short DefaultLanguage
        {
            get
            {
                string _value = GetData()["DefaultLanguage"];
                try
                {
                    short shrvalue = short.Parse(_value);
                    return shrvalue;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Return Default Menu
        /// </summary>
        public static string DefaultMenu
        {
            get
            {
                return GetData()["DefaultMenu"];
            }
        }

        #endregion

        #region finished 2 nd lot
        /// <summary>
        /// Return Default SignIn
        /// </summary>
        public static string DefaultSignIn
        {
            get
            {
                return GetData()["DefaultSignIn"];
            }
        }

        /// <summary>
        /// Return Max Password Age
        /// </summary>
        public static int MaxPasswordAge
        {
            get
            {
                string val = GetData()["MaxPasswordAge"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Return Min Password Age
        /// </summary>
        public static int MinPasswordAge
        {
            get
            {
                string val = GetData()["MinPasswordAge"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Return Min Password Length
        /// </summary>
        public static int MinPasswordLength
        {
            get
            {
                string val = GetData()["MinPasswordLength"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Return Default login fail time
        /// </summary>
        public static int DefaultLoginFailTime
        {
            get
            {
                string val = GetData()["DefaultLoginFailTime"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Return Password History Count
        /// </summary>
        public static int PasswordHistoryCount
        {
            get
            {
                string val = GetData()["PasswordHistoryCount"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static string NotAllowPassword
        {
            get
            {
                return GetData()["NotAllowPassword"];
            }
        }

        /// <summary>
        /// Return Application Name
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                return GetData()["ApplicationName"];
            }
        }

        /// <summary>
        /// Return Due Date of Remittance
        /// </summary>
        public static int DueDateOfRemittance
        {
            get
            {
                string val = GetData()["DueDateOfRemittance"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Return Request Date of Remittance
        /// </summary>
        public static int RequestDateOfRemittance
        {
            get
            {
                string val = GetData()["RequestDateOfRemittance"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        #endregion

        #region finished 3 rd lot
        /// <summary>
        /// Return Request Date of Proxy Port
        /// </summary>
        public static string SMSTRANSID
        {
            get
            {
                return GetData()["SMSTRANSID"];
            }
        }

        /// <summary>
        /// Return Request Date of Proxy Port
        /// </summary>
        public static string ProxyPort
        {
            get
            {
                return GetData()["ProxyPort"];
            }
        }

        /// <summary>
        /// Return Request Date of Proxy Server
        /// </summary>
        public static string ProxyServer
        {
            get
            {
                return GetData()["ProxyServer"];
            }
        }

        /// <summary>
        /// Return Request Date of User Proxy
        /// </summary>
        public static string ProxyUserName
        {
            get
            {
                return GetData()["ProxyUserName"];
            }
        }

        /// <summary>
        /// Return Request Date of Password Proxy
        /// </summary>
        public static string ProxyPassword
        {
            get
            {
                return EnableDecryptPassword ? PasswordEngine.Decrypt(GetData()["ProxyPassword"]) : GetData()["ProxyPassword"];
            }
        }


        /// <summary>
        /// Return Request Date of SMSProductionServer
        /// </summary>
        public static string SMSGateWayServer
        {
            get
            {
                return GetData()["SMSGateWayServer"];
            }
        }

        /// <summary>
        /// Return Request Date of SMSProductionPort
        /// </summary>
        public static string SMSGateWayPort
        {
            get
            {
                return GetData()["SMSGateWayPort"];
            }
        }

        /// <summary>
        /// Return Request Date of SMSPhoneNumber
        /// </summary>
        public static string SMSPhoneNumber
        {
            get
            {
                return GetData()["SMSPhoneNumber"];
            }
        }

        /// <summary>
        /// Return Request Date of SMSPhoneNumber
        /// </summary>
        public static string SMSReport
        {
            get
            {
                return GetData()["SMSReport"];
            }
        }

        /// <summary>
        /// Return Request Date of SMSPhoneNumber
        /// </summary>
        public static string SMSCharge
        {
            get
            {
                return GetData()["SMSCharge"];
            }
        }

        #endregion

        #region finished 4 th lot
        public static double DefaultVAT
        {
            get
            {
                string val = GetData()["DefaultVAT"];
                try
                {
                    double dval = double.Parse(val);
                    return dval;
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }
        }

        /// <summary>
        /// Return Required Requester Approval Amount
        /// </summary>
        public static double RequiredRequesterApprovalAmount
        {
            get
            {
                string val = GetData()["RequiredRequesterApprovalAmount"];
                try
                {
                    double dval = double.Parse(val);
                    return dval;
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }
        }

        public static short USDCurrencyID
        {
            get
            {
                string val = GetData()["USDCurrencyID"];
                try
                {
                    short dval = short.Parse(val);
                    return dval;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static long ImportSystemUserID
        {
            get
            {
                string val = GetData()["SystemUserID"];
                try
                {
                    long dval = long.Parse(val);
                    return dval;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static int MaxUploadFileSize
        {
            get
            {
                string val = GetData()["MaxUploadFileSize"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static string LocalAccessUploadFilePath
        {
            get
            {
                return GetData()["LocalAccessUploadFilePath"];
            }
        }


        public static string RecalculateProgramFilePath
        {
            get
            {
                return GetData()["RefreshWorkFlowProgramPath"];
            }
        }



        public static string RelativeUploadFilePath
        {
            get
            {
                return GetData()["RelativeUploadFilePath"];
            }
        }



        public static int SystemUserID
        {
            get
            {
                string val = GetData()["SystemUserID"];
                try
                {
                    int dval = int.Parse(val);
                    return dval;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static int DefaultUserRoleID
        {
            get
            {
                string val = GetData()["DefaultUserRoleID"];
                try
                {
                    int dval = int.Parse(val);
                    return dval;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        #endregion

        #region finished 5 th lot

        public static string AccountOfficialPerdiem
        {
            get
            {
                return GetData()["AccountOfficialPerdiem"];
            }
        }

        public static string AccountInvoicePerdiem
        {
            get
            {
                return GetData()["AccountInvoicePerdiem"];
            }
        }

        public static string AccountPerdiem
        {
            get
            {
                return GetData()["AccountPerdiem"];
            }
        }
        public static string AccountOfficialPerdiem_DM
        {
            get
            {
                return GetData()["AccountOfficialPerdiem_DM"];
            }
        }
        public static string AccountPerdiem_DM
        {
            get
            {
                return GetData()["AccountPerdiem_DM"];
            }
        }
        #endregion

        #region finished 6 th lot

        public static string AccountExpenseForeign
        {
            get
            {
                return GetData()["AccountExpenseForeign"];
            }
        }
        public static string AccountExpenseDomestic
        {
            get
            {
                return GetData()["AccountExpenseDomestic"];
            }
        }

        public static short EnglishLanguageID
        {
            get
            {
                string val = GetData()["EnglishLanguageID"];
                try
                {
                    short ival = short.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public static bool EnableInsertControlCache
        {
            get
            {
                string boolText = GetData()["EnableInsertControlCache"];
                return boolText.ToLower().Equals(bool.TrueString.ToLower());
            }
        }

        /// <summary>
        /// Return value URL In Email Mail Server
        /// </summary>
        public static string UrlLink
        {
            get
            {
                return GetData()["UrlLink"];
            }
        }
        public static string ApproveRejectURL
        {
            get
            {
                return GetData()["ApproveRejectURL"];
            }
        }


        public static string BAPI_SAPCLIENT
        {
            get
            {
                return GetData()["BAPI_SAPCLIENT"];
            }
        }



        public static string BAPI_SAPUSER
        {
            get
            {
                return GetData()["BAPI_SAPUSER"];
            }
        }



        public static string BAPI_SAPPASSWD
        {
            get
            {
                return EnableDecryptPassword ? PasswordEngine.Decrypt(GetData()["BAPI_SAPPASSWD"]) : GetData()["BAPI_SAPPASSWD"];
            }
        }

        #endregion

        #region finished 7 th lot
        public static string BAPI_SAPLANG
        {
            get
            {
                return GetData()["BAPI_SAPLANG"];
            }
        }

        public static string BAPI_SAPASHOST
        {
            get
            {
                return GetData()["BAPI_SAPASHOST"];
            }
        }

        public static string BAPI_SAPSYSNR
        {
            get
            {
                return GetData()["BAPI_SAPSYSNR"];
            }
        }

        public static string BAPI_OpenSimulator
        {
            get
            {
                return GetData()["BAPI_OpenSimulator"];
            }
        }

        public static string BAPI_ARAccount
        {
            get
            {
                return GetData()["BAPI_ARAccount"];
            }
        }

        public static string BAPI_GLAccount
        {
            get
            {
                return GetData()["BAPI_GLAccount"];
            }
        }

        public static string BAPI_UserCPIC
        {
            get
            {
                return GetData()["BAPI_UserCPIC"];
            }
        }

        public static string BAPI_WHTAccount
        {
            get
            {
                return GetData()["BAPI_WHTAccount"];
            }
        }

        public static string BAPI_MsgServerHost
        {
            get
            {
                return GetData()["BAPI_MsgServerHost"];
            }
        }

        public static string BAPI_LogonGroup
        {
            get
            {
                return GetData()["BAPI_LogonGroup"];
            }
        }

        public static string BAPI_SAPSystemName
        {
            get
            {
                return GetData()["BAPI_SAPSystemName"];
            }
        }

        /// <summary>
        /// ลูกหนี้ประกันสังคม
        /// </summary>
        public static string BAPI_SpecialGLType1
        {
            get
            {
                return GetData()["BAPI_SpecialGLType1"];
            }
        }

        /// <summary>
        /// ลูกหนี้ที่ไม่ต้องส่ง TAX_CODE
        /// </summary>
        public static string BAPI_SpecialGLType2
        {
            get
            {
                return GetData()["BAPI_SpecialGLType2"];
            }
        }

        // Case IC
        public static string BAPI_OSI
        {
            get
            {
                return GetData()["BAPI_OSI"];
            }
        }

        public static string BAPI_FEXP_TEXT
        {
            get
            {
                return GetData()["BAPI_FEXP_TEXT"];
            }
        }

        public static string BAPI_FEXP_TEXT_DOMESTIC
        {
            get
            {
                return GetData()["BAPI_FEXP_TEXT_DOMESTIC"];
            }
        }

        public static string BAPI_Defer_Vat_S7
        {
            get
            {
                return GetData()["BAPI_Defer_Vat_S7"];
            }
        }
        public static string BAPI_Defer_Vat_D7
        {
            get
            {
                return GetData()["BAPI_Defer_Vat_D7"];
            }
        }

        public static string BAPI_SAV
        {
            get
            {
                return GetData()["BAPI_SAV"];
            }
        }

        public static string BAPI_ICC
        {
            get
            {
                return GetData()["BAPI_ICC"];
            }
        }

        public static string BAPI_IGA
        {
            get
            {
                return GetData()["BAPI_IGA"];
            }
        }

        public static string BAPI_IC_WHTType
        {
            get
            {
                return GetData()["BAPI_IC_WHTType"];
            }
        }

        public static string BAPI_IC_WHTType_AR
        {
            get
            {
                return GetData()["BAPI_IC_WHTType_AR"];
            }
        }

        public static string BAPI_IC_WHTCode
        {
            get
            {
                return GetData()["BAPI_IC_WHTCode"];
            }
        }

        public static string BAPI_ALIAS_NAME
        {
            get
            {
                return GetData()["BAPI_ALIAS_NAME"];
            }
        }

        #endregion

        #region finished 8 th lot

        /// <summary>
        /// Return Request of ReportUserName
        /// </summary>
        public static string ReportUserName
        {
            get
            {
                return GetData()["ReportUserName"];
            }
        }

        /// <summary>
        /// Return Request of ReportPassword
        /// </summary>
        public static string ReportPassword
        {
            get
            {
                return (EnableDecryptPassword ? PasswordEngine.Decrypt(GetData()["ReportPassword"]) : GetData()["ReportPassword"]);
            }
        }


        /// <summary>
        /// Return Request of ReportDomainName
        /// </summary>
        public static string ReportDomainName
        {
            get
            {
                return GetData()["ReportDomainName"];
            }
        }

        /// <summary>
        /// Return Request of ReportingURL
        /// </summary>
        public static string ReportingURL
        {
            get
            {
                return GetData()["ReportingURL"];
            }
        }

        /// <summary>
        /// Return Request of ReportDomainName
        /// </summary>
        public static string ReportFolderPath
        {
            get
            {
                return GetData()["ReportFolderPath"];
            }
        }
        /// <summary>
        /// Return Request of ReportingViewersURL
        /// </summary>
        public static string ReportingViewersURL
        {
            get
            {
                string strReportingViewersURL = GetData()["ReportingViewersURL"];
                return strReportingViewersURL;
            }
        }

        #endregion

        #region finished 9 th lot

        public static string RemainDateToSendEMail
        {
            get
            {
                return GetData()["RemainDateToSendEMail"];
            }
        }

        public static string DropDownListCount
        {
            get
            {
                return GetData()["DropDownListCount"];
            }
        }

        public static string MsgDataNotFound
        {
            get
            {
                return GetData()["MsgDataNotFound"];
            }
        }

        public static string DefaultAnnoucementGroup
        {
            get
            {
                return GetData()["DefaultAnnoucementGroup"];
            }
        }

        public static string DisplayNewImage
        {
            get
            {
                return GetData()["DisplayNewImage"];
            }
        }

        public static string FilePathService
        {
            get
            {
                return GetData()["FilePathService"];
            }
        }

        public static string AVCODE
        {
            get
            {
                return GetData()["AVCODE"];
            }
        }

        public static string EXPCODE
        {
            get
            {
                return GetData()["EXPCODE"];
            }
        }

        public static string RMCODE
        {
            get
            {
                return GetData()["RMCODE"];
            }
        }

        #endregion

        #region finished 10 th lot
        public static string DefaultRejectReasonCodeFromMail
        {
            get
            {
                return GetData()["DefaultRejectReasonCodeFromMail"];
            }
        }

        public static string AccountPerdiemDomestic
        {
            get
            {
                return GetData()["AccountPerdiemDomestic"];
            }
        }

        #endregion

        public static string AccountMileageGov
        {
            get
            {
                return GetData()["AccountMileageGov"];
            }
        }

        public static string AccountMileageExtra
        {
            get
            {
                return GetData()["AccountMileageExtra"];
            }
        }

        public static string AccountMileageCompany
        {
            get
            {
                return GetData()["AccountMileageCompany"];
            }
        }

        public static string AllowEveryOneViewDocument
        {
            get
            {
                return GetData()["AllowEveryOneViewDocument"];
            }
        }

        public static string RefreshWorkFlowComputerName
        {
            get
            {
                return GetData()["RefreshWorkFlowComputerName"];
            }
        }

        public static string DefaultTaxCode
        {
            get
            {
                return GetData()["DefaultTaxCode"];
            }
        }

        public static string DefaultWHTCode
        {
            get
            {
                return GetData()["DefaultWHTCode"];
            }
        }

        public static bool EnableDocumentLock
        {
            get
            {
                return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableDocumentLock"]);
            }
        }

        public static bool EnableSMS
        {
            get
            {
                return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableSMS"]);
            }
        }
        public static string NotifyApprovedCompleted
        {
            get
            {
                return GetData()["NotifyApprovedCompleted"];
            }
        }
        public static string NotifyRequestApproveAdvance
        {
            get
            {
                return GetData()["NotifyRequestApproveAdvance"];
            }
        }
        public static string NotifyPaymentHasMadeSMS02
        {
            get
            {
                return GetData()["NotifyPaymentHasMadeSMS02"];
            }
        }
        public static string NotifyPaymentHasMadeSMS03
        {
            get
            {
                return GetData()["NotifyPaymentHasMadeSMS03"];
            }
        }
        public static string NotifyRequestApproveExpense
        {
            get
            {
                return GetData()["NotifyRequestApproveExpense"];
            }
        }

        public static string NotifyApproveWrongFormat
        {
            get
            {
                return GetData()["NotifyApproveWrongFormat"];
            }
        }
        public static string NotifyTokenCodeNotFound
        {
            get
            {
                return GetData()["NotifyTokenCodeNotFound"];
            }
        }
        public static string NotifyWrongMobileNumber
        {
            get
            {
                return GetData()["NotifyWrongMobileNumber"];
            }
        }

        public static short SMSDefaultLanguageID
        {
            get
            {
                return SS.Standard.Utilities.Utilities.ParseShort(GetData()["SMSDefaultLanguageID"]);
            }
        }



        public static string NotifyLoginFailed
        {
            get
            {
                return GetData()["NotifyLoginFailed"];
            }
        }
        public static string NotifyDocStatusChanged
        {
            get
            {
                return GetData()["NotifyDocStatusChanged"];
            }
        }

        public static string NotifyChequeReceive
        {
            get
            {
                return GetData()["NotifyChequeReceive"];
            }
        }

        public static string NotifyCashReceive
        {
            get
            {
                return GetData()["NotifyCashReceive"];
            }
        }

        public static string NotifyRequestApproveTA
        {
            get
            {
                return GetData()["NotifyRequestApproveTA"];
            }
        }
        public static string NotifyRequestApproveTAandAdvance
        {
            get
            {
                return GetData()["NotifyRequestApproveTAandAdvance"];
            }
        }
        public static string AllowExtension
        {
            get
            {
                return GetData()["AllowExtension"];
            }
        }
        public static void Neologize()
        {
            hash = null;
        }

        public static bool EnableSessionViewState
        {
            get
            {
                return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableSessionViewState"]);
            }
        }
        public static int RefreshWorkFlowPermissionListernerPort
        {
            //defualt port 13777
            get
            {
                return SS.Standard.Utilities.Utilities.ParseInt(GetData()["RefreshWorkFlowPermissionListernerPort"]);
            }
        }
        public static int RefreshWorkFlowPermissionProcessTime
        {
            //default 1 Minute
            get
            {
                return SS.Standard.Utilities.Utilities.ParseInt(GetData()["RefreshWorkFlowPermissionProcessTime"]);
            }
        }
        public static int LimitAmountForVerifierChange
        {
            get
            {
                return SS.Standard.Utilities.Utilities.ParseInt(GetData()["LimitAmountForVerifierChange"]);
            }
        }

        /// <summary>
        /// Return Min Password Length
        /// </summary>
        public static int MaxPasswordLength
        {
            get
            {
                string val = GetData()["MaxPasswordLength"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static int LimitLoginUserAmount
        {
            get
            {
                string val = GetData()["LimitLoginUserAmount"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        #region Email Activation
        public static bool EnableEmail01
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail01"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail03
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail03"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail04
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail04"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail05
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail05"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail06
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail06"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail07
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail07"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail08
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail08"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail09
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail09"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail10
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail10"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail11
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail11"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02CC
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02CC"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02Initiator
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02Initiator"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02Approver
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02Approver"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02Requester
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02Requester"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02Creator
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02Creator"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }
        public static bool EnableEmail02Receiver
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail02Receiver"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }

        public static int MaxRetry
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseInt(GetData()["MaxRetry"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static int EmailPendingDuration
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseInt(GetData()["EmailPendingDuration"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static int EmailFlushingDuration
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseInt(GetData()["EmailFlushingDuration"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static bool EnableEmail014
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail014"]);
                }
                catch (Exception)
                {
                    return true;
                }
            }
        }

        /*เพิ่มแจ้งเตือนก่อนครบกำหนด fixedadvance*/
        public static int EMailAlertBeforeDay1
        {
            get
            {
                string val = GetData()["EMailAlertBeforeDay1"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public static int EMailAlertBeforeDay2
        {
            get
            {
                string val = GetData()["EMailAlertBeforeDay2"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public static int EMailAlertOverdueDay1
        {
            get
            {
                string val = GetData()["EMailAlertOverdueDay1"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static bool EnableEmail15
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail15"]);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public static bool EnableEmail16
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableEmail15"]);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Return Perdiem Rate
        /// </summary>
        public static int PerdiemRate
        {
            get
            {
                string val = GetData()["PerdiemRate"];
                try
                {
                    int ival = int.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        public static bool EnableSSLOnLoginPage
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableSSLOnLoginPage"]);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static string ServiceAccountDomainName
        {
            get
            {
                return GetData()["ServiceAccountDomainName"];
            }
        }
        public static string EM08PasswordRemarkEN
        {
            get
            {
                return GetData()["EM08_PasswordRemarkEN"];
            }
        }
        public static string EM08PasswordRemarkTH
        {
            get
            {
                return GetData()["EM08_PasswordRemarkTH"];
            }
        }

        public static bool EnableLoginWithActiveDirectory
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableLoginWithActiveDirectory"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static string ActiveDirectoryLogin
        {
            get
            {
                return GetData()["ActiveDirectoryLogin"];
            }
        }
        public static string ActiveDirectoryPassword
        {
            get
            {
                return EnableDecryptPassword ? PasswordEngine.Decrypt(GetData()["ActiveDirectoryPassword"]) : GetData()["ActiveDirectoryPassword"];
            }
        }

        public static double MotorcycleRateForMileageCalculation
        {
            get
            {
                string val = GetData()["MotorcycleRateForMileageCalculation"];
                try
                {
                    double ival = double.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static double OtherRateForMileageCalculation
        {
            get
            {
                string val = GetData()["OtherRateForMileageCalculation"];
                try
                {
                    double ival = double.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static double OtherFirstDistance
        {
            get
            {
                string val = GetData()["OtherFirstDistance"];
                try
                {
                    double ival = double.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static double MotorcycleFirstDistance
        {
            get
            {
                string val = GetData()["MotorcycleFirstDistance"];
                try
                {
                    double ival = double.Parse(val);
                    return ival;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static string GlobalCatalogs
        {
            get
            {
                return GetData()["GlobalCatalogs"];
            }
        }

        public static string ArchiveUrl
        {
            get
            {
                return GetData()["ArchiveUrl"];
            }
        }

        public static bool EnableLockExpenseChangeAmount
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableLockExpenseChangeAmount"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool EnableShowWarningMsgAmountHasBeenCorrected
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableShowWarningMsgAmountHasBeenCorrected"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool EnableDecryptPassword
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableDecryptPassword"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static bool EditableBusinessArea
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EditableBusinessArea"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static bool EditableSupplementary
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EditableSupplementary"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static string eXpenseUrl
        {
            get
            {
                return GetData()["eXpenseUrl"];
            }
        }

        public static string ECCUrl
        {
            get
            {
                return GetData()["ECCUrl"];
            }
        }

        public static string PersonalTaxFormatRegEx
        {
            get
            {
                return GetData()["PersonalTaxFormatRegEx"];
            }
        }
        public static string ADF_Default_BankAccount
        {
            get
            {
                return GetData()["ADF_Default_BankAccount"];
            }
        }
        public static string Default_Vendor
        {
            get
            {
                return GetData()["Default_Vendor"];
            }
        }
        public static bool RequireVendorBranchCode
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["RequireVendorBranchCode"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static string PayRollReportPath
        {
            get
            {
                return GetData()["PayRollReportPath"];
            }
        }
        public static string PayRollReportFormatFilename
        {
            get
            {
                return GetData()["PayRollReportFormatFilename"];
            }
        }
		public static string Paymentterm
        {
            get
            {
                return GetData()["BAPI_Paymentterm"];
            }
        }

        public static string PaymenttermIC
        {
            get
            {
                return GetData()["BAPI_PaymenttermIC"];
            }
        }

        public static bool EnableInvoiceExcludePerdiemDomesticAccount
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseBool(GetData()["EnableInvoiceExcludePerdiemDomesticAccount"]);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static string Column1_OverdueDayFrom
        {
            get
            {
                return GetData()["Column1_OverdueDayFrom"];
            }
        }

        public static string Column1_OverdueDayTo
        {
            get
            {
                return GetData()["Column1_OverdueDayTo"];
            }
        }

        public static string Column2_OverdueDayFrom
        {
            get
            {
                return GetData()["Column2_OverdueDayFrom"];
            }
        }

        public static string Column2_OverdueDayTo
        {
            get
            {
                return GetData()["Column2_OverdueDayTo"];
            }
        }

        public static string Column3_OverdueDayFrom
        {
            get
            {
                return GetData()["Column3_OverdueDayFrom"];
            }
        }

        public static string Column3_OverdueDayTo
        {
            get
            {
                return GetData()["Column3_OverdueDayTo"];
            }
        }

        public static string Column4_OverdueDayFrom
        {
            get
            {
                return GetData()["Column4_OverdueDayFrom"];
            }
        }

        public static string Column4_OverdueDayTo
        {
            get
            {
                return GetData()["Column4_OverdueDayTo"];
            }
        }

        public static string Column5_OverdueDayFrom
        {
            get
            {
                return GetData()["Column5_OverdueDayFrom"];
            }
        }

        public static string Column5_OverdueDayTo
        {
            get
            {
                return GetData()["Column5_OverdueDayTo"];
            }
        }

        public static string EnableCA
        {
            get
            {
                return GetData()["EnableCA"];
            }
        }

        public static string EnableMPA
        {
            get
            {
                return GetData()["EnableMPA"];
            }
        }

        public static string EnableCheckboxNotifyRemindWaitApprove
        {
            get
            {
                return GetData()["EnableCheckboxNotifyRemindWaitApprove"];
            }
        }

        public static string FixedAdvanceConfigEffectiveDate
        {
            get
            {
                string result;
                if (!string.IsNullOrEmpty(GetData()["FixedAdvanceConfigEffectiveDate"]))
                {
                    result = GetData()["FixedAdvanceConfigEffectiveDate"];
                }
                else
                {
                    result = "12";
                }
                return result;
            }
        }

        public static string WaitForDocumentFollowupDay
        {
            get
            {
                return GetData()["WaitForDocumentFollowupDay"];
            }
        }

        public static string WaitForDocumentRepeatFollowupDay
        {
            get
            {
                return GetData()["WaitForDocumentRepeatFollowupDay"];
            }
        }

        public static int RefreshInterval
        {
            get
            {
                try
                {
                    return SS.Standard.Utilities.Utilities.ParseInt(GetData()["RefreshInterval"]);
                }
                catch (Exception ex)
                {
                    return 1000;
                }
            }
        }

    }
}