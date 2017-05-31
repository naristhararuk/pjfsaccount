using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SS.Standard.Security;

namespace SS.Standard.AlertMsg.DAL.Mssql
{
    public class MessageDAL : Data.Mssql.MssqlHelper, Data.Interfaces.IDBManager, DAL.Interface.IMessageDAL
    {
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
            if (Connection.State == ConnectionState.Open)
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

        #region IMessageDAL Members

        public string GetSqlExceptionNumber(Exception ex)
        {
            if (ex as System.Data.SqlClient.SqlException != null)
            {
                return (ex as System.Data.SqlClient.SqlException).Number.ToString();
            }
            else return ex.GetType().FullName;
        }

        #endregion

        #region IMessageDAL Members

        private SqlCommand getMsgDefaultLanguage(string ALERT_CODE)
        {
            SqlCommand command = new SqlCommand();
            StringBuilder sql = new StringBuilder(string.Empty);
            sql.Append("SELECT B.ALERT_GRP_NAME ");
            sql.Append(",A.TOPIC ");
            sql.Append(",A.MSG ");
            sql.Append(",A.ALERT_CODE ");
            sql.Append(",A.SOLUTION ");
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
        public AlertMessage GetMessage(Exception ex)
        {
            AlertMessage msg = new AlertMessage();
            string DialogHeader = string.Empty, DialogTopic = string.Empty, DialogMsg = string.Empty, DialogSolutions = string.Empty;
            StringBuilder sql = new StringBuilder(string.Empty);
            SqlCommand _SqlCommand = null;
            string alert_code = GetSqlExceptionNumber(ex);
            if (UserAccount.CURRENT_LanguageID == null || UserAccount.CURRENT_LanguageID == UserAccount.LanguageID)
            {
                _SqlCommand = getMsgDefaultLanguage(alert_code);
            }
            else
            {
                _SqlCommand = getMsgAccountLanguage(alert_code);
            }
            OpenConnection();
            IDataReader dr = ExecuteReader(_SqlCommand);
            while (dr.Read())
            {
                msg.DialogHeader = dr["ALERT_GRP_NAME"].ToString();
                msg.DialogTopic  = dr["TOPIC"].ToString();
                msg.DialogMsg = dr["MSG"].ToString();
                msg.DialogSolutions = dr["SOLUTION"].ToString();
            }
            dr.Close();
            CloseConnection();
            return msg;
        }

        #endregion

        #region IMessageDAL Members


        public AlertMessage GetMessage(string MsgID)
        {
            AlertMessage msg = new AlertMessage();
            string DialogHeader = string.Empty, DialogTopic = string.Empty, DialogMsg = string.Empty, DialogSolutions = string.Empty;
            StringBuilder sql = new StringBuilder(string.Empty);
            SqlCommand _SqlCommand = new SqlCommand();
            if (UserAccount.CURRENT_LanguageID == null || UserAccount.CURRENT_LanguageID == UserAccount.LanguageID)
            {
                _SqlCommand = getMsgDefaultLanguage(MsgID);
            }
            else
            {
                _SqlCommand = getMsgAccountLanguage(MsgID);
            }

            OpenConnection();
            IDataReader dr = ExecuteReader(_SqlCommand);
            while (dr.Read())
            {
                msg.DialogHeader = dr["ALERT_GRP_NAME"].ToString();
                msg.DialogTopic = dr["TOPIC"].ToString();
                msg.DialogMsg = dr["MSG"].ToString();
                msg.DialogSolutions = dr["SOLUTION"].ToString();
            }
            dr.Close();
            CloseConnection();
            return msg;
        }

        #endregion

        public string GetShowMessage(string MsgID)
        {
            AlertMessage msg = Provider.MessageDAL.GetMessage(MsgID);
            StringBuilder JSON = new StringBuilder();
            JSON.Append(" var ObjMessage = { ");
            JSON.AppendFormat(" DialogHeader : '{0}'", msg.DialogHeader);
            JSON.AppendFormat(" ,DialogTopic : '{0}'", msg.DialogTopic);
            JSON.AppendFormat(" ,DialogMsg : '{0}'", msg.DialogMsg);
            JSON.AppendFormat(" ,DialogSolutions : '{0}'", msg.DialogSolutions);
            JSON.Append(" } ");
            return JSON.ToString();
        }
    }
}
