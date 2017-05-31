using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DAL;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO.DataSet;
using SCG.eAccounting.Query;
using SS.Standard.Utilities;
using System.Data;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Security;

namespace SCG.eAccounting.BLL.Implement
{
    public class TADocumentScheduleService : ServiceBase<TADocumentSchedule, int>, ITADocumentScheduleService
    {
        #region local variable
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public ITADocumentService TADocumentService { get; set; }
        #endregion local variable

        #region Overrid Method
        public override IDao<TADocumentSchedule, int> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.TADocumentScheduleDao;
        }
        #endregion

        #region public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)
        public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)
        {
            IList<TADocumentSchedule> taDocumentScheduleList = ScgeAccountingQueryProvider.TADocumentScheduleQuery.FindTADocumentScheduleByTADocumentID(taDocumentID);

            foreach (TADocumentSchedule taDocumentSchedule in taDocumentScheduleList)
            {
                TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow = taDocumentDS.TADocumentSchedule.NewTADocumentScheduleRow();

                taDocumentScheduleRow.ScheduleID = taDocumentSchedule.ScheduleID;

                if (taDocumentSchedule.TADocumentID != null)
                {
                    taDocumentScheduleRow.TADocumentID = taDocumentSchedule.TADocumentID.TADocumentID;
                }
                if (taDocumentSchedule.Date != null)
                {
                    taDocumentScheduleRow.Date = taDocumentSchedule.Date.Value;
                }
                if (taDocumentSchedule.DepartureFrom != null)
                {
                    taDocumentScheduleRow.DepartureFrom = taDocumentSchedule.DepartureFrom;
                }
                if (taDocumentSchedule.ArrivalAt != null)
                {
                    taDocumentScheduleRow.ArrivalAt = taDocumentSchedule.ArrivalAt;
                }
                if (taDocumentSchedule.TravelBy != null)
                {
                    taDocumentScheduleRow.TravelBy = taDocumentSchedule.TravelBy;
                }
                if (taDocumentSchedule.Time != null)
                {
                    taDocumentScheduleRow.Time = taDocumentSchedule.Time;
                }
                if (taDocumentSchedule.FlightNo != null)
                {
                    taDocumentScheduleRow.FlightNo = taDocumentSchedule.FlightNo;
                }
                if (taDocumentSchedule.TravellingDetail != null)
                {
                    taDocumentScheduleRow.TravellingDetail = taDocumentSchedule.TravellingDetail;
                }
                taDocumentScheduleRow.Active = taDocumentSchedule.Active;
                taDocumentScheduleRow.CreBy = taDocumentSchedule.CreBy;
                taDocumentScheduleRow.CreDate = taDocumentSchedule.CreDate;
                taDocumentScheduleRow.UpdBy = taDocumentSchedule.UpdBy;
                taDocumentScheduleRow.UpdDate = taDocumentSchedule.UpdDate;
                taDocumentScheduleRow.UpdPgm = taDocumentSchedule.UpdPgm;

                // Add document initiator to datatable taDocumentDS.
                taDocumentDS.TADocumentSchedule.AddTADocumentScheduleRow(taDocumentScheduleRow);
            }
        }
        #endregion public void PrepareDataToDataset(TADocumentDataSet taDocumentDS, long taDocumentID)

        #region public int AddTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule)
        public int AddTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow = taDocumentDS.TADocumentSchedule.NewTADocumentScheduleRow();

            taDocumentScheduleRow.TADocumentID = taDocumentSchedule.TADocumentID.TADocumentID;

            if (taDocumentSchedule.Date == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Travelling date is required."));
            }
            else
            {
                taDocumentScheduleRow.Date = taDocumentSchedule.Date.Value;
            }
            if (string.IsNullOrEmpty(taDocumentSchedule.DepartureFrom))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Departure From is required."));
            }
            else
            {
                taDocumentScheduleRow.DepartureFrom = taDocumentSchedule.DepartureFrom;
            }

            if (string.IsNullOrEmpty(taDocumentSchedule.ArrivalAt))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("Arrival At is required."));
            }
            else
            {
                taDocumentScheduleRow.ArrivalAt = taDocumentSchedule.ArrivalAt;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            taDocumentScheduleRow.DepartureFrom = taDocumentSchedule.DepartureFrom;
            taDocumentScheduleRow.ArrivalAt = taDocumentSchedule.ArrivalAt;
            taDocumentScheduleRow.TravelBy = taDocumentSchedule.TravelBy;
            taDocumentScheduleRow.Time = taDocumentSchedule.Time;
            taDocumentScheduleRow.FlightNo = taDocumentSchedule.FlightNo;
            taDocumentScheduleRow.TravellingDetail = taDocumentSchedule.TravellingDetail;
            taDocumentScheduleRow.Active = taDocumentSchedule.Active;
            taDocumentScheduleRow.CreBy = UserAccount.UserID;
            taDocumentScheduleRow.CreDate = DateTime.Now;
            taDocumentScheduleRow.UpdBy = UserAccount.UserID;
            taDocumentScheduleRow.UpdDate = DateTime.Now;
            taDocumentScheduleRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentDS.TADocumentSchedule.AddTADocumentScheduleRow(taDocumentScheduleRow);
            return taDocumentScheduleRow.ScheduleID;        
        }
        #endregion public int AddTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule)

        #region public void UpdateTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule)
        public void UpdateTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow = taDocumentDS.TADocumentSchedule.FindByScheduleID(taDocumentSchedule.ScheduleID);
            TADocumentDataSet.TADocumentRow taDocumentRow = taDocumentDS.TADocument.FindByTADocumentID(taDocumentSchedule.TADocumentID.TADocumentID);

            taDocumentScheduleRow.BeginEdit();

            if (taDocumentSchedule.TADocumentID != null)
            {
                taDocumentScheduleRow.TADocumentID = taDocumentSchedule.TADocumentID.TADocumentID;
            }

            if (taDocumentSchedule.Date == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravellingDateIsRequired"));
            }
            else
            {
                if (!taDocumentRow.IsNull("FromDate")  && !taDocumentRow.IsNull("ToDate"))
                {
                    if (taDocumentSchedule.Date >= taDocumentRow.FromDate && taDocumentSchedule.Date <= taDocumentRow.ToDate)
                    {
                        taDocumentScheduleRow.Date = taDocumentSchedule.Date.Value;
                    }
                    else
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("TravellingDateIsNotBetweenTravellingFromToDate"));
                    }
                }                
            }

            if (string.IsNullOrEmpty(taDocumentSchedule.DepartureFrom))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("DepartureFromIsRequired"));
            }
            else
            {
                taDocumentScheduleRow.DepartureFrom = taDocumentSchedule.DepartureFrom;
            }

            if (string.IsNullOrEmpty(taDocumentSchedule.ArrivalAt))
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("ArrivalAtIsRequired"));
            }
            else
            {
                taDocumentScheduleRow.ArrivalAt = taDocumentSchedule.ArrivalAt;
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            taDocumentScheduleRow.TravelBy = taDocumentSchedule.TravelBy;
            taDocumentScheduleRow.Time = taDocumentSchedule.Time;
            taDocumentScheduleRow.FlightNo = taDocumentSchedule.FlightNo;
            taDocumentScheduleRow.TravellingDetail = taDocumentSchedule.TravellingDetail;
            taDocumentScheduleRow.Active = taDocumentSchedule.Active;
            taDocumentScheduleRow.CreBy = UserAccount.UserID;
            taDocumentScheduleRow.CreDate = DateTime.Now;
            taDocumentScheduleRow.UpdBy = UserAccount.UserID;
            taDocumentScheduleRow.UpdDate = DateTime.Now;
            taDocumentScheduleRow.UpdPgm = UserAccount.CurrentProgramCode;

            taDocumentScheduleRow.EndEdit();
        }
        #endregion public void UpdateTADocumentScheduleTransaction(Guid txID, TADocumentSchedule taDocumentSchedule)

        #region public void SaveTADocumentSchedule(Guid txID, long taDocumentID)
        public void SaveTADocumentSchedule(Guid txID, long taDocumentID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);

            // Insert, Delete TADocumentTraveller.
            ScgeAccountingDaoProvider.TADocumentScheduleDao.Persist(taDocumentDS.TADocumentSchedule);
        }
        #endregion public void SaveTADocumentSchedule(Guid txID, long taDocumentID)

        #region public DataTable DeleteTADocumentScheduleTransaction(Guid txID, int scheduleID)
        public DataTable DeleteTADocumentScheduleTransaction(Guid txID, int scheduleID)
        {
            TADocumentDataSet taDocumentDS = (TADocumentDataSet)TransactionService.GetDS(txID);
            TADocumentDataSet.TADocumentScheduleRow taDocumentScheduleRow = taDocumentDS.TADocumentSchedule.FindByScheduleID(scheduleID);
            taDocumentScheduleRow.Delete();

            return taDocumentDS.TADocumentSchedule;
        }
        #endregion public DataTable DeleteTADocumentScheduleTransaction(Guid txID, int scheduleID)
    }
}
