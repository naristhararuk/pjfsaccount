using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
    public interface ISuGlobalTranslateLangQuery : IQuery<SuGlobalTranslateLang, long>
    {
        IList<SuGlobalTranslateLang> FindByTranslateId(long translateId);
        IList<GlobalTranslateLang> FindTranslateLangByTranslateId(long translateId);
    }
}
