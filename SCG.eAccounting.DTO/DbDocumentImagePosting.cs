//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SCG.eAccounting.DTO
{
	/// <summary>
	/// POJO for DbDocumentImagePosting. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class DbDocumentImagePosting
	{
		#region Fields
		
		private long documentID;
		private string fIDocNumber;
		private string status;
		private string gUID;
		private string imageDocID;
		private string message;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the DbDocumentImagePosting class
		/// </summary>
		public DbDocumentImagePosting()
		{
		}

		public DbDocumentImagePosting(long documentID)
		{
			this.documentID = documentID;
		}
	
		/// <summary>
		/// Initializes a new instance of the DbDocumentImagePosting class
		/// </summary>
		/// <param name="documentID">Initial <see cref="DbDocumentImagePosting.DocumentID" /> value</param>
		/// <param name="fIDocNumber">Initial <see cref="DbDocumentImagePosting.FIDocNumber" /> value</param>
		/// <param name="status">Initial <see cref="DbDocumentImagePosting.Status" /> value</param>
		/// <param name="gUID">Initial <see cref="DbDocumentImagePosting.GUID" /> value</param>
		/// <param name="imageDocID">Initial <see cref="DbDocumentImagePosting.ImageDocID" /> value</param>
		/// <param name="message">Initial <see cref="DbDocumentImagePosting.Message" /> value</param>
		public DbDocumentImagePosting(long documentID, string fIDocNumber, string status, string gUID, string imageDocID, string message)
		{
			this.documentID = documentID;
			this.fIDocNumber = fIDocNumber;
			this.status = status;
			this.gUID = gUID;
			this.imageDocID = imageDocID;
			this.message = message;
		}
	
	#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the DocumentID for the current DbDocumentImagePosting
		/// </summary>
		public virtual long DocumentID
		{
			get { return this.documentID; }
			set { this.documentID = value; }
		}
		
		/// <summary>
		/// Gets or sets the FIDocNumber for the current DbDocumentImagePosting
		/// </summary>
		public virtual string FIDocNumber
		{
			get { return this.fIDocNumber; }
			set { this.fIDocNumber = value; }
		}
		
		/// <summary>
		/// Gets or sets the Status for the current DbDocumentImagePosting
		/// </summary>
		public virtual string Status
		{
			get { return this.status; }
			set { this.status = value; }
		}
		
		/// <summary>
		/// Gets or sets the GUID for the current DbDocumentImagePosting
		/// </summary>
		public virtual string GUID
		{
			get { return this.gUID; }
			set { this.gUID = value; }
		}
		
		/// <summary>
		/// Gets or sets the ImageDocID for the current DbDocumentImagePosting
		/// </summary>
		public virtual string ImageDocID
		{
			get { return this.imageDocID; }
			set { this.imageDocID = value; }
		}
		
		/// <summary>
		/// Gets or sets the Message for the current DbDocumentImagePosting
		/// </summary>
		public virtual string Message
		{
			get { return this.message; }
			set { this.message = value; }
		}
		
		#endregion
	}
}