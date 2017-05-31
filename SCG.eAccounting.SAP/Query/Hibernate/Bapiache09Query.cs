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
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace SCG.eAccounting.SAP.Query.Hibernate
{
    public class Bapiache09Query : NHibernateQueryBase<Bapiache09, long>, IBapiache09Query
    {
        public IList<Bapiache09> FindByDocID(long DocId, string DocKind)
        {
            IList<Bapiache09> listChe1 =
            GetCurrentSession().CreateQuery(" FROM Bapiache09 A WHERE A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND Order by A.Id")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiache09>();

            IList<Bapiache09> listChe = new List<Bapiache09>();
            //get DocSeq  = M
            var list1 = from x in listChe1
                       where x.DocSeq == "M"
                       select x;

            //get DocSeq <> M AND  NOT LIKE 'B2C%' AND NOT LIKE 'W2C%'
            var list2 = from x in listChe1
                        where x.DocSeq != "M" && !x.DocSeq.Contains("B2C") && !x.DocSeq.Contains("W2C")
                        orderby x.DocSeq ascending
                        select x;

            //get DocSeq contain 'W2C'
            var list3 = from x in listChe1
                        where x.DocSeq.Contains("W2C")
                        orderby x.DocSeq ascending
                        select x;

            //get DocSeq LIKE 'B2C%'
            var list4 = from x in listChe1
                        where x.DocSeq.Contains("B2C")
                        orderby x.DocSeq ascending
                        select x;

            foreach (Bapiache09 cheObj in list1)
                listChe.Add(cheObj);

            foreach (Bapiache09 cheObj in list2)
                listChe.Add(cheObj);

            foreach (Bapiache09 cheObj in list3)
                listChe.Add(cheObj);

            foreach (Bapiache09 cheObj in list4)
                listChe.Add(cheObj);

            return listChe;
        }

        public IList<Bapiache09> FindByDocID(long DocId, string DocSeq, string DocKind)
        {
            return GetCurrentSession().CreateQuery(" FROM Bapiache09 A WHERE A.DocId = :DOC_ID AND A.DocSeq = :DOC_SEQ AND A.DocKind = :DOC_KIND ")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_SEQ", DocSeq)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiache09>();
        }

        public void DeleteByDocID(long DocId, string DocKind)
        {
            GetCurrentSession()
            .Delete(" FROM Bapiache09 A WHERE A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND",
            new object[] { DocId, DocKind },
            new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.String });
        }
    }
}
