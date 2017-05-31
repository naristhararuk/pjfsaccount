using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public partial class TranslatedListItem
	{
		public virtual short Id { get; set; }
		public virtual string Text { get; set; }
	}
}
