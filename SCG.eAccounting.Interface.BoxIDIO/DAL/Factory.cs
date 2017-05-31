using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Query;
using SCG.eAccounting.Interface.Utilities;
using SS.DB.Query;
using SCG.eAccounting.BLL;
using SCG.DB.Query;

namespace SCG.eAccounting.Interface.BoxIDIO.DAL
{
    public class Factory
    {

        public static IDbDocumentBoxIDPostingQuery DbDocumentBoxIDPostingQuery { get; set; }
        public static IDbParameterQuery DbParameterQuery { get; set; }
        public static IDbDocumentBoxIDPostingService DbDocumentBoxIDPostingService { get; set; }
        public static IDbDocumentImagePostingQuery DbDocumentImagePostingQuery { get; set; }
        public static IDbSapInstanceQuery DbSapInstanceQuery { get; set; }

        public static void CreateObject()
        {
            DbDocumentBoxIDPostingQuery = (IDbDocumentBoxIDPostingQuery)ObjectManager.GetObject("DbDocumentBoxIDPostingQuery");
            DbParameterQuery = (IDbParameterQuery)ObjectManager.GetObject("DbParameterQuery");
            DbDocumentBoxIDPostingService = (IDbDocumentBoxIDPostingService)ObjectManager.GetObject("DbDocumentBoxIDPostingService");
            DbDocumentImagePostingQuery = (IDbDocumentImagePostingQuery)ObjectManager.GetObject("DbDocumentImagePostingQuery");
            DbSapInstanceQuery = (IDbSapInstanceQuery)ObjectManager.GetObject("DbSapInstanceQuery");
        }
    }
}
