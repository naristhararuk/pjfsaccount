using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public class SuDivisionSearchResult
	{
		#region Constructor
		public SuDivisionSearchResult()
		{
		
		}
		#endregion

		#region Field
		private short divisionId;
		private string divisionName;
		private short languageId;
		private string comment;
		private bool active;
		#endregion

		#region Property
		public virtual short DivisionId
		{
			get { return this.divisionId; }
			set { this.divisionId = value; }
		}
		public virtual short LanguageId
		{
			get { return this.languageId; }
			set { this.languageId = value; }
		}		
		public virtual string DivisionName
		{
			get { return this.divisionName; }
			set { this.divisionName = value; }
		}
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		#endregion
	}
}
