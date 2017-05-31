using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using SS.Standard.Data;
using SS.Standard.Security;
using System.Globalization;
using SS.Standard.Base.Oracle;
namespace SS.Standard.Base.Oracle
{
    public class daoOracle<T> : baseOracleDAL
    {

        public string Table { get; set; }
        public string[] Coloumn { get; set; }
        public string Prefix { get; set; }
        public bool AutoIncrement { get; set; }
        public int numPK { get; set; }
        private StringBuilder sql;
        private PropertyInfo[] properties;
        private string TimeStampColumn;
        private DateTime ExcutetionDateTime { get; set; }


        public daoOracle(string Table, string[] Coloumn, int numPK, bool AutoIncrement)
        {
            this.Table = Table;
            this.Coloumn = Coloumn;
            this.numPK = numPK;
            this.AutoIncrement = AutoIncrement;
            BuildProperty();
            TimeStampColumn = "ROWVERSION";
        }

        public string DateTimeConvert(DateTime dt)
        {
            if (dt.CompareTo(DateTime.Now) == -1)
                dt = ExcutetionDateTime;

            DateTimeFormatInfo df = DateTimeFormatInfo.InvariantInfo;
            return dt.ToString("g", df);
        }

        public void UpdateDetail(absOracleDTO Obj)
        {
            Obj.UPD_BY = UserAccount.UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = this.PgmName;
        }

        public void CreateDetail(absOracleDTO Obj)
        {
            Obj.UPD_BY = UserAccount.UserID;
            Obj.UPD_DATE = DateTime.Now; ;
            Obj.UPD_PGM = this.PgmName;
            Obj.CRE_BY = UserAccount.UserID;
            Obj.CRE_DATE = DateTime.Now;
        }


        #region BuildProperty

        public void BuildProperty()
        {
            properties = new PropertyInfo[Coloumn.Length];
            for (int i = 0; i < Coloumn.Length; i++)
            {
                properties[i] = typeof(T).GetProperty(Coloumn[i]);
            }
        }

        #endregion

        #region DTO

