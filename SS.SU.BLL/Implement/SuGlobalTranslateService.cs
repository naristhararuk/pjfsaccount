using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.Query;

namespace SS.SU.BLL.Implement
{
    public partial class SuGlobalTranslateService : ServiceBase<SuGlobalTranslate, long>, ISuGlobalTranslateService
    {
        public override IDao<SuGlobalTranslate, long> GetBaseDao()
        {
            return DaoProvider.SuGlobalTranslateDao;
        }
        public long AddGlobalTranslate(SuGlobalTranslate globalTranslate)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(globalTranslate.TranslateSymbol) && string.IsNullOrEmpty(globalTranslate.TranslateControl))
            {
                errors.AddError("GlobalTranslate.Error", new Spring.Validation.ErrorMessage("RequiredSymbolOrControl"));
            }

            //Check ProgramCode and symbol is unique.
            if (DaoProvider.SuGlobalTranslateDao.IsDuplicateProgramCodeSymbol(globalTranslate))
            {
                errors.AddError("GlobalTranslate.Error", new Spring.Validation.ErrorMessage("UniqueProgramCodeSymbol"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            SuGlobalTranslate translate = new SuGlobalTranslate();
            translate.ProgramCode = globalTranslate.ProgramCode;
            translate.TranslateControl = globalTranslate.TranslateControl;
            translate.TranslateSymbol = globalTranslate.TranslateSymbol;
            translate.Comment = globalTranslate.Comment;
            translate.Active = globalTranslate.Active;
            translate.CreBy = globalTranslate.CreBy;
            translate.CreDate = DateTime.Now.Date;
            translate.UpdBy = globalTranslate.UpdBy;
            translate.UpdDate = DateTime.Now.Date;
            translate.UpdPgm = globalTranslate.UpdPgm;

            return DaoProvider.SuGlobalTranslateDao.Save(translate);
        }
        public void UpdateGlobalTranslate(SuGlobalTranslate globalTranslate)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(globalTranslate.TranslateSymbol) && string.IsNullOrEmpty(globalTranslate.TranslateControl))
            {
                errors.AddError("GlobalTranslate.Error", new Spring.Validation.ErrorMessage("RequiredSymbolOrControl"));
            }

            //Check ProgramCode and symbol is unique.
            if (DaoProvider.SuGlobalTranslateDao.IsDuplicateProgramCodeSymbol(globalTranslate))
            {
                errors.AddError("GlobalTranslate.Error", new Spring.Validation.ErrorMessage("UniqueProgramCodeSymbol"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            SuGlobalTranslate translate = DaoProvider.SuGlobalTranslateDao.FindByIdentity(globalTranslate.TranslateId);
            translate.ProgramCode = globalTranslate.ProgramCode;
            translate.TranslateControl = globalTranslate.TranslateControl;
            translate.TranslateSymbol = globalTranslate.TranslateSymbol;
            translate.Comment = globalTranslate.Comment;
            translate.Active = globalTranslate.Active;
            translate.UpdBy = globalTranslate.UpdBy;
            translate.UpdDate = DateTime.Now.Date;
            translate.UpdPgm = globalTranslate.UpdPgm;

            DaoProvider.SuGlobalTranslateDao.SaveOrUpdate(translate);
        }
        public IList<SuGlobalTranslate> FindBySuGolbalTranslateCriteria(SuGlobalTranslate criteria, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<SuGlobalTranslate>(DaoProvider.SuGlobalTranslateDao, "FindBySuGolbalTranslateCriteria", new object[] { criteria }, firstResult, maxResults, sortExpression);
        }
        public int CountBySuGolbalTranslateCriteria(SuGlobalTranslate criteria)
        {
            return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuGlobalTranslateDao, "FindBySuGolbalTranslateCriteria", new object[] { criteria });
        }

		#region Add Program Translate Control and Symbol (use in BasePage)
		public long AddProgramTranslateControl_Symbol(SuGlobalTranslate globalTranslate)
		{
			bool hasError = false;
			long returnValue = -1;

			if (string.IsNullOrEmpty(globalTranslate.TranslateSymbol) && string.IsNullOrEmpty(globalTranslate.TranslateControl))
				hasError = true;

			//Check ProgramCode and symbol is unique.
			if (DaoProvider.SuGlobalTranslateDao.IsDuplicateProgramCodeSymbol(globalTranslate))
				hasError = true;

			if (!hasError)
			{
				SuGlobalTranslate translate = new SuGlobalTranslate();
				translate.ProgramCode = globalTranslate.ProgramCode;
				translate.TranslateControl = globalTranslate.TranslateControl;
				translate.TranslateSymbol = globalTranslate.TranslateSymbol;
				translate.Comment = globalTranslate.Comment;
				translate.Active = globalTranslate.Active;
				translate.CreBy = globalTranslate.CreBy;
				translate.CreDate = DateTime.Now;
				translate.UpdBy = globalTranslate.UpdBy;
				translate.UpdDate = DateTime.Now;
				translate.UpdPgm = globalTranslate.UpdPgm;
				
				returnValue = DaoProvider.SuGlobalTranslateDao.Save(translate);
			}
			
			return returnValue;
		}
		#endregion

		#region DeleteTranslateControl
		public void DeleteByProgramCodeAndControl(string programCode, string translateControl)
		{
			DaoProvider.SuGlobalTranslateDao.DeleteByProgramCodeAndControl(programCode, translateControl);
		}
		#endregion
	}
}
