using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

using SCG.DB.Query;
using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CountrySearch : BaseUserControl
    {
        #region Define Variable
        public IDbCountryLangService DbCountryLangService{get;set;}
        public IDbCountryService DbCountryService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
       
        #endregion

        #region Property
              
        public string LanguageId
        {
            get { return txtLanguageId.Text; }

            set { txtLanguageId.Text = value; }
        }

        //public short RoleId { get; set; }
        //public short LanguageId { get; set; }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            //ctlProgramGrid.DataCountAndBind();
        }
        #endregion        

        #region Public Method
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBoxControl(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlCountryGrid.ClientID + "', '" + ctlCountryGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "validateChkBox", script.ToString(), true);
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbCountryLang dbCountryLang = new DbCountryLang();
            dbCountryLang.CountryName = txtCountryName.Text;
            IList<CountryLang> list = ScgDbQueryProvider.DbCountryLangQuery.FindByDbCountryLang(dbCountryLang, txtCountryCode.Text, UIHelper.ParseShort(LanguageId), startRow, pageSize, sortExpression);
           
            return list;
        }
        public int RequestCount()
        {
            DbCountryLang dbCountryLang = new DbCountryLang();
            dbCountryLang.CountryName = txtCountryName.Text;
            int count = ScgDbQueryProvider.DbCountryLangQuery.CountByDbCountryLangCriteria(dbCountryLang, txtCountryCode.Text, UIHelper.ParseShort(LanguageId));

            return count;
        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            ctlCountryGrid.DataCountAndBind();
            this.UpdatePanelSearchCountry.Update();
            this.UpdatePanelGridView.Update();

            
            this.ModalPopupExtender1.Show();
        }
        public void Hide()
        {
            this.ModalPopupExtender1.Hide();
        }
        #endregion

        #region LinkButton Event
        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            IList<DbCountry> list = new List<DbCountry>();
            foreach (GridViewRow row in ctlCountryGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    short id = UIHelper.ParseShort(ctlCountryGrid.DataKeys[row.RowIndex].Values["CountryID"].ToString());
                    DbCountry country = DbCountryService.FindByIdentity(id);
                    list.Add(country);
                }   
            }
            CallOnObjectLookUpReturn(list);
            this.ModalPopupExtender1.Hide();
        }
        protected void ctlSearch_Click(object sender, EventArgs e)
        {
            UpdatePanelGridView.Update();
            ctlCountryGrid.DataCountAndBind();
        }
        protected void ctlCancel_Click(object sender, EventArgs e)
        {
            txtCountryName.Text = "";
            this.ModalPopupExtender1.Hide();
        }
        #endregion

        #region GridView Event
        protected void ctlCountryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectCountry")
            {
                // Retrieve Object Row from GridView Selected Row
                GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                short countryId = UIHelper.ParseShort(ctlCountryGrid.DataKeys[selectedRow.RowIndex].Value.ToString());
                DbCountryLang selectedCountry = DbCountryLangService.FindByIdentity(countryId);
                // Return Selected Program.
                CallOnObjectLookUpReturn(selectedCountry);
                // Hide Modal Popup.
                this.ModalPopupExtender1.Hide();
            }
        }
        protected void ctlCountryGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlCountryGrid.Rows.Count > 0)
            {
                //divButton.Visible = true;
                RegisterScriptForGridView();
            }
            else
            {
                //divButton.Visible = false;
            }
        }
        #endregion
    }
}