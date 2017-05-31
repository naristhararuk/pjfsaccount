using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.ImageFileIO.DAL;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using System.IO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.DTO;
using SS.SU.BLL;
using SS.SU.DTO;
using SCG.eAccounting.Interface.ImageFileIO.PDFService;
using System.Threading;
using SCG.eAccounting.Report;
using System.Web;
using SS.SU.Helper;
using SCG.DB.DTO.ValueObject;


namespace SCG.eAccounting.Interface.ImageFileIO
{
    public class Program
    {
        #region Define Configuration from DbParameter
        static IDbParameterQuery parameterQuery;
        private static string RootPath
        {
            get;
            set;
        }
        public static string ExportImagePath
        {
            get;
            set;
            //{
            //    return parameterQuery.getParameterByGroupNo_SeqNo("13", "1");
            //}
        }
        public static string ExportImageFile
        {
            get
            {
                //return parameterQuery.getParameterByGroupNo_SeqNo("13", "10");
                return "index";
            }
        }

        public static string ImageTypeAdv
        {
            get
            {
                return ParameterServices.AVCODE;
            }
        }
        public static string ImageTypeExp
        {
            get
            {
                return ParameterServices.EXPCODE;
            }
        }
        public static string ImageTypeRem
        {
            get
            {
                return ParameterServices.RMCODE;
            }
        }
        public static string ImportPath
        {
            get;
            set;
        }
        public static string ImportFile
        {
            get
            {
                //return parameterQuery.getParameterByGroupNo_SeqNo("13", "9");
                //return "tne_logindex";
                return "tne_logimagefile";
            }
        }




        #endregion

        private static IDbDocumentImagePostingService imagePostService;
        private static IDbDocumentImagePostingQuery imgPostingQuery;
        private static IAvAdvanceDocumentService avDocumentService;
        private static IFnExpenseDocumentService expDocumentService;
        private static IAvAdvanceDocumentQuery avDocumentQuery;
        private static IFnExpenseDocumentQuery expDocumentQuery;
        private static ISCGDocumentQuery docQuery;
        private static ISCGDocumentService docService;
        private static ISuImageToSAPLogService imgLogService;
        private static IFnRemittanceQuery fnRemittanceQuery;

        static void Main(string[] args)
        {
            Factory.CreateObject();
            UserSession us = new UserSession();

            us.UserID = SS.DB.Query.ParameterServices.ImportSystemUserID;
            us.CurrentProgramCode = "ImageFileIO";
            LocalDataStoreSlot slot = System.Threading.Thread.AllocateNamedDataSlot(SessionEnum.WebSession.UserProfiles.ToString());
            System.Threading.Thread.SetData(slot, us);

            imagePostService = Factory.DbDocumentImagePostingService;
            avDocumentService = Factory.AvAdvanceDocumentService;
            expDocumentService = Factory.FnExpenseDocumentService;
            avDocumentQuery = Factory.AvAdvanceDocumentQuery;
            expDocumentQuery = Factory.FnExpenseDocumentQuery;
            docQuery = Factory.SCGDocumentQuery;
            imgLogService = Factory.SuImageToSAPLogService;
            parameterQuery = Factory.DbParameterQuery;
            imgPostingQuery = Factory.DbDocumentImagePostingQuery;
            fnRemittanceQuery = Factory.FnRemittanceQuery;
            docService = Factory.SCGDocumentService;

            Console.WriteLine("Program Export Document Image is running.");
            Factory.CreateObject();
            if (args.Length == 0)
            {
                args = new string[1];
                Console.WriteLine("Insert \"I\" or \"O\" to import / export mode");
                args[0] = Console.ReadLine();
            }


            if (args[0].ToLower() == "i")
            {
                Console.WriteLine("Import Start.");
                ImportPath = args[1];

                ReadExportImageLogfile();
            }
            if (args[0].ToLower() == "o")
            {
                Console.WriteLine("Export Start.");
                RootPath = args[1];
                ExportImagePath = args[2];
                ExportDocumentImage();

            }





            Console.WriteLine("Program finish.");

        }

