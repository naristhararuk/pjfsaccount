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
using SCG.eAccounting.Query;

namespace SCG.eAccounting.SAP.BAPI.Service
{
    public class FixedAdvancePostingService : PostingService
    {
        #region public override void CreatePostData(long DocID)
        public override void CreatePostData(long DocID, string DocKind)
        {
            Hashtable paramete = new Hashtable();
            paramete.Add("@DOCUMENT_ID", DocID.ToString());
            DataSet dstPosting = new DBManage().ExecuteQuery("FIXEDADVANCE_POSTING", paramete);
            SCGDocument doc = SCG.eAccounting.Query.ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(DocID);
            DbSapInstance sap = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.GetSAPDocTypeForPosting(doc.CompanyID.CompanyCode);
            if (dstPosting.Tables[0].Rows.Count > 0)
            {

                #region Domestic

                if (dstPosting.Tables[0].Rows[0]["FixedAdvanceType"].ToString() == "1")
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

                    /*N-edit*/
                    //che09.DocType = sap.DocTypeAdvancePostingDM;//DocTypeConst.KR;
                    che09.DocType = sap.DocTypeFixedAdvancePosting;//DocTypeConst.KR;

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
                    che09.UpdPgm = "FixedAdvancePosting";
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
                    cextc.UpdPgm = "FixedAdvancePosting";
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
                    cextc1.UpdPgm = "FixedAdvancePosting";
                    BapiServiceProvider.BapiacextcService.Save(cextc1);
                    #endregion Foolter

                    #region Transfer หรือ Cheque

                    #region Credit
                    Bapiacap09 capItem1 = new Bapiacap09();
                    capItem1.DocId = DocID;
                    capItem1.DocSeq = "M";
                    capItem1.DocKind = DocKind;
                    capItem1.ItemnoAcc = "1";
                    capItem1.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                    capItem1.PmntBlock = PostingConst.PmntBlock;
                    capItem1.Pmnttrms = PostingConst.Pmnttrms;
                    capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                    //capItem1.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString())).EmployeeName;
                    //capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                    capItem1.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                    capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                    capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                    capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                    capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                    capItem1.TaxCode = TaxCodeConst.NV;
                    capItem1.Active = true;
                    capItem1.CreBy = 1;
                    capItem1.CreDate = DateTime.Now;
                    capItem1.UpdBy = 1;
                    capItem1.UpdDate = DateTime.Now;
                    capItem1.UpdPgm = "FixedAdvancePosting";

                    capItem1.Pmtmthsupl = doc.Supplementary; //dstPosting.Tables[0].Rows[0]["Supplementary"].ToString();
                    BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                    Bapiaccr09 accrItem1 = new Bapiaccr09();
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
                    accrItem1.UpdPgm = "FixedAdvancePosting";
                    BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                    #endregion Credit

                    #region Debit
                    Bapiacap09 capItem2 = new Bapiacap09();
                    capItem2.DocId = DocID;
                    capItem2.DocSeq = "M";
                    capItem2.DocKind = DocKind;
                    capItem2.ItemnoAcc = "2";
                    //capItem2.GlAccount = SAPUIHelper.GetFixedPostingAccountDr(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString()));
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
                    capItem2.SpGlInd = SpGlIndConst.H;
                    capItem2.Pmtmthsupl = doc.Supplementary; //dstPosting.Tables[0].Rows[0]["Supplementary"].ToString();

                    capItem2.Active = true;
                    capItem2.CreBy = 1;
                    capItem2.CreDate = DateTime.Now;
                    capItem2.UpdBy = 1;
                    capItem2.UpdDate = DateTime.Now;
                    capItem2.UpdPgm = "FixedAdvancePosting";
                    BapiServiceProvider.Bapiacap09Service.Save(capItem2);

                    Bapiaccr09 accrItem2 = new Bapiaccr09();
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
                    accrItem2.UpdPgm = "FixedAdvancePosting";
                    BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                    #endregion Debit

                    #endregion Transfer หรือ Cheque
                }
                else
                {
                    if (decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString()) > 0)
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

                        /*N-edit*/
                        //che09.DocType = sap.DocTypeAdvancePostingDM;//DocTypeConst.KR;
                        che09.DocType = sap.DocTypeFixedAdvancePosting;//DocTypeConst.KR;

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
                        che09.UpdPgm = "FixedAdvancePosting";
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
                        cextc.UpdPgm = "FixedAdvancePosting";
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
                        cextc1.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.BapiacextcService.Save(cextc1);
                        #endregion Foolter

                        if (dstPosting.Tables[0].Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                        {
                            #region Cash

                            #region Credit
                            Bapiacap09 capItem1 = new Bapiacap09();
                            capItem1.DocId = DocID;
                            capItem1.DocSeq = "M";
                            capItem1.DocKind = DocKind;
                            capItem1.ItemnoAcc = "1";
                            capItem1.VendorNo = dstPosting.Tables[0].Rows[0]["PBCode"].ToString();
                            capItem1.PmntBlock = PostingConst.PmntBlock;
                            capItem1.Pmnttrms = PostingConst.Pmnttrms;
                            capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem1.AllocNmbr = SAPUIHelper.GetFixedPostingAccountAdjustCr(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString()));
                            capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                            capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                            capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem1.TaxCode = TaxCodeConst.NV;
                            capItem1.Pmtmthsupl = doc.Supplementary; //dstPosting.Tables[0].Rows[0]["Supplementary"].ToString();

                            capItem1.Active = true;
                            capItem1.CreBy = 1;
                            capItem1.CreDate = DateTime.Now;
                            capItem1.UpdBy = 1;
                            capItem1.UpdDate = DateTime.Now;
                            capItem1.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                            Bapiaccr09 accrItem1 = new Bapiaccr09();
                            accrItem1.DocId = DocID;
                            accrItem1.DocSeq = "M";
                            accrItem1.DocKind = DocKind;
                            accrItem1.ItemnoAcc = "1";

                            accrItem1.Currency = PostingConst.Currency;
                            accrItem1.AmtDoccur = 0 - (decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString()) - decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString()));

