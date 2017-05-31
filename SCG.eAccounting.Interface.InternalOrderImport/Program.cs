using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.BLL;
using SS.DB.DTO;

using SCG.DB.BLL;
using SCG.DB.DTO;

using SCG.eAccounting.Interface.InternalOrderImport.DAL;

namespace SCG.eAccounting.Interface.InternalOrderImport
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Start import InternalOrder .....");
            
            string path = args[0];
            string eccflag = args[1];
            string aliasName = string.Empty;
            string subFolder = string.Empty;
            if (eccflag == "1")
            {
                aliasName = args[2];
                subFolder = args[3];
            }
            if (path == null)
            {
                Console.WriteLine("Please enter filename .....");
            }
            else 
            {
                Console.WriteLine("Read file : {0}", path + "tne_orderno.txt");

                Importer import = new Importer(path, "tne_orderno.txt", eccflag, aliasName, subFolder);
                import.startImport();
                Console.WriteLine("Import Success .....");
            }

            
        }
    }
}
