using System;

using System.Collections;

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Helper;
using System.IO;
using System.Globalization;

namespace SS.Standard.Security.Implement
{ 
    [Serializable]
    public partial class MenuEngine : IMenuEngine
    {
        public IMenuEngineService MenuEngineService { get; set; }
        public IUserAccount UserAccount { get; set; }
        private List<UserMenu> DataMenu;

        public void UpdateDataMenu()
        {
            DataMenu = MenuEngineService.FindAllMenu();
            HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;

        }


        public void CreateMenu(Menu menu)
        {

            if (!UserAccount.Authentication) return;

            List<UserMenu> DataMenu = GetDataMenu();

            var CurrentMenuData = SortDataMenu();
            if (CurrentMenuData.Count() == 0)
            {
                UpdateDataMenu();
                CurrentMenuData = SortDataMenu();
            }

            menu.MaximumDynamicDisplayLevels = CurrentMenuData.Count();
            

            Hashtable hashMenu = new Hashtable();
            menu.Items.Clear();

            string strEmpty = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            string strEmpty1 = "&nbsp;&nbsp;";
            string thisUrl = "";
            if (System.Web.HttpContext.Current.Request.QueryString.Count > 0)
                thisUrl = System.Web.HttpContext.Current.Request.Url.AbsolutePath + "?" + System.Web.HttpContext.Current.Request.QueryString;
            else
                thisUrl = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            foreach (UserMenu item in CurrentMenuData)
            {
                if (item.MenuMainID == item.MenuID)
                {
                    if (!hashMenu.ContainsKey(item.MenuID))
                    {
                        // Old Code
                        //MenuItem ParentItem = new MenuItem(item.MenuName == null ? "#" : item.MenuName, item.MenuID.ToString(), null, item.ProgramPath == null ? "#" : "~/" + item.ProgramPath);
                        //hashMenu.Add(item.MenuID, ParentItem);
                        //menu.Items.Add(ParentItem);
                        
                        // Edit By Kla
                        MenuItem ParentItem = new MenuItem();
                        ParentItem.Text = (item.MenuName == null ? "#" : item.MenuName) + strEmpty;
                        ParentItem.Value = item.MenuID.ToString();
                        ParentItem.ImageUrl = "~/App_Themes/Default/images/MENU.jpg";

                        if (item.MenuName.ToUpper() == "HOME")
                        {
                            ParentItem.Selectable = true;
                            ParentItem.NavigateUrl = "~/Menu.aspx";
                        }
                        else
                        {
                            ParentItem.Selectable = false;
                            ParentItem.NavigateUrl = item.ProgramPath == null ? thisUrl : "~/" + item.ProgramPath;
                        }
                        hashMenu.Add(item.MenuID, ParentItem);
                        menu.Items.Add(ParentItem);
                    }
                }
                else
                {
                    if (!hashMenu.ContainsKey(item.MenuID))
                    {
                        MenuItem ChildItem = new MenuItem( strEmpty1 + (item.MenuName == null ? "#" : item.MenuName) + strEmpty, item.MenuID.ToString(), null, item.ProgramPath == null ? thisUrl : "~/" + item.ProgramPath);

                        if (item.ProgramPath == "" || item.ProgramPath==null)
                            ChildItem.Selectable = false;

                        hashMenu.Add(item.MenuID, ChildItem);
                        MenuItem ParentItem = hashMenu[item.MenuMainID] as MenuItem;
                        ParentItem.ChildItems.Add(ChildItem);
                    }
                }
            }
            
                        
                       
              

        }

        private List<UserMenu> GetDataMenu()
        {
            if (HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] != null)
            {
                DataMenu = (List<UserMenu>)HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()];
            }
            else
            {
                DataMenu = MenuEngineService.FindAllMenu();
                HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;
            }
            return DataMenu;

            //DataMenu = MenuEngineService.FindAllMenu();
            //HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;
            //return DataMenu;
        }

        public void CreateMenu(TreeView menu)
        {

            if (!UserAccount.Authentication) return;


            if (HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] != null)
            {
                DataMenu = (List<UserMenu>)HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()];
            }
            else
            {
                DataMenu = MenuEngineService.FindAllMenu();
                HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;

            }

            var CurrentMenuData = SortDataMenu();

            
            Hashtable hashMenu = new Hashtable();
            menu.Nodes.Clear();
            foreach (UserMenu item in CurrentMenuData)
            {
                if (item.MenuMainID == item.MenuID)
                {
                    if (!hashMenu.ContainsKey(item.MenuID))
                    {

                        TreeNode ParentNode = new TreeNode(item.MenuName == null ? "#" : item.MenuName, item.MenuID.ToString(), null, item.ProgramPath == null ? "#" : "~/" + item.ProgramPath, null);
                        hashMenu.Add(item.MenuID, ParentNode);
                        menu.Nodes.Add(ParentNode);
                    }
                }
                else
                {
                    if (!hashMenu.ContainsKey(item.MenuID))
                    {
                        TreeNode ChildNode = new TreeNode(item.MenuName == null ? "#" : item.MenuName, item.MenuID.ToString(), null, item.ProgramPath == null ? "#" : "~/" + item.ProgramPath, null);
                        hashMenu.Add(item.MenuID, ChildNode);
                        TreeNode ParentItem = hashMenu[item.MenuMainID] as TreeNode;
                        ParentItem.ChildNodes.Add(ChildNode);
                    }
                }
            }




        }

        private IOrderedEnumerable<UserMenu> SortDataMenu()
        {
            var CurrentMenuData = from xMenu in DataMenu
                                  where xMenu.MenuLanguageID == UserAccount.CurrentLanguageID 
                                  //orderby xMenu.MenuSeq ascending, xMenu.MenuLevel ascending  ,xMenu.MenuMainID ascending
                                  orderby xMenu.MenuMainID ascending,xMenu.MenuLevel ascending, xMenu.MenuSeq ascending
                                  select xMenu;
            return CurrentMenuData;

        }

        public static void WriteError(string errorMessage)
        {
            try
            {
                string path = "~/Error/" + DateTime.Today.ToString("dd-mm-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                                  ". Error Message:" + errorMessage;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }

        }

      
    }
}
