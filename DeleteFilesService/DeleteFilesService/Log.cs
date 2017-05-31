using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace DeleteFilesService
{
    public class Log
    {
        public static void WriteLogs(Exception ex)
        {
            try
            {
                string path = Application.StartupPath + "Log_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt";

                if ((!File.Exists(path)))
                {
                    File.Create(path).Close();
                }
                string msg = string.Empty;

                using (StreamWriter w = File.AppendText(path))
                {
                    msg = ex.Message;
                    w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in : " + ex.GetBaseException().ToString() + ". Error Message:" + msg;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
                ShowMessage(msg);
            }
            catch (Exception ext)
            {
                string error = ext.Message;
            }

        }
        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
