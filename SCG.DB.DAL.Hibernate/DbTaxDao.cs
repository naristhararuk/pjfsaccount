using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbTaxDao : NHibernateDaoBase<DbTax , long>, IDbTaxDao
    {
        public DbTaxDao()
        { 
        }
         public long FindTaxId(string taxCode)
        {
            IList<DbTax> list = GetCurrentSession().CreateQuery("from DbTax p where p.TaxCode = :TaxCode")
                  .SetString("TaxCode", taxCode)
                  .List<DbTax>();
            long taxId = -1;
            if (list.Count > 0)
            {
                taxId = list.ElementAt<DbTax>(0).TaxID;
            }
            return taxId;
        }
         public bool IsDuplicateTaxCode(DbTax tax)
         {
             IList<DbTax> list = GetCurrentSession().CreateQuery("from DbTax p where p.TaxID <> :TaxID and p.TaxCode = :TaxCode")
                   .SetInt64("TaxID", tax.TaxID)
                   .SetString("TaxCode", tax.TaxCode)
                   .List<DbTax>();
             if (list.Count > 0)
             {
                 return true;
             }
             return false;
         }
    }
}
