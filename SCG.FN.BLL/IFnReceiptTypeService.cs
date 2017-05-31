using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL
{
    public interface IFnReceiptTypeService : IService<FnReceiptType,short>
    {
        IList<FnReceiptTypeResult> FindByFnReceiptTypeCriteria(string accNo, short languageId, int firstResult, int maxResults, string sortExpression);
        int CountByFnReceiptTypeCriteria(string accNo, short languageId);
        void UpdateReceiptTypeLang(IList<FnReceiptTypeLang> receiptTypeLangList);
        void AddReceiptType(FnReceiptType fnReceiptType);
        void UpdateReceiptType(FnReceiptType fnReceiptType);
    }
}
