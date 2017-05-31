using System;
using System.Collections.Generic;
using System.Linq;
using SS.Standard.UI;
using SCG.eAccounting.BLL;
using System.Web.UI;
using SCG.eAccounting.Web.Helper;
using System.Web.UI.WebControls;
using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO.ValueObject;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using System.Text;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class ForeignPerdiemRateProfile : BasePage
    {
        public IFnPerdiemProfileService FnPerdiemProfileService { get; set; }
        public IFnPerdiemRateService FnPerdiemRateService { get; set; }
        public IFnPerdiemProfileCountryService FnPerdiemProfileCountryService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlForeignPerdiemRateProfileEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlForeignPerdiemRateProfileEditor.Notify_Cancle += new EventHandler(RefreshGridData);

            ctlForeignPerdiemRateProfileDetailEditor.Notify_Ok += new EventHandler(RefreshDetailGridData);
            ctlForeignPerdiemRateProfileDetailEditor.Notify_Cancle += new EventHandler(RefreshDetailGridData);

            ctlForeignPerdiemRateProfileCountryEditor.Notify_Ok += new EventHandler(RefreshCountryGridData);
            ctlForeignPerdiemRateProfileCountryEditor.Notify_Cancle += new EventHandler(RefreshCountryGridData);

            if (!Page.IsPostBack)
            {

                ctlForeignPerdiemRateProfileGrid.DataCountAndBind();
            }
        }
        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlForeignPerdiemRateProfileEditor.HidePopUp();
            ctlForeignPerdiemRateProfileGrid.DataCountAndBind();
            ctlUpdatePanel.Update();

        }
        private void RefreshDetailGridData(object sender, EventArgs e)
        {
            ctlForeignPerdiemRateProfileDetailEditor.HidePopUp();
            ctlForeignPerdiemRateProfileCountryEditor.HidePopUp();
            BindDetailGrid(UIHelper.ParseLong(foreignPerdiemRateProfileCode2.Value));
            ctlUpdatePanelDetailGrid.Update();
        }
        private void RefreshCountryGridData(object sender, EventArgs e)
        {
            ctlForeignPerdiemRateProfileDetailEditor.HidePopUp();
            ctlForeignPerdiemRateProfileCountryEditor.HidePopUp();
            BindCountryGrid(UIHelper.ParseLong(foreignPerdiemRateProfileCode3.Value));
            ctlUpdatePanelCountryGrid.Update();
        }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlForeignPerdiemRateProfileGrid.DataCountAndBind();
            DetailGridClose();
            CountryGridClose();
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        public long ForeignPerdiemRateProfileCode
        {
            get { return UIHelper.ParseLong(foreignPerdiemRateProfileCode.Value); }
            set { foreignPerdiemRateProfileCode.Value = value.ToString(); }
        }
        protected void ctlForeignPerdiemRateProfileGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("ForeignPerdiemRateProfileEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                ForeignPerdiemRateProfileCode = UIHelper.ParseLong(ctlForeignPerdiemRateProfileGrid.DataKeys[rowIndex].Values["PerdiemProfileID"].ToString());
                ctlForeignPerdiemRateProfileEditor.Initialize(FlagEnum.EditFlag, ForeignPerdiemRateProfileCode);
                ctlForeignPerdiemRateProfileEditor.ShowPopUp();

            }
            if (e.CommandName.Equals("ForeignPerdiemRateProfileDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    ForeignPerdiemRateProfileCode = UIHelper.ParseLong(ctlForeignPerdiemRateProfileGrid.DataKeys[rowIndex].Value.ToString());
                    FnPerdiemProfile fp = ScgeAccountingQueryProvider.FnPerdiemProfileQuery.FindByIdentity(ForeignPerdiemRateProfileCode);
                    FnPerdiemProfileService.Delete(fp);
                    DetailGridClose();
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlForeignPerdiemRateProfileGrid.DataCountAndBind();
                    }
                }

                ctlForeignPerdiemRateProfileGrid.DataCountAndBind();
                ctlUpdatePanel.Update();

            }
            if (e.CommandName.Equals("Detail"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long PofileID = UIHelper.ParseLong(ctlForeignPerdiemRateProfileGrid.DataKeys[rowIndex].Value.ToString());
                ctlForeignPerdiemRateProfileGrid.SelectedIndex = rowIndex;
                foreignPerdiemRateProfileCode2.Value = PofileID.ToString();
                BindDetailGrid(PofileID);
                ctlDetailGrid.Visible = true;
                ctlDetailTools.Visible = true;
                CountryGridClose();
                ctlUpdatePanel.Update();
                ctlUpdatePanelDetailGrid.Update();
            }
            if (e.CommandName.Equals("Country"))
            {
                rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                long PofileID = UIHelper.ParseLong(ctlForeignPerdiemRateProfileGrid.DataKeys[rowIndex].Value.ToString());
                ctlForeignPerdiemRateProfileGrid.SelectedIndex = rowIndex;
                foreignPerdiemRateProfileCode3.Value = PofileID.ToString();
                BindCountryGrid(PofileID);
                ctlCountryGrid.Visible = true;
                ctlCountryTools.Visible = true;
                DetailGridClose();
                ctlUpdatePanel.Update();
                ctlUpdatePanelCountryGrid.Update();
            }

        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            ForeignPerdiemRateProfileCriteria criteria = GetCriteria();
            return ScgeAccountingQueryProvider.FnPerdiemProfileQuery.GetForeignPerdiemRateProfileListByCriteria(criteria, startRow, pageSize, sortExpression);


        }
        public int RequestCount()
        {
            ForeignPerdiemRateProfileCriteria criteria = GetCriteria();
            return ScgeAccountingQueryProvider.FnPerdiemProfileQuery.CountForeignPerdiemRateProfile(criteria);


        }
        public ForeignPerdiemRateProfileCriteria GetCriteria()
        {
            ForeignPerdiemRateProfileCriteria mp = new ForeignPerdiemRateProfileCriteria();
            mp.PerdiemProfileName = ctlPerdiemProfileName.Text;
            mp.Description = ctlDescription.Text;
            return mp;
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            ctlForeignPerdiemRateProfileEditor.Initialize(FlagEnum.NewFlag, 0);
            ctlForeignPerdiemRateProfileEditor.ShowPopUp();
        }
        protected void ctlAddDetail_Click(object sender, ImageClickEventArgs e)
        {
            long id = UIHelper.ParseLong(foreignPerdiemRateProfileCode2.Value);
            ctlForeignPerdiemRateProfileDetailEditor.Initialize(FlagEnum.NewFlag, 0, id);
            ctlForeignPerdiemRateProfileDetailEditor.ShowPopUp();
        }
        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            DetailGridClose();
        }
        protected void ctlAddCountry_Click(object sender, ImageClickEventArgs e)
        {
            long id = UIHelper.ParseLong(foreignPerdiemRateProfileCode3.Value);
            ctlForeignPerdiemRateProfileCountryEditor.Initialize(id);
            ctlForeignPerdiemRateProfileCountryEditor.ShowPopUp();
        }
        protected void ctlDeleteCountry_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlCountryGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    try
                    {
                        long Id = UIHelper.ParseLong(ctlCountryGrid.DataKeys[row.RowIndex]["ID"].ToString());
                        FnPerdiemProfileCountry ppc = ScgeAccountingQueryProvider.FnPerdiemProfileCountryQuery.FindByID(Id);
                        FnPerdiemProfileCountryService.Delete(ppc);

                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);

                            ctlUpdatePanelCountryGrid.Update();
                        }
                    }
                }
            }
            BindCountryGrid(UIHelper.ParseLong(foreignPerdiemRateProfileCode3.Value));
            ctlUpdatePanelCountryGrid.Update();
        }
        public void BindDetailGrid(long Id)
        {
            ctlDetailGrid.DataSource = ScgeAccountingQueryProvider.FnPerdiemRateQuery.FindByPerdiemProfileID(Id, UserAccount.CurrentLanguageID);
            ctlDetailGrid.DataBind();
        }
        public void BindCountryGrid(long Id)
        {
            ctlCountryGrid.DataSource = ScgeAccountingQueryProvider.FnPerdiemProfileCountryQuery.FindByPerdiemProfileID(Id, UserAccount.CurrentLanguageID);
            ctlCountryGrid.DataBind();
        }
        protected void ctlDetailGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlDetailGrid_DataBound(object sender, EventArgs e)
        {
            //if (ctlDetailGrid.Rows.Count > 0)
            //{
            //    RegisterScriptForGridView();
            //    divButton.Visible = true;
            //    ctlDelete.OnClientClick =

            //    String.Format("return ConfirmDelete('{0}')", ctlProgramGrid.ClientID);
            //}
            //else
            //{
            //    divButton.Visible = false;
            //}
        }
        protected void ctlDetailGrid_PageIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ctlDetailGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DetailEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                long RateId = UIHelper.ParseLong(ctlDetailGrid.DataKeys[rowIndex].Value.ToString());
                long ProfileId = UIHelper.ParseLong(foreignPerdiemRateProfileCode2.Value);
                ctlForeignPerdiemRateProfileDetailEditor.Initialize(FlagEnum.EditFlag, RateId, ProfileId);
                ctlForeignPerdiemRateProfileDetailEditor.ShowPopUp();
                ctlUpdatePanelDetailGrid.Update();
            }
            if (e.CommandName.Equals("DetailDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long detailId = UIHelper.ParseLong(ctlDetailGrid.DataKeys[rowIndex].Value.ToString());
                    FnPerdiemRate fp = ScgeAccountingQueryProvider.FnPerdiemRateQuery.FindByIdentity(detailId);
                    FnPerdiemRateService.DeleteFnPerdiemRate(fp);
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);

                        BindDetailGrid(UIHelper.ParseLong(foreignPerdiemRateProfileCode2.Value));
                        ctlUpdatePanelDetailGrid.Update();
                    }
                }
                BindDetailGrid(UIHelper.ParseLong(foreignPerdiemRateProfileCode2.Value));
                ctlUpdatePanelDetailGrid.Update();
            }
        }
        protected void ctlCountryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void ctlCountryGrid_DataBound(object sender, EventArgs e)
        {
            if (ctlCountryGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                ctlDeleteCountry.Visible = true;
                ctlDeleteCountry.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlCountryGrid.ClientID);
            }
        }
        protected void ctlCountryGrid_PageIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ctlCountryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
            
        }
        public void DetailGridClose()
        {
            ctlDetailGrid.Visible = false;
            ctlDetailTools.Visible = false;
            ctlUpdatePanel.Update();
            ctlUpdatePanelDetailGrid.Update();
        }
        public void CountryGridClose()
        {
            ctlCountryGrid.Visible = false;
            ctlCountryTools.Visible = false;
            ctlUpdatePanel.Update();
            ctlUpdatePanelCountryGrid.Update();
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlCountryGrid.ClientID + "', '" + ctlCountryGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "validateChkBox", script.ToString(), true);
        }
    }
}