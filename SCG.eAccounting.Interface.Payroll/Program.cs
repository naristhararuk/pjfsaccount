using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.Interface.Payroll.DAL;
using SCG.eAccounting.BLL;
using System.Globalization;
using System.IO;
using SS.DB.Query;
using System.Net;

namespace SCG.eAccounting.Interface.Payroll
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory.CreateObject();
            ExportPayrollTextFile();
        }
        private static string ExportPath
        {
            // "D:\\PayRollReport\\"
            get { return ParameterServices.PayRollReportPath; }
        }

        private static string ExportFileName
        {
            //"PayRoll{0}{1}{2}.txt"
            get
            { return ParameterServices.PayRollReportFormatFilename; }
        }

        private static void ExportPayrollTextFile()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            string stringDate;
            string result = string.Empty;
            string companyCode = string.Empty;
            string line;
            string textLine;

            try
            {
                IList<ExportPayroll> PayrollList = Factory.FnExpenseDocumentQuery.GetExportPayrollListForInterface(date);
                StreamWriter writer = null;
                int count = 0;
                int countByCom = 0;
                foreach (ExportPayroll item in PayrollList)
                {
                    if (companyCode != item.CompanyCode)
                    {
                        count = 0;
                        countByCom = PayrollList.Where(t => t.CompanyCode == item.CompanyCode).Count();
                        companyCode = item.CompanyCode;
                        result = string.Empty;
                    }
                    count++;
                    if (item.PayrollType.ToUpper().Equals(PayrollType.Mileage))
                        stringDate = date.ToString("ddMMyyyy", new CultureInfo("en-US"));
                    else
                        stringDate = date.AddDays(1).ToString("ddMMyyyy", new CultureInfo("en-US"));
                    textLine = "{0}{1}{2}{3}{4}{5}                         {6}       ";
                    try
                    {
                        item.CompanyCode = item.CompanyCode.Substring(0, 4);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        item.CompanyCode = item.CompanyCode.PadLeft(4);
                    }
                    try
                    {
                        item.EmployeeCode = item.EmployeeCode.Insert(4, "-");
                        item.EmployeeCode = item.EmployeeCode.Substring(0, 11);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        item.EmployeeCode = item.EmployeeCode.PadLeft(11);
                    }
                    try
                    {
                        item.CostCenterCode = string.Empty;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        item.CostCenterCode = item.CostCenterCode.PadLeft(10);
                    }
                    line = string.Format(textLine, item.CompanyCode, stringDate, item.EmployeeCode, item.CostCenterCode.PadLeft(10), item.wagecode, item.totalAmount.ToString().PadRight(16), stringDate);
                    result += line;
                    if (count == countByCom)
                    {
                        try
                        {
                            String filename = string.Format(ExportFileName, item.CompanyCode, date.Year, date.Month);
                            writer = new StreamWriter(ExportPath + filename);
                            writer.Write(result);
                            writer.Flush();
                            writer.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            return;
                        }
                    }
                    else
                    { 
                        result += Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
