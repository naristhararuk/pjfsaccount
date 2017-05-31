using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    public class DbZoneResult
    {
        public short ZoneID { get; set; }
        public string ZoneName { get; set; }

        public short ZoneLangID { get; set; }
        public short LanguageID { get; set; }
        public string LanguageName { get; set; }

        public string Comment { get; set; }
        public bool Active { get; set; }

        public long UpdBy { get; set; }
        public DateTime UpdDate { get; set; }
        public string UpdPgm { get; set; }

        public DbZoneResult()
        {

        }
    }
}
