using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.UI;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;


using SS.Standard.Security;




namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class CountryLookUp : BaseUserControl
    {
        #region Defind Variable
        public IDbCountryLangService dbCountryLangService { get; set; }
        public IDbCountryService dbCountryService { get; set; }
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

        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlCountryGrid.DataCountAndBind();
        }
        public void Show()
        {
            CallOnObjectLookUpCalling();
            ctlCountryGrid.DataCountAndBind();
            this.UpdatePanelSearchCountry.Update();
            this.UpdatePanelGridView.Update();


            this.ModalPopupExtender1.Show();
        }

        protected void ctlSubmit_Click(object sender, EventArgs e)
        {
            //IList<DbCountry> list = new List<DbCountry>();
            //foreach (GridViewRow row in ctlCountryGrid.Rows)
            //{
            //    if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
            //    {
            //        short id = UIHelper.ParseShort(ctlCountryGrid.DataKeys[row.RowIndex].Values["CountryID"].ToString());
            //        DbCountry country = dbCountryService.FindByIdentity(id);
            //        list.Add(country);
            //    }
            //}
            //CallOnObjectLookUpReturn(list);
            //this.ModalPopupExtender1.Hide();
        }

        protected void ctlCountryGrid_DataBound(object sender, EventArgs e)
        {

        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            txtCountryName.Text = string.Empty;
            this.ModalPopupExtender1.Hide();
        }

        protected void ctlCountryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Select"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short countryId = UIHelper.ParseShort(ctlCountryGrid.DataKeys[rowIndex].Value.ToString());
                //DbCountry dbCountry = dbCountryService.FindByIdentity(countryId);
                //CountryLang countryLang = (CountryLang)ctlCountryGrid.Rows[rowIndex].DataItem;
                CountryLang countryLang = ScgDbQueryProvider.DbCountryLangQuery.FindByDbCountryLangKey(countryId, UIHelper.ParseShort(LanguageId));
                CallOnObjectLookUpReturn(countryLang);
                this.ModalPopupExtender1.Hide();
            }
        }
    }
}