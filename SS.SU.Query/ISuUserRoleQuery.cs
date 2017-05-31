using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.Query.NHibernate;
using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuUserRoleQuery : IQuery<SuUserRole, long>
    {
        new IList<SuUserRole> FindAll();

        IList<SuUserRole> FindUserRoleByUserId(long userId);
        IList<UserRole> FindUserRoleByUserId(long userId, short languageId);
		
		IList<SuUserRoleSearchResult> GetSuUserRoleSearchResultList(long userID, short languageID, int firstResult, int maxResult, string sortExpression);
		ISQLQuery FindSuUserRoleSearchResult(long userID, short languageID, string sortExpression, bool isCount);
		int GetCountSuUserRoleSearchResult(long userID, short languageID);

        IList<SuUserRole> FindGroupByUserId(long userID);
        SuUserRole FindUserRoleByUserRoleId(long id);


        IList<SuRole> GetRoleReceiveDocument();
        IList<SuRole> GetRoleVerifyDocument();
        IList<SuRole> GetRoleApproveVerifyDocument();
        IList<SuRole> GetRoleVerifyAndApproveVerifyDocument();
        
        IList<SuRole> GetRoleVerifyPayment();
        IList<SuRole> GetRoleApproveVerifyPayment();
        IList<SuRole> GetRoleVerifyAndApproveVerifyPayment();
        IList<SuRole> GetRoleCounterCashier();

        IList<short> GetUserRoleServiceTeamByUserID(long userID);
        IList<short> GetUserRolePBByUserID(long userID);
    }
}
