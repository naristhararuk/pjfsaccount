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
    public partial class TmpDbInternalOrderDao :NHibernateDaoBase<TmpDbInternalOrder ,long >,ITmpDbInternalOrderDao
    {
        public void deleteAllInternalOrderTmp()
        {
            GetCurrentSession().Delete(" FROM TmpDbInternalOrder ");
        }
        public void setActiveAll(bool active, bool eccflag, string aliasName)
        {
            string sql = "UPDATE DbInternalOrder SET Active = :IsActive from DbCompany ";
            if (eccflag)
                sql += " inner join DbSapInstance  on DbCompany.SAPCode = DbSapInstance.Code ";

            sql += " where DbInternalOrder.CompanyID = DbCompany.CompanyID and DbCompany.UseEcc = :UseEccFlag ";

            if (eccflag)
                sql += " and DbSapInstance.AliasName = :AliasName ";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql);

            query.SetBoolean("IsActive", active);
            query.SetBoolean("UseEccFlag", eccflag);

            if (eccflag)
                query.SetString("AliasName", aliasName);

            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }
        public void addTmpInternalOrder(TmpDbInternalOrder tmpDbInternalOrder)
        {
            ScgDbDaoProvider.TmpDbInternalOrderDao.Save(tmpDbInternalOrder);   
        }

        public void addNewInternalOrderFromTmp()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO DbInternalOrder ");
            sql.Append("(IONumber, IOType, IOText, CostCenterID, CostCenterCode, CompanyID, ");
            sql.Append("CompanyCode, EffectiveDate, ExpireDate, Active, CreBy, CreDate, UpdBy, UpdDate, ");
            sql.Append("UpdPgm, BusinessArea, ProfitCenter) SELECT     Tmp_DbInternalOrder.IONumber, Tmp_DbInternalOrder.IOType, ");
            sql.Append("Tmp_DbInternalOrder.IOText, Tmp_DbInternalOrder.CostCenterID, Tmp_DbInternalOrder.CostCenterCode, ");
            sql.Append("Tmp_DbInternalOrder.CompanyID, Tmp_DbInternalOrder.CompanyCode, Tmp_DbInternalOrder.EffectiveDate, ");
            sql.Append(" Tmp_DbInternalOrder.ExpireDate, Tmp_DbInternalOrder.Active, Tmp_DbInternalOrder.CreBy, Tmp_DbInternalOrder.CreDate, Tmp_DbInternalOrder.UpdBy, ");
            sql.Append("Tmp_DbInternalOrder.UpdDate, Tmp_DbInternalOrder.UpdPgm, Tmp_DbInternalOrder.BusinessArea, Tmp_DbInternalOrder.ProfitCenter ");
            sql.Append("FROM         Tmp_DbInternalOrder LEFT OUTER JOIN ");
            sql.Append("DbInternalOrder AS DbInternalOrder_1 ON Tmp_DbInternalOrder.IONumber = DbInternalOrder_1.IONumber ");
            sql.Append("WHERE     (DbInternalOrder_1.IONumber IS NULL) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void updateNewInternalOrder()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE    DbInternalOrder SET   ");
            sql.Append("IOType = Tmp_DbInternalOrder.IOType, IOText = Tmp_DbInternalOrder.IOText, CostCenterID = Tmp_DbInternalOrder.CostCenterID, ");
            sql.Append("CostCenterCode = Tmp_DbInternalOrder.CostCenterCode, CompanyID = Tmp_DbInternalOrder.CompanyID, CompanyCode = Tmp_DbInternalOrder.CompanyCode, ");
            sql.Append("EffectiveDate = Tmp_DbInternalOrder.EffectiveDate, ExpireDate = Tmp_DbInternalOrder.ExpireDate, Active = Tmp_DbInternalOrder.Active, ");
            sql.Append(" UpdBy = Tmp_DbInternalOrder.UpdBy, UpdDate = Tmp_DbInternalOrder.UpdDate, ");
            sql.Append("UpdPgm = Tmp_DbInternalOrder.UpdPgm, BusinessArea = Tmp_DbInternalOrder.BusinessArea, ProfitCenter = Tmp_DbInternalOrder.ProfitCenter ");
            sql.Append("FROM         DbInternalOrder INNER JOIN ");
            sql.Append("Tmp_DbInternalOrder ON DbInternalOrder.IONumber = Tmp_DbInternalOrder.IONumber");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();

        }

        public void getCompanyIDAndCostCenterIDToTmp()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE    Tmp_DbInternalOrder ");
            sql.Append(" SET    CompanyID = DbCompany.CompanyID, CostCenterID = DbCostCenter.CostCenterID ");
            sql.Append(" FROM         Tmp_DbInternalOrder LEFT OUTER JOIN ");
            sql.Append(" DbCompany ON DbCompany.CompanyCode = Tmp_DbInternalOrder.CompanyCode LEFT OUTER JOIN ");
            sql.Append(" DbCostCenter ON Tmp_DbInternalOrder.CostCenterCode = DbCostCenter.CostCenterCode ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingCompanyIDToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ");
            sql.Append("  ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT   Tmp_DbInternalOrder.IONumber, Tmp_DbInternalOrder.IOType, Tmp_DbInternalOrder.IOText, Tmp_DbInternalOrder.CompanyCode, ");
            sql.Append(" Tmp_DbInternalOrder.CostCenterCode, Tmp_DbInternalOrder.EffectiveDate, Tmp_DbInternalOrder.ExpireDate, Tmp_DbInternalOrder.Active, 1 AS Expr1,  ");
            sql.Append(" 'Missing Company  Record' AS Expr2, Tmp_DbInternalOrder.Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder LEFT OUTER JOIN ");
            sql.Append(" DbCompany ON Tmp_DbInternalOrder.CompanyCode = DbCompany.CompanyCode  ");
            sql.Append(" WHERE     (DbCompany.CompanyCode IS NULL) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingCostCenterIDToLog() 
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ");
            sql.Append("  ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT   Tmp_DbInternalOrder.IONumber, Tmp_DbInternalOrder.IOType, Tmp_DbInternalOrder.IOText, Tmp_DbInternalOrder.CompanyCode, ");
            sql.Append(" Tmp_DbInternalOrder.CostCenterCode, Tmp_DbInternalOrder.EffectiveDate, Tmp_DbInternalOrder.ExpireDate, Tmp_DbInternalOrder.Active, 2 AS Expr1,  ");
            sql.Append(" 'Missing Cost Center  Record' AS Expr2, Tmp_DbInternalOrder.Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder LEFT OUTER JOIN ");
            sql.Append(" DbCostCenter ON Tmp_DbInternalOrder.CostCenterCode = DbCostCenter.CostCenterCode ");
            sql.Append(" WHERE     (DbCostCenter.CostCenterCode IS NULL) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingIONumberToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 4 AS Expr1, 'Column  IONumber contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE     (IONumber = '') OR (IONumber IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingIOTypeToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 4 AS Expr1, 'Column  IOType contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE     (IOType = '') OR (IOType IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingIOTextToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 4 AS Expr1, 'Column  IOText contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE     (IOText = '') OR (IOText IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingCompanyCodeToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 4 AS Expr1, 'Column  CompanyCode contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE     (CompanyCode = '') OR (CompanyCode IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingCostCenterCodeToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 4 AS Expr1, 'Column  CostCenterCode contain no data' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE     (CostCenterCode = '') OR (CostCenterCode IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingEffectiveDateToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 5 AS Expr1, 'Invalid data format of column EffectiveDate' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE  (EffectiveDate IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void putMissingExpireDateDateToLog()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO DbIOImportLog  ");
            sql.Append(" (IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate, Active, ErrorCode, Message, Line) ");
            sql.Append(" SELECT     IONumber, IOType, IOText, CompanyCode, CostCenterCode, EffectiveDate, ExpireDate,   ");
            sql.Append("  Active, 5 AS Expr1, 'Invalid data format of column ExpireDate' AS Expr2, Line ");
            sql.Append(" FROM         Tmp_DbInternalOrder WHERE  (ExpireDate IS NULL) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sql.ToString());
            query.AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void SyncAllInternalOrder()
        {
            GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncAllInternalOrderData]").AddScalar("Count", NHibernateUtil.Int32).UniqueResult();
        }
    }
}
