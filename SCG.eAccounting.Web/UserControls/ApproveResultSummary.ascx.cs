using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.Standard.WorkFlow.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ApproveResultSummary : BaseUserControl
    {
        public List<ApproveVerifyStatus> DataList
        {
            get
            {
                if (ViewState["ApproveStatusList"] != null)
                    return (List<ApproveVerifyStatus>)ViewState["ApproveStatusList"];
                return null;
            }
            set { ViewState["ApproveStatusList"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;
        public void Initialize()
        {

        }

        protected void ctlClose_Click(object sender, EventArgs e)
        {
            if (Notify_Ok != null)
            {
                Notify_Ok(sender, e);
            }
        }

        public void BindGridView()
        {
            ctlApproveSummaryGrid.DataCountAndBind();
            ctlPopUpUpdatePanel.Update();
        }
        protected object ctlApproveSummaryGrid_RequestData(int startRow, int pageSize, string sortExpression)
        {
            if (DataList != null)
            {
                if (sortExpression.Length > 0)
                {
                    string[] sort = sortExpression.Split(' ');

                    if (sort.Length == 2 && sort[1] == "DESC")
                    {
                        DataList = DataList.OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else if (sort.Length == 2 && sort[1] == "ASC")
                    {
                        DataList = DataList.OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                }
                return DataList
                            .Skip<ApproveVerifyStatus>(startRow)
                            .Take<ApproveVerifyStatus>(startRow + pageSize)
                            .ToList();
            }
            return null;
        }
        protected int ctlApproveSummaryGrid_RequestCount()
        {
            if (DataList != null)
                return DataList.Count;
            return 0;
        }
    }
}