using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Xml;
using System.Configuration;
using SCG.eAccounting.SAP.Query;

namespace SCG.eAccounting.SAP
{
    public class DBManage
    {
        private string _strConnectionString;

        #region public DBManage()
        public DBManage() 
        {
            //_strConnectionString = BapiQueryProvider.Bapiache09Query.GetConnectionString();
            //_strConnectionString = @"Data Source=HQSQLSVR\SCG;Persist Security Info=False;Initial Catalog=eAccounting;User ID=account;Password=account";
            //_strConnectionString = @"Data Source=KOOKKLA\SQL2005;Persist Security Info=False;Initial Catalog=eAccounting;User ID=sa;Password=p@ssw0rd";
            Console.Write(_strConnectionString);
        }
        #endregion public DBManage()

        #region public SqlConnection getConnection()
        public SqlConnection getConnection()
        {
            try
            {
                //SqlConnection Con = BapiQueryProvider.Bapiache09Query.GetConnectionString();
                //SqlConnection Con = new SqlConnection(_strConnectionString);
                SqlConnection Con = new SqlConnection(GetConnectionString());
                Con.Open();
                return Con;
            }
            catch(Exception e)
            {
                throw e;
                return null;
            }
        }
        #endregion public SqlConnection getConnection()

        #region public DataSet ExecuteQuery(string strSqlSelect)
        public DataSet ExecuteQuery(string strSqlSelect)
        {
            DataSet dtsReturn = new DataSet();
            dtsReturn.CaseSensitive = false;

            try
            {
                SqlConnection cnn = getConnection();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(strSqlSelect,cnn);
                dataAdapter.Fill(dtsReturn);
                
                if( cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                    cnn = null;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return dtsReturn;
        }
        #endregion public DataSet ExecuteQuery(string strSqlSelect)

        #region public DataSet ExecuteQuery(string sqlStoreProcedure,Hashtable hashParameter)
        public DataSet ExecuteQuery(string sqlStoreProcedure,Hashtable hashParameter)
        {
            DataSet dtsReturn = new DataSet();
            dtsReturn.CaseSensitive = false;

            try
            {
                SqlConnection cnn = getConnection();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand();
                dataAdapter.SelectCommand.Connection = cnn;
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.CommandText = sqlStoreProcedure;
                
                if(hashParameter != null)
                {
                    foreach( DictionaryEntry dictParameter in hashParameter)
                    {
                        dataAdapter.SelectCommand.Parameters.Add( dictParameter.Key.ToString() , dictParameter.Value.ToString() );
                    }
                }
                dataAdapter.Fill(dtsReturn);

                if( cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                    cnn = null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return dtsReturn;
        }
        #endregion public DataSet ExecuteQuery(string sqlStoreProcedure,Hashtable hashParameter)

        #region private string GetConnectionString()
        private string GetConnectionString()
        {
            System.Collections.Specialized.NameValueCollection obj = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("databaseSettings");
            string strServer = obj.GetValues("ServerName")[0].ToString();
            string strDatabaseName = obj.GetValues("DatabaseName")[0].ToString();
            string strUserName = obj.GetValues("UserName")[0].ToString();
            string strPassword = obj.GetValues("Password")[0].ToString();

            string strConnectionString = @"Data Source=" + strServer + ";Persist Security Info=False;Initial Catalog=" + strDatabaseName + ";User ID=" + strUserName + ";Password=" + strPassword;
            return strConnectionString;
        }
        #endregion private string GetConnectionString()

    }
}
