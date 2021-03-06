﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    public class MenuLang
    {
        public short? MenuLangId { get; set; }
        public short? MenuId { get; set; }
        public short? LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string MenuName { get; set; }
        public string Comment { get; set; }
        public Boolean Active { get; set; }

        public MenuLang()
        { }
    }
}
