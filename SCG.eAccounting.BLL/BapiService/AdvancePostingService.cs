using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using System.Collections;
using System.Data;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Service;
using SCG.eAccounting.SAP.Service.Implement;
using SCG.eAccounting.SAP.Service.Interface;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SS.DB.DTO;
using SCG.eAccounting.DTO;
using SCG.DB.DTO;

namespace SCG.eAccounting.SAP.BAPI.Service
{
    public class AdvancePostingService : PostingService
    {
        #region public override void CreatePostData(long DocID)
        public override void CreatePostData(long DocID, string DocKind)
        {
            Hashtable paramete = new Hashtable();
            paramete.Add("@DOCUMENT_ID", DocID.ToString());
            DataSet dstPosting = new DBManage().ExecuteQuery("ADVANCE_POSTING", paramete);
            bool repOffice = false;
            string mainCurrencySymbol = string.Empty;

            SCGDocument doc = SCG.eAccounting.Query.ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(DocID);

            DbSapInstance sap = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.GetSAPDocTypeForPosting(doc.CompanyID.CompanyCode);

            if (dstPosting.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dstPosting.Tables[0].Rows[0]["IsRepOffice"].ToString()))
                {
                    repOffice = bool.Parse(dstPosting.Tables[0].Rows[0]["IsRepOffice"].ToString());
                }

                // get main currency symbol
                if (repOffice)
                {
                    DbCurrency mainCurrency = SS.DB.Query.SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(short.Parse(dstPosting.Tables[0].Rows[0]["MainCurrencyID"].ToString()));
                    if (mainCurrency != null)
                    {
                        mainCurrencySymbol = mainCurrency.Symbol;
                    }
                }

