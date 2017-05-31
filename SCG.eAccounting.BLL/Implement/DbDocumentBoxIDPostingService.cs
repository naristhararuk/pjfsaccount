using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class DbDocumentBoxIDPostingService : ServiceBase<DbDocumentBoxidPosting,long>, IDbDocumentBoxIDPostingService
    {

        public override SS.Standard.Data.NHibernate.Dao.IDao<DbDocumentBoxidPosting, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.DbDocumentBoxIDPostingDao;
        }
    }
}
