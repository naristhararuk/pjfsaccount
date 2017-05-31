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
    public class FnRemittanceItemQuery : NHibernateQueryBase<FnRemittanceItem, long>, IFnRemittanceItemQuery
	{
        public IList<FnRemittanceItem> FindRemittanceItemByRemittanceID(long remittanceID)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(FnRemittanceItem), "ri");
            criteria.Add(Expression.Eq("ri.Remittance.RemittanceID", remittanceID));

            return criteria.List<FnRemittanceItem>();
        }

        public IList<RemittanceValueObj> FindRemittanceItemByRemittanceIds(IList<long> remittanceIdList)
        {
            return FindRemittanceItemByRemittanceIds(remittanceIdList, 1);
        }
        public IList<RemittanceValueObj> FindRemittanceItemByRemittanceIds(IList<long> remittanceIdList, short languageId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT w.WorkflowID, r.RemittanceID as RemittanceID , d.Documentno as RemittanceNo , sl.StatusDesc as PaymentType , c.Symbol as Currency, ");
            sql.Append("ri.ForeignCurrencyAdvanced AS ForeignCurrencyAdvanced,ri.ExchangeRate AS ExchangeRate, ");
            sql.Append("ri.ForeignCurrencyRemitted AS ForeignCurrencyRemitted, ri.AmountTHB AS RemittanceAmountTHB, ");
            sql.Append("ri.MainCurrencyAmount AS RemittanceAmountMainCurrency ");//r.TotalAmount AS RemittanceAmountTHB ");
            sql.Append("FROM FnRemittance AS r ");
            sql.Append("INNER JOIN Document d ON d.DocumentID = r.DocumentID ");
            sql.Append("INNER JOIN Workflow w ON w.DocumentID = d.DocumentID ");
            sql.Append("INNER JOIN WorkflowState wState ON wState.workflowstateid = w.currentState ");
            sql.Append("INNER JOIN FnRemittanceItem ri ON ri.RemittanceID = r.RemittanceID ");
            sql.Append("INNER JOIN DbStatus s ON s.Status = ri.PaymentType AND S.GroupStatus = 'PaymentTypeFRN' ");
            sql.Append("INNER JOIN DbStatusLang sl ON sl.StatusID = s.StatusID AND sl.LanguageID = ");
            sql.Append(languageId.ToString());   
            sql.Append(" INNER JOIN DbCurrency AS c ON c.CurrencyID = ri.CurrencyID ");
            sql.Append("WHERE r.RemittanceID in(:remittanceIdList) and wState.Name = 'Complete' ");
            sql.Append("ORDER BY RemittanceNo");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetParameterList("remittanceIdList", remittanceIdList);
            query.AddScalar("WorkflowID", NHibernateUtil.Int64);
            query.AddScalar("RemittanceID", NHibernateUtil.Int64);
            query.AddScalar("RemittanceNo", NHibernateUtil.String);
            query.AddScalar("PaymentType", NHibernateUtil.String);
            query.AddScalar("Currency", NHibernateUtil.String);
            query.AddScalar("ForeignCurrencyAdvanced", NHibernateUtil.Double);
            query.AddScalar("ExchangeRate", NHibernateUtil.Double);
            query.AddScalar("ForeignCurrencyRemitted", NHibernateUtil.Double);
            query.AddScalar("RemittanceAmountTHB", NHibernateUtil.Double);
            query.AddScalar("RemittanceAmountMainCurrency", NHibernateUtil.Double);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(RemittanceValueObj))).List<RemittanceValueObj>();
        }
	}
}
