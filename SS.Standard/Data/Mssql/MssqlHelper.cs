using System;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SS.Standard.Data.Mssql
{
    public abstract class MssqlHelper
    {
        protected SqlConnection _Connection;
        protected SqlTransaction _Transaction;
        public MssqlHelper() { _Connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["StandardMssqlConnectionString"].ConnectionString); }
        public SqlConnection Connection  { get { return _Connection; } }
        public SqlTransaction Transaction {get;set;}
        protected IDataReader ExecuteReader(DbCommand cmd)
        {
            if (Transaction != null) cmd.Transaction = Transaction;
            if (cmd.Connection == null){
                cmd.Connection = Connection;
                if (cmd.Connection.State == ConnectionState.Closed) {
                    Connection.Open();
                    IDataReader dr = cmd.ExecuteReader();
                    Connection.Close();
                    return dr;
                }
            }
            return cmd.ExecuteReader();
        }
        protected int ExecuteNonQuery(DbCommand cmd)
        {
            if (Transaction != null) cmd.Transaction = Transaction;
            if (cmd.Connection == null) {
                cmd.Connection = Connection;
                if (cmd.Connection.State == ConnectionState.Closed){
                    Connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    Connection.Close();
                    return result;
                }
            }
            return cmd.ExecuteNonQuery();
        }
        protected object ExecuteScalar(DbCommand cmd)
        {
            if (Transaction != null) cmd.Transaction = Transaction;
            if (cmd.Connection == null){
                cmd.Connection = Connection;
                if (cmd.Connection.State == ConnectionState.Closed) {
                    Connection.Open();
                    object result = cmd.ExecuteScalar();
                    Connection.Close();
                    return result;
                }

            }
            return cmd.ExecuteScalar();
        }
    }
}
