using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FixedAdvanceDocumentDao : NHibernateDaoBase<FixedAdvanceDocument, long>, IFixedAdvanceDocumentDao
    {
        #region Save MPADocument
        public long Persist(FixedAdvanceDataSet.FixedAdvanceDocumentDataTable dtFixedAdvanceDocument)
        {
            NHibernateAdapter<FixedAdvanceDocument, long> adapter = new NHibernateAdapter<FixedAdvanceDocument, long>();
            adapter.UpdateChange(dtFixedAdvanceDocument, ScgeAccountingDaoProvider.FixedAdvanceDocumentDao);
            return dtFixedAdvanceDocument.Rows[0].Field<long>(dtFixedAdvanceDocument.Columns["FixedAdvanceID"]);
        }
        #endregion

    }
}
