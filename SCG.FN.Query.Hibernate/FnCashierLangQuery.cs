using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

using SS.DB.DTO;

namespace SCG.FN.Query.Hibernate
{
	public class FnCashierLangQuery : NHibernateQueryBase<FnCashierLang, long>, IFnCashierLangQuery
	{
		#region IFnCashierLangQuery Members
		public IList<TranslatedListItem> GetTranslatedListItem(short languageID)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.Append(" SELECT cashier.CashierID as ID, cashier.CashierName as Text ");
			sqlBuilder.Append(" FROM FnCashierLang cashier WHERE cashier.LanguageID = :LanguageID ");

			QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
			parameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);

			ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
			parameterBuilder.FillParameters(query);
			query.AddScalar("ID", NHibernateUtil.Int16)
				.AddScalar("Text", NHibernateUtil.String);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
		}

        public IList<FnCashierSearchResult> FindCashierByCashierId(short cashierId)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT l.LanguageID as LanguageID, l.LanguageName as LanguageName, cl.CashierID as CashierId, ");
            sqlBuilder.Append(" cl.CashierName as CashierName, cl.Active as Active ");
            sqlBuilder.Append(" FROM DbLanguage l ");
            sqlBuilder.Append(" left join FnCashierLang cl on l.LanguageID = cl.LanguageID and cl.CashierID = :CashierID");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("CashierID", typeof(short), cashierId);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("LanguageID", NHibernateUtil.Int16)
                .AddScalar("LanguageName", NHibernateUtil.String)
                .AddScalar("CashierId", NHibernateUtil.Int16)
                .AddScalar("CashierName", NHibernateUtil.String)
                .AddScalar("Active", NHibernateUtil.Boolean);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(FnCashierSearchResult)));

            return query.List<FnCashierSearchResult>();
        }
		#endregion
	}
}
