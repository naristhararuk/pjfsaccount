using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using SS.SU.DTO;
using SS.SU.BLL.Implement;
using SS.SU.BLL;
using SS.DB.DTO;
using SCG.eAccounting.Web.UserControls;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class MaintainPersonalLevel : BasePage
    {
        public ISuUserPersonalLevelService SuUserPersonalLevelService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlMaintainPersonalLevelGrid.DataCountAndBind();
            }
            ctlMaintainPersonalLevelEditor.Notify_Ok += new EventHandler(RefreshGridData);
            ctlMaintainPersonalLevelEditor.Notify_Cancle += new EventHandler(RefreshGridData);
        }
        private void RefreshGridData(object sender, EventArgs e)
        {
            ctlMaintainPersonalLevelEditor.HidePopUp();
            ctlMaintainPersonalLevelGrid.DataCountAndBind();
            ctlUpdatePanel.Update();

        }
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
        }
        protected void ctlSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlMaintainPersonalLevelGrid.DataCountAndBind();
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        public string MaintainPersonalLevelCode
        {
            get { return personalLevelCode.Value; }
            set { personalLevelCode.Value = value; }
        }
        protected void ctlMaintainPersonalLevelGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("MaintainPersonalLevelEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                MaintainPersonalLevelCode = ctlMaintainPersonalLevelGrid.DataKeys[rowIndex].Values["PersonalLevel"].ToString();


                ctlMaintainPersonalLevelEditor.Initialize(FlagEnum.EditFlag, MaintainPersonalLevelCode);
                ctlMaintainPersonalLevelEditor.ShowPopUp();

            }
            if (e.CommandName.Equals("MaintainPersonalLevelDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    MaintainPersonalLevelCode = ctlMaintainPersonalLevelGrid.DataKeys[rowIndex].Value.ToString();
                    SuUserPersonalLevel su = QueryProvider.SuUserPersonalLevelQuery.FindByIdentity(MaintainPersonalLevelCode);
                    SuUserPersonalLevelService.Delete(su);

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlMaintainPersonalLevelGrid.DataCountAndBind();
                    }
                }

                ctlMaintainPersonalLevelGrid.DataCountAndBind();
                ctlUpdatePanel.Update();

            }


        }
        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            MaintainPersonalLevelCriteria criteria = GetCriteria();
            return QueryProvider.SuUserPersonalLevelQuery.GetMaintainPersonalLevelListByCriteria(criteria, startRow, pageSize, sortExpression);


        }
        public int RequestCount()
        {
            MaintainPersonalLevelCriteria criteria = GetCriteria();
            return QueryProvider.SuUserPersonalLevelQuery.CountMaintainPersonalLevel(criteria);


        }
        public MaintainPersonalLevelCriteria GetCriteria()
        {
            MaintainPersonalLevelCriteria mp = new MaintainPersonalLevelCriteria();
            mp.PersonalLevel = ctlPersonalLevel.Text;
            mp.Description = ctlDescription.Text;
            return mp;
        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {

            ctlMaintainPersonalLevelEditor.Initialize(FlagEnum.NewFlag, "");
            ctlMaintainPersonalLevelEditor.ShowPopUp();
        }
    }
}