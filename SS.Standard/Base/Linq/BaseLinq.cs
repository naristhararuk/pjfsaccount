using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Data.Linq
{

    public abstract class BaseLinq<T> where T : class
    {
        public virtual void Update(T obj)
        {
            LinqHelper.Update<T>(obj);
        }

        public virtual void Update(List<T> obj)
        {
            LinqHelper.Update<T>(obj);
        }

        public virtual void Delete(T obj)
        {
            LinqHelper.Delete<T>(obj);
        }

        public virtual void Delete(List<T> obj)
        {
            LinqHelper.Delete<T>(obj);
        }

        public virtual void Insert(T obj)
        {
            LinqHelper.Insert<T>(obj);
        }

        public virtual void Insert(List<T> obj)
        {
            LinqHelper.Insert<T>(obj);
        }

        public virtual void SubmitChanges()
        {
            LinqHelper.SubmitChanges();
        }
    }
}