using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.FN.DTO;
using NHibernate;

namespace SCG.FN.DAL
{
    public interface IFnReceiptTypeDao : IDao<FnReceiptType,short>
    {
        IQuery FindGlAccountByCriteria(string accNo, short languageId);
        bool IsDuplicateReceiptTypeCode(FnReceiptType fnReceiptType);
        
    }
}
