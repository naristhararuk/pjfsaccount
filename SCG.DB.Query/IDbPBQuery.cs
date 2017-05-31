using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SCG.DB.DTO;
using SS.Standard.Data.Query.NHibernate;
using SCG.DB.DTO.ValueObject;
using NHibernate;


namespace SCG.DB.Query
{
    public interface IDbPBQuery : IQuery<Dbpb, long>
    {
        IList<PaymentTypeListItem> GetPbListItem(long companyID, short languageID);
        VOPB GetDescription(long pbID, int languageID);
        ISQLQuery FindPbByCriteria(VOPB pb, string sortExpression, bool isCount);
        IList<VOPB> GetPbList(VOPB pb, int firstResult, int maxResult, string sortExpression);
        int CountPbByCriteria(VOPB pb);
        bool IsRepOffice(long UserID);       
    }
}
