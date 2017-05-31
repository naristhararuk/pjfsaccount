using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.DB.DTO;

namespace SCG.FN.DTO
{
	[Serializable]
	public class FnPaymentTypeLang
	{
		#region Constructor
		public FnPaymentTypeLang()
		{
		
		}
		#endregion

		#region Property
		/// <summary>
		/// Get and set ID of Current PaymentTypeLang.
		/// </summary>
		private long id;
		public virtual long ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		/// <summary>
		/// Get and set PaymentType of Current PaymentTypeLang.
		/// </summary>
		private FnPaymentType paymentType;
		public virtual FnPaymentType PaymentType
		{
			get { return this.paymentType; }
			set { this.paymentType = value; }
		}

		/// <summary>
		/// Get and set PaymenyTypeID of Current PaymentTypeLang.
		/// </summary>
		public virtual short PaymentTypeID
		{
			get 
			{
				if (paymentType != null)
				{
					return this.paymentType.PaymentTypeID;
				}
				else
				{
					return -1;
				}
			}
			set 
			{
				if (paymentType != null)
				{
					this.paymentType.PaymentTypeID = value;
				}  
			}
		}
		/// <summary>
		/// Get and set PaymenyTypeCode of Current PaymentTypeLang.
		/// </summary>
		public virtual string PaymentTypeCode
		{
			get
			{
				if (paymentType != null)
				{
					return this.paymentType.Code;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				if (paymentType != null)
				{
					this.paymentType.Code = value;
				}
			}
		}

		/// <summary>
		/// Get and set Language of Current PaymentTypeLang.
		/// </summary>
		private DbLanguage language;
		public virtual DbLanguage Language
		{
			get { return this.language; }
			set { this.language = value; }
		}
		/// <summary>
		/// Get and set LanguageID of Current PaymentTypeLang.
		/// </summary>
		public virtual short LanguageID
		{
			get
			{
				if (language != null)
				{
					return this.language.Languageid;
				}
				else
				{
					return -1;
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
		/// Get and set LanguageCode of Current PaymentTypeLang.
		/// </summary>
		public virtual string LanguageCode
		{
			get
			{
				if (language != null)
				{
					return this.language.LanguageCode;
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				if (language != null)
				{
					this.language.LanguageCode = value;
				}
			}
		}

		/// <summary>
		/// Get and set Name of Current PaymentTypeLang.
		/// </summary>
		private string name;
		public virtual string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		/// <summary>
		/// Get and set Active of Current PaymentTypeLang.
		/// </summary>
		private bool active;
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}

		/// <summary>
		/// Get and set CreBy of Current PaymentTypeLang.
		/// </summary>
		private long creBy;
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}

		/// <summary>
		/// Get and set CreDate of Current PaymentTypeLang.
		/// </summary>
		private DateTime creDate;
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}

		/// <summary>
		/// Get and set UpdBy of Current PaymentTypeLang.
		/// </summary>
		private long updBy;
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}

		/// <summary>
		/// Get and set UpdDate of Current PaymentTypeLang.
		/// </summary>
		private DateTime updDate;
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}

		/// <summary>
		/// Get and set UpdPgm of Current PaymentTypeLang.
		/// </summary>
		private string updPgm;
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}

		/// <summary>
		/// Get and set RowVersion of Current PaymentTypeLang.
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
