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
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.Query.Hibernate
{
	public class AvAdvanceItemQuery : NHibernateQueryBase<AvAdvanceItem, long>, IAvAdvanceItemQuery
    {
        #region IList<AvAdvanceItem> FindByAvAdvanceItemAdvanceID(long advanceId)
        public IList<AvAdvanceItem> FindByAvAdvanceItemAdvanceID(long advanceId)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(AvAdvanceItem), "item");
            //criteria.Add(Expression.Eq("item.AdvanceID.AdvanceID", advanceId));
            criteria.Add(Expression.And(Expression.Eq("item.AdvanceID.AdvanceID", advanceId), Expression.Eq("item.Active", true)));

            return criteria.List<AvAdvanceItem>();
        }
        public AvAdvanceItem GetByAvAdvanceItemAdvanceID(long advanceId)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(AvAdvanceItem), "a");
            criteria.Add(Expression.And(Expression.Eq("a.AdvanceID.AdvanceID",advanceId),Expression.Eq("a.Active",true)));
            return criteria.UniqueResult<AvAdvanceItem>();
        }
        #endregion


		#region GetAdvanceExchangeRate
		public InvoiceExchangeRate GetAdvanceExchangeRate(IList<long> advanceIDlist, short currencyID)
		{
			StringBuilder queryBuilder = new StringBuilder();
			queryBuilder.AppendLine(" SELECT SUM(ISNULL(Amount,0)) as TotalAmount, SUM(ISNULL(AmountTHB,0)) as TotalAmountTHB, SUM(ISNULL(MainCurrencyAmount,0)) as TotalAmountMainCurrency ");
			queryBuilder.AppendLine(" FROM AvAdvanceItem ");
			queryBuilder.AppendLine(" WHERE AdvanceID in ( :advanceIDList ) ");
			queryBuilder.AppendLine(" AND CurrencyID = :currencyID ");
			queryBuilder.AppendLine(" GROUP BY CurrencyID ");
			
			ISQLQuery query = GetCurrentSession().CreateSQLQuery(queryBuilder.ToString());
			query.SetParameterList("advanceIDList", advanceIDlist);
			query.SetInt16("currencyID", currencyID);
			query.AddScalar("TotalAmount", NHibernateUtil.Double);
			query.AddScalar("TotalAmountTHB", NHibernateUtil.Double);
            query.AddScalar("TotalAmountMainCurrency", NHibernateUtil.Double);
			query.SetResultTransformer(Transformers.AliasToBean(typeof(InvoiceExchangeRate)));
			
			IList<InvoiceExchangeRate> list = query.List<InvoiceExchangeRate>();
			if (list.Count > 0)
			{
				return list[0];
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
