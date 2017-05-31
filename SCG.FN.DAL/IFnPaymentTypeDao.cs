using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SCG.FN.DTO;

using SS.Standard.Data.NHibernate.Dao;

namespace SCG.FN.DAL
{
	public interface IFnPaymentTypeDao : IDao<FnPaymentType, short>
	{
		
	}
}
