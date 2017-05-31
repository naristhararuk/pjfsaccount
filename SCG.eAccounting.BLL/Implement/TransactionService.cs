using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;

namespace SCG.eAccounting.BLL.Implement
{
    public class TransactionService : ITransactionService
    {
        public Guid Begin(DataSet ds)
        {
            Guid txID = Guid.NewGuid();
            WriteLogFile("Begin", txID);

            HttpContext.Current.Session.Add(txID.ToString(), ds);
            return txID;
        }

        public Guid Begin(Guid parentTxID)
        {
            Guid txID = Guid.NewGuid();
            WriteLogFile("Begin", txID, parentTxID);

            HttpContext.Current.Session.Add(txID.ToString() + "_Prev", parentTxID);

            // Copy dataset from parent session to new session
            HttpContext.Current.Session.Add(txID.ToString(), ((DataSet)HttpContext.Current.Session[parentTxID.ToString()]).Copy());
            return txID;
        }

        public void Commit(Guid txID)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger("TransactionService");
            WriteLogFile("Commit", txID);
            if (HttpContext.Current.Session[txID.ToString() + "_Prev"] != null)
            {
                Guid prevTxID = new Guid(HttpContext.Current.Session[txID.ToString() + "_Prev"].ToString());

                // Copy dataset from commited session into parent session,
                HttpContext.Current.Session[prevTxID.ToString()] = ((DataSet)HttpContext.Current.Session[txID.ToString()]).Copy();
                ClearTransactionStack(txID);
            }
            else
            {
                logger.Error("Error in Transaction Service [Commit] <==> Remove Sesseion: TransactionID =" + txID.ToString());
                HttpContext.Current.Session.Remove(txID.ToString());
            }
        }

        public void Rollback(Guid txID)
        {
            WriteLogFile("Rollback", txID);
            ClearTransactionStack(txID);
        }

        Dictionary<Guid, DateTime> ExpiredTransactions
        {
            get
            {
                if (HttpContext.Current.Session["ExpiredTransactions"] == null)
                {
                    HttpContext.Current.Session["ExpiredTransactions"] = new Dictionary<Guid, DateTime>();
                }
                return HttpContext.Current.Session["ExpiredTransactions"] as Dictionary<Guid, DateTime>;
            }
        }

        void ClearTransactionStack(Guid txID)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger("TransactionService");
            Dictionary<Guid, DateTime> expiredTransactions = ExpiredTransactions;
            List<Guid> removedGuids = new List<Guid>();
            foreach (Guid id in expiredTransactions.Keys)
            {
                if (expiredTransactions[id].AddHours(1) < DateTime.Now)
                {
                    logger.Error("Error in Transaction Service [ClearTransactionStack] <==> Remove Sesseion: TransactionID =" + id.ToString() + " , Parent TransactionID = " + id.ToString() + "_Prev");
                    HttpContext.Current.Session.Remove(id.ToString() + "_Prev");
                    HttpContext.Current.Session.Remove(id.ToString());
                    removedGuids.Add(id);
                }
            }

            foreach (Guid id in removedGuids)
                expiredTransactions.Remove(id);

            expiredTransactions[txID] = DateTime.Now;
        }


        public DataSet GetDS(Guid txID)
        {
            if (HttpContext.Current.Session[txID.ToString()] == null)
                WriteLogFile("GetDS", txID);
            else
            {
                Dictionary<Guid, DateTime> expiredTransactions = ExpiredTransactions;
                if (expiredTransactions.ContainsKey(txID))
                    expiredTransactions.Remove(txID);
            }
            return (DataSet)HttpContext.Current.Session[txID.ToString()];
        }

        public void WriteLogFile(string TransactionType, Guid TxID)
        {
            SS.SU.DTO.UserSession us = (SS.SU.DTO.UserSession)HttpContext.Current.Session["UserProfiles"];
            log4net.ILog logger = log4net.LogManager.GetLogger("TransactionService");

            System.Text.StringBuilder errMsg = new System.Text.StringBuilder();
            errMsg.Append("Error in Transaction Service [ " + TransactionType + "] <==> ");
            errMsg.Append("" + "Transaction ID :" + TxID.ToString());
            errMsg.Append(" , " + "UserID:" + us == null ? 0 : us.UserID);
            errMsg.Append(" , " + "UserName:" + us == null ? string.Empty : us.UserName);
            errMsg.Append(" , " + "ProgramCode:" + us == null ? string.Empty : us.CurrentProgramCode).AppendLine();
            //errMsg.Append(" , " + "ClientIP:" + Request.ServerVariables["REMOTE_ADDR"].ToString());

            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
            foreach (StackFrame f in st.GetFrames())
                errMsg.Append("\t\t==>\t" + f.ToString());

            logger.Error(errMsg.ToString());
        }

        public void WriteLogFile(string TransactionType, Guid TxID, Guid parentTxID)
        {
            SS.SU.DTO.UserSession us = (SS.SU.DTO.UserSession)HttpContext.Current.Session["UserProfiles"];
            log4net.ILog logger = log4net.LogManager.GetLogger("TransactionService");

            System.Text.StringBuilder errMsg = new System.Text.StringBuilder();
            errMsg.Append("Error in Transaction Service [ " + TransactionType + "] <==> ");
            errMsg.Append(" Parent Transaction ID :" + parentTxID.ToString());
            errMsg.Append(" Transaction ID :" + TxID.ToString());
            errMsg.Append(" , " + "UserID:" + us == null ? 0 : us.UserID);
            errMsg.Append(" , " + "UserName:" + us == null ? string.Empty : us.UserName);
            errMsg.Append(" , " + "ProgramCode:" + us == null ? string.Empty : us.CurrentProgramCode).AppendLine();
            //errMsg.Append(" , " + "ClientIP:" + Request.ServerVariables["REMOTE_ADDR"].ToString());

            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
            foreach (StackFrame f in st.GetFrames())
                errMsg.Append("\t\t==>\t" + f.ToString());

            logger.Error(errMsg.ToString());
        }
    }
}
