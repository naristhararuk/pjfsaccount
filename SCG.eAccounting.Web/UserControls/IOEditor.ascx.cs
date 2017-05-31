using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
using SCG.DB.BLL;
using Spring.Validation;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class IOEditor : BaseUserControl
    {
        
        public IDbIOService DbIOService { get; set; }
       
        #region Properties
        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long IOID
        {
            get { return this.ViewState["IOID"] == null ? (long)0 : (long)this.ViewState["IOID"]; }
            set { this.ViewState["IOID"] = value; }
        }
        #endregion

        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;

        public void ResetValue()
        {
            ctlIONumber.Visible = true;
            ctlIONumber.Text = string.Empty;
            ctlIONumberLabelDisplay.Visible = false;
            ctlIONumberLabelDisplay.Text = string.Empty;
            ctlIOType.Text = string.Empty;
            ctlIOText.Text = string.Empty;
            ctlCostCenterField.ResetValue();
            ctlCompanyField.ResetValue();

            ctlCalEffectiveDate.DateValue = string.Empty;
            ctlCalLastDisplayDate.DateValue = string.Empty;
            ctlIOActive.Checked = true;
            ctlBusinessArea.Text = string.Empty;
            ctlProfitCenter.Text = string.Empty;
            ctlIOUpdatePanel.Update();
        }

        public void Initialize(string mode, long ioId)
        {
            Mode = mode;
            IOID = ioId;
           
           
            if (Mode.Equals(FlagEnum.EditFlag) || !ioId.Equals(0))
            {
                //ctlIONumber.Enabled = false;
                DbInternalOrder io = ScgDbQueryProvider.DbIOQuery.FindProxyByIdentity(IOID);
                //DbCompany dbCompany = new DbCompany();
                //if (io != null && io.CompanyID.HasValue)
                //    dbCompany = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(io.CompanyID.Value);

              //  ctlIONumber.Text                = io.IONumber;
                ctlIONumber.Visible = false;
                ctlIONumberLabelDisplay.Text = io.IONumber;
                ctlIONumberLabelDisplay.Visible = true;
                ctlIOType.Text                  = io.IOType;
                ctlIOText.Text                  = io.IOText;

                ctlCalEffectiveDate.DateValue   = UIHelper.BindDate(io.EffectiveDate.ToString());
                ctlCalLastDisplayDate.DateValue = UIHelper.BindDate(io.ExpireDate.ToString());
                ctlIOActive.Checked             = io.Active;
                ctlBusinessArea.Text            = io.BusinessArea;
                ctlProfitCenter.Text            = io.ProfitCenter;
                try
                {
                    if (io.CostCenterID != null)
                        ctlCostCenterField.SetValue(io.CostCenterID.Value);
                }
                catch { }
                try
                {
                    if (io.CompanyCode != null)
                    {
                        //ctlCompanyField.SetValue(io.CompanyID.Value);
                        
                       // long companycode = UIHelper.ParseLong(io.CompanyCode);
                        ctlCompanyField.SetValue(io.CompanyID.Value);
                    }
                }
                catch { }
                
                ctlIOUpdatePanel.Update();
            }
            else if (Mode.Equals(FlagEnum.NewFlag))
            {
                ResetValue();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlCostCenterField.SetWidthTextBox(148);
            ctlCompanyField.SetWidthTextBox(148);
            //ctlCostCenterField.OnObjectLookUpReturn += new BaseUserControl.ObjectLookUpReturnEventHandler(ctlCostCenterField_OnObjectLookUpReturn);
        }

        //protected void ctlCostCenterField_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        //{
        //    DbCostCenter costCenter = (DbCostCenter)e.ObjectReturn;
        //    if (costCenter != null)
        //    {
        //        DbCostCenter dbCost = ScgDbQueryProvider.DbCostCenterQuery.FindProxyByIdentity(costCenter.CostCenterID);
        //        if (dbCost != null)
        //        {
        //            ctlCompanyCode.Text = dbCost.CompanyID.CompanyCode.ToString();
        //            ctlCompanyName.Text = dbCost.CompanyID.CompanyName;
        //        }
        //    }
        //    ctlIOUpdatePanel.Update();
        //}

        private void CheckDataValueUpdate(DbInternalOrder io)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(io.IONumber))
            {
                errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("RequiredIONumber"));
            }
            if (string.IsNullOrEmpty(io.IOType))
            {
                errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("RequiredIOType"));
            }
            if (string.IsNullOrEmpty(io.IOText))
            {
                errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("RequiredIOText"));
            }
            if (io.CompanyID == null || io.CompanyID<=0)
            {
                errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("RequiredCompany"));
            }
            if (!(io.ExpireDate == null || io.EffectiveDate == null))
            {
                if (io.ExpireDate < io.EffectiveDate)
                {
                    errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("ExpireDateMustBeMoreThanEffectiveDate"));
                }
            }
            if (DbIOService.IsExistIO(io) && Mode.Equals(FlagEnum.NewFlag))
            {
                errors.AddError("InternalOrder.Error", new Spring.Validation.ErrorMessage("Duplicate IO Code"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
        }

        protected void ctlInsert_Click1(object sender, ImageClickEventArgs e)
        {
            DbInternalOrder io;
            try
            {
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    io = ScgDbQueryProvider.DbIOQuery.FindByIdentity(IOID);
                }
                else
                {
                    io = new DbInternalOrder();
                    io.IONumber = ctlIONumber.Text;
                }

                
                io.IOType   = ctlIOType.Text;
                io.IOText   = ctlIOText.Text;
                io.Active   = ctlIOActive.Checked;
                io.BusinessArea = ctlBusinessArea.Text;
                io.ProfitCenter = ctlProfitCenter.Text;

                DbCostCenter cost = ScgDbQueryProvider.DbCostCenterQuery.FindByIdentity(UIHelper.ParseLong(ctlCostCenterField.CostCenterId));
                if (cost != null)
                {
                    io.CostCenterID = cost.CostCenterID;
                    io.CostCenterCode = cost.CostCenterCode;
                }

                DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                if (com != null)
                {
                    io.CompanyID = com.CompanyID;
                    io.CompanyCode = com.CompanyCode;
                }

                io.CreBy    = UserAccount.UserID;
                io.CreDate  = DateTime.Now;
                io.UpdBy    = UserAccount.UserID;
                io.UpdDate  = DateTime.Now;
                io.UpdPgm   = UserAccount.CurrentLanguageCode;
          
                if (!string.IsNullOrEmpty(ctlCalEffectiveDate.DateValue))
                {
                    try
                    {
                        io.EffectiveDate = UIHelper.ParseDate(ctlCalEffectiveDate.DateValue).Value;
                    }
                    catch {}
                }
                else
                {
                    io.EffectiveDate = null;
                }

                if (!string.IsNullOrEmpty(ctlCalLastDisplayDate.DateValue))
                {
                    try
                    {
                        io.ExpireDate = UIHelper.ParseDate(ctlCalLastDisplayDate.DateValue).Value;
                    }
                    catch { }
                }
                else
                {
                    io.ExpireDate = null;
                }

                CheckDataValueUpdate(io);
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbIOService.UpdateIO(io);
                }
                else
                {
                    DbIOService.AddIO(io);
                }
                Notify_Ok(sender, e);
            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
          
            catch (NullReferenceException)
            {
                //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                //errors.AddError("InternalOrder.Error", new ErrorMessage("CostCenter and Company Require."));
                //ValidationErrors.MergeErrors(errors);
                //return;
            }

        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Notify_Cancle != null)
            {
                Notify_Cancle(sender, e);
            }
            HidePopUp();

        }
        public void HidePopUp()
        {
            ctlIOModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {
            ctlIOModalPopupExtender.Show();
        }
    }
}
