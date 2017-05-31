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
    public class MPADocumentDao : NHibernateDaoBase<MPADocument, long>, IMPADocumentDao
    {
        #region Save MPADocument
        public long Persist(MPADocumentDataSet.MPADocumentDataTable dtMPADocument)
        {
            NHibernateAdapter<MPADocument, long> adapter = new NHibernateAdapter<MPADocument, long>();
            adapter.UpdateChange(dtMPADocument, ScgeAccountingDaoProvider.MPADocumentDao);
            return dtMPADocument.Rows[0].Field<long>(dtMPADocument.Columns["MPADocumentID"]);
        }
        #endregion

    }
}
