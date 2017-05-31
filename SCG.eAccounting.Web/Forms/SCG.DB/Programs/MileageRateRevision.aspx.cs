using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;
using SCG.DB.Query;
using SCG.eAccounting.BLL;
using SCG.eAccounting.DTO;
using SCG.eAccounting.Query;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO;
using SS.Standard.Security;
using SS.Standard.UI;
using SS.Standard.Utilities;
using System.Data;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class MileageRateRevision : BasePage
    {
        #region Properties

        public IDbMileageRateRevisionService DbMileageRateRevisionService { get; set; }
        public IDbMileageRateRevisionDetailService DbMileageRateRevisionDetailService { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MileageRateRevisionLookUp.OnObjectLookUpCalling += new BaseUserControl.ObjectLookUpCallingEventHandler(ctlMileageRateRevisionLookUp_OnObjectLookUpCalling);
            MileageRateRevisionLookUp.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlMileageRateRevisionLookUp_OnObjectLookUpReturn);
            if (!Page.IsPostBack)
            {
                bool result = ScgDbQueryProvider.DbMileageRateRevisionQuery.ChekPermission(UserAccount.UserID);

                DivMileageIltem.Style.Add("display", "none");
                DivManageMileageRate.Style.Add("display", "none");
                MiRvsEditForm(false);
                ctlMiRvsDetail.Visible = true;
                ctlMiRvsGrid.DataCountAndBind();
                ctlUpdatePanelGridview.Update();
            }
        }
        protected void ctlMileageRateRevisionGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ApproveItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                string ItemId = ctlMiRvsGrid.DataKeys[rowIndex].Values["Id"].ToString();
                string EffectiveFrom = ctlMiRvsGrid.DataKeys[rowIndex].Values["EffectiveFromDate"].ToString();
                string EffectiveTo = ctlMiRvsGrid.DataKeys[rowIndex].Values["EffectiveToDate"].ToString();
                DivMileageIltem.Style.Add("display", "none");

                DbMileageRateRevision MiRvs = new DbMileageRateRevision();
                MiRvs.Id = new Guid(ItemId);
                MiRvs.Status = 1;
                MiRvs.EffectiveFromDate = Convert.ToDateTime(EffectiveFrom);
                MiRvs.EffectiveToDate = Convert.ToDateTime(EffectiveTo);

                DbMileageRateRevisionService.ApproveMileageRateRevision(MiRvs, UserAccount.UserID);
                ctlMiRvsDetail.Visible = false;


            }
            else if (e.CommandName == "ViewItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                string ItemId = ctlMiRvsGrid.DataKeys[rowIndex].Values["Id"].ToString();
                string Status = ctlMiRvsGrid.DataKeys[rowIndex].Values["StatusDesc"].ToString();
                //นำค่า MileageRateRivision Id  ไปเก็บ Hidden Field ของ ตารางล่าง 
                MiRvsId.Value = ItemId;
                MiRvsStatus.Value = Status;
                DivManageMileageRate.Style.Add("display", "none");
                DivMileageIltem.Style.Add("display", "none");
                ctlMiRvsDetail.Visible = true;
                ctlAddItem.Visible = true;
                ctlMiRvsDetail.DataCountAndBind();

            }
            else if (e.CommandName == "RemoveItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                string ItemId = ctlMiRvsGrid.DataKeys[rowIndex].Values["Id"].ToString();

                DbMileageRateRevision MiRvs = new DbMileageRateRevision();
                MiRvs.Id = new Guid(ItemId);

                DbMileageRateRevisionService.RemoveMileageRateRevision(MiRvs);
                ctlMiRvsDetail.Visible = false;
                DivManageMileageRate.Style.Add("display", "none");
                DivMileageIltem.Style.Add("display", "none");
                ctlAddItem.Visible = true;

            }
            else if (e.CommandName == "CancelItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                string ItemId = ctlMiRvsGrid.DataKeys[rowIndex].Values["Id"].ToString();

                string ApprovedDate = ctlMiRvsGrid.DataKeys[rowIndex].Values["ApprovedDate"].ToString();

                string EffectiveFrom = ctlMiRvsGrid.DataKeys[rowIndex].Values["EffectiveFromDate"].ToString();
                string EffectiveTo = ctlMiRvsGrid.DataKeys[rowIndex].Values["EffectiveToDate"].ToString();

                DbMileageRateRevision MiRvs = new DbMileageRateRevision();
                MiRvs.Id = new Guid(ItemId);
                MiRvs.ApprovedDate = Convert.ToDateTime(ApprovedDate);
                MiRvs.EffectiveFromDate = Convert.ToDateTime(EffectiveFrom);
                MiRvs.EffectiveToDate = Convert.ToDateTime(EffectiveTo);
                MiRvs.Status = 2;
                DbMileageRateRevisionService.CancelMileageRateRevision(MiRvs, UserAccount.UserID);
                DivManageMileageRate.Style.Add("display", "none");
                DivMileageIltem.Style.Add("display", "none");
                ctlAddItem.Visible = true;
            }
            else if (e.CommandName == "ImportItem")
            {
                ctlMiRvsDetail.Visible = false;
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                MiRvsItemId.Value = ctlMiRvsGrid.DataKeys[rowIndex].Values["Id"].ToString();
                MileageRateRevisionLookUp.Show();
                DivManageMileageRate.Style.Add("display", "none");
                DivMileageIltem.Style.Add("display", "none");
                ctlAddItem.Visible = true;
            }
            else if (e.CommandName == "EditMiRvsItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                MiRvsId.Value = ctlMiRvsGrid.DataKeys[rowIndex].Values["Id"].ToString();
                DbMileageRateRevision result = ScgDbQueryProvider.DbMileageRateRevisionQuery.FindEffectiveDate(new Guid(MiRvsId.Value));
                ctlEffectiveFrom.Value = result.EffectiveFromDate;
                ctlctlEffectiveTo.Value = result.EffectiveToDate;
                DivMileageIltem.Style.Add("display", "none");
                ctlMiRvsDetail.Visible = false;
                ctlAddItem.Visible = false;
                AddButton.Visible = false;
                UpdateMrrButton.Visible = true;
                DivManageMileageRate.Style.Add("display", "block");
            }

            ctlMiRvsGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }

        protected void ctlAddItem_Click(object sender, ImageClickEventArgs e)
        {
            DivManageMileageRate.Style.Add("display", "block");
            DivMileageIltem.Style.Add("display", "none");
            ctlAddItem.Visible = false;
            ctlMiRvsDetail.Visible = false;
            UpdateMrrButton.Visible = false;
            AddButton.Visible = true;
            ctlEffectiveFrom.Value = null;
            ctlctlEffectiveTo.Value = null;
            ctlUpdatePanelGridview.Update();
        }

        protected void UpdateMrrButton_Click(object sender, ImageClickEventArgs e)
        {
            DbMileageRateRevision MiRvs = new DbMileageRateRevision();
            MiRvs.Id = new Guid(MiRvsId.Value);
            MiRvs.EffectiveFromDate = UIHelper.ParseDate(ctlEffectiveFrom.DateValue);
            MiRvs.EffectiveToDate = UIHelper.ParseDate(ctlctlEffectiveTo.DateValue);
            #region Validate MileageRate
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (MiRvs.EffectiveFromDate > MiRvs.EffectiveToDate)
            {
                errors.AddError("MileageRateRivision.Error", new Spring.Validation.ErrorMessage("Effective From  more than Effective To "));
            }
            #endregion
            if (!errors.IsEmpty)
            {
                this.ValidationErrors.MergeErrors(errors);
                return;
            }

            DbMileageRateRevisionService.UpdateMileageRateRevision(MiRvs, UserAccount.UserID);
            DivManageMileageRate.Style.Add("display", "none");
            ctlAddItem.Visible = true;
            ctlMiRvsGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }

        protected void CancelMrrButton_Click(object sender, ImageClickEventArgs e)
        {
            DivManageMileageRate.Style.Add("display", "none");
            ctlAddItem.Visible = true;
        }

        protected void ctlMileageRateRevisionGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool result = ScgDbQueryProvider.DbMileageRateRevisionQuery.ChekPermission(UserAccount.UserID);
                string status = ctlMiRvsGrid.DataKeys[e.Row.RowIndex].Values["StatusDesc"].ToString();


                if (result == false)
                {
                    ((ImageButton)e.Row.FindControl("BtnApproveDate")).Visible = false;
                    ((ImageButton)e.Row.FindControl("BtnCancel")).Visible = false;

                    if (status.ToLower() == "approve")
                    {
                        ((ImageButton)e.Row.FindControl("BtnRemoveDate")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnImport")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnEdit")).Visible = false;
                    }
                    else if (status.ToLower() == "cancel")
                    {
                        ((ImageButton)e.Row.FindControl("BtnRemoveDate")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnImport")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnEdit")).Visible = false;
                    }
                }
                else
                {

                    if (status.ToLower() == "approve")
                    {
                        ((ImageButton)e.Row.FindControl("BtnApproveDate")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnRemoveDate")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnImport")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnEdit")).Visible = false;
                    }
                    else if (status.ToLower() == "cancel")
                    {

                        ((ImageButton)e.Row.FindControl("BtnRemoveDate")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnApproveDate")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnCancel")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnImport")).Visible = false;
                        ((ImageButton)e.Row.FindControl("BtnEdit")).Visible = false;
                    }
                    else if (status.ToLower() == "draft")
                    {
                        ((ImageButton)e.Row.FindControl("BtnCancel")).Visible = false;
                    }
                }
            }
        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbMileageRateRevisionQuery.GetMileageRateRevisionList(startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            int count = ScgDbQueryProvider.DbMileageRateRevisionQuery.CountByMileageRateRevision();
            return count;
        }

        #region ButtonEvent
        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {

            DbMileageRateRevision MiRvs = new DbMileageRateRevision();
            MiRvs.EffectiveFromDate = UIHelper.ParseDate(ctlEffectiveFrom.DateValue);
            MiRvs.EffectiveToDate = UIHelper.ParseDate(ctlctlEffectiveTo.DateValue);


            #region Validate MileageRate
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (MiRvs.EffectiveFromDate > MiRvs.EffectiveToDate)
            {
                errors.AddError("MileageRateRivision.Error", new Spring.Validation.ErrorMessage("Effective From  more than Effective To "));
            }
            #endregion
            if (!errors.IsEmpty)
            {
                this.ValidationErrors.MergeErrors(errors);
                return;
            }

            DbMileageRateRevisionService.AddMileageRateRevision(MiRvs, UserAccount.UserID);
            ctlEffectiveFrom.DateValue = string.Empty;
            ctlctlEffectiveTo.DateValue = string.Empty;

            ctlMiRvsGrid.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }
        #endregion

        #region MileageRateRivisionItem

        public Object RequestDataMiRvsItem(int startRow, int pageSize, string sortExpression)
        {
            var Id = new Guid(MiRvsId.Value);
            return ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.GetMileageRateRevisionListItem(Id, startRow, pageSize, sortExpression);
        }
        public int RequestCountMiRvsItem()
        {
            var Id = new Guid(MiRvsId.Value);
            int count = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.CountByMileageRateRevisionItem(Id);
            return count;
        }
        protected void ctlMileageRateRevisionItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (MiRvsStatus.Value.ToString().ToLower() == "approve" || MiRvsStatus.Value.ToString().ToLower() == "cancel")
                {
                    ((ImageButton)e.Row.FindControl("BtnEditMiRvsItem")).Visible = false;
                }
            }
        }
        protected void ctlMiRvsItemGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditMiRvsItem")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                if (ctlMiRvsDetail.DataKeys[rowIndex].Values["Id"] != null)
                    MiRvsItemId.Value = ctlMiRvsDetail.DataKeys[rowIndex].Values["Id"].ToString();

                MiRvsProfileId.Value = ctlMiRvsDetail.DataKeys[rowIndex].Values["MileageProfileId"].ToString();
                MiRvsPositionLevel.Value = ctlMiRvsDetail.DataKeys[rowIndex].Values["PersonalLevelGroupCode"].ToString();
                InputCarRate.Text = ctlMiRvsDetail.DataKeys[rowIndex].Values["CarRate"].ToString();
                InputCarRate2.Text = ctlMiRvsDetail.DataKeys[rowIndex].Values["CarRate2"].ToString();
                InputPickUpRate.Text = ctlMiRvsDetail.DataKeys[rowIndex].Values["PickUpRate"].ToString();
                InputPickUpRate2.Text = ctlMiRvsDetail.DataKeys[rowIndex].Values["PickUpRate2"].ToString();
                InputMotocycleRate.Text = ctlMiRvsDetail.DataKeys[rowIndex].Values["MotocycleRate"].ToString();
                InputMotocycleRate2.Text = ctlMiRvsDetail.DataKeys[rowIndex].Values["MotocycleRate2"].ToString();
                DivMileageIltem.Style.Add("display", "block");

                MiRvsEditForm(true);
            }
        }
        protected void ClickSave_MiRvsItem(object sender, ImageClickEventArgs e)
        {
            DbMileageRateRevisionDetail MiRvsItem = new DbMileageRateRevisionDetail();
            Guid revisionItemId = new Guid(MiRvsItemId.Value);
            if (revisionItemId != Guid.Empty)
                MiRvsItem.Id = new Guid(MiRvsItemId.Value);

            MiRvsItem.MileageRateRevisionId = new Guid(MiRvsId.Value);
            MiRvsItem.MileageProfileId = new Guid(MiRvsProfileId.Value);
            MiRvsItem.PersonalLevelGroupCode = MiRvsPositionLevel.Value.ToString();
            MiRvsItem.CarRate = UIHelper.ParseDouble(InputCarRate.Text);
            MiRvsItem.CarRate2 = UIHelper.ParseDouble(InputCarRate2.Text);
            MiRvsItem.PickUpRate = UIHelper.ParseDouble(InputPickUpRate.Text);
            MiRvsItem.PickUpRate2 = UIHelper.ParseDouble(InputPickUpRate2.Text);
            MiRvsItem.MotocycleRate = UIHelper.ParseDouble(InputMotocycleRate.Text);
            MiRvsItem.MotocycleRate2 = UIHelper.ParseDouble(InputMotocycleRate2.Text);
            DbMileageRateRevisionDetailService.AddMileageRateRevisionItem(MiRvsItem, UserAccount.UserID);
            InputCarRate.Text = string.Empty;
            InputCarRate2.Text = string.Empty;
            InputPickUpRate.Text = string.Empty;
            InputPickUpRate2.Text = string.Empty;
            InputMotocycleRate.Text = string.Empty;
            InputMotocycleRate2.Text = string.Empty;
            MiRvsEditForm(false);
            DivMileageIltem.Style.Add("display", "none");
            //ctlMiRvsGrid.DataCountAndBind();
            ctlMiRvsDetail.DataCountAndBind();
            ctlUpdatePanelGridview.Update();
        }
        protected void ClickCancel_MiRvsItem(object sender, ImageClickEventArgs e)
        {
            InputCarRate.Text = string.Empty;
            InputCarRate2.Text = string.Empty;
            InputPickUpRate.Text = string.Empty;
            InputPickUpRate2.Text = string.Empty;
            InputMotocycleRate.Text = string.Empty;
            InputMotocycleRate2.Text = string.Empty;
            MiRvsEditForm(false);
            DivMileageIltem.Style.Add("display", "none");
        }


        protected void MiRvsEditForm(bool visible)
        {
            #region Heading MileageRateRivisionItem Edit Form
            LabelCarRate.Visible = visible;
            LabelCarRate2.Visible = visible;
            LabelPickUpRate.Visible = visible;
            LabelPickUpRate2.Visible = visible;
            LabelMotocycleRate.Visible = visible;
            LabelMotocycleRate2.Visible = visible;
            #endregion

            #region Input  MileageRateRivisionItem Edit Form
            InputCarRate.Visible = visible;
            InputCarRate2.Visible = visible;
            InputPickUpRate.Visible = visible;
            InputPickUpRate2.Visible = visible;
            InputMotocycleRate.Visible = visible;
            InputMotocycleRate2.Visible = visible;
            #endregion

            #region Button Control
            UpdateButton.Visible = visible;
            CancelButton.Visible = visible;

            #endregion
        }

        #endregion


        protected void ctlMileageRateRevisionLookUp_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            MileageRateRevisionLookUp.MRRevitionId = new Guid(MiRvsItemId.Value);
        }
        protected void ctlMileageRateRevisionLookUp_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {

        }
    }
}
