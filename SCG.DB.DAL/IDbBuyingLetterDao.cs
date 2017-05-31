﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL
{
    public interface IDbBuyingLetterDao : IDao<DbBuyingLetter, long>
    {
        void InsertData(DbBuyingLetter buyingLetter);

        void DeleteLetter(DbBuyingLetter documentID);
        IList<string> CheckDuplicateDocumentID(string documentIDs);
    }
}