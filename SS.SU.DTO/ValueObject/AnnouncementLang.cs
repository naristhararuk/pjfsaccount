using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	public class AnnouncementLang
	{
		public AnnouncementLang()
		{
		
		}
		
		public long? AnnouncementLangId { get; set; }
		public short? AnnouncementId { get; set; }
		public short? LanguageId { get; set; }
		public string LanguageName { get; set; }
        public string AnnouncementGroupName { get; set; }
		public string AnnouncementHeader { get; set; }
        public string AnnouncementBody { get; set; }
        public string AnnouncementFooter { get; set; }
        public string ImagePath { get; set; }
		public string Comment { get; set; }
		public Boolean Active { get; set; }
	}
}
