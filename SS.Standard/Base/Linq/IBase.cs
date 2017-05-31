using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Data.Linq
{
    public interface IBase<T>
    {
        void Insert(T obj);
        void Insert(List<T> Obj);

        void Update(T obj);
        void Update(List<T> Obj);

        void Delete(T obj);
        void Delete(List<T> Obj);
        void SubmitChanges();
    }
}
