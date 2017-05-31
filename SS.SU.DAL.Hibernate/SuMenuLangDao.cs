using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate.Expression;
using SS.SU.DTO.ValueObject;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuMenuLangDao : NHibernateDaoBase<SuMenuLang,long>,ISuMenuLangDao
    {
        public IList<SuMenuLang> FindByMenuId(short menuId)
        {
            IList<SuMenuLang> list = GetCurrentSession().CreateQuery("from SuMenuLang where MenuID = :MenuID")
                .SetInt64("MenuID", menuId).List<SuMenuLang>();

            return list;
        }
        public bool IsDuplicateMenu(short menuId, short languageId)
        {
            IList<SuMenuLang> list = GetCurrentSession().CreateQuery("from SuMenuLang where MenuID = :MenuID and Language.LanguageId = :LanguageId")
                .SetInt64("MenuID", menuId)
                .SetInt16("LanguageId", languageId)
                .List<SuMenuLang>();

            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        public void DeleteByMenuIdLanguageId(short menuId, short languageId)
        {
            GetCurrentSession()
                .Delete("from SuMenuLang where MenuID = :menuId and LanguageID = :languageId"
                , new object[] { menuId, languageId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int16, NHibernateUtil.Int16 });
        }
        public void DeleteByMenuId(short menuId)
        {
            GetCurrentSession()
                .Delete("from SuMenuLang where MenuID = :menuId"
                , new object[] { menuId }
                , new NHibernate.Type.IType[] { NHibernateUtil.Int16});
        }
    }
}
