﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;

namespace SS.SU.DAL.Hibernate
{
    public class SuUserLogDao : NHibernateDaoBase<SuUserLog, long>, ISuUserLogDao
    {
    }
}