        public T BuildDTO(IDataReader reader)
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            for (int i = 0; i < reader.FieldCount; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    if (properties[j] != null && properties[j].Name == reader.GetName(i))
                    {
                        properties[j].SetValue(result, (reader[i] == null || reader[i] == DBNull.Value) ? "NULL" : reader[i], new object[] { });
                        break;
                    }

                }
            }
            return result;
        }

        public List<T> BuildListDTO(IDataReader reader)
        {
            List<T> Obj = new List<T>();
            while (reader.Read())
                Obj.Add(BuildDTO(reader));

            reader.Close();
            return Obj;
        }

        #endregion


        public string Select()
        {
            sql = new StringBuilder();
            sql.Append("SELECT ");
            foreach (string name in Coloumn)
            {
                sql.AppendFormat("{0} , ", name);
            }
            sql = sql.Remove(sql.Length - 2, 2);
            sql.AppendFormat(" FROM {0}", Table);
            return sql.ToString();
        }

        public string Insert(T Obj)
        {
            try
            {
                ExcutetionDateTime = DateTime.Now;
                sql = new StringBuilder();
                string value;
                int num1;
                int num2 = 1;
                StringBuilder sqlColumns = new StringBuilder();
                for (int i = 0; i < Coloumn.Length; i++)
                {
                    for (int j = 0; j < properties.Length; j++)
                    {
                        if (Coloumn[i] == properties[j].Name)
                        {
                            if (j == 0)
                                sqlColumns.AppendFormat("[{0}]", properties[j].Name);
                            else
                                sqlColumns.AppendFormat(",[{0}]", properties[j].Name);
                        }
                    }
                }
                sql.AppendFormat("INSERT INTO [{0}] ({1}) VALUES ( ", Table, sqlColumns.ToString());
                if (AutoIncrement == true) num1 = numPK;
                else
                {
                    num1 = 0;
                    num2 = 0;
                }
                for (int i = num1; i < Coloumn.Length; i++)
                {
                    for (int j = num2; j < properties.Length; j++)
                    {


                        if (Coloumn[i] == properties[j].Name)
                        {
                            if (properties[j].GetValue(Obj, null) == null)
                            {
                                value = "null";
                            }
                            else
                            {
                                if (properties[j].PropertyType.FullName == "System.DateTime")
                                {
                                    if (Coloumn[i].ToUpper().Equals("UPD_DATE") || Coloumn[i].ToUpper().Equals("CRE_DATE"))
                                    {
                                        sql.AppendFormat("{0} , ", "GETDATE()");
                                        continue;
                                    }
                                    else
                                        value = DateTimeConvert((DateTime)properties[j].GetValue(Obj, null));
                                }
                                else value = properties[j].GetValue(Obj, null).ToString();
                            }
                            if (Coloumn[i].ToUpper().Equals(TimeStampColumn))
                                sql.AppendFormat("{0} , ", value);
                            else if (Coloumn[i].ToUpper().Equals("UPD_PGM"))
                                sql.AppendFormat(" '{0}',", (this.PgmName == null || this.PgmName == "") ? "no program name" : this.PgmName);
                            else if (Coloumn[i].ToUpper().Equals("UPD_BY") || Coloumn[i].ToUpper().Equals("CRE_BY"))
                                sql.AppendFormat("'{0}' ,", UserAccount.UserID == null ? -1 : UserAccount.UserID);
                            else
                                sql.AppendFormat("{0} , ", value == "null" ? "null" : "'" + value + "'");

                        }



                    }
                }
                sql = sql.Remove(sql.Length - 2, 2);
                sql.Append(" )");
            }
            catch (Exception)
            {
                throw;
            }
            return sql.ToString();
        }

        public string Update(T Obj)
        {
            try
            {
                ExcutetionDateTime = DateTime.Now;
                sql = new StringBuilder();
                string value = string.Empty;
                string TimeStamp = "";
                sql.AppendFormat("UPDATE {0} SET ", Table);
                for (int i = numPK; i < Coloumn.Length; i++)
                {
                    for (int j = 1; j < properties.Length; j++)
                    {
                        if (Coloumn[i] == properties[j].Name)
                        {
                            if (Coloumn[i] != TimeStampColumn)
                            {
                                if (properties[j].GetValue(Obj, null) == null)
                                {
                                    value = "null";

                                }
                                else
                                {

                                    if (properties[j].PropertyType.FullName == "System.DateTime")
                                    {
                                        if (Coloumn[i].ToUpper().Equals("UPD_DATE"))
                                            value = "GETDATE()";
                                        else
                                            value = string.Empty;
                                    }
                                    else
                                    {
                                        value = properties[j].GetValue(Obj, null).ToString();
                                    }
                                }

                                if (Coloumn[i].ToUpper().Equals("UPD_DATE") && value != string.Empty)
                                    sql.AppendFormat(" {0} = {1} ,", Coloumn[i], value);
                                else if (!Coloumn[i].ToUpper().Equals("CRE_BY") && value != string.Empty)
                                {
                                    if (Coloumn[i].ToUpper().Equals("UPD_PGM"))
                                        sql.AppendFormat(" {0} = '{1}' ,", Coloumn[i], this.PgmName);
                                    else if (Coloumn[i].ToUpper().Equals("UPD_BY"))
                                        sql.AppendFormat(" {0} = '{1}' ,", Coloumn[i], UserAccount.UserID);
                                    else
                                        sql.AppendFormat(" {0} = '{1}' ,", Coloumn[i], value);

                                }

                            }
                            else
                            {
                                TimeStamp = HexConvert(properties[j].GetValue(Obj, null) as byte[]);
                            }
                        }
                    }
                }
                sql = sql.Remove(sql.Length - 2, 2);
                sql.AppendFormat(" WHERE ");
                for (int i = 0; i < numPK; i++)
                {
                    sql.AppendFormat("{0}='{1}' AND ", Coloumn[i], properties[i].GetValue(Obj, null));

                }
                sql.AppendFormat(" {0}={1}", TimeStampColumn, TimeStamp);
            }
            catch (Exception)
            {
                throw;
            }

            return sql.ToString();
        }

        public string Delete(T Obj)
        {
            sql = new StringBuilder();
            string TimeStamp = "";

            for (int i = 0; i < properties.Length; i++)
            {
                if (TimeStampColumn == properties[i].Name)
                    TimeStamp = HexConvert(properties[i].GetValue(Obj, null) as byte[]);
            }

            sql.AppendFormat("DELETE FROM {0} WHERE {1}='{2}' AND {3}={4}", Table, Coloumn[0], properties[0].GetValue(Obj, null), TimeStampColumn, TimeStamp);
            return sql.ToString();
        }

        public List<T> Query(DbCommand SelectCommand)
        {
            OpenConnection();
            IDataReader dr = ExecuteReader(SelectCommand);
            List<T> ListObj = BuildListDTO(dr);
            CloseConnection();
            return ListObj;
        }

        #region Delete
        public int Delete(DbCommand DeleteCommand)
        {
            int affectedRow = 0;
            try
            {
                OpenConnection();
                //BeginMainTransaction();
                affectedRow = ExecuteNonQuery(DeleteCommand);
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
                CloseConnection();
                throw;
            }
            return affectedRow;
        }
        public int Delete(List<DbCommand> DeleteCommand)
        {
            int affectedRow = 0;
            OpenConnection();
            try
            {
                BeginTransaction();
                foreach (DbCommand comm in DeleteCommand)
                {
                    int affected = ExecuteNonQuery(comm);
                    affectedRow++;
                }
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
                CloseConnection();
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return affectedRow;
        }
        public int Delete(DbCommand DeleteCommand, DbConnection conn, DbTransaction trans)
        {
            int affectedRow = 0;
            try
            {
                OpenConnection();
                // BeginMainTransaction();
                affectedRow = ExecuteNonQuery(DeleteCommand);
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
                CloseConnection();
                throw;
            }
            return affectedRow;
        }
        public int Delete(List<DbCommand> DeleteCommand, DbConnection conn, DbTransaction trans)
        {
            int affectedRow = 0;
            OpenConnection();
            try
            {
                BeginTransaction();


                foreach (DbCommand comm in DeleteCommand)
                {
                    int affected = ExecuteNonQuery(comm);
                    affectedRow++;
                }
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
            }
            finally
            {
                CloseConnection();
            }
            return affectedRow;
        }
        #endregion
        #region Update
        public int Update(DbCommand UpdateCommand)
        {
            int affectedRow = 0;
            try
            {
                OpenConnection();
                //BeginMainTransaction();
                affectedRow = ExecuteNonQuery(UpdateCommand);
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
                CloseConnection();
                throw;
            }
            return affectedRow;
        }
        public int Update(List<DbCommand> UpdateCommand)
        {
            int affectedRow = 0;
            OpenConnection();
            try
            {
                BeginTransaction();
                foreach (DbCommand comm in UpdateCommand)
                {
                    int affected = ExecuteNonQuery(comm);
                    affectedRow++;
                }
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
                CloseConnection();
                throw;
            }

            return affectedRow;
        }
        public int Update(DbCommand UpdateCommand, DbConnection conn, DbTransaction trans)
        {
            OpenConnection();
            int affectedRow = ExecuteNonQuery(UpdateCommand);
            CloseConnection();
            return affectedRow;
        }
        public int Update(List<DbCommand> UpdateCommand, DbConnection conn, DbTransaction trans)
        {
            int affectedRow = 0;
            OpenConnection();
            try
            {
                BeginTransaction();


                foreach (DbCommand comm in UpdateCommand)
                {
                    int affected = ExecuteNonQuery(comm);
                    affectedRow++;
                }
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
            }
            finally
            {
                CloseConnection();
            }
            return affectedRow;
        }
        #endregion
        #region Insert
        public int Insert(DbCommand InsertCommand)
        {
            int affectedRow = 0;
            OpenConnection();
            try
            {
                //BeginTransaction();
                affectedRow = ExecuteNonQuery(InsertCommand);
                //CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                // RollbackTransaction();
                CloseConnection();
                throw;
            }
            return affectedRow;
        }
        public int Insert(DbCommand InsertCommand, DbTransaction trans)
        {
            int affectedRow = 0;
            try
            {
                InsertCommand.Transaction = trans;
                affectedRow = ExecuteNonQuery(InsertCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return affectedRow;
        }
        public int Insert(List<DbCommand> InsertCommand)
        {
            int affectedRow = 0;
            OpenConnection();
            try
            {
                BeginTransaction();
                foreach (DbCommand comm in InsertCommand)
                {
                    int affected = ExecuteNonQuery(comm);
                    affectedRow++;
                }
                CommitTransaction();
                CloseConnection();
            }
            catch (Exception)
            {
                RollbackTransaction();
                CloseConnection();
                throw;
            }

            return affectedRow;
        }
        public int Insert(DbCommand InsertCommand, DbConnection conn, DbTransaction trans)
        {
            int affectedRow = 0;
            try
            {

                affectedRow = ExecuteNonQuery(InsertCommand);

            }
            catch (Exception)
            {
                throw;
            }
            return affectedRow;
        }
        public int Insert(List<DbCommand> InsertCommand, DbConnection conn, DbTransaction trans)
        {
            int affectedRow = 0;
            try
            {

                foreach (DbCommand comm in InsertCommand)
                {
                    int affected = ExecuteNonQuery(comm);
                    affectedRow++;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return affectedRow;
        }
        #endregion





    }
}
