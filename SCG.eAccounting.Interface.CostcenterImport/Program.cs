using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using SS.DB.BLL;
using SS.DB.DTO;

using SCG.DB.BLL;
using SCG.DB.DTO;

using SCG.eAccounting.Interface.CostcenterImport.DAL;

namespace SCG.eAccounting.Interface.CostcenterImport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("start import Coscenter");

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
                Console.WriteLine("Please enter filename");

            }
            else
            {
                Console.WriteLine(" read file : {0}", path);
                Importer import = new Importer(path, eccflag,aliasName,subFolder);
                try
                {
                    import.startImport();
                }
                catch (IOException)
                {

                    Console.WriteLine("Couldn't find " + path);
                    Console.WriteLine("Program was terminated.");
                    return;
                }

                Console.WriteLine("End Import");
            }

        }



    }
}
