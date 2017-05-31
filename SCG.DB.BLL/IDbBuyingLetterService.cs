using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbBuyingLetterService : IService<DbBuyingLetter, long>
    {
        void AddLetterAndDocument(long documentID, long letterID);
        void DeleteLetter(long DocumentID);
        string CheckDuplicateDocumentID(List<MoneyRequestSearchResult> generateList);
        string GenerateBuyingLetter(List<MoneyRequestSearchResult> allList);
    }
}