using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;
using SS.DB.DTO;

namespace SCG.eAccounting.DTO
{
	[Serializable]
	public class Document
	{
		#region Constructor
		public Document()
		{
		
		}
        public Document(long documentID)
        {
            this.documentID = documentID;
        }
		#endregion

		#region Property
		/// <summary>
		/// Get and set Id of Current Document.
		/// </summary>
		private long documentID;
		public virtual long DocumentID
		{
			get { return this.documentID; }
			set { this.documentID = value; }
		}

		/// <summary>
		/// Get and set Organization of Current Document.
		/// </summary>
		private SuOrganization organization;
		public virtual SuOrganization Organization
		{
			get { return this.organization; }
			set { this.organization = value; }
		}
		public virtual short OrganizationID
		{
			get 
			{
				if (organization == null)
				{
					return -1;
				}
				else
				{
					return organization.Organizationid;
				}
			}
			set 
			{
				if (organization != null)
				{
					this.organization.Organizationid = value;
				}
			}
		}

		/// <summary>
		/// Get and set Requester of Current Document.
		/// </summary>
		private SuUser requester;
		public virtual SuUser Requester
		{
			get { return this.requester; }
			set { this.requester = value; }
		}

		/// <summary>
		/// Get and set Creator of Current Document.
		/// </summary>
		private SuUser creator;
		public virtual SuUser Creator
		{
			get { return this.creator; }
			set { this.creator = value; }
		}

		/// <summary>
		/// Get and set Receiver of Current Document.
		/// </summary>
		private SuUser receiver;
		public virtual SuUser Receiver
		{
			get { return this.receiver; }
			set { this.receiver = value; }
		}

		/// <summary>
		/// Get and set Approver of Current Document.
		/// </summary>
		private SuUser approver;
		public virtual SuUser Approver
		{
			get { return this.approver; }
			set { this.approver = value; }
		}

		/// <summary>
		/// Get and set DocumentNo of Current Document.
		/// </summary>
		private string documentNo;
		public virtual string DocumentNo
		{
			get { return this.documentNo; }
			set { this.documentNo = value; }
		}

		/// <summary>
		/// Get and set DocumentType of Current Document.
		/// </summary>
		private string documentType;
		public virtual string DocumentType
		{
			get { return this.documentType; }
			set { this.documentType = value; }
		}

		/// <summary>
		/// Get and set DocumentStatus of Current Document.
		/// </summary>
		private string documentStatus;
		public virtual string DocumentStatus
		{
			get { return this.documentStatus; }
			set { this.documentStatus = value; }
		}

		/// <summary>
		/// Get and set Active of Current Document.
		/// </summary>
		private bool active;
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Get and set CreBy of Current Document.
		/// </summary>
		private long creBy;
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}

		/// <summary>
		/// Get and set CreDate of Current Document.
		/// </summary>
		private DateTime creDate;
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}

		/// <summary>
		/// Get and set UpdBy of Current Document.
		/// </summary>
		private long updBy;
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}

		/// <summary>
		/// Get and set UpdDate of Current Document.
		/// </summary>
		private DateTime updDate;
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}

		/// <summary>
		/// Get and set UpdPgm of Current Document.
		/// </summary>
		private string updPgm;
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}

		/// <summary>
		/// Get and set RowVersion of Current Document.
		/// </summary>
		private Byte[] rowVersion;
		public virtual Byte[] RowVersion
		{
			get { return this.rowVersion; }
			set { this.rowVersion = value; }
		}
		#endregion
	}
}
