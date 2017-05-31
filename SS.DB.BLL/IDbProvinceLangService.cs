﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.DB.DTO;

namespace SS.DB.BLL
{
    public interface IDbProvinceLangService : IService<DbProvinceLang, long>
    {
        void UpdateProvinceLang(IList<DbProvinceLang> accountLangList);
    }
}
