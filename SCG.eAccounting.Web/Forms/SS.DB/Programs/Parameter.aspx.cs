using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.DB.BLL;
using SS.DB.DAL;
using SS.DB.DTO;
using SS.DB.Query;
using SS.Standard.Security;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.DB.DTO.ValueObject;
using SCG.eAccounting.Web.Helper;
using SS.Standard.Utilities;
//using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SS.DB.Programs
{
    public partial class Parameter : BasePage
    {
        #region Define Variable
        public IDbParameterService DbParameterService { get; set; }
        public IDbParameterQuery DbParameterQuery { get; set; }
        public IDbParameterGroupService DbParameterGroupService {get; set;}
        public IDbParameterGroupQuery DbParameterGroupQuery { get; set; }
        #endregion Define Variable

        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlGridParameterGroup.DataCountAndBind();
                UpdatePanelParamaterGroupGridView.Update();

                Session.Remove("groupNo");
                ctlFieldSetDetailGridView.Visible = false;
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region <== Main Program ==>

        #region DbParameterGroup
        #region public Object ctlParameterGroup_RequestData(int startRow, int pageSize, string sortExpression)
        public Object ctlParameterGroup_RequestData(int startRow, int pageSize, string sortExpression)
        {
            return SsDbQueryProvider.DbParameterGroupQuery.GetParameterGroupList(new DbParameterGroup(),startRow, pageSize, sortExpression);
        }
        #endregion public Object ctlParameterGroup_RequestData(int startRow, int pageSize, string sortExpression)

        #region public int ctlParameterGroup_RequestCount()
        public int ctlParameterGroup_RequestCount()
        {
            int count = SsDbQueryProvider.DbParameterGroupQuery.CountByParameterGroupCriteria(new DbParameterGroup());
            return count;
        }
        #endregion public int ctlParameterGroup_RequestCount()

        #region protected void ctlGridParameterGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridParameterGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            short groupNo;
            if (e.CommandName == "Select")
            {
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                groupNo = UIHelper.ParseShort(ctlGridParameterGroup.DataKeys[rowIndex].Value.ToString());

                Session["groupNo"] = groupNo;

                ctlGridParameter.DataCountAndBind();
                UpdatePanelParamaterGridView.Update();
            }
        }
        #endregion protected void ctlGridParameterGroup_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlGridParameterGroup_DataBound(object sender, EventArgs e)
        protected void ctlGridParameterGroup_DataBound(object sender, EventArgs e)
        {
        }
        #endregion protected void ctlGridParameterGroup_DataBound(object sender, EventArgs e)

        #region protected void ctlGridParameterGroup_PageIndexChanged(object sender, EventArgs e)
        protected void ctlGridParameterGroup_PageIndexChanged(object sender, EventArgs e)
        {
        }
        #endregion protected void ctlGridParameterGroup_PageIndexChanged(object sender, EventArgs e)

        #endregion DbParameterGroup

        #region DbParameter
        #region public Object ctlParameter_RequestData(int startRow, int pageSize, string sortExpression)
        public Object ctlParameter_RequestData(int startRow, int pageSize, string sortExpression)
        {
            short groupNo = UIHelper.ParseShort(Session["groupNo"].ToString());

            return SsDbQueryProvider.DbParameterQuery.GetParameterList(groupNo, startRow, pageSize, sortExpression);
        }
        #endregion public Object ctlParameter_RequestData(int startRow, int pageSize, string sortExpression )

        #region public int ctlParameter_RequestCount()
        public int ctlParameter_RequestCount()
        {
            short groupNo = UIHelper.ParseShort(Session["groupNo"].ToString());
            int count = SsDbQueryProvider.DbParameterQuery.CountByParameterCriteria(groupNo);

            ctlFieldSetDetailGridView.Visible = true;

            return count;
        }
        #endregion public int ctlParameter_RequestCount()

        #region protected void ctlGridParameter_RowCommand(object sender, GridViewCommandEventArgs e)
        protected void ctlGridParameter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ParameterEdit")
            {
                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                short Id = UIHelper.ParseShort(ctlGridParameter.DataKeys[rowIndex].Value.ToString());

                ctlGridParameter.EditIndex = rowIndex;
                IList<DbParameter> parameterList = new List<DbParameter>();
              
                DbParameter parameter = DbParameterService.FindByIdentity(Id);

                parameterList.Add(parameter);

                ctlParameterFormView.DataSource = parameterList;
                ctlParameterFormView.PageIndex = 0;

                ctlParameterFormView.ChangeMode(FormViewMode.Edit);
                ctlParameterFormView.DataBind();

                UpdatePanelParameterForm.Update();
                ctlParameterModalPopupExtender.Show();
            }
        }
        #endregion protected void ctlGridParameter_RowCommand(object sender, GridViewCommandEventArgs e)

        #region protected void ctlGridParameter_DataBound(object sender, EventArgs e)
        protected void ctlGridParameter_DataBound(object sender, EventArgs e)
        {
        }
        #endregion protected void ctlGridParameter_DataBound(object sender, EventArgs e)

        #region protected void ctlGridParameter_PageIndexChanged(object sender, EventArgs e)
        protected void ctlGridParameter_PageIndexChanged(object sender, EventArgs e)
        {
        }
        #endregion protected void ctlGridParameter_PageIndexChanged(object sender, EventArgs e)

        #endregion DbParameter

        #endregion <== Main Program ==>

        #region <== Form View ==>

        #region DbParameter
        #region protected void ctlParameterFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        protected void ctlParameterFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                ctlGridParameter.DataCountAndBind();
                ctlParameterModalPopupExtender.Hide();
                UpdatePanelParamaterGridView.Update();
            }
        }
        #endregion protected void ctlParameterFormView_ItemCommand(object sender, FormViewCommandEventArgs e)

        #region protected void ctlParameterFormView_DataBound(object sender, EventArgs e)
        protected void ctlParameterFormView_DataBound(object sender, EventArgs e)
        {
            if (ctlParameterFormView.CurrentMode.Equals(FormViewMode.Edit))
            {
                short groupNo = UIHelper.ParseShort(Session["groupNo"].ToString());

                Label ctlParameterTypeInGrid = ctlParameterFormView.FindControl("ctlParameterType") as Label;

                #region Hidden
                Panel pnText = ctlParameterFormView.FindControl("ctlTextPanel") as Panel;
                Panel pnInteger = ctlParameterFormView.FindControl("ctlIntegerPanel") as Panel;
                Panel pnDate = ctlParameterFormView.FindControl("ctlDatePanel") as Panel;

                string parameterType = ctlParameterTypeInGrid.Text;

                if (parameterType.ToLower().Trim() == "text")
                {
                    pnText.Visible = true;
                    pnInteger.Visible = false;
                    pnDate.Visible = false;
                }
                else if (parameterType.ToLower().Trim() == "integer")
                {
                    pnText.Visible = false;
                    pnInteger.Visible = true;
                    pnDate.Visible = false;
                }
                else if (parameterType.ToLower().Trim() == "date")
                {
                    pnText.Visible = false;
                    pnInteger.Visible = false;
                    pnDate.Visible = true;

                    Label ctlParameterValueDateInGrid = ctlParameterFormView.FindControl("ctlParameterValueDate") as Label;
                    global::SCG.eAccounting.Web.UserControls.Calendar ctlCalendarInGrid = ctlParameterFormView.FindControl("ctlCalendar") as global::SCG.eAccounting.Web.UserControls.Calendar;
                    ctlCalendarInGrid.DateValue = ctlParameterValueDateInGrid.Text;
                }
                else
                {
                    pnText.Visible = true;
                    pnInteger.Visible = false;
                    pnDate.Visible = false;
                }
                #endregion Hidden

                DbParameter parameter = DbParameterService.FindByIdentity(groupNo);
                DbParameterGroup parameterGroup = DbParameterGroupQuery.FindByIdentity(groupNo);

                Label ctlGroupNameInGrid = ctlParameterFormView.FindControl("ctlGroupName") as Label;

                ctlGroupNameInGrid.Text = parameterGroup.GroupName;
            }
        }
        #endregion protected void ctlParameterFormView_DataBound(object sender, EventArgs e)

        #region protected void ctlParameterFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        protected void ctlParameterFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            short Id = UIHelper.ParseShort(ctlParameterFormView.DataKey.Value.ToString());

            DbParameter dbParam = DbParameterService.FindByIdentity(Id);
           
            TextBox ctlParameterValueTextInGrid = ctlParameterFormView.FindControl("ctlParameterValueText") as TextBox;
            TextBox ctlParameterValueIntInGrid = ctlParameterFormView.FindControl("ctlParameterValueInt") as TextBox;

            global::SCG.eAccounting.Web.UserControls.Calendar ctlCalendarInGrid = ctlParameterFormView.FindControl("ctlCalendar") as global::SCG.eAccounting.Web.UserControls.Calendar;

            Label ctlParameterTypeInGrid = ctlParameterFormView.FindControl("ctlParameterType") as Label;

            string parameterType = ctlParameterTypeInGrid.Text;

            if (parameterType.ToLower().Trim() == "text")
                dbParam.ParameterValue = ctlParameterValueTextInGrid.Text.Trim();
            else if (parameterType.ToLower().Trim() == "integer")
            {
                if (!ctlParameterValueIntInGrid.Text.Equals(string.Empty))
                    dbParam.ParameterValue = ctlParameterValueIntInGrid.Text.Trim();
                else
                    dbParam.ParameterValue = "0";
            }
            else if (parameterType.ToLower().Trim() == "date")
                dbParam.ParameterValue = ctlCalendarInGrid.DateValue;
            else
                dbParam.ParameterValue = ctlParameterValueTextInGrid.Text.Trim();

            try
            {
                DbParameterService.UpdateParameter(dbParam);

                e.Cancel = true;
                ctlGridParameter.DataCountAndBind();
                ctlParameterModalPopupExtender.Hide();
                UpdatePanelParamaterGridView.Update();
            }
            catch (ServiceValidationException ex)
            {
                ValidationErrors.MergeErrors(ex.ValidationErrors);
            } 
        }
        #endregion protected void ctlParameterFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)

        #region protected void ctlParameterFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        protected void ctlParameterFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
        #endregion protected void ctlParameterFormView_ModeChanging(object sender, FormViewModeEventArgs e)
        #endregion DbParameter

        #endregion <== Form View ==>

        #region <== Function ==>

        #region DbParameter
        #region private void RegisterScriptForGridViewParameter()
        private void RegisterScriptForGridViewParameter()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlGridParameter.ClientID + "', '" + ctlGridParameter.HeaderRow.FindControl("ctlSelectHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        #endregion private void RegisterScriptForGridViewParameter()
        #endregion DbParameter
                
        #endregion <== Function ==>
    }
} 
