using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;

namespace DeleteFilesService
{
    public class AppEntry
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.ShowMessage("Inprocess "+DateTime.Now);
                Dispatcher dispatcher = new Dispatcher(args);
                dispatcher.Run();
                Log.ShowMessage("Completed "+DateTime.Now);

            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }
        }
     
    }
}
