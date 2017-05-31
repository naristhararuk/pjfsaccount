using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SCG.eAccounting.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.Security;
using System.Data;
using SCG.eAccounting.Query;
using SS.DB.Query;
using SCG.DB.DTO;
using SCG.DB.Query;

namespace SCG.eAccounting.BLL.Implement
{
    public partial class FnExpenseMileageItemService : ServiceBase<FnExpenseMileageItem, long>, IFnExpenseMileageItemService
    {
        public IUserAccount UserAccount { get; set; }
        public ITransactionService TransactionService { get; set; }
        public IFnExpenseMileageService FnExpenseMileageService { get; set; }
        public IFnExpenseMileageItemQuery FnExpenseMileageItemQuery { get; set; }

        public override IDao<FnExpenseMileageItem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.FnExpenseMileageItemDao;
        }
        public void AddExpenseMileageItemOnTransaction(FnExpenseMileageItem expenseMileageItem, Guid txId)
        {
            this.ValidateMileageItem(expenseMileageItem, txId);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageItemRow row = ds.FnExpenseMileageItem.NewFnExpenseMileageItemRow();

            row.TravelDate = expenseMileageItem.TravelDate.Value;
            row.LocationFrom = expenseMileageItem.LocationFrom;
            row.LocationTo = expenseMileageItem.LocationTo;
            row.CarMeterStart = (decimal)expenseMileageItem.CarMeterStart.Value;
            row.CarMeterEnd = (decimal)expenseMileageItem.CarMeterEnd.Value;
            row.DistanceTotal = (decimal)ComputeDistanceTotal(expenseMileageItem.CarMeterStart.Value, expenseMileageItem.CarMeterEnd.Value);
            if (expenseMileageItem.ExpenseMileage != null)
            {
                row.ExpenseMileageID = expenseMileageItem.ExpenseMileage.ExpenseMileageID;
                if (expenseMileageItem.ExpenseMileage.Owner.Equals(OwnerMileage.Employee))
                {
                    row.DistanceFirst100Km = (decimal)ComputeFirstDistance(expenseMileageItem.CarMeterStart.Value, expenseMileageItem.CarMeterEnd.Value, expenseMileageItem.ExpenseMileage.TypeOfCar);
                    row.DistanceExceed100Km = (decimal)ComputeExceedDistance(expenseMileageItem.CarMeterStart.Value, expenseMileageItem.CarMeterEnd.Value, expenseMileageItem.ExpenseMileage.TypeOfCar);
                }
                else
                {
                    row.DistanceAdjust = (decimal)expenseMileageItem.DistanceAdjust;
                    row.DistanceNet = (decimal)ComputeDistanceNet(row.DistanceTotal, (row.DistanceAdjust));
                }
            }

            row.Active = true;
            row.CreDate = DateTime.Now;
            row.CreBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdBy = UserAccount.UserID;
            row.UpdPgm = UserAccount.CurrentProgramCode;
            ds.FnExpenseMileageItem.AddFnExpenseMileageItemRow(row);

            //Calculate for Mileage
            if (expenseMileageItem.ExpenseMileage != null)
            {
                FnExpenseMileageService.UpdateMileageSummary(txId, expenseMileageItem.ExpenseMileage);
            }
        }

