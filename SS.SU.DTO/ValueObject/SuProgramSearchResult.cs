using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public class SuProgramSearchResult
	{
		#region Constructor
		public SuProgramSearchResult()
		{

		}
		#endregion

		#region Field
		private short programId;
		private string programCode;
		private string programName;
		private string programPath;
		private short languageId;
		private string comment;
		private bool active;
		#endregion

		#region Property
		public virtual short ProgramID
		{
			get { return this.programId; }
			set { this.programId = value; }
		}
		public virtual short LanguageId
		{
			get { return this.languageId; }
			set { this.languageId = value; }
		}
		public virtual string ProgramCode
		{
			get { return this.programCode; }
			set { this.programCode = value; }
		}
		public virtual string ProgramPath
		{
			get { return this.programPath; }
			set { this.programPath = value; }
		}
		public virtual string ProgramName
		{
			get { return this.programName; }
			set { this.programName = value; }
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
