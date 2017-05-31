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
using SS.SU.DTO.ValueObject;
using SS.SU.DTO;
using SS.SU.Query;
using System.Web.Script.Serialization;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class UserProfileLookUp : BasePage
    {
        #region Property
        public string UserIdNotIn
        {
            get
            {
                if (ViewState["UserIDNotIn"] != null)
                    return ViewState["UserIDNotIn"].ToString();
                return string.Empty;
            }
            set { ViewState["UserIDNotIn"] = value; }
        }
        public string UserId
        {
            get { return ctlUserId.Text; }
            set { this.ctlUserId.Text = value; }
        }
        public bool isMultiple
        {
            get
            {
                if (ViewState["isMultiple"] != null)
                    return (bool)ViewState["isMultiple"];
                return false;
            }
            set { ViewState["isMultiple"] = value; }
        }
        public bool IsApprover
        {
            get
            {
                if (ViewState["IsApprover"] != null)
                    return (bool)ViewState["IsApprover"];
                return false;
            }
            set { ViewState["IsApprover"] = value; }
        }
        public long? RequesterID
        {
            get
            {
                if (ViewState["RequesterID"] != null)
                    return UIHelper.ParseLong(ViewState["RequesterID"].ToString());
                return null;
            }
            set { ViewState["RequesterID"] = value; }
        }
        public bool SearchFavoriteApprover
        {
            set { ctlSearchFavoriteApprover.Checked = value; }
        }

        public bool SearchFavoriteInitiator
        {
            set { ctlSearchFavoriteInitiator.Checked = value; }
        }
        public bool SearchApprovalFlag
        {
            set { ctlApprovalFlag.Checked = value; }
        }
        
        #endregion Property

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                isMultiple = bool.Parse(Request["isMultiple"].ToString());
                UserIdNotIn = Request["UserIdNotIn"].ToString();
                SearchFavoriteApprover = bool.Parse(Request["SearchFavoriteApprover"].ToString());
                SearchApprovalFlag = bool.Parse(Request["SearchApprovalFlag"].ToString());
                if(Request["RequesterID"].ToString().Length > 0)
                    RequesterID =  long.Parse(Request["RequesterID"].ToString());

                this.Show();
            }
            //this.Show();
        }

        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlUserSearchResultGrid.ClientID + "', '" + ctlUserSearchResultGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlUserSearchResultGrid.DataCountAndBind();
        }
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            if (isMultiple)
            {
                string listID = string.Empty;
                foreach (GridViewRow row in ctlUserSearchResultGrid.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                    {
                        long id = UIHelper.ParseLong(ctlUserSearchResultGrid.DataKeys[row.RowIndex].Value.ToString());
                        listID += id.ToString() + "|";
                        //listID.Remove(listID.Length - 1, 1);  // ลบ | ตัวสุดท้ายออก
                    }
                }
                if (listID.Length > 0)
                    listID.Remove(listID.Length - 1, 1);

                CallOnObjectLookUpReturn(listID);  // เรียก function CallOnObjectLookUpReturn เพื่อส่งค่ากลับไปที่ UserProfileLookup.ascx
                Hide();
            }
        }

        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();  // clear ค่าหน้าจอ
            // ส่งค่ากลับให้ UserProfileLookup.ascx
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        protected void ctlUserSearchResultGrid_DataBound(object sender, EventArgs e)
        {
            if (isMultiple)
            {
                if (ctlUserSearchResultGrid.Rows.Count > 0)
                {
                    ctlSubmitButton.Visible = true;
                    RegisterScriptForGridView();
                }
                else
                {
                    ctlSubmitButton.Visible = false;
                }
            }
        }

        protected void ctlUserSearchResultGrid_RowDataBound(Object s, GridViewRowEventArgs e)
        {
            CheckBox chkActive = e.Row.FindControl("ctlChkActive") as CheckBox;
            if (chkActive != null && !chkActive.Checked)
                e.Row.ForeColor = System.Drawing.Color.Red;
        }

        protected void ctlUserSearchResultGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!isMultiple)
            {
                if (e.CommandName.Equals("SelectUser"))
                {
                    // Retrieve Object Row from GridView Selected Row
                    GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                    long id = UIHelper.ParseLong(ctlUserSearchResultGrid.DataKeys[selectedRow.RowIndex]["Userid"].ToString());
                    CallOnObjectLookUpReturn(id.ToString());  // เรียก function CallOnObjectLookUpReturn เพื่อส่งค่ากลับไปที่ UserProfileLookup.ascx
                    // clear ค่าหน้าจอ
                    Hide();

                }
            }
        }
        public UserCriteria GetCriteria()
        {
            UserCriteria criteria = new UserCriteria();
            criteria.UserName = ctlUserId.Text;
            criteria.EmployeeName = ctlName.Text;
            criteria.IsFavoriteApprover = ctlSearchFavoriteApprover.Checked;
            criteria.IsFavoriteInitiator = ctlSearchFavoriteInitiator.Checked;
            criteria.IsApprovalFlag = ctlApprovalFlag.Checked;
            criteria.RequesterID = RequesterID;
            criteria.UserIdNOTIN = UserIdNotIn;
            criteria.EmployeeCode = ctlEmployeeCode.Text;

            return criteria;
        }
        public void Show()
        {
            if (isMultiple)
            {
                ctlUserSearchResultGrid.Columns[0].Visible = true;
                ctlUserSearchResultGrid.Columns[1].Visible = false;
            }
            else
            {
                ctlUserSearchResultGrid.Columns[0].Visible = false;
                ctlUserSearchResultGrid.Columns[1].Visible = true;
            }
            ctlUserSearchResultGrid.DataCountAndBind();

            this.UpdatePanelSearchUser.Update();
            this.UpdatePanelGridView.Update();

        }
        public void Hide()
        {
            ctlUserId.Text = string.Empty;
            ctlName.Text = string.Empty;
            ctlSearchFavoriteApprover.Checked = false;
            ctlSearchFavoriteInitiator.Checked = false;
            ctlApprovalFlag.Checked = false;
            RequesterID = null;
            UserIdNotIn = string.Empty;
            UpdatePanelSearchUser.Update();
        }

        protected int RequestCount()
        {
            return QueryProvider.SuUserQuery.CountByUserCriteria(GetCriteria());
        }

        protected Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return QueryProvider.SuUserQuery.GetUserProfileList(GetCriteria(), startRow, pageSize, sortExpression);
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);
        }
    }
}
