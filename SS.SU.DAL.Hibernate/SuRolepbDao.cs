using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.DAL.Hibernate
{
    public class SuRolepbDao : NHibernateDaoBase<SuRolepb,long>,ISuRolepbDao
    {

        #region ISuRolepbDao Members
        /// <summary>
        /// check for RolePb that Is it was added already ?
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="PbID"></param>
        /// <returns></returns>
        public bool IsExist(short RoleID, long PbID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT COUNT(1) AS RolePbCount ");
            sqlBuilder.Append("FROM SuRolePb ");
            sqlBuilder.Append("WHERE RoleID = :RoleID ");
            sqlBuilder.Append("AND PbID = :PbID ");
             QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
             parameterBuilder.AddParameterData("RoleID", typeof(Int16), RoleID);
             parameterBuilder.AddParameterData("PbID", typeof(Int32), PbID);

            
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            query.AddScalar("RolePbCount", NHibernateUtil.Int32);
            int count = (int) query.UniqueResult();

            if (count > 0)
                return true;
            else
                 return false;



                


        }

        #endregion
    }
}
