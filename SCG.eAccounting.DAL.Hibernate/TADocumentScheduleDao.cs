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
    public partial class TADocumentScheduleDao : NHibernateDaoBase<TADocumentSchedule,int> , ITADocumentScheduleDao
    {
        #region public void Persist(TADocumentDataSet.TADocumentScheduleDataTable taDocumentScheduleDT)
        public void Persist(TADocumentDataSet.TADocumentScheduleDataTable taDocumentScheduleDT)
        {
            #region 25/03/2009
            //TADocumentDataSet.TADocumentScheduleDataTable deleteTable =
            //    (TADocumentDataSet.TADocumentScheduleDataTable)taDocumentScheduleDT.GetChanges(DataRowState.Deleted);
            //TADocumentDataSet.TADocumentScheduleDataTable updateTable =
            //    (TADocumentDataSet.TADocumentScheduleDataTable)taDocumentScheduleDT.GetChanges(DataRowState.Modified);
            //TADocumentDataSet.TADocumentScheduleDataTable insertTable =
            //    (TADocumentDataSet.TADocumentScheduleDataTable)taDocumentScheduleDT.GetChanges(DataRowState.Added);

            //int scheduleID = 0;

            //if (deleteTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow in deleteTable)
            //    {
            //        scheduleID = Convert.ToInt32(((DataRow)taDocumentScheduleRow)["ScheduleID", DataRowVersion.Original].ToString());
            //        TADocumentSchedule taDocumentSchedule = new TADocumentSchedule(scheduleID);
            //        this.Delete(taDocumentSchedule);
            //    }
            //}

            //if (updateTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow in updateTable)
            //    {
            //        TADocumentSchedule taDocumentSchedule = new TADocumentSchedule(taDocumentScheduleRow);
            //        this.SaveOrUpdate(taDocumentSchedule);
            //    }
            //}

            //if (insertTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow in insertTable)
            //    {
            //        TADocumentSchedule taDocumentSchedule = new TADocumentSchedule(taDocumentScheduleRow);
            //        this.Save(taDocumentSchedule);
            //    }

            //}
            #endregion 

            NHibernateAdapter<TADocumentSchedule, int> adapter = new NHibernateAdapter<TADocumentSchedule, int>();
            adapter.UpdateChange(taDocumentScheduleDT, ScgeAccountingDaoProvider.TADocumentScheduleDao);
        }
        #endregion public void Persist(TADocumentDataSet.TADocumentScheduleDataTable taDocumentScheduleDT)
    }
}
