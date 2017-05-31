using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SCG.DB.DAL;
using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.Query;
using SCG.DB.DTO.ValueObject;

using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.Standard.Utilities;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class Country : BasePage
    {
       
        public IDbCountryService DbCountryService { get; set; }
        public short CTID
        {
            get { return UIHelper.ParseShort(CountryId.Value); }
            set { CountryId.Value = value.ToString(); }
        }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            ProgramCode = "Country";
        }
        private void RefreshGridData(object sender, EventArgs e)
        {
            CountryEditor.HidePopUp();
            ctlCountryGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CountryEditor.Notify_Ok += new EventHandler(RefreshGridData);
            CountryEditor.Notify_Cancle += new EventHandler(RefreshGridData);

            if (!Page.IsPostBack)
            {

            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
      
        protected void ctlCountryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                CTID = Convert.ToInt16(ctlCountryGrid.DataKeys[rowIndex].Value);
                CountryEditor.Initialize(FlagEnum.EditFlag, CTID);
                CountryEditor.ShowPopUp();
            }
            if (e.CommandName == "UserDelete")
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    CTID = UIHelper.ParseShort(ctlCountryGrid.DataKeys[rowIndex].Value.ToString());
                    DbCountry ct = ScgDbQueryProvider.DbCountryQuery.FindByIdentity(CTID);
                    DbCountryService.Delete(ct);

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlCountryGrid.DataCountAndBind();
                    }
                }

                ctlCountryGrid.DataCountAndBind();
                ctlUpdatePanelGridview.Update();

            }
        }

        protected void ctlCountryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlCountryGrid_DataBound(object sender, EventArgs e)
        {
         
        }
        protected void ctlCountryGrid_PageIndexChanged(object sender, EventArgs e)
        {
        }
        public DbCountry GetCriteria()
        {
            DbCountry ct = new DbCountry();
            ct.CountryCode = ctlCountryCodeCri.Text;
            ct.Comment = ctlCommentCri.Text;
            return ct;
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbCountryQuery.GetCountryList(GetCriteria(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            return ScgDbQueryProvider.DbCountryQuery.CountByCountryCriteria(GetCriteria());
        }
     
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            CountryEditor.Initialize(FlagEnum.NewFlag, 0);
            CountryEditor.ShowPopUp();

        }
        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlCountryGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelectChk")).Checked))
                {
                    try
                    {
                        short countryId = Convert.ToInt16(ctlCountryGrid.DataKeys[row.RowIndex].Value);
                        DbCountry country = DbCountryService.FindByIdentity(countryId);
                        DbCountryService.Delete(country);
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                            ctlCountryGrid.DataCountAndBind();
                            ctlUpdatePanelGridview.Update();
                        }
                    }
                }
            }
            ctlCountryGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlCountryGrid.DataCountAndBind();
        }
        public void ClosePopUp()
        {
            ctlUpdatePanelGridview.Update();
        }
      }
}
