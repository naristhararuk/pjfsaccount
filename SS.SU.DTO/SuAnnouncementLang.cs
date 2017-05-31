//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SS.SU.DTO
{
	/// <summary>
	/// POJO for SuAnnouncementLang. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuAnnouncementLang
	{
		#region Fields
		
		private long id;
		private string announcementHeader;
		private string announcementBody;
		private string announcementFooter;
		//private string announcementIcon;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private SS.SU.DTO.SuAnnouncement announcement;
		private SS.DB.DTO.DbLanguage language;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuAnnouncementLang class
		/// </summary>
		public SuAnnouncementLang()
		{
		}

		public SuAnnouncementLang(long id)
		{
			this.id = id;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuAnnouncementLang class
		/// </summary>
		/// <param name="announcementHeader">Initial <see cref="SuAnnouncementLang.AnnouncementHeader" /> value</param>
		/// <param name="announcementBody">Initial <see cref="SuAnnouncementLang.AnnouncementBody" /> value</param>
		/// <param name="announcementFooter">Initial <see cref="SuAnnouncementLang.AnnouncementFooter" /> value</param>
		/// <param name="announcementIcon">Initial <see cref="SuAnnouncementLang.AnnouncementIcon" /> value</param>
		/// <param name="comment">Initial <see cref="SuAnnouncementLang.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuAnnouncementLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuAnnouncementLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuAnnouncementLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuAnnouncementLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuAnnouncementLang.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuAnnouncementLang.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuAnnouncementLang.Active" /> value</param>
		/// <param name="announcement">Initial <see cref="SuAnnouncementLang.Announcement" /> value</param>
		/// <param name="language">Initial <see cref="SuAnnouncementLang.Language" /> value</param>
		public SuAnnouncementLang(string announcementHeader, string announcementBody, string announcementFooter, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.SU.DTO.SuAnnouncement announcement, SS.DB.DTO.DbLanguage language)
		{
			this.announcementHeader = announcementHeader;
			this.announcementBody = announcementBody;
			this.announcementFooter = announcementFooter;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
			this.announcement = announcement;
			this.language = language;
		}
	
		/// <summary>
		/// Minimal constructor for class SuAnnouncementLang
		/// </summary>
		/// <param name="updBy">Initial <see cref="SuAnnouncementLang.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuAnnouncementLang.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuAnnouncementLang.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuAnnouncementLang.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuAnnouncementLang.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuAnnouncementLang.Active" /> value</param>
		public SuAnnouncementLang(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
		{
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.active = active;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the Id for the current SuAnnouncementLang
		/// </summary>
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		
		/// <summary>
		/// Gets or sets the AnnouncementHeader for the current SuAnnouncementLang
		/// </summary>
		public virtual string AnnouncementHeader
		{
			get { return this.announcementHeader; }
			set { this.announcementHeader = value; }
		}
		
		/// <summary>
		/// Gets or sets the AnnouncementBody for the current SuAnnouncementLang
		/// </summary>
		public virtual string AnnouncementBody
		{
			get { return this.announcementBody; }
			set { this.announcementBody = value; }
		}
		
		/// <summary>
		/// Gets or sets the AnnouncementFooter for the current SuAnnouncementLang
		/// </summary>
		public virtual string AnnouncementFooter
		{
			get { return this.announcementFooter; }
			set { this.announcementFooter = value; }
		}
		
		///// <summary>
		///// Gets or sets the AnnouncementIcon for the current SuAnnouncementLang
		///// </summary>
		//public virtual string AnnouncementIcon
		//{
		//    get { return this.announcementIcon; }
		//    set { this.announcementIcon = value; }
		//}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuAnnouncementLang
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuAnnouncementLang
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuAnnouncementLang
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuAnnouncementLang
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuAnnouncementLang
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuAnnouncementLang
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuAnnouncementLang
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuAnnouncementLang
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the Announcement for the current SuAnnouncementLang
		/// </summary>
		public virtual SS.SU.DTO.SuAnnouncement Announcement
		{
			get { return this.announcement; }
			set { this.announcement = value; }
		}

		public virtual short AnnouncementId
		{
			get
			{
				if (announcement != null)
				{
					return this.announcement.Announcementid;
				}
				else
				{
					return 0;
				}
			}
			set
			{
				if (announcement != null)
				{
					this.announcement.Announcementid = value;
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the Language for the current SuAnnouncementLang
		/// </summary>
		public virtual SS.DB.DTO.DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}

		public virtual short LanguageId
		{
			get 
			{
				if (language != null)
				{
					return this.language.Languageid; 
				}
				else
				{
					return 0;
				}		
			}
			set 
			{
				if (language != null)
				{
					this.language.Languageid = value; 	
				}
			}
		}
		#endregion
	}
}