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
using SCG.eAccounting.SAP.BAPI.Service;
using SCG.eAccounting.SAP.BAPI.Service.Const;
using System.Collections.Generic;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Query;
using SCG.eAccounting.SAP.Service;

namespace SCG.eAccounting.Web.UserControls.ViewPost
{
    public partial class ViewPostEditDate : BaseUserControl
    {
        #region public string DocKind
        public string DocKind
        {
            get
            {
                if (ViewState["DocKind"] == null)
                    return "";
                else
                    return ViewState["DocKind"].ToString();
            }
            set
            {
                ViewState["DocKind"] = value;
            }
        }
        #endregion public DocumentKind DocKind

        #region public long DocID
        public long DocID
        {
            get
            {
                if (ViewState["DocID"] == null)
                    return 0;
                else
                    return (long)ViewState["DocID"];
            }
            set
            {
                ViewState["DocID"] = value;
            }
        }
        #endregion public long DocID

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlPostingDateForReverse.Value = DateTime.Now.Date;
        }

        #region public void Show()
        public void ShowDate(long DocId,string DocKind)
        {
            this.DocID = DocId;
            this.DocKind = DocKind;

            UpdatePanelSearchAccount.Update();
            this.ModalPopupExtender1ShowMessage.Show();
        }
        #endregion public void Show(long DocID, DocumentKind DocKind , DocumentLevel DocKind)

        //public void HideDate()
        //{
        //    NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
        //    this.ModalPopupExtender1ShowMessage.Hide();
        //}

        #region private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()
        private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()
        {
            SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService PostService;
            if (this.DocKind == DocumentKind.Advance.ToString())
                PostService = new AdvancePostingService();
            else if (this.DocKind == DocumentKind.Remittance.ToString())
                PostService = new RemittancePostingService();
            else if (this.DocKind == DocumentKind.Expense.ToString())
                PostService = new ExpensePostingService();
            else if (this.DocKind == DocumentKind.ExpenseRemittance.ToString())
                PostService = new ExpenseRemittancePostingService();
            else
                PostService = new AdvancePostingService();

            return PostService;
        }
        #endregion private SCG.eAccounting.SAP.BAPI.Service.Posting.PostingService GetPostingService()

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ctlPostingDateForReverse.Value != null)
                {
                    IList<Bapiache09> listBAPIACHE09 = BapiQueryProvider.Bapiache09Query.FindByDocID(this.DocID, this.DocKind);
                    for (int i = 0; i < listBAPIACHE09.Count; i++)
                    {
                        listBAPIACHE09[i].ReverseDate = ctlPostingDateForReverse.Value.Value.Year.ToString("0000") + ctlPostingDateForReverse.Value.Value.Month.ToString("00") + ctlPostingDateForReverse.Value.Value.Day.ToString("00");
                        BapiServiceProvider.Bapiache09Service.SaveOrUpdate(listBAPIACHE09[i]);           
                    }
                }
                NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.OK, null));
                this.ModalPopupExtender1ShowMessage.Hide();
            }
            catch
            { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            NotifyPopUpReturn(new PopUpReturnArgs(PopUpReturnType.Cancel, null));
            this.ModalPopupExtender1ShowMessage.Hide();
        }
    }
}