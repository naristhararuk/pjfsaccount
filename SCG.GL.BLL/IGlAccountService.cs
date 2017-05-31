using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.GL.DTO;
//using SCG.GL.DTO.ValueObject;

namespace SCG.GL.BLL
{
    public interface IGlAccountService : IService<GlAccount, short>
    {
        //new IList<GlAccount> FindAll();
        //new void Delete(SuRole domain);
        //new SuRole FindByIdentity(short id);
        //new short Save(SuRole domain);
        //new void SaveOrUpdate(SuRole domain);
        //new void Update(SuRole domain);
        void AddAccount(GlAccount account, GlAccountLang accountLang);
        void UpdateAccount(GlAccount account);
        GlAccount FindAccountByAccountNo(string accNo);
    }
}
