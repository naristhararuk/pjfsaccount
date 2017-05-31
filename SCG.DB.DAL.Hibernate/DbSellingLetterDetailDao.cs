using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;

using SCG.DB.DTO;
using SCG.DB.DAL;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbSellingLetterDetailDao : NHibernateDaoBase<DbSellingLetterDetail, long>, IDbSellingLetterDetailDao
    {
        public DbSellingLetterDetailDao()
        {

        }

        public void InsertData(DbSellingLetterDetail sellingLetterDetail) 
        {
      
            StringBuilder sql = new StringBuilder();

            sql.Append("    INSERT INTO DbSellingLetterDetail (LetterID,LetterNo,BuyingDate,CompanyName,BankName,BankBranch,AccountType,AccountNo,CreBy,CreDate,UpdBy,UpdDate)    ");
            sql.Append("    VALUES ( :letterID , :letterNo , :buyingDate, :companyName, :bankName, :bankBranch, :accountType, :accountNo, :creBy, :creDate, :updBy, :updDate ) ");

            QueryParameterBuilder param = new QueryParameterBuilder();
            param.AddParameterData("letterID", typeof(long), sellingLetterDetail.LetterID);
            param.AddParameterData("letterNo", typeof(long), sellingLetterDetail.LetterNo);
            param.AddParameterData("buyingDate", typeof(long), sellingLetterDetail.BuyingDate);
            param.AddParameterData("companyName", typeof(long), sellingLetterDetail.CompanyName);
            param.AddParameterData("bankName", typeof(long), sellingLetterDetail.BankName);
            param.AddParameterData("bankBranch", typeof(long), sellingLetterDetail.BankBranch);
            param.AddParameterData("accountType", typeof(long), sellingLetterDetail.AccountType);
            param.AddParameterData("accountNo", typeof(long), sellingLetterDetail.AccountNo);
            param.AddParameterData("creBy", typeof(long), sellingLetterDetail.CreBy);
            param.AddParameterData("creDate", typeof(DateTime), sellingLetterDetail.CreDate);
            param.AddParameterData("updBy", typeof(long), sellingLetterDetail.UpdBy);
            param.AddParameterData("updDate", typeof(DateTime), sellingLetterDetail.UpdDate);

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            param.FillParameters(query);
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();

        }
    }
}