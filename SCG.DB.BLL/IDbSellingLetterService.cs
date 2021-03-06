﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO.ValueObject;
using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbSellingLetterService : IService<DbSellingLetter, long>
    {
        void AddLetterAndDocument(long documentID, long letterID);
        string CheckDuplicateDocumentID(List<SellingRequestLetterParameter> generateList);
        string GenerateSellingLetter(List<SellingRequestLetterParameter> allList);
    }
}