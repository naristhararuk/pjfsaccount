using System;
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
    public partial class DbBankLangDao : NHibernateDaoBase<DbBankLang, long>, IDbBankLangDao
    {
        public DbBankLangDao()
        {
        }

        #region public IList<SCG.DB.DTO.ValueObject.BankLang> FindByBankId(short bankId)
        public IList<SCG.DB.DTO.ValueObject.BankLang> FindByBankId(short bankId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT ");
            sql.Append("    DbBankLang.ID , ");
            sql.Append("    DbBankLang.BankId , ");
            sql.Append("    DbBankLang.BankName , ");
            sql.Append("    DbBankLang.ABBRName , ");
            sql.Append("    DbBankLang.Comment , ");
            sql.Append("    DbBankLang.Active , ");
            sql.Append("    DbLanguage.LanguageId , ");
            sql.Append("    DbLanguage.LanguageName ");
            sql.Append(" FROM DbLanguage");
            sql.Append("    LEFT JOIN DbBankLang    ON DbLanguage.LanguageId    = DbBankLang.LanguageId AND DbBankLang.BankId = :BankId ");
            sql.Append("    LEFT JOIN DbBank        ON DbBankLang.BankId        = DbBank.BankId");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            QueryParameterBuilder queryParameterBuilder = new QueryParameterBuilder();
            queryParameterBuilder.AddParameterData("BankId", typeof(Int16), bankId);
            queryParameterBuilder.FillParameters(query);
            query.AddScalar("BankId", NHibernateUtil.Int16);
            query.AddScalar("LanguageId", NHibernateUtil.Int16);
            query.AddScalar("BankName", NHibernateUtil.String);
            query.AddScalar("ABBRName", NHibernateUtil.String);
            query.AddScalar("Comment", NHibernateUtil.String);
            query.AddScalar("Active", NHibernateUtil.Boolean);
            query.AddScalar("LanguageName", NHibernateUtil.String);

            IList<SCG.DB.DTO.ValueObject.BankLang> list =
                query.SetResultTransformer(Transformers.AliasToBean(typeof(SCG.DB.DTO.ValueObject.BankLang))).List<SCG.DB.DTO.ValueObject.BankLang>();
            return list;
        }
        #endregion public IList<SCG.DB.DTO.ValueObject.BankLang> FindByBankId(short bankId)

        #region public void DeleteAllBankLang(short bankId)
        public void DeleteAllBankLang(short bankId)
        {
            GetCurrentSession()
            .Delete(" FROM DbBankLang bank WHERE bank.Bank.Bankid = :BankID ",
            new object[] { bankId },
            new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
        #endregion public void DeleteAllBankLang(short bankId)
    }
}
