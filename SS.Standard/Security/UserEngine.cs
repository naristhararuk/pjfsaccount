using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using SS.Standard.Data;
using SS.Standard.Security.Mssql.DAL.Interface;
using SS.Standard.Security.DAL.Mssql;
using SS.Standard.Data.Mssql;
using SS.Standard.Data.Enum;



/// <summary>
/// Summary description for UserEngine
/// </summary>
namespace SS.Standard.Security
{
    
    public class UserEngine 
    {
        private static DBManager dbm = new DBManager();


        public static string SignIn(string UserName, string Pass)
        {
            DateTime TimeStamp = DateTime.Now;
            string SessoinId = Md5Hash("1" + TimeStamp.ToString());

            UserSession user = new UserSession(1, "Admin", "Admin", "1", SessoinId,1,"1",1,null,true,DateTime.MaxValue,DateTime.MinValue);
            HttpContext.Current.Session["User"] = user;
            return "success";

            //SessionTimeOut();
            //Provider.UserDAL.OpenConnection();
            //UserSession user = Provider.UserDAL.SignIn(UserName, Md5Hash(Pass));
            //if (user != null)
            //{
            //    if (user.Active)
            //    {
            //        HttpContext.Current.Session["User"] = user;
            //        Provider.UserDAL.CloseConnection();
            //        return "success";
            //    }
            //    else
            //    {
            //        Provider.UserDAL.CloseConnection();
            //        return "Account not Active";
            //    }
            //}
            //else
            //{
            //    Provider.UserDAL.UpdateFail(UserName, -1);
            //    Provider.UserDAL.CloseConnection();
            //    return "WrongPass";

            //}
          
        }

       

       
        public static void setLanguage(int languageID)
        {
            Provider.UserDAL.OpenConnection();
            Provider.UserDAL.setLanguage(languageID);
            Provider.UserDAL.CloseConnection();

        }
        public static void LockAcount(int UserId)
        {
            Provider.UserDAL.AccountLock(UserId);
        }

        public static void UnLockAcount(int UserId)
        {
            Provider.UserDAL.OpenConnection();
            Provider.UserDAL.UnAccountLock(UserId);
            Provider.UserDAL.CloseConnection();
        }
       
        public static void SessionTimeOut()
        {
            Provider.UserDAL.OpenConnection();
            Provider.UserDAL.DeleteUserSession(DateTime.Now.AddMinutes(-double.Parse(ConfigurationManager.AppSettings["SessionTimeOut"])));
            Provider.UserDAL.CloseConnection();
        }

       

        //private static int SessionCheck(int UserId, string SessionId, string Time)
        //{
        //    int flag = -1;
        //    try
        //    {
        //        flag = dbm.ExecuteNonQuery("UPDATE SuSession SET SessionID = '" + SessionId + "' , TimeStamp = '" + Time + "' WHERE UserID = '" + UserId + "'", CommandType.Text);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return flag;
        //}

