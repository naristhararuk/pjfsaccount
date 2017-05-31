using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.FN.DTO.ValueObject
{
	public class TranslatedListItem
	{
		#region Constructor
		public TranslatedListItem()
		{
		
		}
		#endregion
		
		#region Property
		public virtual short? ID { get; set; }
		public virtual string Text { get; set; }
		#endregion
	}
}
