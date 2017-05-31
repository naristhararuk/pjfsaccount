using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.DB.DTO;
using SS.DB.DTO.ValueObject;
using SS.DB.Query;
using SCG.eAccounting.Web.Helper;
using SCG.DB.DTO;
using SS.DB.BLL;
using SS.Standard.UI;
using SS.Standard.Security;
using SS.Standard.Utilities;
using SCG.DB.BLL;
using SCG.DB.Query;
using Spring.Validation;
using SCG.DB.DTO.DataSet;
using SS.DB.Query.Hibernate;
using System.Data;
using SCG.eAccounting.BLL;


namespace SCG.eAccounting.Web.UserControls
{
    public partial class PBEditor : BaseUserControl
    {
        public IDbPBLangService DbPBLangService { get; set; }
        public IDbPBService DbPBService { get; set; }
        public event EventHandler Notify_Ok;
        public event EventHandler Notify_Cancle;



        public string Mode
        {
            get { return this.ViewState["Mode"] == null ? FlagEnum.NewFlag : (string)this.ViewState["Mode"]; }
            set { this.ViewState["Mode"] = value; }
        }
        public long PBID
        {
            get { return this.ViewState["PBID"] == null ? (long)0 : (long)this.ViewState["PBID"]; }
            set { this.ViewState["PBID"] = value; }
        }

        public DBPbDataSet PbDataSet
        {
            get { return (DBPbDataSet)this.ViewState["PbDataSet"]; }
            set { this.ViewState["PbDataSet"] = value; }
        }

        public void ResetValue()
        {
            ctlPBCode.Text = string.Empty;
            ctlPettyCashLimit.Text = string.Empty;
            ctlActive.Checked = true;
            ctlPBEditorGrid.DataCountAndBind();
            ctlRepOffice.Checked = false;
            ctlSupplementary.Text = string.Empty;
            ctlPBUpdatePanel.Update();
        }
        public void Initialize(string mode, long id)
        {
            Mode = mode.ToString();
            PbDataSet = new DBPbDataSet();
            ctlMainCurrencyDropdown.DataSource = SsDbQueryProvider.DbCurrencyQuery.FindAll(); //GetCurrencyLangByCurrencyID();//currencyId, UserAccount.CurrentLanguageID);
            ctlMainCurrencyDropdown.DataTextField = "Symbol";
            ctlMainCurrencyDropdown.DataValueField = "CurrencyID";
            ctlMainCurrencyDropdown.DataBind();
            ctlMainCurrencyDropdown.Items.Insert(0, new ListItem("Please Select", string.Empty));
            PBID = id;
            if (mode.Equals(FlagEnum.EditFlag))
            {
                Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(PBID);
                ctlPBCode.Text = pb.PBCode;
                ctlPettyCashLimit.Text = UIHelper.BindDecimal(pb.PettyCashLimit.ToString());
                ctlSupplementary.Text = pb.Supplementary;
                ctlActive.Checked = pb.Active;
                DbCompany company = ScgDbQueryProvider.DbCompanyQuery.getDbCompanyByCompanyCode(pb.CompanyCode);
                if (company != null)
                {
                    ctlCompanyField.SetValue(company.CompanyID);
                }
                ctlPBEditorGrid.DataCountAndBind();
                IList<DbPBCurrency> list = ScgDbQueryProvider.DbPBCurrencyQuery.FindPBCurrencyByPBID(PBID);
                ctlRepOffice.Checked = pb.RepOffice;

                if (pb.MainCurrencyID.HasValue && pb.MainCurrencyID.Value != 0)
                {
                    ctlMainCurrencyDropdown.SelectedValue = pb.MainCurrencyID.ToString();
                }
                
                //loop add to dataset
                foreach (DbPBCurrency pbCurrency in list)
                {
                    DBPbDataSet.DbPBCurrencyRow row = PbDataSet.DbPBCurrency.NewDbPBCurrencyRow();
                    row.ID = pbCurrency.ID;
                    row.PBID = pbCurrency.PBID;
                    row.CurrencyID = pbCurrency.CurrencyID;
                    row.CreBy = pbCurrency.CreBy;
                    row.CreDate = pbCurrency.CreDate;
                    row.UpdBy = pbCurrency.UpdBy;
                    row.UpdDate = pbCurrency.UpdDate;
                    row.UpdPgm = pbCurrency.UpdPgm;
                    PbDataSet.DbPBCurrency.AddDbPBCurrencyRow(row);
                }
                PbDataSet.AcceptChanges();
                BindLocalCurrency();
              
                ctlPBUpdatePanel.Update();
            }
            else if (mode.ToString() == FlagEnum.NewFlag)
            {
                ResetValue();
                ctlCompanyField.ResetValue();
            }

            if (ctlRepOffice.Checked)
            {
                ctlMainCurrencyLabel.Visible = true;
                ctlMainCurrencyDropdown.Visible = true;
                ctlLocalCurrencyLabel.Visible = true;
                ctlLocalCurrencyDropdown.Visible = true;
                ctlAddLocalCurrencyButton.Visible = true;
                ctlPBLocalCurrencyGridview.Visible = true;
            }
            else
            {
                ctlMainCurrencyLabel.Visible = false;
                ctlMainCurrencyDropdown.Visible = false;
                ctlLocalCurrencyLabel.Visible = false;
                ctlLocalCurrencyDropdown.Visible = false;
                ctlAddLocalCurrencyButton.Visible = false;
                ctlPBLocalCurrencyGridview.Visible = false;
            }
        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return ScgDbQueryProvider.DbPbLangQuery.FindPBLangByPBID(PBID);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
           
            Dbpb pb = new Dbpb();
            if (Mode.Equals(FlagEnum.EditFlag))
            {
                pb.Pbid = PBID;
            }

            pb.PBCode = ctlPBCode.Text;
            pb.Supplementary = ctlSupplementary.Text;
            pb.Active = ctlActive.Checked;
            pb.UpdBy = UserAccount.UserID;
            pb.UpdDate = DateTime.Now;
            pb.UpdPgm = UserAccount.CurrentLanguageCode;
            pb.CreDate = DateTime.Now;
            pb.CreBy = UserAccount.UserID;
            pb.CurrencyID = UIHelper.ParseLong(ctlMainCurrencyDropdown.SelectedValue);
            pb.RepOffice = ctlRepOffice.Checked;
            if (!string.IsNullOrEmpty(ctlMainCurrencyDropdown.SelectedValue))
            {
                pb.MainCurrencyID = UIHelper.ParseShort(ctlMainCurrencyDropdown.SelectedValue);
            }
            try
            {
                pb.PettyCashLimit = UIHelper.ParseDouble(ctlPettyCashLimit.Text);
                DbCompany com = ScgDbQueryProvider.DbCompanyQuery.FindByIdentity(UIHelper.ParseLong(ctlCompanyField.CompanyID));
                pb.CompanyCode = com.CompanyCode;
                pb.CompanyID = com;
            }
            catch (Exception)
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                errors.AddError("PB.Error", new ErrorMessage("Check For Petty Cash Limit or Company Code."));
                ValidationErrors.MergeErrors(errors);
                return;
            }
            try
            {
                // save or update PB
                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbPBService.UpdatePB(pb, PbDataSet);
                }
                else
                {
                    long PBId = DbPBService.AddPB(pb, PbDataSet);
                    pb.Pbid = PBId;

                }

