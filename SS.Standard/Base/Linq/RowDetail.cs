using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using SS.Standard.Security;

namespace SS.Standard.Data.Linq
{




    public class RowDetail<T>
    {
        private static PropertyInfo[] properties;
        private static string[] Detail = { "UPD_BY", "UPD_DATE", "UPD_PGM", "CRE_BY", "CRE_DATE" };

        static RowDetail()
        {
            properties = new PropertyInfo[Detail.Length];
            for (int i = 0; i < Detail.Length; i++)
            {
                properties[i] = typeof(T).GetProperty(Detail[i]);
            }
        }

        public static void CreDetail(T Obj)
        {
            Object values;
            for (int i = 0; i < properties.Length; i++)
            {

                if (properties[i].Name == "CRE_BY")
                {
                    values = new object();
                    values = UserAccount.UserID;
                    properties[i].SetValue(Obj, (int)values, null);
                }
                else if (properties[i].Name == "CRE_DATE")
                {
                    values = new object();
                    values = DateTime.Now;
                    properties[i].SetValue(Obj, (DateTime)values, null);
                }
                else if (properties[i].Name == "UPD_BY")
                {
                    values = new object();
                    values = UserAccount.UserID;
                    properties[i].SetValue(Obj, (int)values, null);
                }
                else if (properties[i].Name == "UPD_DATE")
                {
                    values = new object();
                    values = DateTime.Now;
                    properties[i].SetValue(Obj, (DateTime)values, null);
                }
                else if (properties[i].Name == "UPD_PGM")
                {
                    values = new object();
                    values = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf("/") + 1).Split('.')[0];
                    properties[i].SetValue(Obj, values as string, null);
                }


            }
        }
        public static void UpdDetail(T Obj)
        {
            Object values;
            for (int i = 0; i < properties.Length; i++)
            {

                if (properties[i].Name == "UPD_BY")
                {
                    values = new object();
                    values = UserAccount.UserID;
                    properties[i].SetValue(Obj, (int)values, null);
                }
                else if (properties[i].Name == "UPD_DATE")
                {
                    values = new object();
                    values = DateTime.Now;
                    properties[i].SetValue(Obj, (DateTime)values, null);
                }
                else if (properties[i].Name == "UPD_PGM")
                {
                    values = new object();
                    values = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf("/") + 1).Split('.')[0];
                    properties[i].SetValue(Obj, values as string, null);
                }


            }
        }


    }

}