using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCG.FN.DAL
{
    public class DaoProvider
    {
        public DaoProvider() { }
        public static IFnReceiptTypeDao FnReceiptTypeDao { get; set; }
        public static IFnReceiptTypeLangDao FnReceiptTypeLangDao { get; set; }
		public static IFnPaymentTypeDao FnPaymentTypeDao { get; set; }
		public static IFnPaymentTypeLangDao FnPaymentTypeLangDao { get; set; }
        public static IFnCashierDao FnCashierDao { get; set; }
        public static IFnCashierLangDao FnCashierLangDao { get; set; }
        //public static IFnExpenseDocumentDao FnExpenseDocumentDao { get; set; }
        //public static IFnExpenseInvoiceDao FnExpenseInvoiceDao { get; set; }
        //public static IFnExpenseInvoiceItemDao FnExpenseInvoiceItemDao { get; set; }
        //public static IFnExpensePerdiemDao FnExpensePerdiemDao { get; set; }
        //public static IFnExpensePerdiemItemDao FnExpensePerdiemItemDao { get; set; }
    }
}

