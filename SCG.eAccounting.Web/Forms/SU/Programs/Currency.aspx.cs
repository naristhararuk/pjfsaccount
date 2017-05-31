using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.SU.Query;
using SS.SU.DTO;
using System.Text;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SS.SU.BLL;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;
using SS.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class Currency : BasePage
    {
        #region Properties
        public IDbCurrencyService DbCurrencyService { get; set; }
        public IDbExchangeRateService DbExchangeRateService { get; set; }
        #endregion

        #region Event Load
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "Currency";

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            //ctlDelete.Enabled = this.CanDelete;
            //ctlDelete.Visible = this.ctlDelete.Enabled;

            //ctlAddNew.Enabled = this.CanAdd;
            //ctlAddNew.Visible = this.ctlAddNew.Enabled;

            //ctlDelete.Enabled = this.CanDelete;
            //ctlDelete.Visible = this.ctlDelete.Enabled;
        }

        #endregion

        #region Currency Gridview
        protected void ctlCurrencyGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short currencyId = Convert.ToInt16(ctlCurrencyGrid.DataKeys[rowIndex].Value);
                ctlCurrencyForm.PageIndex = (ctlCurrencyGrid.PageIndex * ctlCurrencyGrid.PageSize) + rowIndex;
                ctlCurrencyForm.ChangeMode(FormViewMode.Edit);
                IList<DbCurrency> currencyList = new List<DbCurrency>();
                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindByIdentity(currencyId);
                currencyList.Add(currency);

                ctlCurrencyForm.DataSource = currencyList;
                ctlCurrencyForm.DataBind();
                ctlCurrencyGrid.DataCountAndBind();
                UpdatePanelCurrencyForm.Update();
                ctlCurrencyModalPopupExtender.Show();
            }
            if (e.CommandName == "UserEdit")
            {
                foreach (GridViewRow row in ctlCurrencyGrid.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelectChk")).Checked))
                    {
                        try
                        {

                            short currencyId = UIHelper.ParseShort(ctlCurrencyGrid.DataKeys[row.RowIndex].Value.ToString());
                            DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindProxyByIdentity(currencyId);
                            DbCurrencyService.Delete(currency);
                            ExchangeRateGridViewFinish();
                        }
                        catch (Exception ex)
                        {
                            if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                    "alert('This data is now in use.');", true);
                                ctlCurrencyGrid.DataCountAndBind();
                                ctlCurrencyUpdatePanel.Update();
                            }
                        }
                    }
                }
            }
            if (e.CommandName == "Select")
            {

                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                string currencyId = ctlCurrencyGrid.DataKeys[rowIndex].Value.ToString();//UIHelper.ParseShort(ctlCurrencyGrid.DataKeys[rowIndex].Value.ToString());
                ctlCurrencyIdHidden.Value = currencyId;
                ctlExchangeGrid.DataCountAndBind();
                ctlExchangeButton.Visible = true;
                ctlExchangeGrid.Visible = true;
                ctlExchangeFds.Visible = true;
                //if (ctlExchangeGrid.Rows.Count <= 0)
                //{
                //    ctlExchangeGrid.EmptyDataText = "$No result.$";
                //    ctlExchangeGrid.DataBind();
                //}
                ctlCurrencyGrid.DataCountAndBind();
                ctlExchangeRateUpdatePanel.Update();
                ctlCurrencyUpdatePanel.Update();
            }


        }
        protected void ctlCurrencyGrid_PageIndexChanged(object sender, EventArgs e)
        {
            ExchangeRateGridViewFinish();
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {


            VOUCurrencySetup criteria = GetSuUserCriteria();
            return SsDbQueryProvider.DbCurrencyQuery.GetCurrencyListByCriteria(criteria, startRow, pageSize, sortExpression);

            //return SsDbQueryProvider.DbCurrencyQuery.GetCurrencyList(new DbCurrency(), startRow, pageSize, sortExpression);

        }
        public VOUCurrencySetup GetSuUserCriteria()
        {
            VOUCurrencySetup currency = new VOUCurrencySetup();
            currency.Symbol = ctlSymbol.Text;

            currency.Description = ctrDescription.Text;
            return currency;
        }
        public int RequestCount()
        {
            VOUCurrencySetup criteria = GetSuUserCriteria();
            int count = SsDbQueryProvider.DbCurrencyQuery.CountCurrencyByCriteria(criteria);
            return count;
        }
        #endregion

        #region Exchange Gridview
        protected void ctlExchangeGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UserEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short ExchangeRateId = Convert.ToInt16(ctlExchangeGrid.DataKeys[rowIndex].Value);
                ctlExchangeForm.PageIndex = (ctlExchangeGrid.PageIndex * ctlExchangeGrid.PageSize) + rowIndex;
                ctlExchangeForm.ChangeMode(FormViewMode.Edit);
                IList<DbExchangeRate> exchangeRateList = new List<DbExchangeRate>();
                DbExchangeRate exchangeRate = SsDbQueryProvider.DbExchangeRateQuery.FindByIdentity(ExchangeRateId);
                exchangeRateList.Add(exchangeRate);

                ctlExchangeForm.DataSource = exchangeRateList;
                ctlExchangeForm.DataBind();

                UpdatePanelExchangeForm.Update();
                ctlExchangeModalPopupExtender.Show();
            }

        }
        public Object RequestExchangeData(int startRow, int pageSize, string sortExpression)
        {
            return SsDbQueryProvider.DbExchangeRateQuery.GetExchangeList(UIHelper.ParseShort(ctlCurrencyIdHidden.Value), startRow, pageSize, sortExpression);

        }
        public int RequestExchangeCount()
        {
            int count = SsDbQueryProvider.DbExchangeRateQuery.CountByExchangeCriteria(UIHelper.ParseShort(ctlCurrencyIdHidden.Value));

            return count;
        }
        #endregion

        #region Check box
        protected void ctlCurrencyGrid_Databound(object sender, EventArgs e)
        {
            if (ctlCurrencyGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView(ctlCurrencyGrid.ClientID, ctlCurrencyGrid.HeaderRow.FindControl("ctlSelectAllChk").ClientID);
                divButton.Visible = true;
                //ctlCurrencyDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlCurrencyGrid.ClientID);
            }
            else
            {
                divButton.Visible = false;
            }
        }
        protected void ctlExchangeGrid_Databound(object sender, EventArgs e)
        {
            if (ctlExchangeGrid.Rows.Count > 0)
            {
                RegisterScriptForGridView(ctlExchangeGrid.ClientID, ctlExchangeGrid.HeaderRow.FindControl("ctlSelectAllChk").ClientID);
                divButton.Visible = true;
                ctlExchangeDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlExchangeGrid.ClientID);
            }
        }

        #endregion

        #region CurrencyForm
        protected void ctlCurrencyForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlCurrencyForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlCurrencyForm.ChangeMode(FormViewMode.ReadOnly);
                CloseCurrencyPopUp();
            }
        }
        protected void ctlCurrencyForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlSymbol = (TextBox)ctlCurrencyForm.FindControl("ctlSymbol");
            TextBox ctlComment = (TextBox)ctlCurrencyForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlCurrencyForm.FindControl("ctlActiveChk");

            DbCurrency currency = new DbCurrency();
            currency.Symbol = ctlSymbol.Text;
            currency.Comment = ctlComment.Text;
            currency.Active = ctlActiveChk.Checked;
            currency.CreBy = UserAccount.UserID;
            currency.CreDate = DateTime.Now;
            currency.UpdBy = UserAccount.UserID;
            currency.UpdDate = DateTime.Now;
            currency.UpdPgm = ProgramCode;
            try
            {
                DbCurrencyService.AddCurrency(currency);
                ctlCurrencyGrid.DataCountAndBind();
                ctlCurrencyForm.ChangeMode(FormViewMode.ReadOnly);
                CloseCurrencyPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlCurrencyForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short currencyId = Convert.ToInt16(ctlCurrencyForm.DataKey.Value);
            TextBox ctlSymbol = (TextBox)ctlCurrencyForm.FindControl("ctlSymbol");
            TextBox ctlComment = (TextBox)ctlCurrencyForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlCurrencyForm.FindControl("ctlActiveChk");

            DbCurrency currency = DbCurrencyService.FindByIdentity(currencyId);
            currency.Symbol = ctlSymbol.Text;
            currency.Comment = ctlComment.Text;
            currency.Active = ctlActiveChk.Checked;
            currency.UpdBy = UserAccount.UserID;
            currency.UpdDate = DateTime.Now;
            currency.UpdPgm = ProgramCode;
            try
            {
                DbCurrencyService.UpdateCurrency(currency);
                ctlCurrencyGrid.DataCountAndBind();
                ctlCurrencyForm.ChangeMode(FormViewMode.ReadOnly);
                CloseCurrencyPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlCurrencyForm_DataBound(object sender, EventArgs e)
        {
            if (ctlCurrencyForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlSymbol = ctlCurrencyForm.FindControl("ctlSymbol") as TextBox;
                ctlSymbol.Focus();
            }
        }
        #endregion

        #region ExchangeForm
        protected void ctlExchangeForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            e.Cancel = true;
        }
        protected void ctlExchangeForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlExchangeForm.ChangeMode(FormViewMode.ReadOnly);
                ctlCurrencyGrid.DataCountAndBind();
                CloseExchangePopUp();
            }
        }
        protected void ctlExchangeForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            //modify by tom 28/01/209
            //SCG.eAccounting.Web.UserControls.Calendar ctlFromDateCalendar = ctlExchangeForm.FindControl("Calendar1") as SCG.eAccounting.Web.UserControls.Calendar;
            //SCG.eAccounting.Web.UserControls.Calendar ctlToDateCalendar = ctlExchangeForm.FindControl("Calendar2") as SCG.eAccounting.Web.UserControls.Calendar;
            UserControls.Calendar ctlFromDateCalendar = ctlExchangeForm.FindControl("Calendar1") as UserControls.Calendar;
            UserControls.Calendar ctlToDateCalendar = ctlExchangeForm.FindControl("Calendar2") as UserControls.Calendar;
            TextBox ctlFromDate = ctlFromDateCalendar.FindControl("txtDate") as TextBox;
            TextBox ctlToDate = ctlToDateCalendar.FindControl("txtDate") as TextBox;
            TextBox ctlBuyRate = (TextBox)ctlExchangeForm.FindControl("ctlBuyRate");
            TextBox ctlSellRate = (TextBox)ctlExchangeForm.FindControl("ctlSellRate");
            TextBox ctlComment = (TextBox)ctlExchangeForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlExchangeForm.FindControl("ctlActiveChk");

            DbExchangeRate exchangeRate = new DbExchangeRate();
            DbCurrency currency = new DbCurrency(UIHelper.ParseShort(ctlCurrencyGrid.SelectedValue.ToString()));
            exchangeRate.Currency = currency;
            if (!string.IsNullOrEmpty(ctlFromDate.Text))
            {
                //exchangeRate.FromDate = UIHelper.ParseDate(ctlFromDateCalendar.DateValue, UserCulture).Value; //UIHelper.ParseDate("01-Jan-2009").Value;
            }
            if (!string.IsNullOrEmpty(ctlToDate.Text))
            {
                //exchangeRate.ToDate = UIHelper.ParseDate(ctlToDateCalendar.DateValue, UserCulture).Value; //UIHelper.ParseDate("01-Jan-2009").Value;
            }
            if (!string.IsNullOrEmpty(ctlBuyRate.Text))
            {
                try
                {
                    exchangeRate.BuyRate = Convert.ToSingle(ctlBuyRate.Text);
                }
                catch (FormatException )
                {
                    ValidationErrors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("BuyrateFormat"));
                }
            }
            if (!string.IsNullOrEmpty(ctlSellRate.Text))
            {
                try
                {
                    exchangeRate.SellRate = Convert.ToSingle(ctlSellRate.Text);
                }
                catch (FormatException )
                {
                    ValidationErrors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("SellrateFormat"));
                }
            }
            exchangeRate.Comment = ctlComment.Text;
            exchangeRate.Active = ctlActiveChk.Checked;
            exchangeRate.CreBy = UserAccount.UserID;
            exchangeRate.CreDate = DateTime.Now;
            exchangeRate.UpdBy = UserAccount.UserID;
            exchangeRate.UpdDate = DateTime.Now;
            exchangeRate.UpdPgm = ProgramCode;
            try
            {
                DbExchangeRateService.AddExchangeRate(exchangeRate);
                ctlExchangeGrid.DataCountAndBind();
                ctlExchangeForm.ChangeMode(FormViewMode.ReadOnly);
                CloseExchangePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlExchangeForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short exchangeRateId = Convert.ToInt16(ctlExchangeForm.DataKey.Value);
            //modify by tom 28/01/2009
            //SCG.eAccounting.Web.UserControls.Calendar ctlFromDateCalendar = ctlExchangeForm.FindControl("Calendar1") as SCG.eAccounting.Web.UserControls.Calendar;
            //SCG.eAccounting.Web.UserControls.Calendar ctlToDateCalendar = ctlExchangeForm.FindControl("Calendar2") as SCG.eAccounting.Web.UserControls.Calendar;
            UserControls.Calendar ctlFromDateCalendar = ctlExchangeForm.FindControl("Calendar1") as UserControls.Calendar;
            UserControls.Calendar ctlToDateCalendar = ctlExchangeForm.FindControl("Calendar2") as UserControls.Calendar;
            TextBox ctlFromDate = ctlFromDateCalendar.FindControl("txtDate") as TextBox;
            TextBox ctlToDate = ctlToDateCalendar.FindControl("txtDate") as TextBox;
            TextBox ctlBuyRate = (TextBox)ctlExchangeForm.FindControl("ctlBuyRate");
            TextBox ctlSellRate = (TextBox)ctlExchangeForm.FindControl("ctlSellRate");
            TextBox ctlComment = (TextBox)ctlExchangeForm.FindControl("ctlComment");
            CheckBox ctlActiveChk = (CheckBox)ctlExchangeForm.FindControl("ctlActiveChk");
            DateTime NewDate = new DateTime();
            float newFloat = new float();
            DbExchangeRate exchangeRate = DbExchangeRateService.FindByIdentity(exchangeRateId);
            if (!string.IsNullOrEmpty(ctlFromDate.Text))
            {
                //exchangeRate.FromDate = UIHelper.ParseDate(ctlFromDateCalendar.DateValue, UserCulture).Value; //UIHelper.ParseDate("12-Jan-2009").Value;
            }
            else
            {
                exchangeRate.FromDate = NewDate;
            }
            if (!string.IsNullOrEmpty(ctlToDate.Text))
            {
                //exchangeRate.ToDate = UIHelper.ParseDate(ctlToDateCalendar.DateValue, UserCulture).Value; //UIHelper.ParseDate("12-Jan-2009").Value;
            }
            else
            {
                exchangeRate.ToDate = NewDate;
            }
            if (!string.IsNullOrEmpty(ctlBuyRate.Text))
            {
                try
                {
                    exchangeRate.BuyRate = Convert.ToSingle(ctlBuyRate.Text);
                }
                catch (FormatException )
                {
                    ValidationErrors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("BuyrateFormat"));
                }
            }
            else
            {
                exchangeRate.BuyRate = newFloat;
            }
            if (!string.IsNullOrEmpty(ctlSellRate.Text))
            {
                try
                {
                    exchangeRate.SellRate = Convert.ToSingle(ctlSellRate.Text);
                }
                catch (FormatException )
                {
                    ValidationErrors.AddError("Currency.Error", new Spring.Validation.ErrorMessage("SellrateFormat"));
                }
            }
            else
            {
                exchangeRate.SellRate = newFloat;
            }
            exchangeRate.Comment = ctlComment.Text;
            exchangeRate.Active = ctlActiveChk.Checked;
            exchangeRate.UpdBy = UserAccount.UserID;
            exchangeRate.UpdDate = DateTime.Now;
            exchangeRate.UpdPgm = ProgramCode;
            try
            {
                DbExchangeRateService.UpdateExchangeRate(exchangeRate);
                ctlExchangeGrid.DataCountAndBind();
                ctlExchangeForm.ChangeMode(FormViewMode.ReadOnly);
                CloseExchangePopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlExchangeForm_DataBound(object sender, EventArgs e)
        {
            if (ctlExchangeForm.CurrentMode != FormViewMode.ReadOnly)
            {

            }
            if (ctlExchangeForm.CurrentMode != FormViewMode.Edit)
            {
                //modify by tom 28/01/2009
                //SCG.eAccounting.Web.UserControls.Calendar ctlCalendar = ctlExchangeForm.FindControl("Calendar1") as SCG.eAccounting.Web.UserControls.Calendar;
                UserControls.Calendar ctlCalendar = ctlExchangeForm.FindControl("Calendar1") as UserControls.Calendar;
            }
        }
        #endregion

        #region Button Currency Grid
        protected void ctlCurrencySearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlCurrencyGrid.DataCountAndBind();

        }

        //protected void ctlCurrencyDelete_Click(object sender, ImageClickEventArgs e)
        //{
        //    foreach (GridViewRow row in ctlCurrencyGrid.Rows)
        //    {
        //        if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelectChk")).Checked))
        //        {
        //            try
        //            {

        //                short currencyId = UIHelper.ParseShort(ctlCurrencyGrid.DataKeys[row.RowIndex].Value.ToString());
        //                DbCurrency currency = SsDbQueryProvider.DbCurrencyQuery.FindProxyByIdentity(currencyId);
        //                DbCurrencyService.Delete(currency);
        //                ExchangeRateGridViewFinish();
        //            }
        //            catch (Exception ex)
        //            {
        //                if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
        //                        "alert('This data is now in use.');", true);
        //                    ctlCurrencyGrid.DataCountAndBind();
        //                    ctlCurrencyUpdatePanel.Update();
        //                }
        //            }
        //        }
        //    }
        //    ctlCurrencyGrid.DataCountAndBind();
        //    ctlCurrencyUpdatePanel.Update();
        //}

        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlCurrencyModalPopupExtender.Show();
            ctlCurrencyForm.ChangeMode(FormViewMode.Insert);
            ctlCurrencyGrid.DataCountAndBind();
            UpdatePanelCurrencyForm.Update();

        }

        #endregion

        #region Button Exchange Grid
        protected void ctlExchangeAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlExchangeModalPopupExtender.Show();
            ctlExchangeForm.ChangeMode(FormViewMode.Insert);
            UpdatePanelExchangeForm.Update();

        }
        protected void ctlExchangeDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlExchangeGrid.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelectChk")).Checked))
                {
                    try
                    {

                        short exchangeId = UIHelper.ParseShort(ctlExchangeGrid.DataKeys[row.RowIndex].Value.ToString());
                        DbExchangeRate exchangeRate = SsDbQueryProvider.DbExchangeRateQuery.FindProxyByIdentity(exchangeId);
                        DbExchangeRateService.Delete(exchangeRate);
                    }
                    catch (Exception ex)
                    {
                        if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                                "alert('This data is now in use.');", true);
                            ctlExchangeGrid.DataCountAndBind();
                            ctlExchangeRateUpdatePanel.Update();
                        }
                    }
                }
            }
            ctlExchangeGrid.DataCountAndBind();
            ctlExchangeRateUpdatePanel.Update();
        }
        protected void ctlExchangeCancel_Click(object sender, EventArgs e)
        {
            ExchangeRateGridViewFinish();
        }
        #endregion

        #region private function
        private void RegisterScriptForGridView(string gridviewClientId, string gridviewHeaderCheckbox)
        {
            string functionName = string.Empty;
            StringBuilder script = new StringBuilder();
            if (gridviewClientId.Equals(ctlCurrencyGrid.ClientID))
            {
                script.Append("function validateCheckBox(objChk, objFlag) ");
                functionName = "validateCheckBox";
            }
            else
            {
                script.Append("function validateCheckBox2(objChk, objFlag) ");
                functionName = "validateCheckBox2";
            }
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + gridviewClientId + "', '" + gridviewHeaderCheckbox + "'); ");
            script.Append("} ");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), functionName, script.ToString(), true);
        }
        public void ExchangeRateGridViewFinish()
        {

            ctlExchangeGrid.DataSource = null;
            ctlExchangeGrid.DataBind();
            ctlExchangeRateUpdatePanel.Update();
            ctlExchangeFds.Visible = false;
            ctlExchangeButton.Visible = false;
            ctlCurrencyGrid.SelectedIndex = -1;
        }
        public void CloseCurrencyPopUp()
        {
            ctlCurrencyModalPopupExtender.Hide();
            ctlCurrencyUpdatePanel.Update();
        }
        public void CloseExchangePopUp()
        {
            ctlExchangeModalPopupExtender.Hide();
            ctlExchangeRateUpdatePanel.Update();
        }
        #endregion

    }
}
