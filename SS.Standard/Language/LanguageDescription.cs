using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SS.Standard.Data.Mssql;
using SS.Standard.Data.Interfaces;

namespace SS.Standard.Language
{
    public class LanguageDescription
    {

        public LanguageDescription()
        {
        }
        public string GetLanguageName(string LanguageID)
        {
            Provider.TranslationDAL.OpenConnection();
            string lang_name = "";
            IDataReader reader = Provider.TranslationDAL.GetLanguageName(int.Parse(LanguageID.Trim()));
            while (reader.Read())
            {
                lang_name=reader[0].ToString();
                break;
            }
            reader.Close();
            Provider.TranslationDAL.CloseConnection();
            return lang_name;
        }
    }
}
