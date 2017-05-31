using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.Security;
using SCG.eAccounting.DAL;
using System.Data;
using SS.Standard.Utilities;
using SCG.eAccounting.Query;

namespace SCG.eAccounting.BLL.Implement
{
    class MPAItemService : ServiceBase<MPAItem, long>, IMPAItemService
    {
        public ITransactionService TransactionService { get; set; }
        public IUserAccount UserAccount { get; set; }

        public void ChangeRequesterMAPItem(Guid txID, MPAItem mpaItem)
        {
            MPADocumentDataSet mpaItemDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.MPAItemRow mpaItemRow = mpaItemDS.MPAItem.FindByMPAItemID(mpaItem.MPAItemID);

            mpaItemRow.BeginEdit();

            //mpaItemRow.MPAItemID = mpaItem.MPAItemID;
            mpaItemRow.UserID = mpaItem.UserID.Userid;
            mpaItemRow.ActualAmount = mpaItem.ActualAmount;
            mpaItemRow.ActualAmountNotExceed = mpaItem.ActualAmountNotExceed;
            mpaItemRow.AmountCompanyPackage = mpaItem.AmountCompanyPackage;
            mpaItemRow.TotalAmount = mpaItem.TotalAmount;
            mpaItemRow.MobilePhoneNo = mpaItem.MobilePhoneNo;
            mpaItemRow.MobileBrand = mpaItem.MobileBrand;
            mpaItemRow.MobileModel = mpaItem.MobileModel;
            mpaItemRow.UpdBy = UserAccount.UserID;
            mpaItemRow.UpdDate = DateTime.Now;
            mpaItemRow.UpdPgm = UserAccount.CurrentProgramCode;

            mpaItemRow.EndEdit();
        }

        public long AddMPAItemTransaction(Guid txID, MPAItem mpaItem)
        {
            MPADocumentDataSet mpaItemDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.MPAItemRow mpaItemRow = mpaItemDS.MPAItem.NewMPAItemRow();

            //mpaItemRow.MPAItemID = mpaItem.MPAItemID;
            mpaItemRow.MPADocumentID = mpaItem.MPADocumentID.MPADocumentID;
            mpaItemRow.UserID = mpaItem.UserID.Userid;
            mpaItemRow.UpdBy = UserAccount.UserID;
            mpaItemRow.UpdDate = DateTime.Now;
            mpaItemRow.UpdPgm = UserAccount.CurrentProgramCode;

            mpaItemDS.MPAItem.AddMPAItemRow(mpaItemRow);
            return mpaItemRow.MPAItemID;
        }

        public DataTable DeleteMPAItemTransaction(Guid txID, long MPAItemID)
        {
            MPADocumentDataSet MPAItemDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.MPAItemRow mpaItemRow = MPAItemDS.MPAItem.FindByMPAItemID(MPAItemID);
            mpaItemRow.Delete();

            return MPAItemDS.MPAItem;
        }

        public override SS.Standard.Data.NHibernate.Dao.IDao<MPAItem, long> GetBaseDao()
        {
            return ScgeAccountingDaoProvider.MPAItemDao;
        }

        public void UpdateMPAItemTransaction(Guid txID, MPAItem mpaItem)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            MPADocumentDataSet MPAItemDS = (MPADocumentDataSet)TransactionService.GetDS(txID);
            MPADocumentDataSet.MPAItemRow mpaItemRow = MPAItemDS.MPAItem.FindByMPAItemID(mpaItem.MPAItemID);


            mpaItemRow.BeginEdit();


            mpaItemRow.UserID = mpaItem.UserID.Userid;
            if (mpaItem.ActualAmount != null)
            {
                mpaItemRow.ActualAmount = mpaItem.ActualAmount;
            }
            if (mpaItem.ActualAmountNotExceed != null)
            {
                mpaItemRow.ActualAmountNotExceed = mpaItem.ActualAmountNotExceed;
            }
            if (mpaItem.AmountCompanyPackage != null)
            {
                mpaItemRow.AmountCompanyPackage = mpaItem.AmountCompanyPackage;
            }
            if (mpaItem.TotalAmount != null)
            {
                mpaItemRow.TotalAmount = mpaItem.TotalAmount;
            }
            mpaItemRow.MobilePhoneNo = mpaItem.MobilePhoneNo;
            mpaItemRow.MobileBrand = mpaItem.MobileBrand;
            mpaItemRow.MobileModel = mpaItem.MobileModel;
            mpaItemRow.CreBy = UserAccount.UserID;
            mpaItemRow.CreDate = DateTime.Now;
            mpaItemRow.UpdBy = UserAccount.UserID;
            mpaItemRow.UpdDate = DateTime.Now;
            mpaItemRow.UpdPgm = UserAccount.CurrentProgramCode;

            mpaItemRow.EndEdit();
        }
        public void SaveMPAItem(Guid txID, long documentID)
        {
            MPADocumentDataSet mpaItemDS = (MPADocumentDataSet)TransactionService.GetDS(txID);

            // Insert, Delete TADocumentTraveller.
            ScgeAccountingDaoProvider.MPAItemDao.Persist(mpaItemDS.MPAItem);
        }

        public void PrepareDataToDataset(MPADocumentDataSet MPAItemDS, long mpaDocumentID)
        {
            IList<MPAItem> mpaItemList = ScgeAccountingQueryProvider.MPAItemQuery.FindMPAItemByMPADocumentID(mpaDocumentID);
            //MPADocumentDataSet.MPAItemRow mpaItemRow = MPAItemDS.MPAItem.NewMPAItemRow(); ;

            foreach (MPAItem mpaItem in mpaItemList)
            {
                MPADocumentDataSet.MPAItemRow mpaItemDataRow = MPAItemDS.MPAItem.NewMPAItemRow();

                mpaItemDataRow.MPAItemID = mpaItem.MPAItemID;
                mpaItemDataRow.MPADocumentID = mpaItem.MPADocumentID.MPADocumentID;
                mpaItemDataRow.UserID = mpaItem.UserID.Userid;
                if (mpaItem.ActualAmount != null)
                {
                    mpaItemDataRow.ActualAmount = mpaItem.ActualAmount;
                }
                if (mpaItem.ActualAmountNotExceed != null)
                {
                    mpaItemDataRow.ActualAmountNotExceed = mpaItem.ActualAmountNotExceed;
                }
                if (mpaItem.AmountCompanyPackage != null)
                {
                    mpaItemDataRow.AmountCompanyPackage = mpaItem.AmountCompanyPackage;
                }
                if (mpaItem.TotalAmount != null)
                {
                    mpaItemDataRow.TotalAmount = mpaItem.TotalAmount;
                }
                mpaItemDataRow.MobilePhoneNo = mpaItem.MobilePhoneNo;
                mpaItemDataRow.MobileBrand = mpaItem.MobileBrand;
                mpaItemDataRow.MobileModel = mpaItem.MobileModel;
                mpaItemDataRow.CreBy = UserAccount.UserID;
                mpaItemDataRow.CreDate = DateTime.Now;
                mpaItemDataRow.UpdBy = UserAccount.UserID;
                mpaItemDataRow.UpdDate = DateTime.Now;
                mpaItemDataRow.UpdPgm = UserAccount.CurrentProgramCode;

                // Add document initiator to datatable taDocumentDS.
                MPAItemDS.MPAItem.AddMPAItemRow(mpaItemDataRow);
            }
        }
    }
}
