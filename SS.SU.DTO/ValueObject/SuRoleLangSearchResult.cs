using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	[Serializable]
	public class SuRoleLangSearchResult
	{
		#region Constructor

		#endregion

		#region Property
		public short? RoleId { get; set; }
		public short? LanguageId { get; set; }
		public string LanguageName { get; set; }
		public string RoleName { get; set; }
		public Boolean Active { get; set; }
		#endregion
	}
}
