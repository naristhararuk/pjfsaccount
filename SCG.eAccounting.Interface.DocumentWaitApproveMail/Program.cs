using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.DocumentWaitApproveMail.DAL;
using SCG.eAccounting.DTO.ValueObject;
using System.Collections;
using SS.DB.Query;
using NHibernate;



namespace SCG.eAccounting.Interface.DocumentWaitApproveMail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Factory.CreateObject();
                Factory.SCGDocumentService.SendEmailToOverDueDate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
