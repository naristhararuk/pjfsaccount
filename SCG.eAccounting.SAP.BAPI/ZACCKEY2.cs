
//------------------------------------------------------------------------------
// 
//     This code was generated by a SAP. NET Connector Proxy Generator Version 2.0
//     Created at 2/4/2552
//     Created from Windows
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// 
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Collections;
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
    /// Key of Accounting Doc
    /// </summary>
    //[RfcStructure(AbapName ="ZACCKEY2" , Length = 33, Length2 = 66)]
    [Serializable]
    public class ZACCKEY2 //: SAPStructure
    {


        /// <summary>
        /// Company Code
        /// </summary>

        //[RfcField(AbapName = "BUKRS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 4, Length2 = 8, Offset = 0, Offset2 = 0)]
        //[XmlElement("BUKRS", Form=XmlSchemaForm.Unqualified)]
        public string Bukrs
        {
            get
            {
                return _Bukrs;
            }
            set
            {
                _Bukrs = value;
            }
        }
        private string _Bukrs;


        /// <summary>
        /// Accounting Document Number
        /// </summary>

        //[RfcField(AbapName = "BELNR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 4, Offset2 = 8)]
        //[XmlElement("BELNR", Form=XmlSchemaForm.Unqualified)]
        public string Belnr
        {
            get
            {
                return _Belnr;
            }
            set
            {
                _Belnr = value;
            }
        }
        private string _Belnr;


        /// <summary>
        /// Fiscal Year
        /// </summary>

        //[RfcField(AbapName = "GJAHR", RfcType = RFCTYPE.RFCTYPE_NUM, Length = 4, Length2 = 8, Offset = 14, Offset2 = 28)]
        //[XmlElement("GJAHR", Form=XmlSchemaForm.Unqualified)]
        public string Gjahr
        {
            get
            {
                return _Gjahr;
            }
            set
            {
                _Gjahr = value;
            }
        }
        private string _Gjahr;


        /// <summary>
        /// Number of Line Item Within Accounting Document
        /// </summary>

        //[RfcField(AbapName = "BUZEI", RfcType = RFCTYPE.RFCTYPE_NUM, Length = 3, Length2 = 6, Offset = 18, Offset2 = 36)]
        //[XmlElement("BUZEI", Form=XmlSchemaForm.Unqualified)]
        public string Buzei
        {
            get
            {
                return _Buzei;
            }
            set
            {
                _Buzei = value;
            }
        }
        private string _Buzei;


        /// <summary>
        /// efrom ID
        /// </summary>

        //[RfcField(AbapName = "EFORMID", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 21, Offset2 = 42)]
        //[XmlElement("EFORMID", Form=XmlSchemaForm.Unqualified)]
        public string Eformid
        {
            get
            {
                return _Eformid;
            }
            set
            {
                _Eformid = value;
            }
        }
        private string _Eformid;


        /// <summary>
        /// Approve type
        /// </summary>

        //[RfcField(AbapName = "APP_TYPE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 31, Offset2 = 62)]
        //[XmlElement("APP_TYPE", Form=XmlSchemaForm.Unqualified)]
        public string App_Type
        {
            get
            {
                return _App_Type;
            }
            set
            {
                _App_Type = value;
            }
        }
        private string _App_Type;


        /// <summary>
        /// Approve type flag
        /// </summary>

        //[RfcField(AbapName = "APPV_TYPE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 32, Offset2 = 64)]
        //[XmlElement("APPV_TYPE", Form=XmlSchemaForm.Unqualified)]
        public string Appv_Type
        {
            get
            {
                return _Appv_Type;
            }
            set
            {
                _Appv_Type = value;
            }
        }
        private string _Appv_Type;

        public void SetValue(IRfcStructure structure)
        {
            this.Bukrs = structure.GetString("BUKRS");
            this.Belnr = structure.GetString("BELNR");
            this.Gjahr = structure.GetString("GJAHR");
            this.Buzei = structure.GetString("BUZEI");
            this.Eformid = structure.GetString("EFORMID");
            this.App_Type = structure.GetString("APP_TYPE");
            this.Appv_Type = structure.GetString("APPV_TYPE");
        }

        public IRfcStructure GetStructure(RfcRepository repository)
        {
            IRfcStructure structure = repository.GetStructureMetadata("ZACCKEY2").CreateStructure();

            structure.SetValue("BUKRS", this.Bukrs);
            structure.SetValue("BELNR", this.Belnr);
            structure.SetValue("GJAHR", this.Gjahr);
            structure.SetValue("BUZEI", this.Buzei);
            structure.SetValue("EFORMID", this.Eformid);
            structure.SetValue("APP_TYPE", this.App_Type);
            structure.SetValue("APPV_TYPE", this.Appv_Type);

            return structure;
        }
    }

}