        public void AddExpenseValidationMileageItemOnTransaction(FnExpenseMileageItem expenseMileageItem, Guid txId, long expDocumentID)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = (ExpenseDataSet.FnExpenseMileageRow)ds.FnExpenseMileage.Select("ExpenseID = " + expDocumentID).FirstOrDefault();//.FindByExpenseMileageID(expenseMileageID);
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(expDocumentID);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                DataRow[] row = ds.FnExpenseMileageItem.Select("ExpenseMileageID = " + mileageRow.ExpenseMileageID);
                foreach (DataRow dataRow in row)
                {
                    long id = Convert.ToInt64(dataRow["expenseMileageItemID"]);
                    Double carMeterStart = Convert.ToDouble(dataRow["CarMeterStart"]);
                    Double carMeterEnd = Convert.ToDouble(dataRow["CarMeterEnd"]);
                    DateTime travelDate = Convert.ToDateTime(dataRow["TravelDate"]);

                    if (expenseMileageItem.ExpenseMileageItemID != id)
                    {
                        if (expenseMileageItem.TravelDate > travelDate)
                        {
                            if (expenseMileageItem.CarMeterStart < carMeterEnd)
                            {
                                if (expenseMileageItem.CarMeterStart < carMeterStart)
                                {
                                    errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("TravelDateNotMatchMileageItem"));
                                }
                                else
                                {
                                    errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CarmeterIsDuplicate"));
                                }
                            }
                        }
                        else if (expenseMileageItem.TravelDate < travelDate)
                        {
                            if (expenseMileageItem.CarMeterEnd > carMeterStart)
                            {
                                if (expenseMileageItem.CarMeterStart > carMeterStart)
                                {
                                    errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("TravelDateIsDuplicate"));
                                }
                                else
                                {
                                    errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CarmeterIsDuplicate"));
                                }
                            }
                        }
                        else
                        {
                            if (expenseMileageItem.CarMeterStart >= carMeterStart && expenseMileageItem.CarMeterStart < carMeterEnd)
                            {
                                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CarmeterIsDuplicate"));
                            }
                            else if (expenseMileageItem.CarMeterEnd > carMeterStart && expenseMileageItem.CarMeterStart <= carMeterStart)
                            {
                                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("CarmeterIsDuplicate"));
                            }

                        }
                    }
                    if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                }
            }
        }

        public void ValdationMileageRateByDataset(Guid txId, long expDocumentID, bool isCopy)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = (ExpenseDataSet.FnExpenseMileageRow)ds.FnExpenseMileage.Select("ExpenseID = " + expDocumentID).FirstOrDefault();
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(expDocumentID);
            
            /*check owner employee*/
            
            if (mileageRow != null && mileageRow.Owner != "COM")
            {
                FnExpenseMileage mileage = new FnExpenseMileage();
                mileage.IsOverrideLevel = mileageRow.IsOverrideLevel;
                mileage.First100KmRate = Convert.ToDouble(mileageRow.First100KmRate);
                mileage.Exceed100KmRate = Convert.ToDouble(mileageRow.Exceed100KmRate);
                mileage.TypeOfCar = mileageRow.TypeOfCar;
                if (mileageRow.IsOverrideLevel == true)
                {
                    if (!mileageRow.IsOverrideCompanyIdNull())
                    {
                        mileage.OverrideCompanyId = mileageRow.OverrideCompanyId;
                    }
                    if (!String.IsNullOrEmpty(mileageRow.OverrideUserPersonalLevelCode))
                    {
                        mileage.OverrideUserPersonalLevelCode = mileageRow.OverrideUserPersonalLevelCode;
                    }
                }
                else
                {
                    if (!mileageRow.IsCurrentCompanyIdNull())
                    {
                        mileage.CurrentCompanyId = mileageRow.CurrentCompanyId;
                    } 
                    if (!mileageRow.IsCurrentUserPersonalLevelCodeNull())
                    {
                        mileage.CurrentUserPersonalLevelCode = mileageRow.CurrentUserPersonalLevelCode;
                    }
                }

                DataRow[] row = ds.FnExpenseMileageItem.Select("ExpenseMileageID = " + mileageRow.ExpenseMileageID);

                foreach (DataRow result in row)
                {
                    ValidateMileageRateData(mileage, Convert.ToDateTime(result["TravelDate"]));
                }
            }
        }

        public void ValdationMileageRateByDataBase(long expDocumentID)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            FnExpenseMileage mileage = SCG.eAccounting.Query.ScgeAccountingQueryProvider.FnExpenseMileageQuery.GetMileageByExpenseID(expDocumentID).FirstOrDefault();
            if (mileage != null && mileage.Owner != "COM")
            {
                IList<FnExpenseMileageItem> mileageItem = SCG.eAccounting.Query.ScgeAccountingQueryProvider.FnExpenseMileageItemQuery.GetMileageItemByMileageID(mileage.ExpenseMileageID);

                foreach (FnExpenseMileageItem result in mileageItem)
                {
                    ValidateMileageRateData(mileage, Convert.ToDateTime(result.TravelDate));
                }
            }
        }

        public void ValidateMileageRateData(FnExpenseMileage mileage, DateTime travelDate)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            long companyId;
            string personal;

            if (mileage == null) return;
            if (mileage.IsOverrideLevel == true)
            {
                if (mileage != null)
                {
                    companyId = Convert.ToInt64(mileage.OverrideCompanyId);
                    personal = mileage.OverrideUserPersonalLevelCode;
                }
                else
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("OverrideCompanyIdIsRequare"));
                    throw new ServiceValidationException(errors);
                }
            }
            else
            {
                companyId = Convert.ToInt64(mileage.CurrentCompanyId);
                personal = mileage.CurrentUserPersonalLevelCode;
            }

            Guid MileageProfileId = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.getMileageProfileByCompanyID(companyId);

            if (MileageProfileId == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                throw new ServiceValidationException(errors);
            }

            DbMileageRateRevisionDetail result = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindMileageRateRevision(MileageProfileId, personal, travelDate);
            if (result == null)
            {
                errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("CanNotFindMileageRate"));
                throw new ServiceValidationException(errors);
            }
            switch (mileage.TypeOfCar)
            {
                case TypeOfCar.PrivateCar:
                    if (result.CarRate != Convert.ToDouble(mileage.First100KmRate) || result.CarRate2 != Convert.ToDouble(mileage.Exceed100KmRate))
                    {
                        //errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid"));
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { travelDate.ToString() }));
                    }

                    break;
                case TypeOfCar.MotorCycle:
                    if (result.MotocycleRate != Convert.ToDouble(mileage.First100KmRate) || result.MotocycleRate2 != Convert.ToDouble(mileage.Exceed100KmRate))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { travelDate.ToString() }));
                    }
                    break;
                case TypeOfCar.Pickup:
                    if (result.PickUpRate != Convert.ToDouble(mileage.First100KmRate) || result.PickUpRate2 != Convert.ToDouble(mileage.Exceed100KmRate))
                    {
                        errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageRateInvalid", new object[] { travelDate.ToString() }));
                    }

                    break;
                default:
                    break;
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }


        public void SaveExpenseValidationMileageItemOnTransaction(Guid txId, long expDocumentID, string showError, bool isCopy)
        {
            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageRow mileageRow = (ExpenseDataSet.FnExpenseMileageRow)ds.FnExpenseMileage.Select("ExpenseID = " + expDocumentID).FirstOrDefault();
            ExpenseDataSet.FnExpenseDocumentRow expRow = ds.FnExpenseDocument.FindByExpenseID(expDocumentID);
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (isCopy)
                expDocumentID = -1;
           
            if (mileageRow == null) return;

            if (expRow.ExpenseType == ZoneType.Domestic)
            {
                DataRow[] row = ds.FnExpenseMileageItem.Select("ExpenseMileageID = " + mileageRow.ExpenseMileageID);
                foreach (DataRow dataRow in row)
                {
                    Double carMeterStart = Convert.ToDouble(dataRow["CarMeterStart"]);
                    Double carMeterEnd = Convert.ToDouble(dataRow["CarMeterEnd"]);
                    DateTime travelDate = Convert.ToDateTime(dataRow["TravelDate"]);

                    ValidateMilage mileageItemLeft = FnExpenseMileageItemQuery.GetMileageItemForValidationLeft(expRow.DocumentRow.RequesterID, mileageRow.CarLicenseNo, travelDate, expDocumentID);
                    ValidateMilage mileageItemRight = FnExpenseMileageItemQuery.GetMileageItemForValidationRight(expRow.DocumentRow.RequesterID, mileageRow.CarLicenseNo, travelDate, expDocumentID);
                    ValidateMilage checklength = FnExpenseMileageItemQuery.GetMileageItemForValidationCheckLength(expRow.DocumentRow.RequesterID, mileageRow.CarLicenseNo, travelDate, expDocumentID, carMeterStart);
                    if (mileageItemLeft != null)
                    {
                        if (string.IsNullOrEmpty(mileageItemLeft.DocumentNo))
                            mileageItemLeft.DocumentNo = "Draft";

                        if (mileageItemLeft.CarMeterEnd > carMeterStart)
                        {
                            if (mileageItemLeft.CarMeterStart > carMeterStart)
                            {
                                if (showError == "Mileage")
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentInvalidDateTimeline", new object[] { mileageItemLeft.DocumentNo }));
                                }
                                else
                                {
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentInvalidDateTimeline", new object[] { mileageItemLeft.DocumentNo }));
                                }
                            }
                            else
                            {
                                if (showError == "Mileage")
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { mileageItemLeft.DocumentNo }));
                                }
                                else
                                {
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { mileageItemLeft.DocumentNo }));
                                }
                            }
                        }
                    }
                    if (mileageItemRight != null)
                    {
                        if (string.IsNullOrEmpty(mileageItemRight.DocumentNo))
                            mileageItemRight.DocumentNo = "Draft";

                        if (mileageItemRight.CarMeterStart < carMeterEnd)
                        {
                            if (mileageItemRight.CarMeterStart < carMeterStart)
                            {
                                if (showError == "Mileage")
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageNotMatchAntherDocument", new object[] { mileageItemRight.DocumentNo }));
                                }
                                else
                                {
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentInvalidDateTimeline", new object[] { mileageItemRight.DocumentNo }));
                                }
                            }
                            else
                            {
                                if (showError == "Mileage")
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageNotMatchAntherDocument", new object[] { mileageItemRight.DocumentNo }));
                                }
                                else
                                {
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { mileageItemRight.DocumentNo }));
                                }
                            }
                        }
                    }
                    if (checklength != null)
                    {
                        if (string.IsNullOrEmpty(checklength.DocumentNo))
                            checklength.DocumentNo = "Draft";

                        if (showError == "Mileage")
                        {
                            errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { checklength.DocumentNo }));
                        }
                        else
                        {
                            errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { checklength.DocumentNo }));
                        }
                    }
                    else
                    {
                        ValidateMilage mileageItemEquals = FnExpenseMileageItemQuery.GetMileageItemForValidationEquals(expRow.DocumentRow.RequesterID, mileageRow.CarLicenseNo, travelDate, expDocumentID, carMeterStart);
                        if (mileageItemEquals != null)
                        {
                            if (string.IsNullOrEmpty(mileageItemEquals.DocumentNo))
                                mileageItemEquals.DocumentNo = "Draft";

                            if (mileageItemEquals.CarMeterStart < carMeterEnd)
                            {
                                if (showError == "Mileage")
                                {
                                    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { mileageItemEquals.DocumentNo }));
                                }
                                else
                                {
                                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("MileageAnotherDocumentDuplicateDateTimeline", new object[] { mileageItemEquals.DocumentNo }));
                                }
                            }
                        }
                    }
                    if (!errors.IsEmpty) throw new ServiceValidationException(errors);
                }
            }
        }

        public void UpdateExpenseMileageItemOnTransaction(FnExpenseMileageItem expenseMileageItem, Guid txId)
        {
            this.ValidateMileageItem(expenseMileageItem, txId);

            ExpenseDataSet ds = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageItemRow row = ds.FnExpenseMileageItem.FindByExpenseMileageItemID(expenseMileageItem.ExpenseMileageItemID);

            row.BeginEdit();

            row.TravelDate = expenseMileageItem.TravelDate.Value;
            row.LocationFrom = expenseMileageItem.LocationFrom;
            row.LocationTo = expenseMileageItem.LocationTo;
            row.CarMeterStart = (decimal)expenseMileageItem.CarMeterStart.Value;
            row.CarMeterEnd = (decimal)expenseMileageItem.CarMeterEnd.Value;
            row.DistanceTotal = (decimal)ComputeDistanceTotal(expenseMileageItem.CarMeterStart.Value, expenseMileageItem.CarMeterEnd.Value);

            if (expenseMileageItem.ExpenseMileage != null)
            {
                row.ExpenseMileageID = expenseMileageItem.ExpenseMileage.ExpenseMileageID;
                if (expenseMileageItem.ExpenseMileage.Owner.Equals(OwnerMileage.Employee))
                {
                    row.DistanceFirst100Km = (decimal)ComputeFirstDistance(expenseMileageItem.CarMeterStart.Value, expenseMileageItem.CarMeterEnd.Value, expenseMileageItem.ExpenseMileage.TypeOfCar);
                    row.DistanceExceed100Km = (decimal)ComputeExceedDistance(expenseMileageItem.CarMeterStart.Value, expenseMileageItem.CarMeterEnd.Value, expenseMileageItem.ExpenseMileage.TypeOfCar);
                }
                else
                {
                    row.DistanceAdjust = (decimal)expenseMileageItem.DistanceAdjust;
                    row.DistanceNet = (decimal)ComputeDistanceNet(row.DistanceTotal, (row.DistanceAdjust));
                }
            }

            row.Active = expenseMileageItem.Active;
            //row.CreBy = expenseMileageItem.CreBy;
            //row.CreDate = expenseMileageItem.CreDate;
            row.UpdBy = UserAccount.UserID;
            row.UpdDate = DateTime.Now;
            row.UpdPgm = UserAccount.CurrentProgramCode;
            row.EndEdit();

        }

        public void ValidateMileageItem(FnExpenseMileageItem expenseMileageItem, Guid txId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (expenseMileageItem.TravelDate == null)
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredTravelDate"));

            if (string.IsNullOrEmpty(expenseMileageItem.LocationFrom))
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredLocationFrom"));

            if (string.IsNullOrEmpty(expenseMileageItem.LocationTo))
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredLocationTo"));

            if (expenseMileageItem.CarMeterStart == null)
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredCarMeterStart"));

            if (expenseMileageItem.CarMeterEnd == null)
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredCarMeterEnd"));

            if (expenseMileageItem.CarMeterStart.Equals((decimal)0))
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredCarMeterStartOverZero"));

            if (expenseMileageItem.CarMeterEnd.Equals((decimal)0))
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredCarMeterEndOverZero"));

            if (expenseMileageItem.CarMeterStart >= expenseMileageItem.CarMeterEnd)
                errors.AddError("MileageItem.Error", new Spring.Validation.ErrorMessage("RequiredCarMeterStartOverCarMeterEnd"));


            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }

        public void DeleteExpenseMileageItemOnTransaction(Guid txId, long expenseMileageItemId)
        {
            ExpenseDataSet expDs = (ExpenseDataSet)TransactionService.GetDS(txId);
            ExpenseDataSet.FnExpenseMileageItemRow row = expDs.FnExpenseMileageItem.FindByExpenseMileageItemID(expenseMileageItemId);
            row.Delete();
        }
        public double ComputeDistanceTotal(double carMeterStart, double carMeterEnd)
        {
            return (carMeterEnd - carMeterStart);
        }
        public double ComputeFirstDistance(double carMeterStart, double carMeterEnd, string typeOfCar)
        {
            double distanceTotal = ComputeDistanceTotal(carMeterStart, carMeterEnd);
            //double first100Km = (double)100.0;
            double firstDistance = 0;
            if (typeOfCar.Equals(TypeOfCar.MotorCycle))
            {
                firstDistance = ParameterServices.MotorcycleFirstDistance;
            }
            else
            {
                firstDistance = ParameterServices.OtherFirstDistance;
            }

            if (distanceTotal < firstDistance)
                firstDistance = distanceTotal;

            return firstDistance;
        }
        public double ComputeExceedDistance(double carMeterStart, double carMeterEnd, string typeOfCar)
        {
            double distanceTotal = ComputeDistanceTotal(carMeterStart, carMeterEnd);
            double firstDistance = 0;
            if (typeOfCar.Equals(TypeOfCar.MotorCycle))
            {
                firstDistance = ParameterServices.MotorcycleFirstDistance;
            }
            else
            {
                firstDistance = ParameterServices.OtherFirstDistance;
            }
            double distanceExceed100Km = (double)0.0;

            if (distanceTotal > firstDistance)
                distanceExceed100Km = distanceTotal - firstDistance;

            return distanceExceed100Km;
        }
        public decimal ComputeDistanceNet(decimal distanceTotal, decimal distanceAdjust)
        {
            return distanceTotal - distanceAdjust;
        }

        public void PrepareDataToDataset(ExpenseDataSet ds, long mileageId)
        {
            IList<FnExpenseMileageItem> itemList = ScgeAccountingQueryProvider.FnExpenseMileageItemQuery.GetMileageItemByMileageID(mileageId);

            foreach (FnExpenseMileageItem item in itemList)
            {
                // Set data to invoice row in Dataset.
                ExpenseDataSet.FnExpenseMileageItemRow row = ds.FnExpenseMileageItem.NewFnExpenseMileageItemRow();

                row.ExpenseMileageItemID = item.ExpenseMileageItemID;

                if (item.ExpenseMileage != null)
                    row.ExpenseMileageID = mileageId;

                row.TravelDate = item.TravelDate.Value;
                row.LocationFrom = item.LocationFrom;
                row.LocationTo = item.LocationTo;
                row.CarMeterStart = (decimal)item.CarMeterStart.Value;
                row.CarMeterEnd = (decimal)item.CarMeterEnd.Value;
                row.DistanceTotal = (decimal)item.DistanceTotal;    //(decimal)ComputeDistanceTotal(item.CarMeterStart.Value, item.CarMeterEnd.Value);
                row.DistanceFirst100Km = (decimal)item.DistanceFirst100Km;  //(decimal)ComputeDistanceFirst100Km(item.CarMeterStart.Value, item.CarMeterEnd.Value);
                row.DistanceExceed100Km = (decimal)item.DistanceExceed100Km; //(decimal)ComputeDistanceExceed100Km(item.CarMeterStart.Value, item.CarMeterEnd.Value);
                row.DistanceAdjust = (decimal)item.DistanceAdjust;
                row.DistanceNet = (decimal)item.DistanceNet;

                row.Active = item.Active;
                row.CreBy = item.CreBy;
                row.CreDate = item.CreDate;
                row.UpdBy = item.UpdBy;
                row.UpdDate = item.UpdDate;
                row.UpdPgm = item.UpdPgm;

                // Add mileage item row to documentDataset.
                ds.FnExpenseMileageItem.AddFnExpenseMileageItemRow(row);
            }
        }

        public void UpdateMileageItemByMileageID(Guid txID, long mileageId, FnExpenseMileage mileage)
        {
            //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txID);

            string filter = String.Format("ExpenseMileageID = {0}", mileageId);
            DataRow[] rows = expDS.FnExpenseMileageItem.Select(filter);

            //if (rows.Length == 0)
            //{
            //    errors.AddError("Mileage.Error", new Spring.Validation.ErrorMessage("CannotSaveMileage"));
            //    throw new ServiceValidationException(errors);
            //}
            foreach (DataRow row in rows)
            {
                FnExpenseMileageItem item = new FnExpenseMileageItem();
                item.LoadFromDataRow(row);
                item.ExpenseMileage = mileage;
                this.UpdateExpenseMileageItemOnTransaction(item, txID);

            }
        }

        public void SaveExpenseMileageItem(Guid txId, long expenseId)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            ExpenseDataSet expDS = (ExpenseDataSet)TransactionService.GetDS(txId);

            ScgeAccountingDaoProvider.FnExpenseMileageItemDao.Persist(expDS.FnExpenseMileageItem);
        }
    }
}
