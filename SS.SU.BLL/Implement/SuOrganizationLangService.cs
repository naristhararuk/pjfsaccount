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
	public class SuOrganizationLangService : ServiceBase<SuOrganizationLang, long>, ISuOrganizationLangService
	{
		#region Override Method
		public override IDao<SuOrganizationLang, long> GetBaseDao()
		{
			return DaoProvider.SuOrganizationLangDao;
		}
		#endregion


		#region ISuOrganizationLangService Members
		public long InsertOrganizationLang(SuOrganizationLang organizationLang)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (string.IsNullOrEmpty(organizationLang.OrganizationName))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationNameRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Address))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationAddressRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Province))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationProvinceRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Country))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationCountryRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Postal))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationPostalNoRequired"));
			}
			else
			{
				try
				{
					int intPostalNo = int.Parse(organizationLang.Postal);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationPostalNoTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.Telephone))
			{
				try
				{
					long TelephoneNo = long.Parse(organizationLang.Organization.Telephone);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationTelephoneNoTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.TelephoneExt))
			{
				try
				{
					int TelephoneExt = int.Parse(organizationLang.Organization.TelephoneExt);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationTelephoneNoExtTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.Fax))
			{
				try
				{
					long FaxNo = long.Parse(organizationLang.Organization.Fax);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationFaxNoTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.FaxExt))
			{
				try
				{
					int TelephoneExt = int.Parse(organizationLang.Organization.FaxExt);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationFaxNoExtTypeMismatch"));
				}
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			return DaoProvider.SuOrganizationLangDao.Save(organizationLang);
		}
		public void UpdateOrganizationLang(SuOrganizationLang organizationLang)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (string.IsNullOrEmpty(organizationLang.OrganizationName))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationNameRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Address))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationAddressRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Province))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationProvinceRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Country))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationCountryRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.Postal))
			{
				errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationPostalNoRequired"));
			}
			else
			{
				try
				{
					int intPostalNo = int.Parse(organizationLang.Postal);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationPostalNoTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.Telephone))
			{
				try
				{
					long TelephoneNo = long.Parse(organizationLang.Organization.Telephone);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationTelephoneNoTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.TelephoneExt))
			{
				try
				{
					int TelephoneExt = int.Parse(organizationLang.Organization.TelephoneExt);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationTelephoneNoExtTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.Fax))
			{
				try
				{
					long FaxNo = long.Parse(organizationLang.Organization.Fax);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationFaxNoTypeMismatch"));
				}
			}
			if (!string.IsNullOrEmpty(organizationLang.Organization.FaxExt))
			{
				try
				{
					int TelephoneExt = int.Parse(organizationLang.Organization.FaxExt);
				}
				catch (Exception)
				{
					errors.AddError("OrganizationLang.Error", new Spring.Validation.ErrorMessage("OrganizationFaxNoExtTypeMismatch"));
				}
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			DaoProvider.SuOrganizationLangDao.SaveOrUpdate(organizationLang);
		}
		#endregion
	}
}
