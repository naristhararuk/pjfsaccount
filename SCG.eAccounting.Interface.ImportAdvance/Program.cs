using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SCG.eAccounting.Interface.ImportAdvance.DAL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Interface.ImportAdvance
{
    class Program
    {
        private static DateTime GetDateTime(string dateParam)
        {
            string text = dateParam;
            MyDateTime result;
            string[] textDateTime = text.Split(' ');
            string[] textTime;
            if (textDateTime[0].Contains('-'))
            {
                string[] textDate = textDateTime[0].Split('-');
                result.day = int.Parse(textDate[0]);
                result.month = int.Parse(textDate[1]);
                result.year = int.Parse(textDate[2]);
            }
            else if (textDateTime[0].Contains('/'))
            {
                string[] textDate = textDateTime[0].Split('/');
                result.day = int.Parse(textDate[1]);
                result.month = int.Parse(textDate[0]);
                result.year = int.Parse(textDate[2]);
            }
            else
            {
                result.day = 0;
                result.month = 0;
                result.year = 0;
            }

            try
            {
                textTime = textDateTime[1].Split(':');
                result.hour = int.Parse(textTime[0]);
                result.minute = int.Parse(textTime[1]);
                result.second = int.Parse(textTime[2]);
            }
            catch (Exception)
            {
                result.hour = 0;
                result.minute = 0;
                result.second = 0;
            }

            DateTime dt = new DateTime(result.year, result.month, result.day, result.hour, result.minute, result.second);
            return dt;
        }



        static void Main(string[] args)
        {

            Console.WriteLine("initiating system utillities ..");
            Factory.CreateObject();
            Console.WriteLine("system utillities ready.");
            if (args.Length == 0)
            {
                Console.WriteLine("Error: Argrument is null.");
                Console.WriteLine("Program was terminated.");
                return;
            }
            else
            {
                StreamReader reader = null;
                Console.WriteLine("prepare database");
                Factory.FnEACAdvanceImportTempService.ClearAll();
                Factory.FnEACAdvanceImportLogService.SetActiveFalse();
                try
                {
                    Encoding inputEnc = Encoding.GetEncoding("windows-874");
                    Console.WriteLine("try to read text resource file " + args[0].ToString());
                    reader = new StreamReader(args[0], inputEnc);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: could not find sepecified path.");
                    return;
                }
                string textLine;
                while (!string.IsNullOrEmpty(textLine = reader.ReadLine()))
                {
                    string[] textFeild = textLine.Split('|');
                    FneacAdvanceImportTemp advance = new FneacAdvanceImportTemp();
                    #region create DTO
                    advance.ExpenseNo = textFeild[0];
                    if (string.IsNullOrEmpty(advance.ExpenseNo))
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "EACExpenseID is null or empty.");
                    }
                    advance.ApproveUserCode = textFeild[53];
                    if (string.IsNullOrEmpty(advance.ApproveUserCode))
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "ApproveUserCode is null or empty.");
                    }
                    try
                    {
                        advance.ClearAdvanceDate = GetDateTime(textFeild[34]);
                    }
                    catch (Exception)
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "Invalid format : ClearAdvanceDate (" + textFeild[34] + ")");
                        advance.ClearAdvanceDate = DateTime.Now.Date;
                    }
                    try
                    {
                        advance.ClearAdvanceDueDate = GetDateTime(textFeild[35]);
                    }
                    catch (Exception)
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "Invalid format : ClearAdvanceDueDate (" + textFeild[35] + ")");
                        advance.ClearAdvanceDueDate = DateTime.Now.Date;
                    }

                    advance.ExpenseComCode = textFeild[3];
                    if (string.IsNullOrEmpty(advance.ExpenseComCode))
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "ExpenseComCode is null or empty.");
                    }
                    try
                    {
                        advance.ExpenseCreateDate = GetDateTime(textFeild[31]);
                    }
                    catch (Exception)
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "Invalid format : ExpenseCreateDate (" + textFeild[31] + ")");
                    }

                    try
                    {
                        advance.NetAmount = double.Parse(textFeild[41]);
                    }
                    catch (Exception)
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "Invalid format NetAmount : NetAmount (" + textFeild[41] + ")");
                    }
                    if (textFeild[44] == "T")
                    {
                        advance.PaymentType = "TR";
                    }
                    else if (textFeild[44] == "C")
                    {
                        advance.PaymentType = "CQ";
                    }
                    else
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "Invalid Payment medthod (" + textFeild[44] + ")");
                    }
                    
                    advance.ReceiverUserCode = textFeild[21];
                    if (string.IsNullOrEmpty(advance.ExpenseComCode))
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "ReceiverUserCode is null or empty.");
                    }
                    advance.RequesterUserCode = textFeild[14];
                    if (string.IsNullOrEmpty(advance.ExpenseComCode))
                    {
                        Factory.FnEACAdvanceImportLogService.SaveLog(advance.ExpenseNo, false, "RequesterUserCode is null or empty.");
                    }
                    advance.PBCode = textFeild[45];
                    if (textFeild[36].Length > 99)
                    {
                        advance.ReasonDue = textFeild[36].Substring(0, 99);
                    }
                    else
                    {
                        advance.ReasonDue = textFeild[36];
                    }
                    
                    if (textFeild[37].Length < 50)
                    {
                        advance.Subject = textFeild[37];
                    }
                    else
                    {
                        advance.Subject = textFeild[37].Substring(0,49);
                    }

                    #endregion
                    Factory.FnEACAdvanceImportTempService.Save(advance);
                }

            }
            Console.WriteLine("save to database");
            Factory.FnEACAdvanceImportTempService.ImportIntoDatabase();
            Console.WriteLine("Complete. See import log for more detail.");
        }
    }
    struct MyDateTime
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int second;
    }
}
