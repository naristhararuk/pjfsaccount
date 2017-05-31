using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.DTO;
using SS.DB.DTO;

namespace SCG.FN.DTO
{
	public class FnCashierLang
	{
		#region Constructor
		public FnCashierLang()
		{
		
		}
		#endregion

		#region Property
		/// <summary>
		/// Get and set ID of Current CashierLang.
		/// </summary>
		private long id;
		public virtual long ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		/// <summary>
		/// Get and set Cashier of Current CashierLang.
		/// </summary>
		private FnCashier cashier;
		public virtual FnCashier Cashier
		{
			get { return this.cashier; }
			set { this.cashier = value; }
		}
		public virtual short CashierID
		{
			get
			{
				if (cashier == null)
				{
					return -1;
				}
				else
				{
					return cashier.CashierID;
				}
			}
			set
			{
				if (cashier != null)
				{
					this.cashier.CashierID = value;
				}
			}
		}
		public virtual string CashierCode
		{
			get
			{
				if (cashier == null)
				{
					return string.Empty;
				}
				else
				{
					return cashier.CashierCode;
				}
			}
			set
			{
				if (cashier != null)
				{
					this.cashier.CashierCode = value;
				}
			}
		}

		/// <summary>
		/// Get and set Language of Current CashierLang.
		/// </summary>
		private DbLanguage language;
		public virtual DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}
		public virtual short LanguageID
		{
			get
			{
				if (language == null)
				{
					return -1;
				}
				else
				{
					return language.Languageid;
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

		/// <summary>
		/// Get and set CashierName of Current CashierLang.
		/// </summary>
		private string cashierName;
		public virtual string CashierName
		{
			get { return this.cashierName; }
			set { this.cashierName = value; }
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
