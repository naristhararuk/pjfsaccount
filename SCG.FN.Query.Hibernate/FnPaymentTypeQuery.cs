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
	public class FnPaymentTypeQuery : NHibernateQueryBase<FnPaymentType, short>, IFnPaymentTypeQuery
	{
	}
}
