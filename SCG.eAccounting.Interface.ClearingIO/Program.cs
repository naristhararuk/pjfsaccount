using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SCG.eAccounting.Interface.ClearingIO.DAL;
using SCG.eAccounting.DTO.ValueObject;
using System.IO;
using SCG.eAccounting.DTO;
using System.Text;
using SS.Standard.Security;
using SS.SU.BLL;
using SCG.eAccounting.Interface.Utilities;
using SS.SU.DTO;
using SS.DB.Query;
using SS.Standard.Utilities;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Interface.ClearingIO
{
    public class Program
    {

        #region Configuration Defination
        private static string ExportPath
        {
            get;
            set;
        }

        private static string RootPath
        {
            get;
            set;
        }

        private static string ExportFileName
        {
            get
            {
                return "tne_checkclearing";
            }
            //{
            //    return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "7");
            //}
        }
        private static string ImportFilePath
        {
            get;
            set;
            //get
            //{
            //    return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "16");
            //}
        }
        private static string ImportFileName
        {
            get
            {
                //return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "17");
                return "tne_logcheckclearing.txt";
            }
        }
        #endregion

        public static void Main(string[] args)
        {
            Console.WriteLine("Batch start.");
            Factory.CreateObject();


            if (args.Length == 0)
            {
                args = new string[1];
                Console.WriteLine("Insert \"I\" or \"O\" to import / export mode");
                args[0] = Console.ReadLine();
            }

            IUserEngineService userEngine = (IUserEngineService)ObjectManager.GetObject("UserEngineService");
            SuUser user = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(ParameterServices.ImportSystemUserID);
            userEngine.SignIn(user.UserName);

            IUserAccount userAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
            userAccount.CurrentProgramCode = "Interface";

            if (args[0].ToLower() == "o")
            {
                Console.WriteLine("Export Start.");
                RootPath = args[1];
                ExportPath = args[2];
                Export();
            }
            if (args[0].ToLower() == "i")
            {
                Console.WriteLine("Import Start.");
                ImportFilePath = args[1];
                Import();
            }


        }
        private static void Import()
        {
            //clear temp
            #region Import Autopayment
            Console.WriteLine("Program clearing batch start.");
            Factory.FnAutoPaymentTempService.ClearTemporary();

            Console.WriteLine("Clear temporary data.");

            Encoding inputEnc = Encoding.GetEncoding("windows-874");

            StreamReader sr;
            try
            {
                sr = new StreamReader(ImportFilePath + ImportFileName, inputEnc);
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't find " + ImportFilePath + ImportFileName);
                Console.WriteLine("No any importing file.");
                return;
            }
            string textline;
            List<long> doclist = new List<long>();
            while ((textline = sr.ReadLine()) != null)
            {
                #region Create DTO and Save to temp
                try
                {
                    string[] textfeild = textline.Split('|');
                    FnAutoPaymentTemp autoPayment = new FnAutoPaymentTemp();
                    autoPayment.Active = true;
                    autoPayment.Amount = string.IsNullOrEmpty(textfeild[14]) ? 0 : SS.Standard.Utilities.Utilities.ParseDouble(textfeild[14]); //UIHelper.ParseDouble(textfeild[14]);
                    autoPayment.ChequeBankName = textfeild[24];
                    autoPayment.ChequeDate = DateTime.ParseExact(textfeild[9], "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    autoPayment.ChequeNumber = textfeild[23];
                    autoPayment.CreBy = SS.DB.Query.ParameterServices.ImportSystemUserID;
                    autoPayment.ClearingDocID = textfeild[3];
                    autoPayment.CreDate = DateTime.Now;
                    autoPayment.CurrencyDoc = textfeild[10];
                    autoPayment.CurrencyPay = textfeild[25];
                    autoPayment.FIDoc = textfeild[6];
                    autoPayment.PayeeBankAccountNumber = textfeild[28];
                    autoPayment.PayeeBankName = textfeild[29];
                    autoPayment.PaymentDate = DateTime.ParseExact(textfeild[9], "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    autoPayment.Status = 2;
                    autoPayment.UpdBy = SS.DB.Query.ParameterServices.ImportSystemUserID;
                    autoPayment.UpdDate = DateTime.Now;
                    autoPayment.UpdPgm = "Interface";
                    autoPayment.year = textfeild[5];
                    autoPayment.companycode = textfeild[0];
                    Factory.FnAutoPaymentTempService.Save(autoPayment);
                    //find documentid by fi doc
                    string comCode = textfeild[0];
                    string DocYear = textfeild[5];
                    long documentid = Factory.FnAutoPaymentQuery.GetDocumentIDByFIDOCID(autoPayment.FIDoc, comCode, DocYear);

                    doclist.Add(documentid);

                }
                catch (Exception ex)
                {
                    Console.Out.Write(ex.ToString());
                    continue;
                }

                #endregion
            }

            #region Commit to Database

            
            foreach (long item in doclist)
            {
                try
                {

                    Factory.FnAutoPaymentTempService.NotifyImageFile(item);
                }
                catch (Exception ex)
                {
                    Console.Out.Write(ex.ToString());
                    continue;
                }
            }


            #endregion
            #endregion
        }

        private static void Export()
        {
            Console.WriteLine("Export to fidoc to sap.");

            IList<SapInstanceData> sapInstances = Factory.DbSapInstanceQuery.GetSapInstanceList();

            foreach (SapInstanceData sapInstance in sapInstances)
            {
                //bat file set ExportPath = root directory. etc: C:\SAPUpload\eXpense\
                string newExportPath = RootPath + sapInstance.AliasName + "\\" + ExportPath + "\\";

                DateTime dt = DateTime.Now;
                StreamWriter writer;
                #region Export Autopayment
                Console.WriteLine("Creating Batch AutoPayment Clearing Logfile (SAP : " + sapInstance.AliasName + ").");
                IList<ExportClearing> clearingList = Factory.FnAutoPaymentQuery.GetExportClearingListByDate(sapInstance.Code);
                try
                {
                    writer = new StreamWriter(newExportPath + ExportFileName + ".txt");
                }
                catch (Exception)
                {
                    Console.WriteLine("Program was terminated. Because directory is not existing.");
                    return;
                }
                string FormatLine = "{0}|{1}|{2}";
                foreach (ExportClearing clearing in clearingList)
                {
                    string textLine;

                    textLine = String.Format(FormatLine, clearing.CompanyCode, clearing.FIDOC, clearing.Year);
                    writer.WriteLine(textLine);

                    FnAutoPayment autoPayment;
                    autoPayment = Factory.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(string.IsNullOrEmpty(clearing.DocumentID) ? 0 : SS.Standard.Utilities.Utilities.ParseLong(clearing.DocumentID));
                    if (autoPayment != null)
                    {
                        FnAutoPayment _autoPayment = new FnAutoPayment();
                        _autoPayment.AutoPaymentID = autoPayment.AutoPaymentID;
                        Factory.FnAutoPaymentService.Delete(_autoPayment);
                    }
                    autoPayment = new FnAutoPayment();
                    autoPayment.Active = true;
                    autoPayment.Document = new SCGDocument();
                    autoPayment.Document.DocumentID = string.IsNullOrEmpty(clearing.DocumentID) ? 0 : SS.Standard.Utilities.Utilities.ParseLong(clearing.DocumentID);//UIHelper.ParseLong(clearing.DocumentID);
                    autoPayment.FIDoc = clearing.FIDOC;
                    autoPayment.companycode = clearing.CompanyCode;
                    autoPayment.year = clearing.Year;
                    autoPayment.CreDate = DateTime.Now;
                    autoPayment.UpdDate = DateTime.Now;
                    autoPayment.Status = 1;
                    autoPayment.CreBy = SS.DB.Query.ParameterServices.ImportSystemUserID;
                    autoPayment.UpdBy = SS.DB.Query.ParameterServices.ImportSystemUserID;
                    autoPayment.UpdPgm = "Interface";
                    Factory.FnAutoPaymentService.Save(autoPayment);

                }
                writer.Close();
                Console.WriteLine("Create log finish (SAP : " + sapInstance.AliasName + ").");
            }
            Console.WriteLine("Create log finish.");

            //DateTime dt = DateTime.Now;
            //StreamWriter writer;
            //#region Export Autopayment
            //Console.WriteLine("Creating Batch AutoPayment Clearing Logfile.");
            //IList<ExportClearing> clearingList = Factory.FnAutoPaymentQuery.GetExportClearingListByDate();
            //try
            //{
            //    writer = new StreamWriter(ExportPath + ExportFileName + ".txt");
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("Program was terminated. Because directory is not existing.");
            //    return;
            //}
            //string FormatLine = "{0}|{1}|{2}";
            //foreach (ExportClearing clearing in clearingList)
            //{
            //    string textLine;

            //    textLine = String.Format(FormatLine, clearing.CompanyCode, clearing.FIDOC, clearing.Year);
            //    writer.WriteLine(textLine);

            //    FnAutoPayment autoPayment;
            //    autoPayment = Factory.FnAutoPaymentQuery.GetFnAutoPaymentByDocumentID(UIHelper.ParseLong(clearing.DocumentID));
            //    if (autoPayment != null)
            //    {
            //        FnAutoPayment _autoPayment = new FnAutoPayment();
            //        _autoPayment.AutoPaymentID = autoPayment.AutoPaymentID;
            //        Factory.FnAutoPaymentService.Delete(_autoPayment);
            //    }
            //    autoPayment = new FnAutoPayment();
            //    autoPayment.Active = true;
            //    autoPayment.Document = new SCGDocument();
            //    autoPayment.Document.DocumentID = UIHelper.ParseLong(clearing.DocumentID);
            //    autoPayment.FIDoc = clearing.FIDOC;
            //    autoPayment.companycode = clearing.CompanyCode;
            //    autoPayment.year = clearing.Year;
            //    autoPayment.CreDate = DateTime.Now;
            //    autoPayment.UpdDate = DateTime.Now;
            //    autoPayment.Status = 1;
            //    autoPayment.CreBy = SS.DB.Query.ParameterServices.ImportSystemUserID;
            //    autoPayment.UpdBy = SS.DB.Query.ParameterServices.ImportSystemUserID;
            //    autoPayment.UpdPgm = "Interface";
            //    Factory.FnAutoPaymentService.Save(autoPayment);

            //}
            //writer.Close();
            //Console.WriteLine("Create log finish.");
            #endregion
        }
    }
}
