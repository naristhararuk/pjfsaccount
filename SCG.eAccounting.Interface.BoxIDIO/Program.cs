using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.BoxIDIO.DAL;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.DTO.ValueObject;
using System.IO;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL.Implement;

namespace SCG.eAccounting.Interface.BoxIDIO
{
    public class Program
    {
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
        private static string ExportFile
        {
            get
            {
                //return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "12");
                return "tne_boxidupdate";
            }
            //{
            //    return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "13");
            //}
        }
        private static string ImportPath
        {
            get;
            set;
            //{
            //    return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "14");
            //}
        }
        private static string ImportFile
        {
            get
            {
                //return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "15");
                return "tne_logboxidupdate";
            }
        }

        static void Main(string[] args)
        {
            Factory.CreateObject();
            if (args.Length == 0)
            {
                args = new string[1];
                Console.WriteLine("Insert \"I\" or \"O\" to import / export mode");
                args[0] = Console.ReadLine();
            }


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
                ImportPath = args[1];
                Import();
            }
        }
        private static void Import()
        {
            StreamReader reader;
            try
            {
                reader = new StreamReader(ImportPath + ImportFile + ".txt");

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Couldn't open " + ImportPath + ImportFile + ".txt");
                return;
            } string textLine;
            while (!string.IsNullOrEmpty(textLine = reader.ReadLine()))
            {
                string[] text = textLine.Split('|');
                DbDocumentBoxidPosting boxidPosting = new DbDocumentBoxidPosting();
                DbDocumentImagePosting docImagePosting = Factory.DbDocumentImagePostingQuery.GetDocumentImagePostingByImageID(text[0]);
                if (docImagePosting != null)
                {
                    boxidPosting.DocumentID = docImagePosting.DocumentID;
                    boxidPosting.Message = text[3];
                    boxidPosting.Status = text[2];
                    try
                    {
                        Factory.DbDocumentBoxIDPostingService.Update(boxidPosting);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
            }
        }

        private static void Export()
        {
            Console.WriteLine("BoxID export is starting.");

            IList<SapInstanceData> sapInstances = Factory.DbSapInstanceQuery.GetSapInstanceList();

            foreach (SapInstanceData sapInstance in sapInstances)
            {
                //bat file set ExportPath = root directory. etc: C:\SAPUpload\eXpense\
                string newExportPath = RootPath + sapInstance.AliasName + "\\" + ExportPath + "\\";

                Console.WriteLine("BoxID(SAP : " + sapInstance.AliasName + ") export is starting.");
                IList<ExportBoxID> exportBoxIDList = Factory.DbDocumentBoxIDPostingQuery.GetExportBoxIDList(sapInstance.Code);                
                StreamWriter writer;
                try
                {
                    writer = new StreamWriter(newExportPath + ExportFile + ".txt");
                }
                catch (Exception)
                {
                    Console.WriteLine("BoxID export is fail. Becourse of can't create export file.");
                    return;
                }
                Console.WriteLine("Writing export file...");
                foreach (ExportBoxID item in exportBoxIDList)
                {
                    DbDocumentBoxidPosting boxidPosting;
                    boxidPosting = Factory.DbDocumentBoxIDPostingService.FindByIdentity(item.DocumentID);
                    if (boxidPosting == null)
                    {
                        boxidPosting = new DbDocumentBoxidPosting();
                        boxidPosting.DocumentID = item.DocumentID;
                        boxidPosting.Status = "P";
                        boxidPosting.Message = "Posting";
                        writer.WriteLine(item.ImageDocID + "|" + item.BoxID);
                        writer.Flush();
                        Factory.DbDocumentBoxIDPostingService.Save(boxidPosting);

                    }
                    else
                    {
                        boxidPosting.DocumentID = item.DocumentID;
                        boxidPosting.Status = "P";
                        boxidPosting.Message = "Re Posting";
                        writer.WriteLine(item.ImageDocID + "|" + item.BoxID);
                        writer.Flush();
                        Factory.DbDocumentBoxIDPostingService.Update(boxidPosting);
                    }

                }
                Console.WriteLine("export BoxUpdate(SAP : " + sapInstance.AliasName + ") finish.");
            }

            Console.WriteLine("export BoxUpdate finish.");
            
            //IList<ExportBoxID> exportBoxIDList = Factory.DbDocumentBoxIDPostingQuery.GetExportBoxIDList();
            //StreamWriter writer;
            //try
            //{
            //    writer = new StreamWriter(ExportPath + ExportFile + ".txt");
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("BoxID export is fail. Becourse of can't create export file.");
            //    return;
            //}
            //Console.WriteLine("Writing export file...");
            //foreach (ExportBoxID item in exportBoxIDList)
            //{
            //    DbDocumentBoxidPosting boxidPosting;
            //    boxidPosting = Factory.DbDocumentBoxIDPostingService.FindByIdentity(item.DocumentID);
            //    if (boxidPosting == null)
            //    {
            //        boxidPosting = new DbDocumentBoxidPosting();
            //        boxidPosting.DocumentID = item.DocumentID;
            //        boxidPosting.Status = "P";
            //        boxidPosting.Message = "Posting";
            //        writer.WriteLine(item.ImageDocID + "|" + item.BoxID);
            //        writer.Flush();
            //        Factory.DbDocumentBoxIDPostingService.Save(boxidPosting);

            //    }
            //    else
            //    {
            //        boxidPosting.DocumentID = item.DocumentID;
            //        boxidPosting.Status = "P";
            //        boxidPosting.Message = "Re Posting";
            //        writer.WriteLine(item.ImageDocID + "|" + item.BoxID);
            //        writer.Flush();
            //        Factory.DbDocumentBoxIDPostingService.Update(boxidPosting);
            //    }

            //}
            //Console.WriteLine("export BoxUpdate finish.");
        }
    }
}
