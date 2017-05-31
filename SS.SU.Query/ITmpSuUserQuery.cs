using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;


namespace SS.SU.Query
{
    public interface ITmpSuUserQuery : IQuery<TmpSuUser, long>
    {

    }
}
