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
	/// POJO for SuAnnouncementGroup. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuAnnouncementGroup
	{
		#region Fields
		
		private short announcementGroupid;
		private short? displayOrder;
		private string comment;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
		private Byte[] rowVersion;
		private bool active;
		private string imagePath;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuAnnouncementGroup class
		/// </summary>
		public SuAnnouncementGroup()
		{
		}

		public SuAnnouncementGroup(short announcementGroupid)
		{
			this.announcementGroupid = announcementGroupid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuAnnouncementGroup class
		/// </summary>
		/// <param name="displayOrder">Initial <see cref="SuAnnouncementGroup.DisplayOrder" /> value</param>
		/// <param name="comment">Initial <see cref="SuAnnouncementGroup.Comment" /> value</param>
		/// <param name="updBy">Initial <see cref="SuAnnouncementGroup.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuAnnouncementGroup.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuAnnouncementGroup.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuAnnouncementGroup.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuAnnouncementGroup.UpdPgm" /> value</param>
		/// <param name="rowVersion">Initial <see cref="SuAnnouncementGroup.RowVersion" /> value</param>
		/// <param name="active">Initial <see cref="SuAnnouncementGroup.Active" /> value</param>
		public SuAnnouncementGroup(short displayOrder, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active)
		{
			this.displayOrder = displayOrder;
			this.comment = comment;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
			this.rowVersion = rowVersion;
			this.active = active;
		}
	
		/// <summary>
		/// Minimal constructor for class SuAnnouncementGroup
		/// </summary>
		/// <param name="updBy">Initial <see cref="SuAnnouncementGroup.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="SuAnnouncementGroup.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="SuAnnouncementGroup.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="SuAnnouncementGroup.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="SuAnnouncementGroup.UpdPgm" /> value</param>
		/// <param name="active">Initial <see cref="SuAnnouncementGroup.Active" /> value</param>
		public SuAnnouncementGroup(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
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
		/// Gets or sets the AnnouncementGroupid for the current SuAnnouncementGroup
		/// </summary>
		public virtual short AnnouncementGroupid
		{
			get { return this.announcementGroupid; }
			set { this.announcementGroupid = value; }
		}
		
		/// <summary>
		/// Gets or sets the DisplayOrder for the current SuAnnouncementGroup
		/// </summary>
		public virtual short? DisplayOrder
		{
			get { return this.displayOrder; }
			set { this.displayOrder = value; }
		}
		
		/// <summary>
		/// Gets or sets the Comment for the current SuAnnouncementGroup
		/// </summary>
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current SuAnnouncementGroup
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current SuAnnouncementGroup
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current SuAnnouncementGroup
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current SuAnnouncementGroup
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current SuAnnouncementGroup
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		
		/// <summary>
		/// Gets or sets the RowVersion for the current SuAnnouncementGroup
		/// </summary>
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current SuAnnouncementGroup
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Gets or sets the ImagePath for the current SuAnnouncementGroup
		/// </summary>
		public virtual string ImagePath
		{
			get { return this.imagePath; }
			set { this.imagePath = value; }
		}
		#endregion
	}
}
