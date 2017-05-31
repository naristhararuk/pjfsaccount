using System;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Web;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using SS.Standard.Data;
using SS.Standard.Data.Mssql;
using SS.Standard.Base.Mssql;
using SS.Standard.Data.Interfaces;
namespace SS.Standard.Data.Mssql
{
    public class SSMssqlHelper :MssqlHelper,IDBManager
    {
        public static void Update<T>(T Obj) where T : class
        {
           // T newObj = default(T);
           // if (DataContext.GetTable(Obj.GetType()).GetOriginalEntityState(Obj) == null)
           // {
               // newObj = Detach<T>(Obj);
               // DataContext.GetTable(Obj.GetType()).Attach(newObj);
            //}
        }

        public void Update<T>(List<T> Obj) where T : class
        {
            daoMssql<T> Helper=null;
           // T newObj = default(T);
            //foreach (T O in Obj)
            //{
            //    if (DataContext.GetTable(O.GetType()).GetOriginalEntityState(O) == null)
            //    {
            //        newObj = Detach<T>(O);
            //        DataContext.GetTable(O.GetType()).Attach(newObj);
            //    }
            //}
            OpenConnection();
            BeginTransaction();
            try
            {
                foreach (T item in Obj)
                {
                   
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = Helper.Update(item);
                    int affectedRow = ExecuteNonQuery(command);

                }
                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
            CloseConnection();
        }

        public static void Insert<T>(T Obj) where T : class
        {
           // DataContext.GetTable(Obj.GetType()).InsertOnSubmit(Obj);
        }

        public static void Insert<T>(List<T> Obj) where T : class
        {
            //foreach (T O in Obj)
            //{
            //    DataContext.GetTable(O.GetType()).InsertOnSubmit(O);
            //}
        }

        public static void Delete<T>(T Obj) where T : class
        {
           // T newObj = default(T);
           // if (DataContext.GetTable(Obj.GetType()).GetOriginalEntityState(Obj) == null)
           // {
           //     newObj = Detach<T>(Obj);
           //     DataContext.GetTable(Obj.GetType()).Attach(newObj);
           //     DataContext.GetTable(Obj.GetType()).DeleteOnSubmit(newObj);
           //     return;
           // }
           //DataContext.GetTable(Obj.GetType()).DeleteOnSubmit(Obj);
        }

        public static void Delete<T>(List<T> Obj) where T : class
        {
            //T newObj = default(T);
            //foreach (T O in Obj)
            //{
            //    if (DataContext.GetTable(O.GetType()).GetOriginalEntityState(O) == null)
            //    {
            //        newObj = Detach<T>(O);
            //        DataContext.GetTable(O.GetType()).Attach(newObj);
            //        DataContext.GetTable(O.GetType()).DeleteOnSubmit(newObj);
            //    }
            //    else
            //    {
            //        DataContext.GetTable(O.GetType()).DeleteOnSubmit(O);
            //    }
            //}
        }

        public static void SubmitChanges()
        {
            //if(DataContext != null)
            //    DataContext.SubmitChanges();
        }

       
        public static T Detach<T>(T Obj) where T : class
        {
            if (Obj == null)
                return null;
            T newObj = (T)Activator.CreateInstance(typeof(T));
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(Obj))
            {
                if (prop.Attributes[typeof(ColumnAttribute)] == null)
                {
                    continue;
                }
                object val = prop.GetValue(Obj);
                prop.SetValue(newObj, val);
            }
            return newObj;
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



