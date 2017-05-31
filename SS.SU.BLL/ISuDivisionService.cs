using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuDivisionService : IService<SuDivision, short>   //, ISimpleMasterService
    {
        IList<SuDivision> FindDivisionByOrganization(SuOrganization org, int firstResult, int maxResults, string sortExpression);
        int CountByOrganizationCriteria(SuOrganization criteria);
        short AddDivision(SuDivision div, SuDivisionLang divLang);
        void UpdateDivision(SuDivision div);
    }
}