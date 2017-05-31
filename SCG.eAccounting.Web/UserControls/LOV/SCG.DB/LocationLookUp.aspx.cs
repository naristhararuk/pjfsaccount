using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls.DocumentEditor;
using SCG.DB.DTO.ValueObject;
using System.Web.Script.Serialization;
using SCG.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class LocationLookUp : BasePage
    {
        #region public bool isMultiple
        public bool isMultiple
        {
            get
            {
                if (ViewState["isMultiple"] == null)
                    return false;
                else
                    return bool.Parse(ViewState["isMultiple"].ToString());
            }
            set
            {
                ViewState["isMultiple"] = value;
            }
        }
        #endregion public bool isMultiple

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                this.Show();
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbLocationQuery.GetLocationList(startRow, pageSize, sortExpression, ((ctlCompany.SelectedIndex == 0) ? "" : ctlCompany.SelectedValue), ctlLocationCode.Text, ctlLocationName.Text, UserAccount.CurrentLanguageID);
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbLocationQuery.CountByLocationCriteria(((ctlCompany.SelectedIndex == 0) ? "" : ctlCompany.SelectedValue), ctlLocationCode.Text, ctlLocationName.Text, UserAccount.CurrentLanguageID);
            return count;
        }
        #endregion public int RequestCount()

        #region public void SetCombo()
        private void SetCombo()
        {
            ctlCompany.DataSource = ScgDbQueryProvider.DbLocationQuery.FindCompanyCriteria();
            ctlCompany.DataTextField = "Symbol";
            ctlCompany.DataValueField = "ID";
            ctlCompany.DataBind();

            ctlCompany.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), ""));
        }
        #endregion public void SetCombo()

        #region protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlLocationGridView.DataCountAndBind();
            UpdatePanelGridView.Update();
        }
        #endregion protected void ctlSearch_Click(object sender, ImageClickEventArgs e)

        #region  protected void ctlLocationGrid_DataBound(object sender, EventArgs e)
        protected void ctlLocationGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple && ctlLocationGridView.Rows.Count > 0)
                RegisterScriptForGridView();
        }
        #endregion  protected void ctlLocationGrid_DataBound(object sender, EventArgs e)

        #region protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
            Hide();
        }
        #endregion protected void ctlCancel_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlLocationGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlLocationGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long locationId = UIHelper.ParseLong(ctlLocationGridView.DataKeys[rowIndex].Value.ToString());

                //IList<Location> location = ScgDbQueryProvider.DbLocationQuery.FindByLocationIdentity(locationId);
                //if (location.Count > 0)
                //    NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, location[0]));
                CallOnObjectLookUpReturn(locationId.ToString());
                Hide();
            }
        }
        #endregion protected void ctlLocationGrid_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        {
            if (isMultiple)
            {
                //IList<Location> locationList = new List<Location>();
                string locationList = string.Empty;

                foreach (GridViewRow row in ctlLocationGridView.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                    {
                        //long locationId = UIHelper.ParseShort(ctlLocationGridView.DataKeys[row.RowIndex].Value.ToString());
                        //Label ctlLocationCodeInGrid = ctlLocationGridView.Rows[row.RowIndex].FindControl("ctlLocationCode") as Label;
                        //Label ctlLocationNameInGrid = ctlLocationGridView.Rows[row.RowIndex].FindControl("ctlLocationName") as Label;
                        //Label ctlCompanyNameInGrid = ctlLocationGridView.Rows[row.RowIndex].FindControl("ctlCompanyName") as Label;

                        //Location location = new Location();

                        //location.LocationID = locationId;
                        //location.LocationCode = ctlLocationCodeInGrid.Text;
                        //location.LocationName = ctlLocationNameInGrid.Text;
                        //location.CompanyName = ctlCompanyNameInGrid.Text;

                        //locationList.Add(location);

                        long locationId = UIHelper.ParseLong(ctlLocationGridView.DataKeys[row.RowIndex].Value.ToString());
                        locationList += locationId.ToString() + "|";
                        locationList.Remove(locationList.Length - 1, 1);
                    }
                }

                CallOnObjectLookUpReturn(locationList);
                Hide();
            }
        }
        #endregion protected void ctlSelect_Click(object sender, ImageClickEventArgs e)

        #region private void CallOnObjectLookUpReturn(string id)
        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }
        #endregion private void CallOnObjectLookUpReturn(string id)

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlLocationGridView.ClientID + "', '" + ctlLocationGridView.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public void Show()
        public void Show()
        {
            this.SetCombo();

            if (!isMultiple)
            {
                ctlLocationGridView.Columns[0].Visible = false;
                ctlLocationGridView.Columns[1].Visible = true;
                ctlSelect.Visible = false;
                ctlLine.Visible = false;
            }
            else
            {
                ctlLocationGridView.Columns[0].Visible = true;
                ctlLocationGridView.Columns[1].Visible = false;
                ctlSelect.Visible = true;
                ctlLine.Visible = true;
            }

            ctlLocationGridView.DataCountAndBind();
            this.UpdatePanelSearchAccount.Update();
            this.UpdatePanelGridView.Update();
        }
        #endregion public void Show()

        #region public void Hide()
        public void Hide()
        {
            ctlCompany.DataSource = null;
            ctlLocationCode.Text = string.Empty;
            ctlLocationName.Text = string.Empty;
            UpdatePanelSearchAccount.Update();
        }
        #endregion public void Hide()
    }
}
