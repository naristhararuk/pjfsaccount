using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SS.SU.DAL
{
    public interface ISuMenuDao : IDao<SuMenu, short>  
    {
        bool IsDuplicateMenu(SuMenu menu);
        bool IsDuplicateMenuSeq(SuMenu menu);
        bool IsDuplicateMenuCode(SuMenu menu);
        ICriteria FindBySuMenuCriteria(SuMenu menu);
    }
}
