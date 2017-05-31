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

namespace SS.SU.BLL.Implement
{
	public class SuUserLangService : ServiceBase<SuUserLang, long>, ISuUserLangService
	{
		#region Override Method
		public override IDao<SuUserLang, long> GetBaseDao()
		{
			return DaoProvider.SuUserLangDao;
		}
		#endregion
	
		#region ISuUserLangService Members
		public IList<SuUserLang> FindByUserIdAndLanguageId(long userID, short languageID)
		{
			return DaoProvider.SuUserLangDao.FindByUserIdAndLanguageId(userID, languageID);
		}
		public long AddNewUserLang(SuUserLang userLang)
		{
			#region Validate SuUserLang
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			// Validate userLang.Prefix.
			if (string.IsNullOrEmpty(userLang.Prefix))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("PrefixRequired"));
			}
			// Validate userLang.FirstName.
			if (string.IsNullOrEmpty(userLang.FirstName))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("FirstNameRequired"));
			}
			// Validate userLang.LastName.
			if (string.IsNullOrEmpty(userLang.LastName))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("LastNameRequired"));
			}
			
			// Validate userLang.Address.
			if (string.IsNullOrEmpty(userLang.Address))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("AddressRequired"));
			}

			// Validate userLang.Province.
			if (string.IsNullOrEmpty(userLang.Province))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("ProvinceRequired"));
			}

			// Validate userLang.Postal.
			if (string.IsNullOrEmpty(userLang.Postal))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("PostalNoRequired"));
			}
			else
			{
				try
				{
					int postalNo = int.Parse(userLang.Postal);
				}
				catch (Exception)
				{
					errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("PostalNoMustBeNumber"));
				}
			}

			// Validate userLang.Country.
			if (string.IsNullOrEmpty(userLang.Country))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("CountryRequired"));
			}

			// Validate userLang.Language.
			if (userLang.Language == null)
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("LanguageIsNull"));
			}
			// Validate userLang.User.
			if (userLang.User == null)
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("UserIsNull"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			return DaoProvider.SuUserLangDao.Save(userLang);
		}
		public void UpdateUserLang(SuUserLang userLang)
		{
			#region Validate SuUserLang
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			// Validate userLang.Prefix.
			if (string.IsNullOrEmpty(userLang.Prefix))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("PrefixRequired"));
			}
			// Validate userLang.FirstName.
			if (string.IsNullOrEmpty(userLang.FirstName))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("FirstNameRequired"));
			}
			// Validate userLang.LastName.
			if (string.IsNullOrEmpty(userLang.LastName))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("LastNameRequired"));
			}
			
			// Validate userLang.Address.
			if (string.IsNullOrEmpty(userLang.Address))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("AddressRequired"));
			}
			
			// Validate userLang.Province.
			if (string.IsNullOrEmpty(userLang.Province))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("ProvinceRequired"));
			}
			
			// Validate userLang.Postal.
			if (string.IsNullOrEmpty(userLang.Postal))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("PostalNoRequired"));
			}
			else
			{
				try
				{
					int postalNo = int.Parse(userLang.Postal);
				}
				catch (Exception)
				{
					errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("PostalNoMustBeNumber"));
				}
			}
			
			// Validate userLang.Country.
			if (string.IsNullOrEmpty(userLang.Country))
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("CountryRequired"));
			}

			// Validate userLang.Language.
			if (userLang.Language == null)
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("LanguageIsNull"));
			}
			// Validate userLang.User.
			if (userLang.User == null)
			{
				errors.AddError("UserLang.Error", new Spring.Validation.ErrorMessage("UserIsNull"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			DaoProvider.SuUserLangDao.SaveOrUpdate(userLang);
		}
		#endregion
	}
}
