using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Linq;
using System.Text;
using SS.Standard.Data.Linq;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace SS.Standard.Base.Linq
{
    public class absLinq <T>
    {
        protected StandardDataContext Standard;
        protected string Where;
        protected string Select;
        protected string Order;

        public absLinq()
        {
            
            Standard = new StandardDataContext();
    
        }

        public absLinq(StandardDataContext DataContext)
        {

            Standard = DataContext;

        }

        public virtual void Update(T Obj) 
        {
            T NewObj = default(T);
            HelperLinq.ModeDetail('u',Obj);
            if (Standard.GetTable(Obj.GetType()).GetOriginalEntityState(Obj) == null)
            {
                NewObj = Detach(Obj);
                Standard.GetTable(Obj.GetType()).Attach(NewObj, true);
            }
        }

        public virtual void Update(List<T> Obj) 
        {
            T NewObj = default(T);
            for (int i = 0; i < Obj.Count; i++)
            {
                HelperLinq.ModeDetail('u', Obj[i]);
                if (Standard.GetTable(Obj[i].GetType()).GetOriginalEntityState(Obj[i]) == null)
                {
                    NewObj = Detach(Obj[i]);
                    Standard.GetTable(Obj[i].GetType()).Attach(NewObj, true);
                }
            }
        }

        public virtual void Insert(T Obj) 
        {
            HelperLinq.ModeDetail('u', Obj);
            HelperLinq.ModeDetail('c', Obj);
            Standard.GetTable(Obj.GetType()).InsertOnSubmit(Obj);
        }

        public virtual void Insert(List<T> Obj) 
        {
            for (int i = 0; i < Obj.Count; i++)
            {
                HelperLinq.ModeDetail('u',Obj[i]);
                HelperLinq.ModeDetail('c', Obj[i]);
            }
            Standard.GetTable(Obj.GetType()).InsertAllOnSubmit(Obj);
        }

        public virtual void Delete(T Obj) 
        {
            T NewObj = default(T);
            if (Standard.GetTable(Obj.GetType()).GetOriginalEntityState(Obj) == null)
            {
                NewObj = Detach(Obj);
                Standard.GetTable(Obj.GetType()).Attach(NewObj);
                Standard.GetTable(Obj.GetType()).DeleteOnSubmit(NewObj);
            }
            else Standard.GetTable(Obj.GetType()).DeleteOnSubmit(Obj);
        }

        public virtual void Delete(List<T> Obj) 
        {
            T NewObj = default(T);
            for (int i = 0; i < Obj.Count; i++)
            {
                if (Standard.GetTable(Obj[i].GetType()).GetOriginalEntityState(Obj[i]) == null)
                {
                    NewObj = Detach(Obj[i]);
                    Standard.GetTable(Obj[i].GetType()).Attach(NewObj);
                    Standard.GetTable(Obj[i].GetType()).DeleteOnSubmit(Obj);
                }
            }
            Standard.GetTable(Obj.GetType()).DeleteAllOnSubmit(Obj);
        }

        public void SubmitChanges()
        {
            Standard.SubmitChanges();
        }

        public List<T> ExecuteQuery<T>(string sql) where T : class
        {
                IEnumerable<T> result = Standard.ExecuteQuery<T>(sql);
                return result.ToList<T>();
        }

        public int ExecuteCommand(string sql)
        {
            return Standard.ExecuteCommand(sql);
        }

        public static T Detach(T Obj)
        {
            if (Obj == null)
                return default(T);
            T newObj = (T)Activator.CreateInstance(typeof(T));
            PropertyDescriptorCollection prop = TypeDescriptor.GetProperties(Obj);
            for (int i = 0; i < prop.Count; i++)
            {
                if (prop[i].Attributes[typeof(ColumnAttribute)] != null)
                {
                    object val = prop[i].GetValue(Obj);
                    prop[i].SetValue(newObj, val);
                }

            }
            return newObj;
        }

        //public List<T> Query<T>() where T : class
        //{
        //    var result;
        //    if (Where != null && Order != null && Select != null)
        //    {
        //        result = Standard.GetTable<T>().Where(Where).OrderBy(Order).Select(Select);

        //    }
        //    else if (Where != null && Order != null)
        //    {
        //        result = Standard.GetTable<T>().Where(Where).OrderBy(Order);
        //    }
        //    else if (Where != null)
        //    {
        //        result = Standard.GetTable<T>().Where(Where);
        //    }
        //    return null;
        //}

        //private List<T> AnonymousToList(var arg)
        //{

        //}
    }
}
