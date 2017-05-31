using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class ExpenseFormInvoice : System.Web.UI.UserControl
    {
        public bool ShowButton { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            BindRepeater();
        }

        protected void ctlRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void ctlRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                InvoiceData invoice = (InvoiceData)e.Item.DataItem;
                LinkButton edit = (LinkButton)e.Item.FindControl("ctlEdit");
                LinkButton delete = (LinkButton)e.Item.FindControl("ctlDelete");
                BaseGridView gridview = (BaseGridView)e.Item.FindControl("ctlInvoiceItem");
                if (string.IsNullOrEmpty(invoice.InvoiceNo))
                {
                    edit.Visible = false;
                    delete.Visible = false;
                    ShowButton = true;
                    //gridview.Columns[6].Visible = true;
                }
                else
                {
                    ShowButton = false;
                    //gridview.Columns[6].Visible = false;
                }
                gridview.DataSource = invoice.Item;
                gridview.DataBind();
            }
        }

        public void BindRepeater()
        {
            ctlRepeater.DataSource = GetInvoiceData();
            ctlRepeater.DataBind();
        }

        public IList<InvoiceData> GetInvoiceData()
        {
            IList<InvoiceData> list = new List<InvoiceData>();

            //Seq 1
            //InvoiceData invoice = new InvoiceData();
            //invoice.Seq = "1";
            //invoice.InvoiceNo = "15435";
            //invoice.InvoiceDate = "22/12/2008";
            //invoice.Vendor = "0000999999-บริษัท X จำกัด";
            //invoice.BaseAmount = "2,336.45";
            //invoice.VATAmount = "163.55";
            //invoice.WHTAmount = "0.00";
            //invoice.NetAmount = "2,500.00";

            //IList<InvoiceItem> itemList = new List<InvoiceItem>();
            //InvoiceItem item = new InvoiceItem();
            //item.CostCenter = "0110-94300";
            //item.AccountCode = "0000600000-ค่าสัมนาในประเทศ";
            //item.InternalOrder = "640310";
            //item.Description = "ค่าสัมนา \" ปีแห่งการลงทุน \"";
            //item.Amount = "934.58";
            //itemList.Add(item);

            //item = new InvoiceItem();
            //item.CostCenter = "0110-94300";
            //item.AccountCode = "0000600000-ค่าสัมนาในประเทศ";
            //item.InternalOrder = "640310";
            //item.Description = "ค่าสัมนา \" วิชาชีพบุคคล \"";
            //item.Amount = "1,401.87";
            //itemList.Add(item);

            //invoice.Item = itemList;

            //list.Add(invoice);

            ////Seq 2
            //invoice = new InvoiceData();
            //invoice.Seq = "2";
            //invoice.InvoiceNo = "26440";
            //invoice.InvoiceDate = "22/12/2008";
            //invoice.Vendor = "0000888888-บริษัท Y จำกัด";
            //invoice.BaseAmount = "3,500.00";
            //invoice.VATAmount = "0.00";
            //invoice.WHTAmount = "35.00";
            //invoice.NetAmount = "3,465.00";

            //itemList = new List<InvoiceItem>();
            //item = new InvoiceItem();
            //item.CostCenter = "0110-94300";
            //item.AccountCode = "0000700000-ค่าเช่ารถ";
            //item.InternalOrder = "640310";
            //item.Description = "ค่าเช่ารถตู้ 1 คัน";
            //item.Amount = "3,500.00";
            //itemList.Add(item);

            //invoice.Item = itemList;

            //list.Add(invoice);

            ////Seq 3
            //invoice = new InvoiceData();
            //invoice.Seq = "3";
            //invoice.NetAmount = "210.00";

            //itemList = new List<InvoiceItem>();
            //item = new InvoiceItem();
            //item.CostCenter = "0110-94300";
            //item.AccountCode = "0000200000-ค่าทางด่วน";
            //item.InternalOrder = "640310";
            //item.Description = "เบิกค่าทางด่วน";
            //item.Amount = "90.00";
            //itemList.Add(item);

            //item = new InvoiceItem();
            //item.CostCenter = "0110-94300";
            //item.AccountCode = "0000300000-ค่าแท็กซี่";
            //item.InternalOrder = "640310";
            //item.Description = "เบิกค่าแท็กซี่";
            //item.Amount = "120.00";
            //itemList.Add(item);

            //invoice.Item = itemList;

            //list.Add(invoice);
            return list;
        }

        public struct InvoiceData
        {
           public string Seq { get; set; }
           public string InvoiceNo { get; set; }
           public string InvoiceDate { get; set; }
           public string Vendor { get; set; }
           public string BaseAmount { get; set; }
           public string VATAmount { get; set; }
           public string WHTAmount { get; set; }
           public string NetAmount { get; set; }
           public IList<InvoiceItem> Item { get; set; }
        }

        public struct InvoiceItem
        {
            public string CostCenter { get; set; }
            public string AccountCode { get; set; }
            public string InternalOrder { get; set; }
            public string Description { get; set; }
            public string Amount { get; set; }
            public string RefNo { get; set; }
        }
    }
}