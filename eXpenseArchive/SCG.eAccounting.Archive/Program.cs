using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Archive.BLL;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace SCG.eAccounting.Archive
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //call BLL/ArchiveService
                Logger.Write("..Process Archive Start..");
                ArchiveService service = new ArchiveService();
                int countError = service.ProcessArchive();
                Logger.Write(string.Format("..Finish Process, {0} Error(s)..", countError));
            }
            catch (Exception ex)
            {
                Logger.Write(ex.ToString());
            }
        }
    }
}
