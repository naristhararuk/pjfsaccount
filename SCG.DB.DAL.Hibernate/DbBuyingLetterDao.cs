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
    public partial class DbBuyingLetterDao : NHibernateDaoBase<DbBuyingLetter, long>, IDbBuyingLetterDao
    {
        public DbBuyingLetterDao()
        {

        }

        public void InsertData(DbBuyingLetter buyingLetter)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("    INSERT INTO DbBuyingLetter (LetterID,DocumentID,CreBy,CreDate,UpdBy,UpdDate)    ");
            sql.Append("    VALUES ( :letterID , :documentID , :creBy , :creDate , :updBy , :updDate ) ");

            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("letterID", typeof(long), buyingLetter.LetterID);
            param.AddParameterData("documentID", typeof(long), buyingLetter.DocumentID);
            param.AddParameterData("creBy", typeof(long), buyingLetter.CreBy);
            param.AddParameterData("creDate", typeof(DateTime), buyingLetter.CreDate);
            param.AddParameterData("updBy", typeof(long), buyingLetter.UpdBy);
            param.AddParameterData("updDate", typeof(DateTime), buyingLetter.UpdDate);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            param.FillParameters(query);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();

        }

        public void DeleteLetter(DbBuyingLetter letter)
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(" DELETE FROM DbBuyingLetter WHERE LetterID = :letterId AND DocumentID = :documentID ");
            query.SetInt64("letterId", letter.LetterID);
            query.SetInt64("documentID", letter.DocumentID);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public IList<string> CheckDuplicateDocumentID(string documentIDs)
        {
            string sql = @" select d.DocumentNo as DocumentNo from dbo.DbBuyingLetter letter
                inner join Document d on d.DocumentID = letter.DocumentID
                where letter.DocumentID in (select item from dbo.fnSplit(:documentList,',')) ";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            query.SetString("documentList", documentIDs);
            query.AddScalar("DocumentNo", NHibernateUtil.String);

            return query.List<string>();
        }
    }
}