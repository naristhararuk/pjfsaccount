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
using SS.DB.Query;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SS.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.BLL;
using SCG.eAccounting.SAP.Query;
using SAP.Middleware.Connector;

namespace SCG.eAccounting.SAP.BAPI.Service
{
    public class ExpensePostingService : PostingService
    {
        public override void CreatePostData(long DocID, string DocKind)
        {
            Hashtable parameter = new Hashtable();
            parameter.Add("@DOCUMENT_ID", DocID.ToString());

            #region IC Case

            bool isHaveICCase = false;
            bool comRequestUseEcc = false;
            // Check IC Case
            DataSet dstICCheck = new DBManage().ExecuteQuery("EXPENSE_IC_CHECK", parameter);
            if (dstICCheck.Tables[0].Rows.Count > 0)
            {
                isHaveICCase = true;
                comRequestUseEcc = bool.Parse(dstICCheck.Tables[0].Rows[0]["ComRequesterUseEcc"].ToString());
            }
            else
                isHaveICCase = false;
            #endregion IC Case

            #region Query Data for Expense
            DataSet dstPosting = new DBManage().ExecuteQuery("EXPENSE_POSTING", parameter);
            DataTable dtbHead = dstPosting.Tables[0];
            DataTable dtbItem = dstPosting.Tables[1];
            DataTable dtbAdvance = dstPosting.Tables[2];
            #endregion Query Data for Expense

            string expenseType = dtbHead.Rows[0]["ExpenseType"].ToString();
            SCGDocument doc = SCG.eAccounting.Query.ScgeAccountingQueryProvider.SCGDocumentQuery.FindProxyByIdentity(DocID);
            DbSapInstance sap = SCG.DB.Query.ScgDbQueryProvider.DbCompanyQuery.GetSAPDocTypeForPosting(doc.CompanyID.CompanyCode);

            bool repOffice = false;
            string mainCurrencySymbol = string.Empty;
            // ตรวจสอบว่ามีข้อมูลหรือไม่
            if (dtbHead.Rows.Count > 0)
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

                int intCountSEQ = 0;        // if intCountSEQ>0 แสดงว่าที่ Row ที่มี Vat นะครับ
                double douSumNetAmount = 0;
                double douSumNetAmountMainCurrency = 0;

                bool isHaveVatOrWht = false;
                bool isHaveAdvance = false;
                if (dtbAdvance.Rows.Count > 0) isHaveAdvance = true;

                #region Query Line Item
                DataRow[] drItemAr = dtbItem.Select(" ISNULL(isVAT,0)=0 AND ISNULL(isWHT,0)=0 AND SpecialGL<>'' ");
                DataRow[] drItemIC = dtbItem.Select(" ISNULL(isVAT,0)=0 AND ISNULL(isWHT,0)=0 AND InvoiceDocumentType='P' ");
                DataRow[] drItemGl;
                if (isHaveICCase)
                    drItemGl = dtbItem.Select(" ISNULL(isVAT,0)=0 AND ISNULL(isWHT,0)=0 AND SpecialGL ='' AND InvoiceDocumentType<>'P' ");
                else
                    drItemGl = dtbItem.Select(" ISNULL(isVAT,0)=0 AND ISNULL(isWHT,0)=0 AND SpecialGL ='' ");
                #endregion Query Line Item

                #region Check for have IC Case
                double douSumPerdiem = 0;
                double douVatPerdiem = 0;

                if (!repOffice && isHaveICCase)
                {
                    for (int i = 0; i < drItemIC.Length; i++)
                        douSumPerdiem += double.Parse(drItemIC[i]["AmountItem"].ToString());

                    douVatPerdiem = (douSumPerdiem * 7) / 100;

                    #region บริษัทต้นสังกัด ( Company's Requester )

                    #region บริษัทต้นสังกัด ข้อมูลชุดที่ 1 (B2C01)
                    {
                        // ***********************
                        // Header
                        // ***********************
                        #region HEAD
                        Bapiache09 che09B2C01 = new Bapiache09();
                        che09B2C01.DocId = DocID;
                        che09B2C01.DocSeq = "B2C01";
                        che09B2C01.DocKind = DocKind;
                        che09B2C01.DocStatus = "N";
                        che09B2C01.BusAct = PostingConst.BusAct;
                        che09B2C01.Username = comRequestUseEcc ? sap.UserCPIC : ParameterServices.BAPI_UserCPIC;
                        che09B2C01.CompCode = SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).CompanyID;
                        che09B2C01.DocDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09B2C01.DocKind = DocKind;
                        che09B2C01.DocType = comRequestUseEcc ? sap.DocTypeExpPostingFR : DocTypeConst.KR;
                        che09B2C01.PstngDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09B2C01.RefDocNo = SAPUIHelper.SubString(16, dtbHead.Rows[0]["DocumentNo"].ToString());
                        che09B2C01.DocAppFlag = "V";

                        che09B2C01.Active = true;
                        che09B2C01.CreBy = 1;
                        che09B2C01.CreDate = DateTime.Now;
                        che09B2C01.UpdBy = 1;
                        che09B2C01.UpdDate = DateTime.Now;
                        che09B2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiache09Service.Save(che09B2C01);
                        #endregion HEAD

                        #region Extention BRNCH
                        Bapiacextc cexHeadB2C01 = new Bapiacextc();
                        cexHeadB2C01.DocId = DocID;
                        cexHeadB2C01.DocSeq = "B2C01";
                        cexHeadB2C01.DocKind = DocKind;
                        cexHeadB2C01.Field1 = PostingConst.BRNCH;
                        cexHeadB2C01.Field2 = dtbHead.Rows[0]["BranchCodeHeader"].ToString();

                        cexHeadB2C01.Active = true;
                        cexHeadB2C01.CreBy = 1;
                        cexHeadB2C01.CreDate = DateTime.Now;
                        cexHeadB2C01.UpdBy = 1;
                        cexHeadB2C01.UpdDate = DateTime.Now;
                        cexHeadB2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexHeadB2C01);
                        #endregion Extention BRNCH

                        #region Extention Non Vat
                        Bapiacextc cexNVatB2C01 = new Bapiacextc();
                        cexNVatB2C01.DocId = DocID;
                        cexNVatB2C01.DocSeq = "B2C01";
                        cexNVatB2C01.DocKind = DocKind;
                        cexNVatB2C01.Field1 = PostingConst.VAT;
                        cexNVatB2C01.Field2 = TaxCodeConst.NV;

                        cexNVatB2C01.Active = true;
                        cexNVatB2C01.CreBy = 1;
                        cexNVatB2C01.CreDate = DateTime.Now;
                        cexNVatB2C01.UpdBy = 1;
                        cexNVatB2C01.UpdDate = DateTime.Now;
                        cexNVatB2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexNVatB2C01);
                        #endregion Extention Non Vat

                        // ***********************
                        // Credit
                        // ***********************
                        int intItemICCount = 1;

                        #region Bapiacap09
                        Bapiacap09 cap09B2C01 = new Bapiacap09();
                        cap09B2C01.DocId = DocID;
                        cap09B2C01.DocSeq = "B2C01";
                        cap09B2C01.DocKind = DocKind;
                        cap09B2C01.ItemnoAcc = intItemICCount.ToString();

                        cap09B2C01.VendorNo = SAPUIHelper.ConvertCompanyCodeForIC(dtbHead.Rows[0]["CompanyCode"].ToString());
                        cap09B2C01.AllocNmbr = SAPUIHelper.SubString18(dtbHead.Rows[0]["DocumentNo"].ToString());
                        cap09B2C01.ItemText = SAPUIHelper.SubString50(dtbHead.Rows[0]["DescriptionDocument"].ToString());
                        cap09B2C01.TaxCode = TaxCodeConst.NV;

                        cap09B2C01.Pmnttrms = PostingConst.PmnttrmsIC;
                        cap09B2C01.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                        //cap09B2C01.PymtMeth = string.Empty; // dtbHead.Rows[0]["PaymentMethodCode"].ToString();
                        cap09B2C01.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();

                        cap09B2C01.Active = true;
                        cap09B2C01.CreBy = 1;
                        cap09B2C01.CreDate = DateTime.Now;
                        cap09B2C01.UpdBy = 1;
                        cap09B2C01.UpdDate = DateTime.Now;
                        cap09B2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacap09Service.Save(cap09B2C01);
                        #endregion Bapiacap09

                        #region Bapiaccr09
                        Bapiaccr09 ccr09B2C01 = new Bapiaccr09();
                        ccr09B2C01.DocId = DocID;
                        ccr09B2C01.DocSeq = "B2C01";
                        ccr09B2C01.DocKind = DocKind;
                        ccr09B2C01.ItemnoAcc = intItemICCount.ToString();
                        ccr09B2C01.Currency = PostingConst.Currency;
                        ccr09B2C01.AmtDoccur = 0 - decimal.Parse(douSumPerdiem.ToString());

                        ccr09B2C01.Active = true;
                        ccr09B2C01.CreBy = 1;
                        ccr09B2C01.CreDate = DateTime.Now;
                        ccr09B2C01.UpdBy = 1;
                        ccr09B2C01.UpdDate = DateTime.Now;
                        ccr09B2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccr09B2C01);
                        #endregion Bapiaccr09

