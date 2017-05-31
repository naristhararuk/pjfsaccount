using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;

using SCG.FN.DAL;
using SCG.FN.DTO;
using SCG.FN.BLL;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL.Implement
{
    public partial class FnReceiptTypeService : ServiceBase<FnReceiptType,short>,IFnReceiptTypeService
    {
        public override IDao<FnReceiptType, short> GetBaseDao()
        {
            return DaoProvider.FnReceiptTypeDao;
        }
        public IList<FnReceiptTypeResult> FindByFnReceiptTypeCriteria(string accNo, short languageId, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<FnReceiptTypeResult>(DaoProvider.FnReceiptTypeDao, "FindGlAccountByCriteria", new object[] { accNo, languageId }, firstResult, maxResults, sortExpression);
        }
        public int CountByFnReceiptTypeCriteria(string accNo, short languageId)
        {
            return NHibernateDaoHelper.CountByCriteria(DaoProvider.FnReceiptTypeDao, "FindGlAccountByCriteria", new object[] { accNo, languageId });
        }
        public void UpdateReceiptTypeLang(IList<FnReceiptTypeLang> receiptTypeLangList)
        {
            if (receiptTypeLangList.Count > 0)
            {
                DaoProvider.FnReceiptTypeLangDao.DeleteAllReceiptTypeLang(receiptTypeLangList[0].ReceiptType.ReceiptTypeID);
            }
            foreach (FnReceiptTypeLang receiptTypeLang in receiptTypeLangList)
            {
                DaoProvider.FnReceiptTypeLangDao.Save(receiptTypeLang);
            }
        }

        public void AddReceiptType(FnReceiptType fnReceiptType)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(fnReceiptType.ReceiptTypeCode))
            {
                errors.AddError("FnReceiptType.Error", new Spring.Validation.ErrorMessage("FnReceiptType_CodeRequired"));
            }
            if (DaoProvider.FnReceiptTypeDao.IsDuplicateReceiptTypeCode(fnReceiptType))
            {
                errors.AddError("FnReceiptType.Error", new Spring.Validation.ErrorMessage("UniqueFnReceiptTypeCode"));
            }
            if (fnReceiptType.Account == null)
            {
                errors.AddError("FnReceiptType.Error", new Spring.Validation.ErrorMessage("Account_Required"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            DaoProvider.FnReceiptTypeDao.Save(fnReceiptType);
        }
        public void UpdateReceiptType(FnReceiptType fnReceiptType)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(fnReceiptType.ReceiptTypeCode))
            {
                errors.AddError("FnReceiptType.Error", new Spring.Validation.ErrorMessage("FnReceiptType_CodeRequired"));
            }
            if (DaoProvider.FnReceiptTypeDao.IsDuplicateReceiptTypeCode(fnReceiptType))
            {
                errors.AddError("FnReceiptType.Error", new Spring.Validation.ErrorMessage("UniqueFnReceiptTypeCode"));
            }
            if (fnReceiptType.Account == null)
            {
                errors.AddError("FnReceiptType.Error", new Spring.Validation.ErrorMessage("Account_Required"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            DaoProvider.FnReceiptTypeDao.SaveOrUpdate(fnReceiptType);

        }
    }
}
