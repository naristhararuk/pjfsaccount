using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Dao;

using SS.DB.DTO;
using SS.DB.DAL;
using NHibernate;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;


namespace SS.DB.DAL.Hibernate
{
    public partial class DbProvinceLangDao : NHibernateDaoBase<DbProvinceLang, long>, IDbProvinceLangDao
    {
        #region public void DeleteAllProvinceLang(short provinceId)
        public void DeleteAllProvinceLang(short provinceId)
        {
            GetCurrentSession()
            .Delete(" FROM DbProvinceLang a WHERE a.Province.Provinceid = :ProvinceID ",
            new object[] { provinceId },
            new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
        #endregion public void DeleteAllProvinceLang(short provinceId)
    }
}
