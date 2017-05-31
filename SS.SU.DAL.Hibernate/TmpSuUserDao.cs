using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NHibernate;
using NHibernate.Expression;
using NHibernate.Transform;

using SS.Standard.Data.NHibernate.Dao;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using SCG.DB.DTO;
using SS.DB.Query;



namespace SS.SU.DAL.Hibernate
{
    public partial class TmpSuUserDao : NHibernateDaoBase<TmpSuUser, long>, ITmpSuUserDao
    {
        
        #region ITmpSuUserDao Members
        
        public void DeleteAll()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("truncate table Tmp_SuUser");
            query.AddScalar("result", NHibernateUtil.Int32);
            query.UniqueResult();
        }
        public long FindCostCenter(string costCenter)
        {
            IList<DbCostCenter> list = GetCurrentSession().CreateQuery("from DbCostCenter where CostCenterCode = :CostCenterCode").SetString("CostCenterCode", costCenter).List<DbCostCenter>();
            if (list.Count > 0)
                return list[0].CostCenterID;
            else
                return 0;
        }

        //public long? FindSuperVisor(string userName)
        //{
        //    IList<SuUser> list = GetCurrentSession().CreateQuery("from SuUser where UserName = :UserName").SetString("UserName", userName).List<SuUser>();
        //    if (list.Count > 0)
        //        return list[0].Userid;
        //    else
        //        return null;
        //}

        public long FindLocation(string location)
        {
            IList<DbLocationLang> list = GetCurrentSession().CreateQuery("from DbLocationLang where LocationName = :LocationName").SetString("LocationName", location).List<DbLocationLang>();
            if (list.Count > 0)
                return list[0].LocationID.LocationID;
            else
                return 0;
        }

