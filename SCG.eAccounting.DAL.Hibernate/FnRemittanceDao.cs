using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
    public class FnRemittanceDao : NHibernateDaoBase<FnRemittance, long>, IFnRemittanceDao
    {
        public long Persist(FnRemittanceDataset.FnRemittanceDataTable dtRemittanceDocument)
        {
            NHibernateAdapter<FnRemittance, long> adapter = new NHibernateAdapter<FnRemittance, long>();
            adapter.UpdateChange(dtRemittanceDocument, ScgeAccountingDaoProvider.FnRemittanceDao);

            #region Old 24-March-2009
            //FnRemittanceDataset.FnRemittanceDataTable updateTable =
            //    (FnRemittanceDataset.FnRemittanceDataTable)dtRemittanceDocument.GetChanges(DataRowState.Modified);
            //FnRemittanceDataset.FnRemittanceDataTable insertTable =
            //    (FnRemittanceDataset.FnRemittanceDataTable)dtRemittanceDocument.GetChanges(DataRowState.Added);
            //FnRemittanceDataset.FnRemittanceAdvanceDataTable deleteTable = 
            //    (FnRemittanceDataset.FnRemittanceAdvanceDataTable)dtRemittanceDocument.GetChanges(DataRowState.Deleted);

            //long remittanceDocumentID = 0;

            //// Update BgBudgetDocument.
            //if (updateTable != null)
            //{
            //    FnRemittanceDataset.FnRemittanceRow row = (FnRemittanceDataset.FnRemittanceRow)updateTable.Rows[0];
            //    FnRemittance remittance = this.FindByIdentity(row.RemittanceID);//new FnRemittance((FnRemittanceDataset.FnRemittanceRow)updateTable.Rows[0]);
            //    remittanceDocumentID = remittance.RemittanceID;
            //    this.SaveOrUpdate(remittance);
            //}

            //if (insertTable != null)
            //{
            //    FnRemittanceDataset.FnRemittanceRow row = (FnRemittanceDataset.FnRemittanceRow)insertTable.Rows[0];
            //    FnRemittance remittanceDocument = new FnRemittance(row);
            //    remittanceDocumentID = this.Save(remittanceDocument);

            //    dtRemittanceDocument.RemittanceIDColumn.ReadOnly = false;
            //    dtRemittanceDocument.Rows[0].BeginEdit();
            //    dtRemittanceDocument.Rows[0][dtRemittanceDocument.RemittanceIDColumn.ColumnName] = remittanceDocumentID;
            //    dtRemittanceDocument.Rows[0].EndEdit();
            //    dtRemittanceDocument.RemittanceIDColumn.ReadOnly = true;
            //}
            //if (deleteTable != null)
            //{
            //    long remittanceID = Convert.ToInt64((deleteTable.Rows[0])["RemittanceID", DataRowVersion.Original].ToString());
            //    FnRemittance remittanceDocument = new FnRemittance(remittanceID);
            //    this.Delete(remittanceDocument);

            //}
            #endregion

            return dtRemittanceDocument.Rows[0].Field<long>(dtRemittanceDocument.Columns["RemittanceID"]);
        }
        public void DeleteAllRemittanceItemAndRemittanceAdvance(long remittanceID)
        {
            GetCurrentSession().Delete("From FnRemittanceItem Where RemittanceID =:remittanceID",remittanceID,NHibernateUtil.Int64);

            GetCurrentSession().Delete("From FnRemittanceAdvance Where RemittanceID =:remittanceID", remittanceID, NHibernateUtil.Int64);
                                
        }
    }
}
