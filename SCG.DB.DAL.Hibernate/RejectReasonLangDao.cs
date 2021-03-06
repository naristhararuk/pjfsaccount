﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SCG.DB.DTO;
using SCG.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SCG.DB.DAL.Hibernate
{
    public partial class RejectReasonLangDao : NHibernateDaoBase<DbRejectReasonLang, long>, IRejectReasonLangDao
    {
        public RejectReasonLangDao()
        {

        }
        public void DeleteRejectReasonLangByReasonId(int reasonId)
        {
            GetCurrentSession().Delete(" from DbRejectReasonLang rl where rl.Reason.ReasonID = :ReasonID ",
                new object[] { reasonId }, new NHibernate.Type.IType[] { NHibernateUtil.Int32 });
        }
    }
}
