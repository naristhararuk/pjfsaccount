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
    public partial class RejectReasonDao : NHibernateDaoBase<DbRejectReason , int>, IRejectReasonDao
    {
        public RejectReasonDao()
        { 
        
        }
        public bool IsDuplicateReasonCode(DbRejectReason rejectReason)
        {
            IList<DbRejectReason> list = GetCurrentSession().CreateQuery("from DbRejectReason r where r.ReasonID <> :ReasonID and r.ReasonCode = :ReasonCode ")
                  .SetInt32("ReasonID", rejectReason.ReasonID)
                  .SetString("ReasonCode", rejectReason.ReasonCode)
                  .List<DbRejectReason>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
