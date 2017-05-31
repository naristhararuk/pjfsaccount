using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.DAL
{
    public interface IFnAutoPaymentTempDao : IDao<FnAutoPaymentTemp,long>
    {

        bool CommitToAutoPayment(long DocumentID);
        void DeleteAll();
        bool IsSuccess(long documentID);
    }
}
