using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using System.IO;
using System.Data;

namespace SCG.eAccounting.Web
{
    public partial class GetLog : BasePage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable DTable = new DataTable();
            DTable.Columns.Add(new DataColumn("SEQ"));
            DTable.Columns.Add(new DataColumn("NAME"));
            DTable.Columns.Add(new DataColumn("SIZE"));
            DTable.Columns.Add(new DataColumn("CREDATE"));
            DTable.Columns.Add(new DataColumn("MODDATE"));
            DTable.Columns.Add(new DataColumn("LINK"));

            string pathPicture = Server.MapPath("Logs");
            DirectoryInfo dr = new DirectoryInfo(pathPicture);

            if (dr.Exists == true)
            {
                int intSEQ = 0;
                foreach (FileInfo TmpFl in dr.GetFiles())
                {
                    intSEQ++;
                    long size = TmpFl.Length / 1024;

                    DataRow DRows = DTable.NewRow();
                    DRows["SEQ"]        = intSEQ.ToString();
                    DRows["NAME"]       = TmpFl.Name;
                    DRows["SIZE"]       = size.ToString("###,###,###,###,###,##0") + " KB";
                    DRows["CREDATE"]    = TmpFl.CreationTime;
                    DRows["MODDATE"]    = TmpFl.LastAccessTime;
                    DRows["LINK"]       = "LOG\\" + TmpFl.Name;
                    DTable.Rows.Add(DRows);
                }
            }

            ctlGrdDetail.DataSource = DTable;
            ctlGrdDetail.DataBind();
        }

        protected void ctlGrdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton img = e.Row.Controls[5].FindControl("ctlImgDownload") as ImageButton;
                img.PostBackUrl = "LOGS\\" + e.Row.Cells[1].Text;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" && txtPassword.Text == "")
                Alert("กรุณากรอก UserName และ Password ด้วยครับพี่น้องคร๊าบบบบบ....");
            else if (txtUserName.Text != "softsquare" || txtPassword.Text != "administrator")
                Alert("เสียใจด้วยครับ คุณไม่มีสิทธิเข้า ขอบคุณที่ใช้บริการ....");
            else if (txtUserName.Text == "softsquare" && txtPassword.Text == "administrator")
            {
                divLogin.Visible = false;
                divShow.Visible = true;
                UpdatePanelShow.Update();
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }

        #region private void Alert(string Message)
        private void Alert(string Message)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "DisableDropdown", "alert('" + Message + "');", true);
        }
        #endregion private void Alert(string Message)
    }
}
