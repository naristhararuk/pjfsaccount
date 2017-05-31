using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.AdvanceOverDueEmail.DAL;
using SCG.eAccounting.DTO.ValueObject;
using System.Collections;
using SS.DB.Query;
using NHibernate;



namespace SCG.eAccounting.Interface.AdvanceOverDueEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Factory.CreateObject();
                Factory.AvAdvanceDocumentService.SendEmailToOverDueDate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
