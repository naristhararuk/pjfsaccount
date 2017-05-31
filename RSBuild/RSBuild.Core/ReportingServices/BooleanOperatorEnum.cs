namespace Microsoft.SqlServer.ReportingServices
{
    using System;
    using System.Xml.Serialization;

    [XmlType(Namespace="http://schemas.microsoft.com/sqlserver/2003/12/reporting/reportingservices")]
    public enum BooleanOperatorEnum
    {
        // Fields
        And = 0,
        Or = 1
    }
}

