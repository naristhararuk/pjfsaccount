using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbCompanyTaxDao : NHibernateDaoBase<DbCompanyTax, long>,IDbCompanyTaxDao
    {
        public bool IsDuplicateCompanyCode(DbCompanyTax ct)
        {
            IList<DbCompanyTax> list = GetCurrentSession().CreateQuery("from DbCompanyTax where TaxID = :TaxID and CompanyID = :CompanyID")
                  .SetInt64("TaxID", ct.TaxID)
                  .SetInt64("CompanyID", ct.CompanyID)
                  .List<DbCompanyTax>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
