using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuMenuDao : NHibernateDaoBase<SuMenu, short>, ISuMenuDao
    {
        public bool IsDuplicateMenu(SuMenu menu)
        {
            IList<SuMenu> list = GetCurrentSession().CreateQuery("from SuMenu where MenuID = :MenuID")
                  .SetInt64("MenuID", menu.Menuid)
                  .List<SuMenu>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsDuplicateMenuSeq(SuMenu menu)
        {
            if (menu.MenuMainid.HasValue)
            {
                IList<SuMenu> list = GetCurrentSession().CreateQuery("from SuMenu where MenuSeq = :MenuSeq and MenuMainId = :MenuMainId ")
                      .SetInt64("MenuSeq", menu.MenuSeq)
                      .SetInt64("MenuMainId", menu.MenuMainid.Value)
                      .List<SuMenu>();
                if (list.Count > 0)
                {
                    return true;
                }
            }
            return false;
            
        }
        public bool IsDuplicateMenuCode(SuMenu menu)
        {
            IList<SuMenu> list = GetCurrentSession().CreateQuery("from SuMenu where MenuCode = :MenuCode and MenuId <> :MenuId ")
                  .SetString("MenuCode", menu.MenuCode)
                  .SetInt64("MenuId", menu.Menuid)
                  .List<SuMenu>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public ICriteria FindBySuMenuCriteria(SuMenu menu)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(SuMenu), "t");

            /*
             Insert Criteria
             if (!string.IsNullOrEmpty(criteria))
            {
                criteria.Add(Expression.Like("..."));
            }
             */

            return criteria;
        }
    }
}
