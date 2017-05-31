
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
    /// Add. Contract Accounts Rec. and Payable Document Line Item
    /// </summary>
    //[RfcStructure(AbapName ="BAPIACCAIT" , Length = 154, Length2 = 292)]
    [Serializable]
    public class BAPIACCAIT //: SAPStructure
    {


        /// <summary>
        /// Accounting Document Line Item Number
        /// </summary>

        //[RfcField(AbapName = "ITEMNO_ACC", RfcType = RFCTYPE.RFCTYPE_NUM, Length = 10, Length2 = 20, Offset = 0, Offset2 = 0)]
        //[XmlElement("ITEMNO_ACC", Form=XmlSchemaForm.Unqualified)]
        public string Itemno_Acc
        {
            get
            {
                return _Itemno_Acc;
            }
            set
            {
                _Itemno_Acc = value;
            }
        }
        private string _Itemno_Acc;


        /// <summary>
        /// FI-CA: Contract Account Number
        /// </summary>

        //[RfcField(AbapName = "CONT_ACCT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 12, Length2 = 24, Offset = 10, Offset2 = 20)]
        //[XmlElement("CONT_ACCT", Form=XmlSchemaForm.Unqualified)]
        public string Cont_Acct
        {
            get
            {
                return _Cont_Acct;
            }
            set
            {
                _Cont_Acct = value;
            }
        }
        private string _Cont_Acct;


        /// <summary>
        /// FI-CA: Main Transaction for Line Item
        /// </summary>

        //[RfcField(AbapName = "MAIN_TRANS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 4, Length2 = 8, Offset = 22, Offset2 = 44)]
        //[XmlElement("MAIN_TRANS", Form=XmlSchemaForm.Unqualified)]
        public string Main_Trans
        {
            get
            {
                return _Main_Trans;
            }
            set
            {
                _Main_Trans = value;
            }
        }
        private string _Main_Trans;


        /// <summary>
        /// FI-CA: Subtransaction for Line Item
        /// </summary>

        //[RfcField(AbapName = "SUB_TRANS", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 4, Length2 = 8, Offset = 26, Offset2 = 52)]
        //[XmlElement("SUB_TRANS", Form=XmlSchemaForm.Unqualified)]
        public string Sub_Trans
        {
            get
            {
                return _Sub_Trans;
            }
            set
            {
                _Sub_Trans = value;
            }
        }
        private string _Sub_Trans;


        /// <summary>
        /// Functional Area
        /// </summary>

        //[RfcField(AbapName = "FUNC_AREA", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 4, Length2 = 8, Offset = 30, Offset2 = 60)]
        //[XmlElement("FUNC_AREA", Form=XmlSchemaForm.Unqualified)]
        public string Func_Area
        {
            get
            {
                return _Func_Area;
            }
            set
            {
                _Func_Area = value;
            }
        }
        private string _Func_Area;


        /// <summary>
        /// Financial Management Area
        /// </summary>

        //[RfcField(AbapName = "FM_AREA", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 4, Length2 = 8, Offset = 34, Offset2 = 68)]
        //[XmlElement("FM_AREA", Form=XmlSchemaForm.Unqualified)]
        public string Fm_Area
        {
            get
            {
                return _Fm_Area;
            }
            set
            {
                _Fm_Area = value;
            }
        }
        private string _Fm_Area;


        /// <summary>
        /// Commitment Item
        /// </summary>

        //[RfcField(AbapName = "CMMT_ITEM", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 14, Length2 = 28, Offset = 38, Offset2 = 76)]
        //[XmlElement("CMMT_ITEM", Form=XmlSchemaForm.Unqualified)]
        public string Cmmt_Item
        {
            get
            {
                return _Cmmt_Item;
            }
            set
            {
                _Cmmt_Item = value;
            }
        }
        private string _Cmmt_Item;


        /// <summary>
        /// Funds Center
        /// </summary>

        //[RfcField(AbapName = "FUNDS_CTR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 16, Length2 = 32, Offset = 52, Offset2 = 104)]
        //[XmlElement("FUNDS_CTR", Form=XmlSchemaForm.Unqualified)]
        public string Funds_Ctr
        {
            get
            {
                return _Funds_Ctr;
            }
            set
            {
                _Funds_Ctr = value;
            }
        }
        private string _Funds_Ctr;


        /// <summary>
        /// Fund
        /// </summary>

        //[RfcField(AbapName = "FUND", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 68, Offset2 = 136)]
        //[XmlElement("FUND", Form=XmlSchemaForm.Unqualified)]
        public string Fund
        {
            get
            {
                return _Fund;
            }
            set
            {
                _Fund = value;
            }
        }
        private string _Fund;


        /// <summary>
        /// UUID in X form (binary)
        /// </summary>

        //[RfcField(AbapName = "AGREEMENT_GUID", RfcType = RFCTYPE.RFCTYPE_BYTE, Length = 16, Length2 = 16, Offset = 78, Offset2 = 156)]
        //[XmlElement("AGREEMENT_GUID", Form=XmlSchemaForm.Unqualified)]
        public byte[] Agreement_Guid
        {
            get
            {
                return _Agreement_Guid;
            }
            set
            {
                _Agreement_Guid = value;
            }
        }
        private byte[] _Agreement_Guid;


        /// <summary>
        /// Functional Area
        /// </summary>

        //[RfcField(AbapName = "FUNC_AREA_LONG", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 16, Length2 = 32, Offset = 94, Offset2 = 172)]
        //[XmlElement("FUNC_AREA_LONG", Form=XmlSchemaForm.Unqualified)]
        public string Func_Area_Long
        {
            get
            {
                return _Func_Area_Long;
            }
            set
            {
                _Func_Area_Long = value;
            }
        }
        private string _Func_Area_Long;


        /// <summary>
        /// Commitment item
        /// </summary>

        //[RfcField(AbapName = "CMMT_ITEM_LONG", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 24, Length2 = 48, Offset = 110, Offset2 = 204)]
        //[XmlElement("CMMT_ITEM_LONG", Form=XmlSchemaForm.Unqualified)]
        public string Cmmt_Item_Long
        {
            get
            {
                return _Cmmt_Item_Long;
            }
            set
            {
                _Cmmt_Item_Long = value;
            }
        }
        private string _Cmmt_Item_Long;


        /// <summary>
        /// Grant
        /// </summary>

        //[RfcField(AbapName = "GRANT_NBR", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 20, Length2 = 40, Offset = 134, Offset2 = 252)]
        //[XmlElement("GRANT_NBR", Form=XmlSchemaForm.Unqualified)]
        public string Grant_Nbr
        {
            get
            {
                return _Grant_Nbr;
            }
            set
            {
                _Grant_Nbr = value;
            }
        }
        private string _Grant_Nbr;

        public void SetValue(IRfcStructure structure)
        {
            this.Itemno_Acc = structure.GetInt("ITEMNO_ACC").ToString();
            this.Cont_Acct = structure.GetString("CONT_ACCT");
            this.Main_Trans = structure.GetString("MAIN_TRANS");
            this.Sub_Trans = structure.GetString("SUB_TRANS");
            this.Func_Area = structure.GetString("FUNC_AREA");
            this.Fm_Area = structure.GetString("FM_AREA");
            this.Cmmt_Item = structure.GetString("CMMT_ITEM");
            this.Funds_Ctr = structure.GetString("FUNDS_CTR");
            this.Fund = structure.GetString("FUND");
            this.Agreement_Guid = structure.GetByteArray("AGREEMENT_GUID");
            this.Func_Area_Long = structure.GetString("FUNC_AREA_LONG");
            this.Cmmt_Item_Long = structure.GetString("CMMT_ITEM_LONG");
            this.Grant_Nbr = structure.GetString("GRANT_NBR");
        }

        public IRfcStructure GetStructure(RfcRepository repository)
        {
            IRfcStructure structure = repository.GetStructureMetadata("BAPIACCAIT").CreateStructure();

            structure.SetValue("ITEMNO_ACC", this.Itemno_Acc);
            structure.SetValue("CONT_ACCT", this.Cont_Acct);
            structure.SetValue("MAIN_TRANS", this.Main_Trans);
            structure.SetValue("SUB_TRANS", this.Sub_Trans);
            structure.SetValue("FUNC_AREA", this.Func_Area);
            structure.SetValue("FM_AREA", this.Fm_Area);
            structure.SetValue("CMMT_ITEM", this.Cmmt_Item);
            structure.SetValue("FUNDS_CTR", this.Funds_Ctr);
            structure.SetValue("FUND", this.Fund);
            structure.SetValue("AGREEMENT_GUID", this.Agreement_Guid);
            structure.SetValue("FUNC_AREA_LONG", this.Func_Area_Long);
            structure.SetValue("CMMT_ITEM_LONG", this.Cmmt_Item_Long);
            structure.SetValue("GRANT_NBR", this.Grant_Nbr);

            return structure;
        }

    }

}
