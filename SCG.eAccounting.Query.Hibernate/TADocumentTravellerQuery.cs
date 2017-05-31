using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using System.Collections;
using NHibernate.Expression;

namespace SCG.eAccounting.Query.Hibernate
{
    public class TADocumentTravellerQuery : NHibernateQueryBase<TADocumentTraveller, int>, ITADocumentTravellerQuery
    {
        #region public IList<TADocumentTraveller>FindTADocumentTravellerByTADocumentID(long taDocumentID)
        public IList<TADocumentTraveller>FindTADocumentTravellerByTADocumentID(long taDocumentID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(TADocumentTraveller), "t");
            criteria.Add(Expression.Eq("t.TADocumentID.TADocumentID", taDocumentID));

            return criteria.List<TADocumentTraveller>();
        }
        #endregion public IList<TADocumentTraveller>FindTADocumentTravellerByTADocumentID(long taDocumentID)

        #region public IList<TADocumentTraveller>FindTADocumentTravellerByTADocumentID(long taDocumentID)
        public IList<TADocumentTraveller> FindTADocumentTravellerByTADocumentIDAndUserID(long taDocumentID, long UserID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(TADocumentTraveller), "t");
            criteria
                .Add(Expression.Eq("t.TADocumentID.TADocumentID", taDocumentID))
                .Add(Expression.Eq("t.UserID.Userid", UserID));

            return criteria.List<TADocumentTraveller>();
        }
        #endregion public IList<TADocumentTraveller>FindTADocumentTravellerByTADocumentID(long taDocumentID)
    }
}
