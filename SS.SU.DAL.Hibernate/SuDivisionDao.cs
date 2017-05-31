using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DAL;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.QueryCreator;

namespace SS.SU.DAL.Hibernate
{
    public partial class SuDivisionDao : NHibernateDaoBase<SuDivision, short>, ISuDivisionDao
    {
        //public IList<SuDivision> FindByOrganization(SuOrganization org)
        //{
        //    return GetCurrentSession().CreateQuery("from SuDivision div where div.Organization.OrganizationID = :OrganizationID")
        //        .SetInt16("OrganizationID", org.Organizationid)
        //        .List<SuDivision>();
        //}
        public ISQLQuery FindByOrganizationCriteria(SuOrganization org, bool isCount)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append("select div.DivisionId as Divisionid, div.Comment as Comment from SuDivision div where div.OrganizationID = :OrganizationId");
            }
            else
            {
                sqlBuilder.Append("select count(div.DivisionId) as DivisionCount from SuDivision div where div.OrganizationID = :OrganizationId");
            }

            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("OrganizationId", typeof(short), org.Organizationid);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("Divisionid", NHibernateUtil.Int16)
                    .AddScalar("Comment", NHibernateUtil.String);
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SS.SU.DTO.SuDivision)));
            }
            else
            {
                query.AddScalar("DivisionCount", NHibernateUtil.Int32);
            }

            return query;
        }
    }
}
