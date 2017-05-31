using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Archive.DAL;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace SCG.eAccounting.Archive.BLL
{
    public class ArchiveService
    {
        private int countError;
        public int ProcessArchive()
        {
            countError = 0;
            DBManage dbManage = new DBManage();
            SqlConnection connection = dbManage.GetConnection();
            try
            {
                //handle event sql info message 
                connection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);

                SqlCommand command = connection.CreateCommand();

                //get configuration number of day for move document , get value from DbParameter
                command.CommandText = "SELECT ParameterValue FROM DBParameter WHERE ConfigurationName='NumberOfMonthForArchive'";
                object numberOfMonth = command.ExecuteScalar();

                if (numberOfMonth != null && !string.IsNullOrEmpty(numberOfMonth.ToString()) && int.Parse(numberOfMonth.ToString()) < 0)
                {
                    // execute store procedure for move document
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(dbManage.ExecuteTimeout))
                    {
                        command.CommandTimeout = int.Parse(dbManage.ExecuteTimeout);
                    }

                    command.CommandText = "dbo.ArchiveDocumentTransaction";
                    command.Parameters.Add(new SqlParameter("expenseArchiveDBName", dbManage.ArchiveDatabaseName));
                    command.Parameters.Add(new SqlParameter("numOfMonth", numberOfMonth.ToString()));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message);
                throw ex;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return countError;
        }

        void connection_InfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            foreach (SqlError err in args.Errors)
            {
                string errorMsg = string.Empty;
                if (err.Message.StartsWith("ERROR"))
                {
                    countError++;
                    errorMsg =string.Format(" Line:{0} of procedure {1} on server {2} => {3}", err.LineNumber, err.Procedure, err.Server, err.Message);
                }
                else
                {
                    errorMsg = err.Message;
                }

                Console.WriteLine(errorMsg);
                Logger.Write(errorMsg);
            }
        }
    }
}
