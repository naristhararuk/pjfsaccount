using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using NHibernate.Transform;

namespace SCG.DB.Query.Hibernate
{
    public class DbDictionaryQuery : NHibernateQueryBase<DbDictionary,int>,IDbDictionaryQuery
    {
        #region IDbDictionaryQuery Members

        public DbDictionary GetVerb3ByVerb1(string verb1)
        {
            string sql = "select top 1 verb3,verb3Thai from dbdictionary where verb1='{0}'";
            sql = string.Format(sql, verb1);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            try
            {
                query.AddScalar("Verb3", NHibernateUtil.String);
                query.AddScalar("Verb3Thai", NHibernateUtil.String);
                DbDictionary result = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbDictionary))).UniqueResult<DbDictionary>();

                return result;
            }
            catch (Exception)
            {

                return new DbDictionary(verb1);
            }

       
        }

        #endregion
    }
}
