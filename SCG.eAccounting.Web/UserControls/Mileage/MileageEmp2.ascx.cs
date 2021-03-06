﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class MileageEmp2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlMileageGrid.DataSource = GetList();
            ctlMileageGrid.DataBind();
        }

        public struct MileageItem
        {
            public string Date { get; set; }
            public string LocationFrom { get; set; }
            public string LocationTo { get; set; }
            public string CarMeterStart { get; set; }
            public string CarMeterEnd { get; set; }
            public string Total { get; set; }
            public string Distance { get; set; }
            public string Exceeding { get; set; }
            public string Total1 { get; set; }
            public string Total2 { get; set; }
            public string Total3 { get; set; }
        }
        public IList<MileageItem> GetList()
        {
            IList<MileageItem> list = new List<MileageItem>();
            MileageItem m = new MileageItem();
            m.Date = "01/01/2009";
            m.LocationFrom = "โคราช";
            m.LocationTo = "อุบล-ร้านศิริมหาชัย";
            m.CarMeterStart = "42,655";
            m.CarMeterEnd = "43,058";
            m.Total = "110";
            m.Distance = "100";
            m.Exceeding = "10";

            list.Add(m);

            m = new MileageItem();
            m.Date = "02/01/2009";
            m.LocationFrom = "อุบล";
            m.LocationTo = "สปป.ลาว";
            m.CarMeterStart = "43,058";
            m.CarMeterEnd = "43,068";
            m.Total = "90";
            m.Distance = "90";
            m.Exceeding = "0";
            m.Total1 = "100";
            m.Total2 = "20";
            m.Total3 = "80";

            list.Add(m);

            return list;
        }
    }
}