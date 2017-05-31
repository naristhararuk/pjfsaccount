using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.GL.DTO;
//using SCG.GL.DTO.ValueObject;

namespace SCG.GL.DAL
{
    public interface IGlAccountLangDao : IDao<GlAccountLang, long>
    {
        IList<SCG.GL.DTO.ValueObject.AccountLang> FindByAccountId(short accountId);
        void DeleteAllAccountLang(short accountId);
        GlAccountLang FindAccountLangByAccountID(short accID, short languageID);
    }
}
