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
	/// POJO for SuSession. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuSession
	{
		#region Fields
		
		private long userid;
		private string sessionid;
		private string ip;
		private DateTime timeStamp;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuSession class
		/// </summary>
		public SuSession()
		{
		}

		public SuSession(long userid)
		{
			this.userid = userid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuSession class
		/// </summary>
		/// <param name="userid">Initial <see cref="SuSession.Userid" /> value</param>
		/// <param name="sessionid">Initial <see cref="SuSession.Sessionid" /> value</param>
		/// <param name="ip">Initial <see cref="SuSession.Ip" /> value</param>
		/// <param name="timeStamp">Initial <see cref="SuSession.TimeStamp" /> value</param>
		public SuSession(long userid, string sessionid, string ip,  DateTime timeStamp)
		{
			this.userid = userid;
			this.sessionid = sessionid;
			this.ip = ip;
			this.timeStamp = timeStamp;
		}
	
		/// <summary>
		/// Minimal constructor for class SuSession
		/// </summary>
		/// <param name="userid">Initial <see cref="SuSession.Userid" /> value</param>
		/// <param name="sessionid">Initial <see cref="SuSession.Sessionid" /> value</param>
		/// <param name="timeStamp">Initial <see cref="SuSession.TimeStamp" /> value</param>
		public SuSession(long userid, string sessionid,DateTime timeStamp)
		{
			this.userid = userid;
			this.sessionid = sessionid;
			this.timeStamp = timeStamp;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the Userid for the current SuSession
		/// </summary>
		public virtual long Userid
		{
			get { return this.userid; }
			set { this.userid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Sessionid for the current SuSession
		/// </summary>
		public virtual string Sessionid
		{
			get { return this.sessionid; }
			set { this.sessionid = value; }
		}
		
		/// <summary>
		/// Gets or sets the Ip for the current SuSession
		/// </summary>
		public virtual string Ip
		{
			get { return this.ip; }
			set { this.ip = value; }
		}
		
	
		/// <summary>
		/// Gets or sets the TimeStamp for the current SuSession
		/// </summary>
		public virtual DateTime TimeStamp
		{
			get { return this.timeStamp; }
			set { this.timeStamp = value; }
		}
		
		#endregion
	}
}