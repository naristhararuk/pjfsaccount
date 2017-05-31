using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.FN.DTO;
using NHibernate;
using NHibernate.Transform;
using SCG.FN.DTO.ValueObject;
using SS.DB.DTO;

namespace SCG.FN.Query
{
    public interface IFnReceiptTypeQuery : IQuery<FnReceiptType, short>
    {
        ISQLQuery FindByReceiptTypeCriteria(FnReceiptTypeResult receiptType, string sortExpression, bool isCount, short languageId);
        IList<FnReceiptTypeResult> GetReceiptTypeList(FnReceiptTypeResult receiptType, short languageId, int firstResult, int maxResult, string sortExpression);
        int CountByReceiptTypeCriteria(FnReceiptTypeResult receiptType);
        IList<FnReceiptTypeResult> FindReceiptTypeLangByID(short receiptTypeId);
        IList<DbStatus> FindAllRecFlag(string programCode);
    }
}
