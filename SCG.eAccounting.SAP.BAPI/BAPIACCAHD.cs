
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
    /// Add. Contract Accounts Recievable and Payable Header Line
    /// </summary>
    //[RfcStructure(AbapName ="BAPIACCAHD" , Length = 86, Length2 = 172)]
    [Serializable]
    public class BAPIACCAHD //: SAPStructure
    {


        /// <summary>
        /// FI CA: External Document Number for Contract AP/AR
        /// </summary>

        //[RfcField(AbapName = "DOC_NO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 12, Length2 = 24, Offset = 0, Offset2 = 0)]
        //[XmlElement("DOC_NO", Form=XmlSchemaForm.Unqualified)]
        public string Doc_No
        {
            get
            {
                return _Doc_No;
            }
            set
            {
                _Doc_No = value;
            }
        }
        private string _Doc_No;


        /// <summary>
        /// FI-CA: Doc.Type for Contract Accounts Receivable and Payable
        /// </summary>

        //[RfcField(AbapName = "DOC_TYPE_CA", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 12, Offset2 = 24)]
        //[XmlElement("DOC_TYPE_CA", Form=XmlSchemaForm.Unqualified)]
        public string Doc_Type_Ca
        {
            get
            {
                return _Doc_Type_Ca;
            }
            set
            {
                _Doc_Type_Ca = value;
            }
        }
        private string _Doc_Type_Ca;


        /// <summary>
        /// FI-CA: Reservation Key
        /// </summary>

        //[RfcField(AbapName = "RES_KEY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 14, Offset2 = 28)]
        //[XmlElement("RES_KEY", Form=XmlSchemaForm.Unqualified)]
        public string Res_Key
        {
            get
            {
                return _Res_Key;
            }
            set
            {
                _Res_Key = value;
            }
        }
        private string _Res_Key;


        /// <summary>
        /// FI-CA: Reconciliation Key for General Ledger Accounting
        /// </summary>

        //[RfcField(AbapName = "FIKEY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 12, Length2 = 24, Offset = 44, Offset2 = 88)]
        //[XmlElement("FIKEY", Form=XmlSchemaForm.Unqualified)]
        public string Fikey
        {
            get
            {
                return _Fikey;
            }
            set
            {
                _Fikey = value;
            }
        }
        private string _Fikey;


        /// <summary>
        /// FI-CA: Payment Form Reference
        /// </summary>

        //[RfcField(AbapName = "PAYMENT_FORM_REF", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 30, Length2 = 60, Offset = 56, Offset2 = 112)]
        //[XmlElement("PAYMENT_FORM_REF", Form=XmlSchemaForm.Unqualified)]
        public string Payment_Form_Ref
        {
            get
            {
                return _Payment_Form_Ref;
            }
            set
            {
                _Payment_Form_Ref = value;
            }
        }
        private string _Payment_Form_Ref;

        public void SetValue(IRfcStructure structure)
        {
            this.Doc_No = structure.GetString("DOC_NO");
            this.Doc_Type_Ca = structure.GetString("DOC_TYPE_CA");
            this.Res_Key = structure.GetString("RES_KEY");
            this.Fikey = structure.GetString("FIKEY");
            this.Payment_Form_Ref = structure.GetString("PAYMENT_FORM_REF");
        }

        public IRfcStructure GetStructure(RfcRepository repository)
        {
            IRfcStructure structure = repository.GetStructureMetadata("BAPIACCAHD").CreateStructure();

            structure.SetValue("DOC_NO", this.Doc_No);
            structure.SetValue("DOC_TYPE_CA", this.Doc_Type_Ca);
            structure.SetValue("RES_KEY", this.Res_Key);
            structure.SetValue("FIKEY", this.Fikey);
            structure.SetValue("PAYMENT_FORM_REF", this.Payment_Form_Ref);

            return structure;
        }
    }

}
