using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Mssql;

namespace SS.Standard.Base.Mssql
{
    public abstract class BaseMssql<T> where T : class
    {
        public virtual void Update(T obj)
        {
            SSMssqlHelper.Update<T>(obj);
           
        }

        public virtual void Update(List<T> obj)
        {
           // SSLinqHelper.Update<T>(obj);
           // SSMssqlHelper.Update<T>(obj);
        }

        public virtual void Delete(T obj)
        {
           // SSLinqHelper.Delete<T>(obj);
            SSMssqlHelper.Delete<T>(obj);
        }

        public virtual void Delete(List<T> obj)
        {
           // SSLinqHelper.Delete<T>(obj);
            SSMssqlHelper.Delete<T>(obj);
        }

        public virtual void Insert(T obj)
        {
           // SSLinqHelper.Insert<T>(obj);
            SSMssqlHelper.Insert<T>(obj);
        }

        public virtual void Insert(List<T> obj)
        {
           // SSLinqHelper.Insert<T>(obj);
            SSMssqlHelper.Insert<T>(obj);
        }

        public virtual void SubmitChanges()
        {
           // SSLinqHelper.SubmitChanges();
        }
       
       
    }
}
