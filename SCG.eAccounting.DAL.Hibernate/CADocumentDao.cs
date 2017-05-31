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
    public class CADocumentDao : NHibernateDaoBase<CADocument, long>, ICADocumentDao
    {
        #region Save MPADocument
        public long Persist(CADocumentDataSet.CADocumentDataTable dtCADocument)
        {
            NHibernateAdapter<CADocument, long> adapter = new NHibernateAdapter<CADocument, long>();
            adapter.UpdateChange(dtCADocument, ScgeAccountingDaoProvider.CADocumentDao);
            return dtCADocument.Rows[0].Field<long>(dtCADocument.Columns["CADocumentID"]);
        }
        #endregion

    }
}
