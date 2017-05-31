using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.Security;

namespace SCG.eAccounting.Web
{
    public partial class GenericErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                log4net.ILog logger = log4net.LogManager.GetLogger(Page.ClientID.ToString());
                System.Text.StringBuilder errMsg = new System.Text.StringBuilder();
                string errMsgFormat = "<hr><b>Error Message =</b> {0} <br/> <b>Error Source =</b> {1} <br/> <b>Error Stack =</b> {2}<hr>";

                try
                {
                    SS.SU.DTO.UserSession us = (SS.SU.DTO.UserSession)HttpContext.Current.Session["UserProfiles"];

                    if (us == null)
                    {
                        us = new SS.SU.DTO.UserSession();
                        us.CurrentProgramCode = "Unknown";
                        us.CurrentUserLanguageID = 1;
                    }

                    if (us.CurrentUserLanguageID == 1)
                        errMsg.Append(string.Format("พบข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ (เวลา: {0})",DateTime.Now));
                    else
                        errMsg.Append(string.Format("Any failures found, please contact administrator. (Time: {0})", DateTime.Now));

                    if (Server != null && Server.GetLastError() != null)
                    {
                        Exception ex = Server.GetLastError();
                        while (ex != null)
                        {
                            string ObjectMessage = "Error in Web App <==> ";
                            ObjectMessage += "" + "UserID:" + us.UserID;
                            ObjectMessage += " , " + "UserName:" + us.UserName;
                            ObjectMessage += " , " + "ProgramCode:" + us.CurrentProgramCode;
                            ObjectMessage += " , " + "ClientIP:" + Request.ServerVariables["REMOTE_ADDR"].ToString();
                            //ObjectMessage += "\n" + "Client IP : " + Request.UserHostAddress;

                            logger.Error(ObjectMessage , ex);
                            //errMsg.AppendFormat(errMsgFormat, ex.Message, ex.Source, ex.StackTrace.Replace(" at ", "<br/>at "));
                            ex = ex.InnerException;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error in GenericErrorPage.aspx ", ex);
                    //errMsg.AppendFormat(errMsgFormat, ex.Message, ex.Source, ex.StackTrace);
                }
                finally
                {
                    lblError.Text = errMsg.ToString();
                }
            }
        }
    }
}
