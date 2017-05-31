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

using SS.Standard.Security;

namespace SS.Standard.Security.Implement
{
    [Serializable]
    public partial class MenuEngine : IMenuEngine
    {
        public IMenuEngineService MenuEngineService { get; set; }
        public IUserAccount UserAccount { get; set; }
        private List<UserMenu> DataMenu;
        private List<UserMenu> CurrentMenuData;
        private string applicationMode;

        public void UpdateDataMenu()
        {
            //DataMenu = MenuEngineService.FindAllMenu();
            //HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;

        }


        public void CreateMenu(Menu menu)
        {

            if (!UserAccount.Authentication) return;


            Hashtable hMenu;
            short languageID = UserAccount.CurrentLanguageID;
            if (HttpContext.Current.Session["UserMenu"] != null)
            {
                hMenu = (Hashtable)HttpContext.Current.Session["UserMenu"];
                if (hMenu.ContainsKey(languageID))
                {
                    CurrentMenuData = (List<UserMenu>)hMenu[languageID];
                }
                else
                {
                    CurrentMenuData = SS.SU.Query.QueryProvider.SuMenuQuery.FindAllMenu(UserAccount.UserID, UserAccount.CurrentLanguageID);
                    hMenu.Add(languageID, CurrentMenuData);
                    HttpContext.Current.Session["UserMenu"] = hMenu;

                }
            }
            else
            {
                hMenu = new Hashtable();
                CurrentMenuData = SS.SU.Query.QueryProvider.SuMenuQuery.FindAllMenu(UserAccount.UserID, UserAccount.CurrentLanguageID);
                hMenu.Add(languageID, CurrentMenuData);
                HttpContext.Current.Session["UserMenu"] = hMenu;
            }



            if (CurrentMenuData == null || CurrentMenuData.Count() == 0) return;

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

                        if (applicationMode != "Archive")
                        {
                            ParentItem.ImageUrl = "~/App_Themes/Default/images/MENU.jpg";
                        }
                        else
                        {
                            ParentItem.ImageUrl = "~/App_Themes/Default/images/MENU_Archive.jpg";
                        }

                        if (item.MenuID.Equals(1))
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
                        MenuItem ChildItem = new MenuItem(strEmpty1 + (item.MenuName == null ? "#" : item.MenuName) + strEmpty, item.MenuID.ToString(), null, item.ProgramPath == null ? thisUrl : "~/" + item.ProgramPath);

                        if (item.ProgramPath == "" || item.ProgramPath == null)
                            ChildItem.Selectable = false;

                        hashMenu.Add(item.MenuID, ChildItem);
                        MenuItem ParentItem = hashMenu[item.MenuMainID] as MenuItem;
                        if (ParentItem != null)
                            ParentItem.ChildItems.Add(ChildItem);
                    }
                }
            }
            CleanMenu(menu.Items);
            MenuItem ParentItemHardcode1 = new MenuItem();
            ParentItemHardcode1.ImageUrl = "~/App_Themes/Default/images/MENU.jpg";
            menu.Items.Add(ParentItemHardcode1);
            //newMenu hard code
            MenuItem ParentItemHardcode = new MenuItem();
            if (UserAccount.CurrentLanguageID == 1)
            {
                ParentItemHardcode.Text = "ระบบปฏิบัติงานการจ่ายเงิน";
            }
            else {
                ParentItemHardcode.Text = "Employee Payment Procedure";
            
            }
            ParentItemHardcode.ImageUrl = "~/App_Themes/Default/images/size 50.gif";
            menu.Items.Add(ParentItemHardcode);

            if (UserAccount.CurrentLanguageID == 1)
            {
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-01.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;การเบิกเงินค่าใช้จ่ายของพนักงาน&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-02.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;การเบิกเงินทดรองจ่าย&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-03.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;การเบิกเงินทดรองจ่ายระยะยาว (Fixed Advance)&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-04.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;การเบิกเบี้ยเลี้ยงและค่าใช้จ่ายเดินทาง – ในประเทศ&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-05.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;การเบิกเบี้ยเลี้ยงและค่าใช้จ่ายเดินทาง – ต่างประเทศ&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-06.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;การเบิกชดเชยการใช้รถส่วนตัวของพนักงานในกิจการของบริษัท&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-07.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;วงเงินสดย่อย&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
            }
            else 
            {
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-01.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Employee Expense&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-02.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Advance&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-03.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Fixed Advance&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-04.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Per Diem and Traveling Expense - Domestic&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-05.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Per Diem and Traveling Expense - Foreign&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-06.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Mileage&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty));
                ParentItemHardcode.ChildItems.Add(new MenuItem("<a href='https://exp.scg.co.th/expenseServices/Filelist/Final_Accounting%20Poster-07.pdf' style='text-decoration:none' target = '_blank'>&nbsp;&nbsp;&nbsp;&nbsp;Petty Cash&nbsp;&nbsp;&nbsp;&nbsp;</a>", strEmpty)); 
            }
            
        }

        void CleanMenu(MenuItemCollection items)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                CleanMenu(items[i].ChildItems);
                if (items[i].ChildItems.Count == 0 && !items[i].Selectable)
                {
                    items.RemoveAt(i);
                    i--;
                }
            }
        }

        private List<UserMenu> GetDataMenu()
        {
            /*if (HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] != null)
            {
                DataMenu = (List<UserMenu>)HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()];
            }
            else
            {
                DataMenu = MenuEngineService.FindAllMenu();
                HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;
            }
            return DataMenu;*/
            return null;

            //DataMenu = MenuEngineService.FindAllMenu();
            //HttpContext.Current.Application[ApplicationEnum.WebApplication.DataMenu.ToString()] = DataMenu;
            //return DataMenu;
        }

        public void CreateMenu(TreeView menu)
        {

            if (!UserAccount.Authentication) return;



            Hashtable hMenu;
            short languageID = UserAccount.CurrentLanguageID;
            if (HttpContext.Current.Session["UserMenu"] != null)
            {
                hMenu = (Hashtable)HttpContext.Current.Session["UserMenu"];
                if (hMenu.ContainsKey(languageID))
                {
                    CurrentMenuData = (List<UserMenu>)hMenu[languageID];
                }
                else
                {
                    CurrentMenuData = SS.SU.Query.QueryProvider.SuMenuQuery.FindAllMenu(UserAccount.UserID, UserAccount.CurrentLanguageID);
                    hMenu.Add(languageID, CurrentMenuData);
                    HttpContext.Current.Session["UserMenu"] = hMenu;

                }
            }
            else
            {
                hMenu = new Hashtable();
                CurrentMenuData = SS.SU.Query.QueryProvider.SuMenuQuery.FindAllMenu(UserAccount.UserID, UserAccount.CurrentLanguageID);
                hMenu.Add(languageID, CurrentMenuData);
                HttpContext.Current.Session["UserMenu"] = hMenu;
            }




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
            return null;
            /*
            List<UserRoles> userRoles = UserAccount.UserRole;
            if (userRoles.Count > 0)
            {
                short[] roleFilter = new short[userRoles.Count];
                for (int i = 0; i < userRoles.Count; i++)
                {
                    int index = i - 1;
                    roleFilter.SetValue(userRoles[i].RoleID, i);
                }

                //orderby xMenu.MenuSeq ascending, xMenu.MenuLevel ascending  ,xMenu.MenuMainID ascending

                var CurrentMenuData = from xMenu in DataMenu
                                      where xMenu.MenuLanguageID == UserAccount.CurrentLanguageID && (roleFilter.Contains(xMenu.RoleID.Value) || xMenu.RoleID == 0)
                                      orderby xMenu.MenuMainID ascending, xMenu.MenuLevel ascending, xMenu.MenuSeq ascending
                                      select xMenu;
                return CurrentMenuData;
            }
            else
            {
                return null;
            }
            */
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

        public void SetApplicationMode(string applicationMode)
        {
            this.applicationMode = applicationMode;
        }
    }
}
