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
	/// POJO for SuStatistic. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class SuStatistic
	{
		#region Fields
		
		private int statisticid;
		private long statisticNumber;
		private DateTime statisticDate;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the SuStatistic class
		/// </summary>
		public SuStatistic()
		{
		}

		public SuStatistic(int statisticid)
		{
			this.statisticid = statisticid;
		}
	
		/// <summary>
		/// Initializes a new instance of the SuStatistic class
		/// </summary>
		/// <param name="statisticNumber">Initial <see cref="SuStatistic.StatisticNumber" /> value</param>
		/// <param name="statisticDate">Initial <see cref="SuStatistic.StatisticDate" /> value</param>
		public SuStatistic(long statisticNumber, DateTime statisticDate)
		{
			this.statisticNumber = statisticNumber;
			this.statisticDate = statisticDate;
		}
	
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the Statisticid for the current SuStatistic
		/// </summary>
		public virtual int Statisticid
		{
			get { return this.statisticid; }
			set { this.statisticid = value; }
		}
		
		/// <summary>
		/// Gets or sets the StatisticNumber for the current SuStatistic
		/// </summary>
		public virtual long StatisticNumber
		{
			get { return this.statisticNumber; }
			set { this.statisticNumber = value; }
		}
		
		/// <summary>
		/// Gets or sets the StatisticDate for the current SuStatistic
		/// </summary>
		public virtual DateTime StatisticDate
		{
			get { return this.statisticDate; }
			set { this.statisticDate = value; }
		}
		
		#endregion
	}
}