                if (dstPosting.Tables[0].Rows[0]["AdvanceType"].ToString() == ZoneTypeConst.Domestic)
                {
                    #region HEAD
                    Bapiache09 che09 = new Bapiache09();
                    che09.DocId = DocID;
                    che09.DocSeq = "M";
                    che09.DocKind = DocKind;
                    che09.BusAct = PostingConst.BusAct;
                    che09.Username = sap.UserCPIC; //PostingConst.UserCPIC;
                    che09.CompCode = dstPosting.Tables[0].Rows[0]["COMP_CODE"].ToString().Substring(0, 4);
                    che09.DocDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                    che09.DocKind = DocKind;
                    che09.DocType = sap.DocTypeAdvancePostingDM;//DocTypeConst.KR;
                    che09.PstngDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                    che09.ReverseDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                    che09.RefDocNo = SAPUIHelper.SubString(16, dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString());
                    che09.DocStatus = "N";

                    if (dstPosting.Tables[0].Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                        che09.DocAppFlag = "A";
                    else
                        che09.DocAppFlag = "V";

                    che09.Active = true;
                    che09.CreBy = 1;
                    che09.CreDate = DateTime.Now;
                    che09.UpdBy = 1;
                    che09.UpdDate = DateTime.Now;
                    che09.UpdPgm = "AdvancePosting";
                    BapiServiceProvider.Bapiache09Service.Save(che09);
                    #endregion HEAD

                    #region Foolter
                    Bapiacextc cextc = new Bapiacextc();
                    cextc.DocId = DocID;
                    cextc.DocSeq = "M";
                    cextc.DocKind = DocKind;
                    cextc.Field1 = PostingConst.BRNCH;
                    cextc.Field2 = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();

                    cextc.Active = true;
                    cextc.CreBy = 1;
                    cextc.CreDate = DateTime.Now;
                    cextc.UpdBy = 1;
                    cextc.UpdDate = DateTime.Now;
                    cextc.UpdPgm = "AdvancePosting";
                    BapiServiceProvider.BapiacextcService.Save(cextc);

                    Bapiacextc cextc1 = new Bapiacextc();
                    cextc1.DocId = DocID;
                    cextc1.DocSeq = "M";
                    cextc1.DocKind = DocKind;
                    cextc1.Field1 = PostingConst.VAT;
                    cextc1.Field2 = TaxCodeConst.NV;

                    cextc1.Active = true;
                    cextc1.CreBy = 1;
                    cextc1.CreDate = DateTime.Now;
                    cextc1.UpdBy = 1;
                    cextc1.UpdDate = DateTime.Now;
                    cextc1.UpdPgm = "AdvancePosting";
                    BapiServiceProvider.BapiacextcService.Save(cextc1);
                    #endregion Foolter

                    #region Domestic
                    Bapiacap09 capItem1 = new Bapiacap09();
                    Bapiacap09 capItem2 = new Bapiacap09();
                    Bapiaccr09 accrItem1 = new Bapiaccr09();
                    Bapiaccr09 accrItem2 = new Bapiaccr09();

                    if (!repOffice)
                    {
                        if (dstPosting.Tables[0].Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                        {
                            #region Cash

                            #region Credit
                            capItem1.DocId = DocID;
                            capItem1.DocSeq = "M";
                            capItem1.DocKind = DocKind;
                            capItem1.ItemnoAcc = "1";
                            capItem1.VendorNo = dstPosting.Tables[0].Rows[0]["PBCode"].ToString();
                            capItem1.PmntBlock = PostingConst.PmntBlock;
                            capItem1.Pmnttrms = PostingConst.Pmnttrms;
                            capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem1.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString())).EmployeeName;
                            capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                            capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                            capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem1.TaxCode = TaxCodeConst.NV;

                            if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                capItem1.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                            }

                            capItem1.Active = true;
                            capItem1.CreBy = 1;
                            capItem1.CreDate = DateTime.Now;
                            capItem1.UpdBy = 1;
                            capItem1.UpdDate = DateTime.Now;
                            capItem1.UpdPgm = "AdvancePosting";

                            accrItem1.DocId = DocID;
                            accrItem1.DocSeq = "M";
                            accrItem1.DocKind = DocKind;
                            accrItem1.ItemnoAcc = "1";

                            accrItem1.Currency = PostingConst.Currency;
                            accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString());

                            accrItem1.Active = true;
                            accrItem1.CreBy = 1;
                            accrItem1.CreDate = DateTime.Now;
                            accrItem1.UpdBy = 1;
                            accrItem1.UpdDate = DateTime.Now;
                            accrItem1.UpdPgm = "AdvancePosting";
                            #endregion Credit

                            #region Debit
                            capItem2.DocId = DocID;
                            capItem2.DocSeq = "M";
                            capItem2.DocKind = DocKind;
                            capItem2.ItemnoAcc = "2";
                            capItem2.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                            capItem2.Pmnttrms = PostingConst.Pmnttrms;
                            capItem2.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem2.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem2.PmntBlock = PostingConst.PmntBlock;
                            capItem2.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                            capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem2.AllocNmbr);
                            capItem2.ItemText = dstPosting.Tables[0].Rows[0]["DueDate"].ToString() + "/" + dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem2.ItemText = SAPUIHelper.SubString50(capItem2.ItemText);
                            capItem2.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem2.TaxCode = TaxCodeConst.NV;
                            capItem2.SpGlInd = SpGlIndConst.D;

                            if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                capItem2.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                            }

                            capItem2.Active = true;
                            capItem2.CreBy = 1;
                            capItem2.CreDate = DateTime.Now;
                            capItem2.UpdBy = 1;
                            capItem2.UpdDate = DateTime.Now;
                            capItem2.UpdPgm = "AdvancePosting";

                            accrItem2.DocId = DocID;
                            accrItem2.DocSeq = "M";
                            accrItem2.DocKind = DocKind;
                            accrItem2.ItemnoAcc = "2";

