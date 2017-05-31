using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.GL.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.GL.DTO.ValueObject;

namespace SCG.GL.Query
{
    public interface IGlAccountQuery : IQuery<GlAccount, short>
    {
        //new IList<GlAccount> FindAll();
        //new GlAccount FindByIdentity(short id);

        //List<GlAccount> FindAllData();
        //List<GlAccount> FindDataBy(string strAccNo);

        ISQLQuery FindByAccountCriteria(AccountLang program, bool isCount, short languageId, string sortExpression);
        IList<AccountLang> GetAccoutnList(AccountLang account, short languageId, int firstResult, int maxResult, string sortExpression);
        int CountByAccountCriteria(AccountLang account);
    }
}
