﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;
using SS.Standard.Data.Query;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.Query
{
    public interface IRejectReasonLangQuery : IQuery<DbRejectReasonLang, long>
    {
        IList<VORejectReasonLang> FindAutoComplete(string reasonDetail , string documentType, short languageId);
        IList<VORejectReasonLang> FindReasonLangByReasonId(int reasonId);
    }
}
