using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.BLL;
using SCG.eAccounting.Web.Helper;
using SS.DB.DTO.ValueObject;

using SS.DB.DAL;
using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.Standard.Utilities;
using SS.SU.DTO;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class ProvinceLookUp : BaseUserControl
    {
        public IDbProvinceService DbProvinceService { get; set; }

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region <== Property ==>

        #region public string ProvinceID
        public string ProvinceID
        {
            get { return ctlTxtProvinceNo.Text; }
            set { ctlTxtProvinceNo.Text = value; }
        }
        #endregion public string ProvinceID

        #region public string ProvinceName
        public string ProvinceName
        {
            get { return ctlTxtProvinceName.Text; }
            set { ctlTxtProvinceName.Text = value; }
        }
        #endregion public string ProvinceName

        #region public string RegionID
        public string RegionID
        { 
            get;    
            set;
        }
        #endregion public string RegionID

        #region public bool IsMultiSelect
        public bool IsMultiSelect
        {
            get 
            {
                if (ViewState["IsMultiSelect"] == null)
                    return false;
                else
                    return bool.Parse(ViewState["IsMultiSelect"].ToString());
            }
            set 
            {
                ViewState["IsMultiSelect"] = value;
            }
        }
        #endregion public bool IsMultiSelect

        #endregion <== Property ==>

        #region <== Province Grid ==>

        #region public Object RequestData(int startRow, int pageSize, string sortExpression)
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return SsDbQueryProvider.DbProvinceQuery.GetProvinceLovList(new ProvinceLang(), UserAccount.CurrentLanguageID, startRow, pageSize, sortExpression, ctlTxtProvinceNo.Text, ctlTxtProvinceName.Text, ((ctlCmbRegionCode.SelectedIndex == 0) ? "" : ctlCmbRegionCode.SelectedValue));
        }
        #endregion public Object RequestData(int startRow, int pageSize, string sortExpression)

        #region public int RequestCount()
        public int RequestCount()
        {
            int count = SsDbQueryProvider.DbProvinceQuery.CountByProvinceLovCriteria(new ProvinceLang(), ctlTxtProvinceNo.Text, ctlTxtProvinceName.Text, ( (ctlCmbRegionCode.SelectedIndex == 0) ? "" : ctlCmbRegionCode.SelectedValue ) );
            return count;
        }
        #endregion public int RequestCount()

        #region protected void ctlGridProvince_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridProvince_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (this.IsMultiSelect == false)
            {
                if (e.CommandName.Equals("Select"))
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    short provinceId = UIHelper.ParseShort(ctlGridProvince.DataKeys[rowIndex].Value.ToString());
                    DbProvince province = DbProvinceService.FindByIdentity(provinceId);

                    Label ctlProvinceNameInGrid = ctlGridProvince.Rows[rowIndex].FindControl("ctlLblProvinceName") as Label;
                    Label ctlRegionNameInGrid = ctlGridProvince.Rows[rowIndex].FindControl("ctlLblRegionName") as Label;

                    DBProvinceLovReturn lovReturn = new DBProvinceLovReturn();
                    lovReturn.ProvinceID    = province.Provinceid;
                    lovReturn.RegionID      = province.Region.Regionid;
                    lovReturn.ProvinceName  = ctlProvinceNameInGrid.Text;
                    lovReturn.RegionName    = ctlRegionNameInGrid.Text;

                    CallOnObjectLookUpReturn(lovReturn);
                    Hide();
                }
            }
        }
        #endregion protected void ctlGridProvince_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlGridProvince_DataBound(object sender, EventArgs e)
        protected void ctlGridProvince_DataBound(object sender, EventArgs e)
        {
            if (this.IsMultiSelect==true)
            {
                if (ctlGridProvince.Rows.Count > 0)
                {
                    RegisterScriptForGridView();
                }
            }
        }
        #endregion protected void ctlGridProvince_DataBound(object sender, EventArgs e)

        #endregion <== Province Grid ==>

        #region <== Buttom Search ==>
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlGridProvince.DataCountAndBind();
            UpdatePanelGridView.Update();
        }

        protected void ctlSelect_Click(object sender, ImageClickEventArgs e)
        {
            if (this.IsMultiSelect == true)
            {
                IList<DBProvinceLovReturn> dbProvinceList = new List<DBProvinceLovReturn>();

                foreach (GridViewRow row in ctlGridProvince.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                    {
                        short provinceId = UIHelper.ParseShort(ctlGridProvince.DataKeys[row.RowIndex].Value.ToString());
                        DbProvince province = DbProvinceService.FindByIdentity(provinceId);

                        Label ctlProvinceNameInGrid = ctlGridProvince.Rows[row.RowIndex].FindControl("ctlLblProvinceName") as Label;
                        Label ctlRegionNameInGrid = ctlGridProvince.Rows[row.RowIndex].FindControl("ctlLblRegionName") as Label;

                        DBProvinceLovReturn lovReturn = new DBProvinceLovReturn();
                        lovReturn.ProvinceID = province.Provinceid;
                        lovReturn.RegionID = province.Region.Regionid;
                        lovReturn.ProvinceName = ctlProvinceNameInGrid.Text;
                        lovReturn.RegionName = ctlRegionNameInGrid.Text;

                        dbProvinceList.Add(lovReturn);
                    }
                }

                CallOnObjectLookUpReturn(dbProvinceList);
                Hide();
            }
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            this.Hide();
        }

        #endregion <== Buttom Search ==>

        #region <== Private Function ==>

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridProvince.ClientID + "', '" + ctlGridProvince.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region public void SetCombo()
        private void SetCombo()
        {
            ctlCmbRegionCode.DataSource = SsDbQueryProvider.DbRegionLangQuery.FindRegionByLangCriteria(UserAccount.CurrentLanguageID);
            ctlCmbRegionCode.DataTextField = "Text";
            ctlCmbRegionCode.DataValueField = "Id";
            ctlCmbRegionCode.DataBind();

            ctlCmbRegionCode.Items.Insert(0, new ListItem("--Please Select--", ""));

        }
        #endregion public void SetCombo()

        #endregion <== Private Function ==>

        #region <== Public Function ==>

        #region public void Show()
        public void Show()
        {
            #region Assign Value
            this.SetCombo();

            ctlTxtProvinceNo.Text = this.ProvinceID;
            ctlTxtProvinceName.Text = this.ProvinceName;
            ctlCmbRegionCode.SelectedValue = this.RegionID;

            ctlGridProvince.DataCountAndBind();
            #endregion Assign Value

            if (this.IsMultiSelect == false)
            {
                ctlGridProvince.Columns[0].Visible = false;
                ctlGridProvince.Columns[4].Visible = true;
                ctbSelect.Visible = false;
                ctlLblLine.Visible = false;
            }
            else
            {
                ctlGridProvince.Columns[0].Visible = true;
                ctlGridProvince.Columns[4].Visible = false;
                ctbSelect.Visible = true;
                ctlLblLine.Visible = true;
            }

            CallOnObjectLookUpCalling();
            this.UpdatePanelSearchProvince.Update();
            this.UpdatePanelGridView.Update();
            this.ModalPopupExtender1.Show();
        }
        #endregion public void Show()

        #region public void Hide()
        public void Hide()
        {
            this.ProvinceID = "";
            this.ProvinceName = "";
            this.RegionID = "";

            ctlTxtProvinceNo.Text = "";
            ctlTxtProvinceName.Text = "";
            ctlCmbRegionCode.DataSource = null;

            this.ModalPopupExtender1.Hide();
        }
        #endregion public void Hide()

        #endregion <== Public Function ==>
    }
}