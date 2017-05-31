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
	public class FnPaymentTypeLangQuery : NHibernateQueryBase<FnPaymentTypeLang, long>, IFnPaymentTypeLangQuery
	{
		#region IFnPaymentTypeLangQuery Members
		public IList<TranslatedListItem> GetTranslatedListItem(short languageID)
		{
			StringBuilder sqlBuilder = new StringBuilder();
			sqlBuilder.Append(" SELECT payment.PaymentTypeID as ID, payment.Name as Text ");
			sqlBuilder.Append(" FROM FnPaymentTypeLang payment WHERE payment.LanguageID = :LanguageID ");

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            query.AddScalar("ID", NHibernateUtil.Int16)
				.AddScalar("Text", NHibernateUtil.String);

			return query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem))).List<TranslatedListItem>();
		}
		#endregion
	}
}
