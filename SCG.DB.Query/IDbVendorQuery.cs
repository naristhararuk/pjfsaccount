using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IDbVendorQuery : IQuery<DbVendor, long>
    {
        ISQLQuery FindByVendorCriteria(string taxNo, string vendorCode, string vendorName, bool isCount, string sortExpression);
        IList<DbVendor> GetVendorList(string taxNo, string vendorCode, string vendorName, int firstResult, int maxResult, string sortExpression);
        int CountByCountryCriteria(string taxNo, string vendorCode, string vendorName);

        IList<DbVendor> FindByDbVendor(long vendorID);
        IList<DbVendor> FindByDbVendorAutoComplete(string taxNo);

        bool isDuplicationVendorCode(string VendorCode);
        ISQLQuery FindVendorByCriteria(DbVendor Vendor, bool isCount, short languageId, string sortExpression, string VendorCode, string VendorName);
        IList<DbVendor> GetVendorList(DbVendor Vendor, short languageId, int firstResult, int maxResult, string sortExpression, string VendorCode, string VendorName);
        int CountVendorByCriteria(DbVendor Vendor, string VendorCode, string VendorName);

        IList<VOVendor> FindVendorByVendorTaxNO(string taxNo, string branch);

        #region For Vendor Lookup & AutoComplete (New 05/04/2009)
        ISQLQuery FindVendorByVendorCriteria(VOVendor criteria, bool isCount, string sortExpession);
        IList<VOVendor> GetVendorListByVendorCriteria(VOVendor criteria, int firstResult, int maxResult, string sortExpression);
        int CountVendorByVendorCriteria(VOVendor criteria);
        IList<VOVendor> FindByVendorAutoComplete(string taxNo);
        #endregion
    }
}
