using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
namespace SS.DB.DAL
{
    public interface IDbCurrencyDao : IDao<DbCurrency, short>  
    {
        bool IsDuplicateSymbol(DbCurrency currency);
        //void DeleteByCurrencyId(short currencyId);
        //ICriteria FindBySuProgramCriteria(SuProgram program);
    }
}
