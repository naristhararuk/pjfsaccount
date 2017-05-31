namespace Microsoft.SqlServer.ReportingServices
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://schemas.microsoft.com/sqlserver/2003/12/reporting/reportingservices")]
    public class Extension
    {
        // Methods
        public Extension()
        {
        }


        // Fields
        public ExtensionTypeEnum ExtensionType;
        public string LocalizedName;
        public string Name;
        public bool Visible;
    }
}

