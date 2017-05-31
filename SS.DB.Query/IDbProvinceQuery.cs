using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using NHibernate;

namespace SS.DB.Query
{
    public interface IDbProvinceQuery : IQuery<DbProvince, short>
    {
        ISQLQuery FindByProvinceCriteria(ProvinceLang province, bool isCount, short languageId, string sortExpression);
        int CountByProvinceCriteria(ProvinceLang province);
        IList<ProvinceLang> GetProvinceList(ProvinceLang province, short languageId, int firstResult, int maxResult, string sortExpression);

        ISQLQuery FindByProvinceLovCriteria(ProvinceLang province, bool isCount, short languageId, string sortExpression,string ProvinceID,string ProvinceName,string RegionID);
        int CountByProvinceLovCriteria(ProvinceLang province, string ProvinceID, string ProvinceName, string RegionID);
        IList<ProvinceLang> GetProvinceLovList(ProvinceLang province, short languageId, int firstResult, int maxResult, string sortExpression, string ProvinceID, string ProvinceName, string RegionID);

        IList<DBProvinceLovReturn> FindByProvinceAutoComplete(string ProvincePrefix, string languageId, string RegionID);
    }
}
