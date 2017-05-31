using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using SS.Standard.Security.Mssql.DAL.Interface;

namespace SS.Standard.Security.DAL.Mssql
{
    public class MenuDAL : Data.Mssql.MssqlHelper, Data.Interfaces.IDBManager, IMenuDAL
    {

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

        static string sqlStatement = "SELECT SuMenuGroup.MenuGroupID ,SuMenuGroupLang.MenuGroupName , SuMenuLang.MenuName , SuProgramLang.ProgramsName , SuProgram.ProgramPath  , SuProgram.ProgramCode  FROM SuUserRole INNER JOIN SuUser ON SuUserRole.UserID = SuUser.UserID  INNER JOIN SuProgramRole ON SuUserRole.RoleID = SuProgramRole.RoleID  INNER JOIN SuProgram ON SuProgramRole.ProgramID = SuProgram.ProgramID  INNER JOIN SuRole ON SuUserRole.RoleID = SuRole.RoleID  INNER JOIN SuProgramMenu ON SuProgram.ProgramID = SuProgramMenu.ProgramID  INNER JOIN SuMenu ON SuProgramMenu.MenuID = SuMenu.MenuID  INNER JOIN SuMenuGroup ON SuMenu.MenuGroupID = SuMenuGroup.MenuGroupID  INNER JOIN SuMenuGroupLang ON SuMenuGroup.MenuGroupID = SuMenuGroupLang.MenuGroupID  INNER JOIN SuMenuLang ON SuMenu.MenuID = SuMenuLang.MenuID  INNER JOIN SuProgramLang on SuProgram.ProgramID = SuProgramLang.ProgramID  AND SuMenuGroupLang.LanguageID = @LanguageID1 AND SuMenuLang.LanguageID = @LanguageID2  AND SuProgramLang.LanguageID = @LanguageID3  WHERE SuUser.UserID = @UserID  AND SuProgram.Active = 1  AND SuRole.Active = 1  AND SuProgramRole.DisplayState = 1  ORDER BY SuMenuGroup.MenuGroupID, SuProgram.ProgramCode ";

