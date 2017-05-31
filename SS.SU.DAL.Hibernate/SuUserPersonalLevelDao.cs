using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
namespace SS.SU.DAL.Hibernate
{
    public class SuUserPersonalLevelDao : NHibernateDaoBase<SuUserPersonalLevel, string>, ISuUserPersonalLevelDao
    {
        public SuUserPersonalLevelDao()
        {
        }
        public bool IsDuplicateCode(SuUserPersonalLevel suUserPersonalLevel)
        {
            IList<SuUserPersonalLevel> list = GetCurrentSession().CreateQuery("from SuUserPersonalLevel su where su.PersonalLevel Like :personallevel")
                  .SetString("personallevel", suUserPersonalLevel.PersonalLevel)
                  .List<SuUserPersonalLevel>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
