﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.DTO;
using SS.Standard.Data.NHibernate.Dao;
using SS.DB.DTO.ValueObject;
using NHibernate;

namespace SS.DB.DAL
{
    public interface IDbParameterDao : IDao<DbParameter, short>
    {
        IList<Parameter> FindByDbParameterCriteria(DbParameter parameter);
        void DeleteAll(short Id);
    }
}