using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISimpleMasterService
    {
        void Add(ISimpleMaster obj);
        void Updated(ISimpleMaster obj);
        void DeleteMasterRecord(string id);
    }
}
