using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbLocationLangDao : NHibernateDaoBase<DbLocationLang, long>, IDbLocationLangDao
    {
        public DbLocationLangDao()
        { 
        
        }
        public void DeleteAllLocationLang(long locationId)
        {
            GetCurrentSession()
            .Delete("from DbLocationLang ll where ll.Location.LocationID = :LocationID ", new object[] { locationId }, new NHibernate.Type.IType[] { NHibernateUtil.Int64 });
        }
    }
}
