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
    public partial class TmpDbCostCenterDao : NHibernateDaoBase<TmpDbCostCenter, long>, ITmpDbCostCenterDao
    {
        public TmpDbCostCenterDao()
        {
        }
        public void deleteAllTmpDbCostCenter()
        {
            GetCurrentSession().Delete(" FROM TmpDbCostCenter ");
        }
        public void addTmpCostCenter(TmpDbCostCenter tmpDbCosCenter)
        {
            ScgDbDaoProvider.TmpDbCostCenterDao.Save(tmpDbCosCenter);
        }

        public void setActiveAll(bool active, bool eccflag, string aliasName)
        {
            string sql = "UPDATE DbCostCenter SET Active = ? from DbCompany ";
            if (eccflag)
                sql += " inner join DbSapInstance on DbCompany.SAPCode = DbSapInstance.Code ";
            sql += " where DbCostCenter.CompanyID = DbCompany.CompanyID and DbCompany.UseEcc = ? ";
            if (eccflag)
                sql += " and DbSapInstance.AliasName = ? ";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);
            query.SetBoolean(0, active);
            query.SetBoolean(1, eccflag);

            if (eccflag)
                query.SetString(2, aliasName);

            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void getCompanyIdToTmpCostCenter()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" UPDATE    Tmp_DbCostCenter SET CompanyID = DbCompany.CompanyID ");
            sql.Append(" FROM         Tmp_DbCostCenter  LEFT OUTER JOIN ");
            sql.Append("  DbCompany ON Tmp_DbCostCenter.CompanyCode = DbCompany.CompanyCode ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void insertTmpToDbCostCenter()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("INSERT INTO DbCostCenter (CompanyID, CompanyCode, CostCenterCode, Valid, Expire, ");
            sql.Append("Description, ActualPrimaryCosts, Active, CreBy, CreDate, UpdBy, UpdDate, UpdPgm, BusinessArea, ProfitCenter) ");
            sql.Append("SELECT     Tmp_DbCostCenter.CompanyID, Tmp_DbCostCenter.CompanyCode, Tmp_DbCostCenter.CostCenterCode,");
            sql.Append("Tmp_DbCostCenter.Valid, Tmp_DbCostCenter.Expire, Tmp_DbCostCenter.Description, Tmp_DbCostCenter.ActualPrimaryCosts, ");
            sql.Append(" Tmp_DbCostCenter.Active, Tmp_DbCostCenter.CreBy, Tmp_DbCostCenter.CreDate, Tmp_DbCostCenter.UpdBy, Tmp_DbCostCenter.UpdDate, Tmp_DbCostCenter.UpdPgm, Tmp_DbCostCenter.BusinessArea, Tmp_DbCostCenter.ProfitCenter");
            sql.Append(" FROM Tmp_DbCostCenter LEFT OUTER JOIN ");
            sql.Append(" DbCostCenter AS DbCostCenter_1 ON Tmp_DbCostCenter.CostCenterCode = DbCostCenter_1.CostCenterCode");
            sql.Append(" WHERE     (DbCostCenter_1.CostCenterCode IS NULL AND Tmp_DbCostCenter.CompanyID IS NOT NULL)");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void updateTmpToDbCostCenter()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE DbCostCenter ");
            sql.Append(" SET CompanyID = Tmp_DbCostCenter.CompanyID ,  CompanyCode = Tmp_DbCostCenter.CompanyCode, Valid = Tmp_DbCostCenter.Valid, Expire = Tmp_DbCostCenter.Expire, ");
            sql.Append(" Description = Tmp_DbCostCenter.Description,  Active = Tmp_DbCostCenter.Active, ");
            sql.Append("  UpdBy = Tmp_DbCostCenter.UpdBy, UpdDate = Tmp_DbCostCenter.UpdDate, ");
            sql.Append("   UpdPgm = Tmp_DbCostCenter.UpdPgm, BusinessArea = Tmp_DbCostCenter.BusinessArea, ProfitCenter = Tmp_DbCostCenter.ProfitCenter ");
            sql.Append(" FROM DbCostCenter INNER JOIN");
            sql.Append(" Tmp_DbCostCenter ON DbCostCenter.CostCenterCode = Tmp_DbCostCenter.CostCenterCode");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void checkMissingCostCenterCode()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" INSERT INTO DbCostCenterImportLog ");
            sql.Append(" (CostCenterCode, ValidFrom, ExpireDate, Description, CompanyCode, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     CostCenterCode, Valid, Expire, Description, CompanyCode, Active, ");
            sql.Append(" 2 AS Expr1, 'Column CostCenterCode contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbCostCenter WHERE     ISNULL(CostCenterCode,'') = '' ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void checkMissingCompanyID()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbCostCenterImportLog ");
            sql.Append(" (CostCenterCode, ValidFrom, ExpireDate, Description, CompanyCode, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT Tmp_DbCostCenter.CostCenterCode, Tmp_DbCostCenter.Valid, Tmp_DbCostCenter.Expire, ");
            sql.Append(" Tmp_DbCostCenter.Description, Tmp_DbCostCenter.CompanyCode, ");
            sql.Append(" Tmp_DbCostCenter.Active, 1 AS Expr1, 'Missing Company Record' AS Expr2, Tmp_DbCostCenter.Line ");
            sql.Append(" FROM Tmp_DbCostCenter LEFT OUTER JOIN ");
            sql.Append(" DbCompany ON Tmp_DbCostCenter.CompanyCode = DbCompany.CompanyCode ");
            sql.Append(" WHERE (DbCompany.CompanyCode IS NULL) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void checkMissingCompanyCode()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" INSERT INTO DbCostCenterImportLog ");
            sql.Append(" (CostCenterCode, ValidFrom, ExpireDate, Description, CompanyCode, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT CostCenterCode, Valid, Expire, Description, CompanyCode, Active, 2 AS Expr1, ");
            sql.Append(" 'Column CompanyCode contain no data' AS Expr2, Line ");
            sql.Append(" FROM Tmp_DbCostCenter WHERE ISNULL(CompanyCode,'') = '' ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void checkMissingDescription()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" INSERT INTO DbCostCenterImportLog ");
            sql.Append("  (CostCenterCode, ValidFrom, ExpireDate, Description, CompanyCode, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     CostCenterCode, Valid, Expire, Description, CompanyCode, Active, 2 AS Expr1, ");
            sql.Append("  'Column Description contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbCostCenter WHERE     (Description = '') OR (Description IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void checkValidDateFormat()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" INSERT INTO DbCostCenterImportLog ");
            sql.Append("  (CostCenterCode, ValidFrom, ExpireDate, Description, CompanyCode, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     CostCenterCode, Valid, Expire, Description, CompanyCode, Active, 3 AS Expr1, ");
            sql.Append("  'Invalid data format of column Valid' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbCostCenter WHERE   (Valid IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void checkExpireDateFormat()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" INSERT INTO DbCostCenterImportLog ");
            sql.Append("  (CostCenterCode, ValidFrom, ExpireDate, Description, CompanyCode, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     CostCenterCode, Valid, Expire, Description, CompanyCode, Active, 3 AS Expr1, ");
            sql.Append("  'Invalid data format of column Expire' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbCostCenter WHERE   (Expire IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public bool CheckDuplicateCostCenterCodeFromTmpCostCenter(string costCenterCode)
        {
            // update by Sirilak 06-Nov-2009
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT count(*) as Count FROM Tmp_DbCostCenter WHERE CostCenterCode = :CostCenterCode ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.SetString("CostCenterCode", costCenterCode);
            query.AddScalar("Count", NHibernateUtil.Int32);

            int count = query.UniqueResult<int>();
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public void SyncAllCostCenter()
        {
            GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncAllCostCenterData]").AddScalar("Count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}
