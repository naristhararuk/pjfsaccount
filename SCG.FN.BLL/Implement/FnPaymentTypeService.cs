using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.BLL;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL.Implement
{
	public class FnPaymentTypeService : ServiceBase<FnPaymentType, short>, IFnPaymentTypeService
	{
		#region Override Method
		public override IDao<FnPaymentType, short> GetBaseDao()
		{
			return DaoProvider.FnPaymentTypeDao;
		}
		#endregion
	}
}
