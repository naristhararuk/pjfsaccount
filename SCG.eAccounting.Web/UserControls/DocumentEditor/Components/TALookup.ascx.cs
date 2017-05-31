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
using SCG.eAccounting.DTO.ValueObject;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class TALookup : BaseUserControl
    {
        #region Property

        #region public string DocumentNo
        public string DocumentNo
        {
            get { return ctlTxtDocumentNo.Text; }
            set { ctlTxtDocumentNo.Text = value; }
        }
        #endregion public string DocumentNo

        #region public string Creator
        public string Creator
        {
            get { return ctlTxtCreator.Text; }
            set { ctlTxtCreator.Text = value; }
        }
        #endregion public string Creator

        #region public string CreateDate
        public string CreateDate
        {
            get { return ctlCalendar.DateValue; }
            set { ctlCalendar.DateValue = value; }
        }
        #endregion public string CreateDate

        #region public string DocumentStatus
        public string DocumentStatus
        {
            get;
            set;
        }
        #endregion public string DocumentStatus

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
        #endregion Property

        #region <== Public Function ==>

        #region public void Show()
        public void Show()
        {
            #region Assign Value
            this.SetCombo();

            ctlTxtDocumentNo.Text = this.DocumentNo;
            ctlTxtCreator.Text = this.Creator;
            ctlCalendar.DateValue = this.CreateDate;

            LoadData();
            #endregion Assign Value

            if (!IsMultiSelect)
            {
                ctlGridTADocument.Columns[0].Visible = false;
                ctlGridTADocument.Columns[5].Visible = true;
                ctlImgSelect.Visible = false;
                ctlLblLine.Visible = false;
            }
            else
            {
                ctlGridTADocument.Columns[0].Visible = true;
                ctlGridTADocument.Columns[5].Visible = false;
                ctlImgSelect.Visible = true;
                ctlLblLine.Visible = true;
            }

            CallOnObjectLookUpCalling();
            this.cltUpdatePanelTADocumentSearch.Update();
            this.ctlUpdatePanelTADocumentGridView.Update();
            this.ctlModalPopupExtenderTADocument.Show();
        }
        #endregion public void Show()

        #region public void Hide()
        public void Hide()
        {
            this.DocumentNo = string.Empty;
            this.Creator = string.Empty;
            this.CreateDate = string.Empty;
            this.DocumentStatus = string.Empty;

            ctlTxtDocumentNo.Text = string.Empty;
            ctlTxtCreator.Text = string.Empty;
            ctlCalendar.DateValue = string.Empty;

            this.ctlModalPopupExtenderTADocument.Hide();
        }
        #endregion public void Hide()

        #region public void LoadData()
        public void LoadData()
        {
            List<TADocumentObj> taDocumentList = new List<TADocumentObj>();

            #region Set List
            TADocumentObj tmp1 = new TADocumentObj();
            tmp1.DocumentNo = "1";
            tmp1.Creator = "admin";
            tmp1.CreateDate = "02/10/2009";
            taDocumentList.Add(tmp1);

            TADocumentObj tmp2 = new TADocumentObj();
            tmp2.DocumentNo = "2";
            tmp2.Creator = "admin";
            tmp2.CreateDate = "02/10/2009";
            taDocumentList.Add(tmp2);

            TADocumentObj tmp3 = new TADocumentObj();
            tmp3.DocumentNo = "3";
            tmp3.Creator = "admin";
            tmp3.CreateDate = "02/10/2009";
            taDocumentList.Add(tmp3);

            #endregion Set List

            ctlGridTADocument.DataSource = taDocumentList;
            ctlGridTADocument.DataBind();
        }
        #endregion public void LoadData()

        #region public void SetCombo()
        private void SetCombo()
        {
            //IList<SS.DB.DTO.ValueObject.TranslatedListItem> translateList = new List<SS.DB.DTO.ValueObject.TranslatedListItem>();

            //SS.DB.DTO.ValueObject.TranslatedListItem tmp1 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            //tmp1.ID = UIHelper.ParseShort("1");
            //tmp1.Symbol = "Open";
            //translateList.Add(tmp1);

            //SS.DB.DTO.ValueObject.TranslatedListItem tmp2 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            //tmp2.ID = UIHelper.ParseShort("2");
            //tmp2.Symbol = "Pending";
            //translateList.Add(tmp2);

            //SS.DB.DTO.ValueObject.TranslatedListItem tmp3 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            //tmp3.ID = UIHelper.ParseShort("3");
            //tmp3.Symbol = "Test";
            //translateList.Add(tmp3);

            //ctlDdlDocumentStatus.DataSource = translateList;
            //ctlDdlDocumentStatus.DataTextField = "Symbol";
            //ctlDdlDocumentStatus.DataValueField = "Id";
            //ctlDdlDocumentStatus.DataBind();

            //ctlDdlDocumentStatus.Items.Insert(0, new ListItem("--Please Select--", ""));

        }
        #endregion public void SetCombo()

        #endregion <== Public Function ==>

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region protected void ctlGridTADocument_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridTADocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!IsMultiSelect)
            {
                if (e.CommandName.Equals("Select"))
                {
                    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    short documentId = UIHelper.ParseShort(ctlGridTADocument.DataKeys[rowIndex].Value.ToString());

                    Label ctlLblDocumentNoInGrid = ctlGridTADocument.Rows[rowIndex].FindControl("ctlLblDocumentNo") as Label;
                    Label ctlLblCreatorInGrid = ctlGridTADocument.Rows[rowIndex].FindControl("ctlLblCreator") as Label;
                    Label ctlLblCreateDateInGrid = ctlGridTADocument.Rows[rowIndex].FindControl("ctlLblCreateDate") as Label;

                    TADocumentObj taDocument = new TADocumentObj();

                    taDocument.DocumentNo = ctlLblDocumentNoInGrid.Text;
                    taDocument.Creator = ctlLblCreatorInGrid.Text;
                    taDocument.CreateDate = ctlLblCreateDateInGrid.Text;

                    CallOnObjectLookUpReturn(taDocument);
                    Hide();
                }
            }
        }
        #endregion protected void ctlGridTADocument_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlGridTADocument_DataBound(object sender, EventArgs e)
        protected void ctlGridTADocument_DataBound(object sender, EventArgs e)
        {
            if (IsMultiSelect)
            {
                if (ctlGridTADocument.Rows.Count > 0)
                {
                    RegisterScriptForGridView();
                }
            }
        }
        #endregion protected void ctlGridTADocument_DataBound(object sender, EventArgs e)

        #region private void RegisterScriptForGridView()
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridTADocument.ClientID + "', '" + ctlGridTADocument.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridView()

        #region protected void ctlImgSearch_Click(object sender, ImageClickEventArgs e)
        protected void ctlImgSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadData();
            ctlUpdatePanelTADocumentGridView.Update();
        }
        #endregion protected void ctlImgSearch_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlImgSelect_Click(object sender, ImageClickEventArgs e)
        protected void ctlImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            if (IsMultiSelect)
            {
                IList<TADocumentObj> taDocumentList = new List<TADocumentObj>();

                foreach (GridViewRow row in ctlGridTADocument.Rows)
                {
                    if ((row.RowType == DataControlRowType.DataRow) && ((CheckBox)row.FindControl("ctlSelect")).Checked)
                    {
                        short documentId = UIHelper.ParseShort(ctlGridTADocument.DataKeys[row.RowIndex].Value.ToString());
                        Label ctlLblDocumentNoInGrid = ctlGridTADocument.Rows[row.RowIndex].FindControl("ctlLblDocumentNo") as Label;
                        Label ctlLblCreatorInGrid = ctlGridTADocument.Rows[row.RowIndex].FindControl("ctlLblCreator") as Label;
                        Label ctlLblCreateDateInGrid = ctlGridTADocument.Rows[row.RowIndex].FindControl("ctlLblCreateDate") as Label;

                        TADocumentObj taDocument = new TADocumentObj();

                        taDocument.DocumentNo = ctlLblDocumentNoInGrid.Text;
                        taDocument.Creator = ctlLblCreatorInGrid.Text;
                        taDocument.CreateDate = ctlLblCreateDateInGrid.Text;

                        taDocumentList.Add(taDocument);
                    }
                }

                CallOnObjectLookUpReturn(taDocumentList);
                Hide();
            }
        }
        #endregion protected void ctlImgSelect_Click(object sender, ImageClickEventArgs e)

        #region protected void ctlImgCancel_Click(object sender, ImageClickEventArgs e)
        protected void ctlImgCancel_Click(object sender, ImageClickEventArgs e)
        {
            this.Hide();
        }
        #endregion protected void ctlImgCancel_Click(object sender, ImageClickEventArgs e)
    }
}