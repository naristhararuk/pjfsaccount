using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuUserPersonalLevelService : IService<SuUserPersonalLevel, string>
    {
        void AddSuUserPersonalLevel(SuUserPersonalLevel suUserPersonalLevel);
        void UpdateSuUserPersonalLevel(SuUserPersonalLevel suUserPersonalLevel);
    }
}
