using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SCG.DB.DTO;
using SCG.DB.Helper;
using SCG.DB.Query;
using SCG.eAccounting.Interface.OrganizationImport.DAL;

namespace SCG.eAccounting.Interface.OrganizationImport
{
    class Program
    {
        static void Main(string[] args)
        {
            //string txt = "tne_org20071123.txt";
            string fileName = "tne_ehrorg_" + DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US")) + ".txt";
            string txt = args[0] + fileName;
            if (args.Length > 0)
            {
                Factory.CreateObject();
                Utility utility = new Utility();
                Factory.TmpDbOrganizationChartService.DeleteAll();
                Console.WriteLine("Delete old temp table");
                //List<TmpDbOrganizationchart> tmpOrgChartList = utility.ReadFile(txt);
                Console.WriteLine("Import data to temp table");
                try
                {
                    utility.ReadFile(txt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error. Program was terminated. => " + ex.ToString());
                    //Console.WriteLine("Couldn't find "+txt);
                    return;
                }
               
                //Factory.TmpDbOrganizationChartService.AddTmpOrganizationChartList(tmpOrgChartList);
                Factory.TmpDbOrganizationChartService.CommitImport();
                Console.WriteLine("Imported all oraganization charts");
            }
            else
                Console.WriteLine("Please select file to import"); 
        }
    }
}
