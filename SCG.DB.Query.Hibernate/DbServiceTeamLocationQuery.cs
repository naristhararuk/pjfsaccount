using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;
using NHibernate.Expression;

namespace SCG.DB.Query.Hibernate
{
    public class DbServiceTeamLocationQuery : NHibernateQueryBase<DbServiceTeamLocation, long>, IDbServiceTeamLocationQuery
    {
        public ISQLQuery FindServiceTeamLocationByServiceTeamID(DbServiceTeam serviceTeam, bool isCount, string sortExpression)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     sl.ServiceTeamLocationID as ServiceTeamLocationID,");
                sqlBuilder.Append("     sl.ServiceTeamID as ServiceTeamID,");

                sqlBuilder.Append("     sl.LocationID as LocationID,");
                sqlBuilder.Append("     l.LocationCode as LocationCode,");

                sqlBuilder.Append("     c.CompanyCode as CompanyCode,");
                sqlBuilder.Append("     c.CompanyName as CompanyName,");

                sqlBuilder.Append("     l.Description as Description,");
                
                sqlBuilder.Append("     sl.Active as Active");
                
                sqlBuilder.Append("");
                sqlBuilder.Append(" FROM DbServiceTeamLocation sl LEFT OUTER JOIN DbLocation l ON  sl.LocationID = l.LocationID ");
                sqlBuilder.Append(" LEFT OUTER JOIN DbCompany c ON  l.CompanyID = c.CompanyID ");
                sqlBuilder.Append(" WHERE sl.ServiceTeamID = :ServiceTeamID ");              
           
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY c.CompanyName, l.Description, sl.Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(sl.ServiceTeamLocationID) AS ServiceTeamLocationCount ");
                sqlBuilder.Append(" FROM DbServiceTeamLocation sl LEFT OUTER JOIN DbLocation l ON  sl.LocationID = l.LocationID ");
                sqlBuilder.Append(" LEFT OUTER JOIN DbCompany c ON  l.CompanyID = c.CompanyID ");
                sqlBuilder.Append(" WHERE sl.ServiceTeamID = :ServiceTeamID ");              
            }
           
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("ServiceTeamID", typeof(long), serviceTeam.ServiceTeamID);   
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("ServiceTeamLocationID", NHibernateUtil.Int64);
                query.AddScalar("ServiceTeamID", NHibernateUtil.Int64);
                query.AddScalar("LocationID", NHibernateUtil.Int64);
                query.AddScalar("LocationCode", NHibernateUtil.String);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("CompanyName", NHibernateUtil.String);
                query.AddScalar("Description", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(ServiceTeamLocationResult)));
            }
            else 
            {
                query.AddScalar("ServiceTeamLocationCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            
            return query;
        }

        public int CountServiceTeamLocationByServiceTeamID(DbServiceTeam serviceTeam)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbServiceTeamLocationQuery, "FindServiceTeamLocationByServiceTeamID", new object[] { serviceTeam, true, string.Empty });
        }

        public IList<ServiceTeamLocationResult> GetServiceTeamLocationList(DbServiceTeam serviceTeam, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindByCriteria<ServiceTeamLocationResult>(ScgDbQueryProvider.DbServiceTeamLocationQuery, "FindServiceTeamLocationByServiceTeamID", new object[] { serviceTeam, false, sortExpression });
        }

        public IList<DbServiceTeamLocation> FindServiceTeamByLocationID(long locationId)
        {
            return GetCurrentSession().CreateQuery(" from DbServiceTeamLocation where LocationID = :LocationID")
                .SetInt64("LocationID", locationId)
                .List<DbServiceTeamLocation>();
        }
    }
}
