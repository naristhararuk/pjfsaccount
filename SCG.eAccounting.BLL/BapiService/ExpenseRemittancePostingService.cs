using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using System.Collections;
using System.Data;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Service;
using SCG.DB.Query;
using SCG.DB.DTO;
using SS.DB.DTO;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.SAP.BAPI.Service
{
    public class ExpenseRemittancePostingService : PostingService
    {
        public override void CreatePostData(long DocID, string DocKind)
        {
            Hashtable paramete = new Hashtable();
            paramete.Add("@DOCUMENT_ID", DocID.ToString());

            DataSet dstPosting = new DBManage().ExecuteQuery("EXPENSE_REMITTANCE_POSTING", paramete);
            bool repOffice = false;
            string mainCurrencySymbol = string.Empty;

            SCGDocument doc = SCG.eAccounting.Query.ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(DocID);

            DbSapInstance sap = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.GetSAPDocTypeForPosting(doc.CompanyID.CompanyCode);


            if (dstPosting.Tables[0].Rows.Count >= 1)
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

                #region HEAD
                Bapiache09 che09 = new Bapiache09();
                che09.DocId     = DocID;
                che09.DocSeq    = "M";
                che09.DocKind   = DocKind;
                che09.DocStatus = "N";

                che09.BusAct    = PostingConst.BusAct;
                che09.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                che09.CompCode  = dstPosting.Tables[0].Rows[0]["CompanyCode"].ToString();
                che09.DocType = doc.DocumentType.DocumentTypeID == SCG.eAccounting.BLL.DocumentTypeID.ExpenseDomesticDocument ? sap.DocTypeExpRmtPostingDM : sap.DocTypeExpRmtPostingFR;//DocTypeConst.SV;
                if (dstPosting.Tables[0].Rows[0]["ReceivedMethod"].ToString().Equals("Bank"))
                {
                    che09.DocDate = dstPosting.Tables[0].Rows[0]["PayInValueDate"].ToString();
                    che09.PstngDate = dstPosting.Tables[0].Rows[0]["PayInValueDate"].ToString();
                    che09.ReverseDate = dstPosting.Tables[0].Rows[0]["PayInValueDate"].ToString();
                }
                else
                {
                    che09.DocDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                    che09.PstngDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                    che09.ReverseDate = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                }
                che09.RefDocNo  = SAPUIHelper.SubString(16,dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString());

                che09.Active    = true;
                che09.CreBy     = 1;
                che09.CreDate   = DateTime.Now;
                che09.UpdBy     = 1;
                che09.UpdDate   = DateTime.Now;
                che09.UpdPgm    = "ExpenseRemetancePosting";
                BapiServiceProvider.Bapiache09Service.Save(che09);
                #endregion HEAD

                #region Foolter
                Bapiacextc cextc = new Bapiacextc();
                cextc.DocId     = DocID;
                cextc.DocSeq    = "M";
                cextc.DocKind   = DocKind;
                cextc.Field1    = PostingConst.BRNCH;
                cextc.Field2    = dstPosting.Tables[0].Rows[0]["BranchCode"].ToString();

                cextc.Active    = true;
                cextc.CreBy     = 1;
                cextc.CreDate   = DateTime.Now;
                cextc.UpdBy     = 1;
                cextc.UpdDate   = DateTime.Now;
                cextc.UpdPgm    = "ExpenseRemetancePosting";
                BapiServiceProvider.BapiacextcService.Save(cextc);

                Bapiacextc cextc1 = new Bapiacextc();
                cextc1.DocId    = DocID;
                cextc1.DocSeq   = "M";
                cextc1.DocKind  = DocKind;
                cextc1.Field1   = PostingConst.VAT;
                cextc1.Field2   = TaxCodeConst.NV;

                cextc1.Active   = true;
                cextc1.CreBy    = 1;
                cextc1.CreDate  = DateTime.Now;
                cextc1.UpdBy    = 1;
                cextc1.UpdDate  = DateTime.Now;
                cextc1.UpdPgm   = "ExpenseRemetancePosting";
                BapiServiceProvider.BapiacextcService.Save(cextc1);
                #endregion Foolter

                #region คู่บัญชี
                if (!repOffice)
                {
                    int intSeq = 0;
                    for (int i = 0; i < dstPosting.Tables[0].Rows.Count; i++)
                    {
                        #region Credit
                        intSeq++;
                        Bapiacap09 capItem1 = new Bapiacap09();
                        capItem1.DocId = DocID;
                        capItem1.DocSeq = "M";
                        capItem1.DocKind = DocKind;
                        capItem1.ItemnoAcc = intSeq.ToString();
                        capItem1.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[i]["RequesterID"].ToString())).VendorCode;
                        capItem1.AllocNmbr = dstPosting.Tables[0].Rows[i]["AdvanceNo"].ToString();
                        capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        capItem1.ItemText = dstPosting.Tables[0].Rows[i]["Description"].ToString();
                        capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        capItem1.Businessplace = dstPosting.Tables[0].Rows[i]["BranchCode"].ToString();
                        capItem1.TaxCode = TaxCodeConst.NV;
                        capItem1.SpGlInd = SpGlIndConst.D;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem1.BusArea = dstPosting.Tables[0].Rows[i]["BusinessArea"].ToString();
                        }

                        capItem1.Active = true;
                        capItem1.CreBy = 1;
                        capItem1.CreDate = DateTime.Now;
                        capItem1.UpdBy = 1;
                        capItem1.UpdDate = DateTime.Now;
                        capItem1.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                        Bapiaccr09 accrItem1 = new Bapiaccr09();
                        accrItem1.DocId = DocID;
                        accrItem1.DocSeq = "M";
                        accrItem1.DocKind = DocKind;
                        accrItem1.ItemnoAcc = intSeq.ToString();
                        accrItem1.Currency = PostingConst.Currency;
                        accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[i]["Amount"].ToString());
                        accrItem1.Active = true;
                        accrItem1.CreBy = 1;
                        accrItem1.CreDate = DateTime.Now;
                        accrItem1.UpdBy = 1;
                        accrItem1.UpdDate = DateTime.Now;
                        accrItem1.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);
                        #endregion Credit

                        #region Debit
                        intSeq++;

                        Bapiacgl09 cglItem2 = new Bapiacgl09();
                        cglItem2.DocId = DocID;
                        cglItem2.DocSeq = "M";
                        cglItem2.DocKind = DocKind;
                        cglItem2.ItemnoAcc = intSeq.ToString();
                        if (dstPosting.Tables[0].Rows[i]["ReceivedMethod"].ToString().Equals("Bank"))
                        {
                            string glAccount = dstPosting.Tables[0].Rows[i]["PayInGLAccount"].ToString();
                            if (glAccount.Length < 10)
                            {
                                int length = glAccount.Length;
                                for (int z = 10; z > length; z--)
                                {
                                    glAccount = "0" + glAccount;
                                }
                            }
                            cglItem2.GlAccount = glAccount;
                        }
                        else
                        {
                            cglItem2.GlAccount = PostingConst.GLAccount;
                        }
                        cglItem2.AllocNmbr = dstPosting.Tables[0].Rows[i]["AdvanceNo"].ToString();
                        cglItem2.AllocNmbr = SAPUIHelper.SubString18(cglItem2.AllocNmbr);
                        cglItem2.ItemText = dstPosting.Tables[0].Rows[i]["Description"].ToString();
                        cglItem2.ItemText = SAPUIHelper.SubString50(cglItem2.ItemText);

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            cglItem2.BusArea = dstPosting.Tables[0].Rows[i]["BusinessArea"].ToString();
                        }

                        cglItem2.Active = true;
                        cglItem2.CreBy = 1;
                        cglItem2.CreDate = DateTime.Now;
                        cglItem2.UpdBy = 1;
                        cglItem2.UpdDate = DateTime.Now;
                        cglItem2.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiacgl09Service.Save(cglItem2);

                        Bapiaccr09 accrItem2 = new Bapiaccr09();
                        accrItem2.DocId = DocID;
                        accrItem2.DocSeq = "M";
                        accrItem2.DocKind = DocKind;
                        accrItem2.ItemnoAcc = intSeq.ToString();
                        accrItem2.Currency = PostingConst.Currency;
                        accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[i]["Amount"].ToString());
                        accrItem2.Active = true;
                        accrItem2.CreBy = 1;
                        accrItem2.CreDate = DateTime.Now;
                        accrItem2.UpdBy = 1;
                        accrItem2.UpdDate = DateTime.Now;
                        accrItem2.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                        #endregion Debit
                    }
                }
                else  // for rep office
                {
                    int intSeq = 0;
                    for (int i = 0; i < dstPosting.Tables[0].Rows.Count; i++)
                    {
                        #region Credit
                        intSeq++;
                        Bapiacap09 capItem1 = new Bapiacap09();
                        capItem1.DocId = DocID;
                        capItem1.DocSeq = "M";
                        capItem1.DocKind = DocKind;
                        capItem1.ItemnoAcc = intSeq.ToString();
                        capItem1.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[i]["RequesterID"].ToString())).VendorCode;
                        capItem1.Pmnttrms = PostingConst.Pmnttrms;
                        capItem1.BlineDate = dstPosting.Tables[0].Rows[i]["BaseLineDate"].ToString();
                        capItem1.PmntBlock = PostingConst.PmntBlock;
                        capItem1.AllocNmbr = dstPosting.Tables[0].Rows[i]["AdvanceNo"].ToString();
                        capItem1.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        capItem1.ItemText = dstPosting.Tables[0].Rows[i]["Description"].ToString();
                        capItem1.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        capItem1.Businessplace = dstPosting.Tables[0].Rows[i]["BranchCode"].ToString();
                        capItem1.TaxCode = TaxCodeConst.NV;
                        capItem1.SpGlInd = SpGlIndConst.D;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem1.BusArea = dstPosting.Tables[0].Rows[i]["BusinessArea"].ToString();
                        }

                        capItem1.Active = true;
                        capItem1.CreBy = 1;
                        capItem1.CreDate = DateTime.Now;
                        capItem1.UpdBy = 1;
                        capItem1.UpdDate = DateTime.Now;
                        capItem1.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                        Bapiaccr09 accrItem1 = new Bapiaccr09();
                        accrItem1.DocId = DocID;
                        accrItem1.DocSeq = "M";
                        accrItem1.DocKind = DocKind;
                        accrItem1.ItemnoAcc = intSeq.ToString();

                        accrItem1.Currency = mainCurrencySymbol;
                        accrItem1.AmtDoccur = 0 - decimal.Parse(dstPosting.Tables[0].Rows[i]["AmountMainCurrency"].ToString());
                        accrItem1.ExchRate = decimal.Parse(dstPosting.Tables[0].Rows[i]["ExchangeRateMainToTHBCurrency"].ToString());

                        accrItem1.Active = true;
                        accrItem1.CreBy = 1;
                        accrItem1.CreDate = DateTime.Now;
                        accrItem1.UpdBy = 1;
                        accrItem1.UpdDate = DateTime.Now;
                        accrItem1.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);

                        #endregion Credit

                        #region Debit

                        intSeq++;

                        Bapiacap09 capItem2 = new Bapiacap09();
                        capItem2.DocId = DocID;
                        capItem2.DocSeq = "M";
                        capItem2.DocKind = DocKind;
                        capItem2.ItemnoAcc = intSeq.ToString();
                        capItem2.VendorNo = dstPosting.Tables[0].Rows[i]["PBCode"].ToString();
                        capItem2.PmntBlock = PostingConst.PmntBlock;
                        capItem2.Pmnttrms = PostingConst.Pmnttrms;
                        capItem2.BlineDate = dstPosting.Tables[0].Rows[i]["BaseLineDate"].ToString();
                        capItem2.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[i]["RequesterID"].ToString())).EmployeeName;
                        capItem2.AllocNmbr = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                        capItem2.ItemText = dstPosting.Tables[0].Rows[i]["Description"].ToString();
                        capItem2.ItemText = SAPUIHelper.SubString50(capItem1.ItemText);
                        capItem2.Businessplace = dstPosting.Tables[0].Rows[i]["BranchCode"].ToString();
                        capItem2.TaxCode = TaxCodeConst.NV;

                        if (bool.Parse(dstPosting.Tables[0].Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            capItem2.BusArea = dstPosting.Tables[0].Rows[i]["BusinessArea"].ToString();
                        }

                        capItem2.Active = true;
                        capItem2.CreBy = 1;
                        capItem2.CreDate = DateTime.Now;
                        capItem2.UpdBy = 1;
                        capItem2.UpdDate = DateTime.Now;
                        capItem2.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiacap09Service.Save(capItem2);

                        Bapiaccr09 accrItem2 = new Bapiaccr09();
                        accrItem2.DocId = DocID;
                        accrItem2.DocSeq = "M";
                        accrItem2.DocKind = DocKind;
                        accrItem2.ItemnoAcc = intSeq.ToString();

                        accrItem2.Currency = mainCurrencySymbol;
                        accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[i]["AmountMainCurrency"].ToString());
                        accrItem2.ExchRate = decimal.Parse(dstPosting.Tables[0].Rows[i]["ExchangeRateMainToTHBCurrency"].ToString());

                        accrItem2.Active = true;
                        accrItem2.CreBy = 1;
                        accrItem2.CreDate = DateTime.Now;
                        accrItem2.UpdBy = 1;
                        accrItem2.UpdDate = DateTime.Now;
                        accrItem2.UpdPgm = "ExpenseRemetancePosting";
                        BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);
                        #endregion Debit
                    }
                }
                #endregion คู่บัญชี
            }
        }
    }
}
