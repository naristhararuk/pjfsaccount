using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuProgramService : IService<SuProgram, short>
    {
        new IList<SuProgram> FindAll();
        new void Delete(SuProgram domain);
        new SuProgram FindByIdentity(short id);
        new short Save(SuProgram domain);
        new void SaveOrUpdate(SuProgram domain);
        new void Update(SuProgram domain);
        short AddProgram(SuProgram program);
        void UpdateProgram(SuProgram program);
    }
}
