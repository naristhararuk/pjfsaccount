using System;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
namespace SS.Standard.Data.Oracle
{
   
    public abstract class OracleHelper
    {

        protected OracleConnection _Connection;
        protected OracleTransaction _Transaction;
        public OracleHelper() {
            _Connection = new OracleConnection(WebConfigurationManager.ConnectionStrings["StandardOracleConnectionString"].ConnectionString); 
        }
        public OracleConnection Connection { get { return _Connection; } }
        public OracleTransaction Transaction { get; set; }
        protected IDataReader ExecuteReader(DbCommand cmd)
        {
            if (Transaction != null) cmd.Transaction = Transaction;
            if (cmd.Connection == null)
            {
                cmd.Connection = Connection;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
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
            if (cmd.Connection == null)
            {
                cmd.Connection = Connection;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
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
            if (cmd.Connection == null)
            {
                cmd.Connection = Connection;
                if (cmd.Connection.State == ConnectionState.Closed)
                {
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