        public List<UserMenu> GetMenuData()
        {
                List<UserMenu> DataMenu = new List<UserMenu>();
                StringBuilder sql;
                IDataReader drMenu;
                SqlCommand cmd = new SqlCommand();

               // if (UserAccount.CURRENT_LanguageID == UserAccount.LanguageID)
              //  {
                sql = new StringBuilder();
                //sql.Append(" SELECT SuMenusGroup.MenuGroupID ");
                //sql.Append(" , SuMenusGroup.MenuGroupName"); 
                //sql.Append(" , SuProgramMenu.MenuName");  
                //sql.Append(" , SuProgram.ProgramsName ");
                //sql.Append(" , SuProgram.ProgramPath ");
                //sql.Append(" , SuProgram.ProgramCode");  
                //sql.Append(" FROM SuUserRole ");
                //sql.Append(" INNER JOIN SuUser ON SuUser.UserID = SuUserRole.UserID ");
                //sql.Append(" INNER JOIN SuProgramRole  ON SuProgramRole.RoleID = SuUserRole.RoleID "); 
                //sql.Append(" INNER JOIN SuProgram  ON SuProgram.ProgramID = SuProgramRole.ProgramID ");
                //sql.Append(" INNER JOIN SuRole  ON SuRole.RoleID = SuUserRole.RoleID ");
                //sql.Append(" INNER JOIN SuProgramMenu  ON SuProgramMenu.MenuID = SuProgram.MenuID ");
                //sql.Append(" INNER JOIN SuMenusGroup  ON SuMenusGroup.MenuGroupID = SuProgramMenu.MenuGroupID ");
                //sql.Append(" WHERE SuMenusGroup.OrganizationID = @OrganizationID "); 
                //sql.Append(" AND SuUser.UserID = @UserID ");
                //sql.Append(" AND SuProgram.Active = 1 ");
                //sql.Append(" AND SuRole.Active = 1 ");
                //sql.Append(" AND SuProgram.ShowMenu = 1 ");
                //sql.Append(" ORDER BY SuMenusGroup.MenuGroupID, SuProgram.ProgramCode ");
                cmd.CommandText = sqlStatement.ToString();
                //cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("@LanguageID1", UserAccount.CURRENT_LanguageID);
                cmd.Parameters.AddWithValue("@LanguageID2", UserAccount.CURRENT_LanguageID);
                cmd.Parameters.AddWithValue("@LanguageID3", UserAccount.CURRENT_LanguageID);
               // cmd.Parameters.AddWithValue("@OrganizationID", UserAccount.CURRENT_OrganizationID);
                cmd.Parameters.AddWithValue("@UserID", UserAccount.UserID);
                //}
                //else
                //{
                //sql = new StringBuilder();
                //sql.Append(" SELECT SuMenusGroup.MenuGroupID  ");
                //sql.Append(" , SuMenusGroup_LANG.MenuGroupName "); 
                //sql.Append(" , SuProgramMenu_LANG.MenuName ");  
                //sql.Append(" , SuProgramLang.ProgramsName "); 
                //sql.Append(" , SuProgram.ProgramPath  ");
                //sql.Append(" , SuProgram.ProgramCode ");  
                //sql.Append(" FROM SuUserRole ");
                //sql.Append(" INNER JOIN SuUser  ON SuUser.UserID = SuUserRole.UserID  ");
                //sql.Append(" INNER JOIN SuProgramRole  ON SuProgramRole.RoleID = SuUserRole.RoleID ");  
                //sql.Append(" INNER JOIN SuProgram  ON SuProgram.ProgramID = SuProgramRole.ProgramID  ");
                //sql.Append(" INNER JOIN SuRole  ON SuRole.RoleID = SuUserRole.RoleID  ");
                //sql.Append(" INNER JOIN SuProgramMenu  ON SuProgramMenu.MenuID = SuProgram.MenuID  ");
                //sql.Append(" INNER JOIN SuMenusGroup ON SuMenusGroup.MenuGroupID = SuProgramMenu.MenuGroupID  ");
                //sql.Append(" LEFT JOIN SuMenusGroup_LANG  ON SuMenusGroup.MenuGroupID = SuMenusGroup_LANG.MenuGroupID  ");
                //sql.Append(" LEFT JOIN SuProgramMenu_LANG  ON SuProgramMenu.MenuID = SuProgramMenu_LANG.MenuID  ");
                //sql.Append(" LEFT JOIN SuProgramLang  ON SuProgram.ProgramID = SuProgramLang.ProgramID  ");
                //sql.Append(" AND SuProgramLang.LanguageID = @CURRENT_LanguageID1 ");
                //sql.Append(" AND SuProgramMenu_LANG.LanguageID = @CURRENT_LanguageID2 ");
                //sql.Append(" AND SuMenusGroup_LANG.LanguageID = @CURRENT_LanguageID3 ");
                //sql.Append(" WHERE SuMenusGroup.OrganizationID = @OrganizationID ");
                //sql.Append(" AND SuUser.UserID = @UserID ");
                //sql.Append(" AND SuProgram.Active = 1 ");
                //sql.Append(" AND SuRole.Active = 1 ");
                //sql.Append(" AND SuProgram.ShowMenu = 1  ");
                //sql.Append(" ORDER BY SuMenusGroup.MenuGroupID, SuProgram.ProgramCode ");
                //cmd.CommandText = sql.ToString();
                //cmd.Parameters.AddWithValue("@CURRENT_LanguageID1", UserAccount.CURRENT_LanguageID);
                //cmd.Parameters.AddWithValue("@CURRENT_LanguageID2", UserAccount.CURRENT_LanguageID);
                //cmd.Parameters.AddWithValue("@CURRENT_LanguageID3", UserAccount.CURRENT_LanguageID);
                //cmd.Parameters.AddWithValue("@OrganizationID", UserAccount.CURRENT_OrganizationID);
                //cmd.Parameters.AddWithValue("@UserID", UserAccount.UserID);


                //}
                drMenu = ExecuteReader(cmd);
                while (drMenu.Read())
                {
                    UserMenu data = new UserMenu();
                    //Add Module Group (Menu Level 1)
                    if (drMenu["MenuGroupName"] != DBNull.Value)
                    {
                        data.MenuGroupName = drMenu["MenuGroupName"] as string;
                    }
                    else
                    {
                        data.MenuGroupName = "Can't not find context for this language";
                    }

                    //Add Menu Group (Menu Level 2)
                    if (drMenu["MenuName"] != DBNull.Value)
                    {
                        data.MenuName = drMenu["MenuName"] as string;
                    }
                    else
                    {
                        data.MenuName = "Can't not find context for this language";
                    }
                    //Add Program (Menu Level 3)
                    if (drMenu["ProgramsName"] != DBNull.Value)
                    {
                        data.ProgramsName = drMenu["ProgramsName"] as string;
                    }
                    else
                    {
                        data.ProgramsName = "Can't not find context for this language";
                    }
                    data.ProgramCode = drMenu["ProgramCode"] as string;
                    data.ProgramPath = drMenu["ProgramPath"] as string;

                    DataMenu.Add(data);
                }
                return DataMenu;
        }

    }
}
