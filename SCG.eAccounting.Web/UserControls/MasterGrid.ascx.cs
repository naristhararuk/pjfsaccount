using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.SU.DTO;
using SS.Standard.UI;
using SS.SU.BLL;
using SS.SU.Query;
using System.Text;
using SCG.eAccounting.Web.Helper;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class MasterGrid : BaseUserControl
    {
        //public string QueryMethod { get; set; }
        //public string CountMethod { get; set; }
        //public string InsertMethod { get; set; }
        //public string UpdateMethod { get; set; }
        //public string DeleteMethod { get; set; }

        public string DataKeyNames { get; set; }

        private ISimpleMasterQuery objectQuery;
        public ISimpleMasterQuery ObjectQuery
        {
            set { objectQuery = value; }
            //get { return ViewState["objectQuery"]; }
        }
        private ISimpleMasterService objectService;
        public ISimpleMasterService ObjectService
        {
            set { objectService = value; }
        }
        private ISimpleMaster dto;
        public ISimpleMaster DTO
        {
            set { dto = value; }
            get { return dto; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlMasterGridView.DataKeyNames = DataKeyNames.Split(',');
                ctlMasterGridView.DataCountAndBind();
                ctlUpdatePanelGridView.Update();
            }
        }


        protected void ctlAddNew_Click(object sender, ImageClickEventArgs e)
        {
            //ctlMasterGridView.ShowFooter = true;
            //ctlMasterGridView.DataCountAndBind();
            //ctlUpdatePanelGridView.Update();
        }

        protected void ctlDelete_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in ctlMasterGridView.Rows)
            {
                if ((row.RowType == DataControlRowType.DataRow) && (((CheckBox)row.FindControl("ctlSelect")).Checked))
                {
                    string id = ctlMasterGridView.DataKeys[row.RowIndex].Value.ToString();
                    //Delete
                    //objectService.GetType().GetMethod(DeleteMethod).Invoke(objectService, new object[] { id });
                    objectService.DeleteMasterRecord(id);
                }
            }
            ctlMasterGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        protected void ctlMasterGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                TextBox comment = ctlMasterGridView.FooterRow.FindControl("ctlComment") as TextBox;
                CheckBox active = ctlMasterGridView.FooterRow.FindControl("ctlActive") as CheckBox;
                dto.Comment = comment.Text;
                dto.Active = active.Checked;
                objectService.Add(dto);
                //ctlMasterGridView.ShowFooter = false;
                ctlMasterGridView.DataCountAndBind();
                ctlUpdatePanelGridView.Update();
            }
            if (e.CommandName == "CancelInsert")
            {
                //ctlMasterGridView.ShowFooter = false;
                ctlMasterGridView.DataCountAndBind();
                ctlUpdatePanelGridView.Update();
            }

        }

        protected void ctlMasterGridView_DataBound(object sender, EventArgs e)
        {
            if (ctlMasterGridView.Rows.Count > 0)
            {
                RegisterScriptForGridView();
                ctlDelete.OnClientClick = String.Format("return ConfirmDelete('{0}')", ctlMasterGridView.ClientID);
            }
        }
        protected void ctlMasterGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ctlMasterGridView.EditIndex = e.NewEditIndex;
            ctlMasterGridView.DataCountAndBind();
        }
        protected void ctlMasterGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            e.Cancel = true;
            ctlMasterGridView.EditIndex = -1;
            ctlMasterGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        protected void ctlMasterGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = ctlMasterGridView.DataKeys[e.RowIndex].Value.ToString();
            TextBox code = ctlMasterGridView.FooterRow.FindControl("ctlCode") as TextBox;
            TextBox comment = ctlMasterGridView.Rows[e.RowIndex].FindControl("ctlComment") as TextBox;
            CheckBox active = ctlMasterGridView.Rows[e.RowIndex].FindControl("ctlActive") as CheckBox;
            dto.MenuCode = code.Text;
            dto.Comment = comment.Text;
            dto.Active = active.Checked;
            dto.Menuid = Convert.ToInt16(id);
            //objectService.GetType().GetMethod(UpdateMethod).Invoke(objectService, new object[]{id, comment.Text, active.Checked});
            objectService.Updated(dto);
            ctlMasterGridView.EditIndex = -1;
            ctlMasterGridView.DataCountAndBind();
            ctlUpdatePanelGridView.Update();
        }

        #region Function

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            return objectQuery.GetList(startRow, pageSize, sortExpression);
        }
        public int RequestCount()
        {
            return objectQuery.GetCount();
            //return (int)ObjectQuery.GetType().GetMethod(CountMethod).Invoke(objectQuery, null);
        }
        private void RegisterScriptForGridView()
        {
            StringBuilder script = new StringBuilder();
            script.Append("function validateCheckBox(objChk, objFlag) ");
            script.Append("{ ");
            script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            script.Append("'" + ctlMasterGridView.ClientID + "', '" + ctlMasterGridView.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            script.Append("} ");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }

        #endregion
    }
}