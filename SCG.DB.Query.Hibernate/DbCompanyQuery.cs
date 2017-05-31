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
    public class DbCompanyQuery : NHibernateQueryBase<DbCompany, long>, IDbCompanyQuery
    {

        public SS.Standard.Security.IUserAccount UserAccount { get; set; }

        public ISQLQuery FindCompanyByCriteria(DbCompany company,bool? flagActive, bool isCount, string sortExpression)
        {
            return FindCompanyByCriteria1(company, UserAccount.CurrentLanguageID, flagActive, isCount, sortExpression);
        }

        public IList<CompanyResult> GetCompanyList(DbCompany company,bool? flagAcitive, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<CompanyResult>(ScgDbQueryProvider.DbCompanyQuery, "FindCompanyByCriteria", new object[] { company, flagAcitive, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountCompanyByCriteria(DbCompany company, bool? flagActive)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCompanyQuery, "FindCompanyByCriteria", new object[] { company, flagActive, true, string.Empty });
        }

        public IList<DbCompany> FindAutoComplete(string companyCode, bool? useECC , bool? flagActive)
        {
            ICriteria criteria = GetCurrentSession().CreateCriteria(typeof(DbCompany), "c");
            criteria.Add(Expression.Like("c.CompanyCode", string.Format("%{0}%", companyCode)));

            if (useECC.HasValue)
                criteria.Add(Expression.Eq("c.UseEcc", (useECC.Value ? true : false)));
            if (flagActive.HasValue)
                criteria.Add(Expression.Eq("c.Active", flagActive.Value));
            return criteria.List<DbCompany>();
        }

        public DbCompany getDbCompanyByCompanyCode(string companyCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT com.CompanyID, com.UseEcc, com.CompanyCode, com.SapCode, comEx.MileageProfileId FROM DbCompany com INNER JOIN DbCompanyEx comEx ON com.CompanyCode = comEx.CompanyCode WHERE com.CompanyCode = :companycode");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companycode", typeof(string), companyCode);
            parameterBuilder.FillParameters(query);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("UseEcc", NHibernateUtil.Boolean);
            query.AddScalar("CompanyCode", NHibernateUtil.String);
            query.AddScalar("SapCode", NHibernateUtil.String);
            query.AddScalar("MileageProfileId", NHibernateUtil.Guid);

            IList<DbCompany> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCompany))).List<DbCompany>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbCompany>(0);
        }
        public Guid getMileageProfileByCompanyID(Int64 companyid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT comEx.MileageProfileId FROM DbCompany com INNER JOIN DbCompanyEx comEx ON com.CompanyCode = comEx.CompanyCode WHERE com.CompanyID = :companyid");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companyid", typeof(string), companyid);
            parameterBuilder.FillParameters(query);
            query.AddScalar("MileageProfileId", NHibernateUtil.Guid);

            string result = Convert.ToString(query.UniqueResult());
            if (!string.IsNullOrEmpty(result))
                return new Guid(result);
            else
                return new Guid();
        }
        // Create By Kookkla....
        public ISQLQuery FindCompanyByCriteria1(DbCompany company, short CurrentLang, bool? flagActive, bool isCount, string sortExpression)
        {
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();

            StringBuilder sqlBuilder = new StringBuilder();
            if (isCount)
            {
                sqlBuilder.Append("SELECT count(*) as CompanyCount ");
            }
            else
            {
                sqlBuilder.Append("SELECT c.CompanyID AS CompanyID , c.CompanyCode AS CompanyCode , c.CompanyName AS CompanyName , c.PaymentType as PaymentType , c.Active AS Active, ");
                sqlBuilder.Append("petty.PaymentMethodName as PettyName,Petty.Active as PettyActive , ");
                sqlBuilder.Append("Transfer.PaymentMethodName as TransferName,Transfer.Active as TransferActive, ");
                sqlBuilder.Append("cheque.PaymentMethodName as ChequeName,Cheque.Active as ChequeActive,sl.StatusDesc as StatusDesc, ");
                sqlBuilder.Append("c.DefaultTaxID AS DefaultTaxID,Tax.TaxCode as DefaultTaxCode ");
            }

            sqlBuilder.Append("FROM DbCompany c ");
            sqlBuilder.Append("Left join DbPaymentMethod as Petty on petty.PaymentMethodID = c.PaymentMethodPettyID ");
            sqlBuilder.Append("Left join DbPaymentMethod as Transfer on transfer.PaymentMethodID = c.PaymentMethodTransferID ");
            sqlBuilder.Append("Left join DbPaymentMethod as Cheque on Cheque.PaymentMethodID = c.PaymentMethodChequeID ");
            sqlBuilder.Append("Left join DbTax as Tax on Tax.TaxID = c.DefaultTaxID ");
            sqlBuilder.Append("left outer join DbStatus s on c.PaymentType = s.Status AND s.GroupStatus IN ('PaymentTypeDMT','PaymentTypeFRN') ");
            sqlBuilder.Append("left outer join DbStatusLang sl on s.StatusID = sl.StatusID ");
            sqlBuilder.Append("WHERE c.CompanyCode Like :CompanyCode ");
            sqlBuilder.Append("AND c.CompanyName Like :CompanyName ");
            sqlBuilder.Append("AND sl.LanguageID  = :CurrentLang ");
            sqlBuilder.Append("AND s.GroupStatus IN ('PaymentTypeDMT','PaymentTypeFRN') ");

            if (company.UseEcc.HasValue)
            {
                sqlBuilder.Append("AND c.UseEcc = :UseEcc ");
                queryParameterBuilder.AddParameterData("UseEcc", typeof(bool), company.UseEcc.Value);
            }
            if (flagActive.HasValue)
            {
                sqlBuilder.Append("AND c.Active = :flagActive ");
                queryParameterBuilder.AddParameterData("flagActive", typeof(bool), flagActive.Value);
                
            }
            if (!isCount)
            {
                //PaymentTypeDMT
                if (string.IsNullOrEmpty(sortExpression))
                    sqlBuilder.AppendLine("ORDER BY c.CompanyCode,c.CompanyName,c.Active ");
                else
                    sqlBuilder.AppendLine(string.Format(" ORDER BY {0}", sortExpression));
            }

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            
            queryParameterBuilder.AddParameterData("CompanyCode", typeof(string), string.Format("%{0}%", company.CompanyCode));
            queryParameterBuilder.AddParameterData("CompanyName", typeof(string), string.Format("%{0}%", company.CompanyName));
            queryParameterBuilder.AddParameterData("CurrentLang", typeof(short), CurrentLang);
            queryParameterBuilder.FillParameters(query);

            if (!isCount)
            {
                query.AddScalar("CompanyID", NHibernateUtil.Int64);
                query.AddScalar("CompanyCode", NHibernateUtil.String);
                query.AddScalar("CompanyName", NHibernateUtil.String);
                query.AddScalar("PaymentType", NHibernateUtil.String);
                query.AddScalar("Active", NHibernateUtil.Boolean);

                query.AddScalar("PettyName", NHibernateUtil.String);
                query.AddScalar("PettyActive", NHibernateUtil.Boolean);

                query.AddScalar("TransferName", NHibernateUtil.String);
                query.AddScalar("TransferActive", NHibernateUtil.Boolean);

                query.AddScalar("ChequeName", NHibernateUtil.String);
                query.AddScalar("ChequeActive", NHibernateUtil.Boolean);
                query.AddScalar("StatusDesc", NHibernateUtil.String);
                query.AddScalar("DefaultTaxID", NHibernateUtil.Int64);
                query.AddScalar("DefaultTaxCode", NHibernateUtil.String);

                query.SetResultTransformer(Transformers.AliasToBean(typeof(CompanyResult)));
            }
            else
            {
                query.AddScalar("CompanyCount", NHibernateUtil.Int32);
                query.UniqueResult();
            }
            return query;
        }

        public IList<CompanyResult> GetCompanyList(DbCompany company, short CurrentLang, bool? flagActive, int firstResult, int maxResult, string sortExpression)
        {
            return NHibernateQueryHelper.FindPagingByCriteria<CompanyResult>(ScgDbQueryProvider.DbCompanyQuery, "FindCompanyByCriteria1", new object[] { company, CurrentLang, flagActive, false, sortExpression }, firstResult, maxResult, sortExpression);
        }

        public int CountCompanyByCriteria(DbCompany company, short CurrentLang, bool? flagActive)
        {
            return NHibernateQueryHelper.CountByCriteria(ScgDbQueryProvider.DbCompanyQuery, "FindCompanyByCriteria1", new object[] { company, CurrentLang, flagActive, true, string.Empty });
        }

        public DbCompany getDbCompanyBankAccountByCompanyCode(string companyCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * From DbCompany WHERE CompanyCode = :companycode");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companycode", typeof(string), companyCode);
            parameterBuilder.FillParameters(query);

            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            query.AddScalar("AccountNo", NHibernateUtil.String);
            query.AddScalar("AccountType", NHibernateUtil.String);
            query.AddScalar("BankName", NHibernateUtil.String);
            query.AddScalar("BankBranch", NHibernateUtil.String);
            query.AddScalar("CompanyName", NHibernateUtil.String);

            IList<DbCompany> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCompany))).List<DbCompany>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbCompany>(0);
        }

        public bool getUseECCOfComOfUserByUserName(string userName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select TOP 1 IsNULL(c.UseECC,0) as UseECC from SuUser u inner join DbCompany c on u.CompanyCode = c.CompanyCode where u.Active = 1 and u.UserName = :userName");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("userName", typeof(string), userName);
            parameterBuilder.FillParameters(query);
            query.AddScalar("UseECC", NHibernateUtil.Boolean);
            bool UseECC = query.UniqueResult<bool>();
            return UseECC;
        }

        public SapInstanceData GetSAPInstanceByCompanyCode(string companyCode)
        {
            /*N-Fix Query*/
            string sqlCommand = @"SELECT sap.AliasName, sap.SystemID, sap.Client, sap.UserName ,sap.Password, sap.Language, sap.SystemNumber, sap.MsgServerHost, sap.LogonGroup
                FROM DbCompany 
                INNER JOIN DbSapInstance sap ON DbCompany.SapCode = sap.Code WHERE CompanyCode = :companycode";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlCommand);
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companycode", typeof(string), companyCode);
            parameterBuilder.FillParameters(query);

            query.AddScalar("AliasName", NHibernateUtil.String);
            query.AddScalar("SystemID", NHibernateUtil.String);
            query.AddScalar("Client", NHibernateUtil.String);
            query.AddScalar("UserName", NHibernateUtil.String);
            query.AddScalar("Password", NHibernateUtil.String);
            query.AddScalar("Language", NHibernateUtil.String);
            query.AddScalar("SystemNumber", NHibernateUtil.String);
            query.AddScalar("MsgServerHost", NHibernateUtil.String);
            query.AddScalar("LogonGroup", NHibernateUtil.String);

            IList<SapInstanceData> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(SapInstanceData))).List<SapInstanceData>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<SapInstanceData>(0);
        }

        public DbSapInstance GetSAPDocTypeForPosting(string companyCode)
        {
            /*N-Addnew column DoctypeFixedAdvance*/
            string sqlCommand = @"SELECT sap.DocTypeExpPostingDM, sap.DocTypeExpPostingFR, sap.DocTypeExpRmtPostingDM, sap.DocTypeExpRmtPostingFR, 
                sap.DocTypeExpICPostingFR, sap.DocTypeAdvancePostingDM, sap.DocTypeAdvancePostingFR, sap.DocTypeRmtPosting, sap.UserCPIC
                ,sap.DocTypeFixedAdvance AS DocTypeFixedAdvancePosting,sap.DocTypeFixedAdvanceReturn AS DocTypeFixedAdvanceReturnPosting 
                FROM DbCompany 
                INNER JOIN DbSapInstance sap ON DbCompany.SapCode = sap.Code WHERE CompanyCode = :companycode";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlCommand);
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();
            parameterBuilder.AddParameterData("companycode", typeof(string), companyCode);
            parameterBuilder.FillParameters(query);

            query.AddScalar("DocTypeExpPostingDM", NHibernateUtil.String);
            query.AddScalar("DocTypeExpPostingFR", NHibernateUtil.String);

            query.AddScalar("DocTypeExpRmtPostingDM", NHibernateUtil.String);
            query.AddScalar("DocTypeExpRmtPostingFR", NHibernateUtil.String);

            query.AddScalar("DocTypeExpICPostingFR", NHibernateUtil.String);

            query.AddScalar("DocTypeAdvancePostingDM", NHibernateUtil.String);
            query.AddScalar("DocTypeAdvancePostingFR", NHibernateUtil.String);

            query.AddScalar("DocTypeRmtPosting", NHibernateUtil.String);

            query.AddScalar("UserCPIC", NHibernateUtil.String);

            /*N-edit*/
            query.AddScalar("DocTypeFixedAdvancePosting", NHibernateUtil.String);
            query.AddScalar("DocTypeFixedAdvanceReturnPosting", NHibernateUtil.String);

            IList<DbSapInstance> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbSapInstance))).List<DbSapInstance>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbSapInstance>(0);
        }

        public DbCompany GetDbCompanyByCriteria(string companyCode, bool? useECC , bool? flagActive)
        {
            QueryParameterBuilder parameterBuilder = new QueryParameterBuilder();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT CompanyID From DbCompany WHERE CompanyCode = :companycode ");
            if (useECC.HasValue)
            {
                sql.Append(" and UseEcc = :useECC ");
                parameterBuilder.AddParameterData("useECC", typeof(bool), useECC.Value ? true : false);
            }
            if (flagActive.HasValue)
            {
                sql.Append(" and Active = :flagActive ");
                parameterBuilder.AddParameterData("flagActive", typeof(bool), flagActive);
            }
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            
            parameterBuilder.AddParameterData("companycode", typeof(string), companyCode);
            parameterBuilder.FillParameters(query);
            query.AddScalar("CompanyID", NHibernateUtil.Int64);
            IList<DbCompany> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(DbCompany))).List<DbCompany>();
            if (list.Count <= 0)
                return null;
            else
                return list.ElementAt<DbCompany>(0);
        }
        public long GetFnPerdiemProfileCompany(long companyID)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            QueryParameterBuilder paramBuilder = new QueryParameterBuilder();
            sqlBuilder.Append(" select PerdiemProfileID ");
            sqlBuilder.Append("from FnPerdiemProfileCompany p where p.CompanyID = :CompanyID");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.SetInt64("CompanyID", companyID);
            paramBuilder.FillParameters(query);
            query.AddScalar("PerdiemProfileID", NHibernateUtil.Int64);
            return query.List<long>().FirstOrDefault();
        }
    }
}
