using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using SS.Standard.UI;
using SCG.DB.Query;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SCG.DB.BLL;
using SS.Standard.Utilities;
using SS.DB.Query;
using System.Text;
using SS.DB.DTO;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class PaymentMethod : BasePage
    {
        #region Properties
        public IDbPaymentMethodService DbPaymentMethodService { get; set; }
        #endregion

        #region EventLoad
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlPaymentMethodGrid.DataCountAndBind();
                ctlUpdatePanelPaymentMethodGridview.Update();
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        #endregion

        #region PaymentMethod Gridview Event
        protected void ctlPaymentMethodGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("PaymentMethodEdit"))
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short paymentMethodId = Convert.ToInt16(ctlPaymentMethodGrid.DataKeys[rowIndex].Value);
                ctlPaymentMethodForm.PageIndex = (ctlPaymentMethodGrid.PageIndex * ctlPaymentMethodGrid.PageSize) + rowIndex;
                ctlPaymentMethodForm.ChangeMode(FormViewMode.Edit);
                IList<DbPaymentMethod> paymentMethodList = new List<DbPaymentMethod>();
                DbPaymentMethod paymentMethod = DbPaymentMethodService.FindByIdentity(paymentMethodId);
                paymentMethodList.Add(paymentMethod);
                ctlPaymentMethodForm.DataSource = paymentMethodList;
                ctlPaymentMethodForm.DataBind();

                ctlUpdatePanelPaymentMethodForm.Update();
                ctlPaymentMethodModalPopupExtender.Show();
            }
            if (e.CommandName.Equals("PaymentMethodDelete"))
            {
                try
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    long paymentMethodId = UIHelper.ParseLong(ctlPaymentMethodGrid.DataKeys[rowIndex].Value.ToString());
                    DbPaymentMethod paymentMethod = ScgDbQueryProvider.DbPaymentMethodQuery.FindByIdentity(paymentMethodId);
                    DbPaymentMethodService.Delete(paymentMethod);
                 
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlPaymentMethodGrid.DataCountAndBind();
                    }
                }

                ctlPaymentMethodGrid.DataCountAndBind();
                ctlUpdatePanelPaymentMethodGridview.Update();
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            DbPaymentMethod paymentMethod = GetPaymentMethodCriteria();

            return ScgDbQueryProvider.DbPaymentMethodQuery.GetPaymentMethodList(paymentMethod, startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            DbPaymentMethod paymentMethod = GetPaymentMethodCriteria();

            int count = ScgDbQueryProvider.DbPaymentMethodQuery.CountPaymentMethodByCriteria(paymentMethod);

            return count;
        }
        public DbPaymentMethod GetPaymentMethodCriteria()
        {
            DbPaymentMethod paymentMethod = new DbPaymentMethod();
            paymentMethod.PaymentMethodCode = ctlPaymentMethodCodeCri.Text;
            paymentMethod.PaymentMethodName = ctlPaymentMethodNameCri.Text;
            return paymentMethod;
        }
        #endregion

        #region Button Event
        protected void ctlPaymentMethodSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlPaymentMethodGrid.DataCountAndBind();
            ctlUpdatePanelPaymentMethodGridview.Update();
        }

        protected void ctlPaymentMethodAddNew_Click(object sender, ImageClickEventArgs e)
        {
            ctlPaymentMethodModalPopupExtender.Show();
            ctlPaymentMethodForm.ChangeMode(FormViewMode.Insert);
            ctlUpdatePanelPaymentMethodForm.Update();
        }
        #endregion

        #region PaymentMethod Form Event
        protected void ctlPaymentMethodForm_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            //e.Cancel = true;
        }
        protected void ctlPaymentMethodForm_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlPaymentMethodGrid.DataCountAndBind();
                ClosePaymentMethodPopUp();
            }
        }
        protected void ctlPaymentMethodForm_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            TextBox ctlPaymentMethodCode = (TextBox)ctlPaymentMethodForm.FindControl("ctlPaymentMethodCode");
            TextBox ctlPaymentMethodName = (TextBox)ctlPaymentMethodForm.FindControl("ctlPaymentMethodName");
            CheckBox ctlActiveChk = (CheckBox)ctlPaymentMethodForm.FindControl("ctlActiveChk");

            DbPaymentMethod paymentMethod = new DbPaymentMethod();
            paymentMethod.PaymentMethodCode = ctlPaymentMethodCode.Text;
            paymentMethod.PaymentMethodName = ctlPaymentMethodName.Text;
            paymentMethod.Active = ctlActiveChk.Checked;
            paymentMethod.CreBy = UserAccount.UserID;
            paymentMethod.CreDate = DateTime.Now;
            paymentMethod.UpdBy = UserAccount.UserID;
            paymentMethod.UpdDate = DateTime.Now;
            paymentMethod.UpdPgm = ProgramCode;

            try
            {
                DbPaymentMethodService.AddPaymentMethod(paymentMethod);
                ctlPaymentMethodGrid.DataCountAndBind();
                ctlPaymentMethodForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePaymentMethodPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlPaymentMethodForm_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short paymentMethodId = Convert.ToInt16(ctlPaymentMethodForm.DataKey.Value);
            TextBox ctlPaymentMethodCode = (TextBox)ctlPaymentMethodForm.FindControl("ctlPaymentMethodCode");
            TextBox ctlPaymentMethodName = (TextBox)ctlPaymentMethodForm.FindControl("ctlPaymentMethodName");
            CheckBox ctlActiveChk = (CheckBox)ctlPaymentMethodForm.FindControl("ctlActiveChk");

            DbPaymentMethod paymentMethod = DbPaymentMethodService.FindByIdentity(paymentMethodId);
            paymentMethod.PaymentMethodCode = ctlPaymentMethodCode.Text;
            paymentMethod.PaymentMethodName = ctlPaymentMethodName.Text;

            paymentMethod.Active = ctlActiveChk.Checked;
            paymentMethod.CreBy = UserAccount.UserID;
            paymentMethod.CreDate = DateTime.Now;
            paymentMethod.UpdBy = UserAccount.UserID;
            paymentMethod.UpdDate = DateTime.Now;
            paymentMethod.UpdPgm = ProgramCode;

            try
            {
                DbPaymentMethodService.UpdatePaymentMethod(paymentMethod);
                ctlPaymentMethodGrid.DataCountAndBind();
                ctlPaymentMethodForm.ChangeMode(FormViewMode.ReadOnly);
                ClosePaymentMethodPopUp();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
        }
        protected void ctlPaymentMethodForm_DataBound(object sender, EventArgs e)
        {
            if (ctlPaymentMethodForm.CurrentMode != FormViewMode.ReadOnly)
            {
                TextBox ctlPaymentMethodCode = (TextBox)ctlPaymentMethodForm.FindControl("ctlPaymentMethodCode");
                ctlPaymentMethodCode.Focus();
                CheckBox ctlActiveCheck = (CheckBox)ctlPaymentMethodForm.FindControl("ctlActiveChk");

                if (ctlPaymentMethodForm.CurrentMode == FormViewMode.Edit)
                {
                    short paymentMethodID = UIHelper.ParseShort(ctlPaymentMethodForm.DataKey.Value.ToString());
                    DbPaymentMethod paymentMethod = DbPaymentMethodService.FindByIdentity(paymentMethodID);
                }
                if (ctlPaymentMethodForm.CurrentMode == FormViewMode.Insert)
                {
                    ctlActiveCheck.Checked = true;
                }
            }
        }
        #endregion

        #region Public Function
        public void ClosePaymentMethodPopUp()
        {
            ctlPaymentMethodModalPopupExtender.Hide();
            ctlPaymentMethodForm.ChangeMode(FormViewMode.ReadOnly);
            ctlUpdatePanelPaymentMethodGridview.Update();
        }
        #endregion
    }
}
