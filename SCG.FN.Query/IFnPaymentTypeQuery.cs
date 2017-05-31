﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SS.DB.DTO;

using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.Query
{
	public interface IFnPaymentTypeQuery : IQuery<FnPaymentType, short>
	{
		
	}
}
