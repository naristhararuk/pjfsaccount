using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using NHibernate;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class DbDocumentImagePostingDao : NHibernateDaoBase<DbDocumentImagePosting, long>, IDbDocumentImagePostingDao
    {

        #region IDbDocumentImagePostingDao Members

        public void ImportFreshNewImagePosting()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into dbdocumentimageposting  ");
            sqlBuilder.AppendLine("select document.documentid, bapiache09.fi_doc as fidocnumber, 'N', null, null, ''  ");
            sqlBuilder.AppendLine("from document  inner join bapiache09 on document.documentid = bapiache09.doc_id  ");
            sqlBuilder.AppendLine("left join dbdocumentimageposting on document.documentid = dbdocumentimageposting.documentid  ");
            sqlBuilder.AppendLine("inner join  ( ");
            sqlBuilder.AppendLine("	select count(1) as cnt,document.documentid   ");
            sqlBuilder.AppendLine("	from document inner join bapiache09  ");
            sqlBuilder.AppendLine("	on document.documentid = bapiache09.doc_id  ");
            sqlBuilder.AppendLine("	where bapiache09.DOC_SEQ = 'M'   ");
            sqlBuilder.AppendLine("	GROUP BY document.documentid  ");
            sqlBuilder.AppendLine("	) t on document.documentid = t.documentid ");
            sqlBuilder.AppendLine("where document.documenttypeid in (1,3,4,5,7) and document.postingstatus in ('P','PP','C')  ");
            sqlBuilder.AppendLine("and dbdocumentimageposting.documentid is null and bapiache09.DOC_SEQ = 'M'  ");
            sqlBuilder.AppendLine("and cnt = 1 ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("count", NHibernateUtil.Int32);
            query.UniqueResult();
            
        }


        #endregion
    }
}