                // save or update PBlang
                IList<DbpbLang> list = new List<DbpbLang>();

                foreach (GridViewRow row in ctlPBEditorGrid.Rows)
                {
                    short languageId = UIHelper.ParseShort(ctlPBEditorGrid.DataKeys[row.RowIndex]["LanguageID"].ToString());

                    TextBox Description = row.FindControl("ctrDescription") as TextBox;
                    TextBox Comment = (TextBox)row.FindControl("ctrComment") as TextBox;
                    CheckBox Active = (CheckBox)row.FindControl("ctlActive") as CheckBox;

                    //comment by oum 02/06/2009
                    //ไม่ต้อง check description is null or empty เพราะว่าไม่ใช่ require field
                    //if ((!string.IsNullOrEmpty(Description.Text)))
                    //{
                    DbpbLang pbLang = new DbpbLang();
                    pbLang.Active = Active.Checked;
                    pbLang.CreBy = UserAccount.UserID;
                    pbLang.CreDate = DateTime.Now;
                    pbLang.Description = Description.Text;
                    pbLang.Comment = Comment.Text;
                    pbLang.LanguageID = new DbLanguage(languageId);
                    pbLang.PBID = pb;
                    pbLang.UpdBy = UserAccount.UserID;
                    pbLang.UpdDate = DateTime.Now;
                    pbLang.UpdPgm = UserAccount.CurrentLanguageCode;
                    list.Add(pbLang);
                    //}

                }

                if (Mode.Equals(FlagEnum.EditFlag))
                {
                    DbPBLangService.UpdatePBLang(list);

                }
                if (Mode.Equals(FlagEnum.NewFlag))
                {
                    DbPBLangService.AddPBLang(list);
                }