                            accrItem2.Currency = PostingConst.Currency;
                            accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString());

                            accrItem2.Active = true;
                            accrItem2.CreBy = 1;
                            accrItem2.CreDate = DateTime.Now;
                            accrItem2.UpdBy = 1;
                            accrItem2.UpdDate = DateTime.Now;
                            accrItem2.UpdPgm = "AdvancePosting";
                            #endregion Debit

                            #endregion Cash
                        }
                        else
                        {
                            #region Transfer หรือ Cheque

                            #region Credit
                            capItem1.DocId = DocID;
                            capItem1.DocSeq = "M";
                            capItem1.DocKind = DocKind;
                            capItem1.ItemnoAcc = "1";
                            capItem1.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString())).VendorCode;
                            capItem1.PmntBlock = "";
                            capItem1.Pmnttrms = PostingConst.Pmnttrms;
                            capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem1.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                            capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                            capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                            capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem1.TaxCode = TaxCodeConst.NV;

                            if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                capItem1.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                            }

                            capItem1.Pmtmthsupl = dstPosting.Tables[0].Rows[0]["Supplementary"].ToString();

                            capItem1.Active = true;
                            capItem1.CreBy = 1;
                            capItem1.CreDate = DateTime.Now;
                            capItem1.UpdBy = 1;
                            capItem1.UpdDate = DateTime.Now;
                            capItem1.UpdPgm = "AdvancePosting";

                            accrItem1.DocId = DocID;
                            accrItem1.DocSeq = "M";
                            accrItem1.DocKind = DocKind;
                            accrItem1.ItemnoAcc = "1";
                            accrItem1.Currency = PostingConst.Currency;
                            accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString());

                            accrItem1.Active = true;
                            accrItem1.CreBy = 1;
                            accrItem1.CreDate = DateTime.Now;
                            accrItem1.UpdBy = 1;
                            accrItem1.UpdDate = DateTime.Now;
                            accrItem1.UpdPgm = "AdvancePosting";
                            #endregion Credit

                            #region Debit
                            capItem2.DocId = DocID;
                            capItem2.DocSeq = "M";
                            capItem2.DocKind = DocKind;
                            capItem2.ItemnoAcc = "2";
                            capItem2.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                            capItem2.Pmnttrms = PostingConst.Pmnttrms;
                            capItem2.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem2.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem2.PmntBlock = "";
                            capItem2.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                            capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem2.AllocNmbr);
                            capItem2.ItemText = dstPosting.Tables[0].Rows[0]["DueDate"].ToString() + "/" + dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem2.ItemText = SAPUIHelper.SubString50(capItem2.ItemText);
                            capItem2.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem2.TaxCode = TaxCodeConst.NV;
                            capItem2.SpGlInd = SpGlIndConst.D;

                            if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                capItem2.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                            }

                            capItem2.Pmtmthsupl = dstPosting.Tables[0].Rows[0]["Supplementary"].ToString();

                            capItem2.Active = true;
                            capItem2.CreBy = 1;
                            capItem2.CreDate = DateTime.Now;
                            capItem2.UpdBy = 1;
                            capItem2.UpdDate = DateTime.Now;
                            capItem2.UpdPgm = "AdvancePosting";

                            accrItem2.DocId = DocID;
                            accrItem2.DocSeq = "M";
                            accrItem2.DocKind = DocKind;
                            accrItem2.ItemnoAcc = "2";
                            accrItem2.Currency = PostingConst.Currency;
                            accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString());

                            accrItem2.Active = true;
                            accrItem2.CreBy = 1;
                            accrItem2.CreDate = DateTime.Now;
                            accrItem2.UpdBy = 1;
                            accrItem2.UpdDate = DateTime.Now;
                            accrItem2.UpdPgm = "AdvancePosting";
                            #endregion Debit

                            #endregion Transfer หรือ Cheque
                        }
                    }
                    else  //for rep office
                    {
                        #region Credit
                        capItem1.DocId = DocID;
                        capItem1.DocSeq = "M";
                        capItem1.DocKind = DocKind;
                        capItem1.ItemnoAcc = "1";
                        capItem1.VendorNo = dstPosting.Tables[0].Rows[0]["PBCode"].ToString();
                        capItem1.PmntBlock = PostingConst.PmntBlock;
                        capItem1.Pmnttrms = PostingConst.Pmnttrms;
                        capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                        capItem1.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString())).EmployeeName;
                        capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                        capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                        capItem1.TaxCode = TaxCodeConst.NV;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem1.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                        }

                        capItem1.Active = true;
                        capItem1.CreBy = 1;
                        capItem1.CreDate = DateTime.Now;
                        capItem1.UpdBy = 1;
                        capItem1.UpdDate = DateTime.Now;
                        capItem1.UpdPgm = "AdvancePosting";

                        accrItem1.DocId = DocID;
                        accrItem1.DocSeq = "M";
                        accrItem1.DocKind = DocKind;
                        accrItem1.ItemnoAcc = "1";

                        accrItem1.Currency = mainCurrencySymbol;
                        accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["MainCurrencyAmount"].ToString());
                        accrItem1.ExchRate = decimal.Parse(dstPosting.Tables[0].Rows[0]["ExchangeRateMainToTHB"].ToString());

                        accrItem1.Active = true;
                        accrItem1.CreBy = 1;
                        accrItem1.CreDate = DateTime.Now;
                        accrItem1.UpdBy = 1;
                        accrItem1.UpdDate = DateTime.Now;
                        accrItem1.UpdPgm = "AdvancePosting";
                        #endregion Credit

                        #region Debit
                        capItem2.DocId = DocID;
                        capItem2.DocSeq = "M";
                        capItem2.DocKind = DocKind;
                        capItem2.ItemnoAcc = "2";
                        capItem2.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                        capItem2.Pmnttrms = PostingConst.Pmnttrms;
                        capItem2.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                        capItem2.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                        capItem2.PmntBlock = PostingConst.PmntBlock;
                        capItem2.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                        capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem2.AllocNmbr);
                        capItem2.ItemText = dstPosting.Tables[0].Rows[0]["DueDate"].ToString() + "/" + dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem2.ItemText = SAPUIHelper.SubString50(capItem2.ItemText);
                        capItem2.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                        capItem2.TaxCode = TaxCodeConst.NV;
                        capItem2.SpGlInd = SpGlIndConst.D;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem2.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                        }

                        capItem2.Active = true;
                        capItem2.CreBy = 1;
                        capItem2.CreDate = DateTime.Now;
                        capItem2.UpdBy = 1;
                        capItem2.UpdDate = DateTime.Now;
                        capItem2.UpdPgm = "AdvancePosting";

                        accrItem2.DocId = DocID;
                        accrItem2.DocSeq = "M";
                        accrItem2.DocKind = DocKind;
                        accrItem2.ItemnoAcc = "2";

                        accrItem2.Currency = mainCurrencySymbol;
                        accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[0]["MainCurrencyAmount"].ToString());
                        accrItem2.ExchRate = decimal.Parse(dstPosting.Tables[0].Rows[0]["ExchangeRateMainToTHB"].ToString());

                        accrItem2.Active = true;
                        accrItem2.CreBy = 1;
                        accrItem2.CreDate = DateTime.Now;
                        accrItem2.UpdBy = 1;
                        accrItem2.UpdDate = DateTime.Now;
                        accrItem2.UpdPgm = "AdvancePosting";
                        #endregion Debit
                    }
                    BapiServiceProvider.Bapiacap09Service.Save(capItem1);
                    BapiServiceProvider.Bapiacap09Service.Save(capItem2);
                    BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                    BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                    #endregion Domestic
                }
                else if (dstPosting.Tables[0].Rows[0]["AdvanceType"].ToString() == ZoneTypeConst.Foreign)
                {
                    #region Foreign
                    if (!repOffice)
                    {
                        #region HEAD
                        Bapiache09 che09 = new Bapiache09();
                        che09.DocId = DocID;
                        che09.DocSeq = "M";
                        che09.DocKind = DocKind;
                        che09.BusAct = PostingConst.BusAct;
                        che09.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                        che09.CompCode = dstPosting.Tables[0].Rows[0]["COMP_CODE"].ToString().Substring(0, 4);
                        che09.DocDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                        che09.DocKind = DocKind;
                        che09.DocType = sap.DocTypeAdvancePostingFR;//DocTypeConst.KZ;
                        che09.PstngDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                        che09.ReverseDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                        che09.RefDocNo = SAPUIHelper.SubString(16, dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString());
                        che09.DocStatus = "N";
                        che09.DocAppFlag = "A";

                        che09.Active = true;
                        che09.CreBy = 1;
                        che09.CreDate = DateTime.Now;
                        che09.UpdBy = 1;
                        che09.UpdDate = DateTime.Now;
                        che09.UpdPgm = "AdvancePosting";
                        BapiServiceProvider.Bapiache09Service.Save(che09);
                        #endregion HEAD

                        #region Foolter
                        Bapiacextc cextc = new Bapiacextc();
                        cextc.DocId = DocID;
                        cextc.DocSeq = "M";
                        cextc.DocKind = DocKind;
                        cextc.Field1 = PostingConst.BRNCH;
                        cextc.Field2 = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();

                        cextc.Active = true;
                        cextc.CreBy = 1;
                        cextc.CreDate = DateTime.Now;
                        cextc.UpdBy = 1;
                        cextc.UpdDate = DateTime.Now;
                        cextc.UpdPgm = "AdvancePosting";
                        BapiServiceProvider.BapiacextcService.Save(cextc);

                        Bapiacextc cextc1 = new Bapiacextc();
                        cextc1.DocId = DocID;
                        cextc1.DocSeq = "M";
                        cextc1.DocKind = DocKind;
                        cextc1.Field1 = PostingConst.VAT;
                        cextc1.Field2 = TaxCodeConst.NV;

                        cextc1.Active = true;
                        cextc1.CreBy = 1;
                        cextc1.CreDate = DateTime.Now;
                        cextc1.UpdBy = 1;
                        cextc1.UpdDate = DateTime.Now;
                        cextc1.UpdPgm = "AdvancePosting";
                        BapiServiceProvider.BapiacextcService.Save(cextc1);
                        #endregion Foolter

                        Bapiacgl09 capItem1 = new Bapiacgl09();
                        Bapiacap09 capItem2 = new Bapiacap09();
                        Bapiaccr09 accrItem1 = new Bapiaccr09();
                        Bapiaccr09 accrItem2 = new Bapiaccr09();

                        #region Credit
                        capItem1.DocId = DocID;
                        capItem1.DocSeq = "M";
                        capItem1.DocKind = DocKind;
                        capItem1.ItemnoAcc = "1";
                        capItem1.GlAccount = dstPosting.Tables[0].Rows[0]["BankAccount"].ToString();
                        capItem1.ValueDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                        capItem1.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                        capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        //capItem1.BusArea        = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem1.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                        }

                        capItem1.Active = true;
                        capItem1.CreBy = 1;
                        capItem1.CreDate = DateTime.Now;
                        capItem1.UpdBy = 1;
                        capItem1.UpdDate = DateTime.Now;
                        capItem1.UpdPgm = "AdvancePosting";

                        accrItem1.DocId = DocID;
                        accrItem1.DocSeq = "M";
                        accrItem1.DocKind = DocKind;
                        accrItem1.ItemnoAcc = "1";

                        accrItem1.Currency = PostingConst.Currency;
                        accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString());

                        accrItem1.Active = true;
                        accrItem1.CreBy = 1;
                        accrItem1.CreDate = DateTime.Now;
                        accrItem1.UpdBy = 1;
                        accrItem1.UpdDate = DateTime.Now;
                        accrItem1.UpdPgm = "AdvancePosting";
                        #endregion Credit

                        #region Debit
                        capItem2.DocId = DocID;
                        capItem2.DocSeq = "M";
                        capItem2.DocKind = DocKind;
                        capItem2.ItemnoAcc = "2";
                        capItem2.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                        capItem2.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                        capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem2.AllocNmbr);
                        capItem2.ItemText = dstPosting.Tables[0].Rows[0]["DueDate"].ToString() + "/" + dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem2.ItemText = SAPUIHelper.SubString50(capItem2.ItemText);
                        capItem2.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                        capItem2.TaxCode = TaxCodeConst.NV;
                        capItem2.SpGlInd = SpGlIndConst.D;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem2.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                        }

                        capItem2.Active = true;
                        capItem2.CreBy = 1;
                        capItem2.CreDate = DateTime.Now;
                        capItem2.UpdBy = 1;
                        capItem2.UpdDate = DateTime.Now;
                        capItem2.UpdPgm = "AdvancePosting";

                        accrItem2.DocId = DocID;
                        accrItem2.DocSeq = "M";
                        accrItem2.DocKind = DocKind;
                        accrItem2.ItemnoAcc = "2";

                        accrItem2.Currency = PostingConst.Currency;
                        accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString());

                        accrItem2.Active = true;
                        accrItem2.CreBy = 1;
                        accrItem2.CreDate = DateTime.Now;
                        accrItem2.UpdBy = 1;
                        accrItem2.UpdDate = DateTime.Now;
                        accrItem2.UpdPgm = "AdvancePosting";
                        #endregion Debit

                        BapiServiceProvider.Bapiacgl09Service.Save(capItem1);
                        BapiServiceProvider.Bapiacap09Service.Save(capItem2);
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                    }
                    else  // post ADF for rep office
                    {
                        #region HEAD
                        Bapiache09 che09 = new Bapiache09();
                        che09.DocId = DocID;
                        che09.DocSeq = "M";
                        che09.DocKind = DocKind;
                        che09.BusAct = PostingConst.BusAct;
                        che09.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                        che09.CompCode = dstPosting.Tables[0].Rows[0]["COMP_CODE"].ToString().Substring(0, 4);
                        che09.DocDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                        che09.DocKind = DocKind;
                        che09.DocType = sap.DocTypeAdvancePostingFR;//DocTypeConst.KR;
                        che09.PstngDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                        che09.ReverseDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                        che09.RefDocNo = SAPUIHelper.SubString(16, dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString());
                        che09.DocStatus = "N";
                        che09.DocAppFlag = "A";

                        che09.Active = true;
                        che09.CreBy = 1;
                        che09.CreDate = DateTime.Now;
                        che09.UpdBy = 1;
                        che09.UpdDate = DateTime.Now;
                        che09.UpdPgm = "AdvancePosting";
                        BapiServiceProvider.Bapiache09Service.Save(che09);
                        #endregion HEAD

                        #region Foolter
                        Bapiacextc cextc = new Bapiacextc();
                        cextc.DocId = DocID;
                        cextc.DocSeq = "M";
                        cextc.DocKind = DocKind;
                        cextc.Field1 = PostingConst.BRNCH;
                        cextc.Field2 = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();

                        cextc.Active = true;
                        cextc.CreBy = 1;
                        cextc.CreDate = DateTime.Now;
                        cextc.UpdBy = 1;
                        cextc.UpdDate = DateTime.Now;
                        cextc.UpdPgm = "AdvancePosting";
                        BapiServiceProvider.BapiacextcService.Save(cextc);

                        Bapiacextc cextc1 = new Bapiacextc();
                        cextc1.DocId = DocID;
                        cextc1.DocSeq = "M";
                        cextc1.DocKind = DocKind;
                        cextc1.Field1 = PostingConst.VAT;
                        cextc1.Field2 = TaxCodeConst.NV;

                        cextc1.Active = true;
                        cextc1.CreBy = 1;
                        cextc1.CreDate = DateTime.Now;
                        cextc1.UpdBy = 1;
                        cextc1.UpdDate = DateTime.Now;
                        cextc1.UpdPgm = "AdvancePosting";
                        BapiServiceProvider.BapiacextcService.Save(cextc1);
                        #endregion Foolter

                        Bapiacap09 capItem1 = new Bapiacap09();
                        Bapiacap09 capItem2 = new Bapiacap09();
                        Bapiaccr09 accrItem1 = new Bapiaccr09();
                        Bapiaccr09 accrItem2 = new Bapiaccr09();

                        #region Credit
                        capItem1.DocId = DocID;
                        capItem1.DocSeq = "M";
                        capItem1.DocKind = DocKind;
                        capItem1.ItemnoAcc = "1";
                        capItem1.VendorNo = dstPosting.Tables[0].Rows[0]["PBCode"].ToString();
                        capItem1.PmntBlock = PostingConst.PmntBlock;
                        capItem1.Pmnttrms = PostingConst.Pmnttrms;
                        capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                        capItem1.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString())).EmployeeName;
                        capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                        capItem1.TaxCode = TaxCodeConst.NV;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem1.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                        }

                        capItem1.Active = true;
                        capItem1.CreBy = 1;
                        capItem1.CreDate = DateTime.Now;
                        capItem1.UpdBy = 1;
                        capItem1.UpdDate = DateTime.Now;
                        capItem1.UpdPgm = "AdvancePosting";

                        accrItem1.DocId = DocID;
                        accrItem1.DocSeq = "M";
                        accrItem1.DocKind = DocKind;
                        accrItem1.ItemnoAcc = "1";

                        accrItem1.Currency = mainCurrencySymbol;
                        accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["MainCurrencyAmount"].ToString());
                        accrItem1.ExchRate = decimal.Parse(dstPosting.Tables[0].Rows[0]["ExchangeRateMainToTHB"].ToString());

                        accrItem1.Active = true;
                        accrItem1.CreBy = 1;
                        accrItem1.CreDate = DateTime.Now;
                        accrItem1.UpdBy = 1;
                        accrItem1.UpdDate = DateTime.Now;
                        accrItem1.UpdPgm = "AdvancePosting";
                        #endregion Credit

                        #region Debit
                        capItem2.DocId = DocID;
                        capItem2.DocSeq = "M";
                        capItem2.DocKind = DocKind;
                        capItem2.ItemnoAcc = "2";
                        capItem2.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                        capItem2.Pmnttrms = PostingConst.Pmnttrms;
                        capItem2.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                        capItem2.PmntBlock = PostingConst.PmntBlock;
                        capItem2.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                        capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem2.AllocNmbr);
                        capItem2.ItemText = dstPosting.Tables[0].Rows[0]["DueDate"].ToString() + "/" + dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem2.ItemText = SAPUIHelper.SubString50(capItem2.ItemText);
                        capItem2.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                        capItem2.TaxCode = TaxCodeConst.NV;
                        capItem2.SpGlInd = SpGlIndConst.D;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem2.BusArea = dstPosting.Tables[0].Rows[0]["BusinessArea"].ToString();
                        }

                        capItem2.Active = true;
                        capItem2.CreBy = 1;
                        capItem2.CreDate = DateTime.Now;
                        capItem2.UpdBy = 1;
                        capItem2.UpdDate = DateTime.Now;
                        capItem2.UpdPgm = "AdvancePosting";

                        accrItem2.DocId = DocID;
                        accrItem2.DocSeq = "M";
                        accrItem2.DocKind = DocKind;
                        accrItem2.ItemnoAcc = "2";

                        accrItem2.Currency = mainCurrencySymbol;
                        accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[0]["MainCurrencyAmount"].ToString());
                        accrItem2.ExchRate = decimal.Parse(dstPosting.Tables[0].Rows[0]["ExchangeRateMainToTHB"].ToString());

                        accrItem2.Active = true;
                        accrItem2.CreBy = 1;
                        accrItem2.CreDate = DateTime.Now;
                        accrItem2.UpdBy = 1;
                        accrItem2.UpdDate = DateTime.Now;
                        accrItem2.UpdPgm = "AdvancePosting";
                        #endregion Debit

                        BapiServiceProvider.Bapiacap09Service.Save(capItem1);
                        BapiServiceProvider.Bapiacap09Service.Save(capItem2);
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                    }
                    #endregion Foreign
                }
            }
        }
        #endregion public override void CreatePostData(string DocumentID)
    }
}
