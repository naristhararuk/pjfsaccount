﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface IDbReasonService : IService<DbScgReason, short>
    {
        void AddReason(DbScgReason reason);
        void UpdateReason(DbScgReason reason);
    }
}
