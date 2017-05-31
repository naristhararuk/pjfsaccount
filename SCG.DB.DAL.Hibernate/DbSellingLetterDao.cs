using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.DAL;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbSellingLetterDao : NHibernateDaoBase<DbSellingLetter, long>, IDbSellingLetterDao
    {
        public DbSellingLetterDao()
        {
           
        }

        public void InsertData(DbSellingLetter SellingLetter)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("    INSERT INTO DbSellingLetter (LetterID,DocumentID,CreBy,CreDate,UpdBy,UpdDate)    ");
            sql.Append("    VALUES ( :letterID , :documentID , :creBy , :creDate , :updBy , :updDate ) ");

            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("letterID", typeof(long), SellingLetter.LetterID);
            param.AddParameterData("documentID", typeof(long), SellingLetter.DocumentID);
            param.AddParameterData("creBy", typeof(long), SellingLetter.CreBy);
            param.AddParameterData("creDate", typeof(DateTime), SellingLetter.CreDate);
            param.AddParameterData("updBy", typeof(long), SellingLetter.UpdBy);
            param.AddParameterData("updDate", typeof(DateTime), SellingLetter.UpdDate);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            param.FillParameters(query);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();

        }

        public IList<string> CheckDuplicateDocumentID(string documentIDs)
        {
            string sql = @" select d.DocumentNo as DocumentNo from dbo.DbSellingLetter letter
                inner join Document d on d.DocumentID = letter.DocumentID
                where letter.DocumentID in (select item from dbo.fnSplit(:documentList,',')) ";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            query.SetString("documentList", documentIDs);
            query.AddScalar("DocumentNo", NHibernateUtil.String);

            return query.List<string>();
        }
    }
}
