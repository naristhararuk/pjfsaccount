using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SS.Standard.Data;
using System.Text;
using AjaxControlToolkit;
using SS.Standard.Security;
using SS.Standard.Data.Mssql;
using System.Data.SqlClient;
using SS.Standard.Data.Interfaces;

namespace SS.Standard.AlertMsg
{
    public class Dialog : MssqlHelper, IDBManager
    {
        MasterPage masterpage;

        #region IDBManager Members
        public void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

        }
        
        public void CloseConnection()
        {
            Connection.Close();
        }
        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                Transaction = Connection.BeginTransaction();
            }
            else
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            Transaction.Commit();
        }
        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }
        #endregion

        public Dialog()
        {
        }
        public Dialog(MasterPage masterpage)
        {
            this.masterpage = masterpage;
        }

        public Dialog(object masterpage)
        {
            this.masterpage = (MasterPage)masterpage;
        }

        public static string getSqlExceptionNumber(Exception ex)
        {
            return Provider.MessageDAL.GetSqlExceptionNumber(ex);
        }

        public void Msg(string id)
        {
            string DialogHeader = string.Empty, DialogTopic = string.Empty, DialogMsg = string.Empty, DialogSolutions=string.Empty;
            StringBuilder sql = new StringBuilder(string.Empty);
            SqlCommand _SqlCommand = null;

            if (UserAccount.CURRENT_LanguageID == null || UserAccount.CURRENT_LanguageID == UserAccount.LanguageID)
            {
                _SqlCommand = getMsgDefaultLanguage(id);              
            }
            else
            {
                _SqlCommand = getMsgAccountLanguage(id);
            }
            OpenConnection();
            IDataReader dr = ExecuteReader(_SqlCommand);
            while (dr.Read())
            {
                DialogHeader = dr["ALERT_GRP_NAME"].ToString();
                DialogTopic = dr["TOPIC"].ToString();
                DialogMsg = dr["MSG"].ToString();
                DialogSolutions = dr["SOLUTION"].ToString();
            }
            dr.Close();
            CloseConnection();
            showMessage(DialogHeader, DialogTopic, DialogMsg,string.Empty, msgType.Message);

        }
        public void Msg(string title, string topic, string msg)
        {
            (masterpage.FindControl("DivDialog") as HtmlGenericControl).Style["display"] = "inline";
            (masterpage.FindControl("divSolution") as HtmlGenericControl).Style["display"] = "inline";
            (masterpage.FindControl("DialogOkButton") as Button).Text = "OK";
            (masterpage.FindControl("divCancel") as HtmlGenericControl).Style["display"] = "none";
            (masterpage.FindControl("divOk") as HtmlGenericControl).Style["display"] = "inline";
            (masterpage.FindControl("DialogHeader") as Label).Text = title;
            (masterpage.FindControl("DialogTopic") as Label).Text = topic;
            (masterpage.FindControl("DialogMsg") as Label).Text = msg;
            (masterpage.FindControl("ModalPopupExtender") as ModalPopupExtender).Show();

        }

        public void Error(Exception ex)
        {
            AlertMessage msg = Provider.MessageDAL.GetMessage(ex);
            showMessage(msg.DialogHeader, msg.DialogTopic, msg.DialogMsg, msg.DialogSolutions, msgType.Error);
        }

         public void Error(string id)
        {
            AlertMessage msg = Provider.MessageDAL.GetMessage(id);
            showMessage(msg.DialogHeader, msg.DialogTopic, msg.DialogMsg, msg.DialogSolutions, msgType.Error);
           
        }
        public void Confirm()
        {
            (masterpage.FindControl("DivDialog") as HtmlGenericControl).Style["display"] = "inline";
            (masterpage.FindControl("showModalPopupClientButton") as Button).Text = "GOGO!";
            (masterpage.FindControl("DialogSolution") as Label).Visible = false;
            (masterpage.FindControl("DialogCancelButton") as Button).Text = "Cancel";
            (masterpage.FindControl("DialogOkButton") as Button).Visible = true;
            (masterpage.FindControl("DialogOkButton") as Button).Text = "OK";
            (masterpage.FindControl("ModalPopupExtender") as ModalPopupExtender).Show();
        }

        public string MsgID(string id)
        {
            AlertMessage msg = Provider.MessageDAL.GetMessage(id);
            string message = "Message Header" + "!@#$%" + "Message Topic" + "!@#$%" + "Message";
                   message += msg.DialogHeader + "!@#$%" + msg.DialogTopic + "!@#$%" + msg.DialogMsg;
            return message;
           
        }

        private void showMessage(string MsgHeader,string MsgTopic,string MsgBody,string MsgSolution,msgType AlertType)
        {
            string errorMsgBody = "Please Contact AdministratorTel 02-xxx-xxxx";
            if (MsgHeader.Equals(string.Empty) && MsgTopic.Equals(string.Empty) && MsgBody.Equals(string.Empty))
            {
                (masterpage.FindControl("DialogHeader") as Label).Text = msgType.Message.ToString();
                (masterpage.FindControl("DialogTopic") as Label).Text = "Haven't this ID";
                (masterpage.FindControl("DialogMsg") as Label).Text = errorMsgBody;
                (masterpage.FindControl("ModalPopupExtender") as ModalPopupExtender).Show();

            }
            else
            {
                if (AlertType.Equals(msgType.Message))
                {
                    (masterpage.FindControl("DivDialog") as HtmlGenericControl).Style["display"] = "inline";
                    (masterpage.FindControl("divSolution") as HtmlGenericControl).Style["display"] = "inline";
                    (masterpage.FindControl("DialogOkButton") as Button).Text = "OK";
                    (masterpage.FindControl("divCancel") as HtmlGenericControl).Style["display"] = "none";
                    (masterpage.FindControl("divOk") as HtmlGenericControl).Style["display"] = "inline";


                    (masterpage.FindControl("DialogHeader") as Label).Text = MsgHeader;
                    (masterpage.FindControl("DialogTopic") as Label).Text = MsgTopic;
                    (masterpage.FindControl("DialogMsg") as Label).Text = MsgBody;
                    (masterpage.FindControl("ModalPopupExtender") as ModalPopupExtender).Show();
                }
                else if (AlertType.Equals(msgType.Error))
                {

                    (masterpage.FindControl("DivDialog") as HtmlGenericControl).Style["display"] = "inline";
                    (masterpage.FindControl("divSolution") as HtmlGenericControl).Style["display"] = "inline";
                    (masterpage.FindControl("DialogCancelButton") as Button).Text = "OK";
                    (masterpage.FindControl("divCancel") as HtmlGenericControl).Style["display"] = "inline";
                    (masterpage.FindControl("divOk") as HtmlGenericControl).Style["display"] = "none";


                    (masterpage.FindControl("DialogHeader") as Label).Text = AlertType.ToString();
                    (masterpage.FindControl("DialogTopic") as Label).Text = MsgTopic;
                    (masterpage.FindControl("DialogMsg") as Label).Text = MsgBody;
                    (masterpage.FindControl("DialogSolution") as Label).Text = MsgSolution;
                    (masterpage.FindControl("ModalPopupExtender") as ModalPopupExtender).Show();
                    StringBuilder mScript = new StringBuilder(string.Empty);
                    mScript.AppendFormat("DialogError('{0}','{1}','{2}','{3}')", AlertType.ToString(), MsgTopic, MsgBody, errorMsgBody);
                    ScriptManager.RegisterClientScriptBlock(HttpContext.Current.Handler as Page, (HttpContext.Current.Handler as Page).GetType(), "onload", mScript.ToString(), true);

                
                }
            }
           
        }
        private SqlCommand getMsgDefaultLanguage(string ALERT_CODE)
        {
            SqlCommand command = new SqlCommand();
            StringBuilder sql = new StringBuilder(string.Empty);
            sql.Append("SELECT B.ALERT_GRP_NAME, A.TOPIC, A.MSG, A.ALERT_CODE ,A.SOLUTION ");
            sql.Append(" FROM  SU_ALERT AS A INNER JOIN ");
            sql.Append(" SU_ALERT_GROUP AS B ON A.ALERT_GRP_ID = B.ALERT_GRP_ID ");
            sql.Append(" WHERE A.ALERT_CODE = @ALERT_CODE");

            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            command.Parameters.AddWithValue("@ALERT_CODE", ALERT_CODE);
            return command;
        }
        private SqlCommand getMsgAccountLanguage(string ALERT_CODE)
        {
            SqlCommand command = new SqlCommand();
            StringBuilder sql = new StringBuilder(string.Empty);
            sql.Append(" SELECT     B.ALERT_GRP_NAME ");
            sql.Append(", D.TOPIC");
            sql.Append(", D.MSG ");
            sql.Append(", A.ALERT_CODE");
            sql.Append(", C.ALERT_GRP_NAME");
            sql.Append(", D.SOLUTION  ");
            sql.Append(" FROM         SU_ALERT AS A INNER JOIN  SU_ALERT_GROUP_LANG AS B ON A.ALERT_GRP_ID = B.ALERT_GRP_ID ");
            sql.Append("			   LEFT OUTER JOIN SU_ALERT_GROUP_LANG AS C ON B.ALERT_GRP_ID = C.ALERT_GRP_ID ");
            sql.Append("   LEFT OUTER JOIN SU_ALERT_LANG D ON A.ALERT_ID = D.ALERT_ID");
            sql.Append(" WHERE     (A.ALERT_CODE = @ALERT_CODE) AND D.LanguageID = @LanguageID");

            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            command.Parameters.AddWithValue("@ALERT_CODE", ALERT_CODE);
            command.Parameters.AddWithValue("@LanguageID", UserAccount.CURRENT_LanguageID);
            return command;

        }
        [System.Web.Services.WebMethod]
        public static void GetShowMessage(string MsgID)
        {
            AlertMessage msg = Provider.MessageDAL.GetMessage(MsgID);
            StringBuilder JSON = new StringBuilder();
            JSON.Append(" var ObjMessage = { ");
            JSON.AppendFormat(" ,DialogHeader : '{0}'", msg.DialogHeader);
            JSON.AppendFormat(" ,DialogTopic : '{0}'", msg.DialogTopic);
            JSON.AppendFormat(" ,DialogMsg : '{0}'", msg.DialogMsg);
            JSON.AppendFormat(" ,DialogSolutions : '{0}'", msg.DialogSolutions);
            JSON.Append(" } ");
            System.Web.HttpContext.Current.Response.Write(JSON);
        }
        public static void ShowMessage(MasterPage mPage, string MsgID)
        {
            AlertMessage msg = Provider.MessageDAL.GetMessage(MsgID);
            (mPage.FindControl("ctl_MsgHeaser_Label") as Label).Text = msg.DialogHeader;
            (mPage.FindControl("ctl_MsgTopic_Label") as Label).Text = msg.DialogTopic;
            (mPage.FindControl("ctl_MsgBody_Label") as Label).Text = msg.DialogMsg;
            (mPage.FindControl("ctl_Panel_ModalPopupExtender") as ModalPopupExtender).Show();

        }
        private enum msgType
        { 
            Message,
            Error,
            Information,
            Confirmation
        }



    }
    public class AlertMessage
    { 
    
        public string DialogHeader {get;set;}
        public string DialogTopic {get;set;}
        public string DialogMsg {get;set;}
        public string DialogSolutions { get; set; }

        
    }
}
