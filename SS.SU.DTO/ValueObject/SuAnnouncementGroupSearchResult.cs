using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public class SuAnnouncementGroupSearchResult
	{
		#region Field
		private short announcementGroupId;
		private string announcementGroupName;
		private short displayOrder;
		private string imagePath;
		private short languageId;
		private string languageName;
		private string comment;
		private bool active;
		#endregion
		
		#region Constructor
		public SuAnnouncementGroupSearchResult()
		{
		
		}
		#endregion
		
		#region Property
		public virtual short AnnouncementGroupid
		{
			get { return this.announcementGroupId; }
			set { this.announcementGroupId = value; }
		}
		public virtual short LanguageId
		{
			get { return this.languageId; }
			set { this.languageId = value; }
		}
		public virtual short DisplayOrder
		{
			get { return this.displayOrder; }
			set { this.displayOrder = value; }
		}
		public virtual string ImagePath
		{
			get { return this.imagePath; }
			set { this.imagePath = value; }
		}
		public virtual string AnnouncementGroupName
		{
			get { return this.announcementGroupName; }
			set { this.announcementGroupName = value; }
		}
		public virtual string LanguageName
		{
			get { return this.languageName; }
			set { this.languageName = value; }
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
