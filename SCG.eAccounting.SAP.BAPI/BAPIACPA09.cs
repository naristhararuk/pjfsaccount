
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
    /// Posting in accounting: Partner billing doc (load receivable)
    /// </summary>
    //[RfcStructure(AbapName ="BAPIACPA09" , Length = 353, Length2 = 706)]
    [Serializable]
    public class BAPIACPA09 //: SAPStructure
    {


        /// <summary>
        /// Name 1
        /// </summary>

        //[RfcField(AbapName = "NAME", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 35, Length2 = 70, Offset = 0, Offset2 = 0)]
        //[XmlElement("NAME", Form=XmlSchemaForm.Unqualified)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Name;


        /// <summary>
        /// Name 2
        /// </summary>

        //[RfcField(AbapName = "NAME_2", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 35, Length2 = 70, Offset = 35, Offset2 = 70)]
        //[XmlElement("NAME_2", Form=XmlSchemaForm.Unqualified)]
        public string Name_2
        {
            get
            {
                return _Name_2;
            }
            set
            {
                _Name_2 = value;
            }
        }
        private string _Name_2;


        /// <summary>
        /// Name 3
        /// </summary>

        //[RfcField(AbapName = "NAME_3", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 35, Length2 = 70, Offset = 70, Offset2 = 140)]
        //[XmlElement("NAME_3", Form=XmlSchemaForm.Unqualified)]
        public string Name_3
        {
            get
            {
                return _Name_3;
            }
            set
            {
                _Name_3 = value;
            }
        }
        private string _Name_3;


        /// <summary>
        /// Name 4
        /// </summary>

        //[RfcField(AbapName = "NAME_4", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 35, Length2 = 70, Offset = 105, Offset2 = 210)]
        //[XmlElement("NAME_4", Form=XmlSchemaForm.Unqualified)]
        public string Name_4
        {
            get
            {
                return _Name_4;
            }
            set
            {
                _Name_4 = value;
            }
        }
        private string _Name_4;


        /// <summary>
        /// Postal Code
        /// </summary>

        //[RfcField(AbapName = "POSTL_CODE", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 140, Offset2 = 280)]
        //[XmlElement("POSTL_CODE", Form=XmlSchemaForm.Unqualified)]
        public string Postl_Code
        {
            get
            {
                return _Postl_Code;
            }
            set
            {
                _Postl_Code = value;
            }
        }
        private string _Postl_Code;


        /// <summary>
        /// City
        /// </summary>

        //[RfcField(AbapName = "CITY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 35, Length2 = 70, Offset = 150, Offset2 = 300)]
        //[XmlElement("CITY", Form=XmlSchemaForm.Unqualified)]
        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City = value;
            }
        }
        private string _City;


        /// <summary>
        /// Country Key
        /// </summary>

        //[RfcField(AbapName = "COUNTRY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 185, Offset2 = 370)]
        //[XmlElement("COUNTRY", Form=XmlSchemaForm.Unqualified)]
        public string Country
        {
            get
            {
                return _Country;
            }
            set
            {
                _Country = value;
            }
        }
        private string _Country;


        /// <summary>
        /// Country key in ISO code
        /// </summary>

        //[RfcField(AbapName = "COUNTRY_ISO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 188, Offset2 = 376)]
        //[XmlElement("COUNTRY_ISO", Form=XmlSchemaForm.Unqualified)]
        public string Country_Iso
        {
            get
            {
                return _Country_Iso;
            }
            set
            {
                _Country_Iso = value;
            }
        }
        private string _Country_Iso;


        /// <summary>
        /// House number and street
        /// </summary>

        //[RfcField(AbapName = "STREET", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 35, Length2 = 70, Offset = 190, Offset2 = 380)]
        //[XmlElement("STREET", Form=XmlSchemaForm.Unqualified)]
        public string Street
        {
            get
            {
                return _Street;
            }
            set
            {
                _Street = value;
            }
        }
        private string _Street;


        /// <summary>
        /// PO Box
        /// </summary>

        //[RfcField(AbapName = "PO_BOX", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 225, Offset2 = 450)]
        //[XmlElement("PO_BOX", Form=XmlSchemaForm.Unqualified)]
        public string Po_Box
        {
            get
            {
                return _Po_Box;
            }
            set
            {
                _Po_Box = value;
            }
        }
        private string _Po_Box;


        /// <summary>
        /// P.O. Box Postal Code
        /// </summary>

        //[RfcField(AbapName = "POBX_PCD", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 10, Length2 = 20, Offset = 235, Offset2 = 470)]
        //[XmlElement("POBX_PCD", Form=XmlSchemaForm.Unqualified)]
        public string Pobx_Pcd
        {
            get
            {
                return _Pobx_Pcd;
            }
            set
            {
                _Pobx_Pcd = value;
            }
        }
        private string _Pobx_Pcd;


        /// <summary>
        /// Post office bank current account number
        /// </summary>

        //[RfcField(AbapName = "POBK_CURAC", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 16, Length2 = 32, Offset = 245, Offset2 = 490)]
        //[XmlElement("POBK_CURAC", Form=XmlSchemaForm.Unqualified)]
        public string Pobk_Curac
        {
            get
            {
                return _Pobk_Curac;
            }
            set
            {
                _Pobk_Curac = value;
            }
        }
        private string _Pobk_Curac;


        /// <summary>
        /// Bank Account Number
        /// </summary>

        //[RfcField(AbapName = "BANK_ACCT", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 18, Length2 = 36, Offset = 261, Offset2 = 522)]
        //[XmlElement("BANK_ACCT", Form=XmlSchemaForm.Unqualified)]
        public string Bank_Acct
        {
            get
            {
                return _Bank_Acct;
            }
            set
            {
                _Bank_Acct = value;
            }
        }
        private string _Bank_Acct;


        /// <summary>
        /// Bank number
        /// </summary>

        //[RfcField(AbapName = "BANK_NO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 15, Length2 = 30, Offset = 279, Offset2 = 558)]
        //[XmlElement("BANK_NO", Form=XmlSchemaForm.Unqualified)]
        public string Bank_No
        {
            get
            {
                return _Bank_No;
            }
            set
            {
                _Bank_No = value;
            }
        }
        private string _Bank_No;


        /// <summary>
        /// Bank country key
        /// </summary>

        //[RfcField(AbapName = "BANK_CTRY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 294, Offset2 = 588)]
        //[XmlElement("BANK_CTRY", Form=XmlSchemaForm.Unqualified)]
        public string Bank_Ctry
        {
            get
            {
                return _Bank_Ctry;
            }
            set
            {
                _Bank_Ctry = value;
            }
        }
        private string _Bank_Ctry;


        /// <summary>
        /// Bank country key in ISO code
        /// </summary>

        //[RfcField(AbapName = "BANK_CTRY_ISO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 297, Offset2 = 594)]
        //[XmlElement("BANK_CTRY_ISO", Form=XmlSchemaForm.Unqualified)]
        public string Bank_Ctry_Iso
        {
            get
            {
                return _Bank_Ctry_Iso;
            }
            set
            {
                _Bank_Ctry_Iso = value;
            }
        }
        private string _Bank_Ctry_Iso;


        /// <summary>
        /// Tax Number 1
        /// </summary>

        //[RfcField(AbapName = "TAX_NO_1", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 16, Length2 = 32, Offset = 299, Offset2 = 598)]
        //[XmlElement("TAX_NO_1", Form=XmlSchemaForm.Unqualified)]
        public string Tax_No_1
        {
            get
            {
                return _Tax_No_1;
            }
            set
            {
                _Tax_No_1 = value;
            }
        }
        private string _Tax_No_1;


        /// <summary>
        /// Tax Number 2
        /// </summary>

        //[RfcField(AbapName = "TAX_NO_2", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 11, Length2 = 22, Offset = 315, Offset2 = 630)]
        //[XmlElement("TAX_NO_2", Form=XmlSchemaForm.Unqualified)]
        public string Tax_No_2
        {
            get
            {
                return _Tax_No_2;
            }
            set
            {
                _Tax_No_2 = value;
            }
        }
        private string _Tax_No_2;


        public string Tax_No_3
        {
            get
            {
                return _Tax_No_3;
            }
            set
            {
                _Tax_No_3 = value;
            }
        }
        private string _Tax_No_3;


        public string Tax_No_4
        {
            get
            {
                return _Tax_No_4;
            }
            set
            {
                _Tax_No_4 = value;
            }
        }
        private string _Tax_No_4;

        /// <summary>
        /// Liable for VAT
        /// </summary>

        //[RfcField(AbapName = "TAX", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 326, Offset2 = 652)]
        //[XmlElement("TAX", Form=XmlSchemaForm.Unqualified)]
        public string Tax
        {
            get
            {
                return _Tax;
            }
            set
            {
                _Tax = value;
            }
        }
        private string _Tax;


        /// <summary>
        /// Indicator: Business partner subject to equalization tax ?
        /// </summary>

        //[RfcField(AbapName = "EQUAL_TAX", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 327, Offset2 = 654)]
        //[XmlElement("EQUAL_TAX", Form=XmlSchemaForm.Unqualified)]
        public string Equal_Tax
        {
            get
            {
                return _Equal_Tax;
            }
            set
            {
                _Equal_Tax = value;
            }
        }
        private string _Equal_Tax;


        /// <summary>
        /// Region (State, Province, County)
        /// </summary>

        //[RfcField(AbapName = "REGION", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 3, Length2 = 6, Offset = 328, Offset2 = 656)]
        //[XmlElement("REGION", Form=XmlSchemaForm.Unqualified)]
        public string Region
        {
            get
            {
                return _Region;
            }
            set
            {
                _Region = value;
            }
        }
        private string _Region;


        /// <summary>
        /// Bank Control Key
        /// </summary>

        //[RfcField(AbapName = "CTRL_KEY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 331, Offset2 = 662)]
        //[XmlElement("CTRL_KEY", Form=XmlSchemaForm.Unqualified)]
        public string Ctrl_Key
        {
            get
            {
                return _Ctrl_Key;
            }
            set
            {
                _Ctrl_Key = value;
            }
        }
        private string _Ctrl_Key;


        /// <summary>
        /// Instruction key for data medium exchange
        /// </summary>

        //[RfcField(AbapName = "INSTR_KEY", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 333, Offset2 = 666)]
        //[XmlElement("INSTR_KEY", Form=XmlSchemaForm.Unqualified)]
        public string Instr_Key
        {
            get
            {
                return _Instr_Key;
            }
            set
            {
                _Instr_Key = value;
            }
        }
        private string _Instr_Key;


        /// <summary>
        /// Report key for data medium exchange
        /// </summary>

        //[RfcField(AbapName = "DME_IND", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 1, Length2 = 2, Offset = 335, Offset2 = 670)]
        //[XmlElement("DME_IND", Form=XmlSchemaForm.Unqualified)]
        public string Dme_Ind
        {
            get
            {
                return _Dme_Ind;
            }
            set
            {
                _Dme_Ind = value;
            }
        }
        private string _Dme_Ind;


        /// <summary>
        /// Language according to ISO 639
        /// </summary>

        //[RfcField(AbapName = "LANGU_ISO", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 2, Length2 = 4, Offset = 336, Offset2 = 672)]
        //[XmlElement("LANGU_ISO", Form=XmlSchemaForm.Unqualified)]
        public string Langu_Iso
        {
            get
            {
                return _Langu_Iso;
            }
            set
            {
                _Langu_Iso = value;
            }
        }
        private string _Langu_Iso;


        /// <summary>
        /// Title
        /// </summary>

        //[RfcField(AbapName = "ANRED", RfcType = RFCTYPE.RFCTYPE_CHAR, Length = 15, Length2 = 30, Offset = 338, Offset2 = 676)]
        //[XmlElement("ANRED", Form=XmlSchemaForm.Unqualified)]
        public string Anred
        {
            get
            {
                return _Anred;
            }
            set
            {
                _Anred = value;
            }
        }
        private string _Anred;

        public void SetValue(IRfcStructure structure, bool useEcc)
        {
            this.Name = structure.GetString("NAME");
            this.Name_2 = structure.GetString("NAME_2");
            this.Name_3 = structure.GetString("NAME_3");
            this.Name_4 = structure.GetString("NAME_4");
            this.Postl_Code = structure.GetString("POSTL_CODE");
            this.City = structure.GetString("CITY");
            this.Country = structure.GetString("COUNTRY");
            this.Country_Iso = structure.GetString("COUNTRY_ISO");
            this.Po_Box = structure.GetString("PO_BOX");
            this.Pobx_Pcd = structure.GetString("POBX_PCD");
            this.Pobk_Curac = structure.GetString("POBK_CURAC");
            this.Bank_Acct = structure.GetString("BANK_ACCT");
            this.Bank_No = structure.GetString("BANK_NO");
            this.Bank_Ctry = structure.GetString("BANK_CTRY");
            this.Bank_Ctry_Iso = structure.GetString("BANK_CTRY_ISO");
            this.Tax_No_1 = structure.GetString("TAX_NO_1");
            this.Tax_No_2 = structure.GetString("TAX_NO_2");

            if (useEcc)
            {
                this.Tax_No_3 = structure.GetString("TAX_NO_3");
                this.Tax_No_4 = structure.GetString("TAX_NO_4");
            }

            this.Tax = structure.GetString("TAX");
            this.Equal_Tax = structure.GetString("EQUAL_TAX");
            this.Region = structure.GetString("REGION");
            this.Ctrl_Key = structure.GetString("CTRL_KEY");
            this.Instr_Key = structure.GetString("INSTR_KEY");
            this.Dme_Ind = structure.GetString("DME_IND");
            this.Langu_Iso = structure.GetString("LANGU_ISO");
            this.Anred = structure.GetString("ANRED");
        }

        public IRfcStructure GetStructure(RfcRepository repository, bool useEcc)
        {
            IRfcStructure structure = repository.GetStructureMetadata("BAPIACPA09").CreateStructure();

            structure.SetValue("NAME", this.Name);
            structure.SetValue("NAME_2", this.Name_2);
            structure.SetValue("NAME_3", this.Name_3);
            structure.SetValue("NAME_4", this.Name_4);
            structure.SetValue("POSTL_CODE", this.Postl_Code);
            structure.SetValue("CITY", this.City);
            structure.SetValue("COUNTRY", this.Country);
            structure.SetValue("STREET", this.Street);
            structure.SetValue("COUNTRY_ISO", this.Country_Iso);
            structure.SetValue("PO_BOX", this.Po_Box);
            structure.SetValue("POBX_PCD", this.Pobx_Pcd);
            structure.SetValue("POBK_CURAC", this.Pobk_Curac);
            structure.SetValue("BANK_ACCT", this.Bank_Acct);
            structure.SetValue("BANK_NO", this.Bank_No);
            structure.SetValue("BANK_CTRY", this.Bank_Ctry);
            structure.SetValue("BANK_CTRY_ISO", this.Bank_Ctry_Iso);
            structure.SetValue("TAX_NO_1", this.Tax_No_1);
            structure.SetValue("TAX_NO_2", this.Tax_No_2);

            if (useEcc)
            {
                structure.SetValue("TAX_NO_3", this.Tax_No_3);
                structure.SetValue("TAX_NO_4", this.Tax_No_4);
            }

            structure.SetValue("TAX", this.Tax);
            structure.SetValue("EQUAL_TAX", this.Equal_Tax);
            structure.SetValue("REGION", this.Region);
            structure.SetValue("CTRL_KEY", this.Ctrl_Key);
            structure.SetValue("INSTR_KEY", this.Instr_Key);
            structure.SetValue("DME_IND", this.Dme_Ind);
            structure.SetValue("LANGU_ISO", this.Langu_Iso);
            structure.SetValue("ANRED", this.Anred);

            return structure;
        }
    }

}