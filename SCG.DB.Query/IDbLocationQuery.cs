using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;
using NHibernate;
using SCG.DB.DTO.ValueObject;
using SS.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbLocationQuery : IQuery<DbLocation, long>
    {
        ISQLQuery FindByLocationCriteria(bool isCount,string sortExpression, string companyID, string locationCode, string locationName, short languageId);
        int CountByLocationCriteria(string companyID, string locationCode, string locationName, short languageId);
        IList<Location> GetLocationList(int firstResult, int maxResult, string sortExpression, string companyID, string locationCode, string locationName, short languageId);
        IList<TranslatedListItem> FindCompanyCriteria();
        IList<TranslatedListItem> FindCompanyCriteriaShowIDJoinName();
        IList<Location> FindByLocationIdentity(long locationID);

        IList<PaymentTypeListItem> FindByLocationCompany(long companyID, string companyCode, long userID, long locationID);

        IList<Location> FindLocationByCriteria(Location location, short languageID);
        IList<LocationLangResult> FindLocationLangByLocationID(long locationId);
        IList<Location> FindAutoComplete(string prefixText, Location param);
        IList<DbLocation> FindByLocationNameID(long locationID, string locationName);

        DbLocation FindDbLocationByLocationCode(string locationCode);
        
        int CountByCriteriaLocation(string companyID, string locationCode, string locationName);
        IList<Location> GetListLocation(int firstResult, int maxResult, string sortExpression, string companyID, string locationCode, string locationName);
        ISQLQuery FindByCriteriaLocation(bool isCount, string sortExpression, string companyID, string locationCode, string locationName);
    }
}
