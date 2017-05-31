using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.GL.DTO;
//using SCG.GL.DTO.ValueObject;

namespace SCG.GL.DAL
{
    public interface IGlAccountDao : IDao<GlAccount, short>
    {
        GlAccount FindAccountByAccountNo(string accNo);

    }
}
