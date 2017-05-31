using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service.Implement;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DAL;
using SS.Standard.Utilities;

namespace SCG.DB.BLL.Implement
{
    public class DbSapInstanceService : ServiceBase<DbSapInstance, string>, IDbSapInstanceService
    {

        public override IDao<DbSapInstance, string> GetBaseDao()
        {
            return ScgDbDaoProvider.DbSapInstanceDao;
        }
        public void AddSapInstance(DbSapInstance sapinstance)
        {
            CheckDataValueUpdate(sapinstance,true);
            ScgDbDaoProvider.DbSapInstanceDao.Save(sapinstance);
        }

        public void UpdateSapInstance(DbSapInstance sapinstance)
        {
            CheckDataValueUpdate(sapinstance,false);
            ScgDbDaoProvider.DbSapInstanceDao.Update(sapinstance);
        }
        private void CheckDataValueUpdate(DbSapInstance sapinstance,bool newmode)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            
            if (string.IsNullOrEmpty(sapinstance.Code))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Code Required"));
            }
            if (ScgDbDaoProvider.DbSapInstanceDao.IsDuplicateCode(sapinstance) && newmode)
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Code is Duplicate"));
            }
            if (string.IsNullOrEmpty(sapinstance.AliasName))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("AliasName Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.SystemID))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("SystemID Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.Client))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Client Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.UserName))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("UserName Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.Password))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Password Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.Language))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Language Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.SystemNumber))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("System Number Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.MsgServerHost))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Msg Server Host Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.LogonGroup))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Logon Group Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.UserCPIC))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("UserCPIC Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeExpPostingDM))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Expense Posting(Domestic) Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeExpRmtPostingDM))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Expense Remittance Posting(Domestic) Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeExpPostingFR))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Expense Posting(Foreign) Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeExpRmtPostingFR))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Expense Remittance Posting(Foreign) Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeExpICPostingFR))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Expense IC Posting Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeAdvancePostingDM))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Advance (DM) Posting Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeAdvancePostingFR))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Advace (FR) Posting Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeRmtPosting))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("Remittance Posting Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeFixedAdvancePosting))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("FixedAdvance Posting Required"));
            }
            if (string.IsNullOrEmpty(sapinstance.DocTypeFixedAdvanceReturnPosting))
            {
                errors.AddError("SAPInstance.Error", new Spring.Validation.ErrorMessage("FixedAdvance Return Posting Required"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

        }
    }
}
