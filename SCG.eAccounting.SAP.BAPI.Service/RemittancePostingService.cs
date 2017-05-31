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

namespace SCG.eAccounting.SAP.BAPI.Service
{
    public class RemittancePostingService : PostingService
    {
        public override void CreatePostData(long DocID,string DocKind)
        {
            Hashtable paramete = new Hashtable();
            paramete.Add("@DOCUMENT_ID", DocID.ToString());

            DataSet dstPosting = new DBManage().ExecuteQuery("REMITANCE_POSTING", paramete);

            if (dstPosting.Tables[0].Rows.Count >= 1)
            {
                #region HEAD
                Bapiache09 che09 = new Bapiache09();
                che09.DocId         = DocID;
                che09.DocSeq        = "M";
                che09.DocKind       = DocKind;
                che09.DocStatus     = "N";

                che09.BusAct        = PostingConst.BusAct;
                che09.Username      = PostingConst.UserCPIC;
                che09.CompCode      = dstPosting.Tables[0].Rows[0]["CompanyCode"].ToString();
                che09.DocDate       = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                che09.DocType       = DocTypeConst.SV;
                che09.PstngDate     = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                che09.ReverseDate   = dstPosting.Tables[0].Rows[0]["PostingDate"].ToString();
                che09.RefDocNo      = SAPUIHelper.SubString(16,dstPosting.Tables[0].Rows[0]["DocumentNo"].ToString());

                che09.Active        = true;
                che09.CreBy         = 1;
                che09.CreDate       = DateTime.Now;
                che09.UpdBy         = 1;
                che09.UpdDate       = DateTime.Now;
                che09.UpdPgm        = "RemetancePosting";
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
                cextc.UpdPgm    = "RemetancePosting";
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
                cextc1.UpdPgm   = "RemetancePosting";
                BapiServiceProvider.BapiacextcService.Save(cextc1);
                #endregion Foolter

                #region คู่บัญชี

                
                int intSeq = 0;
                
                for (int i = 0; i < dstPosting.Tables[0].Rows.Count; i++)
                {
                    #region Credit
                    intSeq++;

                    Bapiacap09 capItem1 = new Bapiacap09();
                    capItem1.DocId          = DocID;
                    capItem1.DocSeq         = "M";
                    capItem1.DocKind        = DocKind;
                    capItem1.ItemnoAcc      = intSeq.ToString();
                    capItem1.VendorNo       = SAPUIHelper.GetEmployee(long.Parse(dstPosting.Tables[0].Rows[i]["RequesterID"].ToString())).EmployeeID;
                    capItem1.AllocNmbr      = dstPosting.Tables[0].Rows[i]["AdvanceNo"].ToString();
                    capItem1.AllocNmbr      = SAPUIHelper.SubString18(capItem1.AllocNmbr);
                    capItem1.ItemText       = dstPosting.Tables[0].Rows[i]["Description"].ToString();
                    capItem1.ItemText       = SAPUIHelper.SubString50(capItem1.ItemText);
                    capItem1.Businessplace  = dstPosting.Tables[0].Rows[i]["BranchCode"].ToString();
                    capItem1.TaxCode        = TaxCodeConst.NV;
                    capItem1.SpGlInd        = SpGlIndConst.E;
                    capItem1.Active         = true;
                    capItem1.CreBy          = 1;
                    capItem1.CreDate        = DateTime.Now;
                    capItem1.UpdBy          = 1;
                    capItem1.UpdDate        = DateTime.Now;
                    capItem1.UpdPgm         = "RemetancePosting";
                    BapiServiceProvider.Bapiacap09Service.Save(capItem1);

                    Bapiaccr09 accrItem1 = new Bapiaccr09();
                    accrItem1.DocId         = DocID;
                    accrItem1.DocSeq        = "M";
                    accrItem1.DocKind       = DocKind;
                    accrItem1.ItemnoAcc     = intSeq.ToString();
                    accrItem1.Currency      = PostingConst.Currency;
                    accrItem1.AmtDoccur     = 0 - decimal.Parse(dstPosting.Tables[0].Rows[i]["RemittanceAmount"].ToString());
                    accrItem1.Active        = true;
                    accrItem1.CreBy         = 1;
                    accrItem1.CreDate       = DateTime.Now;
                    accrItem1.UpdBy         = 1;
                    accrItem1.UpdDate       = DateTime.Now;
                    accrItem1.UpdPgm        = "RemetancePosting";
                    BapiServiceProvider.Bapiaccr09Service.Save(accrItem1);

                    #endregion Credit

                    #region Debit
                    intSeq++;

                    Bapiacgl09 cglItem2 = new Bapiacgl09();
                    cglItem2.DocId = DocID;
                    cglItem2.DocSeq = "M";
                    cglItem2.DocKind = DocKind;
                    cglItem2.ItemnoAcc = intSeq.ToString();
                    cglItem2.GlAccount = PostingConst.GLAccount;
                    cglItem2.AllocNmbr = dstPosting.Tables[0].Rows[i]["AdvanceNo"].ToString();
                    cglItem2.AllocNmbr = SAPUIHelper.SubString18(cglItem2.AllocNmbr);
                    cglItem2.ItemText = dstPosting.Tables[0].Rows[i]["Description"].ToString();
                    cglItem2.ItemText = SAPUIHelper.SubString50(cglItem2.ItemText);
                    //cglItem2.BusArea = dstPosting.Tables[0].Rows[i]["BranchCode"].ToString();

                    cglItem2.Active = true;
                    cglItem2.CreBy = 1;
                    cglItem2.CreDate = DateTime.Now;
                    cglItem2.UpdBy = 1;
                    cglItem2.UpdDate = DateTime.Now;
                    cglItem2.UpdPgm = "RemetancePosting";
                    BapiServiceProvider.Bapiacgl09Service.Save(cglItem2);

                    Bapiaccr09 accrItem2 = new Bapiaccr09();
                    accrItem2.DocId     = DocID;
                    accrItem2.DocSeq    = "M";
                    accrItem2.DocKind   = DocKind;
                    accrItem2.ItemnoAcc = intSeq.ToString();
                    accrItem2.Currency  = PostingConst.Currency;
                    accrItem2.AmtDoccur = decimal.Parse(dstPosting.Tables[0].Rows[i]["RemittanceAmount"].ToString());
                    accrItem2.Active    = true;
                    accrItem2.CreBy     = 1;
                    accrItem2.CreDate   = DateTime.Now;
                    accrItem2.UpdBy     = 1;
                    accrItem2.UpdDate   = DateTime.Now;
                    accrItem2.UpdPgm    = "RemetancePosting";
                    BapiServiceProvider.Bapiaccr09Service.Save(accrItem2);

                    #endregion Debit
                }
                

                #endregion คู่บัญชี
            }
        }
    }
}
