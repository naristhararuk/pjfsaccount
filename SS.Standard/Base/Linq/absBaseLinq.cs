using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.Base.Linq
{
    public abstract class absBaseLinq<T>
    {
        public abstract void Update(T Obj);
        public abstract void Update(List<T> Obj);

        public abstract void Insert(T Obj);
        public abstract void Insert(List<T> Obj);

        public abstract void Delete(T Obj);
        public abstract void Delete(List<T> Obj);
    }
}
