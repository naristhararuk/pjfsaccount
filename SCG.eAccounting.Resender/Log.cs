using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace SCG.eAccounting.Resender
{
    public class Log
    {
        public static void WriteLogs(Exception ex)
        {
            try
            {
                string DirectoryPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Log\";
                string FilePath =  "ResenderLog_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt";
                string fullPath = DirectoryPath + FilePath;
                if ((!File.Exists(fullPath)))
                {
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }
                    File.Create(fullPath).Close();
                }
                
                
                string msg = string.Empty;

                using (StreamWriter w = File.AppendText(fullPath))
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
                ShowMessage(ext.Message);
            }

        }
        public static void WriteMsgLogs(string msg,string filesName)
        {
            try
            {
                string path = filesName;

                if ((!File.Exists(path)))
                {
                    File.Create(path).Close();
                }
               


                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    w.WriteLine(msg);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
                ShowMessage(msg);
            }
            catch (Exception ext)
            {
                ShowMessage(ext.Message);
            }

        }
        public static void WriteMsgLogs(string msg)
        {
            try
            {
                string DirectoryPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Log\";
                string FilePath = "ResenderLog_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt";
                string fullPath = DirectoryPath + FilePath;
                if ((!File.Exists(fullPath)))
                {
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }
                    File.Create(fullPath).Close();
                }




                using (StreamWriter w = File.AppendText(fullPath))
                {
                    w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    w.WriteLine(msg);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
                ShowMessage(msg);
            }
            catch (Exception ext)
            {
                ShowMessage(ext.Message);
            }

        }
        public static void ShowMessage(string message)
        {
          //  Console.WriteLine(message);
        }
    }
}
