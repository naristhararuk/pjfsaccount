using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.Helper;
using SS.Standard.UI;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class CostCenterLookupPrototype : BaseUserControl
    {

        #region Property
        //public string Mode
        //{
        //    get
        //    {
        //        if (ViewState["mode"] != null)
        //            return ViewState["mode"].ToString();
        //        else
        //            return "Single";
        //    }
        //    set { this.ViewState["mode"] = value; }
        //}

        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    if (Mode.Equals("Multiple"))
            //    {
            //        ctlSearchResultGrid.Columns[0].Visible = true;
            //    }
            //    else
            //    {
            //        ctlSearchResultGrid.Columns[0].Visible = false;
            //    }
            //}
        }
        #endregion

        #region Public Method

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlSearchResultGrid.ClientID + "', '" + ctlSearchResultGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            IList<CostCenter> list = GetCostCenter();
            return list;
        }
        public int RequestCount()
        {
            return 0;
            //return QueryProvider.SuUserQuery.GetCountUserSearchResultByCriteria(GetCriteria(), LanguageId);
        }

        public void Show()
        {
            CallOnObjectLookUpCalling();
            ctlSearchResultGrid.DataCountAndBind();
            this.UpdatePanelSearch.Update();
            this.UpdatePanelGridView.Update();


            this.ModalPopupExtender1.Show();
        }
        public void Hide()
        {
            ctlCode.Text = string.Empty;
            ctlName.Text = string.Empty;
            this.ModalPopupExtender1.Hide();
        }
        #endregion

        #region LinkButton Event
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            //IList<object> list = new List<object>();
            //foreach (GridViewRow row in ctlSearchResultGrid.Rows)
            //{
            //    if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
            //    {
            //        short Id = UIHelper.ParseShort(ctlSearchResultGrid.DataKeys[row.RowIndex].Value.ToString());
            //        //SuUserSearchResult selectedUser = QueryProvider.SuUserQuery.FindUserByUserIdLanguageId(userId, divId, orgId, LanguageId);
            //        //list.Add(selectedUser);
            //    }
            //}
            //CallOnObjectLookUpReturn(list);
            //Hide();
        }
        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            //UpdatePanelGridView.Update();
            ctlSearchResultGrid.DataCountAndBind();
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
        #endregion

        #region GridView Event
        protected void ctlSearchResultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectCostCenter"))
            {
                // Retrieve Object Row from GridView Selected Row
                GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                short Id = UIHelper.ParseShort(ctlSearchResultGrid.DataKeys[selectedRow.RowIndex].Value.ToString());
                
                //SuUserSearchResult selectedUser = QueryProvider.SuUserQuery.FindUserByUserIdLanguageId(userId, divId, orgId, LanguageId);
                // Return Selected Program.
                //CallOnObjectLookUpReturn(selectedUser);
                // Hide Modal Popup
                Hide();
            }
        }
        protected void ctlSearchResultGrid_DataBound(object sender, EventArgs e)
        {

        }
        #endregion

        public IList<CostCenter> GetCostCenter()
        {
            IList<CostCenter> list = new List<CostCenter>();
            CostCenter costCenter = new CostCenter();

            costCenter.Id = 1;
            costCenter.Code = "0110-94300";
            costCenter.Name = "cost center 1";
            list.Add(costCenter);

            costCenter.Id = 2;
            costCenter.Code = "0110-94301";
            costCenter.Name = "cost center 2";
            list.Add(costCenter);

            costCenter.Id = 3;
            costCenter.Code = "0111-76000";
            costCenter.Name = "cost center 3";
            list.Add(costCenter);

            costCenter.Id = 4;
            costCenter.Code = "0110-84000";
            costCenter.Name = "cost center 4";
            list.Add(costCenter);

            return list;
        }

        public struct CostCenter
        {
            public short Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}