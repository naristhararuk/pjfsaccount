using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using SS.Standard.Data.Interfaces;

namespace SS.Standard.Data.Mssql
{

    public class DBManager : MssqlHelper,IDBManager
    {
        #region Define Variables

        private ArrayList batch = new ArrayList();
        private Database db = null;


        #endregion Define Variables


        #region Constructor
        private string useDatabaseServer = "";
        public DBManager(string db)
        {

            this.useDatabaseServer = db.ToString();
        }

        public DBManager()
        {
            try
            {
                this.useDatabaseServer = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[0].Name;
                if (this.useDatabaseServer.Equals("LocalSqlServer") && System.Web.Configuration.WebConfigurationManager.ConnectionStrings.Count > 1)
                {
                    this.useDatabaseServer = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[1].Name;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion Constructor

        #region public  void AddBatch(string sqlStatement)
        /// <summary>
        /// Add sqlstatement to Batch ArrayList
        /// </summary>
        /// <param name="sqlStatement">Input string statement</param>
        public void AddBatch(string sqlStatement)
        {
            batch.Add(sqlStatement);
        }
        #endregion

        #region public  void  ClearBatch()
        /// <summary>
        /// Clear batch in Batch ArrayList
        /// </summary>
        public void ClearBatch()
        {
            batch.Clear();
        }
        #endregion

        #region public  void ExecuteBatch()
        /// <summary>
        /// ExecuteBatch in ArrayList
        /// </summary>
        public void ExecuteBatch()
        {
            //implement with thread
            
            if (batch.Count > 0)
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                DbTransaction trans=null;
                DbConnection conn=null;
                IDataReader dr = null; 
                try
                {
                    db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                    DbCommand command = null;
                    conn = db.CreateConnection();
                    conn.Open();
                    trans = conn.BeginTransaction();
                    IEnumerator ienum = batch.GetEnumerator();
                    int count = 0;
                    while (ienum.MoveNext())
                    {
                        string sqlTmp = (String)ienum.Current;
                        if (sqlTmp.IndexOf("SELECT @@IDENTITY") != -1)
                        {
                            command = db.GetSqlStringCommand("SELECT @@IDENTITY");
                            command.Connection = conn;
                            dr = db.ExecuteReader(command, trans);
                            if (dr.Read())
                            {
                                sqlTmp = sqlTmp.Replace("SELECT @@IDENTITY", dr.GetDecimal(0).ToString());
                            }
                            else
                            {
                                dr.Close();
                                trans.Rollback(); 
                                conn.Close();
                                throw new Exception("Can't find primary key of 'SELECT @@IDENTITY'");
                            }
                            dr.Close();
                        }
                        command = db.GetSqlStringCommand(sqlTmp);
                        command.Connection = conn;
                        db.ExecuteNonQuery(command, trans);
                        count++;
                    }
                    ClearBatch();
                    trans.Commit();
                    conn.Close();
                    //ts.Complete();
                    //conn.Close();
                }
                catch (System.Exception ex)
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                    ClearBatch();
                    trans.Rollback();  // kong ---- rollback transtion
                    conn.Close();
                    throw ex;
                    //Transaction.Current.Rollback(ex);
                }
                //}
            }

        }
        #endregion

        #region  ==== ExcecuteQuery ====


        public DataSet ExecuteQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType)
        {
            try
            {
                if (null == sqlStatementOrStoreProcedureName || string.Empty.Equals(sqlStatementOrStoreProcedureName))
                    throw new Exception("Error : sqlStatement Or StoreProcedureName is Empty");

                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());

                return db.ExecuteDataSet(commandSelect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet ExecuteQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType, int CommandTimeout)
        {
            try
            {
                if (null == sqlStatementOrStoreProcedureName || string.Empty.Equals(sqlStatementOrStoreProcedureName))
                    throw new Exception("Error : sqlStatement Or StoreProcedureName is Empty");


                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());
                if (CommandTimeout > 0)
                    commandSelect.CommandTimeout = CommandTimeout;

                return db.ExecuteDataSet(commandSelect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Load DataTable เข้าไปยัง DataSet ที่ส่งเข้ามา
        /// </summary>
        /// <param name="sqlStatementOrStoreProcedureName"></param>
        /// <param name="cmdType"></param>
        /// <param name="Dst"></param>
        /// <param name="TableName"></param>
        public void ExecuteQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType, DataSet Dst, string TableName)
        {
            try
            {
                if (null == sqlStatementOrStoreProcedureName || string.Empty.Equals(sqlStatementOrStoreProcedureName))
                    throw new Exception("Error : sqlStatement Or StoreProcedureName is Empty");


                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());


                db.LoadDataSet(commandSelect, Dst, TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Load DataTable เข้าไปยัง DataSet ที่ส่งเข้ามา
        /// </summary>
        /// <param name="SqlStatement"></param>
        /// <param name="Dst"></param>
        /// <param name="TableName"></param>
        public void ExecuteQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType, DataSet Dst, string TableName, int CommandTimeout)
        {
            try
            {
                if (null == sqlStatementOrStoreProcedureName || string.Empty.Equals(sqlStatementOrStoreProcedureName))
                    throw new Exception("Error : sqlStatement Or StoreProcedureName is Empty");


                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());


                if (CommandTimeout > 0)
                    commandSelect.CommandTimeout = CommandTimeout;

                db.LoadDataSet(commandSelect, Dst, TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// public DataSet ExecuteQuery(string StoreProcedureName, string[] sqlParameter)
        /// คืนค่าเป็น Object DataSet
        /// </summary>
        /// <param name="StoreProcedureName">ชื่อ Storeprocedure</param>
        /// <param name="ObjParameter"></param>
        /// <returns></returns>
        public DataSet ExecuteQuery(string StoreProcedureName, object[] ObjParameter)
        {
            try
            {
                if (null == StoreProcedureName || string.Empty.Equals(StoreProcedureName))
                    throw new Exception("Error : StoreProcedureName is Empty");

                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);
                return db.ExecuteDataSet(commandSelect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// public DataSet ExecuteQuery(string StoreProcedureName, string[] sqlParameter, int CommandTimeout)
        /// คืนค่าเป็น Object DataSet
        /// </summary>
        /// <param name="StoreProcedureName">ชื่อ Storeprocedure</param>
        /// <param name="ObjParameter"></param>
        /// <param name="CommandTimeout">ระบุเวลาให้กับ CommandTimeout</param>
        /// <returns></returns>
        public DataSet ExecuteQuery(string StoreProcedureName, object[] ObjParameter, int CommandTimeout)
        {
            try
            {
                if (null == StoreProcedureName || string.Empty.Equals(StoreProcedureName))
                    throw new Exception("Error : StoreProcedureName is Empty");


                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);

                if (CommandTimeout > 0)
                    commandSelect.CommandTimeout = CommandTimeout;


                return db.ExecuteDataSet(commandSelect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Load DataTable เข้าไปไว้ใน DataSet ที่ส่งเข้ามา 
        /// จะไม่ Return ค่าใด ๆ 
        /// </summary>
        /// <param name="StoreProcedureName">ชื่อ Storeprocedure</param>
        /// <param name="ObjParameter">string array ที่มี ชื่อ parameter และ values</param>
        /// <param name="Dst">DataSet ที่ต้องการ Load DataTable </param>
        /// <param name="TableName">ชื่อ Table </param>
        public void ExecuteQuery(string StoreProcedureName, object[] ObjParameter, DataSet Dst, string TableName)
        {
            try
            {
                if (null == StoreProcedureName || string.Empty.Equals(StoreProcedureName))
                    throw new Exception("Error : StoreProcedureName is Empty");


                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);

                db.LoadDataSet(commandSelect, Dst, TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load DataTable เข้าไปใน DataSet ที่ส่งเข้ามา
        /// </summary>
        /// <param name="StoreProcedureName"></param>
        /// <param name="ObjParameter"></param>
        /// <param name="Dst"></param>
        /// <param name="TableName"></param>
        /// <param name="CommandTimeout"></param>
        public void ExecuteQuery(string StoreProcedureName, object[] ObjParameter, DataSet Dst, string TableName, int CommandTimeout)
        {
            try
            {
                if (null == StoreProcedureName || string.Empty.Equals(StoreProcedureName))
                    throw new Exception("Error : StoreProcedureName is Empty");


                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);
                if (CommandTimeout > 0)
                    commandSelect.CommandTimeout = CommandTimeout;

                db.LoadDataSet(commandSelect, Dst, TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion  ==== ExcecuteQuery ===

        #region ==== ExecuteNonQuery ====

        public int ExecuteNonQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType)
        {

            int intReturn = -1;
            try
            {

                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand command = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    command = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    command = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());

                intReturn = db.ExecuteNonQuery(command);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;
        }

        public int ExecuteNonQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType, DbTransaction trans)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand command = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    command = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    command = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());

                intReturn = db.ExecuteNonQuery(command, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;
        }

        public int ExecuteNonQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType, int CommandTimeout)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand command = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    command = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    command = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());

                if (CommandTimeout > 0)
                    command.CommandTimeout = CommandTimeout;

                intReturn = db.ExecuteNonQuery(command);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;
        }

        public int ExecuteNonQuery(string sqlStatementOrStoreProcedureName, CommandType cmdType, int CommandTimeout, DbTransaction trans)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand command = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    command = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    command = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());

                if (CommandTimeout > 0)
                    command.CommandTimeout = CommandTimeout;

                intReturn = db.ExecuteNonQuery(command, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;
        }

        public int ExecuteNonQuery(string StoreProcedureName, object[] ObjParameter)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);
                // DbParameter para = commandSelect.Parameters[0];

                intReturn = db.ExecuteNonQuery(commandSelect);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        public int ExecuteNonQuery(string StoreProcedureName, object[] ObjParameter, DbTransaction trans)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);
                intReturn = db.ExecuteNonQuery(commandSelect, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        public int ExecuteNonQuery(string StoreProcedureName, object[] ObjParameter, int CommandTimeout)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);

                if (CommandTimeout > 0)
                    commandSelect.CommandTimeout = CommandTimeout;

                intReturn = db.ExecuteNonQuery(commandSelect);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;
        }

        public int ExecuteNonQuery(string StoreProcedureName, object[] ObjParameter, int CommandTimeout, DbTransaction trans)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName.Trim(), ObjParameter);

                if (CommandTimeout > 0)
                    commandSelect.CommandTimeout = CommandTimeout;

                intReturn = db.ExecuteNonQuery(commandSelect, trans);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        public int ExecuteNonQuery(string InsertStoreProcedureName, string DeleteStoreProcedureName, string UpdateStoreProcedureName, DataSet dst)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                //DatabaseInstance.
                string tableName = dst.Tables[0].TableName;
                DataTable dtb = getTableStructure(tableName);
                #region Delete Mapping
                DbCommand deleteCommand = db.GetStoredProcCommand(DeleteStoreProcedureName);

                DataColumn[] keyColumn = dtb.PrimaryKey;
                if (0 < keyColumn.Length)
                {
                    for (int i = 0; i < keyColumn.Length; i++)
                        db.AddInParameter(deleteCommand, ((DataColumn)keyColumn.GetValue(i)).ColumnName, ConvertDataTypeToDbType(((DataColumn)keyColumn.GetValue(i)).DataType.Name), keyColumn.GetValue(i).ToString(), DataRowVersion.Current);
                }
                #endregion Delete Mapping

                #region Insert Mapping
                DbCommand insertCommand = db.GetStoredProcCommand(InsertStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(insertCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Insert Mapping

                #region Update Mapping
                DbCommand updateCommand = db.GetStoredProcCommand(UpdateStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(updateCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Update Mapping

                #region Process Command Insert , Update , Delete
                intReturn = db.UpdateDataSet(dst, tableName, insertCommand, updateCommand,
                  deleteCommand, Microsoft.Practices.EnterpriseLibrary.Data.UpdateBehavior.Standard);

                #endregion Process Command Insert , Update , Delete
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        public int ExecuteNonQuery(string InsertStoreProcedureName, string DeleteStoreProcedureName, string UpdateStoreProcedureName, DataSet dst, int updateBatchSize)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                //DatabaseInstance.
                string tableName = dst.Tables[0].TableName;
                DataTable dtb = getTableStructure(tableName);
                #region Delete Mapping
                DbCommand deleteCommand = db.GetStoredProcCommand(DeleteStoreProcedureName);

                DataColumn[] keyColumn = dtb.PrimaryKey;
                if (0 < keyColumn.Length)
                {
                    for (int i = 0; i < keyColumn.Length; i++)
                        db.AddInParameter(deleteCommand, ((DataColumn)keyColumn.GetValue(i)).ColumnName, ConvertDataTypeToDbType(((DataColumn)keyColumn.GetValue(i)).DataType.Name), keyColumn.GetValue(i).ToString(), DataRowVersion.Current);
                }
                #endregion Delete Mapping

                #region Insert Mapping
                DbCommand insertCommand = db.GetStoredProcCommand(InsertStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(insertCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Insert Mapping

                #region Update Mapping
                DbCommand updateCommand = db.GetStoredProcCommand(UpdateStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(updateCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Update Mapping

                #region Process Command Insert , Update , Delete
                intReturn = db.UpdateDataSet(dst, tableName, insertCommand, updateCommand,
                  deleteCommand, Microsoft.Practices.EnterpriseLibrary.Data.UpdateBehavior.Standard, updateBatchSize);

                #endregion Process Command Insert , Update , Delete
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        public int ExecuteNonQuery(string InsertStoreProcedureName, string DeleteStoreProcedureName, string UpdateStoreProcedureName, DataSet dst, DbTransaction trans)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                //DatabaseInstance.
                string tableName = dst.Tables[0].TableName;
                DataTable dtb = getTableStructure(tableName);
                #region Delete Mapping
                DbCommand deleteCommand = db.GetStoredProcCommand(DeleteStoreProcedureName);

                DataColumn[] keyColumn = dtb.PrimaryKey;
                if (0 < keyColumn.Length)
                {
                    for (int i = 0; i < keyColumn.Length; i++)
                        db.AddInParameter(deleteCommand, ((DataColumn)keyColumn.GetValue(i)).ColumnName, ConvertDataTypeToDbType(((DataColumn)keyColumn.GetValue(i)).DataType.Name), keyColumn.GetValue(i).ToString(), DataRowVersion.Current);
                }
                #endregion Delete Mapping

                #region Insert Mapping
                DbCommand insertCommand = db.GetStoredProcCommand(InsertStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(insertCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Insert Mapping

                #region Update Mapping
                DbCommand updateCommand = db.GetStoredProcCommand(UpdateStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(updateCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Update Mapping

                #region Process Command Insert , Update , Delete
                intReturn = db.UpdateDataSet(dst, tableName, insertCommand, updateCommand, deleteCommand, trans);

                #endregion Process Command Insert , Update , Delete
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        public int ExecuteNonQuery(string InsertStoreProcedureName, string DeleteStoreProcedureName, string UpdateStoreProcedureName, DataSet dst, DbTransaction trans, int updateBatchSize)
        {
            int intReturn = -1;
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                //DatabaseInstance.
                string tableName = dst.Tables[0].TableName;
                DataTable dtb = getTableStructure(tableName);
                #region Delete Mapping
                DbCommand deleteCommand = db.GetStoredProcCommand(DeleteStoreProcedureName);

                DataColumn[] keyColumn = dtb.PrimaryKey;
                if (0 < keyColumn.Length)
                {
                    for (int i = 0; i < keyColumn.Length; i++)
                        db.AddInParameter(deleteCommand, ((DataColumn)keyColumn.GetValue(i)).ColumnName, ConvertDataTypeToDbType(((DataColumn)keyColumn.GetValue(i)).DataType.Name), keyColumn.GetValue(i).ToString(), DataRowVersion.Current);
                }
                #endregion Delete Mapping

                #region Insert Mapping
                DbCommand insertCommand = db.GetStoredProcCommand(InsertStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(insertCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Insert Mapping

                #region Update Mapping
                DbCommand updateCommand = db.GetStoredProcCommand(UpdateStoreProcedureName);
                foreach (DataColumn dcl in dst.Tables[0].Columns)
                    db.AddInParameter(updateCommand, dcl.ColumnName, ConvertDataTypeToDbType(dcl.DataType.Name), dcl.ColumnName, DataRowVersion.Current);

                #endregion Update Mapping

                #region Process Command Insert , Update , Delete
                intReturn = db.UpdateDataSet(dst, tableName, insertCommand, updateCommand, deleteCommand, trans, updateBatchSize);

                #endregion Process Command Insert , Update , Delete
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return intReturn;

        }

        #endregion  ==== ExcecuteQuery ====

        #region === ExecuteReader ===
        public IDataReader ExecuteReader(string sqlStatementOrStoreProcedureName, CommandType cmdType)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());

                return db.ExecuteReader(commandSelect);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
      
        public IDataReader ExecuteReader(string sqlStatementOrStoreProcedureName, CommandType cmdType, DbTransaction trans)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName.Trim());
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName.Trim());


                return db.ExecuteReader(commandSelect, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public IDataReader ExecuteReader(string StoreProcedureName, object[] ObjParameter)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName, ObjParameter);

                return db.ExecuteReader(commandSelect);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public IDataReader ExecuteReader(string StoreProcedureName, object[] ObjParameter, DbTransaction trans)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName, ObjParameter);

                return db.ExecuteReader(commandSelect, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion === ExecuteReader ===

        #region === ExecuteScalar ===
        public Object ExecuteScalar(string sqlStatementOrStoreProcedureName, CommandType cmdType)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName);
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName);


                return db.ExecuteScalar(commandSelect);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public Object ExecuteScalar(string sqlStatementOrStoreProcedureName, CommandType cmdType, DbTransaction trans)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                if (CommandType.StoredProcedure.Equals(cmdType))
                    commandSelect = db.GetStoredProcCommand(sqlStatementOrStoreProcedureName);
                else
                    commandSelect = db.GetSqlStringCommand(sqlStatementOrStoreProcedureName);


                return db.ExecuteScalar(commandSelect, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public Object ExecuteScalar(string StoreProcedureName, object[] ObjParameter)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName, ObjParameter);

                return db.ExecuteScalar(commandSelect);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public Object ExecuteScalar(string StoreProcedureName, object[] ObjParameter, DbTransaction trans)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbCommand commandSelect = null;
                commandSelect = db.GetStoredProcCommand(StoreProcedureName, ObjParameter);

                return db.ExecuteScalar(commandSelect, trans);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion === ExecuteScalar ===

        private DbType ConvertDataTypeToDbType(string type)
        {
            try
            {
                switch (type.ToLower())
                {
                    case "string":
                        return DbType.String;
                    case "system.string":
                        return DbType.String;
                    case "timestamp":
                        return DbType.DateTime;
                    case "system.datetime":
                        return DbType.DateTime;
                    case "int":
                        return DbType.Int64;
                    case "system.decimal":
                        return DbType.Decimal;
                    case "double":
                        return DbType.Double;
                    case "system.double":
                        return DbType.Double;
                    case "varchar":
                        return DbType.String;
                    case "numeric":
                        return DbType.Int64;
                    case "datetime":
                        return DbType.DateTime;
                    default:
                        return DbType.String;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getTableStructure(string tableName)
        {
            try
            {
                DataSet dst = new DataSet(tableName);
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                DbDataAdapter adapter = db.GetDataAdapter();
                DbCommand selectCommand = db.GetSqlStringCommand("Select * From " + tableName + " WHERE 1=2");
                adapter.SelectCommand = selectCommand;
                adapter.FillSchema(dst, SchemaType.Mapped, tableName);
                adapter.Fill(dst, tableName);
                return dst.Tables[tableName];
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public DbDataAdapter GetDataAdapter()
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                return db.GetDataAdapter();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public DbConnection CreateConnection()
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(this.useDatabaseServer);
                return db.CreateConnection();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


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
    }
}
