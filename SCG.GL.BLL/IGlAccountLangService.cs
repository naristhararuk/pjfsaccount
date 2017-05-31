using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.GL.DTO;
//using SCG.GL.DTO.ValueObject;

namespace SCG.GL.BLL
{
    public interface IGlAccountLangService : IService<GlAccountLang, long>
    {
        //new IList<GlAccountLang> FindAll();
        //new void Delete(SuRole domain);
        //new SuRole FindByIdentity(short id);
        //new short Save(SuRole domain);
        //new void SaveOrUpdate(SuRole domain);
        //new void Update(SuRole domain);
        //void AddRole(SuRole role, SuRoleLang roleLang);
        //void UpdateRole(SuRole role);

        IList<SCG.GL.DTO.ValueObject.AccountLang> FindByAccountId(short accountId);
        void UpdateAccountLang(IList<GlAccountLang> accountLangList);
        GlAccountLang FindAccountLangByAccountID(short accID, short languageId);
    }
}
