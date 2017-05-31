using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SCG.DB.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SS.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbServiceTeamQuery : IQuery <DbServiceTeam , long>
    {
        ISQLQuery FindServiceTeamByCriteria(DbServiceTeam serviceTeam, bool isCount, string sortExpression);
        IList<DbServiceTeam> GetServiceTeamList(DbServiceTeam serviceTeam, int firstResult, int maxResult, string sortExpression);
        int CountServiceTeamByCriteria(DbServiceTeam serviceTeam);
        IList<TranslatedListItem> GetAllServiceTeamListItem();
        IList<TranslatedListItem> GetServiceTeamListItemByUserID(long userID, IList<short> userRole);
        IList<TranslatedListItem> GetServiceTeamListItemByUserID(short RoleID);
        IList<TranslatedListItem> GetAllServiceTeamListItemByCompanyId(long? companyId);
    }
}