                            accrItem1.Active = true;
                            accrItem1.CreBy = 1;
                            accrItem1.CreDate = DateTime.Now;
                            accrItem1.UpdBy = 1;
                            accrItem1.UpdDate = DateTime.Now;
                            accrItem1.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                            #endregion Credit

                            #region Debit
                            Bapiacap09 capItem2 = new Bapiacap09();
                            capItem2.DocId = DocID;
                            capItem2.DocSeq = "M";
                            capItem2.DocKind = DocKind;
                            capItem2.ItemnoAcc = "2";
                            //capItem2.GlAccount = SAPUIHelper.GetFixedPostingAccountAdjustDr(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString()));
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
                            capItem2.SpGlInd = SpGlIndConst.H;
                            capItem2.Active = true;
                            capItem2.CreBy = 1;
                            capItem2.CreDate = DateTime.Now;
                            capItem2.UpdBy = 1;
                            capItem2.UpdDate = DateTime.Now;
                            capItem2.UpdPgm = "FixedAdvancePosting";
                            capItem2.Pmtmthsupl = doc.Supplementary; //dstPosting.Tables[0].Rows[0]["Supplementary"].ToString();

                            BapiServiceProvider.Bapiacap09Service.Save(capItem2);

                            Bapiaccr09 accrItem2 = new Bapiaccr09();
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
                            accrItem2.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                            #endregion Debit

                            #region CreditDifferentAmount
                            Bapiacap09 capItem3 = new Bapiacap09();
                            capItem3.DocId = DocID;
                            capItem3.DocSeq = "M";
                            capItem3.DocKind = DocKind;
                            capItem3.ItemnoAcc = "3";
                            capItem3.VendorNo = dstPosting.Tables[0].Rows[0]["PBCode"].ToString();
                            capItem3.PmntBlock = PostingConst.PmntBlock;
                            capItem3.Pmnttrms = PostingConst.Pmnttrms;
                            capItem3.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem3.AllocNmbr = SAPUIHelper.GetFixedPostingAccountCr(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString()));
                            capItem3.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                            capItem3.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem3.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem3.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                            capItem3.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem3.TaxCode = TaxCodeConst.NV;

