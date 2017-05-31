using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public partial class SuDivisionWithLang
	{
		public short DivisionID { get; set; }
		public string DivisionName { get; set; }
	}
}
