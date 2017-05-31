using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.FN.Query
{
    public class FnQueryProvider
    {
        public FnQueryProvider() { }
        public static IFnReceiptTypeQuery FnReceiptTypeQuery { get; set; }
		public static IFnPaymentTypeQuery FnPaymentTypeQuery { get; set; }
		public static IFnPaymentTypeLangQuery FnPaymentTypeLangQuery { get; set; }
		public static IFnCashierQuery FnCashierQuery { get; set; }
		public static IFnCashierLangQuery FnCashierLangQuery { get; set; }
    }
}
