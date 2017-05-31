using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Web;

namespace SS.Standard.Language.DAL.Mssql
{
    public class TranslationDAL : Data.Mssql.MssqlHelper, Data.Interfaces.IDBManager,DAL.Interface.ITranslation
    {
        private SqlCommand comm;
        public TranslationDAL()
        {
            comm = new SqlCommand();
        }
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

        public IDataReader QueryLang(int LanguageID, string ProgramCode)
       {
           comm.Parameters.Clear();
          // comm.CommandText="SELECT NAME,WORD FROM SU_TRANSLATE_PGM AS T JOIN SU_TRANSLATE_PGM_LANG AS L ON T.TRANS_ProgramID = L.TRANS_ProgramID JOIN SuProgram P ON T.ProgramID = P.ProgramID WHERE L.LanguageID = @LanguageID AND P.ProgramCode = @ProgramCode";
           comm.CommandText = "SELECT     SuGlobalTranslate.TranslateSymbol , SuGlobalTranslateLang.TranslateWord  FROM         SuGlobalTranslate INNER JOIN  SuGlobalTranslateLang ON SuGlobalTranslate.TranslateID = SuGlobalTranslateLang.TranslateID  WHERE SuGlobalTranslate.Active=1 AND SuGlobalTranslate.ProgramCode =@ProgramCode AND SuGlobalTranslateLang.LanguageID=@LanguageID";
           comm.Parameters.AddWithValue("@LanguageID", LanguageID);
           comm.Parameters.AddWithValue("@ProgramCode", ProgramCode.Trim());
           return ExecuteReader(comm);
       }
        public IDataReader QueryLang(int LanguageID)
       {
           comm.Parameters.Clear();
          // comm.CommandText="SELECT NAME,WORD FROM SU_TRANSLATE AS T JOIN SU_TRANSLATE_LANG AS L ON T.TRANS_ID = L.TRANS_ID WHERE L.LanguageID = @LanguageID";
           comm.CommandText = "SELECT     SuGlobalTranslate.TranslateSymbol , SuGlobalTranslateLang.TranslateWord  FROM         SuGlobalTranslate INNER JOIN  SuGlobalTranslateLang ON SuGlobalTranslate.TranslateID = SuGlobalTranslateLang.TranslateID  WHERE SuGlobalTranslate.Active=1 AND SuGlobalTranslateLang.LanguageID=@LanguageID";
           comm.Parameters.AddWithValue("@LanguageID", LanguageID);
           return ExecuteReader(comm);

       }

       public IDataReader ChangeLang(int Old_LanguageID, int New_LanguageID, string ProgramCode)
       {
           comm.Parameters.Clear();
          // comm.CommandText="SELECT O.WORD , N.WORD FROM SU_TRANSLATE_PGM_LANG AS O JOIN SU_TRANSLATE_PGM_LANG AS N ON O.TRANS_ProgramID = N.TRANS_ProgramID JOIN SU_TRANSLATE_PGM AS T ON T.TRANS_ProgramID = N.TRANS_ProgramID JOIN SuProgram AS P ON T.ProgramID = P.ProgramID WHERE O.LanguageID =@OLD_LanguageID AND N.LanguageID=@NEW_LanguageID AND P.ProgramCode = @ProgramCode";
          comm.CommandText=" SELECT    SuGlobalTranslateLang.TranslateWord , SuGlobalTranslateLang.TranslateWord  FROM         SuGlobalTranslate As O JOIN SuGlobalTranslate As N ON O.ProgramCode = N.ProgramCode	INNER JOIN  SuGlobalTranslateLang ON O.TranslateID = SuGlobalTranslateLang.TranslateID  WHERE O.LanguageID =@OLD_LanguageID AND N.LanguageID=@NEW_LanguageID AND N.ProgramCode = @ProgramCode ";
           comm.Parameters.AddWithValue("@OLD_LanguageID", Old_LanguageID);
           comm.Parameters.AddWithValue("@NEW_LanguageID", New_LanguageID);
           comm.Parameters.AddWithValue("@ProgramCode", ProgramCode.Trim());
           return ExecuteReader(comm);
       }
       public IDataReader ChangeLang(int Old_LanguageID, int New_LanguageID)
       {
           comm.Parameters.Clear();
          // comm = new SqlCommand("SELECT O.WORD , N.WORD FROM SU_TRANSLATE_LANG AS O JOIN SU_TRANSLATE_LANG AS N ON O.TRANS_ID = N.TRANS_ID WHERE O.LanguageID=@OLD_LanguageID AND N.LanguageID=@NEW_LanguageID");
           comm.CommandText = " SELECT    SuGlobalTranslateLang.TranslateWord , SuGlobalTranslateLang.TranslateWord  FROM         SuGlobalTranslate As O JOIN SuGlobalTranslate As N ON O.ProgramCode = N.ProgramCode	INNER JOIN  SuGlobalTranslateLang ON O.TranslateID = SuGlobalTranslateLang.TranslateID  WHERE O.LanguageID =@OLD_LanguageID AND N.LanguageID=@NEW_LanguageID ";

           comm.Parameters.AddWithValue("@OLD_LanguageID", Old_LanguageID);
           comm.Parameters.AddWithValue("@NEW_LanguageID", New_LanguageID);
           return ExecuteReader(comm);

       }
       public IDataReader GetLanguageName(int LanguageID)
       {
           comm.Parameters.Clear();
           comm.CommandText="select LanguageName from SuLanguage where LanguageID = @LanguageID";
           comm.Parameters.AddWithValue("@LanguageID", LanguageID);
           return ExecuteReader(comm);

       }
    }
}
