using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuUserQuery : IQuery<SuUser, long>
    {
        new IList<SuUser> FindAll();
        new SuUser FindByIdentity(long id);
        new SuUser FindProxyByIdentity(long id);
        string FindUserNameByMobilePhoneNo(string MobilePhoneNo);
        IList<SuUserSearchResult> GetTranslatedList(short languageID);
        SuUser GetSuUserByUserName_Password(string UserName, string Password);
        SuUser GetSuUserByUserName(string UserName);
        bool IsLocked(string UserName);
        IList<UserSession> GetUserSessionList(long userID, short languageID);
        IList GetUserList(short languageID);

        IList<SuUserSearchResult> GetSuUserSearchResultList(short languageID, int firstResult, int maxResult, string sortExpression);
        int GetCountSuUserSearchResult(short languageID);
        ISQLQuery FindSuUserSearchResult(short languageID, string sortExpression, bool isCount);
        ISQLQuery FindSuUserSearchResultByCriteria(UserCriteria criteria, short languageId, string sortExpression, bool isCount);
        ISQLQuery FindInitialtorLookupResultByCriteria(UserCriteria criteria, short languageId, string sortExpression, bool isCount);

        IList<SuUserSearchResult> GetUserSearchResultByCriteria(UserCriteria criteria, short languageId, int firstResult, int maxResult, string sortExpression);
        IList<SuUserSearchResult> GetInitialtorLookupSearchResultByCriteria(UserCriteria criteria, short languageId, int firstResult, int maxResult, string sortExpression);

        int GetCountUserSearchResultByCriteria(UserCriteria criteria, short languageId);
        int GetCountInitialtorLookupResultByCriteria(UserCriteria criteria, short languageId);
        SuUserSearchResult FindUserByUserIdLanguageId(long userId, short divId, short orgId, short languageId);
        SuUserSearchResult FindUserByUserIdLanguageId(long userId);

        IList<SuUser> FindAutoComplete(string prefixText, UserAutoCompleteParameter param);
        int CountByUserCriteria(UserCriteria criteria);
        IList<SuUser> GetUserProfileList(UserCriteria criteria, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindByUserCriteria(UserCriteria criteria, bool isCount, string sortExpression);
        SuUser FindUserByID(long userID);
        ISQLQuery FindUserProfileByCriteria(VOUserProfile criteria, bool isCount, string sortExpression);
        IList<VOUserProfile> GetUserProfileListByCriteria(VOUserProfile criteria, int firstResult, int maxResult, string sortExpression);
        int CountUserProfileByCriteria(VOUserProfile criteria);

        IList<SuUser> FindByUser(long userID, string UserName);
        IList<SuUser> FindReminderExpireAccount(DateTime checkPointDate);

        SuUser FindUserByUserName(string username, bool isApprovalFlag); // find user by username and active = true

        SuUser FindSuUserByUserName(string UserName); // for use in method forget password
    }
}
