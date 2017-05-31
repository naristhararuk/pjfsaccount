using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;

namespace SCG.FN.DTO
{
	[Serializable]
	public class FnCashier
	{
		#region Constructor
		public FnCashier()
		{
		
		}
        public FnCashier(short cashierID)
        {
            this.cashierID = cashierID;
        }
		#endregion

		#region Property
		/// <summary>
		/// Get and set CashierID of Current Cashier.
		/// </summary>
		private short cashierID;
		public virtual short CashierID
		{
			get { return this.cashierID; }
			set { this.cashierID = value; }
		}

		/// <summary>
		/// Get and set Organization of Current Cashier.
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
		/// Get and set Division of Current Cashier.
		/// </summary>
		private SuDivision division;
		public virtual SuDivision Division
		{
			get { return this.division; }
			set { this.division = value; }
		}
		public virtual short DivisionID
		{
			get
			{
				if (division == null)
				{
					return -1;
				}
				else
				{
					return division.Divisionid;
				}
			}
			set
			{
				if (division != null)
				{
					this.division.Divisionid = value;
				}
			}
		}

		/// <summary>
		/// Get and set User of Current Cashier.
		/// </summary>
		private SuUser user;
		public virtual SuUser User
		{
			get { return this.user; }
			set { this.user = value; }
		}

		/// <summary>
		/// Get and set CashierCode of Current Cashier.
		/// </summary>
		private string cashierCode;
		public virtual string CashierCode
		{
			get { return this.cashierCode; }
			set { this.cashierCode = value; }
		}

		/// <summary>
		/// Get and set CashierLevel of Current Cashier.
		/// </summary>
		private string cashierLevel;
		public virtual string CashierLevel
		{
			get { return this.cashierLevel; }
			set { this.cashierLevel = value; }
		}

		/// <summary>
		/// Get and set Active of Current Cashier.
		/// </summary>
		private bool active;
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Get and set CreBy of Current Cashier.
		/// </summary>
		private long creBy;
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}

		/// <summary>
		/// Get and set CreDate of Current Cashier.
		/// </summary>
		private DateTime creDate;
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}

		/// <summary>
		/// Get and set UpdBy of Current Cashier.
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
