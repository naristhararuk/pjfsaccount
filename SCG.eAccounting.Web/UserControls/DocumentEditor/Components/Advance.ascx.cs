using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class Advance : BaseUserControl , IEditorComponent
    {
		#region Property
        public Guid TransactionID
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public IList<SCG.eAccounting.DTO.ValueObject.Advance> AdvanceList
        {
            get { return (IList<SCG.eAccounting.DTO.ValueObject.Advance>)ViewState["AdvanceList"]; }
            set { ViewState["AdvanceList"] = value; }
        }
        
        public bool EnableTALookup
        {
            get
            {
                if (ViewState["EnableTALookup"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)ViewState["EnableTALookup"];

                }
            }
            set { ViewState["EnableTALookup"] = value; }
        }
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public string InitialFlag
        {
            get { return ViewState[ViewStateName.InitialFlag].ToString(); }
            set { ViewState[ViewStateName.InitialFlag] = value; }
        }
		public string Width
		{
			set	{ ctlAdvanceTable.Width = value; }
		}
		public string Mode
		{
			get { return ctlMode.Text; }
			set { ctlMode.Text = value; }
		}
        public int RowCount
        {
            get { return ctlAdvanceGridView.Rows.Count; }
        }
		#endregion

        public IFnRemittanceAdvanceService FnRemittanceAdvanceService { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
            {
                ctlAdvanceGridView.DataBind();

            }
        }

		#region Page_Load Event
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				
			}
		}
		#endregion

		#region Public Method
        public void Initialize(Guid txId, long documentID,string initialFlag)
        {
            this.TransactionID = txId;
            this.DocumentID = documentID;
            this.InitialFlag = initialFlag;
        }
		public void ChangeMode()
		{
			if ((!string.IsNullOrEmpty(this.Mode)) && (this.Mode.Equals(ModeEnum.ReadWrite)))
			{
				 ctlAdvanceGridView.Enabled = true;
				 ctlAdvanceGridView.Columns[5].Visible = true;
				 //ctlAddAdvanceTable.Visible = true;
			}
			else
			{
				ctlAdvanceGridView.Enabled = false;
				ctlAdvanceGridView.Columns[5].Visible = false;
				//ctlAddAdvanceTable.Visible = false;
			}
		}
        public void BindCtlAdvanceGridview(IList<SCG.eAccounting.DTO.ValueObject.Advance> advanceList)
        {
            ctlAdvanceGridView.DataSource = advanceList;
            ctlAdvanceGridView.DataBind();
            
        }
        protected void ctlAdvanceGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ctlNoLabel = e.Row.FindControl("ctlNoLabel") as Label;
                ctlNoLabel.Text = ((ctlAdvanceGridView.PageSize * ctlAdvanceGridView.CustomPageIndex) + (e.Row.RowIndex + 1)).ToString();
            }
        }
        protected void ctlAdvanceGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName.Equals("DeleteAdvance"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                //long remittanceID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["RemittanceID"].ToString());
                long AdvanceID = UIHelper.ParseLong(ctlAdvanceGridView.DataKeys[rowIndex]["AdvanceID"].ToString());
                //FnRemittanceAdvance remittanceAdvance = ScgeAccountingQueryProvider.FnRemittanceAdvanceQuery.FindRemittanceAdvanceByRemittanceIDAndAdvanceID(this.DocumentID, AdvanceID);
                FnRemittanceAdvanceService.DeleteRemittanceAdvanceFromTransaction(this.TransactionID, AdvanceID,this.DocumentID);
                AdvanceList.RemoveAt(rowIndex);
                BindCtlAdvanceGridview(AdvanceList);
            }
        }
        protected void ctlAdvanceGridview_DataBound(object sender, EventArgs e)
        {
            if (ctlAdvanceGridView.Rows.Count == 0)
            {
                EnableTALookup = true;
            }
        }
        public void VisibleColumn(int columnIndex,bool isVisible)
        {
            ctlAdvanceGridView.Columns[columnIndex].Visible = isVisible;
        }
		#endregion
    }
}