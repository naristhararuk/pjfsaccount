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
	public class SuOrganizationService : ServiceBase<SuOrganization, short>, ISuOrganizationService
	{
		#region Override Method
		public override IDao<SuOrganization, short> GetBaseDao()
		{
			return DaoProvider.SuOrganizationDao;
		}
		#endregion

		#region ISuOrganizationService Members
		public void AddOrganization(SuOrganization organization, SuOrganizationLang organizationLang)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (organizationLang.Language == null)
			{
				errors.AddError("Organization.Error", new Spring.Validation.ErrorMessage("OrganizationLangRequired"));
			}
			if (string.IsNullOrEmpty(organizationLang.OrganizationName))
			{
				errors.AddError("Organization.Error", new Spring.Validation.ErrorMessage("OrganizationNameRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			DaoProvider.SuOrganizationDao.Save(organization);
			DaoProvider.SuOrganizationLangDao.Save(organizationLang);
		}
		public void UpdateOrganization(SuOrganization organization)
		{
			DaoProvider.SuOrganizationDao.SaveOrUpdate(organization);
		}
		#endregion
	}
}
