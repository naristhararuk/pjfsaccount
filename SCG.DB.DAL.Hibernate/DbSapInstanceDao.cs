using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.DB.DTO;
using SS.Standard.Data.NHibernate.QueryCreator;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbSapInstanceDao : NHibernateDaoBase<DbSapInstance, string>, IDbSapInstanceDao
    {
        public DbSapInstanceDao()
        {
        }
        public bool IsDuplicateCode(DbSapInstance sapinstance)
        {
            IList<DbSapInstance> list = GetCurrentSession().CreateQuery("from DbSapInstance where Code like :code")
                  .SetString("code", sapinstance.Code)
                  .List<DbSapInstance>();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
        //public void InsertData(DbSAPInstance SAPInstance)
        //{
        //    StringBuilder sql = new StringBuilder();

        //    sql.Append("    INSERT INTO DbSapInstance ([Code],[AliasName],[SystemID],[Client],[UserName],[Password],[Language],[SystemNumber],[MsgServerHost],[LogonGroup],[UserCPIC],[DocTypeExpPostingDM],[DocTypeExpRmtPostingDM],[DocTypeExpPostingFR],[DocTypeExpRmtPostingFR],[DocTypeExpICPostingFR],[DocTypeAdvancePostingDM],[DocTypeAdvancePostingFR],[DocTypeRmtPosting],[UpdBy],[UpdDate],[CreBy],[CreDate],[UpdPgm],[RowVersion])    ");
        //    sql.Append("    VALUES (:Code,:AliasName,:SystemID,:Client,:UserName,:Password,:Language,:SystemNumber,:MsgServerHost,:LogonGroup,:UserCPIC,:DocTypeExpPostingDM,:DocTypeExpRmtPostingDM,:DocTypeExpPostingFR,:DocTypeExpRmtPostingFR,:DocTypeExpICPostingFR,:DocTypeAdvancePostingDM,:DocTypeAdvancePostingFR,:DocTypeRmtPosting,:UpdBy,:UpdDate,:CreBy,:CreDate,:UpdPgm,:RowVersion) ");
        //    QueryParameterBuilder param = new QueryParameterBuilder();
        //    param.AddParameterData("Code", typeof(string), SAPInstance.Code);
        //    param.AddParameterData("AliasName", typeof(string), SAPInstance.AliasName);
        //    param.AddParameterData("SystemID", typeof(string), SAPInstance.SystemID);
        //    param.AddParameterData("Client", typeof(long), SAPInstance.Client);
        //    param.AddParameterData("UserName", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("Password", typeof(string), SAPInstance.Password);
        //    param.AddParameterData("Language", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("SystemNumber", typeof(long), SAPInstance.UserName);
        //    param.AddParameterData("MsgServerHost", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("LogonGroup", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("UserCPIC", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeExpPostingDM", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeExpRmtPostingDM", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeExpPostingFR", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeExpRmtPostingFR", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeExpICPostingFR", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeAdvancePostingDM", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeAdvancePostingFR", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("DocTypeRmtPosting", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("UpdBy", typeof(long), SAPInstance.UserName);
        //    param.AddParameterData("UpdDate", typeof(DateTime), SAPInstance.UserName);
        //    param.AddParameterData("CreBy", typeof(long), SAPInstance.UserName);
        //    param.AddParameterData("CreDate", typeof(DateTime), SAPInstance.UserName);
        //    param.AddParameterData("UpdPgm", typeof(string), SAPInstance.UserName);
        //    param.AddParameterData("RowVersion", typeof(DateTime), SAPInstance.UserName);
        //    ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
        //    param.FillParameters(query);
        //    query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        //}
    }
}
