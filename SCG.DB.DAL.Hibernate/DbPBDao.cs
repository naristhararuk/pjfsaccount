using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbPBDao : NHibernateDaoBase<Dbpb, long>, IDbPBDao
    {
        public bool IsDuplicatePBCode(Dbpb pb)
        {
            IList<Dbpb> list = GetCurrentSession().CreateQuery("from Dbpb where Pbid <> :Pbid and PBCode = :PBCode")
                  .SetInt64("Pbid", pb.Pbid)
                  .SetString("PBCode", pb.PBCode)
                  .List<Dbpb>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
