using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnRemittanceAdvanceQuery : NHibernateQueryBase<FnRemittanceAdvance, long>, IFnRemittanceAdvanceQuery
	{
        public IList<FnRemittanceAdvance> FindRemittanceAdvanceByRemittanceID(long remittanceID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnRemittanceAdvance), "ra");
            criteria.Add(Expression.Eq("ra.Remittance.RemittanceID", remittanceID));

            return criteria.List<FnRemittanceAdvance>();
        }
        public FnRemittanceAdvance FindRemittanceAdvanceByRemittanceIDAndAdvanceID(long remittanceID,long advanceID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnRemittanceAdvance), "ra");
            criteria.Add(Expression.And(Expression.Eq("ra.Remittance.RemittanceID", remittanceID),Expression.Eq("ra.Advance.AdvanceID",advanceID)));

            return criteria.UniqueResult<FnRemittanceAdvance>();
        }
        public IList<Advance> FindRemittanceAdvanceAndItemsByAdvanceIDs(List<long> advanceIdList)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT fs.RemittanceID as RemittanceID , fs.AdvanceID as AdvanceID , r.TotalAmount as RemittedAmountTHB ");
            sql.Append("FROM FnRemittanceAdvance as fs ");
            sql.Append("INNER JOIN FnRemittance as r ON fs.RemittanceID = r.RemittanceID ");
            sql.Append("WHERE fs.AdvanceID in (:advanceList) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("advanceList", advanceIdList);

            query.AddScalar("RemittanceID", NHibernateUtil.Int64);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            query.AddScalar("RemittedAmountTHB", NHibernateUtil.Double);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Advance))).List<Advance>();
        }

        public IList<AdvanceData> FindByRemittanceIDForUpdateRemittanceAdvance(long remittanceID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT   distinct  f.AdvanceID, [Document].DocumentDate AS RequestDateOfAdvance, f.UpdBy, f.UpdPgm AS ProgramCode ");
            sqlBuilder.Append(" FROM         FnRemittanceAdvance AS f INNER JOIN ");
            sqlBuilder.Append("           AvAdvanceDocument ON f.AdvanceID = AvAdvanceDocument.AdvanceID INNER JOIN ");
            sqlBuilder.Append("           [Document] ON AvAdvanceDocument.DocumentID = [Document].DocumentID ");
            sqlBuilder.Append(" WHERE f.RemittanceID = :RemittanceID AND f.Active = '1' ");
            sqlBuilder.Append(" ORDER BY [Document].DocumentDate ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetParameter("RemittanceID", remittanceID);
            query.AddScalar("AdvanceID", NHibernateUtil.Int64);
            // Use RequestDateOfAdvance to store document date
            query.AddScalar("RequestDateOfAdvance", NHibernateUtil.DateTime);
            query.AddScalar("UpdBy", NHibernateUtil.Int64);
            query.AddScalar("ProgramCode", NHibernateUtil.String);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(AdvanceData))).List<AdvanceData>();
        }
        public IList<FnRemittanceAdvance> FindRemittanceReferenceAdvanceByAdvanceID(long advanceID)
        {
            return GetCurrentSession().CreateQuery("from FnRemittanceAdvance where AdvanceID = :AdvanceID and active = '1'")
                    .SetInt64("AdvanceID", advanceID)
                    .List<FnRemittanceAdvance>();
        }
	}
}
