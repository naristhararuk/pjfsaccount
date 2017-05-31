using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SS.Standard.UI;
using SCG.eAccounting.SAP.BAPI.Service.Posting;
using System.Collections.Generic;

namespace SCG.eAccounting.Web.UserControls.ViewPost
{
    public partial class ViewPostMessage : BaseUserControl
    {
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        private DataTable GetTable()
        {
            DataTable dtb = new DataTable();
            dtb.Columns.Add("COMCODE");
            dtb.Columns.Add("COMNAME");
            dtb.Columns.Add("DOC_SEQ");
            dtb.Columns.Add("RETURN_TYPE");
            dtb.Columns.Add("MESSAGE");

            return dtb;
        }

        #region public void Show()
        public void Show(IList<BAPISimulateReturn> bapiReturn)
        {
            CallOnObjectLookUpCalling();

            DataTable dtbShow       = GetTable();
            DataTable dtbShowB2C    = GetTable();
            string tmpSEQ = "";

            for (int i = 0; i < bapiReturn.Count; i++)
            {
                if ( bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02" )
                {
                    int intSEQ = 0;
                    int.TryParse(tmpSEQ, out intSEQ);
                    intSEQ++;
                    tmpSEQ = intSEQ.ToString("00");
                }

                for (int j = 0; j < bapiReturn[i].SimulateReturn.Count; j++)
                {
                    

                    if ( bapiReturn[i].DOCSEQ == "B2C01" || bapiReturn[i].DOCSEQ == "B2C02" )
                    {
                        #region Case B2C
                        if (bapiReturn[i].DOCSEQ == "B2C01")
                            tmpSEQ = "01";
                        else if (bapiReturn[i].DOCSEQ == "B2C02")
                            tmpSEQ = "02";

                        DataRow dr = dtbShowB2C.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].SimulateReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].SimulateReturn[j].Type;

                        if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShowB2C.Rows.Add(dr);
                        #endregion Case B2C
                    }
                    else if ( bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02" )
                    {
                        #region Case W2C
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"]       = tmpSEQ;
                        dr["MESSAGE"]       = bapiReturn[i].SimulateReturn[j].Message;
                        dr["RETURN_TYPE"]   = bapiReturn[i].SimulateReturn[j].Type;

                        if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case W2C
                    }
                    else
                    {
                        #region Case ปกติ
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        tmpSEQ        = bapiReturn[i].DOCSEQ;
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = bapiReturn[i].DOCSEQ;
                        dr["MESSAGE"] = bapiReturn[i].SimulateReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].SimulateReturn[j].Type;

                        if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].SimulateReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case ปกติ
                    }
                }
            }

            if (dtbShow.Rows.Count > 0)
            {
                lblComCode.Text = dtbShow.Rows[0]["COMCODE"].ToString();
                lblComName.Text = dtbShow.Rows[0]["COMNAME"].ToString();
            }
            GridViewShow.DataSource = dtbShow;
            GridViewShow.DataBind();


            if (dtbShowB2C.Rows.Count > 0)
            {
                lblComCodeB2C.Text = dtbShowB2C.Rows[0]["COMCODE"].ToString();
                lblComNameB2C.Text = dtbShowB2C.Rows[0]["COMNAME"].ToString();

                GridViewShowB2C.DataSource = dtbShowB2C;
                GridViewShowB2C.DataBind();

                divHeadGridViewShowB2C.Visible = true;
                divGridViewShowB2C.Visible = true;
            }
            else
            {
                divHeadGridViewShowB2C.Visible = false;
                divGridViewShowB2C.Visible = false;
            }
            
            UpdatePanelSearchAccount.Update();

