using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

using SS.Standard.UI;
using SS.SU.Query;
using SS.DB.Query;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class BreadCrumb : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //query program id from currenct program code
            short programID = QueryProvider.SuProgramQuery.FindProgramIDByProgramCode(UserAccount.CurrentProgramCode);
            //find programid for find menu main id
            short menuMainID = QueryProvider.SuMenuQuery.FindMenuMainIDByProgramID(programID);
            //list use keep all su menu
            IList<MenuPath> menuList = new List<MenuPath>();
            //get sumenu follow language
            menuList = QueryProvider.SuMenuQuery.FindAllByLanguage(UserAccount.CurrentLanguageID);
            //declare local variable
            short menuID = 0;
            short menuLevel = 0;
            string stringPath = string.Empty;
            string currentProgramCode = string.Empty;
            string home = string.Empty;
            //keep first path in home 
            var currentHome = from menus in menuList
                                where menus.MenuID == 1
                                select menus;
            if (currentHome != null && (currentHome.Count<MenuPath>() > 0))
                home = currentHome.First<MenuPath>().MenuName;
            //keep current program code
            var currentProgram = from menus in menuList
                                 where menus.ProgramID == programID
                                 select menus;
            if (currentProgram != null && (currentProgram.Count<MenuPath>() > 0))
            {
                if (!currentProgram.First<MenuPath>().MenuName.Equals(home))
                    currentProgramCode = currentProgram.First<MenuPath>().MenuName;
            }
            //loop for select menu name of child to home
            while (menuLevel != 1)
            {
                //select path menu
                var currentMenuID = from menus in menuList
                                    where menus.MenuID == menuMainID
                                    select menus;
                if (currentMenuID != null && (currentMenuID.Count<MenuPath>() > 0))
                {
                    if (!currentProgram.First<MenuPath>().MenuName.Equals(home))//check duplicate menu name 
                    {
                        stringPath += currentMenuID.First<MenuPath>().MenuName + ",";//keep menu follow menu child to master menu
                        menuLevel = currentMenuID.First<MenuPath>().MenuLevel.Value;//for check level ; must be select until root
                        menuMainID = currentMenuID.First<MenuPath>().MenuMainID.Value;
                        menuID = currentMenuID.First<MenuPath>().MenuID.Value;
                    }
                    else
                        menuLevel = 1;
                }
                else
                    menuLevel = 1;
            }
            string pathChild = string.Empty;
            if (stringPath.Length > 0)
            {
                string[] menuChild = stringPath.Split(',');//split string array (keep all child menu)
                
                if (menuChild.Length > 0)
                {
                    for (int i=menuChild.Length;i>0;i--)//for order by menu name from 1 to ....(เพราะตอนที่เราหาค่า menu name ออกมาจะหาจากลูกไปหาแม่ แต่ในการแสดงค่าถ้า order จากแม่ไปหาลูก)
                    {
                        if (menuChild[i - 1] != string.Empty)
                        {
                            if(menuChild[i-1] != menuChild[i])//for check เมนูก่อนหน้า และปัจจุบัน ชื่อเดียวกันหรือไม่
                                pathChild += menuChild[i - 1] + " > ";
                        }
                    }
                }
            }
            if (pathChild.Trim().Length == 0 && currentProgramCode.Trim().Length == 0)//ถ้าไม่มี child ให้ แสดงแค่แม่เท่านั้น
                ctlBreadCrumbText.Text = home;
            else
                ctlBreadCrumbText.Text = home + " > " + pathChild + currentProgramCode;
        }
    }
}