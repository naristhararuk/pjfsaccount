using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.EmailResender.DAL;
using System.Diagnostics;
using System.Threading;

namespace SCG.eAccounting.EmailResender
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Console.WriteLine("Another Instance of the application is already running!");
                Process.GetCurrentProcess().Kill();
            }
            while (true)
            {
                Console.WriteLine("Email Resending Process ...");
                Factory.CreateObject();
                Factory.SCGEmailService.ResendEmail();
                Console.WriteLine("Finish");
                Thread.Sleep(SS.DB.Query.ParameterServices.EmailFlushingDuration*60000);
            }
        }
    }
}
