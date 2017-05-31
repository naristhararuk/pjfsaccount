namespace RSBuild
{
	using System;
    using Microsoft.SqlServer.ReportingServices;

    /// <summary>
    /// Proxy wrapper.
    /// </summary>
	public class WSWrapper2003 : IWSWrapper
	{
        private const string SERVICE_NAME = "ReportService.asmx";
        private ReportingService _proxy;

		// Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="WSWrapper"/> class.
        /// </summary>
        /// <param name="reportServer">The report server.</param>
		private WSWrapper2003(ReportingService proxy)
		{
            _proxy = proxy;
		}

        public static bool TryCreate(ReportServerInfo reportServer, out IWSWrapper result, out Exception exception)
        {
            ReportingService proxy = new ReportingService()
            {
                Url = reportServer.GetServiceUrl(SERVICE_NAME),
                Timeout = reportServer.Timeout ?? -1,
                Credentials = reportServer.CreateCredentials(SERVICE_NAME)
            };

            try
            {
                proxy.ListSecureMethods();
                result = new WSWrapper2003(proxy);
                exception = null;
                return true;
            }
            catch (Exception e)
            {
                proxy.Dispose();
                result = null;
                exception = e;
                return false;
            }
        }

        public void Dispose()
        {
            _proxy.Dispose();
        }

        public void CreateFolder(string Folder, string Parent)
        {
            _proxy.CreateFolder(Folder, Parent, null);
        }

        public void CreateDataSource(DataSource source)
        {
            DataSourceDefinition definition = new DataSourceDefinition();
            definition.ConnectString = source.RSConnectionString;
            definition.CredentialRetrieval = (CredentialRetrievalEnum)source.CredentialRetrieval;
            if (source.UserName != null)
            {
                definition.UserName = source.UserName;
                definition.Password = source.Password;
            }
            definition.Enabled = true;
            definition.EnabledSpecified = true;
            definition.Extension = "SQL";
            definition.ImpersonateUser = false;
            definition.ImpersonateUserSpecified = true;
            definition.Prompt = null;
            definition.WindowsCredentials = source.WindowsCredentials;

            _proxy.CreateDataSource(
                source.Name,
                Util.FormatPath(source.TargetFolder),
                source.Overwrite,
                definition,
                null);

        }

        public void CreateReport(ReportGroup reportGroup, Report report,int seq)
        {
			byte[] definition = report.Process(reportGroup.TargetFolder, reportGroup.DataSource);
            if (definition == null) return;

            Warning[] warnings = _proxy.CreateReport(
                report.Name,
                Util.FormatPath(reportGroup.TargetFolder),
                true,
                definition,
                null);

            if (warnings != null)
            {
                Logger.LogMessage(string.Format("{0} Report:[{1}] / [{2}] published successfully with some warnings",seq, reportGroup.Name, report.Name));
            }
            else
            {
                Logger.LogMessage(string.Format("{0} Report:[{1}] / [{2}] published successfully with no warnings",seq, reportGroup.Name, report.Name));
            }

            if (report.CacheOption != null
                && report.CacheOption.CacheReport 
                && report.CacheOption.ExpirationMinutes != null)
            {
                _proxy.SetCacheOptions(
                    string.Format("{0}/{1}", Util.FormatPath(reportGroup.TargetFolder), report.Name),
                    true,
                    new TimeExpiration() { Minutes = report.CacheOption.ExpirationMinutes.Value });
            }
        }

        #region IWSWrapper Members


        public void CreateBinaryFile(BinaryFilesGroup reportGroup, BinaryFile report,int seq)
        {
            //here it my code : apidesh
            byte[] definition = report.Process(reportGroup.TargetFolder);
            if (definition == null) return;
            string filesPath = report.FilePath;

            int startIndex = filesPath.LastIndexOf('\\') + 1;
            int getIndex = filesPath.Length - (startIndex);
            string ItemName = filesPath.Substring(startIndex, getIndex);

            string[] name = ItemName.Split(new char[] {'.'});
            string fileType = name[1].ToString().ToLower();
            if(fileType.Equals("exe"))
            {
                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, "application/octet-stream" + fileType, new Property[] { });
            }
            else if (fileType.Equals("doc") || fileType.Equals("docx"))
            {
                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, "application/msword" + fileType, new Property[] { });

            } else if (fileType.Equals("pdf"))
            {
                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, "application/pdf" + fileType, new Property[] { });

            }
            else if (fileType.Equals("xls") || fileType.Equals("xlsx"))
            {

                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, "application/vnd.ms-excel" + fileType, new Property[] { });
            }
            else if (fileType.Equals("txt"))
            {

                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, "text/plain" + fileType, new Property[] { });
            }
            else if (fileType.Equals("jpg") || fileType.Equals("gif") || fileType.Equals("bmp") || fileType.Equals("jpeg"))
            {
                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, "image/" + fileType, new Property[] { });
            }
            else
            {
                _proxy.CreateResource(report.Name + "." + fileType, "/" + reportGroup.TargetFolder, true, definition, fileType, new Property[] { });
            }
            Logger.LogMessage(string.Format("{0} Binary File :[{1}] / [{2}] published successfully ", seq, reportGroup.Name, report.Name));
        }

        #endregion
    }
}