                            capItem3.Active = true;
                            capItem3.CreBy = 1;
                            capItem3.CreDate = DateTime.Now;
                            capItem3.UpdBy = 1;
                            capItem3.UpdDate = DateTime.Now;
                            capItem3.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiacap09Service.Save(capItem3);

                            Bapiaccr09 accrItem3 = new Bapiaccr09();
                            accrItem3.DocId = DocID;
                            accrItem3.DocSeq = "M";
                            accrItem3.DocKind = DocKind;
                            accrItem3.ItemnoAcc = "3";

                            accrItem3.Currency = PostingConst.Currency;
                            accrItem3.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString());

                            accrItem3.Active = true;
                            accrItem3.CreBy = 1;
                            accrItem3.CreDate = DateTime.Now;
                            accrItem3.UpdBy = 1;
                            accrItem3.UpdDate = DateTime.Now;
                            accrItem3.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiaccr09Service.Save(accrItem3);
                            #endregion CreditDifferrent

                            #endregion Cash
                        }
                        else
                        {
                            #region Transfer หรือ Cheque

                            #region Credit
                            Bapiacap09 capItem1 = new Bapiacap09();
                            capItem1.DocId = DocID;
                            capItem1.DocSeq = "M";
                            capItem1.DocKind = DocKind;
                            capItem1.ItemnoAcc = "1";
                            capItem1.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                            capItem1.PmntBlock = "";
                            capItem1.Pmnttrms = PostingConst.Pmnttrms;
                            capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            //FixedAdvanceDocument favDb = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByDocumentID(DocID);
                            //FixedAdvanceDocument fixAd = ScgeAccountingQueryProvider.FixedAdvanceDocumentQuery.GetFixedAdvanceByFixedAdvanceID((long)favDb.RefFixedAdvanceID);
                            //SCGDocument scgDoc = ScgeAccountingQueryProvider.SCGDocumentQuery.GetSCGDocumentByDocumentID(fixAd.DocumentID.DocumentID);
                            //capItem1.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                            //capItem1.AllocNmbr = scgDoc.DocumentNo;
                            //capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                            
                            capItem1.AllocNmbr = dstPosting.Tables[0].Rows[0]["RefFixedAdvanceID"].ToString();
                            capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                            capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem1.TaxCode = TaxCodeConst.NV;
                            capItem1.SpGlInd = SpGlIndConst.H;

                            capItem1.Active = true;
                            capItem1.CreBy = 1;
                            capItem1.CreDate = DateTime.Now;
                            capItem1.UpdBy = 1;
                            capItem1.UpdDate = DateTime.Now;
                            capItem1.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                            Bapiaccr09 accrItem1 = new Bapiaccr09();
                            accrItem1.DocId = DocID;
                            accrItem1.DocSeq = "M";
                            accrItem1.DocKind = DocKind;
                            accrItem1.ItemnoAcc = "1";
                            accrItem1.Currency = PostingConst.Currency;
                            accrItem1.AmtDoccur = 0 - (decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString()) - decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString()));

                            accrItem1.Active = true;
                            accrItem1.CreBy = 1;
                            accrItem1.CreDate = DateTime.Now;
                            accrItem1.UpdBy = 1;
                            accrItem1.UpdDate = DateTime.Now;
                            accrItem1.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                            #endregion Credit

                            #region Debit
                            Bapiacap09 capItem2 = new Bapiacap09();
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
                            //capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem2.AllocNmbr);
                            capItem2.ItemText = dstPosting.Tables[0].Rows[0]["DueDate"].ToString() + "/" + dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem2.ItemText = SAPUIHelper.SubString50(capItem2.ItemText);
                            capItem2.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem2.TaxCode = TaxCodeConst.NV;
                            capItem2.SpGlInd = SpGlIndConst.H;

                            capItem2.Active = true;
                            capItem2.CreBy = 1;
                            capItem2.CreDate = DateTime.Now;
                            capItem2.UpdBy = 1;
                            capItem2.UpdDate = DateTime.Now;
                            capItem2.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiacap09Service.Save(capItem2);

                            Bapiaccr09 accrItem2 = new Bapiaccr09();
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
                            accrItem2.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                            #endregion Debit

                            #region CreditDifferrent
                            Bapiacap09 capItem3 = new Bapiacap09();
                            capItem3.DocId = DocID;
                            capItem3.DocSeq = "M";
                            capItem3.DocKind = DocKind;
                            capItem3.ItemnoAcc = "3";
                            capItem3.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                            capItem3.PmntBlock = "";
                            capItem3.Pmnttrms = PostingConst.Pmnttrms;
                            capItem3.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                            capItem3.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                            capItem3.AllocNmbr = SAPUIHelper.SubString18(capItem3.AllocNmbr);
                            capItem3.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                            capItem3.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                            capItem3.ItemText = SAPUIHelper.SubString50(capItem3.ItemText);
                            capItem3.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                            capItem3.TaxCode = TaxCodeConst.NV;
                            //capItem3.SpGlInd = SpGlIndConst.H;

                            capItem3.Active = true;
                            capItem3.CreBy = 1;
                            capItem3.CreDate = DateTime.Now;
                            capItem3.UpdBy = 1;
                            capItem3.UpdDate = DateTime.Now;
                            capItem3.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiacap09Service.Save(capItem3);

                            Bapiaccr09 accrItem3 = new Bapiaccr09();
                            accrItem3.DocId = DocID;
                            accrItem3.DocSeq = "M";
                            accrItem3.DocKind = DocKind;
                            accrItem3.ItemnoAcc = "3";
                            accrItem3.Currency = PostingConst.Currency;
                            accrItem3.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString());

                            accrItem3.Active = true;
                            accrItem3.CreBy = 1;
                            accrItem3.CreDate = DateTime.Now;
                            accrItem3.UpdBy = 1;
                            accrItem3.UpdDate = DateTime.Now;
                            accrItem3.UpdPgm = "FixedAdvancePosting";
                            BapiServiceProvider.Bapiaccr09Service.Save(accrItem3);
                            #endregion CreditDifferrent

                            #endregion Transfer หรือ Cheque
                        }
                    }
                    else
                    {
                        /*PayBack Company*/
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

                        /*N-edit*/
                        //che09.DocType = sap.DocTypeAdvancePostingDM;//DocTypeConst.KR;
                        che09.DocType = sap.DocTypeFixedAdvanceReturnPosting;//DocTypeConst.KR;

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
                        che09.UpdPgm = "FixedAdvancePosting";
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
                        cextc.UpdPgm = "FixedAdvancePosting";
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
                        cextc1.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.BapiacextcService.Save(cextc1);
                        #endregion Foolter

                        #region Cash Or Tranfer

                        #region Credit
                        Bapiacap09 capItem1 = new Bapiacap09();
                        capItem1.DocId = DocID;
                        capItem1.DocSeq = "M";
                        capItem1.DocKind = DocKind;
                        capItem1.ItemnoAcc = "1";
                        capItem1.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString())).VendorCode;
                        capItem1.PmntBlock = PostingConst.PmntBlock;
                        capItem1.Pmnttrms = PostingConst.Pmnttrms;
                        capItem1.BlineDate = dstPosting.Tables[0].Rows[0]["BaseLineDate"].ToString();
                        //capItem1.AllocNmbr = SAPUIHelper.GetFixedPostingAccountAdjustCr(long.Parse(dstPosting.Tables[0].Rows[0]["ReceiverID"].ToString()));
                        //capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        /*N-Edited Ref FixedAdvanceID*/
                        capItem1.AllocNmbr = dstPosting.Tables[0].Rows[0]["RefFixedAdvanceID"].ToString();
                        
                        capItem1.PymtMeth = dstPosting.Tables[0].Rows[0]["PaymentMethod"].ToString();
                        capItem1.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        capItem1.Businessplace = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();
                        capItem1.TaxCode = TaxCodeConst.NV;
                        capItem1.SpGlInd = SpGlIndConst.H;

                        capItem1.Active = true;
                        capItem1.CreBy = 1;
                        capItem1.CreDate = DateTime.Now;
                        capItem1.UpdBy = 1;
                        capItem1.UpdDate = DateTime.Now;
                        capItem1.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                        Bapiaccr09 accrItem1 = new Bapiaccr09();
                        accrItem1.DocId = DocID;
                        accrItem1.DocSeq = "M";
                        accrItem1.DocKind = DocKind;
                        accrItem1.ItemnoAcc = "1";

                        accrItem1.Currency = PostingConst.Currency;
                        accrItem1.AmtDoccur = 0 - (decimal.Parse(dstPosting.Tables[0].Rows[0]["Amount"].ToString()) + (-1 * decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString())));

                        accrItem1.Active = true;
                        accrItem1.CreBy = 1;
                        accrItem1.CreDate = DateTime.Now;
                        accrItem1.UpdBy = 1;
                        accrItem1.UpdDate = DateTime.Now;
                        accrItem1.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                        #endregion Credit

                        #region Debit
                        Bapiacap09 capItem2 = new Bapiacap09();
                        capItem2.DocId = DocID;
                        capItem2.DocSeq = "M";
                        capItem2.DocKind = DocKind;
                        capItem2.ItemnoAcc = "2";
                        //capItem2.GlAccount = SAPUIHelper.GetFixedPostingAccountAdjustDr(long.Parse(dstPosting.Tables[0].Rows[0]["RequesterID"].ToString()));
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
                        capItem2.SpGlInd = SpGlIndConst.H;
                        capItem2.Active = true;
                        capItem2.CreBy = 1;
                        capItem2.CreDate = DateTime.Now;
                        capItem2.UpdBy = 1;
                        capItem2.UpdDate = DateTime.Now;
                        capItem2.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.Bapiacap09Service.Save(capItem2);

                        Bapiaccr09 accrItem2 = new Bapiaccr09();
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
                        accrItem2.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                        #endregion Debit

                        #region DebitDifferentAmount
                        Bapiacgl09 cglItem2 = new Bapiacgl09();
                        cglItem2.DocId = DocID;
                        cglItem2.DocSeq = "M";
                        cglItem2.DocKind = DocKind;
                        cglItem2.ItemnoAcc = "3";
                        cglItem2.GlAccount = dstPosting.Tables[0].Rows[0]["BankAccount"].ToString();
                        cglItem2.AllocNmbr = dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString();
                        cglItem2.AllocNmbr = SAPUIHelper.SubString18(cglItem2.AllocNmbr);
                        cglItem2.ItemText = dstPosting.Tables[0].Rows[0]["Description"].ToString();
                        cglItem2.ItemText = SAPUIHelper.SubString50(cglItem2.ItemText);

                        cglItem2.Active = true;
                        cglItem2.CreBy = 1;
                        cglItem2.CreDate = DateTime.Now;
                        cglItem2.UpdBy = 1;
                        cglItem2.UpdDate = DateTime.Now;
                        cglItem2.UpdPgm = "FixedAdvancePosting";

                        /*N-edited valuedate = requestdate*/
                        cglItem2.ValueDate = dstPosting.Tables[0].Rows[0]["RequestDate"].ToString(); /*DateFormat = YYYYMMDD*/
                        BapiServiceProvider.Bapiacgl09Service.Save(cglItem2);

                        Bapiaccr09 accrItem3 = new Bapiaccr09();
                        accrItem3.DocId = DocID;
                        accrItem3.DocSeq = "M";
                        accrItem3.DocKind = DocKind;
                        accrItem3.ItemnoAcc = "3";

                        accrItem3.Currency = PostingConst.Currency;
                        accrItem3.AmtDoccur = (-1 * decimal.Parse(dstPosting.Tables[0].Rows[0]["NetAmount"].ToString()));

                        accrItem3.Active = true;
                        accrItem3.CreBy = 1;
                        accrItem3.CreDate = DateTime.Now;
                        accrItem3.UpdBy = 1;
                        accrItem3.UpdDate = DateTime.Now;
                        accrItem3.UpdPgm = "FixedAdvancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem3);
                        #endregion DebitDifferentAmount

                        #endregion Cash Or Tranfer
                    }
                }
                #endregion Domestic
            }
        }
        #endregion public override void CreatePostData(string DocumentID)
    }
}
