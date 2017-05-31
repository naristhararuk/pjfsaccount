using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public class SuPasswordHistory
	{
		#region Constructor
		public SuPasswordHistory()
		{

		}
		#endregion

		#region Field
		private long id;
		private SuUser user;
		private DateTime changeDate;
		private string password;
		private bool? active;
		private long? creBy;
		private DateTime? creDate;
		private long? updBy;
		private DateTime? updDate;
		private string updPgm;
		private Byte[] rowVersion;
		#endregion

		#region Property
		public virtual long Id
		{
			get { return this.id; }
			set { this.id = value; }
		}
		public virtual SuUser User
		{
			get { return this.user; }
			set { this.user = value; }
		}
		public virtual long UserId
		{
			get 
			{
				if (user != null)
				{
					return this.user.Userid; 
				}
				else
				{
					return -1;
				}
			}
			set 
			{
				if (user != null)
				{
					this.user.Userid = value;
				}
			}
		}
		public virtual string UserName
		{
			get
			{
				if (user != null)
				{
					return this.user.UserName;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				if (user != null)
				{
					this.user.UserName = value;
				}
			}
		}
		public virtual DateTime ChangeDate
		{
			get { return this.changeDate; }
			set { this.changeDate = value; }
		}
		public virtual string Password
		{
			get { return this.password; }
			set { this.password = value; }
		}
		public virtual bool? Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		public virtual long? CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		public virtual DateTime? CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		public virtual long? UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		public virtual DateTime? UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		#endregion
	}
}