        private static void SessionUpdate()
        {
            try
            {
             Provider.UserDAL.UpdateUserSession(UserAccount.UserID,UserAccount.SessionID,DateTime.Now); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Int64 Timestamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now - origin;
            return (Int64)Math.Floor(diff.TotalSeconds);
        }

        public static string Md5Hash(string input)
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

        //public static string PassRecover(string UserName, string Email)
        //{
        //    string flag = "False";
        //    try
        //    {
        //        string sql = "SELECT  UserName , User_Role , PASSWORD ,FailTime FROM SuUser WHERE Active = '1' AND UserName='" + UserName + "' , AND E_MAIL = '" + Email + "'";
        //        IDataReader Reader = dbm.ExecuteReader("SELECT UserID , UserName FROM SuUser WHERE  UserName='" + UserName + "'  AND E_MAIL = '" + Email + "'", CommandType.Text);
        //        if (Reader.Read())
        //        {
        //            Random rand = new Random();
        //            string pwd = "" + rand.Next(10000, 99999);
        //            string md5 = Md5Hash(pwd);
        //            dbm.ExecuteNonQuery("UPDATE SuUser SET PASSWD = '" + md5 + "' WHERE UserName = '" + UserName + "' AND E_MAIL = '" + Email + "'", CommandType.Text);
        //            flag = pwd;
        //        }
        //        else
        //        {
        //            flag = "False";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return flag;

        //}

        public static bool Permission(ACL acl , string ProgramCode , bool reDirect)
        {
            return true;
            //bool _Permission = false;
            //StringBuilder sql = new StringBuilder();
            //IDataReader Reader;
            //try
            //{
            //    if (UserAccount.Authentication)
            //    {
            //        if (ProgramCode != "Menu")
            //        {
            //            SessionTimeOut();

            //            Reader = dbm.ExecuteReader("SELECT UserID FROM SuSession WHERE  UserID ='" + UserAccount.UserID + "'  AND SessionID = '" + UserAccount.SessionID + "'", CommandType.Text);
            //            if (Reader.Read())
            //            {
            //                SessionUpdate();
            //                string[] Role = UserAccount.UserRole;
            //                sql.AppendFormat("SELECT COUNT(ProgramCode) FROM SuProgram INNER JOIN SuProgramRole ON SuProgram.ProgramID = SuProgramRole.ProgramID INNER JOIN SuProgramMenu ON SuProgram.ProgramID = SuProgramMenu.ProgramID INNER JOIN SuRole ON SuRole.RoleID = SuProgramRole.RoleID WHERE SuProgram.ProgramCode='{0}'", ProgramCode);
            //                if (Role.Length != 0)
            //                {
            //                    sql.Append(" AND ");
            //                    if (acl.ToString() == "Edit") sql.Append(" SuProgramRole.EditState = 1 AND ");
            //                    else if (acl.ToString() == "Add") sql.Append(" SuProgramRole.AddState = 1 AND ");
            //                    else if (acl.ToString() == "Delete") sql.Append(" SuProgramRole.DeleteState =1 AND ");

            //                    int numRole = Role.Length;
            //                    for (int i = 0; i < numRole; i++)
            //                    {
            //                        if (i == 0) sql.AppendFormat("( SuProgramRole.RoleID = {0} ", Role[i]);
            //                        else sql.AppendFormat(" OR SuProgramRole.RoleID = {0}", Role[i]);
            //                    }
            //                    sql.Append(")");

            //                    if ((int)dbm.ExecuteScalar(sql.ToString(), CommandType.Text) > 0)
            //                    {
            //                        _Permission = true;
            //                    }
            //                }

            //            }
            //            else
            //            {
            //                SignOut();
            //            }
            //        }
            //        else _Permission = true;
                    
            //    }

            //    if (!_Permission && reDirect) HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Application["Permission"].ToString(),true);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
           // return _Permission;

        }

        public static bool Permission(ACL acl , string ProgramCode, string url)
        {
            bool _Permission = false;
            StringBuilder sql = new StringBuilder();
            IDataReader Reader;
            try
            {


                if (UserAccount.Authentication)
                {
                    SessionTimeOut();
                  
                        SessionUpdate();
                        Reader = dbm.ExecuteReader("SELECT UserID FROM SuSession WHERE  UserID ='" + UserAccount.UserID + "'  AND SessionID = '" + UserAccount.SessionID + "'", CommandType.Text);
                        if (Reader.Read())
                        {
                            string[] Role = UserAccount.UserRole;
                            sql.AppendFormat("SELECT COUNT(ProgramCode) FROM SuProgram INNER JOIN SuProgramRole ON SuProgram.ProgramID = SuProgramRole.ProgramID INNER JOIN SuProgramMenu ON SuProgram.ProgramID = SuProgramMenu.ProgramID INNER JOIN SuRole ON SuRole.RoleID = SuProgramRole.RoleID WHERE SuProgram.ProgramCode='{0}'", ProgramCode);
                            if (Role.Length != 0)
                            {
                                sql.Append(" AND ");
                                if (acl.ToString() == "Edit") sql.Append(" SuProgramRole.EditState = 1 AND ");
                                else if (acl.ToString() == "Add") sql.Append(" SuProgramRole.AddState = 1 AND ");
                                else if (acl.ToString() == "Delete") sql.Append(" SuProgramRole.DeleteState =1 AND ");

                                int numRole = Role.Length;
                                for (int i = 0; i < numRole; i++)
                                {
                                    if (i == 0) sql.AppendFormat("( SuProgramRole.RoleID = {0} ", Role[i]);
                                    else sql.AppendFormat(" OR SuProgramRole.RoleID = {0}", Role[i]);
                                }
                                sql.Append(")");

                                if ((int)dbm.ExecuteScalar(sql.ToString(), CommandType.Text) > 0)
                                {
                                    _Permission = true;
                                }
                            }

                        }
                        else
                        {
                            SignOut();
                        }
                   

                }

                if (!_Permission) HttpContext.Current.Server.Transfer(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Permission;


        }

        private static List<UserMenu> MenuData()
        {
            try
            {
            List<UserMenu> DataMenu;
            Provider.MenuDAL.OpenConnection();
            DataMenu = Provider.MenuDAL.GetMenuData();
            Provider.MenuDAL.CloseConnection();
            return DataMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UserMenu(Repeater aa)
        {
            
           // aa.
           //menu.ItemTemplate
            
                    //    <ItemTemplate>
                    //        <asp:HyperLink ID="SamplesLink" runat="server" Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("Url") %>' EnableViewState="false" /><br />
            //             </ItemTemplate>
                    //</asp:Repeater>
            //CustomSiteMapDataSource cMenu = new CustomSiteMapDataSource();
        }
        public class CustomSiteMapDataSource : SiteMapDataSource
        {
            protected override HierarchicalDataSourceView GetHierarchicalView(string viewPath)
            {
                SiteMapNode root = Provider.RootNode;
                SiteMapNode newRoot = root.Clone();
                newRoot.ChildNodes = new SiteMapNodeCollection();
                SiteMapNodeCollection collection = new SiteMapNodeCollection();
                collection.Add(newRoot);
                collection.AddRange(root.ChildNodes);

                return new SiteMapHierarchicalDataSourceView(collection);
            }
        }

        public static void ProgramMenu(Menu menu,List<UserMenu> DataMenu,List<UserMenu> showMenu)
        { 
        
            menu.Items.Clear();

            foreach (UserMenu item in showMenu)
            {
                for (int i = 0; i < DataMenu.Count; i++)
                {
                    if (item.ProgramCode == DataMenu[i].ProgramCode)
                    {
                        menu.Items.Add(new MenuItem(DataMenu[i].ProgramsName, null, null, DataMenu[i].ProgramPath));
                        break;
                    }
                }
            }
                
        }

        public static void ProgramMenu(TreeView menu, List<UserMenu> DataMenu, List<UserMenu> showMenu)
        {

            menu.Nodes.Clear();
            foreach (UserMenu item in showMenu)
            {
                for (int i = 0; i < DataMenu.Count; i++)
                {
                    if (item.ProgramCode == DataMenu[i].ProgramCode)
                    {
                        menu.Nodes.Add(new TreeNode(DataMenu[i].ProgramsName, null, null, DataMenu[i].ProgramPath, "_self"));
                        break;
                    }
                }
            }

          

        }

        public static void UserMenu(Menu menu)
        {

            #region Init

            if (!UserAccount.Authentication) return;

            List<UserMenu> DataMenu = new List<UserMenu>();
            menu.Items.Clear();
            MenuItem Module = new MenuItem();
            MenuItem Group = new MenuItem();
            MenuItem Program = new MenuItem();
           
            /* comment by desh 2008-09-18 not show home in menu
            string home = "Home";
            IDataReader drReader = dbm.ExecuteReader("SELECT b.word FROM su_translate a inner join su_translate_lang b on a.trans_id = b.trans_id WHERE [name] = '$HomeSite$' and b.LanguageID = '" + UserAccount.LanguageID+ "'", CommandType.Text);
            while (drReader.Read())
            {
                home = drReader[0].ToString();
                break;
            }
            drReader.Close();
            
           menu.Items.Add(new MenuItem(home, null, null, System.Web.HttpContext.Current.Application["HomeSite"].ToString()));
             */
            #endregion

            try
            {
                DataMenu = MenuData();  

                //if (HttpContext.Current.Session["User"] != null)
                //{
                //    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                //    if (user.UserMenu.Count == 0)
                //    {
                //        DataMenu = MenuData();  
                //        user.UserMenu = DataMenu;

                //    }
                //    else
                //    {
                //        user.UserMenu = DataMenu;
                //    }

                //}
                //else
                //{
                //    DataMenu = MenuData();  //จะปรับเพิ่มเติม
                //    UserSession user = (UserSession)HttpContext.Current.Session["User"];
                //    user.UserMenu = DataMenu;
                //}

                string menuLevel1 = "";
                string menuLevel2 = "";
                string menuLevel3 = "";
                string ProgramPath = "";
                string HomeURL = "";
                if (System.Web.HttpContext.Current.Application["HomeURL"] == null)
                {
                    HomeURL = System.Web.HttpContext.Current.Request.Url.ToString();
                    System.Web.HttpContext.Current.Application["HomeURL"] = HomeURL;
                }
                else
                {
                    HomeURL = System.Web.HttpContext.Current.Application["HomeURL"].ToString();
                }
                menu.Items.Add(new MenuItem("HOME", null, null, HomeURL));


                for (int i = 0; i < DataMenu.Count; i++)
                {
                    menuLevel3 = DataMenu[i].ProgramsName;
                    ProgramPath = DataMenu[i].ProgramPath;

                    if (menuLevel1 == DataMenu[i].MenuGroupName)
                    {
                        menuLevel1 = DataMenu[i].MenuGroupName;

                        if (menuLevel2 == DataMenu[i].MenuName)
                        {
                            menuLevel2 = DataMenu[i].MenuName;
                            Module.ChildItems.Add(new MenuItem(DataMenu[i].ProgramPath, null, null, DataMenu[i].ProgramPath));
                        }
                        else
                        {
                           
                            Module = new MenuItem(DataMenu[i].MenuName, null, null, HttpContext.Current.Request.Path + "#");
                            Module.ChildItems.Add(new MenuItem(DataMenu[i].ProgramPath, null, null, DataMenu[i].ProgramPath));
                            menuLevel2 = DataMenu[i].MenuName;
                            Group.ChildItems.Add(Module);

                        }

                    }
                    else
                    {
                        
                            Group = new MenuItem(DataMenu[i].MenuGroupName, null, null, HttpContext.Current.Request.Path + "#");
                            Module = new MenuItem(DataMenu[i].MenuName, null, null, HttpContext.Current.Request.Path + "#");
                            Module.ChildItems.Add(new MenuItem(DataMenu[i].ProgramPath, null, null, DataMenu[i].ProgramPath));
                            Group.ChildItems.Add(Module);
                            menu.Items.Add(Group);


                        menuLevel1 = DataMenu[i].MenuGroupName;
                        menuLevel2 = DataMenu[i].MenuName;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UserMenu(TreeView menu)
        {

            #region Init

            if (!UserAccount.Authentication) return;

            List<UserMenu> DataMenu = new List<UserMenu>();
            menu.Nodes.Clear();
            TreeNode Module = new TreeNode();
            TreeNode Group = new TreeNode();
           // menu.Nodes.Add(new TreeNode("Home", null, null, System.Web.HttpContext.Current.Application["HomeSite"].ToString(), "_self"));
            string sGroup = "", sModule = "";
            #endregion

            try
            {
                DataMenu = MenuData(); // จะปรับเพิ่มเติม

                for (int i = 0; i < DataMenu.Count; i++)
                {
                    if (sGroup == DataMenu[i].MenuGroupName)
                    {
                        if (sModule == DataMenu[i].MenuName) Module.ChildNodes.Add(new TreeNode(DataMenu[i].ProgramPath, null, null, DataMenu[i].ProgramPath, "_self"));
                        else
                        {
                            Group.ChildNodes.Add(Module);
                            Module = new TreeNode(DataMenu[i].MenuName, null, null, HttpContext.Current.Request.Path + "#", "_self");
                            Module.ChildNodes.Add(new TreeNode(DataMenu[i].ProgramsName, null, null, DataMenu[i].ProgramPath,"_self "));
                            sModule = DataMenu[i].MenuName;
                        }
                    }
                    else
                    {
                        if (sGroup == "")
                        {
                            Group = new TreeNode(DataMenu[i].MenuGroupName, null, null, HttpContext.Current.Request.Path + "#", "_self");
                            Module = new TreeNode(DataMenu[i].MenuName, null, null, HttpContext.Current.Request.Path + "#", "_self");
                        }
                        else
                        {
                            Group.ChildNodes.Add(Module);
                            menu.Nodes.Add(Module);
                            Group = new TreeNode(DataMenu[i].MenuGroupName, null, null, HttpContext.Current.Request.Path + "#", "_self");
                            Module = new TreeNode(DataMenu[i].MenuName, null, null, HttpContext.Current.Request.Path + "#", "_self");
                        }
                        Module.ChildNodes.Add(new TreeNode(DataMenu[i].ProgramsName, null, null, DataMenu[i].ProgramPath, "_self"));
                        sGroup = DataMenu[i].MenuGroupName;
                        sModule = DataMenu[i].MenuName;
                    }
                }
                Group.ChildNodes.Add(Module);
                menu.Nodes.Add(Group);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void SignOut()
        {
            if (UserAccount.Authentication)
            {
                Provider.UserDAL.OpenConnection();
                //Provider.UserDAL.DeleteUserSession(UserAccount.UserID);
                Provider.UserDAL.CloseConnection();
                HttpContext.Current.Response.Redirect("~/SignIn.aspx", true);
            }
        }

        //public static string Md5Hash(string input)
        //{
        //    MD5 md5Hasher = MD5.Create();
        //    byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
        //    StringBuilder sBuilder = new StringBuilder();
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("x2"));
        //    }
        //    return sBuilder.ToString();
        //}

        //public static void SessionTimeOut()
        //{
        //    Provider.UserDAL.DeleteUserSession(DateTime.Now.AddMinutes(-double.Parse(ConfigurationManager.AppSettings["SessionTimeOut"])));
        //}

        public enum ACL
        {
            View,
            Add,
            Edit,
            Delete
        }



        #region IUserEngine Members

       

        #endregion
    }
}
