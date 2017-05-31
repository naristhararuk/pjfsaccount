using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Web.UserControls;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.BLL;
using SCG.DB.DTO.ValueObject;

namespace SCG.eAccounting.Web.Forms.SCG.DB.Programs
{
    public partial class PBManage : BasePage
    {

        public IDbPBService DbPBService { get; set; }
        public long PBID
        {
            get { return UIHelper.ParseShort(pb.Value); }
            set { pb.Value = value.ToString(); }
        }

        private void RefreshGridData(object sender, EventArgs e)
        {
            PBEditor.HidePopUp();
            ctlPBGrid.DataCountAndBind();
            ctlPBUpdatePanel.Update();
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            ProgramCode = "PBManage";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            PBEditor.Notify_Ok += new EventHandler(RefreshGridData);
            PBEditor.Notify_Cancle += new EventHandler(RefreshGridData);

            if (!Page.IsPostBack)
            {

            }

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
        protected void PB_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in ctlPBGrid.Rows)
            {
                HiddenField expenseType = row.FindControl("ctlHidePettyCashLimitLabel") as HiddenField;
                Literal ctlPettyCashLimitLabel = row.FindControl("ctlPettyCashLimitLabel") as Literal;
                //string pettyCashLimit=string.Format("{0:($###,###.00)}", expenseType.Value.ToString());
                
                string pettyCashLimit = UIHelper.BindDecimal(expenseType.Value.ToString());
                ctlPettyCashLimitLabel.Text = pettyCashLimit;
            }
        }
        protected void ctlPB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex;
            if (e.CommandName.Equals("PBEdit"))
            {
                rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                PBID = UIHelper.ParseShort(ctlPBGrid.DataKeys[rowIndex].Values["Pbid"].ToString());


                PBEditor.Initialize(FlagEnum.EditFlag, PBID);
                PBEditor.ShowPopUp();

            }
            if (e.CommandName.Equals("PBDelete"))
            {
                try
                {
                    rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
                    PBID = UIHelper.ParseShort(ctlPBGrid.DataKeys[rowIndex].Value.ToString());
                    Dbpb pb = ScgDbQueryProvider.DbPBQuery.FindByIdentity(PBID);
                    DbPBService.DeletePB(pb);
                

                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex.GetBaseException())).Number == 547)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertInUseData",
                            "alert('This data is now in use.');", true);
                        ctlPBGrid.DataCountAndBind();
                    }
                }

                ctlPBGrid.DataCountAndBind();
                ctlPBUpdatePanel.Update();

            }


        }

        public Object RequestData(int startRow, int pageSize, string sortExpression)
        {
            VOPB criteria = GetPBCriteria();
            return ScgDbQueryProvider.DbPBQuery.GetPbList(criteria, startRow, pageSize, sortExpression);
                

        }
        public int RequestCount()
        {
            VOPB criteria = GetPBCriteria();
            int count = ScgDbQueryProvider.DbPBQuery.CountPbByCriteria(criteria);
            return count;

        }
        protected void ctlAdd_Click(object sender, ImageClickEventArgs e)
        {
            
            PBEditor.Initialize(FlagEnum.NewFlag, 0);
            PBEditor.ShowPopUp();


        }
        protected void ctlPBSearch_Click(object sender, ImageClickEventArgs e)
        {
            ctlPBGrid.DataCountAndBind();

        }
        public VOPB GetPBCriteria()
        {
            VOPB pb = new VOPB();
            pb.PBCode = ctlPBCode.Text;
            pb.Description = ctrDescription.Text;

            return pb;
        }

    }
}
