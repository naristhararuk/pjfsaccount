using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
	public class PaymentTypeListItem
	{
		#region Constructor
        public PaymentTypeListItem()
		{
		
		}
		#endregion
		
		#region Property
        public virtual string Code { get; set; }
		public virtual string Text { get; set; }
        public virtual long ID { get; set; }
		#endregion
	}
}
