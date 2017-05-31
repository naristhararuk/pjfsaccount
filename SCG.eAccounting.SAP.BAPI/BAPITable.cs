using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SCG.eAccounting.SAP.BAPI
{
    public abstract class BAPITable : CollectionBase
    {
        public virtual object CreateNewRow()
        {
            throw new NotImplementedException();
        }
        public virtual Type GetElementType()
        {
            throw new NotImplementedException();
        }
    }
}
