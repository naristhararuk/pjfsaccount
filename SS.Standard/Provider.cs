using System;
using System.Text;
using System.Configuration;
using SS.Standard.AlertMsg.DAL.Interface;
using SS.Standard.Security.Mssql.DAL.Interface;
using SS.Standard.Language.DAL.Interface;
using SS.Standard.Utilities.Interface;
using System.Reflection;

namespace SS.Standard
{
    public class Provider
    {
       // private static readonly string ProviderDAL = ConfigurationManager.AppSettings["LibProvider"].ToString();
        private static IUserDAL _UserDAL;
        private static ITranslation _TranslationDAL;
        private static IMenuDAL _MenuDAL;
        private static IMessageDAL _MessageDAL;
        private static IDbParameter _DbParameter;
        private static StringBuilder ClassName;

        public static IUserDAL UserDAL
        {
            get
            {
                if (_UserDAL == null)
                {
                    ClassName = new StringBuilder();
                    ClassName.AppendFormat("SS.Standard.Security.DAL.{0}.UserDAL", "Mssql");
                    _UserDAL = (IUserDAL)Assembly.Load("SS.Standard").CreateInstance(ClassName.ToString());
                }

                return _UserDAL;
            }
        }

        public static ITranslation TranslationDAL
        {
            get
            {
                if (_TranslationDAL == null)
                {
                    ClassName = new StringBuilder();
                    ClassName.AppendFormat("SS.Standard.Language.DAL.{0}.TranslationDAL", "Mssql");
                    _TranslationDAL = (ITranslation)Assembly.Load("SS.Standard").CreateInstance(ClassName.ToString());
                }

                return _TranslationDAL;
            }
        }

        public static IMenuDAL MenuDAL
        {
            get
            {
                if (_MenuDAL == null)
                {
                    ClassName = new StringBuilder();
                    ClassName.AppendFormat("SS.Standard.Security.DAL.{0}.MenuDAL", "Mssql");
                    _MenuDAL = (IMenuDAL)Assembly.Load("SS.Standard").CreateInstance(ClassName.ToString());
                }

                return _MenuDAL;
            }
        }

        public static IMessageDAL MessageDAL
        {
            get
            {
                if (_MenuDAL == null)
                {
                    ClassName = new StringBuilder();
                    ClassName.AppendFormat("SS.Standard.AlertMsg.DAL.{0}.MessageDAL", "Mssql");
                    _MessageDAL = (IMessageDAL)Assembly.Load("SS.Standard").CreateInstance(ClassName.ToString());
                }

                return _MessageDAL;
            }
        }

        public static IDbParameter DbParameter
        {
            get
            {
                if (_DbParameter == null)
                {
                    ClassName = new StringBuilder();
                    ClassName.AppendFormat("SS.Standard.Utilities.{0}.DbParameterDAL", "Mssql");
                    _DbParameter = (IDbParameter)Assembly.Load("SS.Standard").CreateInstance(ClassName.ToString());
                }

                return _DbParameter;
            }
        }
    }
}