        public IList<NewUserEmail> FindNewUser()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("SELECT SuUser.UserID AS UserID, Tmp_SuUser.OldPassword AS Password FROM SuUser INNER JOIN Tmp_SuUser ON SuUser.PeopleID = Tmp_SuUser.PeopleID AND SuUser.UserName = Tmp_SuUser.UserName WHERE Tmp_SuUser.isNewUser = 'true'");
            query.AddScalar("UserID", NHibernateUtil.Int64);
            query.AddScalar("Password", NHibernateUtil.String);
            IList<NewUserEmail> list = query.SetResultTransformer(Transformers.AliasToBean(typeof(NewUserEmail))).List<NewUserEmail>();
            return list;
        }

        public void BeforeCommit()
        {
            //ID Costcenter Location Company merge to tmp_SuUser
            GetCurrentSession().CreateSQLQuery("UPDATE Tmp_SuUser SET Tmp_SuUser.CompanyID = DbCompany.CompanyID, Tmp_SuUser.LocationID = DbLocation.LocationID, Tmp_SuUser.Supervisor = SuUser.UserID  FROM Tmp_SuUser LEFT OUTER JOIN DbCompany ON Tmp_SuUser.CompanyCode = DbCompany.CompanyCode LEFT OUTER JOIN DbLocation ON Tmp_SuUser.LocationCode = DbLocation.LocationCode LEFT OUTER JOIN SuUser ON Tmp_SuUser.SupervisorName = SuUser.UserName AND SuUser.Active = 'true' ").AddScalar("count", NHibernateUtil.Int32).UniqueResult();

            // Set CostCenter and CostCenter ID
            GetCurrentSession().CreateSQLQuery("UPDATE tmp_SuUser set SelectedCostCenterID = (case when t.CompanyCode = c1.CompanyCode then c1.CostCenterId when t.CompanyCode = c2.CompanyCode then c2.CostCenterId else null end), SelectedCostCenterCode = (case when t.CompanyCode = c1.CompanyCode then c1.CostCenterCode when t.CompanyCode = c2.CompanyCode then c2.CostCenterCode else null end) from tmp_SuUser t left join DbCostCenter c1 on t.CostCenterCode = c1.CostCenterCode left join DbCostCenter c2 on t.PayrollCostCenter = c2.CostCenterCode").AddScalar("count", NHibernateUtil.Int32).UniqueResult(); 

            //Delete CompanyID = null OR AllowFromeHR != true
            GetCurrentSession().CreateSQLQuery("DELETE FROM Tmp_SuUser WHERE (CompanyCode NOT IN (SELECT CompanyCode FROM DbCompany WHERE AllowImportUserFromEHr = 'True')) OR (CompanyCode IS NULL)").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
            GetCurrentSession().CreateSQLQuery("UPDATE tmp_SuUser SET isNewUser = 'true' WHERE PeopleID NOT IN (SELECT PeopleID FROM SuUser)").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
        }

        public void CommitImport(int byId, int RoleId)
        {
            string IsAdUser = "0";
            if (ParameterServices.EnableLoginWithActiveDirectory)
            {
                IsAdUser = "1";
            }
            using (ISession s = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = s.BeginTransaction())
                {
                    s.CreateSQLQuery("UPDATE SuUser SET Active = 'false',UpdBy = " + byId + ", UpdDate = { fn NOW() } , UpdPgm = 'UserImport' WHERE FromEHr = 'true'")
                        .AddScalar("count", NHibernateUtil.Int32).UniqueResult();// Change to FromEHr = true Only.
                    s.CreateSQLQuery("INSERT INTO SuUser(LanguageID, EmployeeCode, CompanyID, CompanyCode, CostCenterID, CostCenterCode, LocationID,LocationCode, UserName, Password, SetFailTime, FailTime, ChangePassword, AllowPasswordChangeDate, PasswordExpiryDate, PeopleID, EmployeeName,SectionName ,PersonalLevel, PersonalDescription , PersonalGroup, PersonalLevelGroupDescription, PositionName, Supervisor, PhoneNo, HireDate, TerminateDate, Email, FromEHr, Active, UpdBy, UpdDate, CreBy, CreDate, UpdPgm,ApprovalFlag, IsAdUser, VendorCode) SELECT 1, EmployeeCode, CompanyID, CompanyCode, SelectedCostCenterID, SelectedCostCenterCode, LocationID,LocationCode, UserName, Password, " + SS.DB.Query.ParameterServices.DefaultLoginFailTime + ", 0, 'true', { fn NOW() }, PasswordExpiryDate, PeopleID, EmployeeName,SectionName,PersonalLevel, PersonalDescription , PersonalGroup, PersonalLevelGroupDescription, PositionName, Supervisor, PhoneNo, HireDate, TerminateDate, Email,FromEHr, Active, " + byId + ", { fn NOW() }, " + byId + ", { fn NOW() }, 'UserImport', ApprovalFlag, '" + IsAdUser + "', VendorCode FROM tmp_SuUser WHERE isNewUser = 'true'")
                        .AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    transaction.Commit();
                }
            } 
            using (ISession s = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = s.BeginTransaction())
                {
                    transaction.Begin();
                    s.CreateSQLQuery("UPDATE SuUser SET SuUser.EmployeeCode = tmp_SuUser.EmployeeCode, SuUser.CompanyID = tmp_SuUser.CompanyID ,SuUser.CompanyCode = tmp_SuUser.CompanyCode, SuUser.CostCenterID = tmp_SuUser.SelectedCostCenterID, SuUser.CostCenterCode = tmp_SuUser.SelectedCostCenterCode, SuUser.LocationID = tmp_SuUser.LocationID,SuUser.LocationCode = tmp_SuUser.LocationCode, SuUser.UserName = tmp_SuUser.UserName, SuUser.PeopleID = tmp_SuUser.PeopleID, SuUser.EmployeeName = tmp_SuUser.EmployeeName, SuUser.SectionName = tmp_SuUser.SectionName, SuUser.PersonalLevel = tmp_SuUser.PersonalLevel, SuUser.PersonalDescription = tmp_SuUser.PersonalDescription , SuUser.PersonalGroup = tmp_SuUser.PersonalGroup, SuUser.PersonalLevelGroupDescription = tmp_SuUser.PersonalLevelGroupDescription, SuUser.PositionName = tmp_SuUser.PositionName, SuUser.Supervisor = tmp_SuUser.Supervisor, SuUser.PhoneNo = tmp_SuUser.PhoneNo, SuUser.HireDate = tmp_SuUser.HireDate, SuUser.TerminateDate = tmp_SuUser.TerminateDate, SuUser.Email = tmp_SuUser.Email, SuUser.FromEHr = tmp_SuUser.FromEHr, SuUser.Active = tmp_SuUser.Active, SuUser.UpdBy = " + byId + ", SuUser.UpdDate = { fn NOW() } , UpdPgm = 'UserImport', SuUser.VendorCode = (CASE WHEN LEFT(tmp_SuUser.VendorCode, 3) = '005' THEN tmp_SuUser.VendorCode ELSE SuUser.VendorCode END) FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    s.CreateSQLQuery("UPDATE Tmp_SuUser SET Tmp_SuUser.Supervisor = SuUser.UserID  FROM Tmp_SuUser LEFT OUTER JOIN SuUser ON Tmp_SuUser.SupervisorName = SuUser.UserName AND SuUser.Active = 'true' ").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    s.CreateSQLQuery("UPDATE SuUser SET SuUser.Supervisor = tmp_SuUser.Supervisor, SuUser.UpdBy = " + byId + ", SuUser.UpdDate = { fn NOW() } , UpdPgm = 'UserImport' FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    //s.CreateSQLQuery("UPDATE SuUser SET SuUser.EmployeeCode = tmp_SuUser.EmployeeCode, SuUser.CompanyCode = tmp_SuUser.CompanyCode, SuUser.CostCenterID = tmp_SuUser.CostCenterID, SuUser.CostCenterCode = tmp_SuUser.CostCenterCode, SuUser.LocationID = tmp_SuUser.LocationID, SuUser.UserName = tmp_SuUser.UserName, SuUser.PeopleID = tmp_SuUser.PeopleID, SuUser.EmployeeName = tmp_SuUser.EmployeeName, SuUser.SectionName = tmp_SuUser.SectionName, SuUser.PersonalLevel = tmp_SuUser.PersonalLevel, SuUser.PersonalDescription = tmp_SuUser.PersonalDescription , SuUser.PersonalGroup = tmp_SuUser.PersonalGroup, SuUser.PersonalLevelGroupDescription = tmp_SuUser.PersonalLevelGroupDescription, SuUser.PositionName = tmp_SuUser.PositionName, SuUser.Supervisor = tmp_SuUser.Supervisor, SuUser.PhoneNo = tmp_SuUser.PhoneNo, SuUser.HireDate = tmp_SuUser.HireDate, SuUser.TerminateDate = tmp_SuUser.TerminateDate, SuUser.Email = tmp_SuUser.Email, SuUser.FromEHr = tmp_SuUser.FromEHr, SuUser.Active = tmp_SuUser.Active FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID WHERE SuUser.UserName <> tmp_SuUser.UserName").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    s.CreateSQLQuery("INSERT INTO SuUserRole(RoleID, UserID, UpdBy, UpdDate, CreBy, CreDate, UpdPgm, Active) SELECT " + RoleId + ",  UserID, " + byId + ", { fn NOW() }, " + byId + ", { fn NOW() }, 'UserImport', 'true' FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID AND SuUser.UserName = tmp_SuUser.UserName WHERE tmp_SuUser.isNewUser = 'true'").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    transaction.Commit();

                }
            }
        }

        public void AfterCommit(int byId)
        {
            GetCurrentSession().CreateSQLQuery("INSERT INTO SuEHrProfileLog(PeopleID, UserName, Message, Active, CreBy, CreDate, UpdBy, UpdDate, UpdPgm) SELECT PeopleID, UserName, 'Missing Location Record at line ' + CAST(LineNumber AS varchar(20)), Active, " + byId + ", { fn NOW() }, " + byId + ", { fn NOW() }, 'UserImport' FROM Tmp_SuUser WHERE LocationID is NULL").AddScalar("Count", NHibernateUtil.Int32).UniqueResult();
            GetCurrentSession().CreateSQLQuery("INSERT INTO SuEHrProfileLog(PeopleID, UserName, Message, Active, CreBy, CreDate, UpdBy, UpdDate, UpdPgm) SELECT PeopleID, UserName, 'Missing CostCenter Record at line ' + CAST(LineNumber AS varchar(20)), Active, " + byId + ", { fn NOW() }, " + byId + ", { fn NOW() }, 'UserImport' FROM Tmp_SuUser WHERE CostCenterID is NULL").AddScalar("Count", NHibernateUtil.Int32).UniqueResult();

            //Auto sync all user data
            GetCurrentSession().CreateSQLQuery("exec [dbo].[SyncAllUserData]").AddScalar("Count", NHibernateUtil.Int32).UniqueResult();
        }

        public void CommitManualImport(int byId, int RoleId)
        {
            using (ISession s = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = s.BeginTransaction())
                {
                    s.CreateSQLQuery("INSERT INTO SuUser(LanguageID, EmployeeCode, CompanyID, CompanyCode, CostCenterID, CostCenterCode, LocationID, UserName, Password, SetFailTime, FailTime, ChangePassword, AllowPasswordChangeDate, PasswordExpiryDate, PeopleID, EmployeeName,SectionName ,PersonalLevel, PersonalDescription , PersonalGroup, PersonalLevelGroupDescription, PositionName, Supervisor, PhoneNo, HireDate, TerminateDate, Email, FromEHr, Active, UpdBy, UpdDate, CreBy, CreDate, UpdPgm,ApprovalFlag, IsAdUser, VendorCode) SELECT 1, EmployeeCode, CompanyID, CompanyCode, SelectedCostCenterID, SelectedCostCenterCode, LocationID, UserName, Password, " + SS.DB.Query.ParameterServices.DefaultLoginFailTime + ", 0, 'true', { fn NOW() }, PasswordExpiryDate, PeopleID, EmployeeName,SectionName,PersonalLevel, PersonalDescription , PersonalGroup, PersonalLevelGroupDescription, PositionName, Supervisor, PhoneNo, HireDate, TerminateDate, Email,FromEHr, Active, " + byId + ", { fn NOW() }, " + byId + ", { fn NOW() }, 'UserImport', ApprovalFlag, isAdUser, VendorCode FROM tmp_SuUser WHERE isNewUser = 'true'")
                        .AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    transaction.Commit();
                }
            }
            using (ISession s = SessionFactory.OpenSession())
            {
                using (ITransaction transaction = s.BeginTransaction())
                {
                    transaction.Begin();
                    s.CreateSQLQuery("UPDATE Tmp_SuUser SET Tmp_SuUser.Supervisor = SuUser.UserID  FROM Tmp_SuUser LEFT OUTER JOIN SuUser ON Tmp_SuUser.SupervisorName = SuUser.UserName AND SuUser.Active = 'true' ").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    s.CreateSQLQuery("UPDATE SuUser SET SuUser.EmployeeCode = tmp_SuUser.EmployeeCode, SuUser.CompanyID = tmp_SuUser.CompanyID ,SuUser.CompanyCode = tmp_SuUser.CompanyCode, SuUser.CostCenterID = tmp_SuUser.SelectedCostCenterID, SuUser.CostCenterCode = tmp_SuUser.SelectedCostCenterCode, SuUser.LocationID = tmp_SuUser.LocationID, SuUser.UserName = tmp_SuUser.UserName, SuUser.PeopleID = tmp_SuUser.PeopleID, SuUser.EmployeeName = tmp_SuUser.EmployeeName, SuUser.SectionName = tmp_SuUser.SectionName, SuUser.PersonalLevel = tmp_SuUser.PersonalLevel, SuUser.PersonalDescription = tmp_SuUser.PersonalDescription , SuUser.PersonalGroup = tmp_SuUser.PersonalGroup, SuUser.PersonalLevelGroupDescription = tmp_SuUser.PersonalLevelGroupDescription, SuUser.PositionName = tmp_SuUser.PositionName, SuUser.Supervisor = tmp_SuUser.Supervisor, SuUser.PhoneNo = tmp_SuUser.PhoneNo, SuUser.HireDate = tmp_SuUser.HireDate, SuUser.TerminateDate = tmp_SuUser.TerminateDate, SuUser.Email = tmp_SuUser.Email, SuUser.FromEHr = tmp_SuUser.FromEHr, SuUser.Active = tmp_SuUser.Active, SuUser.IsAdUser = tmp_SuUser.IsAdUser , SuUser.UpdBy = " + byId + ", SuUser.UpdDate = { fn NOW() } , UpdPgm = 'UserImport', SuUser.VendorCode = tmp_SuUser.VendorCode FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    //s.CreateSQLQuery("UPDATE SuUser SET SuUser.EmployeeCode = tmp_SuUser.EmployeeCode, SuUser.CompanyCode = tmp_SuUser.CompanyCode, SuUser.CostCenterID = tmp_SuUser.CostCenterID, SuUser.CostCenterCode = tmp_SuUser.CostCenterCode, SuUser.LocationID = tmp_SuUser.LocationID, SuUser.UserName = tmp_SuUser.UserName, SuUser.PeopleID = tmp_SuUser.PeopleID, SuUser.EmployeeName = tmp_SuUser.EmployeeName, SuUser.SectionName = tmp_SuUser.SectionName, SuUser.PersonalLevel = tmp_SuUser.PersonalLevel, SuUser.PersonalDescription = tmp_SuUser.PersonalDescription , SuUser.PersonalGroup = tmp_SuUser.PersonalGroup, SuUser.PersonalLevelGroupDescription = tmp_SuUser.PersonalLevelGroupDescription, SuUser.PositionName = tmp_SuUser.PositionName, SuUser.Supervisor = tmp_SuUser.Supervisor, SuUser.PhoneNo = tmp_SuUser.PhoneNo, SuUser.HireDate = tmp_SuUser.HireDate, SuUser.TerminateDate = tmp_SuUser.TerminateDate, SuUser.Email = tmp_SuUser.Email, SuUser.FromEHr = tmp_SuUser.FromEHr, SuUser.Active = tmp_SuUser.Active FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID WHERE SuUser.UserName <> tmp_SuUser.UserName").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    s.CreateSQLQuery("INSERT INTO SuUserRole(RoleID, UserID, UpdBy, UpdDate, CreBy, CreDate, UpdPgm, Active) SELECT " + RoleId + ",  UserID, " + byId + ", { fn NOW() }, " + byId + ", { fn NOW() }, 'UserImport', 'true' FROM SuUser INNER JOIN tmp_SuUser ON SuUser.PeopleID = tmp_SuUser.PeopleID AND SuUser.UserName = tmp_SuUser.UserName WHERE tmp_SuUser.isNewUser = 'true'").AddScalar("count", NHibernateUtil.Int32).UniqueResult();
                    transaction.Commit();

                }
            }
        }

        #endregion
    }
}
