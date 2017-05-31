using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Security.Cryptography;

namespace SS.Standard.Security.DAL.Mssql
{
    public class UserDAL : Data.Mssql.MssqlHelper, Data.Interfaces.IDBManager,SS.Standard.Security.Mssql.DAL.Interface.IUserDAL
    {
        #region IUserDAL Members


        #region IDBManager Members

        public void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

        }

        public void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }



        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                Transaction = Connection.BeginTransaction();
            }
            else
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {

            Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }

        #endregion

        public UserSession SignIn(string UserName, string Pass)
        {
            bool CheckDate  = false;
            TimeSpan DiffDate;
            int userID = 0;
            string EmployeeName = "";
           // string LastName = "";
            int RoleID = 0;
            int OrganizationID = 0;
            bool Active = false;
            DateTime EndDate = DateTime.MinValue;
            DateTime EffDate = DateTime.MinValue;
            List<UserMenu> menu = new List<UserMenu>();
            int default_lange_id = 0;
            string OrganizationName = "";
            int FailTime = 0;
            SqlCommand cmd;
            string Role;
            UserSession objUser=null;

            cmd = new SqlCommand("SELECT TOP 1 U.UserID , U.UserName  , U.PASSWORD , U.FailTime  ,PLang.FirstName , PLang.LastName, U.OrganizationID , U.LanguageID , U.EffDate , U.EndDate , U.Active, SuUserDivision.DivisionID,R.RoleID FROM SuUser U LEFT OUTER JOIN SuUserDivision ON U.UserID = SuUserDivision.UserID LEFT JOIN SuUserProfile P ON U.UserID = P.UserID LEFT OUTER  JOIN   SuUserProfileLang PLang ON U.UserID = P.UserID LEFT OUTER JOIN   SuUserRole R ON U.UserID = R.UserID  WHERE U.UserName=@UserName AND U.Password = @Pass");
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Pass", Pass);
            IDataReader reader = ExecuteReader(cmd);
           
            if (reader.Read())
            {
        

                userID              =    int.Parse(reader["UserID"].ToString()); //(int)reader["UserID"];
                UserName            =    reader["UserName"] as string;
                EmployeeName           =    reader["EmployeeName"] as string;
               // LastName            =    reader["LastName"] as string;
                RoleID              =    int.Parse(reader["RoleID"].ToString());//(int)reader["RoleID"];
                OrganizationID      =    int.Parse(reader["OrganizationID"].ToString());//(int)reader["OrganizationID"];
                Active              =    (bool)reader["Active"];
                EndDate             =    (DateTime)reader["EndDate"];
                EffDate             =    (DateTime)reader["EffDate"];
                default_lange_id    =    int.Parse(reader["LanguageID"].ToString());// (int)reader["LanguageID"];
                FailTime            =    int.Parse(reader["FailTime"].ToString());//(int)reader["FailTime"];
            }
            reader.Close();

            if (userID == 0)
            {
                // ไม่พบ user
                return objUser;
            }
            else
            {
                // พบ user แล้วทำการ เตรียมเพื่อ creae user session
                
                if (EndDate == DateTime.MinValue && EffDate != DateTime.MinValue)
                {
                    DiffDate = (TimeSpan)(DateTime.Now - EffDate);
                    if (DiffDate.TotalDays >= 0)
                    {
                        CheckDate = true;
                    }
                }
                else if (EndDate != DateTime.MinValue && EffDate == DateTime.MinValue )
                {
                    DiffDate = (DateTime)EndDate - DateTime.Now;
                    if (Math.Floor(DiffDate.TotalDays) >= 0)
                    {
                        CheckDate = true;
                    }
                }
                else
                {
                    DiffDate = (DateTime)EffDate - DateTime.Now;
                    if (DiffDate.TotalDays <= 0)
                    {
                        DiffDate = (DateTime)EndDate - DateTime.Now;
                        if (Math.Floor(DiffDate.TotalDays) >= 0)
                        {
                            CheckDate = true;
                        }
                    }
                }
               
                if (CheckDate == true)
                {
                    DateTime TimeStamp = DateTime.Now;
                    string SessoinId = Md5Hash(userID + TimeStamp.ToString());
                    if (UpdateUserSession(userID, SessoinId, TimeStamp) == 0)
                    {
                        InsertUserSession(userID, SessoinId, TimeStamp);
                    }
                    if (FailTime != 0)
                    {
                        UpdateFail(UserName, 0);
                    }
                    GetRole(userID, out cmd, out Role);
                    GetOUName(ref reader, OrganizationID, default_lange_id, ref OrganizationName, ref cmd);
                    objUser = CreateUserSession(userID, UserName, EmployeeName, RoleID, SessoinId, OrganizationID, OrganizationName, default_lange_id, menu, Active, EndDate, EffDate);
                }
                return objUser;
            }
        }
        private UserSession CreateUserSession(int UserID,string UserName,string EmployeeName,int UserRoleID,string SessionID,int DefaultOuId,string DefaultOuName,int CurrentLangId,List<UserMenu> usrerMenu,bool Active,DateTime endDate,DateTime EffiveDate)
        {
            UserSession obj = new UserSession(UserID, UserName, EmployeeName,  UserRoleID.ToString(), SessionID, DefaultOuId, DefaultOuName, CurrentLangId, usrerMenu, Active, endDate, EffiveDate);
            return obj;
        }
        private string Md5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public void AccountLock(int UserId)
        {
            SqlCommand cmd = new SqlCommand("UPDATE SuUser SET FailTime = 0 , Active = '0' WHERE UserID = @UserId ");
            cmd.Parameters.AddWithValue("@UserId", UserId);
            ExecuteNonQuery(cmd);
        }
        public void UnAccountLock(int UserId)
        {
            SqlCommand cmd = new SqlCommand("UPDATE SuUser SET FailTime = 0 , Active = '1' WHERE UserID = @UserId ");
            cmd.Parameters.AddWithValue("@UserId", UserId);
            ExecuteNonQuery(cmd);
        }
        public void UpdateFail(string UserName, int Num)
        {
            SqlCommand cmd = new SqlCommand();
            if (Num == -1)
            {
                cmd.CommandText = "Update SuUser SET FailTime = (FailTime+1) Where UserName =@UserName";
            }
            else
            {
                cmd.CommandText = "Update SuUser SET FailTime = @FailTime Where UserName =@UserName";
                cmd.Parameters.AddWithValue("@FailTime", Num);
            }
            cmd.Parameters.AddWithValue("@UserName", UserName.Trim());
            ExecuteNonQuery(cmd);
        }

        public void InsertUserSession(int UserId, string SessionId, DateTime TimeStamp)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO SuSession VALUES (  @UserId , @SessionId ,@RoleID, @TimeStamp )");
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@SessionId", SessionId);
            cmd.Parameters.AddWithValue("@RoleID", 1);
            cmd.Parameters.Add("@TimeStamp", SqlDbType.SmallDateTime).Value = TimeStamp;
            ExecuteNonQuery(cmd);
        }

        public int UpdateUserSession(int UserId, string SessionId, DateTime TimeStamp)
        {
            StringBuilder sb = new StringBuilder("UPDATE SuSession SET");
            SqlCommand cmd = new SqlCommand();
            if (SessionId != null)
            {
                sb.AppendFormat(" SessionID = @SessionID");

                cmd.Parameters.AddWithValue("@SessionID", SessionId);
            }
            if (TimeStamp != null)
            {
                if (cmd.Parameters.Count != 0)
                {
                    sb.Append(" , ");
                }
                sb.AppendFormat(" TimeStamp = @TimeStamp");
                cmd.Parameters.Add("@TimeStamp", SqlDbType.SmallDateTime).Value = TimeStamp;
            }
            cmd.CommandText = sb.Append(" WHERE UserID = @UserID").ToString();
            cmd.Parameters.AddWithValue("@UserID", UserId);

            return ExecuteNonQuery(cmd);
        }

        public void DeleteUserSession(int UserId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM SuSession WHERE UserID = @UserId");
            cmd.Parameters.AddWithValue("@UserId", UserId);
            ExecuteNonQuery(cmd);
        }

        public void DeleteUserSession(DateTime TimeStamp)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM SuSession WHERE TimeStamp < @TimeStamp");
            cmd.Parameters.Add("@TimeStamp", SqlDbType.SmallDateTime).Value = TimeStamp;
            ExecuteNonQuery(cmd);
        }

        #endregion

        public UserSession GetUserFromReader(IDataReader reader)
        {
            int userID = 0;
            string UserName = "";
            string EmployeeName = "";
            //string LastName = "";
            int RoleID = 0;
            int OrganizationID = 0;
            bool Active = false;
            DateTime EndDate = DateTime.MinValue;
            DateTime EffDate = DateTime.MinValue;
            List<UserMenu> menu = new List<UserMenu>();
            int default_lange_id = 0;
            string OrganizationName = "";
            int FailTime = 0;

            UserSession obj = null;
            if (reader.Read())
            {
                userID = (int)reader["UserID"];
                UserName = reader["UserName"] as string;
                EmployeeName = reader["EmployeeName"] as string;
                //LastName = reader["LastName"] as string;
                RoleID = (int)reader["RoleID"];
                OrganizationID = (int)reader["OrganizationID"];
                Active = (bool)reader["Active"];
                EndDate = (DateTime)reader["EndDate"];
                EffDate = (DateTime)reader["EffDate"];
                default_lange_id = (int)reader["LanguageID"];
                FailTime = (int)reader["FailTime"];
            }
            reader.Close();
            SqlCommand cmd;
            string Role;
            GetRole(userID, out cmd, out Role);

            GetOUName(ref reader, OrganizationID, default_lange_id, ref OrganizationName, ref cmd);

            obj = new UserSession(userID, UserName, EmployeeName, Role, "1", OrganizationID, OrganizationName,default_lange_id, menu, Active, EndDate, EffDate);

            return obj;
        }

        private void GetOUName(ref IDataReader reader, int OrganizationID, int default_lange_id, ref string OrganizationName, ref SqlCommand cmd)
        {
            //if (default_lange_id.ToString() != System.Web.HttpContext.Current.Application["DefaultLanguageId"].ToString())
            //{
                cmd = new SqlCommand("SELECT OrganizationName FROM SuOrganizationLang WHERE OrganizationID=@OrganizationID AND LanguageID =@LanguageID");
                cmd.Parameters.AddWithValue("@LanguageID", default_lange_id);
            //}
            //else
            //{
            //    cmd = new SqlCommand("SELECT OrganizationName FROM SU_ORGANIZATION WHERE OrganizationID=@OrganizationID");
            //}
            cmd.Parameters.AddWithValue("@OrganizationID", OrganizationID);

            reader = ExecuteReader(cmd);
            while (reader.Read())
            {
                OrganizationName = reader["OrganizationName"] as string;
            }
            reader.Close();
        }

        private void GetRole(int userID, out SqlCommand cmd, out string Role)
        {
            cmd = new SqlCommand("SELECT DISTINCT RoleID FROM SuUserRole WHERE UserID=@UserID");
            cmd.Parameters.AddWithValue("@UserID", userID);
            IDataReader read = ExecuteReader(cmd);
            Role = "";
            while (read.Read())
            {
                Role += read["RoleID"] + ",";
            }
            read.Close();
            if (Role.Length > 0)
            {
                Role = Role.Remove(Role.Length - 1);
            }
            //string[] arrayRole = Role.Split(',');
        }


        public void setLanguage(int languageID)
        {
            IDataReader drReader;
            SqlCommand cmd;
            //if (languageID.ToString().Equals(System.Web.HttpContext.Current.Application["DefaultLanguageId"].ToString()))
            //{
            //    cmd = new SqlCommand("SELECT [FirstName],[LastName] FROM [SuUserProfile] WHERE [UserID] = @UserID");
            //}
            //else
            //{
            //    cmd = new SqlCommand("SELECT [FirstName],[LastName] FROM [SuUserProfile_LANG] WHERE [LanguageID] = @LanguageID AND [UserID] = @UserID");
            //    cmd.Parameters.AddWithValue("@LanguageID", languageID);
            //}
            cmd = new SqlCommand(" SELECT SuUser.EmployeeName        Form SuUser  WHERE SuUser.LanguageID = @LanguageID  AND SuUser.UserID = @UserID");
            cmd.Parameters.AddWithValue("@LanguageID", languageID);
            cmd.Parameters.AddWithValue("@UserID", UserAccount.UserID);
            drReader = ExecuteReader(cmd);
            while (drReader.Read())
            {
                UserAccount.EmployeeName = drReader[0].ToString();
                //UserAccount.LastName = drReader[1].ToString();
                break;
            }
            drReader.Close();
            UserAccount.CURRENT_LanguageID = languageID;
        }
    }
}
