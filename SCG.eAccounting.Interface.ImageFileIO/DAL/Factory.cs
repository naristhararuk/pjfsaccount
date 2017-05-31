using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.DB.Query;
using SCG.eAccounting.Interface.Utilities;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Query;
using SS.SU.BLL;
using SCG.DB.Query;

namespace SCG.eAccounting.Interface.ImageFileIO.DAL
{
    public class Factory
    {
        public Factory()
        {

        }

        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static IDbDocumentImagePostingService DbDocumentImagePostingService { get; set; }
        public static IAvAdvanceDocumentService AvAdvanceDocumentService { get; set; }
        public static IFnExpenseDocumentService FnExpenseDocumentService { get; set; }
        public static IAvAdvanceDocumentQuery AvAdvanceDocumentQuery { get; set; }
        public static IFnExpenseDocumentQuery FnExpenseDocumentQuery { get; set; }
        public static IFnRemittanceQuery FnRemittanceQuery { get; set; }
        public static ISCGDocumentQuery SCGDocumentQuery { get; set; }
        public static ISCGDocumentService SCGDocumentService { get; set; }
        public static ISuImageToSAPLogService SuImageToSAPLogService { get; set; }
        public static IDbDocumentImagePostingQuery DbDocumentImagePostingQuery { get; set; }
        public static IDbSapInstanceQuery DbSapInstanceQuery { get; set; }

        public static void CreateObject()
        {
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            DbDocumentImagePostingService = (IDbDocumentImagePostingService)ObjectManager.GetObject("DbDocumentImagePostingService");
            AvAdvanceDocumentService = (IAvAdvanceDocumentService)ObjectManager.GetObject("AvAdvanceDocumentService");
            FnExpenseDocumentService = (IFnExpenseDocumentService)ObjectManager.GetObject("FnExpenseDocumentService");
            AvAdvanceDocumentQuery = (IAvAdvanceDocumentQuery)ObjectManager.GetObject("AvAdvanceDocumentQuery");
            FnExpenseDocumentQuery = (IFnExpenseDocumentQuery)ObjectManager.GetObject("FnExpenseDocumentQuery");
            SCGDocumentQuery = (ISCGDocumentQuery)ObjectManager.GetObject("SCGDocumentQuery");
            //SuImageToSAPLogService = (ISuImageToSAPLogService)ObjectManager.GetObject("SuImageToSAPLogService");
            DbDocumentImagePostingQuery = (IDbDocumentImagePostingQuery)ObjectManager.GetObject("DbDocumentImagePostingQuery");
            FnRemittanceQuery = (IFnRemittanceQuery)ObjectManager.GetObject("FnRemittanceQuery");
            SuImageToSAPLogService = (ISuImageToSAPLogService)ObjectManager.GetObject("SuImageToSAPLogService");
            SCGDocumentService = (ISCGDocumentService)ObjectManager.GetObject("SCGDocumentService");
            DbSapInstanceQuery = (IDbSapInstanceQuery)ObjectManager.GetObject("DbSapInstanceQuery");
        }
    }
}
