using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.InboxSearchResult
{
    public partial class SearchResult : System.Web.UI.UserControl
    {
        public SearchCriteria Criteria { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindGridView()
        {
            ctlInboxGrid.DataSource = GetInvoiceData();
            ctlInboxGrid.DataBind();
        }

        public IList<SearchResultData> GetInvoiceData()
        {
            IList<SearchResultData> list = new List<SearchResultData>();

            //Seq 1
            SearchResultData searchresult = new SearchResultData();

            searchresult.Seq = "1";
            searchresult.DocumentNo = "CS2552A10000001";
            searchresult.DocumentType = "ใบขออนุมัติเพิ่ม-ลดงบประมาณ";
            searchresult.Creator = "xxxxx xxxxx";
            searchresult.Amount = 3500000.00;
            searchresult.DocumentStatus = "Draft";
            list.Add(searchresult);
            //Seq 2
            searchresult = new SearchResultData();
            searchresult.Seq = "2";
            searchresult.DocumentNo = "CS2552A10000002";
            searchresult.DocumentType = "ใบขออนุมัติเพิ่ม-ลดงบประมาณ";
            searchresult.Creator = "xxxxx xxxxx";
            searchresult.Amount = 2000000.00;
            searchresult.DocumentStatus = "Draft";
            list.Add(searchresult);

            return list;
        }
        public struct SearchResultData
        {
            public string Seq { get; set; }
            public string DocumentNo { get; set; }
            public string DocumentType { get; set; }
            public string Creator { get; set; }
            public double Amount { get; set; }
            public string DocumentStatus { get; set; }
        }
    }
}