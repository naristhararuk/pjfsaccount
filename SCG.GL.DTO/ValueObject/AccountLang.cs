using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.GL.DTO.ValueObject
{
    public class AccountLang
    {
        public short?   AccountLangId   { get; set; }
        public short?   AccId           { get; set; }
        public short?   LanguageId      { get; set; }
        public string   LanguageName    { get; set; }
        public string   AccountName     { get; set; }
        public string   AccNo       { get; set; }
        public short   AccType         { get; set; }
        public short   AccLevel        { get; set; }

        public string   MainAccID       { get; set; }
        public bool     TransactionYN   { get; set; }
        public bool     Active          { get; set; }
        public int?     AccCount        { get; set; }
        public string Comment { get; set; }

        public AccountLang()
        {
        }
    }
}
