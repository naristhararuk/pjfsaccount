using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.Query
{
    public class ScgeAccountingQueryProvider
    {
        public static IAvAdvanceDocumentQuery AvAdvanceDocumentQuery { get; set; }
        public static IAvAdvanceItemQuery AvAdvanceItemQuery { get; set; }
        public static IDocumentInitiatorQuery DocumentInitiatorQuery { get; set; }
        public static IDocumentAttachmentQuery DocumentAttachmentQuery { get; set; }
        public static ISCGDocumentQuery SCGDocumentQuery { get; set; }
        public static ITADocumentQuery TADocumentQuery { get; set; }
        public static ITADocumentAdvanceQuery TADocumentAdvanceQuery { get; set; }
        public static ITADocumentScheduleQuery TADocumentScheduleQuery { get; set; }
        public static ITADocumentTravellerQuery TADocumentTravellerQuery { get; set; }
        public static IFnRemittanceItemQuery FnRemittanceItemQuery { get; set; }
        public static IFnRemittanceQuery FnRemittanceQuery { get; set; }
        public static IFnRemittanceAdvanceQuery FnRemittanceAdvanceQuery { get; set; }
        public static IFnExpenseDocumentQuery FnExpenseDocumentQuery { get; set; }
        public static IFnExpenseInvoiceQuery FnExpenseInvoiceQuery { get; set; }
        public static IFnExpenseInvoiceItemQuery FnExpenseInvoiceItemQuery { get; set; }
        public static IFnExpensePerdiemQuery FnExpensePerdiemQuery { get; set; }
        public static IFnExpensePerdiemItemQuery FnExpensePerdiemItemQuery { get; set; }
        public static IFnExpenseMileageQuery FnExpenseMileageQuery { get; set; }
        public static IFnExpenseMileageItemQuery FnExpenseMileageItemQuery { get; set; }
        public static IFnExpenseAdvanceQuery FnExpenseAdvanceQuery { get; set; }
        public static IDbDocumentImagePostingQuery DbDocumentImagePostingQuery { get; set; }
        public static IFnPerdiemRateQuery FnPerdiemRateQuery { get; set; }
        public static IEmailLogQuery EmailLogQuery { get; set; }
        public static IFnAutoPaymentQuery FnAutoPaymentQuery { get; set; }
        public static IFnExpenseRemittanceQuery FnExpenseRemittanceQuery { get; set; }
        public static IFnExpenseMileageInvoiceQuery FnExpenseMileageInvoiceQuery { get; set; }
        public static IFnExpensePerdiemDetailQuery FnExpensePerdiemDetailQuery { get; set; }
        public static IFnEACAdvanceImportLogQuery FnEACAdvanceImportLogQuery { get; set; }
        public static IFnPerdiemProfileQuery FnPerdiemProfileQuery { get; set; }
        public static IFnPerdiemProfileCountryQuery FnPerdiemProfileCountryQuery { get; set; }
        public static IDbMonitoringInboxQuery DbMonitoringInboxQuery { get; set; }
        public static IDbMonitoringDocumentQuery DbMonitoringDocumentQuery { get; set; }
        public static IMPADocumentQuery MPADocumentQuery { get; set; }
        public static ICADocumentQuery CADocumentQuery { get; set; }
        public static IMPAItemQuery MPAItemQuery { get; set; }
        public static IFnExpensMPAQuery FnExpensMPAQuery { get; set; }
        public static IFixedAdvanceDocumentQuery FixedAdvanceDocumentQuery { get; set; }
    }
}

