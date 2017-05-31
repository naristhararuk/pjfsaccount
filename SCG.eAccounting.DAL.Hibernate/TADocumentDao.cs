using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class TADocumentDao : NHibernateDaoBase<TADocument,long> , ITADocumentDao
    {
        #region public long Persist(TADocumentDataSet.TADocumentDataTable taDocumentDT)
        public long Persist(TADocumentDataSet.TADocumentDataTable taDocumentDT)
        {
            #region 25/03/2009
            //TADocumentDataSet.TADocumentDataTable deleteTable =
            //    (TADocumentDataSet.TADocumentDataTable)taDocumentDT.GetChanges(DataRowState.Deleted);
            //TADocumentDataSet.TADocumentDataTable updateTable =
            //    (TADocumentDataSet.TADocumentDataTable)taDocumentDT.GetChanges(DataRowState.Modified);
            //TADocumentDataSet.TADocumentDataTable insertTable = 
            //    (TADocumentDataSet.TADocumentDataTable)taDocumentDT.GetChanges(DataRowState.Added);

            //long taDocumentID = 0;

            //if (deleteTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentRow taDocumentRow in deleteTable)
            //    {
            //        taDocumentID = Convert.ToInt64(((DataRow)taDocumentRow)["TADocumentID", DataRowVersion.Original].ToString());
            //        TADocument taDocument = new TADocument(taDocumentID);
            //        this.Delete(taDocument);
            //    }
            //}

            //if (updateTable != null)
            //{
            //    TADocument taDocument = new TADocument((TADocumentDataSet.TADocumentRow)updateTable.Rows[0]);
            //    taDocumentID = taDocument.TADocumentID;
            //    this.SaveOrUpdate(taDocument);
            //}

            //if (insertTable != null)
            //{
            //    TADocument taDocument = new TADocument((TADocumentDataSet.TADocumentRow)insertTable.Rows[0]);
            //    taDocumentID = this.Save(taDocument);

            //    #region Update New TADocumentID to Dataset
            //    taDocumentDT.TADocumentIDColumn.ReadOnly = false;
            //    taDocumentDT.Rows[0].BeginEdit();
            //    taDocumentDT.Rows[0][taDocumentDT.TADocumentIDColumn.ColumnName] = taDocumentID;
            //    taDocumentDT.Rows[0].EndEdit();
            //    taDocumentDT.TADocumentIDColumn.ReadOnly = true;
            //    #endregion
            //}

            //return taDocumentID;
            #endregion 

            NHibernateAdapter<TADocument, long> adapter = new NHibernateAdapter<TADocument, long>();
            adapter.UpdateChange(taDocumentDT, ScgeAccountingDaoProvider.TADocumentDao);
            return taDocumentDT.Rows[0].Field<long>(taDocumentDT.Columns["TADocumentID"]);
        }
        #endregion public long Persist(TADocumentDataSet.TADocumentDataTable taDocumentDT)
    }
}
