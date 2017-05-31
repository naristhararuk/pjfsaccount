using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Query.Interface;
using SCG.eAccounting.SAP.DTO.ValueObject;

namespace SCG.eAccounting.SAP.Query.Hibernate
{
    public class Bapiret2Query : NHibernateQueryBase<Bapiret2, long>, IBapiret2Query
    {
        public IList<Bapiret2> FindByDocID(long DocId, string DocKind)
        {
            return GetCurrentSession().CreateQuery("FROM Bapiret2 A WHERE  A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiret2>();
        }

        public IList<Bapiret2> FindByDocID(long DocId, string DocSeq, string DocKind)
        {
            return GetCurrentSession().CreateQuery("FROM Bapiret2 A WHERE A.DocId = :DOC_ID AND A.DocSeq = :DOC_SEQ AND A.DocKind = :DOC_KIND")
                .SetInt64("DOC_ID", DocId)
                .SetString("DOC_SEQ", DocSeq)
                .SetString("DOC_KIND", DocKind)
                .List<Bapiret2>();
        }

        public void DeleteByDocID(long DocId, string DocKind)
        {
            GetCurrentSession()
            .Delete(" FROM Bapiret2 A WHERE A.DocId = :DOC_ID AND A.DocKind = :DOC_KIND",
            new object[] { DocId, DocKind },
            new NHibernate.Type.IType[] { NHibernateUtil.Int64, NHibernateUtil.String });
        }


        public ISQLQuery FindBapiret2ByCriteria(SapLog ret2, bool isCount, string sortExpression, string DocNo, string PostingDate, string Type)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            StringBuilder sqlBuilder = new StringBuilder();

            #region Old Code
//            if (!isCount)
//            {
//                #region Select
//                sqlBuilder.Append(@"
//                        SELECT      
//	                        BAPIRET2.Id1                                                    AS Id,
//	                        BAPIRET2.Doc_Id                                                 AS DocId, 
//	                        [Document].DocumentNo                                           AS DocNo, 
//	                        BAPIRET2.Doc_Seq                                                 AS DocSeq, 
//	                        BAPIACHE09.Doc_Year                                              AS DocYear,
//                            CONVERT(VARCHAR(10),CONVERT(DATETIME,BAPIACHE09.Pstng_Date),103) AS PostingDate,
//                            SuUser.EmployeeCode                                             AS EmployeeCode, 
//	                        SuUser.EmployeeName                                             AS EmployeeName, 
//	                        '[' + SuUser.EmployeeCode + '] ' + SuUser.EmployeeName          AS Employee,
//	                        DbCompany.CompanyCode                                           AS CompanyCode, 
//	                        DbCompany.CompanyName                                           AS CompanyName,
//	                        '[' + DbCompany.CompanyCode + '] ' + DbCompany.CompanyName      AS Company,
//	                        CASE 
//		                        WHEN BAPIRET2.Type = 'S' THEN 'Success' 
//		                        WHEN BAPIRET2.Type = 'E' THEN 'Error' 
//		                        WHEN BAPIRET2.Type = 'W' THEN 'Warning' 
//		                        ELSE 'Error'
//	                        END                                                             AS Type, 
//	                        BAPIRET2.Message                                                AS Message
//                        FROM         
//                            DbCompany AS DbCompany 
//                                RIGHT OUTER JOIN
//                            BAPIACHE09 AS BAPIACHE09 
//                                RIGHT OUTER JOIN
//                            BAPIRET2 AS BAPIRET2 ON 
//                                BAPIACHE09.Doc_Id = BAPIRET2.Doc_Id AND 
//                                BAPIACHE09.Doc_Seq = BAPIRET2.Doc_Seq 
//                                LEFT OUTER JOIN
//                            [Document] AS Document ON 
//                                BAPIRET2.Doc_Id = [Document].DocumentID 
//                                LEFT OUTER JOIN
//                            SuUser AS SuUser ON 
//                                [Document].RequesterID = SuUser.Userid ON 
//                            DbCompany.CompanyId = [Document].CompanyId
//                        WHERE 1=1
//                    ");
//                #endregion Select
//            }
//            else
//            {
//                #region Count
//                sqlBuilder.Append(@"
//                        SELECT      
//	                        COUNT(BAPIRET2.Id1) AS SapLogCount
//                        FROM         
//                            DbCompany AS DbCompany 
//                                RIGHT OUTER JOIN
//                            BAPIACHE09 AS BAPIACHE09 
//                                RIGHT OUTER JOIN
//                            BAPIRET2 AS BAPIRET2 ON 
//                                BAPIACHE09.Doc_Id = BAPIRET2.Doc_Id AND 
//                                BAPIACHE09.Doc_Seq = BAPIRET2.Doc_Seq 
//                                LEFT OUTER JOIN
//                            [Document] AS Document ON 
//                                BAPIRET2.Doc_Id = [Document].DocumentID 
//                                LEFT OUTER JOIN
//                            SuUser AS SuUser ON 
//                                [Document].RequesterID = SuUser.Userid ON 
//                            DbCompany.CompanyId = [Document].CompanyId
//                        WHERE 1=1
//                    ");
//                #endregion Count
//            }
            #endregion Old Code

            if (!isCount)
            {
                #region Select
                sqlBuilder.Append(@"
                        SELECT      
	                        VIEW_POST_LOG.Id            AS Id,
	                        VIEW_POST_LOG.DocId         AS DocId, 
                            VIEW_POST_LOG.PostingType   AS PostingType,
	                        VIEW_POST_LOG.DocNo         AS DocNo, 
	                        VIEW_POST_LOG.DocSeq        AS DocSeq, 
	                        VIEW_POST_LOG.DocYear       AS DocYear,
                            VIEW_POST_LOG.PostingDate   AS PostingDate,
                            VIEW_POST_LOG.FiDoc         AS FiDoc,
                            VIEW_POST_LOG.EmployeeCode  AS EmployeeCode, 
	                        VIEW_POST_LOG.EmployeeName  AS EmployeeName, 
	                        VIEW_POST_LOG.Employee      AS Employee,
	                        VIEW_POST_LOG.CompanyCode   AS CompanyCode, 
	                        VIEW_POST_LOG.CompanyName   AS CompanyName,
	                        VIEW_POST_LOG.Company       AS Company,
	                        VIEW_POST_LOG.Type          AS Type, 
	                        VIEW_POST_LOG.MESSAGE       AS Message
                        FROM         
                            VIEW_POST_LOG AS VIEW_POST_LOG
                        WHERE 1=1
                    ");
                #endregion Select
            }
            else
            {
                #region Count
                sqlBuilder.Append(@"
                        SELECT      
	                        COUNT(VIEW_POST_LOG.Id) AS SapLogCount
                        FROM         
                            VIEW_POST_LOG AS VIEW_POST_LOG
                        WHERE 1=1
                    ");
                #endregion Count
            }

            #region เงื่อนไข
            if (!string.IsNullOrEmpty(DocNo))
            {
                sqlBuilder.Append(" AND VIEW_POST_LOG.DocNo = :DocNo ");
                parameterBuilder.AddParameterData("DocNo", typeof(string), DocNo);
            }
            if (!string.IsNullOrEmpty(PostingDate))
            {
                sqlBuilder.Append(" AND VIEW_POST_LOG.PostingDateString = :PostingDate ");
                parameterBuilder.AddParameterData("PostingDate", typeof(string), PostingDate);
            }
            if (!string.IsNullOrEmpty(Type))
            {
                sqlBuilder.Append(" AND VIEW_POST_LOG.TypeString = :Type ");
                parameterBuilder.AddParameterData("Type", typeof(string), Type);
            }
            #endregion เงื่อนไข

            #region Order By
            if (!isCount)
            {
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine(" ORDER BY PostingDate,EmployeeCode,DocNo,DocSeq,PostingType,DocYear,CompanyCode,FiDoc,Type,Message  ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }
            #endregion Order By

            

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            parameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("Id"            , NHibernateUtil.Int64);
                query.AddScalar("DocId"         , NHibernateUtil.Int64);
                query.AddScalar("PostingType"   , NHibernateUtil.String);
                query.AddScalar("DocNo", NHibernateUtil.String);
                query.AddScalar("DocSeq", NHibernateUtil.String);
                query.AddScalar("DocYear", NHibernateUtil.String);
                query.AddScalar("PostingDate", NHibernateUtil.DateTime);
                query.AddScalar("FiDoc"         , NHibernateUtil.String);
                query.AddScalar("Employee", NHibernateUtil.String);
                query.AddScalar("EmployeeCode", NHibernateUtil.String);
                query.AddScalar("EmployeeName", NHibernateUtil.String);
                query.AddScalar("Company", NHibernateUtil.String);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("CompanyName", NHibernateUtil.String);
                query.AddScalar("Type"          , NHibernateUtil.String);
                query.AddScalar("Message"       , NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(SapLog)));
            }
            else
            {
                query.AddScalar("SapLogCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }

            return query;
        }

        public IList<SapLog> GetBapiret2ByCriteria(SapLog ret2, int firstResult, int maxResult, string sortExpression, string DocNo, string PostingDate, string Type)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<SapLog>(BapiQueryProvider.Bapiret2Query, "FindBapiret2ByCriteria", new object[] { ret2, false, sortExpression, DocNo, PostingDate, Type }, firstResult, maxResult, sortExpression);
        }

        public int CountBapiret2ByCriteria(SapLog ret2, string DocNo, string PostingDate, string Type)
        {
            return NHibernateQueryHelper.CountByCriteria(BapiQueryProvider.Bapiret2Query, "FindBapiret2ByCriteria", new object[] { ret2, true, string.Empty, DocNo, PostingDate, Type });
        }
    }
}
