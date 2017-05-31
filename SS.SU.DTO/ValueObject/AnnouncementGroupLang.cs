using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	public class AnnouncementGroupLang
	{
		public AnnouncementGroupLang()
		{
		
		}

		public short? AnnouncementGroupLangId { get; set; }
		public short? AnnouncementGroupId { get; set; }
		public short? LanguageId { get; set; }
		public string LanguageName { get; set; }
		public string AnnouncementGroupName { get; set; }
		public string Comment { get; set; }
		public Boolean Active { get; set; }
	}
}