        public static void ExportDocumentImage()
        {
            DbDocumentImagePosting docImgPst;
            Console.WriteLine("Export document image utilities are initializing.");
            Console.WriteLine("Create Fresh-New ImagePosting.");
            imagePostService.ImportFreshNewImagePosting();

            IList<SapInstanceData> sapInstances = Factory.DbSapInstanceQuery.GetSapInstanceList();

            foreach (SapInstanceData sapInstance in sapInstances)
            {
                //bat file set ExportPath = root directory. etc: C:\SAPUpload\eXpense\
                string newExportPath = RootPath + sapInstance.AliasName + "\\" + ExportImagePath + "\\" + DateTime.Now.ToString("yyMMdd");
                string newExportPathTemp = RootPath + sapInstance.AliasName + "\\" + ExportImagePath + "\\Temp";
                string exportfileName = ExportImageFile + DateTime.Now.Date.ToString("yyyyMMdd");
                StreamWriter writer;
                try
                {
                    if (!Directory.Exists(newExportPath))
                    {
                        Directory.CreateDirectory(newExportPath);
                    }
                    writer = new StreamWriter(newExportPath + "\\" + exportfileName + ".txt");
                }
                catch (Exception)
                {

                    Console.WriteLine("Could not find " + newExportPath + "\\" + exportfileName + ".txt");
                    Console.WriteLine("Program was terminated.");
                    return;
                }

                Console.WriteLine("Reading data from database ...");
                IList<ExportDocumentImage> Doclist = docQuery.FindDocumentImage("N", sapInstance.Code);
                string textLineTemplate = "{0}-{1}-{2}-{3}-{4}-{5}-{6}{7}";
                int nn = 0;
                Console.WriteLine("Exporting to " + newExportPath + "\\...");

                foreach (ExportDocumentImage item in Doclist)
                {
                    nn++;
                    Console.WriteLine("Formating log representation.");
                    #region Format text line representation
                    try
                    {
                        item.CompanyCode = item.CompanyCode.Substring(0, 4);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        item.CompanyCode = item.CompanyCode.PadLeft(4);
                    }

                    try
                    {
                        item.DocNumber = item.DocNumber.Substring(0, 10);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        item.DocNumber = item.DocNumber.PadLeft(10, ' ');
                    }


                    try
                    {
                        item.ImageDocumentType = item.ImageDocumentType.Substring(0, 7);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        item.ImageDocumentType = item.ImageDocumentType.PadLeft(7, ' ');
                    }



                    nn = nn % 100;
                    string strn;

                    strn = nn.ToString();
                    if (strn.Length == 1)
                    {
                        strn = "0" + strn;

                    }

                    //nullreferance safety
                    string BoxID = "";
                    BoxID += expDocumentQuery.GetBoxIDByDocuemntID(item.DocumentID);

                    string docType = "";

                    if (item.ImageDocumentType == "Advance")
                    {
                        docType = ImageTypeAdv;
                    }
                    else if (item.ImageDocumentType == "Expense")
                    {
                        docType = ImageTypeExp;
                    }
                    else if (item.ImageDocumentType == "Remiitance")
                    {
                        docType = ImageTypeRem;
                    }
                    #endregion

                    SaveLogToDatabase(item.DocumentID.ToString(), "New", "Wait for Regenerate");

                    #region craete filename
                    string textLine = string.Format(textLineTemplate,
                        "BKPF",
                        item.FI_DOC,
                        item.DocumentDate.Year.ToString(),
                        item.CompanyCode,
                        strn,
                        docType,
                        BoxID,
                        ".pdf"
                        );
                    #endregion

                    #region Zone create PDF
                    docImgPst = imagePostService.FindByIdentity(item.DocumentID);
                    if (docImgPst == null)
                    {
                        continue;
                    }
                    
                    Console.WriteLine("Creating PDF content.");
                    string filesname = textLine;

                    try
                    {
                        byte[] pdfContent = docService.GeneratePDF(docImgPst.DocumentID);

                        FilesGenerator rp = new FilesGenerator();
                        if (!Directory.Exists(newExportPathTemp))
                        {
                            Directory.CreateDirectory(newExportPathTemp);
                        }

                        FileStream stream = File.Create(newExportPathTemp + "\\" + filesname, pdfContent.Length);
                        stream.Write(pdfContent, 0, pdfContent.Length);
                        stream.Close();


                        docImgPst.Status = "G";
                        docImgPst.ImageDocID = textLine;
                        docImgPst.Message = "Generated";
                        SaveLogToDatabase(item.DocumentID.ToString(), "New", "Generated");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DocID=" + docImgPst.DocumentID + ", FiDoc=" + docImgPst.FIDocNumber + " => " + ex.ToString());
                        docImgPst.Status = "N";
                        docImgPst.Message = "Wait for Regenerate";
                        SaveLogToDatabase(item.DocumentID.ToString(), "New", "Wait for Regenerate");
                        continue;
                    }
                    finally
                    {
                        imagePostService.Update(docImgPst);
                    }//Save log to database

                    #endregion
                }

                //Loop for create IndexTextFile
                //Query both of Fail and Generated.
                IList<ExportDocumentImage> failDoclist = docQuery.FindDocumentImage("GF", sapInstance.Code);
                Console.WriteLine("Create indexfile with generated and yesterday fail.");
                foreach (ExportDocumentImage item in failDoclist)
                {
                    if (!Directory.Exists(newExportPath))
                    {
                        Directory.CreateDirectory(newExportPath);
                    }

                    docImgPst = imagePostService.FindByIdentity(item.DocumentID);
                    Interface.Utilities.FileManager.CopyFile(newExportPathTemp + "\\" + docImgPst.ImageDocID, newExportPath + "\\" + docImgPst.ImageDocID);
                    docImgPst.Status = "P";
                    docImgPst.Message = "Posting";
                    imagePostService.Update(docImgPst);
                    writer.WriteLine(docImgPst.ImageDocID);
                    SaveLogToDatabase(item.DocumentID.ToString(), "File", "Posting");

                }
                writer.Close();
                Console.WriteLine("Finnish.");
            }

            

            //string filePath = ExportImagePath;
            //string exportfileName = ExportImageFile + DateTime.Now.Date.ToString("yyyyMMdd");
            //StreamWriter writer;
            //try
            //{
            //    if (!Directory.Exists(ExportImagePath + DateTime.Now.ToString("yyMMdd")))
            //    {
            //        Directory.CreateDirectory(ExportImagePath + DateTime.Now.ToString("yyMMdd"));
            //    }
            //    writer = new StreamWriter(ExportImagePath + DateTime.Now.ToString("yyMMdd") + "\\" + exportfileName + ".txt");
            //}
            //catch (Exception)
            //{

            //    Console.WriteLine("Could not find " + exportfileName + exportfileName + ".txt");
            //    Console.WriteLine("Program was terminated.");
            //    return;
            //}

            //Console.WriteLine("Reading data from database ...");
            //IList<ExportDocumentImage> Doclist = docQuery.FindDocumentImage("N");
            //string textLineTemplate = "{0}-{1}-{2}-{3}-{4}-{5}-{6}{7}";
            //int nn = 0;
            //Console.WriteLine("Exporting to " + filePath + "...");

            //foreach (ExportDocumentImage item in Doclist)
            //{
            //    nn++;
            //    Console.WriteLine("Formating log representation.");
            //    #region Format text line representation
            //    try
            //    {
            //        item.CompanyCode = item.CompanyCode.Substring(0, 4);
            //    }
            //    catch (ArgumentOutOfRangeException)
            //    {
            //        item.CompanyCode = item.CompanyCode.PadLeft(4);
            //    }

            //    try
            //    {
            //        item.DocNumber = item.DocNumber.Substring(0, 10);
            //    }
            //    catch (ArgumentOutOfRangeException)
            //    {
            //        item.DocNumber = item.DocNumber.PadLeft(10, ' ');
            //    }


            //    try
            //    {
            //        item.ImageDocumentType = item.ImageDocumentType.Substring(0, 7);
            //    }
            //    catch (ArgumentOutOfRangeException)
            //    {
            //        item.ImageDocumentType = item.ImageDocumentType.PadLeft(7, ' ');
            //    }



            //    nn = nn % 100;
            //    string strn;

            //    strn = nn.ToString();
            //    if (strn.Length == 1)
            //    {
            //        strn = "0" + strn;

            //    }

            //    //nullreferance safety
            //    string BoxID = "";
            //    BoxID += expDocumentQuery.GetBoxIDByDocuemntID(item.DocumentID);

            //    string docType = "";

            //    if (item.ImageDocumentType == "Advance")
            //    {
            //        docType = ImageTypeAdv;
            //    }
            //    else if (item.ImageDocumentType == "Expense")
            //    {
            //        docType = ImageTypeExp;
            //    }
            //    else if (item.ImageDocumentType == "Remiitance")
            //    {
            //        docType = ImageTypeRem;
            //    }
            //    #endregion

            //    SaveLogToDatabase(item.DocumentID.ToString(), "New", "Wait for Regenerate");

            //    #region craete filename
            //    string textLine = string.Format(textLineTemplate,
            //        "BKPF",
            //        item.FI_DOC,
            //        item.DocumentDate.Year.ToString(),
            //        item.CompanyCode,
            //        strn,
            //        docType,
            //        BoxID,
            //        ".pdf"
            //        );
            //    #endregion

            //    string tempFodler = ExportImagePath + "temp";
            //    Console.WriteLine("Creating PDF content.");

            //    #region Zone create PDF
            //    docImgPst = imagePostService.FindByIdentity(item.DocumentID);
            //    if (docImgPst == null)
            //    {
            //        continue;
            //    }

            //    string filesname = textLine;
            //    Console.WriteLine("Creating PDF content.");
            //    try
            //    {
            //        byte[] pdfContent = docService.GeneratePDF(docImgPst.DocumentID);

            //        FilesGenerator rp = new FilesGenerator();
            //        if (!Directory.Exists(tempFodler))
            //        {
            //            Directory.CreateDirectory(tempFodler);
            //        }

            //        FileStream stream = File.Create(tempFodler + "\\" + filesname, pdfContent.Length);
            //        stream.Write(pdfContent, 0, pdfContent.Length);
            //        stream.Close();


            //        docImgPst.Status = "G";
            //        docImgPst.ImageDocID = textLine;
            //        docImgPst.Message = "Generated";
            //        SaveLogToDatabase(item.DocumentID.ToString(), "New", "Generated");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("DocID=" + docImgPst.DocumentID + ", FiDoc=" + docImgPst.FIDocNumber + " => " + ex.ToString());
            //        docImgPst.Status = "N";
            //        docImgPst.Message = "Wait for Regenerate";
            //        SaveLogToDatabase(item.DocumentID.ToString(), "New", "Wait for Regenerate");
            //        continue;
            //    }
            //    finally
            //    {
            //        imagePostService.Update(docImgPst);
            //    }//Save log to database

            //    #endregion
            //}

            ////Loop for create IndexTextFile
            ////Query both of Fail and Generated.
            //IList<ExportDocumentImage> failDoclist = docQuery.FindDocumentImage("GF");
            //Console.WriteLine("Create indexfile with generated and yesterday fail.");
            //foreach (ExportDocumentImage item in failDoclist)
            //{
            //    string outputFolder = ExportImagePath + "\\" + DateTime.Now.ToString("yyMMdd");
            //    if (!Directory.Exists(outputFolder))
            //    {
            //        Directory.CreateDirectory(outputFolder);
            //    }

            //    docImgPst = imagePostService.FindByIdentity(item.DocumentID);
            //    Interface.Utilities.FileManager.CopyFile(ExportImagePath + "temp\\" + docImgPst.ImageDocID, ExportImagePath + DateTime.Now.ToString("yyMMdd") + "\\" + docImgPst.ImageDocID);
            //    docImgPst.Status = "P";
            //    docImgPst.Message = "Posting";
            //    imagePostService.Update(docImgPst);
            //    writer.WriteLine(docImgPst.ImageDocID);
            //    SaveLogToDatabase(item.DocumentID.ToString(), "File", "Posting");

            //}
            //writer.Close();
            //Console.WriteLine("Finnish.");


        }

