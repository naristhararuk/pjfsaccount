using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using NHibernate;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class TADocumentAdvanceDao : NHibernateDaoBase<TADocumentAdvance,int> , ITADocumentAdvanceDao
    {
        #region public void Persist(TADocumentDataSet.TADocumentAdvanceDataTable taDocumentAdvanceDT)
        public void Persist(TADocumentDataSet.TADocumentAdvanceDataTable taDocumentAdvanceDT)
        {
            NHibernateAdapter<TADocumentAdvance, int> adapter = new NHibernateAdapter<TADocumentAdvance, int>();
            adapter.UpdateChange(taDocumentAdvanceDT, ScgeAccountingDaoProvider.TADocumentAdvanceDao);
        }
        #endregion public void Persist(TADocumentDataSet.TADocumentAdvanceDataTable taDocumentAdvanceDT)
    }
}
