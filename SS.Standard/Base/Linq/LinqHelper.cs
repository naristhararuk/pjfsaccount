using System;
using System.Configuration;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Web;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using SS.Standard.Data.Linq;
namespace SS.Standard.Data.Linq
{
    public class LinqHelper
    {

        public static StandardDataContext DataContext
        {
            get
            {
                if (System.Web.HttpContext.Current.Items["DataContext"] == null)
                {
                    StandardDataContext _DataContext = new StandardDataContext();
                    StringBuilder Log = new StringBuilder();
                    System.IO.TextWriter tw = new System.IO.StringWriter(Log);
                    _DataContext.Log = tw;
                    System.Web.HttpContext.Current.Items["DataContext"] = _DataContext;
                }
                return System.Web.HttpContext.Current.Items["DataContext"] as StandardDataContext;
            }
        }


        public static void Update<T>(T Obj) where T : class
        {
            T newObj = default(T);
            if (DataContext.GetTable(Obj.GetType()).GetOriginalEntityState(Obj) == null)
            {
                newObj = Detach<T>(Obj);
                DataContext.GetTable(Obj.GetType()).Attach(newObj);
            }
        }

        public static void Update<T>(List<T> Obj) where T : class
        {
            T newObj = default(T);
            foreach (T O in Obj)
            {
                if (DataContext.GetTable(O.GetType()).GetOriginalEntityState(O) == null)
                {
                    newObj = Detach<T>(O);
                    DataContext.GetTable(O.GetType()).Attach(newObj);
                }
            }
        }

        public static void Insert<T>(T Obj) where T : class
        {
            DataContext.GetTable(Obj.GetType()).InsertOnSubmit(Obj);
        }

        public static void Insert<T>(List<T> Obj) where T : class
        {
            foreach (T O in Obj)
            {
                DataContext.GetTable(O.GetType()).InsertOnSubmit(O);
            }
        }

        public static void Delete<T>(T Obj) where T : class
        {
            T newObj = default(T);
            if (DataContext.GetTable(Obj.GetType()).GetOriginalEntityState(Obj) == null)
            {
                newObj = Detach<T>(Obj);
                DataContext.GetTable(Obj.GetType()).Attach(newObj);
                DataContext.GetTable(Obj.GetType()).DeleteOnSubmit(newObj);
                return;
            }
            DataContext.GetTable(Obj.GetType()).DeleteOnSubmit(Obj);
        }

        public static void Delete<T>(List<T> Obj) where T : class
        {
            T newObj = default(T);
            foreach (T O in Obj)
            {
                if (DataContext.GetTable(O.GetType()).GetOriginalEntityState(O) == null)
                {
                    newObj = Detach<T>(O);
                    DataContext.GetTable(O.GetType()).Attach(newObj);
                    DataContext.GetTable(O.GetType()).DeleteOnSubmit(newObj);
                }
                else
                {
                    DataContext.GetTable(O.GetType()).DeleteOnSubmit(O);
                }
            }
        }

        public static void SubmitChanges()
        {
            if (System.Web.HttpContext.Current.Items["DataContext"] != null)
            {
                (System.Web.HttpContext.Current.Items["DataContext"] as DataContext).SubmitChanges();
            }

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

    }
}