        private static void SaveLogToDatabase(string DocID, string status, string message)
        {
            Console.WriteLine("Save log to database.");
            SuImageTosapLog imageToSAPLog = new SuImageTosapLog();
            imageToSAPLog.Message = message;
            imageToSAPLog.RequestNo = DocID;
            imageToSAPLog.Status = status;
            imageToSAPLog.Active = true;
            imageToSAPLog.SubmitDate = DateTime.Now.Date;
            imageToSAPLog.UpdPgm = "Interface";
            imageToSAPLog.UpdBy = 1;
            imageToSAPLog.CreBy = 1;
            imageToSAPLog.CreDate = DateTime.Now;
            imageToSAPLog.UpdDate = DateTime.Now;
            imgLogService.Save(imageToSAPLog);
        }

        public static void ReadExportImageLogfile()
        {
            Console.WriteLine("Read yesterday logindexfile file for appending today indexfile.");
            StreamReader reader;

            string fileOutPath = ImportPath;
            //DateTime yesterday = DateTime.Now.AddDays(-1);
            //New version update 27/04/2009
            DateTime yesterday = DateTime.Now;
            //tne_logimageindexfile.txt
            string inPath = @ImportPath + ImportFile + yesterday.ToString("yyMMdd") + ".txt";
            Console.WriteLine("Appending index file for SAP re-read image.");
            try
            {
                reader = new StreamReader(inPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't open " + inPath);
                Console.WriteLine(ex.ToString());
                return;
            }

            string textLine;

            while ((textLine = reader.ReadLine()) != null)
            {
                string[] textField = textLine.Split('|');
                DbDocumentImagePosting docImagePost = imgPostingQuery.GetDocumentImagePostingByImageID(textField[0]);
                if (docImagePost == null)
                {
                    continue;
                }
                if (textField[2] == "s" || textField[2] == "S")
                {
                    try
                    {
                        Interface.Utilities.FileManager.DeleteFile(ImportPath + "temp\\" + docImagePost.ImageDocID);
                    }
                    catch (Exception)
                    {

                    }
                    docImagePost.Status = textField[2];
                    docImagePost.Message = textField[3];
                    docImagePost.ImageDocID = textField[1];
                    SaveLogToDatabase(docImagePost.DocumentID.ToString(), "Success", docImagePost.Message);
                    //call Notify function.
                    //imagePostService.NotifyImageFile(docImagePost.DocumentID);

                }
                else
                {
                    docImagePost.Status = "F";
                    docImagePost.Message = textField[3];
                    //docImagePost.ImageDocID = textField[1];
                    SaveLogToDatabase(docImagePost.DocumentID.ToString(), "Fail", docImagePost.Message);
                }
                imagePostService.Update(docImagePost);
            }

            Console.WriteLine("Append index file success.");
        }
    }
}
