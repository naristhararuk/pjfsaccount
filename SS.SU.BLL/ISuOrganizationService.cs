using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
	public interface ISuOrganizationService : IService<SuOrganization, short>
	{
		void AddOrganization(SuOrganization organization, SuOrganizationLang organizationLang);
		void UpdateOrganization(SuOrganization organization);
	}
}
