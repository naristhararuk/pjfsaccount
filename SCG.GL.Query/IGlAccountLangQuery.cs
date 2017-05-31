using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.GL.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SCG.GL.Query
{
    public interface IGlAccountLangQuery : IQuery<GlAccountLang, long>
    {
        //new IList<GlAccount> FindAll();
        //new GlAccount FindByIdentity(short id);

        //List<GlAccountLang> FindDataByAccIDLangID(string strAccNo);
    }
}
