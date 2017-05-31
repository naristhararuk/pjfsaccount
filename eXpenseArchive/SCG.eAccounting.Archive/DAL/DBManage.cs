using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace SCG.eAccounting.Archive.DAL
{
    public class DBManage
    {
        private string archiveDatabaseName;
        public string ArchiveDatabaseName { get { return archiveDatabaseName; } }

        private string eXpenseDatabaseServerName;
        private string eXpenseDatabaseName;
        private string eXpenseDatabaseUserName;
        private string eXpenseDatabasePassword;
        private string executeTimeout;
        public string ExecuteTimeout { get { return executeTimeout; } }

        public DBManage()
        {
            GetDatabaseConfiguration();
        }

        public SqlConnection GetConnection()
        {
            try
            {
                string strConnectionString = @"Data Source=" + eXpenseDatabaseServerName + ";Persist Security Info=False;Initial Catalog=" + eXpenseDatabaseName + ";User ID=" + eXpenseDatabaseUserName + ";Password=" + eXpenseDatabasePassword+";Timeout=300";
                SqlConnection Con = new SqlConnection(strConnectionString);
                Con.Open();
                return Con;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void GetDatabaseConfiguration()
        {
            System.Collections.Specialized.NameValueCollection obj = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("databaseSettings");
            eXpenseDatabaseServerName = obj.GetValues("ServerName")[0].ToString();
            eXpenseDatabaseName = obj.GetValues("DatabaseName")[0].ToString();
            eXpenseDatabaseUserName = obj.GetValues("UserName")[0].ToString();
            eXpenseDatabasePassword = obj.GetValues("Password")[0].ToString();
            archiveDatabaseName = obj.GetValues("ArchiveDatabaseName")[0].ToString();
            executeTimeout = obj.GetValues("ExecuteTimeout")[0].ToString();
        }
    }
}
