using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.Service.Interface;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Service.Implement;
using System.Configuration;
using SCG.eAccounting.SAP.Query;
using SCG.eAccounting.SAP.Service;
using SCG.eAccounting.SAP.BAPI.Service.SIMULATOR;
using SS.DB.Query;
using SS.Standard.Security;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.Query;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SCG.eAccounting.BLL;
using log4net;

namespace SCG.eAccounting.SAP.BAPI.Service.Posting
{
    public abstract class PostingService
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(PostingService));
        public abstract void CreatePostData(long DocID, string DocKind);

        #region protected string GetSAPConnectionString()
        protected string GetSAPConnectionString()
        {
            #region Old Code 2009-05-15
            //string strCLIENT    = ConfigurationSettings.AppSettings["SAPCLIENT"].ToString();
            //string strUSER      = ConfigurationSettings.AppSettings["SAPUSER"].ToString();
            //string strPASSWD    = ConfigurationSettings.AppSettings["SAPPASSWD"].ToString();
            //string strLANG      = ConfigurationSettings.AppSettings["SAPLANG"].ToString();
            //string strASHOST    = ConfigurationSettings.AppSettings["SAPASHOST"].ToString();
            //string strSYSNR     = ConfigurationSettings.AppSettings["SAPSYSNR"].ToString();

            //string strReturn = "";
            //strReturn += "CLIENT="  + ParameterServices.BAPI_SAPCLIENT  + " ";
            //strReturn += "USER="    + ParameterServices.BAPI_SAPUSER    + " ";
            //strReturn += "PASSWD="  + ParameterServices.BAPI_SAPPASSWD  + " ";
            //strReturn += "LANG="    + ParameterServices.BAPI_SAPLANG    + " ";
            //strReturn += "ASHOST="  + ParameterServices.BAPI_SAPASHOST  + " ";
            //strReturn += "SYSNR="   + ParameterServices.BAPI_SAPSYSNR   + " ";

            //return strReturn;
            #endregion Old Code 2009-05-15

            global::SAP.Connector.Destination strConnection = new global::SAP.Connector.Destination();
            strConnection.SystemNumber  = short.Parse(ParameterServices.BAPI_SAPSYSNR);
            strConnection.Username      = ParameterServices.BAPI_SAPUSER;
            strConnection.Password      = ParameterServices.BAPI_SAPPASSWD;
            strConnection.Language      = ParameterServices.BAPI_SAPLANG;
            strConnection.Client        = short.Parse(ParameterServices.BAPI_SAPCLIENT);
            strConnection.MsgServerHost = ParameterServices.BAPI_MsgServerHost;
            strConnection.LogonGroup    = ParameterServices.BAPI_LogonGroup;
            strConnection.SAPSystemName = ParameterServices.BAPI_SAPSystemName;
            //strConnection.AppServerHost = ParameterServices.BAPI_SAPASHOST;
            return strConnection.ConnectionString.ToString();
        }
        #endregion protected string GetSAPConnectionString()

        #region protected string IsOpenSimulator()
        protected bool IsOpenSimulator()
        {
            //return Convert.ToBoolean(ConfigurationSettings.AppSettings["OpenSimulator"].ToString());
            return Convert.ToBoolean(ParameterServices.BAPI_OpenSimulator);
        }
        #endregion protected string IsOpenSimulator()


        #region public void DeletePostingDataByDocId(long DocID, string DocKind)
        public void DeletePostingDataByDocId(long DocID, string DocKind)
        {
            BapiQueryProvider.Bapiache09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiactx09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiacpa09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiacap09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiacar09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.BapiacextcQuery.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiacgl09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiaccr09Query.DeleteByDocID(DocID, DocKind);
            BapiQueryProvider.Bapiret2Query.DeleteByDocID(DocID, DocKind);
        }
        #endregion public void DeletePostingDataByDocId(long DocID, string DocKind)

        #region protected void GetPostingDataByDocId
        protected void GetPostingDataByDocId(    
            long DocId , string DocSeq,string DocKind,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacextc> listBAPIACEXTC,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacpa09> listBAPIACPA09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacap09> listBAPIACAP09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacgl09> listBAPIACGL09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiactx09> listBAPIACTX09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiaccr09> listBAPIACCR09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacar09> listBAPIACAR09)
        {
            // Hearder
            listBAPIACEXTC = BapiQueryProvider.BapiacextcQuery.FindByDocID(DocId, DocSeq, DocKind); //Head2
            listBAPIACPA09 = BapiQueryProvider.Bapiacpa09Query.FindByDocID(DocId, DocSeq, DocKind); //Head for VAT
            // Detail
            listBAPIACAP09 = BapiQueryProvider.Bapiacap09Query.FindByDocID(DocId, DocSeq, DocKind); //Account
            listBAPIACGL09 = BapiQueryProvider.Bapiacgl09Query.FindByDocID(DocId, DocSeq, DocKind); //GL
            listBAPIACTX09 = BapiQueryProvider.Bapiactx09Query.FindByDocID(DocId, DocSeq, DocKind); //for VAT
            listBAPIACAR09 = BapiQueryProvider.Bapiacar09Query.FindByDocID(DocId, DocSeq, DocKind); //for ลูกหนี้
            // Foolter
            listBAPIACCR09 = BapiQueryProvider.Bapiaccr09Query.FindByDocID(DocId, DocSeq, DocKind); //for Detail
        }
        #endregion protected void GetPostDataByDocID

        #region protected void SetPostingDataToSAP
        protected void SetPostingDataToSAP(
            Bapiache09 listBAPIACHE09,
            IList<SCG.eAccounting.SAP.DTO.Bapiacpa09> listBAPIACPA09,
            IList<SCG.eAccounting.SAP.DTO.Bapiacextc> listBAPIACEXTC,
            IList<SCG.eAccounting.SAP.DTO.Bapiacap09> listBAPIACAP09,
            IList<SCG.eAccounting.SAP.DTO.Bapiacgl09> listBAPIACGL09,
            IList<SCG.eAccounting.SAP.DTO.Bapiactx09> listBAPIACTX09,
            IList<SCG.eAccounting.SAP.DTO.Bapiaccr09> listBAPIACCR09,
            IList<SCG.eAccounting.SAP.DTO.Bapiacar09> listBAPIACAR09,
            ref BAPIACHE09      ACHE09,
            ref BAPIACPA09      ACPA09,
            ref BAPIACEXTCTable ACEXTCTable,
            ref BAPIACAP09Table ACAP09Table,
            ref BAPIACGL09Table ACGL09Table,
            ref BAPIACTX09Table ACTX09Table,
            ref BAPIACCR09Table ACCR09Table,
            ref BAPIACAR09Table ACAR09Table )
        {
            #region BAPIACHE09
            ACHE09.Ac_Doc_No        = listBAPIACHE09.AcDocNo;
            ACHE09.Acc_Principle    = listBAPIACHE09.AccPrinciple;
            ACHE09.Bus_Act          = listBAPIACHE09.BusAct;
            ACHE09.Comp_Code        = listBAPIACHE09.CompCode;
            ACHE09.Compo_Acc        = listBAPIACHE09.CompoAcc;
            ACHE09.Doc_Date         = listBAPIACHE09.DocDate;
            ACHE09.Doc_Type         = listBAPIACHE09.DocType;
            ACHE09.Fis_Period       = listBAPIACHE09.FisPeriod;
            ACHE09.Fisc_Year        = listBAPIACHE09.FiscYear;
            ACHE09.Header_Txt       = listBAPIACHE09.HeaderTxt;
            ACHE09.Neg_Postng       = listBAPIACHE09.NegPostng;
            ACHE09.Obj_Key          = listBAPIACHE09.ObjKey;
            ACHE09.Obj_Key_Inv      = listBAPIACHE09.ObjKeyInv;
            ACHE09.Obj_Key_R        = listBAPIACHE09.ObjKeyR;
            ACHE09.Obj_Sys          = listBAPIACHE09.ObjSys;
            ACHE09.Obj_Type         = listBAPIACHE09.ObjType;
            ACHE09.Pstng_Date       = listBAPIACHE09.PstngDate;
            ACHE09.Reason_Rev       = listBAPIACHE09.ReasonRev;
            ACHE09.Ref_Doc_No       = listBAPIACHE09.RefDocNo;
            ACHE09.Ref_Doc_No_Long  = listBAPIACHE09.RefDocNoLong;
            ACHE09.Trans_Date       = listBAPIACHE09.TransDate;
            ACHE09.Username         = listBAPIACHE09.Username;
            #endregion BAPIACHE09

            #region BAPIACPA09
            if (listBAPIACPA09.Count >= 1)
            {
                ACPA09.Anred            = listBAPIACPA09[0].Anred;
                ACPA09.Bank_Acct        = listBAPIACPA09[0].BankAcct;
                ACPA09.Bank_Ctry        = listBAPIACPA09[0].BankCtry;
                ACPA09.Bank_Ctry_Iso    = listBAPIACPA09[0].BankCtryIso;
                ACPA09.Bank_No          = listBAPIACPA09[0].BankNo;
                ACPA09.City             = listBAPIACPA09[0].City;
                ACPA09.Country          = listBAPIACPA09[0].Country;
                ACPA09.Country_Iso      = listBAPIACPA09[0].CountryIso;
                ACPA09.Ctrl_Key         = listBAPIACPA09[0].CtrlKey;
                ACPA09.Dme_Ind          = listBAPIACPA09[0].DmeInd;
                ACPA09.Equal_Tax        = listBAPIACPA09[0].EqualTax;
                ACPA09.Instr_Key        = listBAPIACPA09[0].InstrKey;
                ACPA09.Langu_Iso        = listBAPIACPA09[0].LanguIso;
                ACPA09.Name             = listBAPIACPA09[0].Name;
                ACPA09.Name_2           = listBAPIACPA09[0].Name2;
                ACPA09.Name_3           = listBAPIACPA09[0].Name3;
                ACPA09.Name_4           = listBAPIACPA09[0].Name4;
                ACPA09.Po_Box           = listBAPIACPA09[0].PoBox;
                ACPA09.Pobk_Curac       = listBAPIACPA09[0].PobkCurac;
                ACPA09.Pobx_Pcd         = listBAPIACPA09[0].PobxPcd;
                ACPA09.Postl_Code       = listBAPIACPA09[0].PostlCode;
                ACPA09.Region           = listBAPIACPA09[0].Region;
                ACPA09.Street           = listBAPIACPA09[0].Street;
                ACPA09.Tax              = listBAPIACPA09[0].Tax;
                ACPA09.Tax_No_1         = listBAPIACPA09[0].TaxNo1;
                ACPA09.Tax_No_2         = listBAPIACPA09[0].TaxNo2;
            }
            #endregion BAPIACPA09

            #region BAPIACEXTC
            for (int i = 0; i < listBAPIACEXTC.Count; i++)
            {
                BAPIACEXTC tmp1 = new BAPIACEXTC();
                tmp1.Field1 = listBAPIACEXTC[i].Field1;

                if (tmp1.Field1 == "WTH2")
                    tmp1.Field1 = "WTH1";

                tmp1.Field2 = listBAPIACEXTC[i].Field2;
                tmp1.Field3 = listBAPIACEXTC[i].Field3;
                tmp1.Field4 = listBAPIACEXTC[i].Field4;
                ACEXTCTable.Add(tmp1);
            }
            #endregion BAPIACEXTC


            #region BAPIACAP09
            for (int i = 0; i < listBAPIACAP09.Count; i++)
            {
                BAPIACAP09 tmp      = new BAPIACAP09();
                tmp.Alloc_Nmbr      = listBAPIACAP09[i].AllocNmbr;
                tmp.Alt_Payee       = listBAPIACAP09[i].AltPayee;
                tmp.Alt_Payee_Bank  = listBAPIACAP09[i].AltPayeeBank;
                tmp.Bank_Id         = listBAPIACAP09[i].BankId;
                tmp.Bline_Date      = listBAPIACAP09[i].BlineDate;
                tmp.Bllsrv_Ind      = listBAPIACAP09[i].BllsrvInd;
                tmp.Branch          = listBAPIACAP09[i].Branch;
                tmp.Bus_Area        = listBAPIACAP09[i].BusArea;
                tmp.Businessplace   = listBAPIACAP09[i].Businessplace;
                tmp.Comp_Code       = listBAPIACAP09[i].CompCode;
                tmp.Dsct_Days1      = listBAPIACAP09[i].DsctDays1;
                tmp.Dsct_Days2      = listBAPIACAP09[i].DsctDays2;
                tmp.Dsct_Pct1       = listBAPIACAP09[i].DsctPct1;
                tmp.Dsct_Pct2       = listBAPIACAP09[i].DsctPct2;
                tmp.Gl_Account      = listBAPIACAP09[i].GlAccount;
                tmp.Instr1          = listBAPIACAP09[i].Instr1;
                tmp.Instr2          = listBAPIACAP09[i].Instr2;
                tmp.Instr3          = listBAPIACAP09[i].Instr3;
                tmp.Instr4          = listBAPIACAP09[i].Instr4;
                tmp.Item_Text       = listBAPIACAP09[i].ItemText;
                tmp.Itemno_Acc      = listBAPIACAP09[i].ItemnoAcc;
                tmp.Netterms        = listBAPIACAP09[i].Netterms;
                tmp.Partner_Bk      = listBAPIACAP09[i].PartnerBk;
                tmp.Partner_Guid    = listBAPIACAP09[i].PartnerGuid;
                tmp.Pmnt_Block      = listBAPIACAP09[i].PmntBlock;
                tmp.Pmnttrms        = listBAPIACAP09[i].Pmnttrms;
                tmp.Pmtmthsupl      = listBAPIACAP09[i].Pmtmthsupl;
                tmp.Po_Checkdg      = listBAPIACAP09[i].PoCheckdg;
                tmp.Po_Ref_No       = listBAPIACAP09[i].PoRefNo;
                tmp.Po_Sub_No       = listBAPIACAP09[i].PoSubNo;
                tmp.Pymt_Amt        = listBAPIACAP09[i].PymtAmt;
                tmp.Pymt_Cur        = listBAPIACAP09[i].PymtCur;
                tmp.Pymt_Cur_Iso    = listBAPIACAP09[i].PymtCurIso;
                tmp.Pymt_Meth       = listBAPIACAP09[i].PymtMeth;
                tmp.Ref_Key_1       = listBAPIACAP09[i].RefKey1;
                tmp.Ref_Key_2       = listBAPIACAP09[i].RefKey2;
                tmp.Ref_Key_3       = listBAPIACAP09[i].RefKey3;
                tmp.Scbank_Ind      = listBAPIACAP09[i].ScbankInd;
                tmp.Sectioncode     = listBAPIACAP09[i].Sectioncode;
                tmp.Sp_Gl_Ind       = listBAPIACAP09[i].SpGlInd;
                tmp.Supcountry      = listBAPIACAP09[i].Supcountry;
                tmp.Supcountry_Iso  = listBAPIACAP09[i].SupcountryIso;
                tmp.Tax_Code        = listBAPIACAP09[i].TaxCode;
                tmp.Tax_Date        = listBAPIACAP09[i].TaxDate;
                tmp.Taxjurcode      = listBAPIACAP09[i].Taxjurcode;
                tmp.Vendor_No       = listBAPIACAP09[i].VendorNo;
                tmp.W_Tax_Code      = listBAPIACAP09[i].WTaxCode;
                ACAP09Table.Add(tmp);
            }
            #endregion BAPIACAP09

            #region BAPIACGL09
            for (int i = 0; i < listBAPIACGL09.Count; i++)
            {
                BAPIACGL09 tmp = new BAPIACGL09();
                tmp.Ac_Doc_No = listBAPIACGL09[i].AcDocNo;
                tmp.Acct_Key = listBAPIACGL09[i].AcctKey;
                tmp.Acct_Type = listBAPIACGL09[i].AcctType;
                tmp.Activity = listBAPIACGL09[i].Activity;
                tmp.Acttype = listBAPIACGL09[i].Acttype;
                tmp.Alloc_Nmbr = listBAPIACGL09[i].AllocNmbr;
                tmp.Asset_No = listBAPIACGL09[i].AssetNo;
                tmp.Asval_Date = listBAPIACGL09[i].AsvalDate;
                tmp.Base_Uom = listBAPIACGL09[i].BaseUom;
                tmp.Base_Uom_Iso = listBAPIACGL09[i].BaseUomIso;
                tmp.Bill_Type = listBAPIACGL09[i].BillType;
                tmp.Bus_Area = listBAPIACGL09[i].BusArea;
                tmp.Bus_Scenario = listBAPIACGL09[i].BusScenario;
                tmp.Cmmt_Item = listBAPIACGL09[i].CmmtItem;
                tmp.Cmmt_Item_Long = listBAPIACGL09[i].CmmtItemLong;
                tmp.Co_Busproc = listBAPIACGL09[i].CoBusproc;
                tmp.Comp_Code = listBAPIACGL09[i].CompCode;
                tmp.Cond_Category = listBAPIACGL09[i].CondCategory;
                tmp.Cond_Count = listBAPIACGL09[i].CondCount;
                tmp.Cond_St_No = listBAPIACGL09[i].CondStNo;
                tmp.Cond_Type = listBAPIACGL09[i].CondType;
                tmp.Costcenter = listBAPIACGL09[i].Costcenter;
                tmp.Costobject = listBAPIACGL09[i].Costobject;
                tmp.Cs_Trans_T = listBAPIACGL09[i].CsTransT;
                tmp.Cshdis_Ind = listBAPIACGL09[i].CshdisInd;
                tmp.Customer = listBAPIACGL09[i].Customer;
                tmp.De_Cre_Ind = listBAPIACGL09[i].DeCreInd;
                tmp.Distr_Chan = listBAPIACGL09[i].DistrChan;
                tmp.Division = listBAPIACGL09[i].Division;
                tmp.Doc_Type = listBAPIACGL09[i].DocType;
                tmp.Entry_Qnt = listBAPIACGL09[i].EntryQnt;
                tmp.Entry_Uom = listBAPIACGL09[i].EntryUom;
                tmp.Entry_Uom_Iso = listBAPIACGL09[i].EntryUomIso;
                tmp.Ext_Object_Id = listBAPIACGL09[i].ExtObjectId;
                tmp.Fis_Period = listBAPIACGL09[i].FisPeriod;
                tmp.Fisc_Year = listBAPIACGL09[i].FiscYear;
                tmp.Fm_Area = listBAPIACGL09[i].FmArea;
                tmp.Func_Area = listBAPIACGL09[i].FuncArea;
                tmp.Func_Area_Long = listBAPIACGL09[i].FuncAreaLong;
                tmp.Fund = listBAPIACGL09[i].Fund;
                tmp.Funds_Ctr = listBAPIACGL09[i].FundsCtr;
                tmp.Gl_Account = listBAPIACGL09[i].GlAccount;
                tmp.Grant_Nbr = listBAPIACGL09[i].GrantNbr;
                tmp.Gross_Wt = listBAPIACGL09[i].GrossWt;
                tmp.Inv_Qty = listBAPIACGL09[i].InvQty;
                tmp.Inv_Qty_Su = listBAPIACGL09[i].InvQtySu;
                tmp.Item_Cat = listBAPIACGL09[i].ItemCat;
                tmp.Item_Text = listBAPIACGL09[i].ItemText;
                tmp.Itemno_Acc = listBAPIACGL09[i].ItemnoAcc;
                tmp.Itm_Number = listBAPIACGL09[i].ItmNumber;
                tmp.Log_Proc = listBAPIACGL09[i].LogProc;
                tmp.Material = listBAPIACGL09[i].Material;
                tmp.Matl_Type = listBAPIACGL09[i].MatlType;
                tmp.Mvt_Ind = listBAPIACGL09[i].MvtInd;
                tmp.Net_Weight = listBAPIACGL09[i].NetWeight;
                tmp.Network = listBAPIACGL09[i].Network;
                tmp.Order_Itno = listBAPIACGL09[i].OrderItno;
                tmp.Orderid = listBAPIACGL09[i].Orderid;
                tmp.Orig_Group = listBAPIACGL09[i].OrigGroup;
                tmp.Orig_Mat = listBAPIACGL09[i].OrigMat;
                tmp.P_El_Prctr = listBAPIACGL09[i].PElPrctr;
                tmp.Part_Acct = listBAPIACGL09[i].PartAcct;
                tmp.Part_Prctr = listBAPIACGL09[i].PartPrctr;
                tmp.Plant = listBAPIACGL09[i].Plant;
                tmp.Po_Item = listBAPIACGL09[i].PoItem;
                tmp.Po_Number = listBAPIACGL09[i].PoNumber;
                tmp.Po_Pr_Qnt = listBAPIACGL09[i].PoPrQnt;
                tmp.Po_Pr_Uom = listBAPIACGL09[i].PoPrUom;
                tmp.Po_Pr_Uom_Iso = listBAPIACGL09[i].PoPrUomIso;
                tmp.Profit_Ctr = listBAPIACGL09[i].ProfitCtr;
                tmp.Pstng_Date = listBAPIACGL09[i].PstngDate;
                tmp.Quantity = listBAPIACGL09[i].Quantity;
                tmp.Ref_Key_1 = listBAPIACGL09[i].RefKey1;
                tmp.Ref_Key_2 = listBAPIACGL09[i].RefKey2;
                tmp.Ref_Key_3 = listBAPIACGL09[i].RefKey3;
                tmp.Reval_Ind = listBAPIACGL09[i].RevalInd;
                tmp.Routing_No = listBAPIACGL09[i].RoutingNo;
                tmp.S_Ord_Item = listBAPIACGL09[i].SOrdItem;
                tmp.Sales_Grp = listBAPIACGL09[i].SalesGrp;
                tmp.Sales_Off = listBAPIACGL09[i].SalesOff;
                tmp.Sales_Ord = listBAPIACGL09[i].SalesOrd;
                tmp.Sales_Unit = listBAPIACGL09[i].SalesUnit;
                tmp.Sales_Unit_Iso = listBAPIACGL09[i].SalesUnitIso;
                tmp.Salesorg = listBAPIACGL09[i].Salesorg;
                tmp.Serial_No = listBAPIACGL09[i].SerialNo;
                tmp.Sold_To = listBAPIACGL09[i].SoldTo;
                tmp.Stat_Con = listBAPIACGL09[i].StatCon;
                tmp.Sub_Number = listBAPIACGL09[i].SubNumber;
                tmp.Tax_Code = listBAPIACGL09[i].TaxCode;
                tmp.Taxjurcode = listBAPIACGL09[i].Taxjurcode;
                tmp.Tr_Part_Ba = listBAPIACGL09[i].TrPartBa;
                tmp.Trade_Id = listBAPIACGL09[i].TradeId;
                tmp.Unit_Of_Wt = listBAPIACGL09[i].UnitOfWt;
                tmp.Unit_Of_Wt_Iso = listBAPIACGL09[i].UnitOfWtIso;
                tmp.Val_Area = listBAPIACGL09[i].ValArea;
                tmp.Val_Type = listBAPIACGL09[i].ValType;
                tmp.Value_Date = listBAPIACGL09[i].ValueDate;
                tmp.Vendor_No = listBAPIACGL09[i].VendorNo;
                tmp.Volume = listBAPIACGL09[i].Volume;
                tmp.Volumeunit = listBAPIACGL09[i].Volumeunit;
                tmp.Volumeunit_Iso = listBAPIACGL09[i].VolumeunitIso;
                tmp.Wbs_Element = listBAPIACGL09[i].WbsElement;
                tmp.Xmfrw = listBAPIACGL09[i].Xmfrw;

                ACGL09Table.Add(tmp);
            }
            #endregion BAPIACGL09

            #region BAPIACTX09
            for (int i = 0; i < listBAPIACTX09.Count; i++)
            {
                BAPIACTX09 tmp = new BAPIACTX09();
                tmp.Acct_Key = listBAPIACTX09[i].AcctKey;
                tmp.Cond_Key = listBAPIACTX09[i].CondKey;
                tmp.Direct_Tax = listBAPIACTX09[i].DirectTax;
                tmp.Gl_Account = listBAPIACTX09[i].GlAccount;
                tmp.Itemno_Acc = listBAPIACTX09[i].ItemnoAcc;
                tmp.Itemno_Tax = listBAPIACTX09[i].ItemnoTax;
                tmp.Tax_Code = listBAPIACTX09[i].TaxCode;
                tmp.Tax_Date = listBAPIACTX09[i].TaxDate;
                tmp.Tax_Rate = listBAPIACTX09[i].TaxRate;
                tmp.Taxjurcode = listBAPIACTX09[i].Taxjurcode;
                tmp.Taxjurcode_Deep = listBAPIACTX09[i].TaxjurcodeDeep;
                tmp.Taxjurcode_Level = listBAPIACTX09[i].TaxjurcodeLevel;
                ACTX09Table.Add(tmp);
            }
            #endregion BAPIACTX09

            #region BAPIACAR09
            for (int i = 0; i < listBAPIACAR09.Count; i++)
            {
                BAPIACAR09 tmp = new BAPIACAR09();
                tmp.Alloc_Nmbr = listBAPIACAR09[i].AllocNmbr;
                tmp.Alt_Payee = listBAPIACAR09[i].AltPayee;
                tmp.Alt_Payee_Bank = listBAPIACAR09[i].AltPayeeBank;
                tmp.Bank_Id = listBAPIACAR09[i].BankId;
                tmp.Bline_Date = listBAPIACAR09[i].BlineDate;
                tmp.Branch = listBAPIACAR09[i].Branch;
                tmp.Bus_Area = listBAPIACAR09[i].BusArea;
                tmp.Businessplace = listBAPIACAR09[i].Businessplace;
                tmp.C_Ctr_Area = listBAPIACAR09[i].CCtrArea;
                tmp.Comp_Code = listBAPIACAR09[i].CompCode;
                tmp.Customer = listBAPIACAR09[i].Customer;
                tmp.Dsct_Days1 = listBAPIACAR09[i].DsctDays1;
                tmp.Dsct_Days2 = listBAPIACAR09[i].DsctDays2;
                tmp.Dsct_Pct1 = listBAPIACAR09[i].DsctPct1;
                tmp.Dsct_Pct2 = listBAPIACAR09[i].DsctPct2;
                tmp.Dunn_Area = listBAPIACAR09[i].DunnArea;
                tmp.Dunn_Block = listBAPIACAR09[i].DunnBlock;
                tmp.Dunn_Key = listBAPIACAR09[i].DunnKey;
                tmp.Gl_Account = listBAPIACAR09[i].GlAccount;
                tmp.Item_Text = listBAPIACAR09[i].ItemText;
                tmp.Itemno_Acc = listBAPIACAR09[i].ItemnoAcc;
                tmp.Netterms = listBAPIACAR09[i].Netterms;
                tmp.Partner_Bk = listBAPIACAR09[i].PartnerBk;
                tmp.Partner_Guid = listBAPIACAR09[i].PartnerGuid;
                tmp.Paymt_Ref = listBAPIACAR09[i].PaymtRef;
                tmp.Pmnt_Block = listBAPIACAR09[i].PmntBlock;
                tmp.Pmnttrms = listBAPIACAR09[i].Pmnttrms;
                tmp.Pmtmthsupl = listBAPIACAR09[i].Pmtmthsupl;
                tmp.Pymt_Amt = listBAPIACAR09[i].PymtAmt;
                tmp.Pymt_Cur = listBAPIACAR09[i].PymtCur;
                tmp.Pymt_Cur_Iso = listBAPIACAR09[i].PymtCurIso;
                tmp.Pymt_Meth = listBAPIACAR09[i].PymtMeth;
                tmp.Ref_Key_1 = listBAPIACAR09[i].RefKey1;
                tmp.Ref_Key_2 = listBAPIACAR09[i].RefKey2;
                tmp.Ref_Key_3 = listBAPIACAR09[i].RefKey3;
                tmp.Scbank_Ind = listBAPIACAR09[i].ScbankInd;
                tmp.Sectioncode = listBAPIACAR09[i].Sectioncode;
                tmp.Sp_Gl_Ind = listBAPIACAR09[i].SpGlInd;
                tmp.Supcountry = listBAPIACAR09[i].Supcountry;
                tmp.Supcountry_Iso = listBAPIACAR09[i].SupcountryIso;
                tmp.Tax_Code = listBAPIACAR09[i].TaxCode;
                tmp.Tax_Date = listBAPIACAR09[i].TaxDate;
                tmp.Taxjurcode = listBAPIACAR09[i].Taxjurcode;
                tmp.Vat_Reg_No = listBAPIACAR09[i].VatRegNo;
                ACAR09Table.Add(tmp);
            }
            #endregion BAPIACAR09

            #region BAPIACCR09
            for (int i = 0; i < listBAPIACCR09.Count; i++)
            {
                BAPIACCR09 tmp = new BAPIACCR09();
                tmp.Amt_Base    = listBAPIACCR09[i].AmtBase;
                tmp.Amt_Doccur  = listBAPIACCR09[i].AmtDoccur;
                tmp.Curr_Type   = listBAPIACCR09[i].CurrType;
                tmp.Currency    = listBAPIACCR09[i].Currency;
                tmp.Currency_Iso= listBAPIACCR09[i].CurrencyIso;
                tmp.Disc_Amt    = listBAPIACCR09[i].DiscAmt;
                tmp.Disc_Base   = listBAPIACCR09[i].DiscBase;
                tmp.Exch_Rate   = listBAPIACCR09[i].ExchRate;
                tmp.Exch_Rate_V = listBAPIACCR09[i].ExchRateV;
                tmp.Itemno_Acc  = listBAPIACCR09[i].ItemnoAcc;
                tmp.Tax_Amt     = listBAPIACCR09[i].TaxAmt;
                ACCR09Table.Add(tmp);
            }
            #endregion BAPIACCR09
        }
        #endregion protected void SetPostingDataToSAP

        #region protected void SaveReturnPostingDataToDatabase
        protected void SaveReturnPostingDataToDatabase(
            long DocId,string DocSeq,string DocKind,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacpa09> listBAPIACPA09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacextc> listBAPIACEXTC,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacap09> listBAPIACAP09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacgl09> listBAPIACGL09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiactx09> listBAPIACTX09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiaccr09> listBAPIACCR09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiacar09> listBAPIACAR09,
            ref IList<SCG.eAccounting.SAP.DTO.Bapiret2> listBAPIRET2,
            BAPIACPA09 ACPA09,
            BAPIACEXTCTable ACEXTCTable,
            BAPIACAP09Table ACAP09Table,
            BAPIACGL09Table ACGL09Table,
            BAPIACTX09Table ACTX09Table,
            BAPIACCR09Table ACCR09Table,
            BAPIACAR09Table ACAR09Table,
            BAPIRET2Table   RET2Table           )
        {
            #region Before Delete Data
            //BapiQueryProvider.BapiacextcQuery.DeleteByDocID(DocId,DocKind);
            //BapiQueryProvider.Bapiacpa09Query.DeleteByDocID(DocId,DocKind);
            //BapiQueryProvider.Bapiacap09Query.DeleteByDocID(DocId,DocKind);
            //BapiQueryProvider.Bapiacgl09Query.DeleteByDocID(DocId,DocKind);
            //BapiQueryProvider.Bapiactx09Query.DeleteByDocID(DocId,DocKind);
            //BapiQueryProvider.Bapiaccr09Query.DeleteByDocID(DocId,DocKind);
            //BapiQueryProvider.Bapiret2Query.DeleteByDocID(DocId,DocKind);
            #endregion Before Delete Data

            #region RET2Table
            try
            {
                for (int i = 0; i < RET2Table.Count; i++)
                {
                    Bapiret2 tmp = new Bapiret2();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.Field = SAPUIHelper.SubString(4, RET2Table[i].Field);
                    tmp.Id = SAPUIHelper.SubString(20, RET2Table[i].Id);
                    tmp.LogMsgNo = SAPUIHelper.SubString(6, RET2Table[i].Log_Msg_No);
                    tmp.LogNo = SAPUIHelper.SubString(20, RET2Table[i].Log_No);
                    tmp.Message = SAPUIHelper.SubString(220, RET2Table[i].Message);
                    tmp.MessageV1 = SAPUIHelper.SubString(50, RET2Table[i].Message_V1);
                    tmp.MessageV2 = SAPUIHelper.SubString(50, RET2Table[i].Message_V2);
                    tmp.MessageV3 = SAPUIHelper.SubString(50, RET2Table[i].Message_V3);
                    tmp.MessageV4 = SAPUIHelper.SubString(50, RET2Table[i].Message_V4);
                    tmp.Number = SAPUIHelper.SubString(3, RET2Table[i].Number);
                    tmp.Parameter = SAPUIHelper.SubString(32, RET2Table[i].Parameter);
                    tmp.System = SAPUIHelper.SubString(10, RET2Table[i].System);
                    tmp.Type = SAPUIHelper.SubString(1, RET2Table[i].Type);

                    tmp.Active = true;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIRET2.Add(tmp);
                    BapiServiceProvider.Bapiret2Service.Save(tmp);
                }
            }
            catch(Exception ex)
            {
                if (logger != null)
                {
                    logger.Error("PostingService.SaveReturnPostingDataToDatabase() (ex.StackTrace) : " + ex.StackTrace);
                    logger.Error("PostingService.SaveReturnPostingDataToDatabase() (ex.Message) : " + ex.Message);
                }
            }
            #endregion RET2Table

            #region Don't Insert
            bool isInsert = false;
            if (isInsert)
            {
                #region ACPA09
                if (ACPA09.Name != "")
                {
                    Bapiacpa09 tmp = new Bapiacpa09();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.Anred = ACPA09.Anred;
                    tmp.BankAcct = ACPA09.Bank_Acct;
                    tmp.BankCtry = ACPA09.Bank_Ctry;
                    tmp.BankCtryIso = ACPA09.Bank_Ctry_Iso;
                    tmp.BankNo = ACPA09.Bank_No;
                    tmp.City = ACPA09.City;
                    tmp.Country = ACPA09.Country;
                    tmp.CountryIso = ACPA09.Country_Iso;
                    tmp.CtrlKey = ACPA09.Ctrl_Key;
                    tmp.DmeInd = ACPA09.Dme_Ind;
                    tmp.EqualTax = ACPA09.Equal_Tax;
                    tmp.InstrKey = ACPA09.Instr_Key;
                    tmp.LanguIso = ACPA09.Langu_Iso;
                    tmp.Name = ACPA09.Name;
                    tmp.Name2 = ACPA09.Name_2;
                    tmp.Name3 = ACPA09.Name_3;
                    tmp.Name4 = ACPA09.Name_4;
                    tmp.PobkCurac = ACPA09.Pobk_Curac;
                    tmp.PoBox = ACPA09.Po_Box;
                    tmp.PobxPcd = ACPA09.Pobx_Pcd;
                    tmp.PostlCode = ACPA09.Postl_Code;
                    tmp.Region = ACPA09.Region;
                    tmp.Street = ACPA09.Street;
                    tmp.Tax = ACPA09.Tax;
                    tmp.TaxNo1 = ACPA09.Tax_No_1;
                    tmp.TaxNo2 = ACPA09.Tax_No_2;

                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACPA09.Add(tmp);
                    BapiServiceProvider.Bapiacpa09Service.Save(tmp);
                }
                #endregion ACPA09

                #region ACEXTCTable
                for (int i = 0; i < ACEXTCTable.Count; i++)
                {
                    Bapiacextc tmp = new Bapiacextc();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.Field1 = ACEXTCTable[i].Field1;
                    tmp.Field2 = ACEXTCTable[i].Field2;
                    tmp.Field3 = ACEXTCTable[i].Field3;
                    tmp.Field4 = ACEXTCTable[i].Field4;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACEXTC.Add(tmp);
                    BapiServiceProvider.BapiacextcService.Save(tmp);
                }
                #endregion ACEXTCTable

                #region ACGL09Table
                for (int i = 0; i < ACGL09Table.Count; i++)
                {
                    Bapiacgl09 tmp = new Bapiacgl09();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.AcctKey = ACGL09Table[i].Acct_Key;
                    tmp.AcctType = ACGL09Table[i].Acct_Type;
                    tmp.AcDocNo = ACGL09Table[i].Ac_Doc_No;
                    tmp.Activity = ACGL09Table[i].Activity;
                    tmp.Acttype = ACGL09Table[i].Acttype;
                    tmp.AllocNmbr = ACGL09Table[i].Alloc_Nmbr;
                    tmp.AssetNo = ACGL09Table[i].Asset_No;
                    tmp.AsvalDate = ACGL09Table[i].Asval_Date;
                    tmp.BaseUom = ACGL09Table[i].Base_Uom;
                    tmp.BaseUomIso = ACGL09Table[i].Base_Uom_Iso;
                    tmp.BillType = ACGL09Table[i].Bill_Type;
                    tmp.BusArea = ACGL09Table[i].Bus_Area;
                    tmp.BusScenario = ACGL09Table[i].Bus_Scenario;
                    tmp.CmmtItem = ACGL09Table[i].Cmmt_Item;
                    tmp.CmmtItemLong = ACGL09Table[i].Cmmt_Item_Long;
                    tmp.CoBusproc = ACGL09Table[i].Co_Busproc;
                    tmp.CompCode = ACGL09Table[i].Comp_Code;
                    tmp.CondCategory = ACGL09Table[i].Cond_Category;
                    tmp.CondCount = ACGL09Table[i].Cond_Count;
                    tmp.CondStNo = ACGL09Table[i].Cond_St_No;
                    tmp.CondType = ACGL09Table[i].Cond_Type;
                    tmp.Costcenter = ACGL09Table[i].Costcenter;
                    tmp.Costobject = ACGL09Table[i].Costobject;
                    tmp.CshdisInd = ACGL09Table[i].Cshdis_Ind;
                    tmp.CsTransT = ACGL09Table[i].Cs_Trans_T;
                    tmp.Customer = ACGL09Table[i].Customer;
                    tmp.DeCreInd = ACGL09Table[i].De_Cre_Ind;
                    tmp.DistrChan = ACGL09Table[i].Distr_Chan;
                    tmp.Division = ACGL09Table[i].Division;
                    tmp.EntryQnt = ACGL09Table[i].Entry_Qnt;
                    tmp.EntryUom = ACGL09Table[i].Entry_Uom;
                    tmp.EntryUomIso = ACGL09Table[i].Entry_Uom_Iso;
                    tmp.ExtObjectId = ACGL09Table[i].Ext_Object_Id;
                    tmp.FiscYear = ACGL09Table[i].Fisc_Year;
                    tmp.FisPeriod = ACGL09Table[i].Fis_Period;
                    tmp.FmArea = ACGL09Table[i].Fm_Area;
                    tmp.FuncArea = ACGL09Table[i].Func_Area;
                    tmp.FuncAreaLong = ACGL09Table[i].Func_Area_Long;
                    tmp.Fund = ACGL09Table[i].Fund;
                    tmp.FundsCtr = ACGL09Table[i].Funds_Ctr;
                    tmp.GlAccount = ACGL09Table[i].Gl_Account;
                    tmp.GrantNbr = ACGL09Table[i].Grant_Nbr;
                    tmp.GrossWt = ACGL09Table[i].Gross_Wt;
                    tmp.InvQty = ACGL09Table[i].Inv_Qty;
                    tmp.InvQtySu = ACGL09Table[i].Inv_Qty_Su;
                    tmp.ItemCat = ACGL09Table[i].Item_Cat;
                    tmp.ItemnoAcc = ACGL09Table[i].Item_Text;
                    tmp.ItemText = ACGL09Table[i].Itemno_Acc;
                    tmp.ItmNumber = ACGL09Table[i].Itm_Number;
                    tmp.LogProc = ACGL09Table[i].Log_Proc;
                    tmp.Material = ACGL09Table[i].Material;
                    tmp.MatlType = ACGL09Table[i].Matl_Type;
                    tmp.MvtInd = ACGL09Table[i].Mvt_Ind;
                    tmp.NetWeight = ACGL09Table[i].Net_Weight;
                    tmp.Network = ACGL09Table[i].Network;
                    tmp.Orderid = ACGL09Table[i].Orderid;
                    tmp.OrderItno = ACGL09Table[i].Order_Itno;
                    tmp.OrigGroup = ACGL09Table[i].Orig_Group;
                    tmp.OrigMat = ACGL09Table[i].Orig_Mat;
                    tmp.PartAcct = ACGL09Table[i].Part_Acct;
                    tmp.PartPrctr = ACGL09Table[i].Part_Prctr;
                    tmp.PElPrctr = ACGL09Table[i].P_El_Prctr;
                    tmp.Plant = ACGL09Table[i].Plant;
                    tmp.PoItem = ACGL09Table[i].Po_Item;
                    tmp.PoNumber = ACGL09Table[i].Po_Number;
                    tmp.PoPrQnt = ACGL09Table[i].Po_Pr_Qnt;
                    tmp.PoPrUom = ACGL09Table[i].Po_Pr_Uom;
                    tmp.PoPrUomIso = ACGL09Table[i].Po_Pr_Uom_Iso;
                    tmp.ProfitCtr = ACGL09Table[i].Profit_Ctr;
                    tmp.PstngDate = ACGL09Table[i].Pstng_Date;
                    tmp.Quantity = ACGL09Table[i].Quantity;
                    tmp.RefKey1 = ACGL09Table[i].Ref_Key_1;
                    tmp.RefKey2 = ACGL09Table[i].Ref_Key_2;
                    tmp.RefKey3 = ACGL09Table[i].Ref_Key_3;
                    tmp.RevalInd = ACGL09Table[i].Reval_Ind;
                    tmp.RoutingNo = ACGL09Table[i].Routing_No;
                    tmp.SalesGrp = ACGL09Table[i].S_Ord_Item;
                    tmp.SalesOff = ACGL09Table[i].Sales_Off;
                    tmp.SalesOrd = ACGL09Table[i].Sales_Ord;
                    tmp.Salesorg = ACGL09Table[i].Salesorg;
                    tmp.SalesUnit = ACGL09Table[i].Sales_Unit;
                    tmp.SalesUnitIso = ACGL09Table[i].Sales_Unit_Iso;
                    tmp.SerialNo = ACGL09Table[i].Serial_No;
                    tmp.SoldTo = ACGL09Table[i].Sold_To;
                    tmp.SOrdItem = ACGL09Table[i].S_Ord_Item;
                    tmp.StatCon = ACGL09Table[i].Stat_Con;
                    tmp.SubNumber = ACGL09Table[i].Sub_Number;
                    tmp.TaxCode = ACGL09Table[i].Tax_Code;
                    tmp.Taxjurcode = ACGL09Table[i].Taxjurcode;
                    tmp.TradeId = ACGL09Table[i].Trade_Id;
                    tmp.TrPartBa = ACGL09Table[i].Tr_Part_Ba;
                    tmp.UnitOfWt = ACGL09Table[i].Unit_Of_Wt;
                    tmp.UnitOfWtIso = ACGL09Table[i].Unit_Of_Wt_Iso;
                    tmp.ValArea = ACGL09Table[i].Val_Area;
                    tmp.ValType = ACGL09Table[i].Val_Type;
                    tmp.ValueDate = ACGL09Table[i].Value_Date;
                    tmp.VendorNo = ACGL09Table[i].Vendor_No;
                    tmp.Volume = ACGL09Table[i].Volume;
                    tmp.Volumeunit = ACGL09Table[i].Volumeunit;
                    tmp.VolumeunitIso = ACGL09Table[i].Volumeunit_Iso;
                    tmp.WbsElement = ACGL09Table[i].Wbs_Element;
                    tmp.Xmfrw = ACGL09Table[i].Xmfrw;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACGL09.Add(tmp);
                    BapiServiceProvider.Bapiacgl09Service.Save(tmp);
                }
                #endregion ACGL09Table

                #region ACAP09Table
                for (int i = 0; i < ACAP09Table.Count; i++)
                {
                    Bapiacap09 tmp = new Bapiacap09();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.AllocNmbr = ACAP09Table[i].Alloc_Nmbr;
                    tmp.AltPayee = ACAP09Table[i].Alt_Payee;
                    tmp.AltPayeeBank = ACAP09Table[i].Alt_Payee_Bank;
                    tmp.BankId = ACAP09Table[i].Bank_Id;
                    tmp.BlineDate = ACAP09Table[i].Bline_Date;
                    tmp.BllsrvInd = ACAP09Table[i].Bllsrv_Ind;
                    tmp.Branch = ACAP09Table[i].Branch;
                    tmp.BusArea = ACAP09Table[i].Bus_Area;
                    tmp.Businessplace = ACAP09Table[i].Businessplace;
                    tmp.CompCode = ACAP09Table[i].Comp_Code;
                    tmp.DsctDays1 = ACAP09Table[i].Dsct_Days1;
                    tmp.DsctDays2 = ACAP09Table[i].Dsct_Days2;
                    tmp.DsctPct1 = ACAP09Table[i].Dsct_Pct1;
                    tmp.DsctPct2 = ACAP09Table[i].Dsct_Pct2;
                    tmp.GlAccount = ACAP09Table[i].Gl_Account;
                    tmp.Instr1 = ACAP09Table[i].Instr1;
                    tmp.Instr2 = ACAP09Table[i].Instr2;
                    tmp.Instr3 = ACAP09Table[i].Instr3;
                    tmp.Instr4 = ACAP09Table[i].Instr4;
                    tmp.ItemnoAcc = ACAP09Table[i].Itemno_Acc;
                    tmp.ItemText = ACAP09Table[i].Item_Text;
                    tmp.Netterms = ACAP09Table[i].Netterms;
                    tmp.PartnerBk = ACAP09Table[i].Partner_Bk;
                    tmp.PartnerGuid = ACAP09Table[i].Partner_Guid;
                    tmp.PmntBlock = ACAP09Table[i].Pmnt_Block;
                    tmp.Pmnttrms = ACAP09Table[i].Pmnttrms;
                    tmp.Pmtmthsupl = ACAP09Table[i].Pmtmthsupl;
                    tmp.PoCheckdg = ACAP09Table[i].Po_Checkdg;
                    tmp.PoRefNo = ACAP09Table[i].Po_Ref_No;
                    tmp.PoSubNo = ACAP09Table[i].Po_Sub_No;
                    tmp.PymtAmt = ACAP09Table[i].Pymt_Amt;
                    tmp.PymtCur = ACAP09Table[i].Pymt_Cur;
                    tmp.PymtCurIso = ACAP09Table[i].Pymt_Cur_Iso;
                    tmp.PymtMeth = ACAP09Table[i].Pymt_Meth;
                    tmp.RefKey1 = ACAP09Table[i].Ref_Key_1;
                    tmp.RefKey2 = ACAP09Table[i].Ref_Key_2;
                    tmp.RefKey3 = ACAP09Table[i].Ref_Key_3;
                    tmp.ScbankInd = ACAP09Table[i].Scbank_Ind;
                    tmp.Sectioncode = ACAP09Table[i].Sectioncode;
                    tmp.SpGlInd = ACAP09Table[i].Sp_Gl_Ind;
                    tmp.Supcountry = ACAP09Table[i].Supcountry;
                    tmp.SupcountryIso = ACAP09Table[i].Supcountry_Iso;
                    tmp.TaxCode = ACAP09Table[i].Tax_Code;
                    tmp.TaxDate = ACAP09Table[i].Tax_Date;
                    tmp.Taxjurcode = ACAP09Table[i].Taxjurcode;
                    tmp.VendorNo = ACAP09Table[i].Vendor_No;
                    tmp.WTaxCode = ACAP09Table[i].W_Tax_Code;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACAP09.Add(tmp);
                    BapiServiceProvider.Bapiacap09Service.Save(tmp);
                }
                #endregion ACAP09Table

                #region ACTX09Table
                for (int i = 0; i < ACTX09Table.Count; i++)
                {
                    Bapiactx09 tmp = new Bapiactx09();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.AcctKey = ACTX09Table[i].Acct_Key;
                    tmp.CondKey = ACTX09Table[i].Cond_Key;
                    tmp.DirectTax = ACTX09Table[i].Direct_Tax;
                    tmp.GlAccount = ACTX09Table[i].Gl_Account;
                    tmp.ItemnoAcc = ACTX09Table[i].Itemno_Acc;
                    tmp.ItemnoTax = ACTX09Table[i].Itemno_Tax;
                    tmp.TaxCode = ACTX09Table[i].Tax_Code;
                    tmp.TaxDate = ACTX09Table[i].Tax_Date;
                    tmp.Taxjurcode = ACTX09Table[i].Taxjurcode;
                    tmp.TaxjurcodeDeep = ACTX09Table[i].Taxjurcode_Deep;
                    tmp.TaxjurcodeLevel = ACTX09Table[i].Taxjurcode_Level;
                    tmp.TaxRate = ACTX09Table[i].Tax_Rate;

                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACTX09.Add(tmp);
                    BapiServiceProvider.Bapiactx09Service.Save(tmp);
                }
                #endregion ACTX09Table


                #region ACCR09Table
                for (int i = 0; i < ACCR09Table.Count; i++)
                {
                    Bapiaccr09 tmp = new Bapiaccr09();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.AmtBase = ACCR09Table[i].Amt_Base;
                    tmp.AmtDoccur = ACCR09Table[i].Amt_Doccur;
                    tmp.Currency = ACCR09Table[i].Currency;
                    tmp.CurrencyIso = ACCR09Table[i].Currency_Iso;
                    tmp.CurrType = ACCR09Table[i].Curr_Type;
                    tmp.DiscAmt = ACCR09Table[i].Disc_Amt;
                    tmp.DiscBase = ACCR09Table[i].Disc_Base;
                    tmp.ExchRate = ACCR09Table[i].Exch_Rate;
                    tmp.ExchRateV = ACCR09Table[i].Exch_Rate_V;
                    tmp.ItemnoAcc = ACCR09Table[i].Itemno_Acc;
                    tmp.TaxAmt = ACCR09Table[i].Tax_Amt;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACCR09.Add(tmp);
                    BapiServiceProvider.Bapiaccr09Service.Save(tmp);
                }
                #endregion ACCR09Table

                #region ACAR09Table
                for (int i = 0; i < ACAR09Table.Count; i++)
                {
                    Bapiacar09 tmp = new Bapiacar09();
                    tmp.DocId = DocId;
                    tmp.DocSeq = DocSeq;
                    tmp.DocKind = DocKind;
                    tmp.AllocNmbr = ACAR09Table[i].Alloc_Nmbr;
                    tmp.AltPayee = ACAR09Table[i].Alt_Payee;
                    tmp.AltPayeeBank = ACAR09Table[i].Alt_Payee_Bank;
                    tmp.BankId = ACAR09Table[i].Bank_Id;
                    tmp.BlineDate = ACAR09Table[i].Bline_Date;
                    tmp.Branch = ACAR09Table[i].Branch;
                    tmp.BusArea = ACAR09Table[i].Bus_Area;
                    tmp.Businessplace = ACAR09Table[i].Businessplace;
                    tmp.CCtrArea = ACAR09Table[i].C_Ctr_Area;
                    tmp.CompCode = ACAR09Table[i].Comp_Code;
                    tmp.Customer = ACAR09Table[i].Customer;
                    tmp.DsctDays1 = ACAR09Table[i].Dsct_Days1;
                    tmp.DsctDays2 = ACAR09Table[i].Dsct_Days2;
                    tmp.DsctPct1 = ACAR09Table[i].Dsct_Pct1;
                    tmp.DsctPct2 = ACAR09Table[i].Dsct_Pct2;
                    tmp.DunnArea = ACAR09Table[i].Dunn_Area;
                    tmp.DunnBlock = ACAR09Table[i].Dunn_Block;
                    tmp.DunnKey = ACAR09Table[i].Dunn_Key;
                    tmp.GlAccount = ACAR09Table[i].Gl_Account;
                    tmp.ItemnoAcc = ACAR09Table[i].Itemno_Acc;
                    tmp.ItemText = ACAR09Table[i].Item_Text;
                    tmp.Netterms = ACAR09Table[i].Netterms;
                    tmp.PartnerBk = ACAR09Table[i].Partner_Bk;
                    tmp.PartnerGuid = ACAR09Table[i].Partner_Guid;
                    tmp.PaymtRef = ACAR09Table[i].Paymt_Ref;
                    tmp.PmntBlock = ACAR09Table[i].Pmnt_Block;
                    tmp.Pmnttrms = ACAR09Table[i].Pmnttrms;
                    tmp.Pmtmthsupl = ACAR09Table[i].Pmtmthsupl;
                    tmp.PymtAmt = ACAR09Table[i].Pymt_Amt;
                    tmp.PymtCur = ACAR09Table[i].Pymt_Cur;
                    tmp.PymtCurIso = ACAR09Table[i].Pymt_Cur_Iso;
                    tmp.PymtMeth = ACAR09Table[i].Pymt_Meth;
                    tmp.RefKey1 = ACAR09Table[i].Ref_Key_1;
                    tmp.RefKey2 = ACAR09Table[i].Ref_Key_2;
                    tmp.RefKey3 = ACAR09Table[i].Ref_Key_3;
                    tmp.ScbankInd = ACAR09Table[i].Scbank_Ind;
                    tmp.Sectioncode = ACAR09Table[i].Sectioncode;
                    tmp.SpGlInd = ACAR09Table[i].Sp_Gl_Ind;
                    tmp.Supcountry = ACAR09Table[i].Supcountry;
                    tmp.SupcountryIso = ACAR09Table[i].Supcountry_Iso;
                    tmp.TaxCode = ACAR09Table[i].Tax_Code;
                    tmp.TaxDate = ACAR09Table[i].Tax_Date;
                    tmp.Taxjurcode = ACAR09Table[i].Taxjurcode;
                    tmp.VatRegNo = ACAR09Table[i].Vat_Reg_No;

                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    listBAPIACAR09.Add(tmp);
                    BapiServiceProvider.Bapiacar09Service.Save(tmp);
                }
                #endregion ACCR09Table
            }
            #endregion Don't Insert
        }
        #endregion protected void SaveReturnPostingDataToDatabase

        #region protected string GetDocumentStatus(long DocId, string DocSeq , string DocKind)
        protected string GetDocumentStatus(long DocId, string DocSeq , string DocKind)
        {
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId,DocSeq,DocKind);
            if (listBAPIACHE09.Count >= 1)
                return listBAPIACHE09[0].DocStatus;
            else
                return "NULL";
        }
        #endregion protected string GetDocumentStatus(long DocId, string DocSeq , string DocKind)

        #region public string GetDocumentStatus(long DocId,string DocKind)
        public string GetDocumentStatus(long DocId,string DocKind)
        {
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

            int intN = 0;
            int intS = 0;
            int intP = 0;
            int intA = 0;

            for (int i = 0; i < listBAPIACHE09.Count; i++)
            {
                if (listBAPIACHE09[i].DocStatus == "N")
                    intN++;
                else if (listBAPIACHE09[i].DocStatus == "S")
                    intS++;
                else if (listBAPIACHE09[i].DocStatus == "P")
                    intP++;
                else if (listBAPIACHE09[i].DocStatus == "A")
                    intA++;
            }

            if (listBAPIACHE09.Count <= 0)
                return null;
            else if (intN == listBAPIACHE09.Count)
                return "N";
            else if (intS == listBAPIACHE09.Count)
                return "S";
            else if (intP == listBAPIACHE09.Count)
                return "P";
            else if (intA == listBAPIACHE09.Count)
                return "A";
            else if (intN > 0 && intN < listBAPIACHE09.Count && intP==0 && intA==0)
                return "N";
            else if (intS > 0 && intS < listBAPIACHE09.Count && intP==0 && intA==0)
                return "N";
            else if (intN > 0 && intS > 0 && intP == 0 && intA == 0)
                return "PS"; //case patial simulate
            else if (intN == 0 && intS > 0 &&  intP > 0 && intA == 0)
                return "PP"; //case paitial post
            else if (intN == 0 && intS > 0 && intP == 0 && intA > 0)
                return "PP"; //case paitial post
            else if (intN == 0 && intS == 0 && intP > 0 && intA > 0)
                return "P";  //case patial approve
            else
                return null;
        }
        #endregion public string GetDocumentStatus(long DocId,string DocKind)


        #region public virtual IList<BAPISimulateReturn> BAPISimulate(string DocId)
        public virtual IList<BAPISimulateReturn> BAPISimulate(long DocId,string DocKind)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPISimulateReturn>   SimulateReturn = new List<BAPISimulateReturn>();
            IList<Bapiache09>           listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId,DocKind);

            if (IsOpenSimulator())
            {
                #region OpenSimulator
                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("N") || strDocId.Equals("S"))
                    {
                        #region Simulator
                        listBAPIACHE09[i].DocStatus = "S";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[i]);

                        BAPIRET2Table RET2Table = new BAPIRET2Table();
                        BAPIRET2 RET2           = new BAPIRET2();
                        RET2.Type               = "S";
                        RET2.Message            = "Simulate Complete !!!";
                        RET2Table.Add(RET2);

                        BAPISimulateReturn bapiReturn = new BAPISimulateReturn();
                        bapiReturn.ComCode          = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ           = listBAPIACHE09[i].DocSeq;
                        bapiReturn.SimulateStatus   = "S";
                        bapiReturn.SimulateReturn   = RET2Table;
                        SimulateReturn.Add(bapiReturn);
                        #endregion Simulator
                    }
                }
                #endregion OpenSimulator
            }
            else
            {
                #region BAPI Service

                #region Design Variable SQL Server
                IList<SCG.eAccounting.SAP.DTO.Bapiacap09> listBAPIACAP09 = new List<SCG.eAccounting.SAP.DTO.Bapiacap09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacar09> listBAPIACAR09 = new List<SCG.eAccounting.SAP.DTO.Bapiacar09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiaccr09> listBAPIACCR09 = new List<SCG.eAccounting.SAP.DTO.Bapiaccr09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacextc> listBAPIACEXTC = new List<SCG.eAccounting.SAP.DTO.Bapiacextc>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacgl09> listBAPIACGL09 = new List<SCG.eAccounting.SAP.DTO.Bapiacgl09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacpa09> listBAPIACPA09 = new List<SCG.eAccounting.SAP.DTO.Bapiacpa09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiactx09> listBAPIACTX09 = new List<SCG.eAccounting.SAP.DTO.Bapiactx09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiret2> listBAPIRET2 = new List<SCG.eAccounting.SAP.DTO.Bapiret2>();
                #endregion Design Variable SQL Server

                #region Design Variable SAP
                BAPIACCAHD ACCAHD = new BAPIACCAHD();
                BAPIACPA09 ACPA09 = new BAPIACPA09();
                BAPIACHE09 ACHE09 = new BAPIACHE09();                           //Send Head
                BAPIACGL09Table ACGL09Table = new BAPIACGL09Table();
                BAPIACAP09Table ACAP09Table = new BAPIACAP09Table();            //Send Detail1
                BAPIACAR09Table ACAR09Table = new BAPIACAR09Table();
                BAPIACTX09Table ACTX09Table = new BAPIACTX09Table();
                BAPIACCAITTable ACCAITTable = new BAPIACCAITTable();
                BAPIACKEC9Table ACKEC9Table = new BAPIACKEC9Table();
                BAPIACCR09Table ACCR09Table = new BAPIACCR09Table();            //Send Detail2
                BAPIACEXTCTable ACEXTCTable = new BAPIACEXTCTable();            //Send Folter
                BAPIPAREXTable PAREXTable = new BAPIPAREXTable();
                BAPIACPC09Table ACPC09Table = new BAPIACPC09Table();
                BAPIACRE09Table ACRE09Table = new BAPIACRE09Table();
                BAPIRET2Table RET2Table = new BAPIRET2Table();
                BAPIACKEV9Table ACKEV9Table = new BAPIACKEV9Table();
                #endregion Design Variable SAP

                SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_CHECK simulate = new SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_CHECK(GetSAPConnectionString());

                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("N") || strDocId.Equals("S"))
                    {
                        #region New Variable SAP
                        ACCAHD = new BAPIACCAHD();
                        ACPA09 = new BAPIACPA09();
                        ACHE09 = new BAPIACHE09();                      //Send Head
                        ACGL09Table = new BAPIACGL09Table();
                        ACAP09Table = new BAPIACAP09Table();            //Send Detail1
                        ACAR09Table = new BAPIACAR09Table();
                        ACTX09Table = new BAPIACTX09Table();
                        ACCAITTable = new BAPIACCAITTable();
                        ACKEC9Table = new BAPIACKEC9Table();
                        ACCR09Table = new BAPIACCR09Table();            //Send Detail2
                        ACEXTCTable = new BAPIACEXTCTable();            //Send Folter
                        PAREXTable = new BAPIPAREXTable();
                        ACPC09Table = new BAPIACPC09Table();
                        ACRE09Table = new BAPIACRE09Table();
                        RET2Table = new BAPIRET2Table();
                        ACKEV9Table = new BAPIACKEV9Table();
                        #endregion New Variable SAP

                        #region GetPostingDataByDocId
                        this.GetPostingDataByDocId(
                            listBAPIACHE09[i].DocId, listBAPIACHE09[i].DocSeq, listBAPIACHE09[i].DocKind,
                            ref listBAPIACEXTC, ref listBAPIACPA09,
                            ref listBAPIACAP09, ref listBAPIACGL09,
                            ref listBAPIACTX09, ref listBAPIACCR09,
                            ref listBAPIACAR09);
                        #endregion GetPostingDataByDocId()

                        #region SetPostingDataToSAP
                        this.SetPostingDataToSAP(
                            listBAPIACHE09[i],
                            listBAPIACPA09, listBAPIACEXTC,
                            listBAPIACAP09, listBAPIACGL09,
                            listBAPIACTX09, listBAPIACCR09, listBAPIACAR09,
                            ref ACHE09,
                            ref ACPA09, ref ACEXTCTable,
                            ref ACAP09Table, ref ACGL09Table,
                            ref ACTX09Table, ref ACCR09Table, ref ACAR09Table);
                        #endregion SetPostingDataToSAP

                        #region Call SAP BAPI

                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Start Simulate");
                        simulate.Ybapi_Acc_Document_Check(
                            ACCAHD, ACPA09, ACHE09,
                            ref ACGL09Table, ref ACAP09Table, ref ACAR09Table,
                            ref ACTX09Table, ref ACCAITTable, ref ACKEC9Table,
                            ref ACCR09Table, ref ACEXTCTable, ref PAREXTable,
                            ref ACPC09Table, ref ACRE09Table, ref RET2Table,
                            ref ACKEV9Table);
                        simulate.CommitWork();
                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Finished Simulate");
                        #endregion Call SAP BAPI

                        #region Update Value To WebApp
                        if (GetReturnStatus(RET2Table))
                        {
                            listBAPIACHE09[i].DocStatus = "S";
                            BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[i]);
                        }
                        #endregion Update Value To WebApp

                        #region SaveReturnPostingDataToDatabase
                        this.SaveReturnPostingDataToDatabase(
                            listBAPIACHE09[i].DocId, listBAPIACHE09[i].DocSeq, listBAPIACHE09[i].DocKind + "-Simulate",
                            ref listBAPIACPA09,
                            ref listBAPIACEXTC, ref listBAPIACAP09,
                            ref listBAPIACGL09, ref listBAPIACTX09,
                            ref listBAPIACCR09, ref listBAPIACAR09, ref listBAPIRET2,
                            ACPA09,
                            ACEXTCTable, ACAP09Table,
                            ACGL09Table, ACTX09Table,
                            ACCR09Table, ACAR09Table, RET2Table);
                        #endregion SaveReturnPostingDataToDatabase

                        BAPISimulateReturn bapiReturn = new BAPISimulateReturn();
                        bapiReturn.ComCode = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ           = listBAPIACHE09[i].DocSeq;
                        bapiReturn.SimulateStatus   = RET2Table[0].Type;
                        bapiReturn.SimulateReturn   = RET2Table;
                        SimulateReturn.Add(bapiReturn);
                    }
                }

                simulate.Connection.Close();
                simulate.Dispose();
                #endregion BAPI Service
            }
            
            return SimulateReturn;
        }
        #endregion public virtual IList<BAPISimulateReturn> BAPISimulate(string DocId)

        #region public virtual IList<BAPIPostingReturn> BAPIPosting(string DocId)
        public virtual IList<BAPIPostingReturn> BAPIPosting(long DocId,string DocKind)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIPostingReturn> PostingReturn = new List<BAPIPostingReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

            if (IsOpenSimulator())
            {
                #region OpenSimulator
                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("S"))
                    {
                        #region Simulator
                        listBAPIACHE09[i].DocStatus = "P";
                        listBAPIACHE09[i].FiDoc = GenerateRunning.GetFIDoc(listBAPIACHE09[i].DocDate.Substring(0, 4), listBAPIACHE09[i].DocDate.Substring(4, 2));
                        listBAPIACHE09[i].DocYear = listBAPIACHE09[i].DocDate.Substring(0, 4);
                        listBAPIACHE09[i].FiscYear = listBAPIACHE09[i].DocDate.Substring(0, 4);
                        listBAPIACHE09[i].FisPeriod = listBAPIACHE09[i].DocDate.Substring(3, 2);
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[i]);

                        BAPIRET2Table RET2Table = new BAPIRET2Table();
                        BAPIRET2 RET2   = new BAPIRET2();
                        RET2.Type       = "S";
                        RET2.Message    = "Posting Complete !!!";
                        RET2Table.Add(RET2);

                        BAPIPostingReturn bapiReturn = new BAPIPostingReturn();
                        bapiReturn.ComCode          = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ           = listBAPIACHE09[i].DocSeq;
                        bapiReturn.PostingStatus    = "S";
                        bapiReturn.PostingReturn    = RET2Table;
                        PostingReturn.Add(bapiReturn);
                        #endregion Simulator
                    }
                }
                #endregion OpenSimulator
            }
            else
            {
                #region BAPI Service

                #region Design Variable SQL Server
                IList<SCG.eAccounting.SAP.DTO.Bapiacap09> listBAPIACAP09 = new List<SCG.eAccounting.SAP.DTO.Bapiacap09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacar09> listBAPIACAR09 = new List<SCG.eAccounting.SAP.DTO.Bapiacar09>();

                IList<SCG.eAccounting.SAP.DTO.Bapiaccr09> listBAPIACCR09 = new List<SCG.eAccounting.SAP.DTO.Bapiaccr09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacextc> listBAPIACEXTC = new List<SCG.eAccounting.SAP.DTO.Bapiacextc>();

                IList<SCG.eAccounting.SAP.DTO.Bapiacgl09> listBAPIACGL09 = new List<SCG.eAccounting.SAP.DTO.Bapiacgl09>();

                IList<SCG.eAccounting.SAP.DTO.Bapiacpa09> listBAPIACPA09 = new List<SCG.eAccounting.SAP.DTO.Bapiacpa09>();

                IList<SCG.eAccounting.SAP.DTO.Bapiactx09> listBAPIACTX09 = new List<SCG.eAccounting.SAP.DTO.Bapiactx09>();

                IList<SCG.eAccounting.SAP.DTO.Bapiret2> listBAPIRET2 = new List<SCG.eAccounting.SAP.DTO.Bapiret2>();
                #endregion Design Variable SQL Server

                #region Design Variable SAP
                BAPIACCAHD ACCAHD = new BAPIACCAHD();
                BAPIACPA09 ACPA09 = new BAPIACPA09();
                BAPIACHE09 ACHE09 = new BAPIACHE09();                           //Send Head
                BAPIACGL09Table ACGL09Table = new BAPIACGL09Table();
                BAPIACAP09Table ACAP09Table = new BAPIACAP09Table();            //Send Detail1
                BAPIACAR09Table ACAR09Table = new BAPIACAR09Table();
                BAPIACTX09Table ACTX09Table = new BAPIACTX09Table();
                BAPIACCAITTable ACCAITTable = new BAPIACCAITTable();
                BAPIACKEC9Table ACKEC9Table = new BAPIACKEC9Table();
                BAPIACCR09Table ACCR09Table = new BAPIACCR09Table();            //Send Detail2
                BAPIACEXTCTable ACEXTCTable = new BAPIACEXTCTable();            //Send Folter
                BAPIPAREXTable PAREXTable = new BAPIPAREXTable();
                BAPIACPC09Table ACPC09Table = new BAPIACPC09Table();
                BAPIACRE09Table ACRE09Table = new BAPIACRE09Table();
                BAPIRET2Table RET2Table = new BAPIRET2Table();
                BAPIACKEV9Table ACKEV9Table = new BAPIACKEV9Table();
                #endregion Design Variable SAP

                SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_POST_COMMIT check = new SAPProxy_YBAPI_ACC_DOCUMENT_POST_COMMIT(GetSAPConnectionString());

                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("S"))
                    {
                        #region BAPI Service
                        string ObjKey;
                        string ObjSys;
                        string ObjType;

                        #region New Variable SAP
                        ACCAHD = new BAPIACCAHD();
                        ACPA09 = new BAPIACPA09();
                        ACHE09 = new BAPIACHE09();                           //Send Head
                        ACGL09Table = new BAPIACGL09Table();
                        ACAP09Table = new BAPIACAP09Table();            //Send Detail1
                        ACAR09Table = new BAPIACAR09Table();
                        ACTX09Table = new BAPIACTX09Table();
                        ACCAITTable = new BAPIACCAITTable();
                        ACKEC9Table = new BAPIACKEC9Table();
                        ACCR09Table = new BAPIACCR09Table();            //Send Detail2
                        ACEXTCTable = new BAPIACEXTCTable();            //Send Folter
                        PAREXTable = new BAPIPAREXTable();
                        ACPC09Table = new BAPIACPC09Table();
                        ACRE09Table = new BAPIACRE09Table();
                        RET2Table = new BAPIRET2Table();
                        ACKEV9Table = new BAPIACKEV9Table();
                        #endregion New Variable SAP
   
                        #region GetPostingDataByDocId
                        this.GetPostingDataByDocId(
                            listBAPIACHE09[i].DocId, listBAPIACHE09[i].DocSeq, listBAPIACHE09[i].DocKind,
                            ref listBAPIACEXTC, ref listBAPIACPA09,
                            ref listBAPIACAP09, ref listBAPIACGL09,
                            ref listBAPIACTX09, ref listBAPIACCR09, ref listBAPIACAR09);
                        #endregion GetPostingDataByDocId()

                        #region SetPostingDataToSAP
                        this.SetPostingDataToSAP(
                            listBAPIACHE09[i],
                            listBAPIACPA09, listBAPIACEXTC,
                            listBAPIACAP09, listBAPIACGL09,
                            listBAPIACTX09, listBAPIACCR09, listBAPIACAR09,
                            ref ACHE09,
                            ref ACPA09, ref ACEXTCTable,
                            ref ACAP09Table, ref ACGL09Table,
                            ref ACTX09Table, ref ACCR09Table, ref ACAR09Table);
                        #endregion SetPostingDataToSAP

                        #region Call SAP BAPI
                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Start Posting");
                        check.Ybapi_Acc_Document_Post_Commit(
                            ACCAHD, ACPA09, ACHE09,
                            out ObjKey, out ObjSys, out ObjType,
                            ref ACGL09Table, ref ACAP09Table, ref ACAR09Table,
                            ref ACTX09Table, ref ACCAITTable, ref ACKEC9Table,
                            ref ACCR09Table, ref ACEXTCTable, ref PAREXTable,
                            ref ACPC09Table, ref ACRE09Table, ref RET2Table,
                            ref ACKEV9Table);
                        check.CommitWork();
                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Finished Posting");
                        #endregion Call SAP BAPI

                        #region Update Value To WebApp
                        if (GetReturnStatus(RET2Table))
                        {
                            listBAPIACHE09[i].DocStatus = "P";
                            listBAPIACHE09[i].FiDoc     = ObjKey.Substring(0, 10);
                            listBAPIACHE09[i].DocYear   = ObjKey.Substring(14, 4);
                            listBAPIACHE09[i].ObjKey    = ObjKey;
                            listBAPIACHE09[i].ObjType   = ObjType;
                            listBAPIACHE09[i].ObjSys    = ObjSys;
                            listBAPIACHE09[i].FiscYear  = ACHE09.Fisc_Year;
                            listBAPIACHE09[i].FisPeriod = ACHE09.Fis_Period;
                            listBAPIACHE09[i].HeaderTxt = BAPIGetFIDoc(ObjKey.Substring(0, 10), ObjKey.Substring(14, 4), listBAPIACHE09[i].CompCode);
                            listBAPIACHE09[i].ObjKeyR   = ACHE09.Obj_Key_R;
                            BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[i]);
                        }
                        #endregion Update Value To WebApp

                        #region SaveReturnPostingDataToDatabase
                        this.SaveReturnPostingDataToDatabase(
                            listBAPIACHE09[i].DocId, listBAPIACHE09[i].DocSeq, listBAPIACHE09[i].DocKind + "-Posting",
                            ref listBAPIACPA09,
                            ref listBAPIACEXTC, ref listBAPIACAP09,
                            ref listBAPIACGL09, ref listBAPIACTX09,
                            ref listBAPIACCR09, ref listBAPIACAR09, ref listBAPIRET2,
                            ACPA09,
                            ACEXTCTable, ACAP09Table,
                            ACGL09Table, ACTX09Table,
                            ACCR09Table, ACAR09Table, RET2Table);
                        #endregion SaveReturnPostingDataToDatabase

                        // ***********************
                        // Return Value
                        // ***********************
                        BAPIPostingReturn bapiReturn = new BAPIPostingReturn();
                        bapiReturn.ComCode          = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ           = listBAPIACHE09[i].DocSeq;
                        bapiReturn.ObjKey           = ObjKey;
                        bapiReturn.ObjSys           = ObjSys;
                        bapiReturn.ObjType          = ObjType;
                        bapiReturn.PostingStatus    = RET2Table[0].Type;
                        bapiReturn.PostingReturn    = RET2Table;
                        PostingReturn.Add(bapiReturn);
                        #endregion BAPI Service
                    }
                }

                check.Connection.Close();
                check.Dispose();
                #endregion BAPI Service
            }

            #region Update Document Table
            SCG.eAccounting.BLL.Implement.SCGDocumentService scgService = new SCGDocumentService();

            if (DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                if (GetDocumentStatus(DocId, DocKind) == "P")
                    scgService.UpdatePostingStatusFnDocument(DocId, "P");
                else if (GetDocumentStatus(DocId, DocKind) == "PP")
                    scgService.UpdatePostingStatusFnDocument(DocId, "PP");
            }
            else
            {
                if (GetDocumentStatus(DocId, DocKind) == "P")
                    scgService.UpdatePostingStatusDocument(DocId, "P");
                else if (GetDocumentStatus(DocId, DocKind) == "PP")
                    scgService.UpdatePostingStatusDocument(DocId, "PP");
            }
            #endregion Update Document Table

            return PostingReturn;
        }
        #endregion public virtual string BAPIPosting(string DocId)

        #region public virtual IList<BAPIApproveReturn> BAPIApprove(long DocId, string DocKind)
        public virtual IList<BAPIApproveReturn> BAPIApprove(long DocId, string DocKind , long UserAccountID)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIApproveReturn> ApproveReturn = new List<BAPIApproveReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);
            ZACCKEY2Table zaccker2Table = new ZACCKEY2Table();

            if (IsOpenSimulator())
            {
                #region OpenSimulator
                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("P"))
                    {
                        #region Simulator
                        listBAPIACHE09[i].DocStatus = "A";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[0]);

                        BAPIRET2Table RET2Table = new BAPIRET2Table();
                        BAPIRET2 RET2 = new BAPIRET2();
                        RET2.Type = "S";
                        RET2.Message = "Approve Complete !!!";
                        RET2Table.Add(RET2);

                        BAPIApproveReturn bapiReturn = new BAPIApproveReturn();
                        bapiReturn.ComCode = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ = listBAPIACHE09[i].DocSeq;
                        bapiReturn.ApproveStatus = "S";
                        bapiReturn.ApproveReturn = RET2Table;
                        ApproveReturn.Add(bapiReturn);
                        #endregion Simulator
                    }
                }
                #endregion OpenSimulator
            }
            else
            {
                #region BAPI Service

                #region Design Variable SQL Server
                IList<SCG.eAccounting.SAP.DTO.Bapiacap09> listBAPIACAP09 = new List<SCG.eAccounting.SAP.DTO.Bapiacap09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacar09> listBAPIACAR09 = new List<SCG.eAccounting.SAP.DTO.Bapiacar09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiaccr09> listBAPIACCR09 = new List<SCG.eAccounting.SAP.DTO.Bapiaccr09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacextc> listBAPIACEXTC = new List<SCG.eAccounting.SAP.DTO.Bapiacextc>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacgl09> listBAPIACGL09 = new List<SCG.eAccounting.SAP.DTO.Bapiacgl09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiacpa09> listBAPIACPA09 = new List<SCG.eAccounting.SAP.DTO.Bapiacpa09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiactx09> listBAPIACTX09 = new List<SCG.eAccounting.SAP.DTO.Bapiactx09>();
                IList<SCG.eAccounting.SAP.DTO.Bapiret2> listBAPIRET2 = new List<SCG.eAccounting.SAP.DTO.Bapiret2>();
                #endregion Design Variable SQL Server

                #region Design Variable SAP
                BAPIACCAHD ACCAHD = new BAPIACCAHD();
                BAPIACPA09 ACPA09 = new BAPIACPA09();
                BAPIACHE09 ACHE09 = new BAPIACHE09();                           //Send Head
                BAPIACGL09Table ACGL09Table = new BAPIACGL09Table();
                BAPIACAP09Table ACAP09Table = new BAPIACAP09Table();            //Send Detail1
                BAPIACAR09Table ACAR09Table = new BAPIACAR09Table();
                BAPIACTX09Table ACTX09Table = new BAPIACTX09Table();
                BAPIACCAITTable ACCAITTable = new BAPIACCAITTable();
                BAPIACKEC9Table ACKEC9Table = new BAPIACKEC9Table();
                BAPIACCR09Table ACCR09Table = new BAPIACCR09Table();            //Send Detail2
                BAPIACEXTCTable ACEXTCTable = new BAPIACEXTCTable();            //Send Folter
                BAPIPAREXTable PAREXTable = new BAPIPAREXTable();
                BAPIACPC09Table ACPC09Table = new BAPIACPC09Table();
                BAPIACRE09Table ACRE09Table = new BAPIACRE09Table();
                BAPIRET2Table RET2Table = new BAPIRET2Table();
                BAPIACKEV9Table ACKEV9Table = new BAPIACKEV9Table();
                #endregion Design Variable SAP

                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("P"))
                    {
                        #region New Variable SAP
                        ACCAHD = new BAPIACCAHD();
                        ACPA09 = new BAPIACPA09();
                        ACHE09 = new BAPIACHE09();                           //Send Head
                        ACGL09Table = new BAPIACGL09Table();
                        ACAP09Table = new BAPIACAP09Table();            //Send Detail1
                        ACAR09Table = new BAPIACAR09Table();
                        ACTX09Table = new BAPIACTX09Table();
                        ACCAITTable = new BAPIACCAITTable();
                        ACKEC9Table = new BAPIACKEC9Table();
                        ACCR09Table = new BAPIACCR09Table();            //Send Detail2
                        ACEXTCTable = new BAPIACEXTCTable();            //Send Folter
                        PAREXTable = new BAPIPAREXTable();
                        ACPC09Table = new BAPIACPC09Table();
                        ACRE09Table = new BAPIACRE09Table();
                        RET2Table = new BAPIRET2Table();
                        ACKEV9Table = new BAPIACKEV9Table();
                        #endregion New Variable SAP

                        #region GetPostingDataByDocId
                        this.GetPostingDataByDocId(
                            listBAPIACHE09[i].DocId, listBAPIACHE09[i].DocSeq, listBAPIACHE09[i].DocKind,
                            ref listBAPIACEXTC, ref listBAPIACPA09,
                            ref listBAPIACAP09, ref listBAPIACGL09,
                            ref listBAPIACTX09, ref listBAPIACCR09,
                            ref listBAPIACAR09);
                        #endregion GetPostingDataByDocId()

                        #region Call SAP BAPI
                        ZACCKEYTable    zacKeyTable = new ZACCKEYTable();
                        ZACCKEY         zacKey      = new ZACCKEY();

                        if (listBAPIACHE09[i].DocAppFlag == null || listBAPIACHE09[i].DocAppFlag.ToString() == "")
                            zacKey.App_Type = "A";
                        else
                            zacKey.App_Type = listBAPIACHE09[i].DocAppFlag;

                        zacKey.Bukrs = listBAPIACHE09[i].CompCode;
                        zacKey.Belnr = listBAPIACHE09[i].FiDoc;
                        zacKey.Gjahr = listBAPIACHE09[i].DocYear;
                        zacKeyTable.Add(zacKey);

                        string eFormID = listBAPIACHE09[0].RefDocNo.Substring(0, 3) + listBAPIACHE09[0].RefDocNo.Substring(listBAPIACHE09[0].RefDocNo.Length - 7, 7);
                        SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_EFORMID check = new SAPProxy_YBAPI_EFORMID(GetSAPConnectionString());

                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Start Approve");
                        check.Ybapi_Eformid(
                            SAPUIHelper.GetEmployee(UserAccountID).UserName,
                            eFormID,
                            ref zacKeyTable,
                            ref RET2Table);
                        check.CommitWork();
                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Finished Approve");
                        #endregion Call SAP BAPI

                        #region Update Value To WebApp
                        if (GetReturnStatus(RET2Table))
                        {
                            listBAPIACHE09[i].DocStatus = "A";
                            BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[0]);
                        }
                        #endregion Update Value To WebApp

                        #region SaveReturnPostingDataToDatabase
                        this.SaveReturnPostingDataToDatabase(
                            listBAPIACHE09[i].DocId, listBAPIACHE09[i].DocSeq, listBAPIACHE09[i].DocKind + "-Approve",
                            ref listBAPIACPA09,
                            ref listBAPIACEXTC, ref listBAPIACAP09,
                            ref listBAPIACGL09, ref listBAPIACTX09,
                            ref listBAPIACCR09, ref listBAPIACAR09, ref listBAPIRET2,
                            ACPA09,
                            ACEXTCTable, ACAP09Table,
                            ACGL09Table, ACTX09Table,
                            ACCR09Table, ACAR09Table, RET2Table);
                        #endregion SaveReturnPostingDataToDatabase

                        // ***********************
                        // Return Value
                        // ***********************
                        BAPIApproveReturn bapiReturn = new BAPIApproveReturn();
                        bapiReturn.ComCode = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ = listBAPIACHE09[i].DocSeq;
                        bapiReturn.ApproveStatus = RET2Table[0].Type;
                        bapiReturn.ApproveReturn = RET2Table;
                        ApproveReturn.Add(bapiReturn);
                    }
                }
                #endregion BAPI Service
            }

            #region Update Document Table
            SCG.eAccounting.BLL.Implement.SCGDocumentService scgService = new SCGDocumentService();

            if (DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                if (GetDocumentStatus(DocId, DocKind) == "A")
                    scgService.UpdatePostingStatusFnDocument(DocId, "C");
                else if (GetDocumentStatus(DocId, DocKind) == "P")
                    scgService.UpdatePostingStatusFnDocument(DocId, "P");
            }
            else
            {
                if (GetDocumentStatus(DocId, DocKind) == "A")
                    scgService.UpdatePostingStatusDocument(DocId, "C");
                else if (GetDocumentStatus(DocId, DocKind) == "P")
                    scgService.UpdatePostingStatusDocument(DocId, "P");
            }
            #endregion Update Document Table

            return ApproveReturn;
        }
        #endregion public virtual IList<BAPIApproveReturn> BAPIApprove(long DocId, string DocKind)

        #region public virtual IList<BAPIReverseReturn> BAPIReverse(string DocId)
        public virtual IList<BAPIReverseReturn> BAPIReverse(long DocId, string DocKind)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIReverseReturn> ReverseReturn = new List<BAPIReverseReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

            if (IsOpenSimulator())
            {
                #region OpenSimulator
                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("P") || strDocId.Equals("A"))
                    {
                        #region Simulator
                        listBAPIACHE09[i].RefDocNo = GenerateRunning.GetFIDoc(listBAPIACHE09[i].DocDate.Substring(0, 4), listBAPIACHE09[i].DocDate.Substring(4, 2));
                        listBAPIACHE09[i].DocStatus = "S";
                        listBAPIACHE09[i].DocYear = "";
                        listBAPIACHE09[i].FiDoc = "";
                        listBAPIACHE09[i].ObjKey = "";
                        listBAPIACHE09[i].ObjSys = "";
                        listBAPIACHE09[i].ObjType = "";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[0]);

                        BAPIRET2Table RET2Table = new BAPIRET2Table();
                        BAPIRET2 RET2 = new BAPIRET2();
                        RET2.Type = "S";
                        RET2.Message = "Reverse Complete !!!";
                        RET2Table.Add(RET2);

                        BAPIReverseReturn bapiReturn = new BAPIReverseReturn();
                        bapiReturn.ComCode = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ = listBAPIACHE09[i].DocSeq;
                        bapiReturn.ReverseStatus = "S";
                        bapiReturn.ReverseReturn = RET2Table;
                        ReverseReturn.Add(bapiReturn);
                        #endregion Simulator
                    }
                }
                #endregion OpenSimulator
            }
            else
            {
                #region BAPI Service
                SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_DOCUMENT_POST_REVERSE reverse = new SAPProxy_YBAPI_DOCUMENT_POST_REVERSE(GetSAPConnectionString());

                for (int i = 0; i < listBAPIACHE09.Count; i++)
                {
                    string strDocId = string.IsNullOrEmpty(listBAPIACHE09[i].DocStatus) ? string.Empty : listBAPIACHE09[i].DocStatus;
                    if (strDocId.Equals("P") || strDocId.Equals("A"))
                    {
                        #region BAPI Service
                        string strComID = listBAPIACHE09[i].CompCode;
                        string strFIDOC = listBAPIACHE09[i].FiDoc;
                        string strDocYear = listBAPIACHE09[i].DocYear;
                        string strPstDate = listBAPIACHE09[i].PstngDate;
                        string strReason = "01";

                        string strFiDocOld = "";
                        string strFiDocReverse = "";

                        #region Call BAPI
                        string strOutCompanyCode;
                        string strOutDocId;
                        string strOutDocYear;
                        string strOutFlag;
                        string strOutMsg;

                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Start Reverse");
                        reverse.Ybapi_Document_Post_Reverse(
                            strComID, strFIDOC, strDocYear, strPstDate, strReason,
                            out strOutCompanyCode, out strOutDocId, out strOutDocYear, out strOutFlag, out strOutMsg);
                        reverse.CommitWork();
                        bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listBAPIACHE09[i].DocSeq + " Finished Reverse");
                        #endregion Call BAPI

                        #region Update Value To WebApp
                        if (strOutDocId != "" && strOutFlag != "E" && strOutFlag != "e")
                        {
                            #region History
                            strFiDocOld = listBAPIACHE09[i].FiDoc;
                            strFiDocReverse = strOutDocId;

                            Bapireverse bapiReverse = new Bapireverse();
                            bapiReverse.DocId = listBAPIACHE09[i].DocId;
                            bapiReverse.DocKind = listBAPIACHE09[i].DocKind;
                            bapiReverse.DocSeq = listBAPIACHE09[i].DocSeq;
                            bapiReverse.DocYear = listBAPIACHE09[i].DocYear;
                            bapiReverse.DocAppFlag = listBAPIACHE09[i].DocAppFlag;
                            bapiReverse.FiDoc = listBAPIACHE09[i].FiDoc;
                            bapiReverse.ObjKey = listBAPIACHE09[i].ObjKey;
                            bapiReverse.ObjSys = listBAPIACHE09[i].ObjSys;
                            bapiReverse.ObjType = listBAPIACHE09[i].ObjType;
                            bapiReverse.ReverseDoc = strOutDocId;
                            bapiReverse.ReverseDocFlag = strOutFlag;
                            bapiReverse.ReverseDocMsg = strOutMsg;
                            bapiReverse.ReverseDocYear = strOutDocYear;

                            bapiReverse.Active = true;
                            bapiReverse.CreBy = 1;
                            bapiReverse.CreDate = DateTime.Now;
                            bapiReverse.UpdBy = 1;
                            bapiReverse.UpdDate = DateTime.Now;
                            bapiReverse.UpdPgm = "PostingService";
                            BapiServiceProvider.BapireverseService.SaveOrUpdate(bapiReverse);
                            #endregion History

                            listBAPIACHE09[i].ReverseDoc = strOutDocId;
                            listBAPIACHE09[i].ReasonRev = strReason;
                            listBAPIACHE09[i].DocStatus = "S";

                            listBAPIACHE09[i].DocYear = "";
                            listBAPIACHE09[i].FiDoc = "";
                            listBAPIACHE09[i].ObjKey = "";
                            listBAPIACHE09[i].ObjSys = "";
                            listBAPIACHE09[i].ObjType = "";
                            listBAPIACHE09[i].HeaderTxt = "";
                            BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[0]);
                        }
                        #endregion Update Value To WebApp

                        // ***********************
                        // Return Value
                        // ***********************
                        // For Return Message
                        string strMsg = "";
                        if (strOutDocId != "" && strOutFlag != "E" && strOutFlag != "e")
                            strMsg = strOutMsg + " --->>> Old FI DOC : " + strFiDocOld + " --->>> FI DOC Reverse : " + strFiDocReverse;
                        else
                            strMsg = strOutMsg;

                        #region Return to From
                        BAPIRET2Table   RET2Table   = new BAPIRET2Table();
                        BAPIRET2        RET2        = new BAPIRET2();
                        RET2.Type       = strOutFlag;
                        RET2.Message    = strMsg;
                        RET2Table.Add(RET2);

                        BAPIReverseReturn bapiReturn = new BAPIReverseReturn();
                        bapiReturn.ComCode = listBAPIACHE09[i].CompCode;
                        bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listBAPIACHE09[i].CompCode).CompanyName;

                        bapiReturn.DOCSEQ           = listBAPIACHE09[i].DocSeq;
                        bapiReturn.OutCompanyCode   = strOutCompanyCode;
                        bapiReturn.OutDocId         = strOutDocId;
                        bapiReturn.OutDocYear       = strOutDocYear;
                        bapiReturn.OutFlag          = strOutFlag;
                        bapiReturn.OutMsg           = strOutMsg;
                        bapiReturn.ReverseStatus    = strOutFlag;
                        bapiReturn.ReverseReturn    = RET2Table;

                        ReverseReturn.Add(bapiReturn);
                        #endregion Return to From

                        #region SaveReturnPostingDataToDatabase
                        Bapiret2 tmp    = new Bapiret2();
                        tmp.DocId       = listBAPIACHE09[i].DocId;
                        tmp.DocSeq      = listBAPIACHE09[i].DocSeq;
                        tmp.DocKind     = listBAPIACHE09[i].DocKind + "-Reverse";
                        tmp.Message     = SAPUIHelper.SubString(220, strMsg);
                        tmp.MessageV1   = SAPUIHelper.SubString(50, strOutMsg);
                        tmp.MessageV2   = SAPUIHelper.SubString(50, "Old FI DOC : " + strFiDocOld);
                        tmp.MessageV3   = SAPUIHelper.SubString(50, "FI DOC Reverse : " + strFiDocReverse);
                        tmp.Type        = SAPUIHelper.SubString(1, strOutFlag);

                        tmp.Active      = true;
                        tmp.CreBy       = 1;
                        tmp.CreDate     = DateTime.Now;
                        tmp.UpdBy       = 1;
                        tmp.UpdDate     = DateTime.Now;
                        tmp.UpdPgm      = "POSTING";

                        BapiServiceProvider.Bapiret2Service.Save(tmp);
                        #endregion SaveReturnPostingDataToDatabase

                        #endregion BAPI Service
                    }
                }

                reverse.Connection.Close();
                reverse.Dispose();
                #endregion BAPI Service
            }

            #region Update Document Table
            SCG.eAccounting.BLL.Implement.SCGDocumentService scgService = new SCGDocumentService();

            if (DocKind == DocumentKind.ExpenseRemittance.ToString())
            {
                if (GetDocumentStatus(DocId, DocKind) == "S")
                    scgService.UpdatePostingStatusFnDocument(DocId, "N");
                else if (GetDocumentStatus(DocId, DocKind) == "PP")
                    scgService.UpdatePostingStatusFnDocument(DocId, "PP");
            }
            else
            {
                if (GetDocumentStatus(DocId, DocKind) == "S")
                    scgService.UpdatePostingStatusDocument(DocId, "N");
                else if (GetDocumentStatus(DocId, DocKind) == "PP")
                    scgService.UpdatePostingStatusDocument(DocId, "PP");
            }
            #endregion Update Document Table

            return ReverseReturn;
        }
        #endregion public virtual IList<BAPIReverseReturn> BAPIReverse(string DocId)


        #region protected bool GetReturnStatus(BAPIRET2Table RET2Table)
        /// <summary>
        /// if true = S
        /// if false = E
        /// </summary>
        /// <param name="RET2Table"></param>
        /// <returns></returns>
        protected bool GetReturnStatus(BAPIRET2Table RET2Table)
        {
            bool boolReturn = false;
            if (RET2Table.Count > 0)
            {
                if (RET2Table[0].Type == "S" || RET2Table[0].Type == "s" || RET2Table[0].Type == "W" || RET2Table[0].Type == "w")
                    boolReturn = true;
            }
            return boolReturn;

            #region Old Code
            //bool boolReturn = true;
            //for (int i = 0; i < RET2Table.Count; i++)
            //{
            //    if (RET2Table[i].Type == "S" || RET2Table[i].Type == "s" || RET2Table[i].Type == "W" || RET2Table[i].Type == "w")
            //    { }
            //    else
            //    {
            //        boolReturn = false;
            //        break;
            //    }
            //}
            //return boolReturn;
            #endregion Old Code
        }
        #endregion protected bool GetReturnStatus(BAPIRET2Table RET2Table)

        #region protected string BAPIGetFIDoc(string FIDoc , string DocYear , string ComCode )
        protected string BAPIGetFIDoc(string FIDoc, string DocYear, string ComCode)
        {
            YGETFIDOC_HTable hTable = new YGETFIDOC_HTable();
            YGETFIDOC_ITable iTable = new YGETFIDOC_ITable();

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_GETFIDOC getFiDoc = new SAPProxy_YBAPI_GETFIDOC(GetSAPConnectionString());
            getFiDoc.Ybapi_Getfidoc(ComCode, FIDoc, DocYear, ref hTable, ref iTable);

            return hTable[0].Bktxt;
        }
        #endregion protected string BAPIGetFIDoc(string FIDoc , string DocYear , string ComCode )

    }

    #region Struct for Return
    public struct BAPISimulateReturn
    {
        public string ComCode;
        public string ComName;

        public string DOCSEQ;
        public string SimulateStatus;
        public BAPIRET2Table SimulateReturn;
    }

    public struct BAPIPostingReturn
    {
        public string ComCode;
        public string ComName;

        public string DOCSEQ;
        public string ObjKey;
        public string ObjSys;
        public string ObjType;
        public string PostingStatus;
        public BAPIRET2Table PostingReturn;
    }

    public struct BAPIReverseReturn
    {
        public string ComCode;
        public string ComName;

        public string DOCSEQ;
        public string OutCompanyCode;
        public string OutDocId;
        public string OutDocYear;
        public string OutFlag;
        public string OutMsg;
        public string ReverseStatus;
        public BAPIRET2Table ReverseReturn;
    }

    public struct BAPIApproveReturn
    {
        public string ComCode;
        public string ComName;

        public string DOCSEQ;
        public string ApproveStatus;
        public BAPIRET2Table ApproveReturn;
    }
    #endregion Struct for Return
}
