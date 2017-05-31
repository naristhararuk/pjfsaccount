using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.DocumentReminder.DAL;
using System.IO;

namespace SCG.eAccounting.Interface.DocumentReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory.CreateObject();
            StreamWriter sw;
            string FilePath = "Log\\DocumentReminder_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt";
            try
            {
                sw = new StreamWriter(FilePath);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Log");
                sw = new StreamWriter(FilePath);
            }
        
            sw.WriteLine("Program start.");
            IList<long> documentIdList = Factory.SCGDocumentQuery.GetDocumentIDFollowUpList();
            foreach (long item in documentIdList)
            {
                sw.WriteLine("Remind document no :" + item.ToString());
                Factory.SCGEmailService.SendEmailEM09(item, string.Empty, string.Empty, "Auto");
            }
            sw.WriteLine("Program stop.");
            sw.Close();

        }
    }
}
