using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.FixedAdvanceOverDueEmail.DAL;
using SCG.eAccounting.DTO.ValueObject;
using System.Collections;
using SS.DB.Query;
using NHibernate;
using System.Diagnostics;

namespace SCG.eAccounting.Interface.FixedAdvanceOverDueEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Factory.CreateObject();    
                Factory.FixedAdvanceDocumentService.SendEmailToOverDueDate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                Factory.CreateObject();
                Factory.FixedAdvanceDocumentService.SendEmailToBeforeDueDate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
