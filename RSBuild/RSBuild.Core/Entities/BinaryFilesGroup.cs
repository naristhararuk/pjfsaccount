namespace RSBuild
{
    using System;

    /// <summary>
	/// Represents a report group.
	/// </summary>
	[Serializable]
	public class BinaryFilesGroup
	{
		private string _Name;
		private string _TargetFolder;
	//	private DataSource _DataSource;
		private ReportServerInfo _ReportServerInfo;
		private BinaryFile[] _BinaryFiles;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
		{
			get
			{
				return _Name;
			}
		}

        /// <summary>
        /// Gets the target folder.
        /// </summary>
        /// <value>The target folder.</value>
		public string TargetFolder
		{
			get
			{
				return _TargetFolder;
			}
		}


        /// <summary>
        /// Gets the report server.
        /// </summary>
        /// <value>The report server.</value>
        public ReportServerInfo ReportServer
        {
            get
            {
                return _ReportServerInfo;
            }
        }

        /// <summary>
        /// Gets the reports.
        /// </summary>
        /// <value>The reports.</value>
		public BinaryFile[] BinaryFiles
		{
			get
			{
				return _BinaryFiles;
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryFilesGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="targetFolder">The target folder.</param>
        /// <param name="dataSourceName">Name of the data source.</param>
        /// <param name="reportServer">The report server.</param>
        /// <param name="reports">The reports.</param>
		public BinaryFilesGroup(string name, string targetFolder, string reportServer, BinaryFile[] reports)
		{
			_Name = name;
			_TargetFolder = targetFolder;
            //if (dataSourceName != null)
            //{
            //    if (Settings.DataSources.ContainsKey(dataSourceName))
            //    {
            //        _DataSource = (DataSource)Settings.DataSources[dataSourceName];
            //    }
            //}
			if (reportServer != null)
			{
				if (Settings.ReportServers.ContainsKey(reportServer))
				{
                    _ReportServerInfo = (ReportServerInfo)Settings.ReportServers[reportServer];
				}
			}
			if (targetFolder != null && targetFolder.Length > 0)
			{
				_TargetFolder = targetFolder.Trim();
			}
			if (reports != null && reports.Length > 0)
			{
				_BinaryFiles = reports;
			}

		}
	}
}
