using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using System.Data;
using SCG.eAccounting.DTO.DataSet;


namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class DocumentViewLockDao : NHibernateDaoBase<DocumentViewLock, long>, IDocumentViewLockDao
    {
        public DocumentViewLockDao()
        {
        }
    }
}

