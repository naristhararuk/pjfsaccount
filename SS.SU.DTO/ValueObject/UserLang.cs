using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
	[Serializable]
	public class UserLang
	{
		#region Constructor
		public UserLang()
		{

		}
		#endregion

		#region Property
		public long? UserId { get; set; }
		public long? UserLangId	{ get; set; }
		public short? LanguageId { get; set; }
		public string LanguageName { get; set; }
        public string EmployeeName { get; set; }
		//public string FirstName { get; set; EmployeeName
		//public string LastName { get; set; }
		public string Comment { get; set; }
		public Boolean Active { get; set; }
		#endregion
	}
}