                        // ***********************
                        // Debit
                        // ***********************
                        for (int i = 0; i < drItemIC.Length; i++)
                        {
                            intItemICCount++;

                            #region Bapiacgl09
                            Bapiacgl09 cgl09B2W01 = new Bapiacgl09();
                            cgl09B2W01.DocId = DocID;
                            cgl09B2W01.DocSeq = "B2C01";
                            cgl09B2W01.DocKind = DocKind;
                            cgl09B2W01.ItemnoAcc = intItemICCount.ToString();

                            cgl09B2W01.AllocNmbr = SAPUIHelper.SubString18(SAPUIHelper.GetEmployee(long.Parse(drItemIC[i]["RequesterID"].ToString())).EmployeeName);
                            cgl09B2W01.ItemText = SAPUIHelper.SubString50(drItemIC[i]["Description"].ToString());

                            string glAccountCode = string.Empty;
                            string costcenterCode = SAPUIHelper.GetEmployee(long.Parse(drItemIC[i]["RequesterID"].ToString())).CostCenterCode;

                            string expenseGroup = string.IsNullOrEmpty(costcenterCode) ? "0" : costcenterCode.Substring(3, 1).Equals("0") ? "0" : "1";

                            switch (i)
                            {
                                case 0:
                                    glAccountCode = (expenseType == ZoneType.Domestic) ? ParameterServices.AccountOfficialPerdiem_DM : ParameterServices.AccountOfficialPerdiem;
                                    break;
                                case 1:
                                    if (drItemIC[i]["AccountCode"].ToString() == ParameterServices.AccountPerdiem || drItemIC[i]["AccountCode"].ToString() == ParameterServices.AccountPerdiem_DM)
                                    {
                                        glAccountCode = (expenseType == ZoneType.Domestic) ? ParameterServices.AccountPerdiem_DM : ParameterServices.AccountPerdiem;
                                    }
                                    else
                                    {
                                        glAccountCode = (expenseType == ZoneType.Domestic) ? ParameterServices.AccountPerdiem_DM : ParameterServices.AccountInvoicePerdiem;
                                    }
                                    break;
                                case 2:
                                    glAccountCode = (expenseType == ZoneType.Domestic) ? string.Empty : ParameterServices.AccountInvoicePerdiem;
                                    break;
                            }

                            if (comRequestUseEcc)
                            {
                                cgl09B2W01.GlAccount = glAccountCode;
                                DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindDbAccountByAccountCode(glAccountCode);
                                if (account != null)
                                {
                                    if (account.TaxCode != (int)TaxCodeOption.None)
                                        cgl09B2W01.TaxCode = TaxCodeConst.NV;
                                    else
                                        cgl09B2W01.TaxCode = string.Empty;

                                    if (account.CostCenter != (int)CostCenterOption.None)
                                        cgl09B2W01.Costcenter = SAPUIHelper.GetEmployee(long.Parse(drItemIC[i]["RequesterID"].ToString())).CostCenterCode;
                                    else
                                        cgl09B2W01.Costcenter = string.Empty;

                                    if (account.InternalOrder != (int)SaleOrderOption.None)
                                    {
                                        cgl09B2W01.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[i]["SaleOrder"].ToString());
                                        cgl09B2W01.SOrdItem = drItemIC[i]["SaleItem"].ToString();
                                    }
                                    else
                                    {
                                        cgl09B2W01.SalesOrd = string.Empty;
                                        cgl09B2W01.SOrdItem = string.Empty;
                                    }
                                }
                                else
                                {
                                    cgl09B2W01.Costcenter = SAPUIHelper.GetEmployee(long.Parse(drItemIC[i]["RequesterID"].ToString())).CostCenterCode;
                                    cgl09B2W01.TaxCode = TaxCodeConst.NV;
                                    cgl09B2W01.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[i]["SaleOrder"].ToString());
                                    cgl09B2W01.SOrdItem = drItemIC[i]["SaleItem"].ToString();
                                }
                            }
                            else
                            {
                                cgl09B2W01.GlAccount = SAPUIHelper.GetAccountCodeExpMapping(glAccountCode, expenseGroup);
                                cgl09B2W01.TaxCode = TaxCodeConst.NV;
                                cgl09B2W01.Costcenter = SAPUIHelper.GetEmployee(long.Parse(drItemIC[i]["RequesterID"].ToString())).CostCenterCode;
                                cgl09B2W01.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[i]["SaleOrder"].ToString());
                                cgl09B2W01.SOrdItem = drItemIC[i]["SaleItem"].ToString();
                            }

                            cgl09B2W01.Active = true;
                            cgl09B2W01.CreBy = 1;
                            cgl09B2W01.CreDate = DateTime.Now;
                            cgl09B2W01.UpdBy = 1;
                            cgl09B2W01.UpdDate = DateTime.Now;
                            cgl09B2W01.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacgl09Service.Save(cgl09B2W01);
                            #endregion Bapiacgl09

                            #region Bapiaccr09
                            Bapiaccr09 ccrt09B2C01 = new Bapiaccr09();
                            ccrt09B2C01.DocId = DocID;
                            ccrt09B2C01.DocSeq = "B2C01";
                            ccrt09B2C01.DocKind = DocKind;
                            ccrt09B2C01.ItemnoAcc = intItemICCount.ToString();
                            ccrt09B2C01.Currency = PostingConst.Currency;
                            ccrt09B2C01.AmtDoccur = decimal.Parse(drItemIC[i]["AmountItem"].ToString());

                            ccrt09B2C01.Active = true;
                            ccrt09B2C01.CreBy = 1;
                            ccrt09B2C01.CreDate = DateTime.Now;
                            ccrt09B2C01.UpdBy = 1;
                            ccrt09B2C01.UpdDate = DateTime.Now;
                            ccrt09B2C01.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccrt09B2C01);
                            #endregion Bapiaccr09
                        }
                    }
                    #endregion บริษัทต้นสังกัด ข้อมูลชุดที่ 1 (B2C01)

                    #region บริษัทต้นสังกัด ข้อมูลชุดที่ 2 (B2C02)
                    {
                        // ***********************
                        // Header
                        // ***********************
                        #region HEAD
                        Bapiache09 che09B2C02 = new Bapiache09();
                        che09B2C02.DocId = DocID;
                        che09B2C02.DocSeq = "B2C02";
                        che09B2C02.DocKind = DocKind;
                        che09B2C02.DocStatus = "N";
                        che09B2C02.BusAct = PostingConst.BusAct;
                        che09B2C02.Username = comRequestUseEcc ? sap.UserCPIC : ParameterServices.BAPI_UserCPIC;
                        che09B2C02.CompCode = SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).CompanyID;
                        che09B2C02.DocDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09B2C02.DocKind = DocKind;
                        che09B2C02.DocType = comRequestUseEcc ? sap.DocTypeExpICPostingFR : DocTypeConst.DI;
                        che09B2C02.PstngDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09B2C02.RefDocNo = SAPUIHelper.SubString(16, dtbHead.Rows[0]["DocumentNo"].ToString());
                        che09B2C02.DocAppFlag = "A";

                        che09B2C02.Active = true;
                        che09B2C02.CreBy = 1;
                        che09B2C02.CreDate = DateTime.Now;
                        che09B2C02.UpdBy = 1;
                        che09B2C02.UpdDate = DateTime.Now;
                        che09B2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiache09Service.Save(che09B2C02);
                        #endregion HEAD

                        #region Extention BRNCH
                        Bapiacextc cexHeadB2C02 = new Bapiacextc();
                        cexHeadB2C02.DocId = DocID;
                        cexHeadB2C02.DocSeq = "B2C02";
                        cexHeadB2C02.DocKind = DocKind;
                        cexHeadB2C02.Field1 = PostingConst.BRNCH;
                        cexHeadB2C02.Field2 = dtbHead.Rows[0]["BranchCodeHeader"].ToString();

                        cexHeadB2C02.Active = true;
                        cexHeadB2C02.CreBy = 1;
                        cexHeadB2C02.CreDate = DateTime.Now;
                        cexHeadB2C02.UpdBy = 1;
                        cexHeadB2C02.UpdDate = DateTime.Now;
                        cexHeadB2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexHeadB2C02);
                        #endregion Extention BRNCH

                        #region Extention Vat
                        Bapiacextc cexNVatB2C02 = new Bapiacextc();
                        cexNVatB2C02.DocId = DocID;
                        cexNVatB2C02.DocSeq = "B2C02";
                        cexNVatB2C02.DocKind = DocKind;
                        cexNVatB2C02.Field1 = PostingConst.VAT;
                        cexNVatB2C02.Field2 = TaxCodeConst.S7;

                        cexNVatB2C02.Active = true;
                        cexNVatB2C02.CreBy = 1;
                        cexNVatB2C02.CreDate = DateTime.Now;
                        cexNVatB2C02.UpdBy = 1;
                        cexNVatB2C02.UpdDate = DateTime.Now;
                        cexNVatB2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexNVatB2C02);
                        #endregion Extention Vat

                        #region Extention WTH

                        if (comRequestUseEcc)
                        {
                            double douWHTAmount = (douSumPerdiem * 3) / 100;

                            Bapiacextc cexWHTB2C02 = new Bapiacextc();
                            cexWHTB2C02.DocId = DocID;
                            cexWHTB2C02.DocSeq = "B2C02";
                            cexWHTB2C02.DocKind = DocKind;
                            cexWHTB2C02.Field1 = "WTH1";
                            cexWHTB2C02.Field2 = ParameterServices.BAPI_IC_WHTType_AR;
                            cexWHTB2C02.Field3 = comRequestUseEcc ? ParameterServices.BAPI_IC_WHTCode : SAPUIHelper.GetWHTCodeExpMapping(ParameterServices.BAPI_IC_WHTCode);
                            cexWHTB2C02.Field4 = douSumPerdiem.ToString() + " | " + douWHTAmount.ToString();

                            cexWHTB2C02.Active = true;
                            cexWHTB2C02.CreBy = 1;
                            cexWHTB2C02.CreDate = DateTime.Now;
                            cexWHTB2C02.UpdBy = 1;
                            cexWHTB2C02.UpdDate = DateTime.Now;
                            cexWHTB2C02.UpdPgm = "PostingService";
                            BapiServiceProvider.BapiacextcService.Save(cexWHTB2C02);
                        }

                        #endregion Extention WTH

                        // ***********************
                        // Debit
                        // ***********************
                        #region Bapiacar09
                        Bapiacar09 car09B2C02 = new Bapiacar09();
                        car09B2C02.DocId = DocID;
                        car09B2C02.DocSeq = "B2C02";
                        car09B2C02.DocKind = DocKind;
                        car09B2C02.ItemnoAcc = "1";

                        car09B2C02.Customer = SAPUIHelper.ConvertCompanyCodeForIC(dtbHead.Rows[0]["CompanyCode"].ToString());
                        car09B2C02.AllocNmbr = SAPUIHelper.SubString(18, dtbHead.Rows[0]["DocumentNo"].ToString());
                        car09B2C02.ItemText = SAPUIHelper.SubString50(dtbHead.Rows[0]["DescriptionDocument"].ToString());
                        car09B2C02.TaxCode = TaxCodeConst.S7;
                        car09B2C02.Pmnttrms = PostingConst.PmnttrmsIC;

                        car09B2C02.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                        car09B2C02.SpGlInd = "";

                        car09B2C02.Active = true;
                        car09B2C02.CreBy = 1;
                        car09B2C02.CreDate = DateTime.Now;
                        car09B2C02.UpdBy = 1;
                        car09B2C02.UpdDate = DateTime.Now;
                        car09B2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacar09Service.Save(car09B2C02);
                        #endregion Bapiacar09

                        #region Bapiaccr09
                        double douSumVatPerdiem = douSumPerdiem + douVatPerdiem;

                        Bapiaccr09 ccr09B2C022 = new Bapiaccr09();
                        ccr09B2C022.DocId = DocID;
                        ccr09B2C022.DocSeq = "B2C02";
                        ccr09B2C022.DocKind = DocKind;
                        ccr09B2C022.ItemnoAcc = "1";
                        ccr09B2C022.Currency = PostingConst.Currency;
                        ccr09B2C022.AmtDoccur = decimal.Parse(douSumVatPerdiem.ToString());

                        ccr09B2C022.Active = true;
                        ccr09B2C022.CreBy = 1;
                        ccr09B2C022.CreDate = DateTime.Now;
                        ccr09B2C022.UpdBy = 1;
                        ccr09B2C022.UpdDate = DateTime.Now;
                        ccr09B2C022.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccr09B2C022);
                        #endregion Bapiaccr09

                        // ***********************
                        // Credit
                        // ***********************
                        #region Bapiacgl09
                        Bapiacgl09 cgl09B2W02 = new Bapiacgl09();
                        cgl09B2W02.DocId = DocID;
                        cgl09B2W02.DocSeq = "B2C02";
                        cgl09B2W02.DocKind = DocKind;
                        cgl09B2W02.ItemnoAcc = "2";

                        cgl09B2W02.AllocNmbr = SAPUIHelper.SubString18(SAPUIHelper.GetEmployee(long.Parse(drItemIC[0]["RequesterID"].ToString())).EmployeeName);
                        cgl09B2W02.ItemText = SAPUIHelper.SubString50((expenseType == ZoneType.Domestic) ? ParameterServices.BAPI_FEXP_TEXT_DOMESTIC : ParameterServices.BAPI_FEXP_TEXT);

                        if (comRequestUseEcc)
                        {
                            cgl09B2W02.GlAccount = ParameterServices.BAPI_OSI;
                            cgl09B2W02.BusArea = string.Empty;

                            DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindDbAccountByAccountCode(ParameterServices.BAPI_OSI);
                            if (account != null)
                            {
                                if (account.CostCenter != (int)CostCenterOption.None)
                                    cgl09B2W02.Costcenter = SAPUIHelper.GetEmployee(long.Parse(drItemIC[0]["RequesterID"].ToString())).CostCenterCode;
                                else
                                    cgl09B2W02.Costcenter = string.Empty;

                                if (account.TaxCode != (int)TaxCodeOption.None)
                                    cgl09B2W02.TaxCode = TaxCodeConst.S7;
                                else
                                    cgl09B2W02.TaxCode = string.Empty;

                                if (account.SaleOrder != (int)SaleOrderOption.None)
                                {
                                    cgl09B2W02.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                                    cgl09B2W02.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                                }
                                else
                                {
                                    cgl09B2W02.SalesOrd = string.Empty;
                                    cgl09B2W02.SOrdItem = string.Empty;
                                }
                            }
                            else
                            {
                                cgl09B2W02.TaxCode = TaxCodeConst.S7;
                                cgl09B2W02.Costcenter = SAPUIHelper.GetEmployee(long.Parse(drItemIC[0]["RequesterID"].ToString())).CostCenterCode;

                                cgl09B2W02.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                                cgl09B2W02.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                            }

                        }
                        else
                        {
                            cgl09B2W02.GlAccount = SAPUIHelper.GetAccountCodeExpMapping(ParameterServices.BAPI_OSI, string.Empty);
                            cgl09B2W02.BusArea = drItemIC[0]["BranchCode"].ToString();

                            cgl09B2W02.TaxCode = TaxCodeConst.S7;
                            cgl09B2W02.Costcenter = SAPUIHelper.GetEmployee(long.Parse(drItemIC[0]["RequesterID"].ToString())).CostCenterCode;

                            cgl09B2W02.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                            cgl09B2W02.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                        }

                        cgl09B2W02.Active = true;
                        cgl09B2W02.CreBy = 1;
                        cgl09B2W02.CreDate = DateTime.Now;
                        cgl09B2W02.UpdBy = 1;
                        cgl09B2W02.UpdDate = DateTime.Now;
                        cgl09B2W02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacgl09Service.Save(cgl09B2W02);
                        #endregion Bapiacgl09

                        #region Bapiaccr09
                        Bapiaccr09 ccrt09B2C02 = new Bapiaccr09();
                        ccrt09B2C02.DocId = DocID;
                        ccrt09B2C02.DocSeq = "B2C02";
                        ccrt09B2C02.DocKind = DocKind;
                        ccrt09B2C02.ItemnoAcc = "2";
                        ccrt09B2C02.Currency = PostingConst.Currency;
                        ccrt09B2C02.AmtDoccur = 0 - decimal.Parse(douSumPerdiem.ToString());

                        ccrt09B2C02.Active = true;
                        ccrt09B2C02.CreBy = 1;
                        ccrt09B2C02.CreDate = DateTime.Now;
                        ccrt09B2C02.UpdBy = 1;
                        ccrt09B2C02.UpdDate = DateTime.Now;
                        ccrt09B2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccrt09B2C02);
                        #endregion Bapiaccr09

                        #region Bapiactx09
                        Bapiactx09 ctx09B2C02 = new Bapiactx09();
                        ctx09B2C02.DocId = DocID;
                        ctx09B2C02.DocSeq = "B2C02";
                        ctx09B2C02.DocKind = DocKind;
                        ctx09B2C02.ItemnoAcc = "3";

                        ctx09B2C02.GlAccount = comRequestUseEcc ? ParameterServices.BAPI_Defer_Vat_S7 : SAPUIHelper.GetAccountCodeExpMapping(ParameterServices.BAPI_Defer_Vat_S7, string.Empty);
                        ctx09B2C02.TaxCode = TaxCodeConst.S7;

                        ctx09B2C02.Active = true;
                        ctx09B2C02.CreBy = 1;
                        ctx09B2C02.CreDate = DateTime.Now;
                        ctx09B2C02.UpdBy = 1;
                        ctx09B2C02.UpdDate = DateTime.Now;
                        ctx09B2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiactx09Service.Save(ctx09B2C02);
                        #endregion Bapiactx09

                        #region Bapiaccr09
                        Bapiaccr09 ccrt09B2C021 = new Bapiaccr09();
                        ccrt09B2C021.DocId = DocID;
                        ccrt09B2C021.DocSeq = "B2C02";
                        ccrt09B2C021.DocKind = DocKind;
                        ccrt09B2C021.ItemnoAcc = "3";
                        ccrt09B2C021.Currency = PostingConst.Currency;
                        ccrt09B2C021.AmtDoccur = 0 - decimal.Parse(douVatPerdiem.ToString());
                        ccrt09B2C021.AmtBase = 0 - decimal.Parse(douSumPerdiem.ToString());

                        ccrt09B2C021.Active = true;
                        ccrt09B2C021.CreBy = 1;
                        ccrt09B2C021.CreDate = DateTime.Now;
                        ccrt09B2C021.UpdBy = 1;
                        ccrt09B2C021.UpdDate = DateTime.Now;
                        ccrt09B2C021.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccrt09B2C021);
                        #endregion Bapiaccr09


                    }
                    #endregion บริษัทต้นสังกัด ข้อมูลชุดที่ 2 (B2C02)

                    #endregion บริษัทต้นสังกัด ( Company's Requester )

                    #region บริษัทปฏิบัติงาน ( Company's Document )

                    #region บริษัทปฏิบัติงาน ข้อมูลชุดที่ 2 (W2C02)
                    {
                        // ***********************
                        // Header
                        // ***********************
                        #region HEAD
                        Bapiache09 che09W2C02 = new Bapiache09();
                        che09W2C02.DocId = DocID;
                        che09W2C02.DocSeq = "W2C01";
                        che09W2C02.DocKind = DocKind;
                        che09W2C02.DocStatus = "N";
                        che09W2C02.BusAct = PostingConst.BusAct;
                        che09W2C02.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                        che09W2C02.CompCode = dtbHead.Rows[0]["CompanyCode"].ToString();
                        che09W2C02.DocDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09W2C02.DocKind = DocKind;
                        che09W2C02.DocType = sap.DocTypeExpPostingFR;//DocTypeConst.KR;
                        che09W2C02.PstngDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09W2C02.RefDocNo = SAPUIHelper.SubString(16, dtbHead.Rows[0]["DocumentNo"].ToString());
                        che09W2C02.DocAppFlag = "V";

                        che09W2C02.Active = true;
                        che09W2C02.CreBy = 1;
                        che09W2C02.CreDate = DateTime.Now;
                        che09W2C02.UpdBy = 1;
                        che09W2C02.UpdDate = DateTime.Now;
                        che09W2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiache09Service.Save(che09W2C02);
                        #endregion HEAD

                        #region Extention BRNCH
                        Bapiacextc cexHeadW2C02 = new Bapiacextc();
                        cexHeadW2C02.DocId = DocID;
                        cexHeadW2C02.DocSeq = "W2C01";
                        cexHeadW2C02.DocKind = DocKind;
                        cexHeadW2C02.Field1 = PostingConst.BRNCH;
                        cexHeadW2C02.Field2 = dtbHead.Rows[0]["BranchCodeHeader"].ToString();

                        cexHeadW2C02.Active = true;
                        cexHeadW2C02.CreBy = 1;
                        cexHeadW2C02.CreDate = DateTime.Now;
                        cexHeadW2C02.UpdBy = 1;
                        cexHeadW2C02.UpdDate = DateTime.Now;
                        cexHeadW2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexHeadW2C02);
                        #endregion Extention BRNCH

                        #region Extention Vat
                        Bapiacextc cexNVatW2C02 = new Bapiacextc();
                        cexNVatW2C02.DocId = DocID;
                        cexNVatW2C02.DocSeq = "W2C01";
                        cexNVatW2C02.DocKind = DocKind;
                        cexNVatW2C02.Field1 = PostingConst.VAT;
                        cexNVatW2C02.Field2 = TaxCodeConst.D7;

                        cexNVatW2C02.Active = true;
                        cexNVatW2C02.CreBy = 1;
                        cexNVatW2C02.CreDate = DateTime.Now;
                        cexNVatW2C02.UpdBy = 1;
                        cexNVatW2C02.UpdDate = DateTime.Now;
                        cexNVatW2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexNVatW2C02);
                        #endregion Extention Vat

                        #region Extention WTH
                        double douWHTAmount = (douSumPerdiem * 3) / 100;

                        Bapiacextc cexWHT = new Bapiacextc();
                        cexWHT.DocId = DocID;
                        cexWHT.DocSeq = "W2C01";
                        cexWHT.DocKind = DocKind;
                        cexWHT.Field1 = "WTH1";
                        cexWHT.Field2 = ParameterServices.BAPI_IC_WHTType;
                        cexWHT.Field3 = ParameterServices.BAPI_IC_WHTCode;
                        cexWHT.Field4 = douSumPerdiem.ToString() + " | " + douWHTAmount.ToString();

                        cexWHT.Active = true;
                        cexWHT.CreBy = 1;
                        cexWHT.CreDate = DateTime.Now;
                        cexWHT.UpdBy = 1;
                        cexWHT.UpdDate = DateTime.Now;
                        cexWHT.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexWHT);
                        #endregion Extention WTH


                        // ***********************
                        // Credit
                        // ***********************
                        #region Bapiacap09
                        Bapiacap09 cap09W2C02 = new Bapiacap09();
                        cap09W2C02.DocId = DocID;
                        cap09W2C02.DocSeq = "W2C01";
                        cap09W2C02.DocKind = DocKind;
                        cap09W2C02.ItemnoAcc = "1";

                        cap09W2C02.VendorNo = SAPUIHelper.ConvertCompanyCodeForIC(SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).CompanyID);
                        cap09W2C02.AllocNmbr = SAPUIHelper.SubString18(dtbHead.Rows[0]["DocumentNo"].ToString());
                        cap09W2C02.ItemText = SAPUIHelper.SubString50(dtbHead.Rows[0]["DescriptionDocument"].ToString());
                        cap09W2C02.TaxCode = TaxCodeConst.D7;

                        cap09W2C02.Pmnttrms = PostingConst.PmnttrmsIC;
                        cap09W2C02.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                        // cap09W2C02.PymtMeth = dtbHead.Rows[0]["PaymentMethodCode"].ToString();
                        cap09W2C02.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();
                        cap09W2C02.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();

                        cap09W2C02.Active = true;
                        cap09W2C02.CreBy = 1;
                        cap09W2C02.CreDate = DateTime.Now;
                        cap09W2C02.UpdBy = 1;
                        cap09W2C02.UpdDate = DateTime.Now;
                        cap09W2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacap09Service.Save(cap09W2C02);
                        #endregion Bapiacap09

                        #region Bapiaccr09
                        double douSumVatPerdiem = douSumPerdiem + douVatPerdiem;

                        Bapiaccr09 ccr09W2C02 = new Bapiaccr09();
                        ccr09W2C02.DocId = DocID;
                        ccr09W2C02.DocSeq = "W2C01";
                        ccr09W2C02.DocKind = DocKind;
                        ccr09W2C02.ItemnoAcc = "1";
                        ccr09W2C02.Currency = PostingConst.Currency;
                        ccr09W2C02.AmtDoccur = 0 - decimal.Parse(douSumVatPerdiem.ToString());

                        ccr09W2C02.Active = true;
                        ccr09W2C02.CreBy = 1;
                        ccr09W2C02.CreDate = DateTime.Now;
                        ccr09W2C02.UpdBy = 1;
                        ccr09W2C02.UpdDate = DateTime.Now;
                        ccr09W2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccr09W2C02);
                        #endregion Bapiaccr09

                        // ***********************
                        // Debit
                        // ***********************
                        #region Bapiacgl09
                        Bapiacgl09 cgl09W2C02 = new Bapiacgl09();
                        cgl09W2C02.DocId = DocID;
                        cgl09W2C02.DocSeq = "W2C01";
                        cgl09W2C02.DocKind = DocKind;
                        cgl09W2C02.ItemnoAcc = "2";

                        cgl09W2C02.GlAccount = SAPUIHelper.GetAccountCodeOfCostCenterForIC(drItemIC[0]["CostCenterCode"].ToString());
                        cgl09W2C02.AllocNmbr = SAPUIHelper.SubString18(SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).EmployeeName);
                        cgl09W2C02.ItemText = SAPUIHelper.SubString50(drItemIC[0]["Description"].ToString());

                        DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindDbAccountByAccountCode(cgl09W2C02.GlAccount);

                        if (account != null)
                        {
                            if (account.TaxCode != (int)TaxCodeOption.None)
                                cgl09W2C02.TaxCode = TaxCodeConst.D7;
                            else
                                cgl09W2C02.TaxCode = string.Empty;

                            if (account.CostCenter != (int)CostCenterOption.None)
                                cgl09W2C02.Costcenter = drItemIC[0]["CostCenterCode"].ToString();
                            else
                                cgl09W2C02.Costcenter = string.Empty;

                            if (account.SaleOrder != (int)SaleOrderOption.None)
                            {
                                cgl09W2C02.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                                cgl09W2C02.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                            }
                            else
                            {
                                cgl09W2C02.SalesOrd = string.Empty;
                                cgl09W2C02.SOrdItem = string.Empty;
                            }
                        }
                        else
                        {
                            cgl09W2C02.TaxCode = TaxCodeConst.D7;
                            cgl09W2C02.Costcenter = drItemIC[0]["CostCenterCode"].ToString();
                            cgl09W2C02.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                            cgl09W2C02.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                        }

                        cgl09W2C02.Active = true;
                        cgl09W2C02.CreBy = 1;
                        cgl09W2C02.CreDate = DateTime.Now;
                        cgl09W2C02.UpdBy = 1;
                        cgl09W2C02.UpdDate = DateTime.Now;
                        cgl09W2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacgl09Service.Save(cgl09W2C02);
                        #endregion Bapiacgl09

                        #region Bapiaccr09
                        Bapiaccr09 ccrt09W2C021 = new Bapiaccr09();
                        ccrt09W2C021.DocId = DocID;
                        ccrt09W2C021.DocSeq = "W2C01";
                        ccrt09W2C021.DocKind = DocKind;
                        ccrt09W2C021.ItemnoAcc = "2";
                        ccrt09W2C021.Currency = PostingConst.Currency;
                        ccrt09W2C021.AmtDoccur = decimal.Parse(douSumPerdiem.ToString());

                        ccrt09W2C021.Active = true;
                        ccrt09W2C021.CreBy = 1;
                        ccrt09W2C021.CreDate = DateTime.Now;
                        ccrt09W2C021.UpdBy = 1;
                        ccrt09W2C021.UpdDate = DateTime.Now;
                        ccrt09W2C021.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccrt09W2C021);
                        #endregion Bapiaccr09

                        #region Bapiactx09
                        Bapiactx09 ctx09W2C02 = new Bapiactx09();
                        ctx09W2C02.DocId = DocID;
                        ctx09W2C02.DocSeq = "W2C01";
                        ctx09W2C02.DocKind = DocKind;
                        ctx09W2C02.ItemnoAcc = "3";
                        ctx09W2C02.GlAccount = ParameterServices.BAPI_Defer_Vat_D7;
                        ctx09W2C02.TaxCode = TaxCodeConst.D7;

                        ctx09W2C02.Active = true;
                        ctx09W2C02.CreBy = 1;
                        ctx09W2C02.CreDate = DateTime.Now;
                        ctx09W2C02.UpdBy = 1;
                        ctx09W2C02.UpdDate = DateTime.Now;
                        ctx09W2C02.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiactx09Service.Save(ctx09W2C02);
                        #endregion Bapiactx09

                        #region Bapiaccr09
                        Bapiaccr09 ccrt09W2C022 = new Bapiaccr09();
                        ccrt09W2C022.DocId = DocID;
                        ccrt09W2C022.DocSeq = "W2C01";
                        ccrt09W2C022.DocKind = DocKind;
                        ccrt09W2C022.ItemnoAcc = "3";
                        ccrt09W2C022.Currency = PostingConst.Currency;
                        ccrt09W2C022.AmtDoccur = decimal.Parse(douVatPerdiem.ToString());
                        ccrt09W2C022.AmtBase = decimal.Parse(douSumPerdiem.ToString());

                        ccrt09W2C022.Active = true;
                        ccrt09W2C022.CreBy = 1;
                        ccrt09W2C022.CreDate = DateTime.Now;
                        ccrt09W2C022.UpdBy = 1;
                        ccrt09W2C022.UpdDate = DateTime.Now;
                        ccrt09W2C022.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccrt09W2C022);
                        #endregion Bapiaccr09
                    }
                    #endregion บริษัทปฏิบัติงาน ข้อมูลชุดที่ 2 (W2C02)

                    #region บริษัทปฏิบัติงาน ข้อมูลชุดที่ 1 (W2C01)
                    {
                        // ***********************
                        // Header
                        // ***********************
                        #region HEAD
                        Bapiache09 che09W2C01 = new Bapiache09();
                        che09W2C01.DocId = DocID;
                        che09W2C01.DocSeq = "W2C02";
                        che09W2C01.DocKind = DocKind;
                        che09W2C01.DocStatus = "N";
                        che09W2C01.BusAct = PostingConst.BusAct;
                        che09W2C01.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                        che09W2C01.CompCode = dtbHead.Rows[0]["CompanyCode"].ToString();
                        che09W2C01.DocDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09W2C01.DocKind = DocKind;
                        che09W2C01.DocType = sap.DocTypeExpICPostingFR;//DocTypeConst.DI;
                        che09W2C01.PstngDate = dtbHead.Rows[0]["PostingDate"].ToString();
                        che09W2C01.RefDocNo = SAPUIHelper.SubString(16, dtbHead.Rows[0]["DocumentNo"].ToString());
                        che09W2C01.DocAppFlag = "A";

                        che09W2C01.Active = true;
                        che09W2C01.CreBy = 1;
                        che09W2C01.CreDate = DateTime.Now;
                        che09W2C01.UpdBy = 1;
                        che09W2C01.UpdDate = DateTime.Now;
                        che09W2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiache09Service.Save(che09W2C01);
                        #endregion HEAD

                        #region Extention BRNCH
                        Bapiacextc cexHeadW2C01 = new Bapiacextc();
                        cexHeadW2C01.DocId = DocID;
                        cexHeadW2C01.DocSeq = "W2C02";
                        cexHeadW2C01.DocKind = DocKind;
                        cexHeadW2C01.Field1 = PostingConst.BRNCH;
                        cexHeadW2C01.Field2 = dtbHead.Rows[0]["BranchCodeHeader"].ToString();

                        cexHeadW2C01.Active = true;
                        cexHeadW2C01.CreBy = 1;
                        cexHeadW2C01.CreDate = DateTime.Now;
                        cexHeadW2C01.UpdBy = 1;
                        cexHeadW2C01.UpdDate = DateTime.Now;
                        cexHeadW2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexHeadW2C01);
                        #endregion Extention BRNCH

                        #region Extention Non Vat
                        Bapiacextc cexNVatW2C01 = new Bapiacextc();
                        cexNVatW2C01.DocId = DocID;
                        cexNVatW2C01.DocSeq = "W2C02";
                        cexNVatW2C01.DocKind = DocKind;
                        cexNVatW2C01.Field1 = PostingConst.VAT;
                        cexNVatW2C01.Field2 = TaxCodeConst.NO;

                        cexNVatW2C01.Active = true;
                        cexNVatW2C01.CreBy = 1;
                        cexNVatW2C01.CreDate = DateTime.Now;
                        cexNVatW2C01.UpdBy = 1;
                        cexNVatW2C01.UpdDate = DateTime.Now;
                        cexNVatW2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexNVatW2C01);
                        #endregion Extention Non Vat

                        // ***********************
                        // Debit
                        // ***********************
                        #region Bapiacar09
                        Bapiacar09 car09W2C01 = new Bapiacar09();
                        car09W2C01.DocId = DocID;
                        car09W2C01.DocSeq = "W2C02";
                        car09W2C01.DocKind = DocKind;
                        car09W2C01.ItemnoAcc = "1";

                        car09W2C01.Customer = SAPUIHelper.ConvertCompanyCodeForIC(SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).CompanyID);
                        car09W2C01.AllocNmbr = SAPUIHelper.SubString(18, dtbHead.Rows[0]["DocumentNo"].ToString());
                        car09W2C01.ItemText = SAPUIHelper.SubString50(dtbHead.Rows[0]["DescriptionDocument"].ToString());
                        car09W2C01.TaxCode = TaxCodeConst.NO;
                        car09W2C01.Pmnttrms = PostingConst.PmnttrmsIC;

                        car09W2C01.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                        car09W2C01.SpGlInd = "";

                        car09W2C01.Active = true;
                        car09W2C01.CreBy = 1;
                        car09W2C01.CreDate = DateTime.Now;
                        car09W2C01.UpdBy = 1;
                        car09W2C01.UpdDate = DateTime.Now;
                        car09W2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacar09Service.Save(car09W2C01);
                        #endregion Bapiacar09

                        #region Bapiaccr09
                        Bapiaccr09 ccr09W2C012 = new Bapiaccr09();
                        ccr09W2C012.DocId = DocID;
                        ccr09W2C012.DocSeq = "W2C02";
                        ccr09W2C012.DocKind = DocKind;
                        ccr09W2C012.ItemnoAcc = "1";
                        ccr09W2C012.Currency = PostingConst.Currency;
                        ccr09W2C012.AmtDoccur = decimal.Parse(douSumPerdiem.ToString());

                        ccr09W2C012.Active = true;
                        ccr09W2C012.CreBy = 1;
                        ccr09W2C012.CreDate = DateTime.Now;
                        ccr09W2C012.UpdBy = 1;
                        ccr09W2C012.UpdDate = DateTime.Now;
                        ccr09W2C012.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccr09W2C012);
                        #endregion Bapiaccr09

                        // ***********************
                        // Credit
                        // ***********************
                        #region Bapiacgl09
                        Bapiacgl09 cgl09C2W01 = new Bapiacgl09();
                        cgl09C2W01.DocId = DocID;
                        cgl09C2W01.DocSeq = "W2C02";
                        cgl09C2W01.DocKind = DocKind;
                        cgl09C2W01.ItemnoAcc = "2";

                        cgl09C2W01.GlAccount = ParameterServices.BAPI_SAV;
                        cgl09C2W01.AllocNmbr = SAPUIHelper.SubString18(dtbHead.Rows[0]["DocumentNo"].ToString());
                        cgl09C2W01.ItemText = "";

                        DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindDbAccountByAccountCode(cgl09C2W01.GlAccount);
                        if (account != null)
                        {
                            if (account.TaxCode != (int)TaxCodeOption.None)
                                cgl09C2W01.TaxCode = TaxCodeConst.NO;
                            else
                                cgl09C2W01.TaxCode = string.Empty;

                            if (account.InternalOrder != (int)InternalOrderOption.None)
                                cgl09C2W01.Orderid = drItemIC[0]["OrderNo"].ToString();
                            else
                                cgl09C2W01.Orderid = string.Empty;

                            if (account.SaleOrder != (int)SaleOrderOption.None)
                            {
                                cgl09C2W01.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                                cgl09C2W01.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                            }
                            else
                            {
                                cgl09C2W01.SalesOrd = string.Empty;
                                cgl09C2W01.SOrdItem = string.Empty;
                            }

                        }
                        else
                        {
                            cgl09C2W01.TaxCode = TaxCodeConst.NO;
                            cgl09C2W01.Orderid = drItemIC[0]["OrderNo"].ToString();
                            cgl09C2W01.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                            cgl09C2W01.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                        }

                        cgl09C2W01.Active = true;
                        cgl09C2W01.CreBy = 1;
                        cgl09C2W01.CreDate = DateTime.Now;
                        cgl09C2W01.UpdBy = 1;
                        cgl09C2W01.UpdDate = DateTime.Now;
                        cgl09C2W01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacgl09Service.Save(cgl09C2W01);
                        #endregion Bapiacgl09

                        #region Bapiaccr09
                        Bapiaccr09 ccrt09W2C01 = new Bapiaccr09();
                        ccrt09W2C01.DocId = DocID;
                        ccrt09W2C01.DocSeq = "W2C02";
                        ccrt09W2C01.DocKind = DocKind;
                        ccrt09W2C01.ItemnoAcc = "2";
                        ccrt09W2C01.Currency = PostingConst.Currency;
                        ccrt09W2C01.AmtDoccur = 0 - decimal.Parse(douSumPerdiem.ToString());

                        ccrt09W2C01.Active = true;
                        ccrt09W2C01.CreBy = 1;
                        ccrt09W2C01.CreDate = DateTime.Now;
                        ccrt09W2C01.UpdBy = 1;
                        ccrt09W2C01.UpdDate = DateTime.Now;
                        ccrt09W2C01.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccrt09W2C01);
                        #endregion Bapiaccr09
                    }
                    #endregion บริษัทปฏิบัติงาน ข้อมูลชุดที่ 1 (W2C01)

                    #endregion บริษัทปฏิบัติงาน ( Company's Document )

                }
                #endregion Check for have IC Case

                #region Check for Head haves Vat & WHT
                {
                    DataRow[] drHead = dtbHead.Select(" ISNULL(isVAT,0)=1 OR ISNULL(isWHT,0)=1 ", "CreDate ASC");
                    if (drHead.Length > 0) isHaveVatOrWht = true;

                    for (int i = 0; i < drHead.Length; i++)
                    {
                        intCountSEQ++;

                        //สร้างหัวตาราง
                        #region HEAD
                        Bapiache09 che09 = new Bapiache09();
                        che09.DocId = DocID;
                        che09.DocSeq = intCountSEQ.ToString("00");
                        che09.DocKind = DocKind;
                        che09.DocStatus = "N";
                        che09.BusAct = PostingConst.BusAct;
                        che09.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                        che09.CompCode = drHead[i]["CompanyCode"].ToString();
                        che09.DocDate = drHead[i]["InvoiceDate"].ToString();
                        che09.DocKind = DocKind;
                        che09.DocType = doc.DocumentType.DocumentTypeID == SCG.eAccounting.BLL.DocumentTypeID.ExpenseDomesticDocument ? sap.DocTypeExpPostingDM : sap.DocTypeExpPostingFR; //DocTypeConst.KR;
                        che09.PstngDate = drHead[i]["PostingDate"].ToString();
                        che09.ReverseDate = drHead[i]["PostingDate"].ToString();
                        che09.RefDocNo = SAPUIHelper.SubString(16, drHead[i]["InvoiceNo"].ToString());

                        che09.Active = true;
                        che09.CreBy = 1;
                        che09.CreDate = DateTime.Now;
                        che09.UpdBy = 1;
                        che09.UpdDate = DateTime.Now;
                        che09.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiache09Service.Save(che09);
                        #endregion HEAD

                        //สร้าง Extention
                        #region Extention BRNCH
                        Bapiacextc cexHead = new Bapiacextc();
                        cexHead.DocId = DocID;
                        cexHead.DocSeq = intCountSEQ.ToString("00");
                        cexHead.DocKind = DocKind;
                        cexHead.Field1 = PostingConst.BRNCH;
                        cexHead.Field2 = drHead[i]["BranchCode"].ToString();

                        cexHead.Active = true;
                        cexHead.CreBy = 1;
                        cexHead.CreDate = DateTime.Now;
                        cexHead.UpdBy = 1;
                        cexHead.UpdDate = DateTime.Now;
                        cexHead.UpdPgm = "PostingService";
                        BapiServiceProvider.BapiacextcService.Save(cexHead);
                        #endregion Extention BRNCH

                        #region Extention VAT
                        if ((bool)drHead[i]["isVAT"])
                        {
                            // ตาราง Extention
                            Bapiacextc cexVat = new Bapiacextc();
                            cexVat.DocId = DocID;
                            cexVat.DocSeq = intCountSEQ.ToString("00");
                            cexVat.DocKind = DocKind;
                            cexVat.Field1 = PostingConst.VAT;
                            cexVat.Field2 = drHead[i]["TaxCode"].ToString();

                            cexVat.Active = true;
                            cexVat.CreBy = 1;
                            cexVat.CreDate = DateTime.Now;
                            cexVat.UpdBy = 1;
                            cexVat.UpdDate = DateTime.Now;
                            cexVat.UpdPgm = "PostingService";
                            BapiServiceProvider.BapiacextcService.Save(cexVat);
                        }
                        #endregion Extention VAT

                        #region Extention WTH
                        if ((bool)drHead[i]["isWHT"])
                        {
                            // Have WHT1
                            if (drHead[i]["WHTID1"] != null && drHead[i]["WHTID1"].ToString() != "")
                            {
                                double BaseAmount1 = 0;
                                double WHTAmount1 = 0;

                                double.TryParse(drHead[i]["BaseAmount1"].ToString(), out BaseAmount1);
                                double.TryParse(drHead[i]["WHTAmount1"].ToString(), out WHTAmount1);

                                if (BaseAmount1 == 0 || WHTAmount1 == 0)
                                { }
                                else
                                {
                                    #region สร้าง Extention WTH1
                                    short WHTCode = SS.Standard.Utilities.Utilities.ParseShort(drHead[i]["WHTID1"].ToString());
                                    DbWithHoldingTax dbWHTCode = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindByIdentity(WHTCode);

                                    short WHTType = SS.Standard.Utilities.Utilities.ParseShort(drHead[i]["WHTTypeID1"].ToString());
                                    DbWithHoldingTaxType dbWHTType = ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.FindByIdentity(WHTType);

                                    Bapiacextc cexWHT = new Bapiacextc();
                                    cexWHT.DocId = DocID;
                                    cexWHT.DocSeq = intCountSEQ.ToString("00");
                                    cexWHT.DocKind = DocKind;
                                    cexWHT.Field1 = "WTH1";
                                    cexWHT.Field2 = (dbWHTType != null) ? dbWHTType.WhtTypeCode : "";
                                    cexWHT.Field3 = (dbWHTCode != null) ? dbWHTCode.WhtCode : "";
                                    cexWHT.Field4 = drHead[i]["BaseAmount1"].ToString() + " | " + drHead[i]["WHTAmount1"].ToString();

                                    cexWHT.Active = true;
                                    cexWHT.CreBy = 1;
                                    cexWHT.CreDate = DateTime.Now;
                                    cexWHT.UpdBy = 1;
                                    cexWHT.UpdDate = DateTime.Now;
                                    cexWHT.UpdPgm = "PostingService";
                                    BapiServiceProvider.BapiacextcService.Save(cexWHT);
                                    #endregion สร้าง Extention WTH1
                                }
                            }
                            // Have WHT2
                            if (drHead[i]["WHTID2"] != null && drHead[i]["WHTID2"].ToString() != "")
                            {
                                double BaseAmount2 = 0;
                                double WHTAmount2 = 0;

                                double.TryParse(drHead[i]["BaseAmount2"].ToString(), out BaseAmount2);
                                double.TryParse(drHead[i]["WHTAmount2"].ToString(), out WHTAmount2);

                                if (BaseAmount2 == 0 || WHTAmount2 == 0)
                                { }
                                else
                                {
                                    #region สร้าง Extention WTH2
                                    short WHTCode = SS.Standard.Utilities.Utilities.ParseShort(drHead[i]["WHTID2"].ToString());
                                    DbWithHoldingTax dbWHTCode = ScgDbQueryProvider.DbWithHoldingTaxQuery.FindByIdentity(WHTCode);

                                    short WHTType = SS.Standard.Utilities.Utilities.ParseShort(drHead[i]["WHTTypeID2"].ToString());
                                    DbWithHoldingTaxType dbWHTType = ScgDbQueryProvider.DbWithHoldingTaxTypeQuery.FindByIdentity(WHTType);

                                    Bapiacextc cexWHT = new Bapiacextc();
                                    cexWHT.DocId = DocID;
                                    cexWHT.DocSeq = intCountSEQ.ToString("00");
                                    cexWHT.DocKind = DocKind;
                                    cexWHT.Field1 = "WTH2";
                                    cexWHT.Field2 = (dbWHTType != null) ? dbWHTType.WhtTypeCode : "";
                                    cexWHT.Field3 = (dbWHTCode != null) ? dbWHTCode.WhtCode : "";
                                    cexWHT.Field4 = drHead[i]["BaseAmount2"].ToString() + " | " + drHead[i]["WHTAmount2"].ToString();

                                    cexWHT.Active = true;
                                    cexWHT.CreBy = 1;
                                    cexWHT.CreDate = DateTime.Now;
                                    cexWHT.UpdBy = 1;
                                    cexWHT.UpdDate = DateTime.Now;
                                    cexWHT.UpdPgm = "PostingService";
                                    BapiServiceProvider.BapiacextcService.Save(cexWHT);
                                    #endregion สร้าง Extention WTH2
                                }
                            }
                        }
                        #endregion Extention WTH

                        #region ตาราง Verdor VAT & WHT
                        short vendor = SS.Standard.Utilities.Utilities.ParseShort(drHead[i]["VendorID"].ToString());
                        DbVendor dbVendor = ScgDbQueryProvider.DbVendorQuery.FindByIdentity(vendor);

                        Bapiacpa09 cpa09 = new Bapiacpa09();
                        cpa09.DocId = DocID;
                        cpa09.DocSeq = intCountSEQ.ToString("00");
                        cpa09.DocKind = DocKind;

                        //modify by tom 22/07/2009, Vendor Name 100 digits
                        string strName = "";
                        strName = drHead[i]["VendorName"].ToString();
                        cpa09.Name = SAPUIHelper.CutLeft(ref strName, 35);
                        //strName         = strName.Replace(cpa09.Name,"");
                        cpa09.Name2 = SAPUIHelper.CutLeft(ref strName, 35);
                        //strName         = strName.Replace(cpa09.Name2, "");
                        cpa09.Name3 = SAPUIHelper.CutLeft(ref strName, 35);
                        //strName         = strName.Replace(cpa09.Name3, "");
                        //cpa09.Name4     = SAPUIHelper.CutLeft(ref strName, 35);

                        cpa09.PostlCode = SAPUIHelper.SubString(10, drHead[i]["PostalCode"].ToString());
                        cpa09.City = drHead[i]["City"].ToString();
                        if (cpa09.City.Length > 35)
                            cpa09.City = cpa09.City.Substring(0, 35);

                        cpa09.Country = drHead[i]["Country"].ToString();
                        if (cpa09.Country.Length > 3)
                            cpa09.Country = cpa09.Country.Substring(0, 3);

                        cpa09.Street = drHead[i]["Street"].ToString();
                        if (cpa09.Street.Length > 35)
                            cpa09.Street = cpa09.Street.Substring(0, 35);

                        //if (drHead[i]["VendorTaxCode"].ToString().Length == 13)
                        //    cpa09.TaxNo1 = drHead[i]["VendorTaxCode"].ToString();
                        //else if (drHead[i]["VendorTaxCode"].ToString().Length == 10)
                        //    cpa09.TaxNo2 = drHead[i]["VendorTaxCode"].ToString();
                        //else
                        //    cpa09.TaxNo1 = drHead[i]["VendorTaxCode"].ToString();

                        if (drHead[i]["VendorTaxCode"].ToString().Length == 13)
                            cpa09.TaxNo3 = drHead[i]["VendorTaxCode"].ToString();

                        cpa09.TaxNo4 = drHead[i]["VendorBranch"].ToString();

                        cpa09.Active = true;
                        cpa09.CreBy = 1;
                        cpa09.CreDate = DateTime.Now;
                        cpa09.UpdBy = 1;
                        cpa09.UpdDate = DateTime.Now;
                        cpa09.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacpa09Service.Save(cpa09);
                        #endregion ตาราง Verdor VAT & WHT

                        //Loop For ITEM
                        #region Create ITEM
                        int intItemCount = 0;
                        DataRow[] drItem = dtbItem.Select(" InvoiceID = '" + drHead[i]["InvoiceID"].ToString() + "' ");
                        if (drItem.Length > 0)
                        {
                            #region Credit Into PCADVCL

                            intItemCount++;
                            douSumNetAmount += double.Parse(drHead[i]["NetAmount"].ToString());

                            #region Bapiacap09
                            Bapiacap09 cap09 = new Bapiacap09();
                            cap09.DocId = DocID;
                            cap09.DocSeq = intCountSEQ.ToString("00");
                            cap09.DocKind = DocKind;
                            cap09.ItemnoAcc = intItemCount.ToString();
                            cap09.VendorNo = PostingConst.PCADVCL;
                            cap09.Pmnttrms = PostingConst.Pmnttrms;
                            cap09.BlineDate = drHead[i]["BaseLineDate"].ToString();
                            cap09.PymtMeth = drHead[i]["PaymentMethodCode"].ToString();
                            cap09.AllocNmbr = drHead[i]["DocumentNo"].ToString();
                            cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);
                            cap09.ItemText = drHead[i]["Description"].ToString();
                            cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                            cap09.Businessplace = drHead[i]["BranchCode"].ToString();

                            if ((bool)drHead[i]["isVAT"])
                                cap09.TaxCode = drHead[i]["TaxCode"].ToString();
                            else
                                cap09.TaxCode = TaxCodeConst.NV;

                            cap09.Active = true;
                            cap09.CreBy = 1;
                            cap09.CreDate = DateTime.Now;
                            cap09.UpdBy = 1;
                            cap09.UpdDate = DateTime.Now;
                            cap09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacap09Service.Save(cap09);
                            #endregion Bapiacap09

                            #region Bapiaccr09
                            Bapiaccr09 ccr09 = new Bapiaccr09();
                            ccr09.DocId = DocID;
                            ccr09.DocSeq = intCountSEQ.ToString("00");
                            ccr09.DocKind = DocKind;
                            ccr09.ItemnoAcc = intItemCount.ToString();
                            ccr09.Currency = PostingConst.Currency;

                            if ((bool)drHead[i]["isWHT"])
                                ccr09.AmtDoccur = 0 - (decimal.Parse(drHead[i]["NetAmount"].ToString()) + decimal.Parse(drHead[i]["WHTAmount"].ToString()));
                            else
                                ccr09.AmtDoccur = 0 - decimal.Parse(drHead[i]["NetAmount"].ToString());

                            ccr09.Active = true;
                            ccr09.CreBy = 1;
                            ccr09.CreDate = DateTime.Now;
                            ccr09.UpdBy = 1;
                            ccr09.UpdDate = DateTime.Now;
                            ccr09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                            #endregion Bapiaccr09

                            #endregion Credit

                            #region Debit Into CGL or CTX
                            // Into CGL
                            for (int j = 0; j < drItem.Length; j++)
                            {
                                intItemCount++;

                                #region Bapiacgl09
                                Bapiacgl09 cgl09 = new Bapiacgl09();
                                cgl09.DocId = DocID;
                                cgl09.DocSeq = intCountSEQ.ToString("00");
                                cgl09.DocKind = DocKind;
                                cgl09.ItemnoAcc = intItemCount.ToString();
                                cgl09.GlAccount = drItem[j]["AccountCode"].ToString();

                                cgl09.ItemText = drItem[j]["Description"].ToString();
                                cgl09.ItemText = SAPUIHelper.SubString50(cgl09.ItemText);
                                cgl09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(drItem[j]["RequesterID"].ToString())).EmployeeName;
                                cgl09.AllocNmbr = SAPUIHelper.SubString18(cgl09.AllocNmbr);

                                if (drItem[j]["SpecialGL"].ToString() == ParameterServices.BAPI_SpecialGLType2)
                                {
                                    if (int.Parse(drItem[j]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                        cgl09.TaxCode = string.Empty;
                                    else
                                        cgl09.TaxCode = drItem[j]["TaxCode"].ToString();
                                }
                                else if ((bool)drItem[j]["isVAT"])
                                {
                                    if (int.Parse(drItem[j]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                        cgl09.TaxCode = string.Empty;
                                    else
                                        cgl09.TaxCode = drItem[j]["TaxCode"].ToString();
                                }
                                else
                                {
                                    if (int.Parse(drItem[j]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                        cgl09.TaxCode = string.Empty;
                                    else
                                        cgl09.TaxCode = TaxCodeConst.NV;
                                }

                                if (int.Parse(drItem[j]["CostCenterOption"].ToString()) == (int)CostCenterOption.None)
                                    cgl09.Costcenter = string.Empty;
                                else
                                    cgl09.Costcenter = drItem[j]["CostCenterCode"].ToString();

                                if (int.Parse(drItem[j]["InternalOrderOption"].ToString()) != (int)InternalOrderOption.None)
                                    cgl09.Orderid = drItem[j]["OrderNo"].ToString();
                                else
                                    cgl09.Orderid = string.Empty;


                                if (int.Parse(drItem[j]["SaleOrderOption"].ToString()) != (int)SaleOrderOption.None)
                                {
                                    cgl09.SalesOrd = SAPUIHelper.PadLeftString(10, drItem[j]["SaleOrder"].ToString());
                                    cgl09.SOrdItem = drItem[j]["SaleItem"].ToString();
                                }
                                else
                                {
                                    cgl09.SalesOrd = string.Empty;
                                    cgl09.SOrdItem = string.Empty;
                                }

                                cgl09.Active = true;
                                cgl09.CreBy = 1;
                                cgl09.CreDate = DateTime.Now;
                                cgl09.UpdBy = 1;
                                cgl09.UpdDate = DateTime.Now;
                                cgl09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacgl09Service.Save(cgl09);
                                #endregion Bapiacgl09

                                #region Bapiaccr09
                                Bapiaccr09 ccrt09 = new Bapiaccr09();
                                ccrt09.DocId = DocID;
                                ccrt09.DocSeq = intCountSEQ.ToString("00");
                                ccrt09.DocKind = DocKind;
                                ccrt09.ItemnoAcc = intItemCount.ToString();
                                ccrt09.Currency = PostingConst.Currency;
                                ccrt09.AmtDoccur = decimal.Parse(drItem[j]["TotalBaseAmountItem"].ToString());
                                //ccrt09.ExchRate = decimal.Parse(drItem[j]["ExchangeRate"].ToString());

                                ccrt09.Active = true;
                                ccrt09.CreBy = 1;
                                ccrt09.CreDate = DateTime.Now;
                                ccrt09.UpdBy = 1;
                                ccrt09.UpdDate = DateTime.Now;
                                ccrt09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccrt09);
                                #endregion Bapiaccr09
                            }
                            // If have VAT Into CTX
                            if ((bool)drHead[i]["isVAT"])
                            {
                                intItemCount++;

                                #region Bapiactx09
                                Bapiactx09 ctx09 = new Bapiactx09();
                                ctx09.DocId = DocID;
                                ctx09.DocSeq = intCountSEQ.ToString("00");
                                ctx09.DocKind = DocKind;
                                ctx09.ItemnoAcc = intItemCount.ToString();
                                ctx09.GlAccount = drHead[i]["TaxGL"].ToString();
                                ctx09.TaxCode = drHead[i]["TaxCode"].ToString();

                                ctx09.Active = true;
                                ctx09.CreBy = 1;
                                ctx09.CreDate = DateTime.Now;
                                ctx09.UpdBy = 1;
                                ctx09.UpdDate = DateTime.Now;
                                ctx09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiactx09Service.Save(ctx09);
                                #endregion Bapiactx09

                                #region Bapiaccr09
                                Bapiaccr09 ccrt09 = new Bapiaccr09();
                                ccrt09.DocId = DocID;
                                ccrt09.DocSeq = intCountSEQ.ToString("00");
                                ccrt09.DocKind = DocKind;
                                ccrt09.ItemnoAcc = intItemCount.ToString();
                                ccrt09.Currency = PostingConst.Currency;
                                ccrt09.AmtDoccur = decimal.Parse(drHead[i]["VatAmount"].ToString());
                                ccrt09.AmtBase = decimal.Parse(drHead[i]["TotalAmount"].ToString());

                                ccrt09.Active = true;
                                ccrt09.CreBy = 1;
                                ccrt09.CreDate = DateTime.Now;
                                ccrt09.UpdBy = 1;
                                ccrt09.UpdDate = DateTime.Now;
                                ccrt09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccrt09);
                                #endregion Bapiaccr09
                            }
                            #endregion Debit Into
                        }
                        #endregion Create ITEM
                    }
                }
                #endregion Check for Head haves Vat & WHT

                // คู่สรุป
                #region Check for Head haves not Vat & not WHT
                {
                    #region สร้างหัวตาราง

                    //สร้างหัวตาราง
                    #region HEAD
                    Bapiache09 che09 = new Bapiache09();
                    che09.DocId = DocID;
                    che09.DocSeq = "M";
                    che09.DocKind = DocKind;
                    che09.DocStatus = "N";
                    che09.BusAct = PostingConst.BusAct;
                    che09.Username = sap.UserCPIC;//PostingConst.UserCPIC;
                    che09.CompCode = dtbHead.Rows[0]["CompanyCode"].ToString();
                    che09.DocDate = dtbHead.Rows[0]["PostingDate"].ToString();
                    che09.DocKind = DocKind;
                    che09.DocType = doc.DocumentType.DocumentTypeID == SCG.eAccounting.BLL.DocumentTypeID.ExpenseDomesticDocument ? sap.DocTypeExpPostingDM : sap.DocTypeExpPostingFR; //DocTypeConst.KR;
                    che09.PstngDate = dtbHead.Rows[0]["PostingDate"].ToString();
                    che09.ReverseDate = dtbHead.Rows[0]["PostingDate"].ToString();
                    che09.RefDocNo = SAPUIHelper.SubString(16, dtbHead.Rows[0]["DocumentNo"].ToString());

                    if (dtbHead.Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                        che09.DocAppFlag = "A";
                    else
                        che09.DocAppFlag = "V";

                    che09.Active = true;
                    che09.CreBy = 1;
                    che09.CreDate = DateTime.Now;
                    che09.UpdBy = 1;
                    che09.UpdDate = DateTime.Now;
                    che09.UpdPgm = "PostingService";
                    BapiServiceProvider.Bapiache09Service.Save(che09);
                    #endregion HEAD

                    //สร้าง Extention
                    #region Extention BRNCH
                    Bapiacextc cexHead = new Bapiacextc();
                    cexHead.DocId = DocID;
                    cexHead.DocSeq = "M";
                    cexHead.DocKind = DocKind;
                    cexHead.Field1 = PostingConst.BRNCH;
                    cexHead.Field2 = dtbHead.Rows[0]["BranchCodeHeader"].ToString();

                    cexHead.Active = true;
                    cexHead.CreBy = 1;
                    cexHead.CreDate = DateTime.Now;
                    cexHead.UpdBy = 1;
                    cexHead.UpdDate = DateTime.Now;
                    cexHead.UpdPgm = "PostingService";
                    BapiServiceProvider.BapiacextcService.Save(cexHead);
                    #endregion Extention BRNCH

                    #region Extention Non Vat
                    Bapiacextc cexNVat = new Bapiacextc();
                    cexNVat.DocId = DocID;
                    cexNVat.DocSeq = "M";
                    cexNVat.DocKind = DocKind;
                    cexNVat.Field1 = PostingConst.VAT;
                    cexNVat.Field2 = TaxCodeConst.NV;

                    cexNVat.Active = true;
                    cexNVat.CreBy = 1;
                    cexNVat.CreDate = DateTime.Now;
                    cexNVat.UpdBy = 1;
                    cexNVat.UpdDate = DateTime.Now;
                    cexNVat.UpdPgm = "PostingService";
                    BapiServiceProvider.BapiacextcService.Save(cexNVat);
                    #endregion Extention Non Vat

                    #endregion สร้างหัวตาราง

                    int intItemCount = 0;

                    // ถ้ามี Invoice ที่มี VAT OR WHT
                    // ต้องลงบัญชี PCADVCL ในตาราง AP ฝั่ง Debit < ไม่ติดลบ >
                    // if intCountSEQ>0 แสดงว่าที่ Row ที่มี Vat นะครับ
                    #region ส่วนของ Debit -->> PCADVCL
                    if (intCountSEQ != 0)
                    {
                        intItemCount++;

                        #region Bapiacap09
                        Bapiacap09 cap09 = new Bapiacap09();
                        cap09.DocId = DocID;
                        cap09.DocSeq = "M";
                        cap09.DocKind = DocKind;
                        cap09.ItemnoAcc = intItemCount.ToString();
                        cap09.VendorNo = PostingConst.PCADVCL;
                        cap09.Pmnttrms = PostingConst.Pmnttrms;
                        cap09.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                        cap09.PymtMeth = dtbHead.Rows[0]["PaymentMethodCode"].ToString();
                        cap09.AllocNmbr = dtbHead.Rows[0]["DocumentNo"].ToString();
                        cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);
                        cap09.ItemText = dtbHead.Rows[0]["DescriptionDocument"].ToString();
                        cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                        cap09.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                        cap09.TaxCode = TaxCodeConst.NV;

                        if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                        {
                            cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                        }

                        cap09.Active = true;
                        cap09.CreBy = 1;
                        cap09.CreDate = DateTime.Now;
                        cap09.UpdBy = 1;
                        cap09.UpdDate = DateTime.Now;
                        cap09.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacap09Service.Save(cap09);
                        #endregion Bapiacap09

                        #region Bapiaccr09
                        Bapiaccr09 ccr09 = new Bapiaccr09();
                        ccr09.DocId = DocID;
                        ccr09.DocSeq = "M";
                        ccr09.DocKind = DocKind;
                        ccr09.ItemnoAcc = intItemCount.ToString();

                        if (!repOffice)
                        {
                            ccr09.Currency = PostingConst.Currency;
                            ccr09.AmtDoccur = decimal.Parse(douSumNetAmount.ToString());
                            if (isHaveVatOrWht && isHaveAdvance)
                                ccr09.DiscBase = ccr09.AmtDoccur;
                        }
                        else
                        {
                            ccr09.Currency = mainCurrencySymbol;
                            ccr09.AmtDoccur = decimal.Parse(douSumNetAmountMainCurrency.ToString());
                            ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());
                        }

                        ccr09.Active = true;
                        ccr09.CreBy = 1;
                        ccr09.CreDate = DateTime.Now;
                        ccr09.UpdBy = 1;
                        ccr09.UpdDate = DateTime.Now;
                        ccr09.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                        #endregion Bapiaccr09
                    }
                    #endregion ส่วนของ Debit -->> PCADVCL

                    #region ส่วนของ Credit

                    if (!repOffice)
                    {
                        for (int i = 0; i < drItemAr.Length; i++)
                            douSumNetAmount += double.Parse(drItemAr[i]["TotalBaseAmountItem"].ToString());
                        for (int i = 0; i < drItemGl.Length; i++)
                            douSumNetAmount += double.Parse(drItemGl[i]["TotalBaseAmountItem"].ToString());
                        if (isHaveICCase)
                        {
                            for (int i = 0; i < drItemIC.Length; i++)
                                douSumNetAmount += double.Parse(drItemIC[i]["AmountItem"].ToString());
                        }

                        double douTotalExpense = double.Parse(dtbHead.Rows[0]["TotalExpense"].ToString());
                        double douTotalAdvance = double.Parse(dtbHead.Rows[0]["TotalAdvance"].ToString());

                        // กรณีที่ 1 : ไม่อ้างอิงใบ Advance
                        if (dtbAdvance.Rows.Count <= 0)
                        {
                            intItemCount++;

                            #region Bapiacap09
                            Bapiacap09 cap09 = new Bapiacap09();
                            cap09.DocId = DocID;
                            cap09.DocSeq = "M";
                            cap09.DocKind = DocKind;
                            cap09.ItemnoAcc = intItemCount.ToString();

                            // กรณีรับเงินสด
                            if (dtbHead.Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                            {
                                cap09.VendorNo = dtbHead.Rows[0]["PBCode"].ToString();
                                cap09.PmntBlock = PostingConst.PmntBlock;
                                cap09.AllocNmbr = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).EmployeeName;
                            }
                            else
                            {
                                cap09.VendorNo = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).VendorCode;
                                cap09.AllocNmbr = dtbHead.Rows[0]["DocumentNo"].ToString();
                            }

                            cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);

                            cap09.Pmnttrms = PostingConst.Pmnttrms;
                            cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();
                            cap09.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                            cap09.PymtMeth = dtbHead.Rows[0]["PaymentMethodCode"].ToString();

                            cap09.ItemText = dtbHead.Rows[0]["DescriptionDocument"].ToString();
                            cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                            cap09.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                            cap09.TaxCode = TaxCodeConst.NV;

                            if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                            }

                            cap09.Active = true;
                            cap09.CreBy = 1;
                            cap09.CreDate = DateTime.Now;
                            cap09.UpdBy = 1;
                            cap09.UpdDate = DateTime.Now;
                            cap09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacap09Service.Save(cap09);
                            #endregion Bapiacap09

                            #region Bapiaccr09
                            Bapiaccr09 ccr09 = new Bapiaccr09();
                            ccr09.DocId = DocID;
                            ccr09.DocSeq = "M";
                            ccr09.DocKind = DocKind;
                            ccr09.ItemnoAcc = intItemCount.ToString();
                            ccr09.Currency = PostingConst.Currency;
                            ccr09.AmtDoccur = 0 - decimal.Parse(douSumNetAmount.ToString());

                            ccr09.Active = true;
                            ccr09.CreBy = 1;
                            ccr09.CreDate = DateTime.Now;
                            ccr09.UpdBy = 1;
                            ccr09.UpdDate = DateTime.Now;
                            ccr09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                            #endregion Bapiaccr09
                        }
                        // กรณีที่ 2 : Expense = Advance
                        else if (douTotalExpense == douTotalAdvance)
                        {
                            for (int i = 0; i < dtbAdvance.Rows.Count; i++)
                            {
                                intItemCount++;

                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dtbAdvance.Rows[0]["RequesterID"].ToString())).VendorCode;
                                cap09.Pmnttrms = PostingConst.Pmnttrms;
                                cap09.BlineDate = dtbAdvance.Rows[i]["BaseLineDate"].ToString();
                                cap09.PymtMeth = dtbAdvance.Rows[i]["PaymentMethodCode"].ToString();
                                cap09.AllocNmbr = dtbAdvance.Rows[i]["AdvanceNo"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);

                                cap09.ItemText = dtbAdvance.Rows[i]["DescriptionAdvance"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = dtbAdvance.Rows[i]["BranchCode"].ToString();
                                cap09.TaxCode = TaxCodeConst.NV;
                                cap09.SpGlInd = SpGlIndConst.D;

                                if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                                {
                                    cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                                }

                                cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacap09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = PostingConst.Currency;
                                ccr09.AmtDoccur = 0 - decimal.Parse(dtbAdvance.Rows[i]["ExpenseAmount"].ToString());
                                if (isHaveVatOrWht && isHaveAdvance)
                                    ccr09.DiscBase = ccr09.AmtDoccur;

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }
                        }
                        // กรณีที่ 3 : Expense < Advance
                        else if (douTotalExpense < douTotalAdvance)
                        {
                            for (int i = 0; i < dtbAdvance.Rows.Count; i++)
                            {
                                intItemCount++;

                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dtbAdvance.Rows[0]["RequesterID"].ToString())).VendorCode;
                                cap09.Pmnttrms = PostingConst.Pmnttrms;
                                cap09.BlineDate = dtbAdvance.Rows[i]["BaseLineDate"].ToString();
                                cap09.PymtMeth = dtbAdvance.Rows[i]["PaymentMethodCode"].ToString();
                                cap09.AllocNmbr = dtbAdvance.Rows[i]["AdvanceNo"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);

                                cap09.ItemText = dtbAdvance.Rows[i]["DescriptionAdvance"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = dtbAdvance.Rows[i]["BranchCode"].ToString();
                                cap09.TaxCode = TaxCodeConst.NV;
                                cap09.SpGlInd = SpGlIndConst.D;

                                if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                                {
                                    cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                                }

                                cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacap09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = PostingConst.Currency;
                                ccr09.AmtDoccur = 0 - decimal.Parse(dtbAdvance.Rows[i]["ExpenseAmount"].ToString());
                                if (isHaveVatOrWht && isHaveAdvance)
                                    ccr09.DiscBase = ccr09.AmtDoccur;

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }
                        }
                        // กรณีที่ 4 : Expense > Advance
                        else if (douTotalExpense > douTotalAdvance)
                        {
                            for (int i = 0; i < dtbAdvance.Rows.Count; i++)
                            {
                                intItemCount++;

                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dtbAdvance.Rows[0]["RequesterID"].ToString())).VendorCode;
                                cap09.Pmnttrms = PostingConst.Pmnttrms;
                                cap09.BlineDate = dtbAdvance.Rows[i]["BaseLineDate"].ToString();
                                cap09.PymtMeth = dtbAdvance.Rows[i]["PaymentMethodCode"].ToString();
                                cap09.AllocNmbr = dtbAdvance.Rows[i]["AdvanceNo"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);
                                cap09.ItemText = dtbAdvance.Rows[i]["DescriptionAdvance"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = dtbAdvance.Rows[i]["BranchCode"].ToString();
                                cap09.TaxCode = TaxCodeConst.NV;
                                cap09.SpGlInd = SpGlIndConst.D;

                                if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                                {
                                    cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                                }

                                cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacap09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = PostingConst.Currency;
                                ccr09.AmtDoccur = 0 - decimal.Parse(dtbAdvance.Rows[i]["ExpenseAmount"].ToString());
                                if (isHaveVatOrWht && isHaveAdvance)
                                    ccr09.DiscBase = ccr09.AmtDoccur;

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }

                            intItemCount++;
                            // ส่วนเกินตั้งพนักงานเป็นเจ้าหนี้
                            #region Bapiacap09
                            Bapiacap09 cap109 = new Bapiacap09();
                            cap109.DocId = DocID;
                            cap109.DocSeq = "M";
                            cap109.DocKind = DocKind;
                            cap109.ItemnoAcc = intItemCount.ToString();

                            // กรณีรับเงินสด
                            if (dtbHead.Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                            {
                                cap109.VendorNo = dtbHead.Rows[0]["PBCode"].ToString();
                                cap109.PmntBlock = PostingConst.PmntBlock;
                                cap109.AllocNmbr = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).EmployeeName;
                            }
                            else
                            {
                                cap109.VendorNo = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).VendorCode;
                                cap109.AllocNmbr = dtbHead.Rows[0]["DocumentNo"].ToString();
                            }

                            cap109.AllocNmbr = SAPUIHelper.SubString18(cap109.AllocNmbr);

                            cap109.Pmnttrms = PostingConst.Pmnttrms;
                            cap109.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();
                            cap109.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                            cap109.PymtMeth = dtbHead.Rows[0]["PaymentMethodCode"].ToString();
                            cap109.ItemText = dtbHead.Rows[0]["DescriptionDocument"].ToString();
                            cap109.ItemText = SAPUIHelper.SubString50(cap109.ItemText);
                            cap109.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                            cap109.TaxCode = TaxCodeConst.NV;

                            if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                cap109.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                            }

                            cap109.Active = true;
                            cap109.CreBy = 1;
                            cap109.CreDate = DateTime.Now;
                            cap109.UpdBy = 1;
                            cap109.UpdDate = DateTime.Now;
                            cap109.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacap09Service.Save(cap109);
                            #endregion Bapiacap09

                            #region Bapiaccr09
                            double douAmountNew = douTotalExpense - douTotalAdvance;

                            Bapiaccr09 ccr109 = new Bapiaccr09();
                            ccr109.DocId = DocID;
                            ccr109.DocSeq = "M";
                            ccr109.DocKind = DocKind;
                            ccr109.ItemnoAcc = intItemCount.ToString();
                            ccr109.Currency = PostingConst.Currency;
                            ccr109.AmtDoccur = 0 - decimal.Parse(douAmountNew.ToString());
                            if (isHaveVatOrWht && isHaveAdvance)
                                ccr109.DiscBase = ccr109.AmtDoccur;

                            ccr109.Active = true;
                            ccr109.CreBy = 1;
                            ccr109.CreDate = DateTime.Now;
                            ccr109.UpdBy = 1;
                            ccr109.UpdDate = DateTime.Now;
                            ccr109.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccr109);
                            #endregion Bapiaccr09
                        }
                    }
                    else //case rep office
                    {
                        for (int i = 0; i < drItemAr.Length; i++)
                            douSumNetAmountMainCurrency += double.Parse(drItemAr[i]["TotalBaseAmountMainCurrencyItem"].ToString());
                        for (int i = 0; i < drItemGl.Length; i++)
                            douSumNetAmountMainCurrency += double.Parse(drItemGl[i]["TotalBaseAmountMainCurrencyItem"].ToString());

                        double douTotalExpenseMainCurrency = double.Parse(dtbHead.Rows[0]["TotalExpenseMainCurrency"].ToString());
                        double douTotalAdvanceMainCurrency = double.Parse(dtbHead.Rows[0]["TotalAdvanceMainCurrency"].ToString());

                        // กรณีที่ 1 : ไม่อ้างอิงใบ Advance
                        if (dtbAdvance.Rows.Count <= 0)
                        {
                            intItemCount++;

                            #region Bapiacap09
                            Bapiacap09 cap09 = new Bapiacap09();
                            cap09.DocId = DocID;
                            cap09.DocSeq = "M";
                            cap09.DocKind = DocKind;
                            cap09.ItemnoAcc = intItemCount.ToString();

                            // กรณีรับเงินสด
                            if (dtbHead.Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                            {
                                cap09.VendorNo = dtbHead.Rows[0]["PBCode"].ToString();
                                cap09.PmntBlock = PostingConst.PmntBlock;
                                cap09.AllocNmbr = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).EmployeeName;
                            }
                            else
                            {
                                cap09.VendorNo = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).VendorCode;
                                cap09.AllocNmbr = dtbHead.Rows[0]["DocumentNo"].ToString();
                            }

                            cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);

                            cap09.Pmnttrms = PostingConst.Pmnttrms;
                            cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();
                            cap09.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                            cap09.PymtMeth = dtbHead.Rows[0]["PaymentMethodCode"].ToString();

                            cap09.ItemText = dtbHead.Rows[0]["DescriptionDocument"].ToString();
                            cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                            cap09.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                            cap09.TaxCode = TaxCodeConst.NV;

                            if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                            }

                            cap09.Active = true;
                            cap09.CreBy = 1;
                            cap09.CreDate = DateTime.Now;
                            cap09.UpdBy = 1;
                            cap09.UpdDate = DateTime.Now;
                            cap09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacap09Service.Save(cap09);
                            #endregion Bapiacap09

                            #region Bapiaccr09
                            Bapiaccr09 ccr09 = new Bapiaccr09();
                            ccr09.DocId = DocID;
                            ccr09.DocSeq = "M";
                            ccr09.DocKind = DocKind;
                            ccr09.ItemnoAcc = intItemCount.ToString();
                            ccr09.Currency = mainCurrencySymbol;
                            ccr09.AmtDoccur = 0 - decimal.Parse(douSumNetAmountMainCurrency.ToString());
                            ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                            ccr09.Active = true;
                            ccr09.CreBy = 1;
                            ccr09.CreDate = DateTime.Now;
                            ccr09.UpdBy = 1;
                            ccr09.UpdDate = DateTime.Now;
                            ccr09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                            #endregion Bapiaccr09
                        }
                        // กรณีที่ 2 : Expense = Advance
                        else if (douTotalExpenseMainCurrency == douTotalAdvanceMainCurrency)
                        {
                            for (int i = 0; i < dtbAdvance.Rows.Count; i++)
                            {
                                intItemCount++;

                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dtbAdvance.Rows[0]["RequesterID"].ToString())).VendorCode;
                                cap09.Pmnttrms = PostingConst.Pmnttrms;
                                cap09.BlineDate = dtbAdvance.Rows[i]["BaseLineDate"].ToString();
                                cap09.PymtMeth = dtbAdvance.Rows[i]["PaymentMethodCode"].ToString();
                                cap09.AllocNmbr = dtbAdvance.Rows[i]["AdvanceNo"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);

                                cap09.ItemText = dtbAdvance.Rows[i]["DescriptionAdvance"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = dtbAdvance.Rows[i]["BranchCode"].ToString();
                                cap09.TaxCode = TaxCodeConst.NV;
                                cap09.SpGlInd = SpGlIndConst.D;

                                if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                                {
                                    cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                                }

                                cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacap09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = mainCurrencySymbol;
                                ccr09.AmtDoccur = 0 - decimal.Parse(string.IsNullOrEmpty(dtbAdvance.Rows[i]["ExpenseAmountMainCurrency"].ToString()) ? "0" : dtbAdvance.Rows[i]["ExpenseAmountMainCurrency"].ToString());
                                ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }
                        }
                        // กรณีที่ 3 : Expense < Advance
                        else if (douTotalExpenseMainCurrency < douTotalAdvanceMainCurrency)
                        {
                            for (int i = 0; i < dtbAdvance.Rows.Count; i++)
                            {
                                intItemCount++;

                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dtbAdvance.Rows[0]["RequesterID"].ToString())).VendorCode;
                                cap09.Pmnttrms = PostingConst.Pmnttrms;
                                cap09.BlineDate = dtbAdvance.Rows[i]["BaseLineDate"].ToString();
                                cap09.PymtMeth = dtbAdvance.Rows[i]["PaymentMethodCode"].ToString();
                                cap09.AllocNmbr = dtbAdvance.Rows[i]["AdvanceNo"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);

                                cap09.ItemText = dtbAdvance.Rows[i]["DescriptionAdvance"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = dtbAdvance.Rows[i]["BranchCode"].ToString();
                                cap09.TaxCode = TaxCodeConst.NV;
                                cap09.SpGlInd = SpGlIndConst.D;

                                if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                                {
                                    cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                                }

                                cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacap09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = mainCurrencySymbol;
                                ccr09.AmtDoccur = 0 - decimal.Parse(string.IsNullOrEmpty(dtbAdvance.Rows[i]["ExpenseAmountMainCurrency"].ToString()) ? "0" : dtbAdvance.Rows[i]["ExpenseAmountMainCurrency"].ToString());
                                ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());
                                //if (isHaveVatOrWht && isHaveAdvance)
                                //    ccr09.DiscBase = ccr09.AmtDoccur;

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }
                        }
                        // กรณีที่ 4 : Expense > Advance
                        else if (douTotalExpenseMainCurrency > douTotalAdvanceMainCurrency)
                        {
                            for (int i = 0; i < dtbAdvance.Rows.Count; i++)
                            {
                                intItemCount++;

                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = SAPUIHelper.GetEmployee(long.Parse(dtbAdvance.Rows[0]["RequesterID"].ToString())).VendorCode;
                                cap09.Pmnttrms = PostingConst.Pmnttrms;
                                cap09.BlineDate = dtbAdvance.Rows[i]["BaseLineDate"].ToString();
                                cap09.PymtMeth = dtbAdvance.Rows[i]["PaymentMethodCode"].ToString();
                                cap09.AllocNmbr = dtbAdvance.Rows[i]["AdvanceNo"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);
                                cap09.ItemText = dtbAdvance.Rows[i]["DescriptionAdvance"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = dtbAdvance.Rows[i]["BranchCode"].ToString();
                                cap09.TaxCode = TaxCodeConst.NV;
                                cap09.SpGlInd = SpGlIndConst.D;

                                if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                                {
                                    cap09.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                                }

                                cap09.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacap09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = mainCurrencySymbol;
                                ccr09.AmtDoccur = 0 - decimal.Parse(string.IsNullOrEmpty(dtbAdvance.Rows[i]["ExpenseAmountMainCurrency"].ToString()) ? "0" : dtbAdvance.Rows[i]["ExpenseAmountMainCurrency"].ToString());
                                ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }

                            intItemCount++;
                            // ส่วนเกินตั้งพนักงานเป็นเจ้าหนี้
                            #region Bapiacap09
                            Bapiacap09 cap109 = new Bapiacap09();
                            cap109.DocId = DocID;
                            cap109.DocSeq = "M";
                            cap109.DocKind = DocKind;
                            cap109.ItemnoAcc = intItemCount.ToString();

                            // กรณีรับเงินสด
                            if (dtbHead.Rows[0]["PaymentType"].ToString() == PaymentTypeConst.DomesticCash)
                            {
                                cap109.VendorNo = dtbHead.Rows[0]["PBCode"].ToString();
                                cap109.PmntBlock = PostingConst.PmntBlock;
                                cap109.AllocNmbr = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).EmployeeName;
                            }
                            else
                            {
                                cap109.VendorNo = SCG.eAccounting.SAP.SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["ReceiverID"].ToString())).VendorCode;
                                cap109.AllocNmbr = dtbHead.Rows[0]["DocumentNo"].ToString();
                            }

                            cap109.AllocNmbr = SAPUIHelper.SubString18(cap109.AllocNmbr);

                            cap109.Pmnttrms = PostingConst.Pmnttrms;
                            cap109.Pmtmthsupl = dtbHead.Rows[0]["Supplementary"].ToString();
                            cap109.BlineDate = dtbHead.Rows[0]["BaseLineDate"].ToString();
                            cap109.PymtMeth = dtbHead.Rows[0]["PaymentMethodCode"].ToString();
                            cap109.ItemText = dtbHead.Rows[0]["DescriptionDocument"].ToString();
                            cap109.ItemText = SAPUIHelper.SubString50(cap109.ItemText);
                            cap109.Businessplace = dtbHead.Rows[0]["BranchCode"].ToString();
                            cap109.TaxCode = TaxCodeConst.NV;

                            if (bool.Parse(dtbHead.Rows[0]["RequireBusinessArea"].ToString()))
                            {
                                cap109.BusArea = dtbHead.Rows[0]["BusinessArea"].ToString();
                            }

                            cap109.Active = true;
                            cap109.CreBy = 1;
                            cap109.CreDate = DateTime.Now;
                            cap109.UpdBy = 1;
                            cap109.UpdDate = DateTime.Now;
                            cap109.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacap09Service.Save(cap109);
                            #endregion Bapiacap09

                            #region Bapiaccr09
                            double douAmountNew = douTotalExpenseMainCurrency - douTotalAdvanceMainCurrency;

                            Bapiaccr09 ccr109 = new Bapiaccr09();
                            ccr109.DocId = DocID;
                            ccr109.DocSeq = "M";
                            ccr109.DocKind = DocKind;
                            ccr109.ItemnoAcc = intItemCount.ToString();
                            ccr109.Currency = mainCurrencySymbol;
                            ccr109.AmtDoccur = 0 - decimal.Parse(douAmountNew.ToString());
                            ccr109.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());
                            //if (isHaveVatOrWht && isHaveAdvance)
                            //    ccr109.DiscBase = ccr109.AmtDoccur;

                            ccr109.Active = true;
                            ccr109.CreBy = 1;
                            ccr109.CreDate = DateTime.Now;
                            ccr109.UpdBy = 1;
                            ccr109.UpdDate = DateTime.Now;
                            ccr109.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccr109);
                            #endregion Bapiaccr09
                        }
                    }
                    #endregion ส่วนของ Credit

                    #region ส่วนของ Debit

                    #region BAPIACAR09 ลูกหนี้ชนิดพิเศษ
                    if (!repOffice)
                    {
                        // ถ้ามี Invoice ที่ไม่มี VAT OR WHT และมี SpecialGL
                        // ต้องลงบัญชี ในตาราง AR ฝั่ง Debit < ไม่ติดลบ >
                        for (int i = 0; i < drItemAr.Length; i++)
                        {
                            intItemCount++;
                            //douSumNetAmount += double.Parse(drItemAr[i]["TotalBaseAmountItem"].ToString());
                            if (bool.Parse(drItemAr[i]["SaveAsVendor"].ToString()))
                            {
                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = drItemAr[i]["VendorCodeAP"].ToString();
                                cap09.AllocNmbr = drItemAr[i]["Assignment"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);
                                cap09.ItemText = drItemAr[i]["Description"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = drItemAr[i]["BranchCode"].ToString();

                                if (int.Parse(drItemAr[i]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                    cap09.TaxCode = string.Empty;
                                else
                                    cap09.TaxCode = drItemAr[i]["TaxCode"].ToString();

                                cap09.SpGlInd = drItemAr[i]["SpecialGL"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacar09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = PostingConst.Currency;
                                ccr09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountItem"].ToString());
                                if (isHaveVatOrWht && isHaveAdvance)
                                    ccr09.DiscBase = ccr09.AmtDoccur;

                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09); 
                                #endregion Bapiaccr09
                            }
                            else
                            {
                                if (drItemAr[i]["SpecialGL"].ToString() == ParameterServices.BAPI_SpecialGLType2)  //SpecialGLType2 = NT บัญชีพิเศษไม่ต้องส่ง TAX_CODE
                                {
                                    #region Bapiacgl09
                                    Bapiacgl09 cgl09 = new Bapiacgl09();
                                    cgl09.DocId = DocID;
                                    cgl09.DocSeq = "M";
                                    cgl09.DocKind = DocKind;
                                    cgl09.ItemnoAcc = intItemCount.ToString();
                                    cgl09.GlAccount = drItemAr[i]["AccountCode"].ToString();
                                    cgl09.ItemText = drItemAr[i]["Description"].ToString();
                                    cgl09.ItemText = SAPUIHelper.SubString50(cgl09.ItemText);
                                    cgl09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).EmployeeName;
                                    cgl09.AllocNmbr = SAPUIHelper.SubString18(cgl09.AllocNmbr);

                                    if (int.Parse(drItemAr[i]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                        cgl09.TaxCode = string.Empty;
                                    else
                                        cgl09.TaxCode = drItemAr[i]["TaxCode"].ToString();

                                    if (int.Parse(drItemAr[i]["CostCenterOption"].ToString()) == (int)CostCenterOption.None)
                                        cgl09.Costcenter = string.Empty;
                                    else
                                        cgl09.Costcenter = drItemAr[i]["CostCenterCode"].ToString();

                                    if (int.Parse(drItemAr[i]["InternalOrderOption"].ToString()) != (int)InternalOrderOption.None)
                                        cgl09.Orderid = drItemAr[i]["OrderNo"].ToString();
                                    else
                                        cgl09.Orderid = string.Empty;

                                    if (int.Parse(drItemAr[i]["SaleOrderOption"].ToString()) != (int)SaleOrderOption.None)
                                    {
                                        cgl09.SalesOrd = SAPUIHelper.PadLeftString(10, drItemAr[i]["SaleOrder"].ToString());
                                        cgl09.SOrdItem = drItemAr[i]["SaleItem"].ToString();
                                    }
                                    else
                                    {
                                        cgl09.SalesOrd = string.Empty;
                                        cgl09.SOrdItem = string.Empty;
                                    }

                                    cgl09.Active = true;
                                    cgl09.CreBy = 1;
                                    cgl09.CreDate = DateTime.Now;
                                    cgl09.UpdBy = 1;
                                    cgl09.UpdDate = DateTime.Now;
                                    cgl09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiacgl09Service.Save(cgl09);
                                    #endregion Bapiacgl09

                                    #region Bapiaccr09
                                    Bapiaccr09 ccrt09 = new Bapiaccr09();
                                    ccrt09.DocId = DocID;
                                    ccrt09.DocSeq = "M";
                                    ccrt09.DocKind = DocKind;
                                    ccrt09.ItemnoAcc = intItemCount.ToString();
                                    ccrt09.Currency = PostingConst.Currency;
                                    ccrt09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountItem"].ToString());

                                    ccrt09.Active = true;
                                    ccrt09.CreBy = 1;
                                    ccrt09.CreDate = DateTime.Now;
                                    ccrt09.UpdBy = 1;
                                    ccrt09.UpdDate = DateTime.Now;
                                    ccrt09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiaccr09Service.Save(ccrt09);
                                    #endregion Bapiaccr09
                                }
                                // ลูกหนี้ประกันสังคม
                                else if (drItemAr[i]["SpecialGL"].ToString() == ParameterServices.BAPI_SpecialGLType1) //SpecialGLType2 = 01 ลูกหนี้ประกันสังคม
                                {
                                    #region Bapiacar09
                                    Bapiacar09 car09 = new Bapiacar09();
                                    car09.DocId = DocID;
                                    car09.DocSeq = "M";
                                    car09.DocKind = DocKind;
                                    car09.ItemnoAcc = intItemCount.ToString();
                                    car09.Customer = PostingConst.ARCustomer;
                                    car09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(drItemAr[i]["RequesterID"].ToString())).EmployeeID;
                                    car09.AllocNmbr = SAPUIHelper.SubString18(car09.AllocNmbr);
                                    car09.ItemText = drItemAr[i]["Description"].ToString();
                                    car09.ItemText = SAPUIHelper.SubString50(car09.ItemText);
                                    car09.Businessplace = drItemAr[i]["BranchCode"].ToString();
                                    if (bool.Parse(drItemAr[i]["SaveAsDebtor"].ToString()))
                                    {
                                        car09.TaxCode = TaxCodeConst.NO;
                                    }
                                    car09.SpGlInd = "";

                                    car09.Active = true;
                                    car09.CreBy = 1;
                                    car09.CreDate = DateTime.Now;
                                    car09.UpdBy = 1;
                                    car09.UpdDate = DateTime.Now;
                                    car09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiacar09Service.Save(car09);
                                    #endregion Bapiacar09

                                    #region Bapiaccr09
                                    Bapiaccr09 ccr09 = new Bapiaccr09();
                                    ccr09.DocId = DocID;
                                    ccr09.DocSeq = "M";
                                    ccr09.DocKind = DocKind;
                                    ccr09.ItemnoAcc = intItemCount.ToString();
                                    ccr09.Currency = PostingConst.Currency;
                                    ccr09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountItem"].ToString());

                                    ccr09.Active = true;
                                    ccr09.CreBy = 1;
                                    ccr09.CreDate = DateTime.Now;
                                    ccr09.UpdBy = 1;
                                    ccr09.UpdDate = DateTime.Now;
                                    ccr09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                    #endregion Bapiaccr09
                                }
                                // เงินกู้ชนิดพิเศษ
                                else
                                {
                                    #region Bapiacar09
                                    Bapiacar09 car09 = new Bapiacar09();
                                    car09.DocId = DocID;
                                    car09.DocSeq = "M";
                                    car09.DocKind = DocKind;
                                    car09.ItemnoAcc = intItemCount.ToString();
                                    car09.Customer = SAPUIHelper.GetEmployee(long.Parse(drItemAr[i]["RequesterID"].ToString())).VendorCode;
                                    car09.AllocNmbr = drItemAr[i]["Assignment"].ToString();
                                    car09.AllocNmbr = SAPUIHelper.SubString18(car09.AllocNmbr);
                                    car09.ItemText = drItemAr[i]["Description"].ToString();
                                    car09.ItemText = SAPUIHelper.SubString50(car09.ItemText);
                                    car09.Businessplace = drItemAr[i]["BranchCode"].ToString();
                                    if (bool.Parse(drItemAr[i]["SaveAsDebtor"].ToString()))
                                    {
                                        car09.TaxCode = TaxCodeConst.NO;
                                    }
                                    car09.SpGlInd = drItemAr[i]["SpecialGL"].ToString();

                                    car09.Active = true;
                                    car09.CreBy = 1;
                                    car09.CreDate = DateTime.Now;
                                    car09.UpdBy = 1;
                                    car09.UpdDate = DateTime.Now;
                                    car09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiacar09Service.Save(car09);
                                    #endregion Bapiacar09

                                    #region Bapiaccr09
                                    Bapiaccr09 ccr09 = new Bapiaccr09();
                                    ccr09.DocId = DocID;
                                    ccr09.DocSeq = "M";
                                    ccr09.DocKind = DocKind;
                                    ccr09.ItemnoAcc = intItemCount.ToString();
                                    ccr09.Currency = PostingConst.Currency;
                                    ccr09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountItem"].ToString());

                                    ccr09.Active = true;
                                    ccr09.CreBy = 1;
                                    ccr09.CreDate = DateTime.Now;
                                    ccr09.UpdBy = 1;
                                    ccr09.UpdDate = DateTime.Now;
                                    ccr09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                    #endregion Bapiaccr09
                                }
                            }
                        }
                    }
                    else  // case rep office
                    {
                        // ถ้ามี Invoice ที่ไม่มี VAT OR WHT และมี SpecialGL
                        // ต้องลงบัญชี ในตาราง AR ฝั่ง Debit < ไม่ติดลบ >
                        for (int i = 0; i < drItemAr.Length; i++)
                        {
                            intItemCount++;
                            if (bool.Parse(drItemAr[i]["SaveAsVendor"].ToString()))
                            {
                                #region Bapiacap09
                                Bapiacap09 cap09 = new Bapiacap09();
                                cap09.DocId = DocID;
                                cap09.DocSeq = "M";
                                cap09.DocKind = DocKind;
                                cap09.ItemnoAcc = intItemCount.ToString();
                                cap09.VendorNo = drItemAr[i]["VendorCodeAP"].ToString();
                                cap09.AllocNmbr = drItemAr[i]["Assignment"].ToString();
                                cap09.AllocNmbr = SAPUIHelper.SubString18(cap09.AllocNmbr);
                                cap09.ItemText = drItemAr[i]["Description"].ToString();
                                cap09.ItemText = SAPUIHelper.SubString50(cap09.ItemText);
                                cap09.Businessplace = drItemAr[i]["BranchCode"].ToString();

                                if (int.Parse(drItemAr[i]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                    cap09.TaxCode = string.Empty;
                                else
                                    cap09.TaxCode = drItemAr[i]["TaxCode"].ToString();

                                cap09.SpGlInd = drItemAr[i]["SpecialGL"].ToString();

                                cap09.Active = true;
                                cap09.CreBy = 1;
                                cap09.CreDate = DateTime.Now;
                                cap09.UpdBy = 1;
                                cap09.UpdDate = DateTime.Now;
                                cap09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiacap09Service.Save(cap09);
                                #endregion Bapiacar09

                                #region Bapiaccr09
                                Bapiaccr09 ccr09 = new Bapiaccr09();
                                ccr09.DocId = DocID;
                                ccr09.DocSeq = "M";
                                ccr09.DocKind = DocKind;
                                ccr09.ItemnoAcc = intItemCount.ToString();
                                ccr09.Currency = mainCurrencySymbol;
                                ccr09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountMainCurrencyItem"].ToString());
                                ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());


                                ccr09.Active = true;
                                ccr09.CreBy = 1;
                                ccr09.CreDate = DateTime.Now;
                                ccr09.UpdBy = 1;
                                ccr09.UpdDate = DateTime.Now;
                                ccr09.UpdPgm = "PostingService";
                                BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                #endregion Bapiaccr09
                            }
                            else
                            {
                                if (drItemAr[i]["SpecialGL"].ToString() == ParameterServices.BAPI_SpecialGLType2)
                                {
                                    #region Bapiacgl09
                                    Bapiacgl09 cgl09 = new Bapiacgl09();
                                    cgl09.DocId = DocID;
                                    cgl09.DocSeq = "M";
                                    cgl09.DocKind = DocKind;
                                    cgl09.ItemnoAcc = intItemCount.ToString();
                                    cgl09.GlAccount = drItemAr[i]["AccountCode"].ToString();
                                    cgl09.ItemText = drItemAr[i]["Description"].ToString();
                                    cgl09.ItemText = SAPUIHelper.SubString50(cgl09.ItemText);
                                    cgl09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).EmployeeName;
                                    cgl09.AllocNmbr = SAPUIHelper.SubString18(cgl09.AllocNmbr);

                                    if (int.Parse(drItemAr[i]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                        cgl09.TaxCode = string.Empty;
                                    else
                                        cgl09.TaxCode = drItemAr[i]["TaxCode"].ToString();

                                    if (int.Parse(drItemAr[i]["CostCenterOption"].ToString()) == (int)CostCenterOption.None)
                                        cgl09.Costcenter = string.Empty;
                                    else
                                        cgl09.Costcenter = drItemAr[i]["CostCenterCode"].ToString();

                                    if (int.Parse(drItemAr[i]["InternalOrderOption"].ToString()) != (int)InternalOrderOption.None)
                                        cgl09.Orderid = drItemAr[i]["OrderNo"].ToString();
                                    else
                                        cgl09.Orderid = string.Empty;

                                    if (int.Parse(drItemAr[i]["SaleOrderOption"].ToString()) != (int)SaleOrderOption.None)
                                    {
                                        cgl09.SalesOrd = SAPUIHelper.PadLeftString(10, drItemAr[i]["SaleOrder"].ToString());
                                        cgl09.SOrdItem = drItemAr[i]["SaleItem"].ToString();
                                    }
                                    else
                                    {
                                        cgl09.SalesOrd = string.Empty;
                                        cgl09.SOrdItem = string.Empty;
                                    }

                                    cgl09.Active = true;
                                    cgl09.CreBy = 1;
                                    cgl09.CreDate = DateTime.Now;
                                    cgl09.UpdBy = 1;
                                    cgl09.UpdDate = DateTime.Now;
                                    cgl09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiacgl09Service.Save(cgl09);
                                    #endregion Bapiacgl09

                                    #region Bapiaccr09
                                    Bapiaccr09 ccrt09 = new Bapiaccr09();
                                    ccrt09.DocId = DocID;
                                    ccrt09.DocSeq = "M";
                                    ccrt09.DocKind = DocKind;
                                    ccrt09.ItemnoAcc = intItemCount.ToString();
                                    ccrt09.Currency = mainCurrencySymbol;
                                    ccrt09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountMainCurrencyItem"].ToString());
                                    ccrt09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                                    ccrt09.Active = true;
                                    ccrt09.CreBy = 1;
                                    ccrt09.CreDate = DateTime.Now;
                                    ccrt09.UpdBy = 1;
                                    ccrt09.UpdDate = DateTime.Now;
                                    ccrt09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiaccr09Service.Save(ccrt09);
                                    #endregion Bapiaccr09
                                }
                                // ลูกหนี้ประกันสังคม
                                else if (drItemAr[i]["SpecialGL"].ToString() == ParameterServices.BAPI_SpecialGLType1)
                                {
                                    #region Bapiacar09
                                    Bapiacar09 car09 = new Bapiacar09();
                                    car09.DocId = DocID;
                                    car09.DocSeq = "M";
                                    car09.DocKind = DocKind;
                                    car09.ItemnoAcc = intItemCount.ToString();
                                    car09.Customer = PostingConst.ARCustomer;
                                    car09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(drItemAr[i]["RequesterID"].ToString())).EmployeeID;
                                    car09.AllocNmbr = SAPUIHelper.SubString18(car09.AllocNmbr);
                                    car09.ItemText = drItemAr[i]["Description"].ToString();
                                    car09.ItemText = SAPUIHelper.SubString50(car09.ItemText);
                                    car09.Businessplace = drItemAr[i]["BranchCode"].ToString();
                                    car09.TaxCode = TaxCodeConst.NO;
                                    car09.SpGlInd = "";

                                    car09.Active = true;
                                    car09.CreBy = 1;
                                    car09.CreDate = DateTime.Now;
                                    car09.UpdBy = 1;
                                    car09.UpdDate = DateTime.Now;
                                    car09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiacar09Service.Save(car09);
                                    #endregion Bapiacar09

                                    #region Bapiaccr09
                                    Bapiaccr09 ccr09 = new Bapiaccr09();
                                    ccr09.DocId = DocID;
                                    ccr09.DocSeq = "M";
                                    ccr09.DocKind = DocKind;
                                    ccr09.ItemnoAcc = intItemCount.ToString();
                                    ccr09.Currency = mainCurrencySymbol;
                                    ccr09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountMainCurrencyItem"].ToString());
                                    ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                                    ccr09.Active = true;
                                    ccr09.CreBy = 1;
                                    ccr09.CreDate = DateTime.Now;
                                    ccr09.UpdBy = 1;
                                    ccr09.UpdDate = DateTime.Now;
                                    ccr09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                    #endregion Bapiaccr09
                                }
                                // เงินกู้ชนิดพิเศษ
                                else
                                {
                                    #region Bapiacar09
                                    Bapiacar09 car09 = new Bapiacar09();
                                    car09.DocId = DocID;
                                    car09.DocSeq = "M";
                                    car09.DocKind = DocKind;
                                    car09.ItemnoAcc = intItemCount.ToString();
                                    car09.Customer = SAPUIHelper.GetEmployee(long.Parse(drItemAr[i]["RequesterID"].ToString())).VendorCode;
                                    car09.AllocNmbr = drItemAr[i]["Assignment"].ToString();
                                    car09.AllocNmbr = SAPUIHelper.SubString18(car09.AllocNmbr);
                                    car09.ItemText = drItemAr[i]["Description"].ToString();
                                    car09.ItemText = SAPUIHelper.SubString50(car09.ItemText);
                                    car09.Businessplace = drItemAr[i]["BranchCode"].ToString();
                                    car09.TaxCode = TaxCodeConst.NO;
                                    car09.SpGlInd = drItemAr[i]["SpecialGL"].ToString();

                                    car09.Active = true;
                                    car09.CreBy = 1;
                                    car09.CreDate = DateTime.Now;
                                    car09.UpdBy = 1;
                                    car09.UpdDate = DateTime.Now;
                                    car09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiacar09Service.Save(car09);
                                    #endregion Bapiacar09

                                    #region Bapiaccr09
                                    Bapiaccr09 ccr09 = new Bapiaccr09();
                                    ccr09.DocId = DocID;
                                    ccr09.DocSeq = "M";
                                    ccr09.DocKind = DocKind;
                                    ccr09.ItemnoAcc = intItemCount.ToString();
                                    ccr09.Currency = mainCurrencySymbol;
                                    ccr09.AmtDoccur = decimal.Parse(drItemAr[i]["TotalBaseAmountMainCurrencyItem"].ToString());
                                    ccr09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                                    ccr09.Active = true;
                                    ccr09.CreBy = 1;
                                    ccr09.CreDate = DateTime.Now;
                                    ccr09.UpdBy = 1;
                                    ccr09.UpdDate = DateTime.Now;
                                    ccr09.UpdPgm = "PostingService";
                                    BapiServiceProvider.Bapiaccr09Service.Save(ccr09);
                                    #endregion Bapiaccr09
                                }
                            }
                        }
                    }
                    #endregion BAPIACAR09 ลูกหนี้ชนิดพิเศษ

                    #region ส่วนของ IC
                    if (!repOffice && isHaveICCase)
                    {
                        intItemCount++;

                        #region Bapiacgl09
                        Bapiacgl09 cgl09IC = new Bapiacgl09();
                        cgl09IC.DocId = DocID;
                        cgl09IC.DocSeq = "M";
                        cgl09IC.DocKind = DocKind;
                        cgl09IC.ItemnoAcc = intItemCount.ToString();

                        cgl09IC.GlAccount = ParameterServices.BAPI_SAV;
                        cgl09IC.AllocNmbr = SAPUIHelper.SubString18(dtbHead.Rows[0]["DocumentNo"].ToString());
                        cgl09IC.ItemText = "";

                        DbAccount account = ScgDbQueryProvider.DbAccountQuery.FindDbAccountByAccountCode(ParameterServices.BAPI_SAV);
                        if (account != null)
                        {
                            if (account.TaxCode != (int)TaxCodeOption.None)
                                cgl09IC.TaxCode = TaxCodeConst.NV;
                            else
                                cgl09IC.TaxCode = string.Empty;

                            if (account.InternalOrder != (int)SaleOrderOption.None)
                            {
                                cgl09IC.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                                cgl09IC.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                            }
                            else
                            {
                                cgl09IC.SalesOrd = string.Empty;
                                cgl09IC.SOrdItem = string.Empty;
                            }
                        }
                        else
                        {
                            cgl09IC.TaxCode = TaxCodeConst.NV;
                            cgl09IC.Orderid = drItemIC[0]["OrderNo"].ToString();
                            cgl09IC.SalesOrd = SAPUIHelper.PadLeftString(10, drItemIC[0]["SaleOrder"].ToString());
                            cgl09IC.SOrdItem = drItemIC[0]["SaleItem"].ToString();
                        }

                        cgl09IC.Active = true;
                        cgl09IC.CreBy = 1;
                        cgl09IC.CreDate = DateTime.Now;
                        cgl09IC.UpdBy = 1;
                        cgl09IC.UpdDate = DateTime.Now;
                        cgl09IC.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiacgl09Service.Save(cgl09IC);
                        #endregion Bapiacgl09

                        #region Bapiaccr09
                        Bapiaccr09 ccrt09IC = new Bapiaccr09();
                        ccrt09IC.DocId = DocID;
                        ccrt09IC.DocSeq = "M";
                        ccrt09IC.DocKind = DocKind;
                        ccrt09IC.ItemnoAcc = intItemCount.ToString();
                        ccrt09IC.Currency = PostingConst.Currency;
                        ccrt09IC.AmtDoccur = decimal.Parse(douSumPerdiem.ToString());

                        ccrt09IC.Active = true;
                        ccrt09IC.CreBy = 1;
                        ccrt09IC.CreDate = DateTime.Now;
                        ccrt09IC.UpdBy = 1;
                        ccrt09IC.UpdDate = DateTime.Now;
                        ccrt09IC.UpdPgm = "PostingService";
                        BapiServiceProvider.Bapiaccr09Service.Save(ccrt09IC);
                        #endregion Bapiaccr09
                    }
                    #endregion ส่วนของ IC

                    #region BAPIACGL09
                    if (!repOffice)
                    {
                        // ถ้ามี Invoice ที่ไม่มี VAT OR WHT และไม่มี SpecialGL
                        // ต้องลงบัญชี ในตาราง GL ฝั่ง Debit < ไม่ติดลบ >
                        for (int i = 0; i < drItemGl.Length; i++)
                        {
                            intItemCount++;
                            //douSumNetAmount += double.Parse(drItemGl[i]["TotalBaseAmountItem"].ToString());

                            #region Bapiacgl09
                            Bapiacgl09 cgl09 = new Bapiacgl09();
                            cgl09.DocId = DocID;
                            cgl09.DocSeq = "M";
                            cgl09.DocKind = DocKind;
                            cgl09.ItemnoAcc = intItemCount.ToString();
                            cgl09.GlAccount = drItemGl[i]["AccountCode"].ToString();
                            cgl09.ItemText = drItemGl[i]["Description"].ToString();
                            cgl09.ItemText = SAPUIHelper.SubString50(cgl09.ItemText);
                            cgl09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).EmployeeName;
                            cgl09.AllocNmbr = SAPUIHelper.SubString18(cgl09.AllocNmbr);

                            if (int.Parse(drItemGl[i]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                cgl09.TaxCode = string.Empty;
                            else
                                cgl09.TaxCode = TaxCodeConst.NV;

                            if (int.Parse(drItemGl[i]["CostCenterOption"].ToString()) != (int)CostCenterOption.None) //if (SAPUIHelper.IsValidCostCenterForAccountCode(drItemGl[i]["AccountCode"].ToString()))
                                cgl09.Costcenter = drItemGl[i]["CostCenterCode"].ToString();
                            else
                                cgl09.Costcenter = string.Empty;

                            if (int.Parse(drItemGl[i]["SaleOrderOption"].ToString()) != (int)SaleOrderOption.None)
                            {
                                cgl09.SalesOrd = SAPUIHelper.PadLeftString(10, drItemGl[i]["SaleOrder"].ToString());
                                cgl09.SOrdItem = drItemGl[i]["SaleItem"].ToString();
                            }
                            else
                            {
                                cgl09.SalesOrd = string.Empty;
                                cgl09.SOrdItem = string.Empty;
                            }

                            if (int.Parse(drItemGl[i]["InternalOrderOption"].ToString()) != (int)InternalOrderOption.None)
                                cgl09.Orderid = drItemGl[i]["OrderNo"].ToString();
                            else
                                cgl09.Orderid = string.Empty;

                            cgl09.Active = true;
                            cgl09.CreBy = 1;
                            cgl09.CreDate = DateTime.Now;
                            cgl09.UpdBy = 1;
                            cgl09.UpdDate = DateTime.Now;
                            cgl09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacgl09Service.Save(cgl09);
                            #endregion Bapiacgl09

                            #region Bapiaccr09
                            Bapiaccr09 ccrt09 = new Bapiaccr09();
                            ccrt09.DocId = DocID;
                            ccrt09.DocSeq = "M";
                            ccrt09.DocKind = DocKind;
                            ccrt09.ItemnoAcc = intItemCount.ToString();
                            ccrt09.Currency = PostingConst.Currency;
                            ccrt09.AmtDoccur = decimal.Parse(drItemGl[i]["TotalBaseAmountItem"].ToString());

                            ccrt09.Active = true;
                            ccrt09.CreBy = 1;
                            ccrt09.CreDate = DateTime.Now;
                            ccrt09.UpdBy = 1;
                            ccrt09.UpdDate = DateTime.Now;
                            ccrt09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccrt09);
                            #endregion Bapiaccr09
                        }
                    }
                    else //case rep office
                    {
                        // ถ้ามี Invoice ที่ไม่มี VAT OR WHT และไม่มี SpecialGL
                        // ต้องลงบัญชี ในตาราง GL ฝั่ง Debit < ไม่ติดลบ >
                        for (int i = 0; i < drItemGl.Length; i++)
                        {
                            intItemCount++;
                            //douSumNetAmount += double.Parse(drItemGl[i]["TotalBaseAmountItem"].ToString());

                            #region Bapiacgl09
                            Bapiacgl09 cgl09 = new Bapiacgl09();
                            cgl09.DocId = DocID;
                            cgl09.DocSeq = "M";
                            cgl09.DocKind = DocKind;
                            cgl09.ItemnoAcc = intItemCount.ToString();
                            cgl09.GlAccount = drItemGl[i]["AccountCode"].ToString();
                            cgl09.ItemText = drItemGl[i]["Description"].ToString();
                            cgl09.ItemText = SAPUIHelper.SubString50(cgl09.ItemText);
                            cgl09.AllocNmbr = SAPUIHelper.GetEmployee(long.Parse(dtbHead.Rows[0]["RequesterID"].ToString())).EmployeeName;
                            cgl09.AllocNmbr = SAPUIHelper.SubString18(cgl09.AllocNmbr);

                            if (int.Parse(drItemGl[i]["TaxCodeOption"].ToString()) == (int)TaxCodeOption.None)
                                cgl09.TaxCode = string.Empty;
                            else
                                cgl09.TaxCode = TaxCodeConst.NV;


                            if (int.Parse(drItemGl[i]["CostCenterOption"].ToString()) != (int)CostCenterOption.None)  //if (SAPUIHelper.IsValidCostCenterForAccountCode(drItemGl[i]["AccountCode"].ToString()))
                                cgl09.Costcenter = drItemGl[i]["CostCenterCode"].ToString();
                            else
                                cgl09.Costcenter = string.Empty;

                            if (int.Parse(drItemGl[i]["SaleOrderOption"].ToString()) != (int)SaleOrderOption.None)
                            {
                                cgl09.SalesOrd = SAPUIHelper.PadLeftString(10, drItemGl[i]["SaleOrder"].ToString());
                                cgl09.SOrdItem = drItemGl[i]["SaleItem"].ToString();
                            }
                            else
                            {
                                cgl09.SalesOrd = string.Empty;
                                cgl09.SOrdItem = string.Empty;
                            }

                            if (int.Parse(drItemGl[i]["InternalOrderOption"].ToString()) != (int)InternalOrderOption.None)
                                cgl09.Orderid = drItemGl[i]["OrderNo"].ToString();
                            else
                                cgl09.Orderid = string.Empty;

                            cgl09.Active = true;
                            cgl09.CreBy = 1;
                            cgl09.CreDate = DateTime.Now;
                            cgl09.UpdBy = 1;
                            cgl09.UpdDate = DateTime.Now;
                            cgl09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiacgl09Service.Save(cgl09);
                            #endregion Bapiacgl09

                            #region Bapiaccr09
                            Bapiaccr09 ccrt09 = new Bapiaccr09();
                            ccrt09.DocId = DocID;
                            ccrt09.DocSeq = "M";
                            ccrt09.DocKind = DocKind;
                            ccrt09.ItemnoAcc = intItemCount.ToString();
                            ccrt09.Currency = mainCurrencySymbol;
                            ccrt09.AmtDoccur = decimal.Parse(drItemGl[i]["TotalBaseAmountMainCurrencyItem"].ToString());
                            ccrt09.ExchRate = decimal.Parse(dtbHead.Rows[0]["ExchangeRateMainToTHBCurrency"].ToString());

                            ccrt09.Active = true;
                            ccrt09.CreBy = 1;
                            ccrt09.CreDate = DateTime.Now;
                            ccrt09.UpdBy = 1;
                            ccrt09.UpdDate = DateTime.Now;
                            ccrt09.UpdPgm = "PostingService";
                            BapiServiceProvider.Bapiaccr09Service.Save(ccrt09);
                            #endregion Bapiaccr09
                        }
                    }
                    #endregion BAPIACGL09

                    #endregion ส่วนของ Debit
                }
                #endregion Check for Head haves not Vat & not WHT
            }
        }

        public override IList<BAPISimulateReturn> BAPISimulate(long DocId, string DocKind)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPISimulateReturn> SimulateReturn = new List<BAPISimulateReturn>();

            Hashtable parameter = new Hashtable();
            parameter.Add("@DOCUMENT_ID", DocId.ToString());

            #region IC Case

            bool isHaveICCase = false;
            bool comRequestUseEcc = false;
            // Check IC Case
            DataSet dstICCheck = new DBManage().ExecuteQuery("EXPENSE_IC_CHECK", parameter);
            if (dstICCheck.Tables[0].Rows.Count > 0)
            {
                isHaveICCase = true;
                comRequestUseEcc = bool.Parse(dstICCheck.Tables[0].Rows[0]["ComRequesterUseEcc"].ToString());
            }
            else
                isHaveICCase = false;
            #endregion IC Case

            if (!isHaveICCase)
                return base.BAPISimulate(DocId, DocKind);

            if (comRequestUseEcc)
            {
                if (dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString() != dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString())
                {
                    RfcDestination orginalDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString());
                    RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                    SimulateReturn = ICBAPISimulate(orginalDestination, workDestination, DocId, DocKind, true, true);
                }
                else
                {
                    SimulateReturn = base.BAPISimulate(DocId, DocKind);
                }
            }
            else
            {
                RfcDestination orginalDestination = GetDestination();
                RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                SimulateReturn = ICBAPISimulate(orginalDestination, workDestination, DocId, DocKind, false, true);
            }

            return SimulateReturn;
        }

        public override IList<BAPIPostingReturn> BAPIPosting(long DocId, string DocKind)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIPostingReturn> PostingReturn = new List<BAPIPostingReturn>();

            Hashtable parameter = new Hashtable();
            parameter.Add("@DOCUMENT_ID", DocId.ToString());

            #region IC Case

            bool isHaveICCase = false;
            bool comRequestUseEcc = false;
            // Check IC Case
            DataSet dstICCheck = new DBManage().ExecuteQuery("EXPENSE_IC_CHECK", parameter);
            if (dstICCheck.Tables[0].Rows.Count > 0)
            {
                isHaveICCase = true;
                comRequestUseEcc = bool.Parse(dstICCheck.Tables[0].Rows[0]["ComRequesterUseEcc"].ToString());
            }
            else
                isHaveICCase = false;
            #endregion IC Case

            if (!isHaveICCase)
                return base.BAPIPosting(DocId, DocKind);

            if (comRequestUseEcc)
            {
                if (dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString() != dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString())
                {
                    RfcDestination orginalDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString());
                    RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                    PostingReturn = ICBAPIPosting(orginalDestination, workDestination, DocId, DocKind, true, true);
                }
                else
                {
                    PostingReturn = base.BAPIPosting(DocId, DocKind);
                }
            }
            else
            {
                RfcDestination orginalDestination = GetDestination();
                RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                PostingReturn = ICBAPIPosting(orginalDestination, workDestination, DocId, DocKind, false, true);
            }

            return PostingReturn;
        }

        public override IList<BAPIApproveReturn> BAPIApprove(long DocId, string DocKind, long UserAccountID)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIApproveReturn> ApproveReturn = new List<BAPIApproveReturn>();

            Hashtable parameter = new Hashtable();
            parameter.Add("@DOCUMENT_ID", DocId.ToString());

            #region IC Case

            bool isHaveICCase = false;
            bool comRequestUseEcc = false;
            // Check IC Case
            DataSet dstICCheck = new DBManage().ExecuteQuery("EXPENSE_IC_CHECK", parameter);
            if (dstICCheck.Tables[0].Rows.Count > 0)
            {
                isHaveICCase = true;
                comRequestUseEcc = bool.Parse(dstICCheck.Tables[0].Rows[0]["ComRequesterUseEcc"].ToString());
            }
            else
                isHaveICCase = false;
            #endregion IC Case

            if (!isHaveICCase)
                return base.BAPIApprove(DocId, DocKind, UserAccountID);

            if (comRequestUseEcc)
            {
                if (dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString() != dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString())
                {
                    RfcDestination orginalDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString());
                    RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                    ApproveReturn = ICBAPIApprove(orginalDestination, workDestination, DocId, DocKind, UserAccountID, true, true);
                }
                else
                {
                    ApproveReturn = base.BAPIApprove(DocId, DocKind, UserAccountID);
                }
            }
            else
            {
                RfcDestination orginalDestination = GetDestination();
                RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                ApproveReturn = ICBAPIApprove(orginalDestination, workDestination, DocId, DocKind, UserAccountID, false, true);
            }

            return ApproveReturn;
        }

        public override IList<BAPIReverseReturn> BAPIReverse(long DocId, string DocKind)
        {
            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIReverseReturn> ReverseReturn = new List<BAPIReverseReturn>();

            Hashtable parameter = new Hashtable();
            parameter.Add("@DOCUMENT_ID", DocId.ToString());

            #region IC Case

            bool isHaveICCase = false;
            bool comRequestUseEcc = false;
            // Check IC Case
            DataSet dstICCheck = new DBManage().ExecuteQuery("EXPENSE_IC_CHECK", parameter);
            if (dstICCheck.Tables[0].Rows.Count > 0)
            {
                isHaveICCase = true;
                comRequestUseEcc = bool.Parse(dstICCheck.Tables[0].Rows[0]["ComRequesterUseEcc"].ToString());
            }
            else
                isHaveICCase = false;
            #endregion IC Case

            if (!isHaveICCase)
                return base.BAPIReverse(DocId, DocKind);

            if (comRequestUseEcc)
            {
                if (dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString() != dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString())
                {
                    RfcDestination orginalDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComRequesterSapCode"].ToString());
                    RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                    ReverseReturn = ICBAPIReverse(orginalDestination, workDestination, DocId, DocKind, true, true);
                }
                else
                {
                    ReverseReturn = base.BAPIReverse(DocId, DocKind);
                }
            }
            else
            {
                RfcDestination orginalDestination = GetDestination();
                RfcDestination workDestination = GetDestination(dstICCheck.Tables[0].Rows[0]["ComDocumentSapCode"].ToString());
                ReverseReturn = ICBAPIReverse(orginalDestination, workDestination, DocId, DocKind, false, true);
            }

            return ReverseReturn;
        }



        private IList<BAPISimulateReturn> ICBAPISimulate(RfcDestination orginalDestination, RfcDestination workDestination, long DocId, string DocKind, bool orginalUseEcc, bool workUseEcc)
        {
            if (IsOpenSimulator())
                return SimulateWithoutSAP(DocId, DocKind);

            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPISimulateReturn> SimulateReturn = new List<BAPISimulateReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

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

            //connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2
            #region connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2
            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_CHECK simulateOrigin = new SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_CHECK(orginalDestination);

            var listOrigin = listBAPIACHE09.Where(t => t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listOrigin.Count; i++)
            {
                string strDocId = GetDocumentStatus(listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind);
                if (strDocId.Equals("N") || strDocId.Equals("S"))
                {
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
                    #endregion

                    #region GetPostingDataByDocId
                    this.GetPostingDataByDocId(
                        listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind,
                        ref listBAPIACEXTC, ref listBAPIACPA09,
                        ref listBAPIACAP09, ref listBAPIACGL09,
                        ref listBAPIACTX09, ref listBAPIACCR09,
                        ref listBAPIACAR09);
                    #endregion GetPostingDataByDocId()

                    #region SetPostingDataToSAP
                    this.SetPostingDataToSAP(
                        listOrigin[i],
                        listBAPIACPA09, listBAPIACEXTC,
                        listBAPIACAP09, listBAPIACGL09,
                        listBAPIACTX09, listBAPIACCR09, listBAPIACAR09,
                        ref ACHE09,
                        ref ACPA09, ref ACEXTCTable,
                        ref ACAP09Table, ref ACGL09Table,
                        ref ACTX09Table, ref ACCR09Table, ref ACAR09Table);
                    #endregion SetPostingDataToSAP

                    #region Call SAP BAPI

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Start Simulate");
                    simulateOrigin.Ybapi_Acc_Document_Check(
                        ACCAHD, ACPA09, ACHE09,
                        ref ACGL09Table, ref ACAP09Table, ref ACAR09Table,
                        ref ACTX09Table, ref ACCAITTable, ref ACKEC9Table,
                        ref ACCR09Table, ref ACEXTCTable, ref PAREXTable,
                        ref ACPC09Table, ref ACRE09Table, ref RET2Table,
                        ref ACKEV9Table, orginalUseEcc);
                    //simulate.CommitWork();
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Finished Simulate");
                    #endregion Call SAP BAPI

                    #region Update Value To WebApp
                    if (GetReturnStatus(RET2Table))
                    {
                        listOrigin[i].DocStatus = "S";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listOrigin[i]);
                    }
                    #endregion Update Value To WebApp

                    #region SaveReturnPostingDataToDatabase
                    this.SaveReturnPostingDataToDatabase(
                        listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind + "-Simulate",
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
                    bapiReturn.ComCode = listOrigin[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listOrigin[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listOrigin[i].DocSeq;
                    bapiReturn.SimulateStatus = RET2Table[0].Type;
                    bapiReturn.SimulateReturn = RET2Table;
                    SimulateReturn.Add(bapiReturn);
                }
            }

            #endregion

            // connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน
            #region connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน
            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_CHECK simulateWork = new SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_CHECK(workDestination);

            var listWork = listBAPIACHE09.Where(t => !t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listWork.Count; i++)
            {
                string strDocId = GetDocumentStatus(listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind);
                if (strDocId.Equals("N") || strDocId.Equals("S"))
                {
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
                    #endregion

                    #region GetPostingDataByDocId
                    this.GetPostingDataByDocId(
                        listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind,
                        ref listBAPIACEXTC, ref listBAPIACPA09,
                        ref listBAPIACAP09, ref listBAPIACGL09,
                        ref listBAPIACTX09, ref listBAPIACCR09,
                        ref listBAPIACAR09);
                    #endregion GetPostingDataByDocId()

                    #region SetPostingDataToSAP
                    this.SetPostingDataToSAP(
                        listWork[i],
                        listBAPIACPA09, listBAPIACEXTC,
                        listBAPIACAP09, listBAPIACGL09,
                        listBAPIACTX09, listBAPIACCR09, listBAPIACAR09,
                        ref ACHE09,
                        ref ACPA09, ref ACEXTCTable,
                        ref ACAP09Table, ref ACGL09Table,
                        ref ACTX09Table, ref ACCR09Table, ref ACAR09Table);
                    #endregion SetPostingDataToSAP

                    #region Call SAP BAPI

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Start Simulate");
                    simulateWork.Ybapi_Acc_Document_Check(
                        ACCAHD, ACPA09, ACHE09,
                        ref ACGL09Table, ref ACAP09Table, ref ACAR09Table,
                        ref ACTX09Table, ref ACCAITTable, ref ACKEC9Table,
                        ref ACCR09Table, ref ACEXTCTable, ref PAREXTable,
                        ref ACPC09Table, ref ACRE09Table, ref RET2Table,
                        ref ACKEV9Table, workUseEcc);
                    //simulate.CommitWork();
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Finished Simulate");
                    #endregion Call SAP BAPI

                    #region Update Value To WebApp
                    if (GetReturnStatus(RET2Table))
                    {
                        listWork[i].DocStatus = "S";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listWork[i]);
                    }
                    #endregion Update Value To WebApp

                    #region SaveReturnPostingDataToDatabase
                    this.SaveReturnPostingDataToDatabase(
                        listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind + "-Simulate",
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
                    bapiReturn.ComCode = listWork[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listWork[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listWork[i].DocSeq;
                    bapiReturn.SimulateStatus = RET2Table[0].Type;
                    bapiReturn.SimulateReturn = RET2Table;
                    SimulateReturn.Add(bapiReturn);
                }
            }

            #endregion

            return SimulateReturn;
        }

        private IList<BAPIPostingReturn> ICBAPIPosting(RfcDestination orginalDestination, RfcDestination workDestination, long DocId, string DocKind, bool orginalUseEcc, bool workUseEcc)
        {
            if (IsOpenSimulator())
                return PostingWithoutSAP(DocId, DocKind);

            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIPostingReturn> PostingReturn = new List<BAPIPostingReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

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

            //connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2
            #region connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_POST_COMMIT checkOrigin = new SAPProxy_YBAPI_ACC_DOCUMENT_POST_COMMIT(orginalDestination);

            var listOrigin = listBAPIACHE09.Where(t => t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listOrigin.Count; i++)
            {
                string strDocId = GetDocumentStatus(listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind);
                if (strDocId.Equals("S"))
                {
                    #region BAPI Service
                    string ObjKey;
                    string ObjSys;
                    string ObjType;

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

                    #region GetPostingDataByDocId
                    this.GetPostingDataByDocId(
                        listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind,
                        ref listBAPIACEXTC, ref listBAPIACPA09,
                        ref listBAPIACAP09, ref listBAPIACGL09,
                        ref listBAPIACTX09, ref listBAPIACCR09, ref listBAPIACAR09);
                    #endregion GetPostingDataByDocId()

                    #region SetPostingDataToSAP
                    this.SetPostingDataToSAP(
                        listOrigin[i],
                        listBAPIACPA09, listBAPIACEXTC,
                        listBAPIACAP09, listBAPIACGL09,
                        listBAPIACTX09, listBAPIACCR09, listBAPIACAR09,
                        ref ACHE09,
                        ref ACPA09, ref ACEXTCTable,
                        ref ACAP09Table, ref ACGL09Table,
                        ref ACTX09Table, ref ACCR09Table, ref ACAR09Table);
                    #endregion SetPostingDataToSAP

                    #region Call SAP BAPI
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Start Posting");
                    checkOrigin.Ybapi_Acc_Document_Post_Commit(
                        ACCAHD, ACPA09, ACHE09,
                        out ObjKey, out ObjSys, out ObjType,
                        ref ACGL09Table, ref ACAP09Table, ref ACAR09Table,
                        ref ACTX09Table, ref ACCAITTable, ref ACKEC9Table,
                        ref ACCR09Table, ref ACEXTCTable, ref PAREXTable,
                        ref ACPC09Table, ref ACRE09Table, ref RET2Table,
                        ref ACKEV9Table, orginalUseEcc);
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Finished Posting");
                    #endregion Call SAP BAPI

                    #region Update Value To WebApp
                    if (GetReturnStatus(RET2Table))
                    {
                        listOrigin[i].DocStatus = "P";
                        listOrigin[i].FiDoc = ObjKey.Substring(0, 10);
                        listOrigin[i].DocYear = ObjKey.Substring(14, 4);
                        listOrigin[i].ObjKey = ObjKey;
                        listOrigin[i].ObjType = ObjType;
                        listOrigin[i].ObjSys = ObjSys;
                        listOrigin[i].FiscYear = ACHE09.Fisc_Year;
                        listOrigin[i].FisPeriod = ACHE09.Fis_Period;
                        listOrigin[i].HeaderTxt = BAPIGetFIDoc(ObjKey.Substring(0, 10), ObjKey.Substring(14, 4), listOrigin[i].CompCode);
                        listOrigin[i].ObjKeyR = ACHE09.Obj_Key_R;
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listOrigin[i]);
                    }
                    #endregion

                    #region SaveReturnPostingDataToDatabase
                    this.SaveReturnPostingDataToDatabase(
                        listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind + "-Posting",
                        ref listBAPIACPA09,
                        ref listBAPIACEXTC, ref listBAPIACAP09,
                        ref listBAPIACGL09, ref listBAPIACTX09,
                        ref listBAPIACCR09, ref listBAPIACAR09, ref listBAPIRET2,
                        ACPA09,
                        ACEXTCTable, ACAP09Table,
                        ACGL09Table, ACTX09Table,
                        ACCR09Table, ACAR09Table, RET2Table);
                    #endregion

                    // ***********************
                    // Return Value
                    // ***********************
                    BAPIPostingReturn bapiReturn = new BAPIPostingReturn();
                    bapiReturn.ComCode = listOrigin[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listOrigin[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listOrigin[i].DocSeq;
                    bapiReturn.ObjKey = ObjKey;
                    bapiReturn.ObjSys = ObjSys;
                    bapiReturn.ObjType = ObjType;
                    bapiReturn.PostingStatus = RET2Table[0].Type;
                    bapiReturn.PostingReturn = RET2Table;
                    PostingReturn.Add(bapiReturn);
                    #endregion
                }
            }

            #endregion

            // connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน
            #region connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_ACC_DOCUMENT_POST_COMMIT checkWork = new SAPProxy_YBAPI_ACC_DOCUMENT_POST_COMMIT(workDestination);

            var listWork = listBAPIACHE09.Where(t => !t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listWork.Count; i++)
            {
                string strDocId = GetDocumentStatus(listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind);
                if (strDocId.Equals("S"))
                {
                    #region BAPI Service
                    string ObjKey;
                    string ObjSys;
                    string ObjType;

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

                    #region GetPostingDataByDocId
                    this.GetPostingDataByDocId(
                        listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind,
                        ref listBAPIACEXTC, ref listBAPIACPA09,
                        ref listBAPIACAP09, ref listBAPIACGL09,
                        ref listBAPIACTX09, ref listBAPIACCR09, ref listBAPIACAR09);
                    #endregion GetPostingDataByDocId()

                    #region SetPostingDataToSAP
                    this.SetPostingDataToSAP(
                        listWork[i],
                        listBAPIACPA09, listBAPIACEXTC,
                        listBAPIACAP09, listBAPIACGL09,
                        listBAPIACTX09, listBAPIACCR09, listBAPIACAR09,
                        ref ACHE09,
                        ref ACPA09, ref ACEXTCTable,
                        ref ACAP09Table, ref ACGL09Table,
                        ref ACTX09Table, ref ACCR09Table, ref ACAR09Table);
                    #endregion SetPostingDataToSAP

                    #region Call SAP BAPI
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Start Posting");
                    checkWork.Ybapi_Acc_Document_Post_Commit(
                        ACCAHD, ACPA09, ACHE09,
                        out ObjKey, out ObjSys, out ObjType,
                        ref ACGL09Table, ref ACAP09Table, ref ACAR09Table,
                        ref ACTX09Table, ref ACCAITTable, ref ACKEC9Table,
                        ref ACCR09Table, ref ACEXTCTable, ref PAREXTable,
                        ref ACPC09Table, ref ACRE09Table, ref RET2Table,
                        ref ACKEV9Table, workUseEcc);
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Finished Posting");
                    #endregion Call SAP BAPI

                    #region Update Value To WebApp
                    if (GetReturnStatus(RET2Table))
                    {
                        listWork[i].DocStatus = "P";
                        listWork[i].FiDoc = ObjKey.Substring(0, 10);
                        listWork[i].DocYear = ObjKey.Substring(14, 4);
                        listWork[i].ObjKey = ObjKey;
                        listWork[i].ObjType = ObjType;
                        listWork[i].ObjSys = ObjSys;
                        listWork[i].FiscYear = ACHE09.Fisc_Year;
                        listWork[i].FisPeriod = ACHE09.Fis_Period;
                        listWork[i].HeaderTxt = BAPIGetFIDoc(ObjKey.Substring(0, 10), ObjKey.Substring(14, 4), listWork[i].CompCode);
                        listWork[i].ObjKeyR = ACHE09.Obj_Key_R;
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listWork[i]);
                    }
                    #endregion

                    #region SaveReturnPostingDataToDatabase
                    this.SaveReturnPostingDataToDatabase(
                        listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind + "-Posting",
                        ref listBAPIACPA09,
                        ref listBAPIACEXTC, ref listBAPIACAP09,
                        ref listBAPIACGL09, ref listBAPIACTX09,
                        ref listBAPIACCR09, ref listBAPIACAR09, ref listBAPIRET2,
                        ACPA09,
                        ACEXTCTable, ACAP09Table,
                        ACGL09Table, ACTX09Table,
                        ACCR09Table, ACAR09Table, RET2Table);
                    #endregion

                    // ***********************
                    // Return Value
                    // ***********************
                    BAPIPostingReturn bapiReturn = new BAPIPostingReturn();
                    bapiReturn.ComCode = listWork[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listWork[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listWork[i].DocSeq;
                    bapiReturn.ObjKey = ObjKey;
                    bapiReturn.ObjSys = ObjSys;
                    bapiReturn.ObjType = ObjType;
                    bapiReturn.PostingStatus = RET2Table[0].Type;
                    bapiReturn.PostingReturn = RET2Table;
                    PostingReturn.Add(bapiReturn);
                    #endregion
                }
            }

            #endregion

            #region Update Document Table
            SCG.eAccounting.BLL.Implement.SCGDocumentService scgService = new SCG.eAccounting.BLL.Implement.SCGDocumentService();

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

        private IList<BAPIApproveReturn> ICBAPIApprove(RfcDestination orginalDestination, RfcDestination workDestination, long DocId, string DocKind, long UserAccountID, bool orginalUseEcc, bool workUseEcc)
        {
            if (IsOpenSimulator())
                return ApproveWithoutSAP(DocId, DocKind);

            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIApproveReturn> ApproveReturn = new List<BAPIApproveReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

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

            //connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2
            #region connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_EFORMID check = new SAPProxy_YBAPI_EFORMID(orginalDestination);

            var listOrigin = listBAPIACHE09.Where(t => t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listOrigin.Count; i++)
            {
                string strDocId = GetDocumentStatus(listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind);
                if (strDocId.Equals("P"))
                {
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

                    #region GetPostingDataByDocId
                    this.GetPostingDataByDocId(
                        listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind,
                        ref listBAPIACEXTC, ref listBAPIACPA09,
                        ref listBAPIACAP09, ref listBAPIACGL09,
                        ref listBAPIACTX09, ref listBAPIACCR09,
                        ref listBAPIACAR09);
                    #endregion GetPostingDataByDocId()

                    #region Call SAP BAPI
                    ZACCKEYTable zacKeyTable = new ZACCKEYTable();
                    ZACCKEY zacKey = new ZACCKEY();

                    if (listOrigin[i].DocAppFlag == null || listOrigin[i].DocAppFlag.ToString() == "")
                        zacKey.App_Type = "A";
                    else
                        zacKey.App_Type = listOrigin[i].DocAppFlag;

                    zacKey.Bukrs = listOrigin[i].CompCode;
                    zacKey.Belnr = listOrigin[i].FiDoc;
                    zacKey.Gjahr = listOrigin[i].DocYear;
                    zacKeyTable.Add(zacKey);

                    string eFormID = listOrigin[0].RefDocNo.Substring(0, 3) + listOrigin[0].RefDocNo.Substring(listOrigin[0].RefDocNo.Length - 7, 7);

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Start Approve");
                    check.Ybapi_Eformid(
                        SAPUIHelper.GetEmployee(UserAccountID).UserName,
                        eFormID,
                        ref zacKeyTable,
                        ref RET2Table);
                    //check.CommitWork();
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Finished Approve");
                    #endregion Call SAP BAPI

                    #region Update Value To WebApp
                    if (GetReturnStatus(RET2Table))
                    {
                        listOrigin[i].DocStatus = "A";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listOrigin[0]);
                    }
                    #endregion Update Value To WebApp

                    #region SaveReturnPostingDataToDatabase
                    this.SaveReturnPostingDataToDatabase(
                        listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind + "-Approve",
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
                    bapiReturn.ComCode = listOrigin[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listOrigin[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listOrigin[i].DocSeq;
                    bapiReturn.ApproveStatus = RET2Table[0].Type;
                    bapiReturn.ApproveReturn = RET2Table;
                    ApproveReturn.Add(bapiReturn);
                }
            }

            #endregion

            // connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน
            #region connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_EFORMID checkWork = new SAPProxy_YBAPI_EFORMID(workDestination);

            var listWork = listBAPIACHE09.Where(t => !t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listWork.Count; i++)
            {
                string strDocId = GetDocumentStatus(listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind);
                if (strDocId.Equals("P"))
                {
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

                    #region GetPostingDataByDocId
                    this.GetPostingDataByDocId(
                        listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind,
                        ref listBAPIACEXTC, ref listBAPIACPA09,
                        ref listBAPIACAP09, ref listBAPIACGL09,
                        ref listBAPIACTX09, ref listBAPIACCR09,
                        ref listBAPIACAR09);
                    #endregion GetPostingDataByDocId()

                    #region Call SAP BAPI
                    ZACCKEYTable zacKeyTable = new ZACCKEYTable();
                    ZACCKEY zacKey = new ZACCKEY();

                    if (listWork[i].DocAppFlag == null || listWork[i].DocAppFlag.ToString() == "")
                        zacKey.App_Type = "A";
                    else
                        zacKey.App_Type = listWork[i].DocAppFlag;

                    zacKey.Bukrs = listWork[i].CompCode;
                    zacKey.Belnr = listWork[i].FiDoc;
                    zacKey.Gjahr = listWork[i].DocYear;
                    zacKeyTable.Add(zacKey);

                    string eFormID = listWork[0].RefDocNo.Substring(0, 3) + listWork[0].RefDocNo.Substring(listWork[0].RefDocNo.Length - 7, 7);

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Start Approve");
                    checkWork.Ybapi_Eformid(
                        SAPUIHelper.GetEmployee(UserAccountID).UserName,
                        eFormID,
                        ref zacKeyTable,
                        ref RET2Table);
                    //check.CommitWork();
                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Finished Approve");
                    #endregion Call SAP BAPI

                    #region Update Value To WebApp
                    if (GetReturnStatus(RET2Table))
                    {
                        listWork[i].DocStatus = "A";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listWork[0]);
                    }
                    #endregion Update Value To WebApp

                    #region SaveReturnPostingDataToDatabase
                    this.SaveReturnPostingDataToDatabase(
                        listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind + "-Approve",
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
                    bapiReturn.ComCode = listWork[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listWork[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listWork[i].DocSeq;
                    bapiReturn.ApproveStatus = RET2Table[0].Type;
                    bapiReturn.ApproveReturn = RET2Table;
                    ApproveReturn.Add(bapiReturn);
                }
            }

            #endregion

            #region Update Document Table
            SCG.eAccounting.BLL.Implement.SCGDocumentService scgService = new SCG.eAccounting.BLL.Implement.SCGDocumentService();

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

        private IList<BAPIReverseReturn> ICBAPIReverse(RfcDestination orginalDestination, RfcDestination workDestination, long DocId, string DocKind, bool orginalUseEcc, bool workUseEcc)
        {
            if (IsOpenSimulator())
                return ReverseWithoutSAP(DocId, DocKind);

            log4net.ILog bapilogger = log4net.LogManager.GetLogger("BapiLog");
            IList<BAPIReverseReturn> ReverseReturn = new List<BAPIReverseReturn>();
            IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(DocId, DocKind);

            //connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2
            #region connect SAP สำหรับ post คู่บัญชี ต้นสังกัด ชุดที่ 1, 2

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_DOCUMENT_POST_REVERSE reverseOrigin = new SAPProxy_YBAPI_DOCUMENT_POST_REVERSE(orginalDestination);

            var listOrigin = listBAPIACHE09.Where(t => t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listOrigin.Count; i++)
            {
                string strDocId = GetDocumentStatus(listOrigin[i].DocId, listOrigin[i].DocSeq, listOrigin[i].DocKind);
                if (strDocId.Equals("P") || strDocId.Equals("A"))
                {
                    #region BAPI Service
                    string strComID = listOrigin[i].CompCode;
                    string strFIDOC = listOrigin[i].FiDoc;
                    string strDocYear = listOrigin[i].DocYear;
                    string strPstDate = listOrigin[i].ReverseDate;
                    string strReason = "01";

                    string strFiDocOld = "";
                    string strFiDocReverse = "";

                    #region Call BAPI
                    string strOutCompanyCode;
                    string strOutDocId;
                    string strOutDocYear;
                    string strOutFlag;
                    string strOutMsg;

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Start Reverse");
                    reverseOrigin.Ybapi_Document_Post_Reverse(
                        strComID, strFIDOC, strDocYear, strPstDate, strReason,
                        out strOutCompanyCode, out strOutDocId, out strOutDocYear, out strOutFlag, out strOutMsg);

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listOrigin[i].DocSeq + " Finished Reverse");
                    #endregion Call BAPI

                    #region Update Value To WebApp
                    if (strOutDocId != "" && strOutFlag != "E" && strOutFlag != "e")
                    {
                        #region History
                        strFiDocOld = listOrigin[i].FiDoc;
                        strFiDocReverse = strOutDocId;

                        Bapireverse bapiReverse = new Bapireverse();
                        bapiReverse.DocId = listOrigin[i].DocId;
                        bapiReverse.DocKind = listOrigin[i].DocKind;
                        bapiReverse.DocSeq = listOrigin[i].DocSeq;
                        bapiReverse.DocYear = listOrigin[i].DocYear;
                        bapiReverse.DocAppFlag = listOrigin[i].DocAppFlag;
                        bapiReverse.FiDoc = listOrigin[i].FiDoc;
                        bapiReverse.ObjKey = listOrigin[i].ObjKey;
                        bapiReverse.ObjSys = listOrigin[i].ObjSys;
                        bapiReverse.ObjType = listOrigin[i].ObjType;
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

                        listOrigin[i].ReverseDoc = strOutDocId;
                        listOrigin[i].ReasonRev = strReason;
                        listOrigin[i].DocStatus = "S";

                        listOrigin[i].DocYear = "";
                        listOrigin[i].FiDoc = "";
                        listOrigin[i].ObjKey = "";
                        listOrigin[i].ObjSys = "";
                        listOrigin[i].ObjType = "";
                        listOrigin[i].HeaderTxt = "";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listOrigin[0]);
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
                    BAPIRET2Table RET2Table = new BAPIRET2Table();
                    BAPIRET2 RET2 = new BAPIRET2();
                    RET2.Type = strOutFlag;
                    RET2.Message = strMsg;
                    RET2Table.Add(RET2);

                    BAPIReverseReturn bapiReturn = new BAPIReverseReturn();
                    bapiReturn.ComCode = listOrigin[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listOrigin[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listOrigin[i].DocSeq;
                    bapiReturn.OutCompanyCode = strOutCompanyCode;
                    bapiReturn.OutDocId = strOutDocId;
                    bapiReturn.OutDocYear = strOutDocYear;
                    bapiReturn.OutFlag = strOutFlag;
                    bapiReturn.OutMsg = strOutMsg;
                    bapiReturn.ReverseStatus = strOutFlag;
                    bapiReturn.ReverseReturn = RET2Table;

                    ReverseReturn.Add(bapiReturn);
                    #endregion Return to From


                    #region SaveReturnPostingDataToDatabase
                    Bapiret2 tmp = new Bapiret2();
                    tmp.DocId = listOrigin[i].DocId;
                    tmp.DocSeq = listOrigin[i].DocSeq;
                    tmp.DocKind = listOrigin[i].DocKind + "-Reverse";
                    tmp.Message = SAPUIHelper.SubString(220, strMsg);
                    tmp.MessageV1 = SAPUIHelper.SubString(50, strOutMsg);
                    tmp.MessageV2 = SAPUIHelper.SubString(50, "Old FI DOC : " + strFiDocOld);
                    tmp.MessageV3 = SAPUIHelper.SubString(50, "FI DOC Reverse : " + strFiDocReverse);
                    tmp.Type = SAPUIHelper.SubString(1, strOutFlag);

                    tmp.Active = true;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    BapiServiceProvider.Bapiret2Service.Save(tmp);
                    #endregion SaveReturnPostingDataToDatabase

                    #endregion BAPI Service
                }
            }

            #endregion

            // connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน
            #region connect SAP สำหรับ post คู่บัญชี ปฏิบัติงาน

            SCG.eAccounting.SAP.BAPI.SAPProxy_YBAPI_DOCUMENT_POST_REVERSE reverseWork = new SAPProxy_YBAPI_DOCUMENT_POST_REVERSE(workDestination);

            var listWork = listBAPIACHE09.Where(t => !t.DocSeq.StartsWith("B2C")).Select(t => t).ToList();

            for (int i = 0; i < listWork.Count; i++)
            {
                string strDocId = GetDocumentStatus(listWork[i].DocId, listWork[i].DocSeq, listWork[i].DocKind);
                if (strDocId.Equals("P") || strDocId.Equals("A"))
                {
                    #region BAPI Service
                    string strComID = listWork[i].CompCode;
                    string strFIDOC = listWork[i].FiDoc;
                    string strDocYear = listWork[i].DocYear;
                    string strPstDate = listWork[i].ReverseDate;
                    string strReason = "01";

                    string strFiDocOld = "";
                    string strFiDocReverse = "";

                    #region Call BAPI
                    string strOutCompanyCode;
                    string strOutDocId;
                    string strOutDocYear;
                    string strOutFlag;
                    string strOutMsg;

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Start Reverse");
                    reverseWork.Ybapi_Document_Post_Reverse(
                        strComID, strFIDOC, strDocYear, strPstDate, strReason,
                        out strOutCompanyCode, out strOutDocId, out strOutDocYear, out strOutFlag, out strOutMsg);

                    bapilogger.Info("DocID:" + DocId + ",DocKind:" + DocKind + ",DocSeq:" + listWork[i].DocSeq + " Finished Reverse");
                    #endregion Call BAPI

                    #region Update Value To WebApp
                    if (strOutDocId != "" && strOutFlag != "E" && strOutFlag != "e")
                    {
                        #region History
                        strFiDocOld = listWork[i].FiDoc;
                        strFiDocReverse = strOutDocId;

                        Bapireverse bapiReverse = new Bapireverse();
                        bapiReverse.DocId = listWork[i].DocId;
                        bapiReverse.DocKind = listWork[i].DocKind;
                        bapiReverse.DocSeq = listWork[i].DocSeq;
                        bapiReverse.DocYear = listWork[i].DocYear;
                        bapiReverse.DocAppFlag = listWork[i].DocAppFlag;
                        bapiReverse.FiDoc = listWork[i].FiDoc;
                        bapiReverse.ObjKey = listWork[i].ObjKey;
                        bapiReverse.ObjSys = listWork[i].ObjSys;
                        bapiReverse.ObjType = listWork[i].ObjType;
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

                        listWork[i].ReverseDoc = strOutDocId;
                        listWork[i].ReasonRev = strReason;
                        listWork[i].DocStatus = "S";

                        listWork[i].DocYear = "";
                        listWork[i].FiDoc = "";
                        listWork[i].ObjKey = "";
                        listWork[i].ObjSys = "";
                        listWork[i].ObjType = "";
                        listWork[i].HeaderTxt = "";
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listWork[0]);
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
                    BAPIRET2Table RET2Table = new BAPIRET2Table();
                    BAPIRET2 RET2 = new BAPIRET2();
                    RET2.Type = strOutFlag;
                    RET2.Message = strMsg;
                    RET2Table.Add(RET2);

                    BAPIReverseReturn bapiReturn = new BAPIReverseReturn();
                    bapiReturn.ComCode = listWork[i].CompCode;
                    bapiReturn.ComName = SAPUIHelper.GetCompanyByCode(listWork[i].CompCode).CompanyName;

                    bapiReturn.DOCSEQ = listWork[i].DocSeq;
                    bapiReturn.OutCompanyCode = strOutCompanyCode;
                    bapiReturn.OutDocId = strOutDocId;
                    bapiReturn.OutDocYear = strOutDocYear;
                    bapiReturn.OutFlag = strOutFlag;
                    bapiReturn.OutMsg = strOutMsg;
                    bapiReturn.ReverseStatus = strOutFlag;
                    bapiReturn.ReverseReturn = RET2Table;

                    ReverseReturn.Add(bapiReturn);
                    #endregion Return to From

                    #region SaveReturnPostingDataToDatabase
                    Bapiret2 tmp = new Bapiret2();
                    tmp.DocId = listWork[i].DocId;
                    tmp.DocSeq = listWork[i].DocSeq;
                    tmp.DocKind = listWork[i].DocKind + "-Reverse";
                    tmp.Message = SAPUIHelper.SubString(220, strMsg);
                    tmp.MessageV1 = SAPUIHelper.SubString(50, strOutMsg);
                    tmp.MessageV2 = SAPUIHelper.SubString(50, "Old FI DOC : " + strFiDocOld);
                    tmp.MessageV3 = SAPUIHelper.SubString(50, "FI DOC Reverse : " + strFiDocReverse);
                    tmp.Type = SAPUIHelper.SubString(1, strOutFlag);

                    tmp.Active = true;
                    tmp.CreBy = 1;
                    tmp.CreDate = DateTime.Now;
                    tmp.UpdBy = 1;
                    tmp.UpdDate = DateTime.Now;
                    tmp.UpdPgm = "POSTING";

                    BapiServiceProvider.Bapiret2Service.Save(tmp);
                    #endregion SaveReturnPostingDataToDatabase

                    #endregion BAPI Service
                }
            }

            #endregion

            #region Update Document Table
            SCG.eAccounting.BLL.Implement.SCGDocumentService scgService = new SCG.eAccounting.BLL.Implement.SCGDocumentService();

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
            #endregion

            return ReverseReturn;
        }
    }
}
