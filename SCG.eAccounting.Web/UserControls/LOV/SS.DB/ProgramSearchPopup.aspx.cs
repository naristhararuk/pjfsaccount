using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.SU.DTO.ValueObject;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class ProgramSearchPopup : BasePage
    {
        #region Define Variable
        public ISuProgramLangService SuProgramLangService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
        #endregion

        #region Property
        public string RoleId
        {
            get { return txtRoleId.Text; }

            set { txtRoleId.Text = value; }
        }
        public string LanguageId
        {
            get { return txtLanguageId.Text; }

            set { txtLanguageId.Text = value; }
        }

        //public short RoleId { get; set; }
        //public short LanguageId { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Public Method
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlProgramGrid.ClientID + "', '" + ctlProgramGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SuProgramLang suProgramLang = new SuProgramLang();
            suProgramLang.ProgramsName = txtCompanyName.Text;

            IList<ProgramLang> list = SuProgramLangService.FindBySuProgramLangQuery(suProgramLang, UIHelper.ParseShort(RoleId), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
            return list;
        }
        public int RequestCount()
        {
            SuProgramLang suProgramLang = new SuProgramLang();
            suProgramLang.ProgramsName = txtCompanyName.Text;
            int count = SuProgramLangService.CountBySuProgramLangCriteria(suProgramLang, UIHelper.ParseShort(RoleId), UserAccount.CurrentLanguageID);

            return count;
        }
        public void Show()
        {
            //CallOnObjectLookUpCalling();
            ctlProgramGrid.DataCountAndBind();
            this.UpdatePanelSearchProgram.Update();
            this.UpdatePanelGridView.Update();


            //this.ModalPopupExtender1.Show();
        }
        public void Hide()
        {
            ctlProgramName.Text = string.Empty;
            ctlSearchbox.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
        }
        #endregion

        #region LinkButton Event
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            //IList<ProgramLang> list = new List<ProgramLang>();
            string listID = string.Empty;
            foreach (GridViewRow row in ctlProgramGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
             
                    short id = UIHelper.ParseShort(ctlProgramGrid.DataKeys[row.RowIndex].Values["ProgramLangId"].ToString());
                    listID += id.ToString() + "|";
                }
            }
            if(listID.Length>0)
                listID.Remove(listID.Length - 1, 1);
            CallOnObjectLookUpReturn(listID);
            Hide();
        }

        private void CallOnObjectLookUpReturn(string id)
        {
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            //UpdatePanelGridView.Update();
            ctlProgramGrid.DataCountAndBind();
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            txtCompanyName.Text = "";
            //this.ModalPopupExtender1.Hide();
            Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel')", true);

        }
        #endregion

        #region GridView Event
        protected void ctlProgramGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectProgram")
            {
                // Retrieve Object Row from GridView Selected Row
                GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                short programId = UIHelper.ParseShort(ctlProgramGrid.DataKeys[selectedRow.RowIndex].Value.ToString());
                //SuProgramLang selectedProgram = SuProgramLangService.FindByIdentity(programId);
                // Return Selected Program.
                CallOnObjectLookUpReturn(programId.ToString());
                // Hide Modal Popup.
                Hide();
            }
        }
        protected void ctlProgramGrid_RowDataBound(Object s, GridViewRowEventArgs e)
        {
            CheckBox chkActive = e.Row.FindControl("ctlChkActive") as CheckBox;
            if (chkActive != null && !chkActive.Checked)
                e.Row.ForeColor = System.Drawing.Color.Red;
        }

        protected void ctlProgramGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlProgramGrid.Rows.Count > 0)
            {
                //divButton.Visible = true;
                RegisterScriptForGridView();
            }
            //else
            //{
            //    //divButton.Visible = false;
            //}
        }
        #endregion
    }
}
