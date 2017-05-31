﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using NHibernate;
using System.Data;
using SCG.DB.DTO;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbProfileListDao : NHibernateDaoBase<DbProfileList, Guid>, IDbProfileListDao
    {
        public DbProfileListDao()
        {
        }
    }
}
