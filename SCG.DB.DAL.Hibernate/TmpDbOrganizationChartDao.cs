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
    public partial class TmpDbOrganizationChartDao : NHibernateDaoBase<TmpDbOrganizationchart, long>, ITmpDbOrganizationChartDao
    {
        public TmpDbOrganizationChartDao()
        {
        }

        public void DeleteAll()
        {
        
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("truncate table Tmp_DbOrganizationchart");
            query.AddScalar("result", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void CommitImport()
        {
            using (ISession s = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = s.BeginTransaction())
                {
                    s.Delete(" FROM DbOrganizationchart");
                    ISQLQuery sqlSelectInto = s.CreateSQLQuery("INSERT INTO DbOrganizationChart(CostCenterCode, DepartmentCode ,DepartmentDescriptionEN, ParentDepartmentCode, Active, UpdBy, UpdDate, CreBy, CreDate, UpdPgm,DepartmentDescriptionTH,DepartmentLevelCode,DepartmentLevelName,Relationship,CompanyCode,CompanyNameTH,CostCenterDescription,ManagerPersonID,ManagerName,ManagerPosition,ManagerPositionName,ManagerUserID,ManagerReportToUserID,Sub1BusinessUnitEN,Sub1BusinessUnitTH,Sub1_1BusinessUnitEN,Sub1_1BusinessUnitTH,CompanyEN,CompanyTH,Sub1CompanyEN,Sub1CompanyTH,DivisionNameEN,DivisionNameTH,Sub1DivisionNameEN,Sub1DivisionNameTH,DepartmentNameEN,DepartmentNameTH,SubDepartmentNameEN,SubDepartmentNameTH,SectionNameEN,SectionNameTH,Sub1SectionNameEN,Sub1SectionNameTH,ShiftNameEN,ShiftNameTH,Sub1ShiftNameEN,Sub1ShiftNameTH,OrgIDOfSub1BusinessUnit,OrgIDOfSub1_1BusinessUnit,OrgIDOfCompany,OrgIDOfSub1Company,OrgIDOfDivision,OrgIDOfSub1Division,OrgIDOfDepartment,OrgIDOfSub1Department,OrgIDOfSection,OrgIDOfSub1Section,OrgIDOfShift,OrgIDOfSub1Shift,StartDate,DelimitDate,SystemDate)SELECT CostCenterCode, DepartmentCode ,DepartmentDescriptionEN, ParentDepartmentCode, Active, UpdBy, UpdDate, CreBy, CreDate, UpdPgm,DepartmentDescriptionTH,DepartmentLevelCode,DepartmentLevelName,Relationship,CompanyCode,CompanyNameTH,CostCenterDescription,ManagerPersonID,ManagerName,ManagerPosition,ManagerPositionName,ManagerUserID,ManagerReportToUserID,Sub1BusinessUnitEN,Sub1BusinessUnitTH,Sub1_1BusinessUnitEN,Sub1_1BusinessUnitTH,CompanyEN,CompanyTH,Sub1CompanyEN,Sub1CompanyTH,DivisionNameEN,DivisionNameTH,Sub1DivisionNameEN,Sub1DivisionNameTH,DepartmentNameEN,DepartmentNameTH,SubDepartmentNameEN,SubDepartmentNameTH,SectionNameEN,SectionNameTH,Sub1SectionNameEN,Sub1SectionNameTH,ShiftNameEN,ShiftNameTH,Sub1ShiftNameEN,Sub1ShiftNameTH,OrgIDOfSub1BusinessUnit,OrgIDOfSub1_1BusinessUnit,OrgIDOfCompany,OrgIDOfSub1Company,OrgIDOfDivision,OrgIDOfSub1Division,OrgIDOfDepartment,OrgIDOfSub1Department,OrgIDOfSection,OrgIDOfSub1Section,OrgIDOfShift,OrgIDOfSub1Shift,StartDate,DelimitDate,SystemDate FROM Tmp_DbOrganizationChart");
                    sqlSelectInto.AddScalar("count", NHibernateUtil.Int64).UniqueResult();
                    transaction.Commit();
                }
            }
        }
    }
}
