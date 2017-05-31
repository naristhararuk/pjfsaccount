using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO;

namespace SS.SU.Query
{
    public interface ISuEmailResendingQuery : IQuery<SuEmailResending,long>
    {
        IList<SuEmailResending> FindAllEmailResending();
        void DeleteSuccessItem();
    }
}