            this.modalPopupMessage.Show();
        }
        public void Show(IList<BAPIPostingReturn> bapiReturn)
        {
            CallOnObjectLookUpCalling();

            DataTable dtbShow = GetTable();
            DataTable dtbShowB2C = GetTable();
            string tmpSEQ = "";

            for (int i = 0; i < bapiReturn.Count; i++)
            {
                if (bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02")
                {
                    int intSEQ = 0;
                    int.TryParse(tmpSEQ, out intSEQ);
                    intSEQ++;
                    tmpSEQ = intSEQ.ToString("00");
                }

                for (int j = 0; j < bapiReturn[i].PostingReturn.Count; j++)
                {


                    if (bapiReturn[i].DOCSEQ == "B2C01" || bapiReturn[i].DOCSEQ == "B2C02")
                    {
                        #region Case B2C
                        if (bapiReturn[i].DOCSEQ == "B2C01")
                            tmpSEQ = "01";
                        else if (bapiReturn[i].DOCSEQ == "B2C02")
                            tmpSEQ = "02";

                        DataRow dr = dtbShowB2C.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].PostingReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].PostingReturn[j].Type;

                        if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShowB2C.Rows.Add(dr);
                        #endregion Case B2C
                    }
                    else if (bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02")
                    {
                        #region Case W2C
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].PostingReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].PostingReturn[j].Type;

                        if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case W2C
                    }
                    else
                    {
                        #region Case ปกติ
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        tmpSEQ = bapiReturn[i].DOCSEQ;
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = bapiReturn[i].DOCSEQ;
                        dr["MESSAGE"] = bapiReturn[i].PostingReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].PostingReturn[j].Type;

                        if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].PostingReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case ปกติ
                    }
                }
            }

            if (dtbShow.Rows.Count > 0)
            {
                lblComCode.Text = dtbShow.Rows[0]["COMCODE"].ToString();
                lblComName.Text = dtbShow.Rows[0]["COMNAME"].ToString();
            }
            GridViewShow.DataSource = dtbShow;
            GridViewShow.DataBind();


            if (dtbShowB2C.Rows.Count > 0)
            {
                lblComCodeB2C.Text = dtbShowB2C.Rows[0]["COMCODE"].ToString();
                lblComNameB2C.Text = dtbShowB2C.Rows[0]["COMNAME"].ToString();

                GridViewShowB2C.DataSource = dtbShowB2C;
                GridViewShowB2C.DataBind();

                divHeadGridViewShowB2C.Visible = true;
                divGridViewShowB2C.Visible = true;
            }
            else
            {
                divHeadGridViewShowB2C.Visible = false;
                divGridViewShowB2C.Visible = false;
            }

            UpdatePanelSearchAccount.Update();

            this.modalPopupMessage.Show();
        }
        public void Show(IList<BAPIApproveReturn> bapiReturn)
        {
            CallOnObjectLookUpCalling();
            DataTable dtbShow = GetTable();
            DataTable dtbShowB2C = GetTable();
            string tmpSEQ = "";

            for (int i = 0; i < bapiReturn.Count; i++)
            {
                if (bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02")
                {
                    int intSEQ = 0;
                    int.TryParse(tmpSEQ, out intSEQ);
                    intSEQ++;
                    tmpSEQ = intSEQ.ToString("00");
                }

                for (int j = 0; j < bapiReturn[i].ApproveReturn.Count; j++)
                {


                    if (bapiReturn[i].DOCSEQ == "B2C01" || bapiReturn[i].DOCSEQ == "B2C02")
                    {
                        #region Case B2C
                        if (bapiReturn[i].DOCSEQ == "B2C01")
                            tmpSEQ = "01";
                        else if (bapiReturn[i].DOCSEQ == "B2C02")
                            tmpSEQ = "02";

                        DataRow dr = dtbShowB2C.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].ApproveReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].ApproveReturn[j].Type;

                        if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShowB2C.Rows.Add(dr);
                        #endregion Case B2C
                    }
                    else if (bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02")
                    {
                        #region Case W2C
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].ApproveReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].ApproveReturn[j].Type;

                        if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case W2C
                    }
                    else
                    {
                        #region Case ปกติ
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        tmpSEQ = bapiReturn[i].DOCSEQ;
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = bapiReturn[i].DOCSEQ;
                        dr["MESSAGE"] = bapiReturn[i].ApproveReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].ApproveReturn[j].Type;

                        if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].ApproveReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case ปกติ
                    }
                }
            }

            if (dtbShow.Rows.Count > 0)
            {
                lblComCode.Text = dtbShow.Rows[0]["COMCODE"].ToString();
                lblComName.Text = dtbShow.Rows[0]["COMNAME"].ToString();
            }
            GridViewShow.DataSource = dtbShow;
            GridViewShow.DataBind();


            if (dtbShowB2C.Rows.Count > 0)
            {
                lblComCodeB2C.Text = dtbShowB2C.Rows[0]["COMCODE"].ToString();
                lblComNameB2C.Text = dtbShowB2C.Rows[0]["COMNAME"].ToString();

                GridViewShowB2C.DataSource = dtbShowB2C;
                GridViewShowB2C.DataBind();

                divHeadGridViewShowB2C.Visible = true;
                divGridViewShowB2C.Visible = true;
            }
            else
            {
                divHeadGridViewShowB2C.Visible = false;
                divGridViewShowB2C.Visible = false;
            }

            UpdatePanelSearchAccount.Update();

            this.modalPopupMessage.Show();
        }

        public void Show(IList<BAPIReverseReturn> bapiReturn)
        {
            CallOnObjectLookUpCalling();
            DataTable dtbShow = GetTable();
            DataTable dtbShowB2C = GetTable();
            string tmpSEQ = "";

            for (int i = 0; i < bapiReturn.Count; i++)
            {
                if (bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02")
                {
                    int intSEQ = 0;
                    int.TryParse(tmpSEQ, out intSEQ);
                    intSEQ++;
                    tmpSEQ = intSEQ.ToString("00");
                }

                for (int j = 0; j < bapiReturn[i].ReverseReturn.Count; j++)
                {


                    if (bapiReturn[i].DOCSEQ == "B2C01" || bapiReturn[i].DOCSEQ == "B2C02")
                    {
                        #region Case B2C
                        if (bapiReturn[i].DOCSEQ == "B2C01")
                            tmpSEQ = "01";
                        else if (bapiReturn[i].DOCSEQ == "B2C02")
                            tmpSEQ = "02";

                        DataRow dr = dtbShowB2C.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].ReverseReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].ReverseReturn[j].Type;

                        if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShowB2C.Rows.Add(dr);
                        #endregion Case B2C
                    }
                    else if (bapiReturn[i].DOCSEQ == "W2C01" || bapiReturn[i].DOCSEQ == "W2C02")
                    {
                        #region Case W2C
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = tmpSEQ;
                        dr["MESSAGE"] = bapiReturn[i].ReverseReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].ReverseReturn[j].Type;

                        if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case W2C
                    }
                    else
                    {
                        #region Case ปกติ
                        DataRow dr = dtbShow.NewRow();
                        dr.BeginEdit();
                        tmpSEQ = bapiReturn[i].DOCSEQ;
                        dr["COMCODE"] = bapiReturn[i].ComCode;
                        dr["COMNAME"] = bapiReturn[i].ComName;

                        dr["DOC_SEQ"] = bapiReturn[i].DOCSEQ;
                        dr["MESSAGE"] = bapiReturn[i].ReverseReturn[j].Message;
                        dr["RETURN_TYPE"] = bapiReturn[i].ReverseReturn[j].Type;

                        if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "S")
                            dr["RETURN_TYPE"] = "Success";
                        else if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "W")
                            dr["RETURN_TYPE"] = "Warning";
                        else if (bapiReturn[i].ReverseReturn[j].Type.ToUpper() == "E")
                            dr["RETURN_TYPE"] = "Error";

                        dr.EndEdit();
                        dtbShow.Rows.Add(dr);
                        #endregion Case ปกติ
                    }
                }
            }

            if (dtbShow.Rows.Count > 0)
            {
                lblComCode.Text = dtbShow.Rows[0]["COMCODE"].ToString();
                lblComName.Text = dtbShow.Rows[0]["COMNAME"].ToString();
            }
            GridViewShow.DataSource = dtbShow;
            GridViewShow.DataBind();


            if (dtbShowB2C.Rows.Count > 0)
            {
                lblComCodeB2C.Text = dtbShowB2C.Rows[0]["COMCODE"].ToString();
                lblComNameB2C.Text = dtbShowB2C.Rows[0]["COMNAME"].ToString();

                GridViewShowB2C.DataSource = dtbShowB2C;
                GridViewShowB2C.DataBind();

                divHeadGridViewShowB2C.Visible = true;
                divGridViewShowB2C.Visible = true;
            }
            else
            {
                divHeadGridViewShowB2C.Visible = false;
                divGridViewShowB2C.Visible = false;
            }

            UpdatePanelSearchAccount.Update();

            this.modalPopupMessage.Show();
        }
        #endregion public void Show(long DocID, DocumentKind DocKind , DocumentLevel DocKind)

        #region protected void btnClose_Click(object sender, EventArgs e)
        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.modalPopupMessage.Hide();
        }
        #endregion protected void btnClose_Click(object sender, EventArgs e)

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            this.modalPopupMessage.Hide();
        }

        #region protected void GridViewShow_RowDataBound(object sender, GridViewRowEventArgs e)
        protected void GridViewShow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[1].Text.Length > 3)
            //{
            //    e.Row.BackColor = System.Drawing.Color.Yellow;
            //    e.Row.ForeColor = System.Drawing.Color.Blue;
            //}
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.Cells[2].Text == "PCADVCL")
            //{
            //    e.Row.BackColor = System.Drawing.Color.Gray;
            //}
        }
        #endregion protected void GridViewShow_RowDataBound(object sender, GridViewRowEventArgs e)
    }
}