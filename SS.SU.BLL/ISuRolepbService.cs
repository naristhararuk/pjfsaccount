using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuRolepbService : IService<SuRolepb,long>
    {
         void AddRolepb(SuRolepb suRolepb);
         void DeleteRolepb(SuRolepb suRolepb);
        
    }
}
