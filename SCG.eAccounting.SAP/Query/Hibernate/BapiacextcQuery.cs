using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Query.Interface;

namespace SCG.eAccounting.SAP.Query.Hibernate
{
    public class BapiacextcQuery : NHibernateQueryBase<Bapiacextc, long>, IBapiacextcQuery
    {
        public IList<Bapiacextc> FindByDocID(long DocId, string DocKind)
        {
            return GetCurrentSession().CreateQuery("FROM Bapiacextc A WHERE  A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiacextc>();
        }
        public IList<Bapiacextc> FindByDocID(long DocId, string DocSeq, string DocKind)
        {
            return GetCurrentSession().CreateQuery("FROM Bapiacextc A WHERE A.DocId = :DOC_ID AND A.DocSeq = :DOC_SEQ AND A.DocKind = :DOC_KIND")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_SEQ", DocSeq)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiacextc>();
        }

        public void DeleteByDocID(long DocId, string DocKind)
        {
            GetCurrentSession()
            .Delete(" FROM Bapiacextc A WHERE A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND",
            new object[] { DocId, DocKind },
            new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.String });
        }
    }
}
