using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using NHibernate;
using SCG.eAccounting.DTO.DataSet;
using System.Data;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class TADocumentTravellerDao : NHibernateDaoBase<TADocumentTraveller,int> , ITADocumentTravellerDao
    {
        #region public void Persist(TADocumentDataSet.TADocumentTravellerDataTable taDocumentTravellerDT)
        public void Persist(TADocumentDataSet.TADocumentTravellerDataTable taDocumentTravellerDT)
        {
            #region 25/03/2009
            //TADocumentDataSet.TADocumentTravellerDataTable deleteTable =
            //    (TADocumentDataSet.TADocumentTravellerDataTable)taDocumentTravellerDT.GetChanges(DataRowState.Deleted);
            //TADocumentDataSet.TADocumentTravellerDataTable updateTable =
            //    (TADocumentDataSet.TADocumentTravellerDataTable)taDocumentTravellerDT.GetChanges(DataRowState.Modified);
            //TADocumentDataSet.TADocumentTravellerDataTable insertTable =
            //    (TADocumentDataSet.TADocumentTravellerDataTable)taDocumentTravellerDT.GetChanges(DataRowState.Added);

            //int travellerID = 0;

            //if (deleteTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow in deleteTable)
            //    {
            //        travellerID = Convert.ToInt32(((DataRow)taDocumentTravellerRow)["TravellerID", DataRowVersion.Original].ToString());
            //        TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(travellerID);
            //        this.Delete(taDocumentTraveller);
            //    }
            //}

            //if (updateTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow in updateTable)
            //    {
            //        TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(taDocumentTravellerRow);
            //        this.SaveOrUpdate(taDocumentTraveller);
            //    }
            //}

            //if (insertTable != null)
            //{
            //    foreach (TADocumentDataSet.TADocumentTravellerRow taDocumentTravellerRow in insertTable)
            //    {
            //        TADocumentTraveller taDocumentTraveller = new TADocumentTraveller(taDocumentTravellerRow);
            //        this.Save(taDocumentTraveller);
            //    }
            //}
            #endregion 

            NHibernateAdapter<TADocumentTraveller, int> adapter = new NHibernateAdapter<TADocumentTraveller, int>();
            adapter.UpdateChange(taDocumentTravellerDT, ScgeAccountingDaoProvider.TADocumentTravellerDao);
        }
        #endregion public void Persist(TADocumentDataSet.TADocumentTravellerDataTable taDocumentTravellerDT)
    }
}
