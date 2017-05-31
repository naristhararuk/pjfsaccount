using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.QueryDao;
using SCG.DB.DTO;
using NHibernate;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate.Transform;
using SCG.DB.DTO.ValueObject;
using SS.DB.DTO.ValueObject;
using NHibernate.Expression;

namespace SCG.DB.Query.Hibernate
{
    public class DbLocationQuery : NHibernateQueryBase<DbLocation, long>, IDbLocationQuery
    {
        #region public ISQLQuery FindByLocationCriteria(bool isCount, string sortExpression, string companyID, string locationCode, string locationName, short languageId)
        public ISQLQuery FindByLocationCriteria(bool isCount, string sortExpression, string companyID, string locationCode, string locationName, short languageId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" SELECT ");
                sqlBuilder.Append("     DbCompany.CompanyID       AS CompanyID ,");
                sqlBuilder.Append("     DbCompany.CompanyCode     AS CompanyCode, ");
                sqlBuilder.Append("     DbCompany.CompanyName     AS CompanyName, ");
                sqlBuilder.Append("     DbLocation.LocationID     AS LocationID ,");
                sqlBuilder.Append("     DbLocation.LocationCode   AS LocationCode ,");
                sqlBuilder.Append("     DbLocation.Description   AS LocationName ,");
                sqlBuilder.Append("     DbCompany.Active          AS Active ");
                sqlBuilder.Append(" FROM DbCompany ");
                sqlBuilder.Append("     INNER JOIN DbLocation ON ");
                sqlBuilder.Append("         DbCompany.CompanyID = DbLocation.CompanyID AND ");
                sqlBuilder.Append("         DbCompany.Active   = 1 ");
                //sqlBuilder.Append("     INNER JOIN DbLocationLang ON ");
                //sqlBuilder.Append("         DbLocationLang.LocationID = DbLocation.LocationID AND ");
                //sqlBuilder.Append("         DbLocationLang.LanguageID = :LanguageId ");
                sqlBuilder.Append(" WHERE 1=1 ");

                //parameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);

                if (companyID != null && companyID != "")
                {
                    sqlBuilder.Append(" AND DbCompany.CompanyID = :CompanyID ");
                    parameterBuilder.AddParameterData("CompanyID", typeof(short), short.Parse(companyID));
                }
                if (locationCode != null && locationCode != "")
                {
                    sqlBuilder.Append(" AND DbLocation.LocationCode LIKE :LocationCode ");
                    parameterBuilder.AddParameterData("LocationCode", typeof(string), "%" + locationCode + "%");
                }
                //if (locationName != null && locationName != "")
                //{
                //    sqlBuilder.Append(" AND DbLocationLang.LocationName = :LocationName ");
                //    parameterBuilder.AddParameterData("LocationName", typeof(string), "%" + locationName + "%");
                //}

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY DbCompany.CompanyID,DbLocation.LocationCode"); //,DbLocationLang.LocationName ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(*) AS LocationCount ");
                sqlBuilder.Append(" FROM DbCompany ");
                sqlBuilder.Append("     INNER JOIN DbLocation ON ");
                sqlBuilder.Append("         DbCompany.CompanyID = DbLocation.CompanyID AND ");
                sqlBuilder.Append("         DbCompany.CompanyCode = DbLocation.CompanyCode AND ");
                sqlBuilder.Append("         DbCompany.Active   = 1 ");
                //sqlBuilder.Append("     INNER JOIN DbLocationLang ON ");
                //sqlBuilder.Append("         DbLocationLang.LocationID = DbLocation.LocationID AND ");
                //sqlBuilder.Append("         DbLocationLang.LanguageID = :LanguageId ");
                sqlBuilder.Append(" WHERE 1=1 ");

                //parameterBuilder.AddParameterData("LanguageId", typeof(Int16), languageId);

