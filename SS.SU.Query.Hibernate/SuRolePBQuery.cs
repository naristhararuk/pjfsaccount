using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;
using SS.SU.DTO;
using SS.SU.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query.Hibernate
{
    class SuRolePBQuery : NHibernateQueryBase<SuRolepb,long>,ISuRolePBQuery
    {
        #region ISuRolePBQuery Members

        public ISQLQuery FindRolePBByCriteria(SuRole role, bool isCount, string sortExpression,short languageID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            if (!isCount)
            {
                //quering
                sqlBuilder.Append("select rpb.rolepbid as RolePbID,rpb.pbid as PBID,pb.pbcode as PBCode,pbl.description as PBName,c.companyname as Company ");
                sqlBuilder.Append("from dbpb pb,dbpblang pbl,dbcompany c,surolepb rpb ");
                sqlBuilder.Append("where 1=1 ");
                sqlBuilder.Append("and c.companycode = pb.companycode ");
                sqlBuilder.Append("and rpb.pbid = pb.pbid ");
                sqlBuilder.Append("and pbl.pbid = pb.pbid ");
                sqlBuilder.Append("and pbl.languageID = :languageID ");
                sqlBuilder.Append("and rpb.roleid= :roleID ");
                sqlBuilder.Append("and rpb.active=1 ");
 

                if (string.IsNullOrEmpty(sortExpression))
                {
                    sqlBuilder.AppendLine("ORDER BY PBID,PBCode,PBName,Company");
                }
                else
                {
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));

                }
            }
            else
            {
                //counting
                sqlBuilder.Append("select count(1) as RolePBCount ");
                sqlBuilder.Append("from dbpb pb,dbpblang pbl,dbcompany c,surolepb rpb ");
                sqlBuilder.Append("where 1=1 ");
                sqlBuilder.Append("and c.companycode = pb.companycode ");
                sqlBuilder.Append("and rpb.pbid = pb.pbid ");
                sqlBuilder.Append("and pbl.pbid = pb.pbid ");
                sqlBuilder.Append("and pbl.languageID = :languageID  ");
                sqlBuilder.Append("and rpb.roleid= :roleID  ");


            }
            parameterBuilder.AddParameterData("languageID", typeof(string), languageID);
            parameterBuilder.AddParameterData("roleID", typeof(short), role.RoleID);
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);
            if (!isCount)
            {
                query.AddScalar("RolePbID", NHibernateUtil.Int64);
                query.AddScalar("PBID", NHibernateUtil.Int64);
                query.AddScalar("PBCode", NHibernateUtil.String);
                query.AddScalar("PBName", NHibernateUtil.String);
                query.AddScalar("Company", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(PBInformation)));
            }
            else
            {
                query.AddScalar("RolePBCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<PBInformation> FindPBInfoByRole(SuRole role, int firstResult, int maxResult, string sortExpression, short languageID)
        {
          
            return FindRolePBByCriteria(role,false,sortExpression,languageID).List<PBInformation>();
       
        }

        public int FindCount(SuRole role,short languageID)
        {
            return NHibernateQueryHelper.CountByCriteria(QueryProvider.SuRolePBQuery, "FindRolePBByCriteria", new object[] { role, true, string.Empty, languageID });
        }

        public IList<SuRole> FindRoleByPBID(long pbID)
        {
            IList<SuRole> role = new List<SuRole>();

            IList<SuRolepb> rolePBs = GetCurrentSession().CreateQuery("from SuRolepb where PBID = :PBID and active = '1'")
                .SetInt64("PBID", pbID)
                .List<SuRolepb>();

            foreach (SuRolepb rolePB in rolePBs)
            {
                role.Add(rolePB.RoleID);
            }

            return role;
        }

        #endregion
    }
}
