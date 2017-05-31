using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SCG.eAccounting.Interface.ImportExpense.DAL;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.Interface.ImportExpense
{
    public class Program
    {
        public static string ImportPath
        {
            get;
            set;

        }
        public static string ImportFile
        {

            get
            {
                //return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "18");
                return "tne_ehrexpense_";
            }
        }

        public static void Main(string[] args)
        {
            Factory.CreateObject();
            if (args.Length == 0)
            {
                Console.WriteLine("path parameter with running program needed.");
                return;
            }
            ImportPath = args[0];
            Console.WriteLine("Program import expense started.");
            Console.WriteLine("Initialing program utilities.");
            Console.WriteLine("Clearing Temporary ..");
            string fileName = ImportPath + ImportFile + DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US")) + ".txt";
            //string fileName = @"D:\tne_ehrexpense_20141028 - Copy.txt";
            StreamReader reader;
            Factory.FnEHRexpenseTempQuery.ClearTemporary();
            try
            {
                //Encoding inputEnc = Encoding.GetEncoding("windows-874");
                reader = new StreamReader(fileName);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not fine " + fileName);
                Console.WriteLine("Program was terminated");
                return;
            }

            #region Read content
            string textline = "";
            while (!String.IsNullOrEmpty((textline = reader.ReadLine())))
            {
                //to do import code
                string[] texts = textline.Split('|');
                FnehRexpenseTemp ehrExpense = new FnehRexpenseTemp();
                ehrExpense.EHRExpenseID = texts[0]; //50
                ehrExpense.ExpenseDate = DateTime.ParseExact(texts[1], "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                ehrExpense.ReceiverUserCode = texts[2]; //50
                ehrExpense.ApproverUserCode = texts[3];//50
                ehrExpense.CreateUserCode = texts[4];//50
                ehrExpense.ApproveDate = DateTime.ParseExact(texts[5], "yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                if (texts[6] == "T")
                {
                    ehrExpense.PaymentType = "TR";
                }
                else
                {
                    ehrExpense.PaymentType = "CQ";
                }

                ehrExpense.ExpenseComCode = texts[7];//50
                ehrExpense.TotalAdvance = 0;
                ehrExpense.TotalExpense = string.IsNullOrEmpty(texts[12]) ? 0 : SS.Standard.Utilities.Utilities.ParseDouble(texts[12]);//UIHelper.ParseDouble(texts[12]);
                ehrExpense.ItemID = texts[8]; //5
                ehrExpense.AccountCode = texts[9]; //10
                ehrExpense.CostCentercode = texts[10]; //50
                ehrExpense.InvoiceDescription = texts[11]; //100
                ehrExpense.InvoiceBaseAmount = string.IsNullOrEmpty(texts[12]) ? 0 : SS.Standard.Utilities.Utilities.ParseDouble(texts[12]);//UIHelper.ParseDouble(texts[12]);

                ehrExpense.Flag_NR = texts[13];//1
                ehrExpense.ReferID = texts[14];//15
                try
                {
                    ehrExpense.IONumber = texts[15];
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Missing io number. . .");
                }
                try
                {
                    Factory.FnEHRexpenseTempService.Save(ehrExpense);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("EHR Expense ID : {0}, ExpenseDate : {1}, ApproveDate : {2}",  ehrExpense.EHRExpenseID, ehrExpense.ExpenseDate, ehrExpense.ApproveDate));
                    Console.WriteLine(ex.ToString());
                    //continue;
                }
            }
            #endregion
            try
            {
                //Console.WriteLine("Resolving ID.");
                //Factory.FnEHRexpenseTempQuery.ResolveID();
                //Console.WriteLine("Resolveing detail ID.");
                //Factory.FnEHRexpenseTempQuery.ResolvePBSERVICEID();
                //Console.WriteLine("Process documenting.");
                //Factory.FnEHRexpenseTempQuery.CommitNewExpense();
                //Console.WriteLine("Finish.");
                Factory.FnEHRexpenseTempQuery.ImportExpense();
                Console.WriteLine("Finish.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
