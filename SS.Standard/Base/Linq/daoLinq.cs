using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Security;
using SS.Standard.Data.Linq;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace SS.Standard.Base.Linq
{
    public class daoLinq <T>
    {
        private System.Data.Linq.DataContext Standard;

        public daoLinq(System.Data.Linq.DataContext DataContext)
        {

            Standard = DataContext;

        }

        public virtual void Update(T Obj) 
        {
            
            UserEngine.SessionTimeOut();
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
            UserEngine.SessionTimeOut();
            T NewObj = default(T);
            System.Data.Linq.ITable Table = null;
            for (int i = 0; i < Obj.Count; i++)
            {
                if (i == 0) Table = Standard.GetTable(Obj[i].GetType());
                HelperLinq.ModeDetail('u', Obj[i]);
                if (Table.GetOriginalEntityState(Obj[i]) == null)
                {
                    NewObj = Detach(Obj[i]);
                    Table.Attach(NewObj, true);
                }
            }
        }

        public virtual void Insert(T Obj) 
        {
            UserEngine.SessionTimeOut();
            HelperLinq.ModeDetail('u', Obj);
            HelperLinq.ModeDetail('c', Obj);
            Standard.GetTable(Obj.GetType()).InsertOnSubmit(Obj);
        }

        public virtual void Insert(List<T> Obj) 
        {
            UserEngine.SessionTimeOut();
            System.Data.Linq.ITable Table = null;
            for (int i = 0; i < Obj.Count; i++)
            {
                if (i == 0) Table = Standard.GetTable(Obj[i].GetType());
                HelperLinq.ModeDetail('u', Obj[i]);
                HelperLinq.ModeDetail('c', Obj[i]);
                Table.InsertOnSubmit(Obj[i]);
            }
        }

        public virtual void Delete(T Obj) 
        {
            UserEngine.SessionTimeOut();
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
            UserEngine.SessionTimeOut();
            T NewObj = default(T);
            System.Data.Linq.ITable Table = null;
            for (int i = 0; i < Obj.Count; i++)
            {
                if (i == 0) Table = Standard.GetTable(Obj[i].GetType());
                if (Table.GetOriginalEntityState(Obj[i]) == null)
                {
                    NewObj = Detach(Obj[i]);
                    Table.Attach(NewObj);
                    Table.DeleteOnSubmit(NewObj);
                }
                else Table.DeleteOnSubmit(Obj[i]);
            }
        }

        public void SubmitChanges()
        {
            Standard.SubmitChanges();
        }

        public List<T> ExecuteQuery<T>(string sql) where T : class
        {
                UserEngine.SessionTimeOut();
                IEnumerable<T> result = Standard.ExecuteQuery<T>(sql);
                return result.ToList<T>();
        }

        public int ExecuteCommand(string sql)
        {
            UserEngine.SessionTimeOut();
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
    }
}
