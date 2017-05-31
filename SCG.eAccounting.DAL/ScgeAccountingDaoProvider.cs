using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.WorkFlow.DAL;

namespace SCG.eAccounting.DAL
{
	public class ScgeAccountingDaoProvider
	{
		public static IAvAdvanceDocumentDao AvAdvanceDocumentDao { get; set; }
		public static IAvAdvanceItemDao AvAdvanceItemDao { get; set; }
		public static IDocumentInitiatorDao DocumentInitiatorDao { get; set; }
        public static IDocumentAttachmentDao DocumentAttachmentDao { get; set; }
        public static ITADocumentAdvanceDao TADocumentAdvanceDao { get; set; }
        public static ITADocumentTravellerDao TADocumentTravellerDao { get; set; }
        public static ITADocumentScheduleDao TADocumentScheduleDao { get; set; }
        public static ITADocumentDao TADocumentDao { get; set; }
        public static ISCGDocumentDao SCGDocumentDao { get; set; }
		public static IFnRemittanceDao FnRemittanceDao { get; set; }
        public static IFnRemittanceItemDao FnRemittanceItemDao { get; set; }
        public static IFnRemittanceAdvanceDao FnRemittanceAdvanceDao { get; set; }
        public static IFnExpenseDocumentDao FnExpenseDocumentDao { get; set; }
        public static IFnExpenseInvoiceDao FnExpenseInvoiceDao { get; set; }
        public static IFnExpenseInvoiceItemDao FnExpenseInvoiceItemDao { get; set; }
        public static IFnExpensePerdiemDao FnExpensePerdiemDao { get; set; }
        public static IFnExpensePerdiemItemDao FnExpensePerdiemItemDao { get; set; }
        public static IFnExpenseMileageDao FnExpenseMileageDao { get; set; }
        public static IFnExpenseMileageItemDao FnExpenseMileageItemDao { get; set; }
        public static IDbDocumentImagePostingDao DbDocumentImagePostingDao { get; set; }
        public static IDbDocumentBoxIDPostingDao DbDocumentBoxIDPostingDao { get; set; }
        public static IFnExpenseAdvanceDao FnExpenseAdvanceDao { get; set; }
        public static IFnExpenseRemittanceDao FnExpenseRemittanceDao { get; set; }
        public static IFnExpensePerdiemDetailDao FnExpensePerdiemDetailDao { get; set; }
        public static IFnExpenseMileageInvoiceDao FnExpenseMileageInvoiceDao { get; set; }
        public static ISuEmailLogDao SuEmailLogDao { get; set; }
        public static IFnAutoPaymentDao FnAutoPaymentDao { get; set; }
        public static IFnAutoPaymentTempDao FnAutoPaymentTempDao { get; set; }
        public static IFnEHRexpenseTempDao FnEHRexpenseTempDao { get; set; }
        public static IDocumentViewLockDao DocumentViewLockDao { get; set; }
        public static IFnEACAdvanceImportTempDao FnEACAdvanceImportTempDao { get; set; }
        public static IFnEACAdvanceImportLogDao FnEACAdvanceImportLogDao { get; set; }
        public static IFnPerdiemProfileDao FnPerdiemProfileDao { get; set; }
        public static IFnPerdiemRateDao FnPerdiemRateDao { get; set; }
        public static IFnPerdiemProfileCountryDao FnPerdiemProfileCountryDao { get; set; }
        public static IMPADocumentDao MPADocumentDao { get; set; }
        public static ICADocumentDao CADocumentDao { get; set; }
        public static IMPAItemDao MPAItemDao { get; set; }
		public static IExpensesMPADao ExpensesMPADao { get; set; }
        public static IExpensesCADao ExpensesCADao { get; set; }
        public static IFixedAdvanceDocumentDao FixedAdvanceDocumentDao { get; set; }
	}
}
