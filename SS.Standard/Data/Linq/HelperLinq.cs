using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SS.Standard.Security;

namespace SS.Standard.Data.Linq
{
    public class HelperLinq 
    {
        public static int UserID
        {
            get { return UserAccount.UserID; }
        }

        public static string Pgm
        {
            get { return HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf("/") + 1).Split('.')[0]; }
        }


        #region Create and update Detail Gateway

        public static void ModeDetail(char Mode , Object Obj)
        {
            if (Mode == 'u')
            {
                if (Obj as SU_LANG != null) UpdDetail(Obj as SU_LANG);

                else if (Obj as SU_ALERT_GROUP != null) UpdDetail(Obj as SU_ALERT_GROUP);
                else if (Obj as SU_ALERT_GROUP_LANG != null) UpdDetail(Obj as SU_ALERT_GROUP_LANG);
                else if (Obj as SU_ALERT != null) UpdDetail(Obj as SU_ALERT);
                else if (Obj as SU_ALERT_LANG != null) UpdDetail(Obj as SU_ALERT_LANG);

                else if (Obj as SU_ORGANIZATION != null) UpdDetail(Obj as SU_ORGANIZATION);
                else if (Obj as SU_ORGANIZATION_LANG != null) UpdDetail(Obj as SU_ORGANIZATION_LANG);

                else if (Obj as SU_PROGRAM != null) UpdDetail(Obj as SU_PROGRAM);
                else if (Obj as SU_PROGRAM_LANG != null) UpdDetail(Obj as SU_PROGRAM_LANG);

                else if (Obj as SU_MODULE_GROUP != null) UpdDetail(Obj as SU_MODULE_GROUP);
                else if (Obj as SU_MODULE_GROUP_LANG != null) UpdDetail(Obj as SU_MODULE_GROUP_LANG);
                else if (Obj as SU_MODULE_PGM != null) UpdDetail(Obj as SU_MODULE_PGM);
                else if (Obj as SU_MODULE_PGM_LANG != null) UpdDetail(Obj as SU_MODULE_PGM_LANG);

                else if (Obj as SU_ROLE != null) UpdDetail(Obj as SU_ROLE);
                else if (Obj as SU_ROLE_LANG != null) UpdDetail(Obj as SU_ROLE_LANG);
                else if (Obj as SU_ROLE_GROUP != null) UpdDetail(Obj as SU_ROLE_GROUP);
                else if (Obj as SU_ROLE_GROUP_LANG != null) UpdDetail(Obj as SU_ROLE_GROUP_LANG);
                else if (Obj as SU_ROLE_PGM != null) UpdDetail(Obj as SU_ROLE_PGM);

                else if (Obj as SU_TRANSLATE != null) UpdDetail(Obj as SU_TRANSLATE);
                else if (Obj as SU_TRANSLATE_LANG != null) UpdDetail(Obj as SU_TRANSLATE_LANG);
                else if (Obj as SU_TRANSLATE_PGM != null) UpdDetail(Obj as SU_TRANSLATE_PGM);
                else if (Obj as SU_TRANSLATE_PGM_LANG != null) UpdDetail(Obj as SU_TRANSLATE_PGM_LANG);

                else if (Obj as SU_DIV_TYPE != null) UpdDetail(Obj as SU_DIV_TYPE);
                else if (Obj as SU_DIV_TYPE_LANG != null) UpdDetail(Obj as SU_DIV_TYPE_LANG);
                else if (Obj as SU_DIVISION != null) UpdDetail(Obj as SU_DIVISION);
                else if (Obj as SU_DIVISION_LANG != null) UpdDetail(Obj as SU_DIVISION_LANG);

                else if (Obj as SU_USER != null) UpdDetail(Obj as SU_USER);
                else if (Obj as SU_USER_DIV != null) UpdDetail(Obj as SU_USER_DIV);
                else if (Obj as SU_USER_PROFILE != null) UpdDetail(Obj as SU_USER_PROFILE);
                else if (Obj as SU_USER_PROFILE_LANG != null) UpdDetail(Obj as SU_USER_PROFILE_LANG);
                else if (Obj as SU_USER_ROLE != null) UpdDetail(Obj as SU_USER_ROLE);

               // else if (Obj as SU_LOG_TYPE != null) UpdDetail(Obj as SU_LOG_TYPE);

            }
            else if (Mode == 'c')
            {
                if (Obj as SU_LANG != null) CreDetail(Obj as SU_LANG);
                else if (Obj as SU_ALERT_GROUP != null) CreDetail(Obj as SU_ALERT_GROUP);
                else if (Obj as SU_ALERT_GROUP_LANG != null) CreDetail(Obj as SU_ALERT_GROUP_LANG);
                else if (Obj as SU_ALERT != null) CreDetail(Obj as SU_ALERT);
                else if (Obj as SU_ALERT_LANG != null) CreDetail(Obj as SU_ALERT_LANG);

                else if (Obj as SU_ORGANIZATION != null) CreDetail(Obj as SU_ORGANIZATION);
                else if (Obj as SU_ORGANIZATION_LANG != null) CreDetail(Obj as SU_ORGANIZATION_LANG);

                else if (Obj as SU_MODULE_GROUP != null) CreDetail(Obj as SU_MODULE_GROUP);
                else if (Obj as SU_MODULE_GROUP_LANG != null) CreDetail(Obj as SU_MODULE_GROUP_LANG);
                else if (Obj as SU_MODULE_PGM != null) CreDetail(Obj as SU_MODULE_PGM);
                else if (Obj as SU_MODULE_PGM_LANG != null) CreDetail(Obj as SU_MODULE_PGM_LANG);

                else if (Obj as SU_PROGRAM != null) CreDetail(Obj as SU_PROGRAM);
                else if (Obj as SU_PROGRAM_LANG != null) CreDetail(Obj as SU_PROGRAM_LANG);

                else if (Obj as SU_ROLE != null) CreDetail(Obj as SU_ROLE);
                else if (Obj as SU_ROLE_LANG != null) CreDetail(Obj as SU_ROLE_LANG);
                else if (Obj as SU_ROLE_GROUP != null) CreDetail(Obj as SU_ROLE_GROUP);
                else if (Obj as SU_ROLE_GROUP_LANG != null) CreDetail(Obj as SU_ROLE_GROUP_LANG);
                else if (Obj as SU_ROLE_PGM != null) CreDetail(Obj as SU_ROLE_PGM);

                else if (Obj as SU_TRANSLATE != null) CreDetail(Obj as SU_TRANSLATE);
                else if (Obj as SU_TRANSLATE_LANG != null) CreDetail(Obj as SU_TRANSLATE_LANG);
                else if (Obj as SU_TRANSLATE_PGM != null) CreDetail(Obj as SU_TRANSLATE_PGM);
                else if (Obj as SU_TRANSLATE_PGM_LANG != null) CreDetail(Obj as SU_TRANSLATE_PGM_LANG);

                else if (Obj as SU_DIV_TYPE != null) CreDetail(Obj as SU_DIV_TYPE);
                else if (Obj as SU_DIV_TYPE_LANG != null) CreDetail(Obj as SU_DIV_TYPE_LANG);
                else if (Obj as SU_DIVISION != null) CreDetail(Obj as SU_DIVISION);
                else if (Obj as SU_DIVISION_LANG != null) CreDetail(Obj as SU_DIVISION_LANG);

                else if (Obj as SU_USER != null) CreDetail(Obj as SU_USER);
                else if (Obj as SU_USER_DIV != null) CreDetail(Obj as SU_USER_DIV);
                else if (Obj as SU_USER_PROFILE != null) CreDetail(Obj as SU_USER_PROFILE);
                else if (Obj as SU_USER_PROFILE_LANG != null) CreDetail(Obj as SU_USER_PROFILE_LANG);
                else if (Obj as SU_USER_ROLE != null) CreDetail(Obj as SU_USER_ROLE);

               // else if (Obj as SU_LOG_TYPE != null) CreDetail(Obj as SU_LOG_TYPE);
            }
        }

