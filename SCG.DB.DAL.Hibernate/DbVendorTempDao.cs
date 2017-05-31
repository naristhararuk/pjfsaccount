using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.DB.DTO;
using NHibernate;

namespace SCG.DB.DAL.Hibernate
{
    public partial class DbVendorTempDao : NHibernateDaoBase<DbVendorTemp, long>, IDbVendorTempDao
    {

        #region IDbVendorTempDao Members

        public void DeleteAll()
        {
            string SQL = "truncate table DbVendor_Temp";
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(SQL);
            query.AddScalar("result", NHibernateUtil.Int32);
            query.UniqueResult();

           
        }

        public void SaveTempToVendor()
        {
            StringBuilder sqlInsert = new StringBuilder();
            #region Insert Query Builder
            sqlInsert.AppendLine("INSERT INTO DbVendor(VendorCode,VendorTitle,VendorName1,");
            sqlInsert.AppendLine("VendorName2,Street,City,District,Country,PostalCode,");
            sqlInsert.AppendLine("TaxNo1,TaxNo2,BlockDelete,BlockPost,Active,CreBy,CreDate,");
            sqlInsert.AppendLine("UpdBy,UpdDate,UpdPgm,TaxNo3,TaxNo4)");
            sqlInsert.AppendLine("SELECT DISTINCT DbVendor_temp.VendorCode,");
            sqlInsert.AppendLine("DbVendor_temp.VendorTitle,");
            sqlInsert.AppendLine("DbVendor_temp.VendorName1,");
            sqlInsert.AppendLine("DbVendor_temp.VendorName2,");
            sqlInsert.AppendLine("DbVendor_temp.Street,");
            sqlInsert.AppendLine("DbVendor_temp.City,");
            sqlInsert.AppendLine("DbVendor_temp.District,");
            sqlInsert.AppendLine("DbVendor_temp.Country,");
            sqlInsert.AppendLine("DbVendor_temp.PostalCode,");
            sqlInsert.AppendLine("DbVendor_temp.TaxNo1,");
            sqlInsert.AppendLine("DbVendor_temp.TaxNo2,");
            sqlInsert.AppendLine("DbVendor_temp.BlockDelete,");
            sqlInsert.AppendLine("DbVendor_temp.BlockPost,");
            sqlInsert.AppendLine("DbVendor_temp.Active,");
            sqlInsert.AppendLine("DbVendor_temp.CreBy,");
            sqlInsert.AppendLine("DbVendor_temp.CreDate,");
            sqlInsert.AppendLine("DbVendor_temp.UpdBy,");
            sqlInsert.AppendLine("DbVendor_temp.UpdDate,");
            sqlInsert.AppendLine("DbVendor_temp.UpdPgm,");
            sqlInsert.AppendLine("DbVendor_temp.TaxNo3,");
            sqlInsert.AppendLine("DbVendor_temp.TaxNo4");
            sqlInsert.AppendLine("FROM DbVendor_temp  ");
            sqlInsert.AppendLine("left join DbVendor");
            sqlInsert.AppendLine("ON DbVendor_temp.VendorCode = DbVendor.VendorCode ");
            sqlInsert.AppendLine("WHERE DbVendor.VendorCode is null ");
            #endregion
            ISQLQuery InsertQuery = GetCurrentSession().CreateSQLQuery(sqlInsert.ToString());
            InsertQuery.AddScalar("result", NHibernateUtil.Int32);
            InsertQuery.UniqueResult();
        }


        public void UpdateTempToVendor()
        {
            StringBuilder sqlUpdate = new StringBuilder();
            #region Update Query Builder
            sqlUpdate.AppendLine("UPDATE DbVendor ");
            sqlUpdate.AppendLine("SET  ");
            sqlUpdate.AppendLine("DbVendor.VendorCode = DbVendor_t.VendorCode,");
            sqlUpdate.AppendLine("DbVendor.VendorTitle = DbVendor_t.VendorTitle,");
            sqlUpdate.AppendLine("DbVendor.VendorName1 = DbVendor_t.VendorName1,");
            sqlUpdate.AppendLine("DbVendor.VendorName2 = DbVendor_t.VendorName2,");
            sqlUpdate.AppendLine("DbVendor.Street = DbVendor_t.Street,");
            sqlUpdate.AppendLine("DbVendor.City = DbVendor_t.City,");
            sqlUpdate.AppendLine("DbVendor.District = DbVendor_t.District,");
            sqlUpdate.AppendLine("DbVendor.Country = DbVendor_t.Country,");
            sqlUpdate.AppendLine("DbVendor.PostalCode = DbVendor_t.PostalCode,");
            sqlUpdate.AppendLine("DbVendor.TaxNo1 = DbVendor_t.TaxNo1,");
            sqlUpdate.AppendLine("DbVendor.TaxNo2 = DbVendor_t.TaxNo2,");
            sqlUpdate.AppendLine("DbVendor.BlockDelete = DbVendor_t.BlockDelete,");
            sqlUpdate.AppendLine("DbVendor.BlockPost= DbVendor_t.BlockPost,");
            sqlUpdate.AppendLine("DbVendor.Active = DbVendor_t.Active,");
            sqlUpdate.AppendLine("DbVendor.CreBy = DbVendor_t.CreBy,");
            sqlUpdate.AppendLine("DbVendor.CreDate = DbVendor_t.CreDate,");
            sqlUpdate.AppendLine("DbVendor.UpdBy = DbVendor_t.UpdBy,");
            sqlUpdate.AppendLine("DbVendor.UpdDate = DbVendor_t.UpdDate,");
            sqlUpdate.AppendLine("DbVendor.UpdPgm = DbVendor_t.UpdPgm,");
            sqlUpdate.AppendLine("DbVendor.TaxNo3 = DbVendor_t.TaxNo3,");
            sqlUpdate.AppendLine("DbVendor.TaxNo4 = DbVendor_t.TaxNo4 ");
            sqlUpdate.AppendLine("FROM DbVendor_temp DbVendor_t ");
            sqlUpdate.AppendLine("WHERE DbVendor_t.VendorCode = DbVendor.VendorCode");
            #endregion
            ISQLQuery UpdateQuery = GetCurrentSession().CreateSQLQuery(sqlUpdate.ToString());
            UpdateQuery.AddScalar("result", NHibernateUtil.Int32);
            UpdateQuery.UniqueResult();
      
        }

        #endregion
    }
}
