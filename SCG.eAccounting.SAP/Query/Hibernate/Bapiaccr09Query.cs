﻿using System;
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
    public class Bapiaccr09Query : NHibernateQueryBase<Bapiaccr09, long>, IBapiaccr09Query
    {
        public IList<Bapiaccr09> FindByDocID(long DocId, string DocKind)
        {
            return GetCurrentSession().CreateQuery("FROM Bapiaccr09 A WHERE  A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiaccr09>();
        }
        public IList<Bapiaccr09> FindByDocID(long DocId, string DocSeq, string DocKind)
        {
            return GetCurrentSession()
                .CreateQuery("FROM Bapiaccr09 A WHERE A.DocId = :DOC_ID AND A.DocSeq = :DOC_SEQ AND A.DocKind = :DOC_KIND")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_SEQ", DocSeq)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiaccr09>();
        }

        public void DeleteByDocID(long DocId, string DocKind)
        {
            GetCurrentSession()
            .Delete(" FROM Bapiaccr09 A WHERE A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND",
            new object[] { DocId, DocKind },
            new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.String });
        }
    }
}