                if (companyID != null && companyID != "")
                {
                    sqlBuilder.Append(" AND DbCompany.CompanyID = :CompanyID ");
                    parameterBuilder.AddParameterData("CompanyID", typeof(short), short.Parse(companyID));
                }
                if (locationCode != null && locationCode != "")
                {
                    sqlBuilder.Append(" AND DbLocation.LocationCode LIKE :LocationCode ");
                    parameterBuilder.AddParameterData("LocationCode", typeof(string), "%" + locationCode + "%");
                }
                //if (locationName != null && locationName != "")
                //{
                //    sqlBuilder.Append(" AND DbLocationLang.Description = :LocationName ");
                //    parameterBuilder.AddParameterData("LocationName", typeof(string), "%" + locationName + "%");
                //}
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("CompanyID", NHibernateUtil.Int64);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("CompanyName", NHibernateUtil.String);
                query.AddScalar("LocationID", NHibernateUtil.Int64);
                query.AddScalar("LocationCode", NHibernateUtil.String);
                query.AddScalar("LocationName", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(Location)));
            }
            else
            {
                query.AddScalar("LocationCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByLocationCriteria(bool isCount, string sortExpression, string companyID, string locationCode, string locationName, short languageId)

        #region public IList<Location> GetLocationList(int firstResult, int maxResult, string sortExpression,string companyID, string locationCode, string locationName, short languageId)
        public IList<Location> GetLocationList(int firstResult, int maxResult, string sortExpression, string companyID, string locationCode, string locationName, short languageId)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<Location>(ScgDbQueryProvider.DbLocationQuery, "FindByLocationCriteria", new object[] { false, sortExpression, companyID, locationCode, locationName, languageId }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<Location> GetLocationList(int firstResult, int maxResult, string sortExpression,string companyID, string locationCode, string locationName, short languageId)

        #region public int CountByLocationCriteria(string companyID, string locationCode, string locationName, short languageId)
        public int CountByLocationCriteria(string companyID, string locationCode, string locationName, short languageId)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbLocationQuery, "FindByLocationCriteria", new object[] { true, string.Empty, companyID, locationCode, locationName, languageId });
        }
        #endregion public int CountByLocationCriteria(string companyID, string locationCode, string locationName, short languageId)

        #region public IList<TranslatedListItem>FindCompanyCriteria()
        public IList<TranslatedListItem> FindCompanyCriteria()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     CompanyID   AS ID, ");
            sqlBuilder.Append("     CompanyCode + '-' + CompanyName AS Symbol ");
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append("     DbCompany ");
            sqlBuilder.Append(" WHERE Active = 1 ORDER BY CompanyCode ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }
        #endregion public IList<TranslatedListItem>FindCompanyCriteria()

        #region public IList<TranslatedListItem>FindCompanyCriteria()
        public IList<TranslatedListItem> FindCompanyCriteriaShowIDJoinName()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     CompanyID   AS ID, ");
            sqlBuilder.Append("     '[ ' + CONVERT(VARCHAR,CompanyCode) + ' ]  ' + CompanyName  AS Symbol ");
            sqlBuilder.Append(" FROM ");
            sqlBuilder.Append("     DbCompany ");
            sqlBuilder.Append(" WHERE Active = 1 ORDER BY CompanyCode");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("ID", NHibernateUtil.Int16);
            query.AddScalar("Symbol", NHibernateUtil.String);
            query.SetResultTransformer(Transformers.AliasToBean(typeof(TranslatedListItem)));

            return query.List<TranslatedListItem>();
        }
        #endregion public IList<TranslatedListItem>FindCompanyCriteria()

        #region public IList<Location> FindByLocationIdentity(long locationId)
        public IList<Location> FindByLocationIdentity(long locationId)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append(" SELECT ");
            sqlBuilder.Append("     DbCompany.CompanyID       AS CompanyID ,");
            sqlBuilder.Append("     DbCompany.CompanyCode     AS CompanyCode, ");
            sqlBuilder.Append("     DbCompany.CompanyName     AS CompanyName, ");
            sqlBuilder.Append("     DbLocation.LocationID     AS LocationID ,");
            sqlBuilder.Append("     DbLocation.LocationCode   AS LocationCode ,");
            //sqlBuilder.Append("     DbLocationLang.Description   AS LocationName ,");
            sqlBuilder.Append("     DbCompany.Active          AS Active ");
            sqlBuilder.Append(" FROM DbCompany ");
            sqlBuilder.Append("     INNER JOIN DbLocation ON ");
            sqlBuilder.Append("         DbCompany.CompanyID = DbLocation.CompanyID AND ");
            sqlBuilder.Append("         DbCompany.CompanyCode = DbLocation.CompanyCode AND ");
            sqlBuilder.Append("         DbCompany.Active   = 1 ");
            //sqlBuilder.Append("     INNER JOIN DbLocationLang ON ");
            //sqlBuilder.Append("         DbLocationLang.LocationID = DbLocation.LocationID ");
            sqlBuilder.Append(" WHERE DbLocation.LocationID = :LocationID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.AddParameterData("LocationID", typeof(Int64), locationId);
            parameterBuilder.FillParameters(query);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("CompanyCode", NHibernateUtil.String);
            query.AddScalar("CompanyName", NHibernateUtil.String);
            query.AddScalar("LocationID", NHibernateUtil.Int64);
            query.AddScalar("LocationCode", NHibernateUtil.String);
            //query.AddScalar("LocationName", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            IList<Location> list =
               query.SetResultTransformer(Transformers.AliasToBean(typeof(Location))).List<Location>();
            return list;
        }
        #endregion public IList<Location> FindByLocationIdentity(long locationId)

        #region public IList<PaymentTypeListItem> FindByLocationCompany(long companyID, string companyCode, long locationID)
        /// <summary>
        /// for bind ddl (service Team)
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public IList<PaymentTypeListItem> FindByLocationCompany(long companyID, string companyCode, long userID, long locationID)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            string qs = @"
                select distinct t4.ServiceTeamCode as Code, t4.Description as Text, t4.ServiceTeamID as ID
                from 
                     dbcompany t1 
		                inner join dblocation 	t2 on t1.companyid = t2.companyid
		                inner join dbserviceteamlocation t3 on t2.locationid = t3.locationid
		                inner join dbserviceteam t4 on t3.serviceteamid = t4.serviceteamid
                where 
	                t1.active = 1 and t2.active = 1 and t3.active = 1 and t4.active = 1 and 
                    t1.companyid = :companyID
                    ";
            parameterBuilder.AddParameterData("companyID", typeof(Int64), companyID);

            if(!String.IsNullOrEmpty(companyCode)) 
            {
                qs += " and t1.companycode = :companyCode";
                parameterBuilder.AddParameterData("companyCode", typeof(String), companyCode);
            }
            //if (locationID > 0)
            //{
            //    qs += " and t2.locationId = :locationID";
            //    parameterBuilder.AddParameterData("locationID", typeof(long), locationID);
            //}

            //qs += " group by t4.ServiceTeamCode, t4.Description, t4.ServiceTeamID ";

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(qs);
            parameterBuilder.FillParameters(query);
            query.AddScalar("Code", NHibernateUtil.String);
            query.AddScalar("Text", NHibernateUtil.String);
            query.AddScalar("ID", NHibernateUtil.Int64);

            IList<PaymentTypeListItem> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(PaymentTypeListItem))).List<PaymentTypeListItem>();
            return list;
        }

        #endregion public IList<PaymentTypeListItem> FindByLocationCompany(long companyID, string companyCode)

        public IList<Location> FindLocationByCriteria(Location location, short languageID)
        {

            #region with locationlang
            //StringBuilder sqlBuilder = new StringBuilder();

            //sqlBuilder.Append("SELECT l.LocationID as LocationID,l.LocationCode as LocationCode, ll.LocationName as LocationName, l.Active as Active ");
            //sqlBuilder.Append("FROM DbLocation as l ");
            //sqlBuilder.Append("LEFT JOIN DbLocationLang as ll on l.LocationID = ll.LocationID ");
            //sqlBuilder.Append("WHERE l.CompanyID =:CompanyID AND l.LocationCode Like :LocationCode AND ll.LocationName Like :LocationName AND ll.LanguageID =:LanguageID ");
            //sqlBuilder.AppendLine("ORDER BY l.LocationCode,ll.LocationName ");

            //ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            //QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            //queryParameterBuilder.AddParameterData("CompanyID", typeof(long), location.CompanyID);
            //queryParameterBuilder.AddParameterData("LanguageID", typeof(short), languageID);
            //queryParameterBuilder.AddParameterData("LocationCode", typeof(string), string.Format("%{0}%", location.LocationCode));
            //queryParameterBuilder.AddParameterData("LocationName", typeof(string), string.Format("%{0}%", location.LocationName));
            //queryParameterBuilder.FillParameters(query);

            //query.AddScalar("LocationID", NHibernateUtil.Int64);
            //query.AddScalar("LocationCode", NHibernateUtil.String);
            //query.AddScalar("LocationName", NHibernateUtil.String);
            //query.AddScalar("Active", NHibernateUtil.Boolean);

            //return query.SetResultTransformer(Transformers.AliasToBean(typeof(Location))).List<Location>();
            #endregion
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT l.LocationID as LocationID,l.LocationCode as LocationCode, l.Description as LocationName, l.Active as Active ");
            sqlBuilder.Append("FROM DbLocation as l ");
            sqlBuilder.Append("WHERE l.CompanyID =:CompanyID AND isnull(l.LocationCode,'') Like :LocationCode AND isnull(l.Description,'') Like :LocationName ");
            sqlBuilder.AppendLine("ORDER BY l.LocationCode,l.Description ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("CompanyID", typeof(long), location.CompanyID);
            queryParameterBuilder.AddParameterData("LocationCode", typeof(string), string.Format("%{0}%", location.LocationCode));
            queryParameterBuilder.AddParameterData("LocationName", typeof(string), string.Format("%{0}%", location.LocationName));
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LocationID", NHibernateUtil.Int64);
            query.AddScalar("LocationCode", NHibernateUtil.String);
            query.AddScalar("LocationName", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(Location))).List<Location>();
        }
        public IList<LocationLangResult> FindLocationLangByLocationID(long locationId)
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("Select lang.LanguageID as LanguageID,lang.LanguageName as LanguageName "); //, ll.Description as LocationName, ll.Comment as Comment, ll.Active as Active ");
            sqlBuilder.Append("FROM DbLanguage as lang ");
            //sqlBuilder.Append("LEFT JOIN DbLocationLang as ll on lang.LanguageID = ll.LanguageID AND ll.LocationID =:LocationID ");
            sqlBuilder.Append("Where lang.LanguageID = :LanguageID ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("LocationID", typeof(long), locationId);
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LanguageID", NHibernateUtil.Int16);
            query.AddScalar("LanguageName", NHibernateUtil.String);
            //query.AddScalar("LocationName", NHibernateUtil.String);
            //query.AddScalar("Comment", NHibernateUtil.String);
            //query.AddScalar("Active", NHibernateUtil.Boolean);

            return query.SetResultTransformer(Transformers.AliasToBean(typeof(LocationLangResult))).List<LocationLangResult>();
        }

        public IList<Location> FindAutoComplete(string prefixText, Location param)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select dbLocate.LocationID as LocationID,dbLocate.LocationCode as LocationCode, dbLocate.Description as LocationName ");
            sqlBuilder.Append(" from DbLocation dbLocate ");

            StringBuilder whereClauseBuilder = new StringBuilder();
            whereClauseBuilder.Append(" and dbLocate.Active = 1 ");
            if (!string.IsNullOrEmpty(prefixText))
            {
                whereClauseBuilder.Append(" and ((dbLocate.LocationCode Like :prefixText) or (dbLocate.Description Like :prefixText)) ");
                queryParameterBuilder.AddParameterData("prefixText", typeof(string), String.Format("{0}%", prefixText));
            }
            if (param.CompanyID >0)
            {
                whereClauseBuilder.Append(" and dbLocate.CompanyID = :CompanyID ");
                queryParameterBuilder.AddParameterData("CompanyID", typeof(long),param.CompanyID);
            }

            if (!string.IsNullOrEmpty(whereClauseBuilder.ToString()))
            {
                sqlBuilder.Append(String.Format(" where 1=1 {0} ", whereClauseBuilder.ToString()));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            queryParameterBuilder.FillParameters(query);

            query.AddScalar("LocationID", NHibernateUtil.Int64)
                .AddScalar("LocationCode", NHibernateUtil.String)
                .AddScalar("LocationName", NHibernateUtil.String);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(Location)));
            IList<Location> list = query.List<Location>();

            return list;
        }


   //new location table  use in LocationLookup
      

        public IList<DbLocation> FindByLocationNameID(long locationID, string locationName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" SELECT LocationID as LocationID , LocationCode as LocationCode ,Description as LocationName ");
            sqlBuilder.Append(" FROM DbLocation ");
            sqlBuilder.Append(" WHERE  (LocationID = :locationID) ");

            if (!string.IsNullOrEmpty(locationName))
            {
                sqlBuilder.Append(" And Description like :locationName ");
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());

            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("locationID", typeof(long), locationID);
            if (!string.IsNullOrEmpty(locationName))
                queryParameterBuilder.AddParameterData("locationName", typeof(string), "%" + locationName + "%");
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("LocationID", NHibernateUtil.Int64);
            query.AddScalar("LocationCode", NHibernateUtil.String);
            query.AddScalar("LocationName", NHibernateUtil.String);


            return query.SetResultTransformer(Transformers.AliasToBean(typeof(DbLocation))).List<DbLocation>();
        }

        #region public int CountByLocationCriteria(string companyID, string locationCode, string locationName)
        public int CountByCriteriaLocation(string companyID, string locationCode, string locationName)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbLocationQuery, "FindByCriteriaLocation", new object[] { true, string.Empty, companyID, locationCode, locationName });
        }
        #endregion public int CountByLocationCriteria(string companyID, string locationCode, string locationName)

        #region public IList<Location> GetLocationList(int firstResult, int maxResult, string sortExpression,string companyID, string locationCode, string locationName)
        public IList<Location> GetListLocation(int firstResult, int maxResult, string sortExpression, string companyID, string locationCode, string locationName)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<Location>(ScgDbQueryProvider.DbLocationQuery, "FindByCriteriaLocation", new object[] { false, sortExpression, companyID, locationCode, locationName }, firstResult, maxResult, sortExpression);
        }
        #endregion public IList<Location> GetLocationList(int firstResult, int maxResult, string sortExpression,string companyID, string locationCode, string locationName)

        #region public ISQLQuery FindByCriteriaLocation(bool isCount, string sortExpression, string companyID, string locationCode, string locationName, short languageId)
        public ISQLQuery FindByCriteriaLocation(bool isCount, string sortExpression, string companyID, string locationCode, string locationName)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();
            if (!isCount)
            {
                sqlBuilder.Append(" select  DbLocation.LocationID as LocationID ,  DbLocation.LocationCode as LocationCode,");
                sqlBuilder.Append(" DbLocation.Description as LocationName , DbLocation.CompanyID as CompanyID, ");
                sqlBuilder.Append(" DbLocation.CompanyCode as CompanyCode ,DbCompany.CompanyName as CompanyName from DbLocation ");
                sqlBuilder.Append(" inner join DbCompany on DbLocation.CompanyID = DbCompany.CompanyID ");
                sqlBuilder.Append(" WHERE 1=1 ");

                if (companyID != null && companyID != "")
                {
                    sqlBuilder.Append(" AND DbLocation.CompanyID = :CompanyID ");
                    parameterBuilder.AddParameterData("CompanyID", typeof(long), long.Parse(companyID));
                }
                if (locationCode != null && locationCode != "")
                {
                    sqlBuilder.Append(" AND DbLocation.LocationCode LIKE :LocationCode ");
                    parameterBuilder.AddParameterData("LocationCode", typeof(string), "%" + locationCode + "%");
                }
                if (locationName != null && locationName != "")
                {
                    sqlBuilder.Append(" AND DbLocation.Description LIKE :LocationName ");
                    parameterBuilder.AddParameterData("LocationName", typeof(string), "%" + locationName + "%");
                }

                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY DbLocation.CompanyID,DbLocation.LocationCode,DbLocation.Description ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            else
            {
                sqlBuilder.Append(" SELECT COUNT(DbLocation.LocationID) as LocationCount  From DbLocation ");

                sqlBuilder.Append(" WHERE 1=1 ");
                if (companyID != null && companyID != "")
                {
                    sqlBuilder.Append(" AND DbLocation.CompanyID = :CompanyID ");
                    parameterBuilder.AddParameterData("CompanyID", typeof(long), long.Parse(companyID));
                }
                if (locationCode != null && locationCode != "")
                {
                    sqlBuilder.Append(" AND DbLocation.LocationCode LIKE :LocationCode ");
                    parameterBuilder.AddParameterData("LocationCode", typeof(string), "%" + locationCode + "%");
                }
                if (locationName != null && locationName != "")
                {
                    sqlBuilder.Append(" AND DbLocation.Description = :LocationName ");
                    parameterBuilder.AddParameterData("LocationName", typeof(string), "%" + locationName + "%");
                }

            
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("LocationID", NHibernateUtil.Int64);
                query.AddScalar("LocationCode", NHibernateUtil.String);
                query.AddScalar("LocationName", NHibernateUtil.String);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("CompanyID", NHibernateUtil.Int64);
                query.AddScalar("CompanyName", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(Location)));
            }
            else
            {
                query.AddScalar("LocationCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }
        #endregion public ISQLQuery FindByCriteriaLocation(bool isCount, string sortExpression, string companyID, string locationCode, string locationName)

        ///// end new location table 

        public DbLocation FindDbLocationByLocationCode(string locationCode)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbLocation), "a");
            criteria.Add(Expression.Eq("a.LocationCode", locationCode));

            return criteria.UniqueResult<DbLocation>();
        }
    }
}
