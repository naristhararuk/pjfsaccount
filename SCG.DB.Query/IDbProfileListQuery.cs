using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO;

namespace SCG.DB.Query
{
    public interface IDbProfileListQuery : IQuery<DbProfileList, Guid>
    {
        ISQLQuery FindByProfileListCriteria( bool isCount);
        //IList<IDbProfileList> GetProfileList(IDbProfileList profileList, int firstResult, int maxResult, string sortExpression);
        IList<DbProfileList> GetProfileList(int firstResult, int maxResult, string sortExpression);
        Guid? GetProfileListIdByName(string Name);
        int CountByProfileCriteria();
        IList<DbCompany> FindProfileToUse(Guid Id);
    }
}
