using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using NHibernate;

namespace SS.SU.DAL
{
    public interface ISuDivisionDao : IDao<SuDivision, short>
    {
        //IList<SuDivision> FindByOrganization(SuOrganization org);
        ISQLQuery FindByOrganizationCriteria(SuOrganization org, bool isCounto);
    }
}
