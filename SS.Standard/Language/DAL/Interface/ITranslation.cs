using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SS.Standard.Language.DAL.Interface
{
  public   interface ITranslation
    {
      IDataReader QueryLang(int LanguageID, string ProgramCode);
      IDataReader QueryLang(int LanguageID);
      IDataReader ChangeLang(int Old_LanguageID,int New_LanguageID, string ProgramCode);
      IDataReader ChangeLang(int Old_LanguageID,int New_LanguageID);
      IDataReader GetLanguageName(int LanguageID);
      void OpenConnection();
      void CloseConnection();

    }
}