        #endregion



        #region Impement Create and update Detail

        #region SU_LOG

        //private static void UpdDetail(SU_LOG_TYPE Obj)
        //{
        //    Obj.UPD_BY = UserID;
        //    Obj.UPD_DATE = DateTime.Now;
        //    Obj.UPD_PGM = Pgm;
        //}

        //private static void CreDetail(SU_LOG_TYPE Obj)
        //{
        //    Obj.CRE_BY = UserID;
        //    Obj.CRE_DATE = DateTime.Now;
        //}

        #endregion

        #region SU_DIVISION

        private static void UpdDetail(SU_DIV_TYPE Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_DIV_TYPE Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_DIV_TYPE_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_DIV_TYPE_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_DIVISION Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_DIVISION Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_DIVISION_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_DIVISION_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_TRANSLATE

        private static void UpdDetail(SU_TRANSLATE Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_TRANSLATE Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_TRANSLATE_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_TRANSLATE_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_TRANSLATE_PGM Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_TRANSLATE_PGM Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_TRANSLATE_PGM_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_TRANSLATE_PGM_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_ROLE

        private static void UpdDetail(SU_ROLE Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ROLE Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ROLE_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ROLE_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ROLE_GROUP Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ROLE_GROUP Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ROLE_GROUP_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ROLE_GROUP_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ROLE_PGM Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ROLE_PGM Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_PROGRAM

        private static void UpdDetail(SU_PROGRAM Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_PROGRAM Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_PROGRAM_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_PROGRAM_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_MODULE

        private static void UpdDetail(SU_MODULE_GROUP Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_MODULE_GROUP Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_MODULE_GROUP_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_MODULE_GROUP_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_MODULE_PGM Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_MODULE_PGM Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_MODULE_PGM_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_MODULE_PGM_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_ALERT

        private static void UpdDetail(SU_ALERT Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ALERT Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ALERT_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ALERT_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ALERT_GROUP Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ALERT_GROUP Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ALERT_GROUP_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ALERT_GROUP_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_ORGANIZATION

        private static void UpdDetail(SU_ORGANIZATION Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ORGANIZATION Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_ORGANIZATION_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_ORGANIZATION_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #region SU_LANG

        private static void CreDetail(SU_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        #endregion

        #region SU_USER

        private static void UpdDetail(SU_USER Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_USER Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_USER_DIV Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_USER_DIV Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_USER_PROFILE Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_USER_PROFILE Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_USER_PROFILE_LANG Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_USER_PROFILE_LANG Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        private static void UpdDetail(SU_USER_ROLE Obj)
        {
            Obj.UPD_BY = UserID;
            Obj.UPD_DATE = DateTime.Now;
            Obj.UPD_PGM = Pgm;
        }

        private static void CreDetail(SU_USER_ROLE Obj)
        {
            Obj.CRE_BY = UserID;
            Obj.CRE_DATE = DateTime.Now;
        }

        #endregion

        #endregion

        public static System.IO.MemoryStream Serialization<T>(T Obj) where T : class
        {
            System.Runtime.Serialization.DataContractSerializer ds = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            ds.WriteObject(stream, Obj);
            stream.Position = 0;
            return stream;
        }

        public static T DeSerialization<T>(object obj) where T : class
        {
            System.Runtime.Serialization.DataContractSerializer ds = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            T result = ds.ReadObject(obj as System.IO.MemoryStream) as T;
            return result;
        }

    }

    
}
