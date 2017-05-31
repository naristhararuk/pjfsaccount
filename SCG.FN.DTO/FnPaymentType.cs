using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.FN.DTO
{
	[Serializable]
	public class FnPaymentType
	{
		#region Constructor
		public FnPaymentType()
		{
		
		}
		#endregion

		#region Property
		/// <summary>
		/// Get and set PaymentID of Current PaymentType.
		/// </summary>
		private short paymentTypeID;
		public virtual short PaymentTypeID
		{
			get { return this.paymentTypeID; }
			set { this.paymentTypeID = value; }
		}

		/// <summary>
		/// Get and set Code of Current PaymentType.
		/// </summary>
		private string code;
		public virtual string Code
		{
			get { return this.code; }
			set { this.code = value; }
		}

		/// <summary>
		/// Get and set Active of Current PaymentType.
		/// </summary>
		private bool active;
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Get and set CreBy of Current PaymentType.
		/// </summary>
		private long creBy;
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}

		/// <summary>
		/// Get and set CreDate of Current PaymentType.
		/// </summary>
		private DateTime creDate;
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}

		/// <summary>
		/// Get and set UpdBy of Current PaymentType.
		/// </summary>
		private long updBy;
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}

		/// <summary>
		/// Get and set UpdDate of Current PaymentType.
		/// </summary>
		private DateTime updDate;
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}

		/// <summary>
		/// Get and set UpdPgm of Current PaymentType.
		/// </summary>
		private string updPgm;
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}

		/// <summary>
		/// Get and set RowVersion of Current PaymentType.
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
