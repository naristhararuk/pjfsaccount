
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a SAP. NET Connector Proxy Generator Version 2.0
//     Created at 2/4/2552
//     Created from Windows
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using SAP.Middleware.Connector;
//using SAP.Connector;

namespace SCG.eAccounting.SAP.BAPI
{

    /// <summary>
    /// Client SAP proxy class
    /// </summary>
    //[WebServiceBinding(Name = "dummy.Binding", Namespace = "urn:sap-com:document:sap:rfc:functions")]
    [Serializable]
    public class SAPProxy_YBAPI_UPD_INSTKEY1 //: SAPClient
    {
        /*
        /// <summary>
        /// Initializes a new SAPProxy_YBAPI_UPD_INSTKEY1.
        /// </summary>
        public SAPProxy_YBAPI_UPD_INSTKEY1() { }

        /// <summary>
        /// Initializes a new SAPProxy_YBAPI_UPD_INSTKEY1 with a new connection based on the specified connection string.
        /// </summary>
        /// <param name="connectionString">A connection string (e.g. RFC or URL) specifying the system where the proxy should connect to.</param>
        public SAPProxy_YBAPI_UPD_INSTKEY1(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Initializes a new SAPProxy_YBAPI_UPD_INSTKEY1 instance and adds it to the given container.
        /// This allows automated connection mananged by VS component designer:
        /// If container is disposed, it will also dispose this SAPClient instance,
        /// which will dispose a contained connection if needed.
        /// </summary>
        /// <param name="container"<The container where the new SAPClient instance is to be added.>/param<
        public SAPProxy_YBAPI_UPD_INSTKEY1(System.ComponentModel.IContainer container) : base(container) { }

        /// <summary>
        /// Remote Function Module YBAPI_UPD_INSTKEY1.  
        /// Update eForm Id to Accounting Doc.
        /// </summary>
        /// <param name="Acckey_Tab">Key of Accounting Doc</param>
        /// <param name="Return0">Return Parameter</param>
        [RfcMethod(AbapName = "YBAPI_UPD_INSTKEY1")]
        [SoapDocumentMethodAttribute("http://tempuri.org/YBAPI_UPD_INSTKEY1",
         RequestNamespace = "urn:sap-com:document:sap:rfc:functions",
         RequestElementName = "YBAPI_UPD_INSTKEY1",
         ResponseNamespace = "urn:sap-com:document:sap:rfc:functions",
         ResponseElementName = "YBAPI_UPD_INSTKEY1.Response")]
        public virtual void Ybapi_Upd_Instkey1(

         [RfcParameter(AbapName = "ACCKEY_TAB", RfcType = RFCTYPE.RFCTYPE_ITAB, Optional = true, Direction = RFCINOUT.INOUT)]
     [XmlArray("ACCKEY_TAB", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     [XmlArrayItem("item", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     ref ZACCKEY2Table Acckey_Tab,
     [RfcParameter(AbapName = "RETURN", RfcType = RFCTYPE.RFCTYPE_ITAB, Optional = true, Direction = RFCINOUT.INOUT)]
     [XmlArray("RETURN", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     [XmlArrayItem("item", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     ref BAPIRET2Table Return0)
        {
            object[] results = null;
            results = this.SAPInvoke("Ybapi_Upd_Instkey1", new object[] {
                            Acckey_Tab,Return0 });
            Acckey_Tab = (ZACCKEY2Table)results[0];
            Return0 = (BAPIRET2Table)results[1];

        }
         */

        private RfcDestination destination;

        public SAPProxy_YBAPI_UPD_INSTKEY1() { }

        public SAPProxy_YBAPI_UPD_INSTKEY1(RfcDestination destination)
        {
            this.destination = destination;
        }

        /// <summary>
        /// Remote Function Module YBAPI_UPD_INSTKEY1.  
        /// Update eForm Id to Accounting Doc.
        /// </summary>
        /// <param name="Acckey_Tab">Key of Accounting Doc</param>
        /// <param name="Return0">Return Parameter</param>
        //[RfcMethod(AbapName = "YBAPI_UPD_INSTKEY1")]
        //[SoapDocumentMethodAttribute("http://tempuri.org/YBAPI_UPD_INSTKEY1",
        // RequestNamespace = "urn:sap-com:document:sap:rfc:functions",
        // RequestElementName = "YBAPI_UPD_INSTKEY1",
        // ResponseNamespace = "urn:sap-com:document:sap:rfc:functions",
        // ResponseElementName = "YBAPI_UPD_INSTKEY1.Response")]
        public virtual void Ybapi_Upd_Instkey1(

     //    [RfcParameter(AbapName = "ACCKEY_TAB", RfcType = RFCTYPE.RFCTYPE_ITAB, Optional = true, Direction = RFCINOUT.INOUT)]
     //[XmlArray("ACCKEY_TAB", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     //[XmlArrayItem("item", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     ref ZACCKEY2Table Acckey_Tab,
     //[RfcParameter(AbapName = "RETURN", RfcType = RFCTYPE.RFCTYPE_ITAB, Optional = true, Direction = RFCINOUT.INOUT)]
     //[XmlArray("RETURN", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     //[XmlArrayItem("item", IsNullable = false, Form = XmlSchemaForm.Unqualified)]
     ref BAPIRET2Table Return0)
        {
            //object[] results = null;
            //results = this.SAPInvoke("Ybapi_Upd_Instkey1", new object[] {
            //                Acckey_Tab,Return0 });
            //Acckey_Tab = (ZACCKEY2Table)results[0];
            //Return0 = (BAPIRET2Table)results[1];

            IRfcFunction function = destination.Repository.CreateFunction("BAPI_UPD_INSTKEY1");

            #region set parameter value

            function.SetValue("ACCKEY_TAB", Acckey_Tab.GetTable(destination.Repository));
            function.SetValue("RETURN", Return0.GetTable(destination.Repository));

            #endregion

            function.Invoke(destination);


            Acckey_Tab.SetValue(function.GetTable("ACCKEY_TAB"));//Acckey_Tab = (ZACCKEY2Table)results[0];
            Return0.SetValue(function.GetTable("RETURN"));//Return0 = (BAPIRET2Table)results[1];
        }

    }

}