                Notify_Ok(sender, e);

            }
            catch (ServiceValidationException ex)
            {
                this.ValidationErrors.MergeErrors(ex.ValidationErrors);
            }
            //}

        }




        protected void PBEditor_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlPBEditorGrid.Rows)
            {
                TextBox ctrDescription = row.FindControl("ctrDescription") as TextBox;
                TextBox ctrComment = row.FindControl("ctrComment") as TextBox;
                CheckBox active = row.FindControl("ctlActive") as CheckBox;

                if ((string.IsNullOrEmpty(ctrDescription.Text)) && (string.IsNullOrEmpty(ctrComment.Text)))
                {

                    active.Checked = true;
                }
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
        protected void ctlPB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        public void HidePopUp()
        {
            ctlPBEditorModalPopupExtender.Hide();
        }
        public void ShowPopUp()
        {

            ctlPBEditorModalPopupExtender.Show();

        }


        protected void ctlAddLocal_Click(object sender, ImageClickEventArgs e)
        {
            if (UIHelper.ParseShort(ctlLocalCurrencyDropdown.SelectedValue) > 0)
            {
                DataRow[] rows = PbDataSet.DbPBCurrency.Select(string.Format("CurrencyID='{0}'",UIHelper.ParseShort(ctlLocalCurrencyDropdown.SelectedValue)));

                if (rows.Length == 0)
                {                          
                    DBPbDataSet.DbPBCurrencyRow row = PbDataSet.DbPBCurrency.NewDbPBCurrencyRow();                   
                    row.PBID = this.PBID;
                    row.CurrencyID = UIHelper.ParseShort(ctlLocalCurrencyDropdown.SelectedValue);
                    row.UpdBy = UserAccount.UserID;
                    row.UpdDate = DateTime.Now;
                    row.CreBy = UserAccount.UserID;
                    row.CreDate = DateTime.Now;
                    row.UpdPgm = UserAccount.CurrentProgramCode;
                    PbDataSet.DbPBCurrency.AddDbPBCurrencyRow(row);
                    ctlErrorLocalCurrencyLabel.Visible = false;
                    ctlErrorLocalCurrencyLabel.Text = string.Empty;
                    ctlLocalCurrencyDropdown.ResetControl();
                }
                else
                {
                    ctlErrorLocalCurrencyLabel.Visible = true;
                    ctlErrorLocalCurrencyLabel.Text = "LocalCurrencyIsDuplicate";
                }

                BindLocalCurrency();
            }
            ctlPBUpdatePanel.Update();
        }

        protected void ctlPBLocalCurrencyGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                short currencyId = UIHelper.ParseShort(ctlPBLocalCurrencyGridview.DataKeys[e.Row.RowIndex]["CurrencyID"].ToString()); // get currency id from gridview  
               
                VOUCurrencySetup Currency = SsDbQueryProvider.DbCurrencyQuery.GetCurrencyLangByCurrencyID(currencyId, UserAccount.CurrentLanguageID);

                Literal symbol = e.Row.FindControl("ctlLocalCurrency") as Literal;
                if (symbol != null)
                    symbol.Text = Currency.Symbol;

                Literal description = e.Row.FindControl("ctrLocalDescription") as Literal;
                if (description != null)
                    description.Text = Currency.Description;
            }
        }
        protected void BindLocalCurrency()
        {
            ctlPBLocalCurrencyGridview.DataSource = PbDataSet.DbPBCurrency.Select().OrderBy(t => t.Field<DateTime>("CreDate")).ToList();
            ctlPBLocalCurrencyGridview.DataBind();
        }


        protected void ctlPBLocalCurrencyGridview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteLocalCurrency")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short currencyId = UIHelper.ParseShort(ctlPBLocalCurrencyGridview.DataKeys[rowIndex]["CurrencyID"].ToString()); // get currency id from gridview  
                DataRow[] rows = PbDataSet.DbPBCurrency.Select(string.Format("CurrencyID='{0}'", currencyId));
                foreach (DataRow row in rows)
                {
                    row.Delete();
                }
                BindLocalCurrency();
            }
            ctlPBUpdatePanel.Update();

        }


        protected void ctlRepOffice_CheckedChanged(object sender, EventArgs e)
        {
            if (ctlRepOffice.Checked)
            {
                ctlMainCurrencyLabel.Visible = true;
                ctlMainCurrencyDropdown.Visible = true;         
                ctlLocalCurrencyLabel.Visible = true;
                ctlLocalCurrencyDropdown.Visible = true;
                ctlAddLocalCurrencyButton.Visible = true;
                ctlPBLocalCurrencyGridview.Visible = true;
            }else
            {
                ctlMainCurrencyLabel.Visible = false;
                ctlMainCurrencyDropdown.Visible = false;
                ctlLocalCurrencyLabel.Visible = false;
                ctlLocalCurrencyDropdown.Visible = false;
                ctlAddLocalCurrencyButton.Visible = false;
                ctlPBLocalCurrencyGridview.Visible = false;
            }
        }
    }
}