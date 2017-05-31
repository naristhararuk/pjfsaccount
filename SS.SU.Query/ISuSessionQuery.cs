using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Query;
namespace SS.SU.Query
{
     public interface ISuSessionQuery : IQuery<SuSession, long> 
    {
         IList<SuSession> GetUserSessionTimeOutList(DateTime TimeStamp);
         SuSession GetUserSession(long userID);

    }
}
