using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.DB.BLL;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls;
using SCG.DB.Query;
using SCG.DB.DTO;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class ManageSapInstance : BasePage
    {
        public IDbSapInstanceService DbSapInstanceService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlSapInstanceEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlSapInstanceEditor.Notify_Cancle += new EventHandler(RefreshGridData);
        }
        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlSapInstanceEditor.HidePopUp();
            ctlSapGrid.DataCountAndBind();
            ctlUpdatePanel.Update();

        }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "ManageIO";
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //    ctlSapInstanceEditor.Notify_Ok += new EventHandler(RefreshGridData);
        //    ctlSapInstanceEditor.Notify_Cancle += new EventHandler(RefreshGridData);

        //    if (!Page.IsPostBack)
        //    {

        //    }

        //} 
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlSapGrid.DataCountAndBind();
        }
        public string SapInstanceCode
        {
            get { return sapInstanceCode.Value; }
            set { sapInstanceCode.Value = value; }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        protected void ctlSapGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("SapEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                SapInstanceCode = ctlSapGrid.DataKeys[rowIndex].Values["Code"].ToString();


                ctlSapInstanceEditor.Initialize(FlagEnum.EditFlag, SapInstanceCode);
                ctlSapInstanceEditor.ShowPopUp();

            }
            if (e.CommandName.Equals("SapDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    SapInstanceCode = ctlSapGrid.DataKeys[rowIndex].Value.ToString();
                    DbSapInstance sa = ScgDbQueryProvider.DbSapInstanceQuery.FindByIdentity(SapInstanceCode);
                    DbSapInstanceService.Delete(sa);

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlSapGrid.DataCountAndBind();
                    }
                }

                ctlSapGrid.DataCountAndBind();
                ctlUpdatePanel.Update();

            }


        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            SapInstanceCriteria criteria = GetSuUserCriteria();
            return ScgDbQueryProvider.DbSapInstanceQuery.GetSapInstanceListByCriteria(criteria, startRow, pageSize, sortExpression);


        }
        public int RequestCount()
        {
            SapInstanceCriteria criteria = GetSuUserCriteria();
            return ScgDbQueryProvider.DbSapInstanceQuery.CountSapInstance(criteria);


        }
        public SapInstanceCriteria GetSuUserCriteria()
        {
            SapInstanceCriteria sa = new SapInstanceCriteria();
            sa.AliasName = ctlAliasName.Text;

            sa.MsgServerHost = ctlMsgServerHost.Text;
            return sa;
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            ctlSapInstanceEditor.Initialize(FlagEnum.NewFlag, "");
            ctlSapInstanceEditor.ShowPopUp();
        }
    }
}