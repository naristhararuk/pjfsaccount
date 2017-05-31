using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Net;
using System.Net.Sockets;

using SS.Standard.UI;
using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.DTO;
using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Query.Hibernate;
using SS.Standard.Utilities;
using SS.Standard.WorkFlow.Service.Implement;
using SCG.eAccounting.Web.Helper;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.Service;
using SS.DB.Query;

namespace SCG.eAccounting.Web
{
    public partial class DLVReceive : BasePage
    {
        public ISMSService SMSService { get; set; }
        public ISuSmsLogQuery SuSmsLogQuery { get; set; }
        private static string ResponeOK = "<XML><STATUS>OK</STATUS><DETAIL></DETAIL></XML>";
        private string ResponeERR = "<XML><STATUS>ERR</STATUS><DETAIL>{0}</DETAIL></XML>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["SMID"] == null
                    || Request.Form["FROM"] == null
                    || Request.Form["STATUS"] == null
                    || Request.Form["DETAIL"] == null)
            {
                string ResponseError = string.Empty;
                if (Request.Form["SMID"] == null)
                     ResponseError = string.Format(ResponeERR, "Parameter SMID is null");
                else if (Request.Form["FROM"] == null)
                     ResponseError = string.Format(ResponeERR, "Parameter FROM is null");
                else if (Request.Form["STATUS"] == null)
                     ResponseError = string.Format(ResponeERR, "Parameter STATUS is null");
                else if (Request.Form["DETAIL"] == null)
                {
                     ResponseError = string.Format(ResponeERR, "Parameter DETAIL is null");
                }
                else
                {
                    ResponseError = string.Format(ResponeERR, "Parameter is null");
                }

                Response.ClearHeaders();
                Response.Clear();
                Response.Write(ResponseError);
                Response.End();
            }
            else
            {
                string FROM = Request.Form["FROM"] == null ? string.Empty : Request.Form["FROM"].Trim();
                string SMID = Request.Form["SMID"] == null ? string.Empty : Request.Form["SMID"].Trim();
                string STATUS = Request.Form["STATUS"] == null ? string.Empty : Request.Form["STATUS"].Trim();
                string DETAIL = Request.Form["DETAIL"] == null ? string.Empty : Request.Form["DETAIL"].Trim();

                updateSuSmsLog(FROM, SMID, STATUS, DETAIL);
                Response.ClearHeaders();
                Response.Clear();
                Response.Write(ResponeOK);
                Response.End();

            }


            
        }
        private void updateSuSmsLog(string FROM,string SMID,string STATUS,string DETAIL)
        {
            try
            {
                SuSmsLog smslog = FindBySMID(SMID);
                if (smslog != null && smslog.SendMsgSMID.Length > 0)
                {
                    smslog.DlvrRepDate = DateTime.Now;
                    smslog.DlvrRepDetail = DETAIL;
                    smslog.DlvrRepFrom = FROM;
                    smslog.DlvrRepSMID = SMID;
                    smslog.DlvrRepStatus = STATUS;

                    SMSService.UpdateDLVRREP(smslog);
                }
            }
            catch (Exception ex)
            {

                SS.Standard.Utilities.Utilities.WriteLogs("Update DLVRREP : " + ex.Message.Replace("remote", "SMS remote"), "smslog", "Logs", "txt");

            }
           

        }
        private SuSmsLog FindBySMID(string SMID)
        {
            return SuSmsLogQuery.FindBySendMsgSMID(SMID);
        }
    }
}
