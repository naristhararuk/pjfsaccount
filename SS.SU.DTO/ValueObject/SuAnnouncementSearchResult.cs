using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public class SuAnnouncementSearchResult
	{
		#region Constructor
		public SuAnnouncementSearchResult()
		{
		
		}
		#endregion

		#region Field
		private short announcementId;
		private string announcementHeader;
		private DateTime effectiveDate;
		private DateTime lastDisplayDate;
		private short languageId;
		private string comment;
		private bool active;
		#endregion

		#region Property
		public virtual short Announcementid
		{
			get { return this.announcementId; }
			set { this.announcementId = value; }
		}
		public virtual short LanguageId
		{
			get { return this.languageId; }
			set { this.languageId = value; }
		}
		public virtual string AnnouncementHeader
		{
			get { return this.announcementHeader; }
			set { this.announcementHeader = value; }
		}
		public virtual DateTime EffectiveDate
		{
			get	{ return this.effectiveDate; }
			set { this.effectiveDate = value; }
		}
		public virtual DateTime LastDisplayDate
		{
			get { return this.lastDisplayDate; }
			set { this.lastDisplayDate = value; }
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
