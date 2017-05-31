using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using SS.Standard.Data.Mssql;
namespace SS.Standard.Base.Mssql
{
    public abstract class absMssqlBase<T>
    {
        public abstract void Update(T Obj);
        public abstract void Update(List<T> Obj);

        public abstract void Insert(T Obj);
        public abstract void Insert(List<T> Obj);

        public abstract void Delete(T Obj);
        public abstract void Delete(List<T> Obj);



    }
}
