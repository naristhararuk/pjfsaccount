using System;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO;
using System.Collections.Generic;
using SCG.DB.DTO.ValueObject;


namespace SCG.DB.Query
{
    

    public interface IDbAccountCompanyQuery : IQuery<DbAccountCompany, long>
    {
       IList<VOAccountCompany> FindAccountCompanyByAccountID(long accountID);
       IList<DbAccountCompany> FindAccountByCompanyID(long companyId);       
       IList<VOAccountCompany> FindAccountCompany(long companyId, long accountId);
    }
}
