using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace SS.Standard.Utilities.Mssql
{
    public class DbParameterDAL : Data.Mssql.MssqlHelper, Data.Interfaces.IDBManager, Interface.IDbParameter
    {
        private SqlCommand comm;
        private IDataReader reader;

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

        #region IDbParameter Members

        public string getDbParameter(string group_no, string seq_no)
        {
  
            string paramvalue = string.Empty;
            OpenConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "SELECT [Parameter_Value] FROM [DbParameter] WHERE [GroupNo]=@GROUP_NO AND [SeqNo]=@SEQ_NO AND [Active]=@Active";
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue("@GROUP_NO", group_no);
            comm.Parameters.AddWithValue("@SEQ_NO", seq_no);
            comm.Parameters.AddWithValue("@Active", true);
            reader = ExecuteReader(comm);
            while (reader.Read())
            {
                paramvalue = reader.GetString(0);
                break;
            }
            reader.Close();
            CloseConnection();
            return paramvalue;

       
        }

        public string getDbParameter(int group_no, int seq_no)
        {
            string paramvalue = string.Empty;
            OpenConnection();
            comm = new SqlCommand();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "SELECT [Parameter_Value] FROM [DbParameter] WHERE [GroupNo]=@GROUP_NO AND [SeqNo]=@SEQ_NO AND [Active]=@Active";
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue("@GROUP_NO", group_no);
            comm.Parameters.AddWithValue("@SEQ_NO", seq_no);
            comm.Parameters.AddWithValue("@Active", true);
            
            reader = ExecuteReader(comm);
            while (reader.Read())
            {
                paramvalue = reader.GetString(0);
                break;
            }
            reader.Close();
            CloseConnection();
            return paramvalue;

        }

       

        #endregion
    }
}
