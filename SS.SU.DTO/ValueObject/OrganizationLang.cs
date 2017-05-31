using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	public class OrganizationLang
	{
		public OrganizationLang()
		{
		
		}

		public long? OrganizationLangId { get; set; }
		public string OrganizationName { get; set; }
		public short? OrganizationId { get; set; }
		public short? LanguageId { get; set; }
		public string LanguageName { get; set; }
		public string Comment { get; set; }
		public Boolean Active { get; set; }
	}
}
