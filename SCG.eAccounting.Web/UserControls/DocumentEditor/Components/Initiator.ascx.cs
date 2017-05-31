using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SCG.eAccounting.Query;
using SCG.eAccounting.DTO;
using SCG.eAccounting.DTO.ValueObject;
using SCG.eAccounting.BLL;
using SS.Standard.UI;
using SS.Standard.Utilities;
using System.Threading;
using SS.SU.DTO;
using SS.SU.Query;
using SS.SU.BLL;
using SCG.eAccounting.Web.Helper;
using System.Data;
using SCG.eAccounting.BLL.Implement;
namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    [Serializable]
    public partial class Initiator : BaseUserControl, IEditorComponent
    {
        #region ====    Author  ====
        //Create By     : Apidesh  Wongsa(Desh)
        //Create Date   : 3-Jan-2009
        //Description   : Insert,Update Initiator on Table DocumentInitiator and query data by DocumentID
        //LastUpdate    : Manipulate data on ViewSate before Update on Database Server
        #endregion

        #region ====    Define Variable ====

        public IDocumentInitiatorQuery DocumentInitiatorQuery { get; set; }
        public IDocumentInitiatorService DocumentInitiatorService { get; set; }
        public ITransactionService TransactionService { get; set; }

        public ISuUserQuery SuUserQuery { get; set; }
        #endregion ==== Define Variable ====

        #region ====    Control Properties  ====
        public Guid TransactionId
        {
            get { return (Guid)ViewState[ViewStateName.TransactionID]; }
            set { ViewState[ViewStateName.TransactionID] = value; }
        }
        public long DocumentID
        {
            get { return UIHelper.ParseLong(ViewState[ViewStateName.DocumentID] == null ? "0" : ViewState["DocumentID"].ToString()); }
            set { ViewState[ViewStateName.DocumentID] = value; }
        }
        public string InitialFlag
        {
            get { return ViewState["InitialFlag"].ToString(); }
            set { ViewState["InitialFlag"] = value; }
        }
        public string Mode
        {
            get { return ctlMode.Text; }
            set { ctlMode.Text = value; }
        }

        public long RequesterID
        {
            get { return Int64.Parse(ViewState["RequesterID"].ToString()); }
            set { ViewState["RequesterID"] = value; }
        }

        public bool IsControlVisible
        {
            get { return (bool)ViewState["IsControlVisible"]; }
            set { ViewState["IsControlVisible"] = value; }
        }
        public bool IsControlEditable
        {
            get { return (bool)ViewState["IsControlEditable"]; }
            set { ViewState["IsControlEditable"] = value; }
        }

        public bool IsEmptyData
        {
            get { return (bool)ViewState["IsEmptyData"]; }
            set { ViewState["IsEmptyData"] = value; }
        }

        public object ControlGroupID { get; set; }

        #endregion ==== Control Properties   ====



        #region ===== Initialize =======
        public void Initialize(Guid txId, long documentID, string initFlag)
        {
            this.TransactionId  = txId;
            this.InitialFlag    = initFlag;

            DataSet ds = TransactionService.GetDS(txId);
            if (ds.Tables["Document"] != null)
            {
                this.DocumentID = UIHelper.ParseLong(ds.Tables["Document"].Rows[0]["DocumentID"].ToString());
            }
            ChangeMode();
            this.BindControl();
            RequesterID = GetRequesterID();
        }
        #endregion ===== Initialize =======

        #region ====    On Page Load   ====
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlInitiatorLookup.OnObjectLookUpCalling += new ObjectLookUpCallingEventHandler(CtlInitiatorLookup_OnObjectLookUpCalling);
            ctlInitiatorLookup.OnObjectLookUpReturn += new ObjectLookUpReturnEventHandler(CtlInitiatorLookup_OnObjectLookUpReturn);
            ctlInitiatorLookup.OnObjectIsHideReturn += new ObjectIsHideReturnEventHandler(CtlInitiatorLookup_OnObjIsHideReturn);
        }
        #endregion====    On Page Load   ====

        #region ==== LookUp Calling ====
        protected void CtlInitiatorLookup_OnObjectLookUpCalling(object sender, ObjectLookUpCallingEventArgs e)
        {
            StringBuilder userFilter = new StringBuilder();
            List<DocumentInitiatorLang> documentInitiatorList = GetInitiators();

            //set filter userid
            UserControls.LOV.SCG.DB.UserProfileLookup lookup = sender as UserControls.LOV.SCG.DB.UserProfileLookup;
            //lookup.RequesterID = RequesterID;
            if (documentInitiatorList != null)
            {
                foreach (DocumentInitiatorLang item in documentInitiatorList)
                {
                    userFilter.AppendFormat("{0},", item.UserID);
                }
            }
            lookup.UserIdNotIn = userFilter.ToString().TrimEnd(',');
        }

        protected void CtlInitiatorLookup_OnObjIsHideReturn(object sender, ObjectIsHideReturnArgs e)
        {
            //bool hideFlag = (bool)e.ObjectReturn;
        }


        protected void CtlInitiatorLookup_OnObjectLookUpReturn(object sender, ObjectLookUpReturnArgs e)
        {
            //Insert New Initiator on ViewSate

            IList<SuUser> list = (IList<SuUser>)e.ObjectReturn;

            List<DocumentInitiatorLang> documentInitiatorList = GetInitiators();

            int countSeq = documentInitiatorList.Count;

            foreach (SuUser item in list)
            {

                int count = (from i in documentInitiatorList
                             where i.UserID == item.Userid
                             select i).Count(); ;
                if (count > 0) continue;

                ++countSeq;
                DocumentInitiatorLang documentInitiatorItem = new DocumentInitiatorLang();
                documentInitiatorItem.DocumentID = DocumentID;
                documentInitiatorItem.Email = item.Email;
                documentInitiatorItem.EmployeeName = item.EmployeeName;
                // documentInitiatorItem.LastName = item.LastName;
                documentInitiatorItem.Seq = UIHelper.ParseShort(countSeq.ToString());
                documentInitiatorItem.UserID = item.Userid;
                documentInitiatorItem.InitiatorType = "1";
                documentInitiatorList.Add(documentInitiatorItem);
            }

            InitiatorReorderList.DataSource = ReorderInitiators(documentInitiatorList);
            InitiatorReorderList.DataBind();
            InitiatorUpdatePanel.Update();

        }
        #endregion

        public void Save()
        {
            List<DocumentInitiatorLang> documentInitiatorList = ReorderInitiators(GetInitiators());

            DataSet ds = TransactionService.GetDS(this.TransactionId);


            // the program below is supposed to put in Service layer
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            foreach (DocumentInitiatorLang item in documentInitiatorList)
            {
                if (String.IsNullOrEmpty(item.InitiatorType))
                {
                    errors.AddError("Provider.Error", new Spring.Validation.ErrorMessage("RequiredInitiatorType"));
                }
            }

            if (!errors.IsEmpty)
            {
                throw new ServiceValidationException(errors);
            }

            foreach (DataRow row in ds.Tables["DocumentInitiator"].Select())  //("DocumentID=" + DocumentID))
                row.Delete();

            DateTime dt = DateTime.Now;
            long UserID = UserAccount.UserID;
            long documentID = DocumentID;
            string progamecode = UserAccount.CurrentProgramCode;

            int seq = 1;
            foreach (DocumentInitiatorLang item in documentInitiatorList)
            {
                // new initiator
                DataRow dr = ds.Tables["DocumentInitiator"].NewRow();
                dr["Seq"] = item.Seq;
                dr["DocumentID"] = item.DocumentID;
                dr["UserID"] = item.UserID;
                dr["InitiatorType"] = item.InitiatorType;
                dr["DoApprove"] = item.DoApprove;
                dr["Active"] = true;
                dr["CreBy"] = UserID;
                dr["CreDate"] = dt;
                dr["UpdBy"] = UserID;
                dr["UpdDate"] = dt;
                dr["UpdPgm"] = progamecode;
                //dr["RowFlag"] = DataRowState.Added.ToString();
                ds.Tables["DocumentInitiator"].Rows.Add(dr);
            }


            InitiatorReorderList.DataSource = documentInitiatorList;
            InitiatorReorderList.DataBind();
            InitiatorUpdatePanel.Update();
        }

        #region ====  private methods ====
        //private void SetReorderListData()
        //{ 

        //}


        private void BindReorderList()
        {
            DataSet ds = TransactionService.GetDS(this.TransactionId);
            DataTable initiatorTable = ds.Tables["DocumentInitiator"];
            //DataRow[] rows = initiatorTable.Select("", "Seq ASC");
            DataRow[] rows = initiatorTable.Select("", "InitiatorType ASC");

            List<DocumentInitiatorLang> items = new List<DocumentInitiatorLang>();

            foreach (DataRow row in rows)
            {
                DocumentInitiatorLang item = new DocumentInitiatorLang();
                item.Seq = Utilities.ParseShort(row["Seq"].ToString());
                item.UserID = Utilities.ParseLong(row["UserID"].ToString());
                item.InitiatorType = row["InitiatorType"].ToString();
                SuUser user = SuUserQuery.FindByIdentity(item.UserID);
                item.EmployeeName = user.EmployeeName;
                item.Email = user.Email;
                item.SMS = user.SMSApproveOrReject;

                items.Add(item);
            }

            if (ReorderInitiators(items).Count > 0)
                IsEmptyData = false;
            else
                IsEmptyData = true;

            InitiatorReorderList.DataSource = ReorderInitiators(items);
            InitiatorReorderList.DataBind();
            this.InitiatorUpdatePanel.Update();
        }

        private void SetControlMode(Boolean IsReadonly)
        {
            //this.btnAddInitiatorAutoItem.Enabled = IsReadonly;
            //this.btnAddInitiatorItem.Enabled = IsReadonly;
            this.btnAddInitiatorAutoItem.Visible = !IsReadonly;
            this.btnAddInitiatorItem.Visible = !IsReadonly;
        }
        #endregion ====  private methods ====

        #region ==== Reorder List Control ====

        protected void InitiatorReorderList_DeleteCommand(object sender, AjaxControlToolkit.ReorderListCommandEventArgs e)
        {
            List<DocumentInitiatorLang> documentInitiatorList = GetInitiators();

            documentInitiatorList.RemoveAt(e.Item.ItemIndex);

            InitiatorReorderList.DataSource = ReorderInitiators(documentInitiatorList);
            InitiatorReorderList.DataBind();
            InitiatorUpdatePanel.Update();

        }

        protected void InitiatorReorderList_ItemReorder(object sender, AjaxControlToolkit.ReorderListItemReorderEventArgs e)
        {
            List<DocumentInitiatorLang> documentInitiatorList = GetInitiators();

            int NewIndex = e.NewIndex + 1;
            int OldIndex = e.OldIndex + 1;
            short oldSeq = UIHelper.ParseShort(OldIndex.ToString());
            short newSeq = UIHelper.ParseShort(NewIndex.ToString());
            documentInitiatorList[e.OldIndex].Seq = newSeq;
            documentInitiatorList[e.NewIndex].Seq = oldSeq;

            InitiatorReorderList.DataSource = ReorderInitiators(documentInitiatorList);
            InitiatorReorderList.DataBind();
            InitiatorUpdatePanel.Update();

        }

        protected void InitiatorReorderList_ItemDataBound(object sender, AjaxControlToolkit.ReorderListItemEventArgs e)
        {
            if ((!string.IsNullOrEmpty(this.Mode)) && (this.Mode.Equals(ModeEnum.Readonly)))
            {
                ((ImageButton)e.Item.FindControl("ImageDeleteButton")).Visible = false;
                ((RadioButtonList)e.Item.FindControl("RadioButtonList1")).Enabled = false;
            }
            else
            {
                ((ImageButton)e.Item.FindControl("ImageDeleteButton")).Visible = true;
                ((RadioButtonList)e.Item.FindControl("RadioButtonList1")).Enabled = true;
            }
        }

        protected void ctlLooupInitiator_Click(object sender, ImageClickEventArgs e)
        {
            //ctlInitiatorLookup.Mode = "Multiple";
            ctlInitiatorLookup.Show();
        }





        #endregion

        #region Public Method
        public void ChangeMode()
        {
            Control currentControl = this;
            while (currentControl != null)
            {
                if (currentControl is IDocumentEditor)
                {
                    this.IsControlVisible = ((IDocumentEditor)currentControl).IsContainVisibleFields(ControlGroupID);
                    this.IsControlEditable = ((IDocumentEditor)currentControl).IsContainEditableFields(ControlGroupID);

                    break;
                }
                else
                {
                    currentControl = currentControl.Parent;
                }
            }

            if (this.InitialFlag.Equals(FlagEnum.NewFlag))
            {
                InitiatorReorderList.Enabled = true;
                InitiatorReorderList.AllowReorder = true;
                SetControlMode(false);
                this.Mode = ModeEnum.ReadWrite;
            }
            else if ((this.InitialFlag.Equals(FlagEnum.EditFlag)))
            {
                if (IsControlEditable)
                {
                    InitiatorReorderList.Enabled = true;
                    InitiatorReorderList.AllowReorder = true;
                    SetControlMode(false);
                    this.Mode = ModeEnum.ReadWrite;
                }
                else
                {
                    this.Mode = ModeEnum.Readonly;
                }
            }
            else
            {
                //InitiatorReorderList.Enabled = false;
                InitiatorReorderList.AllowReorder = false;
                SetControlMode(true);
                this.Mode = ModeEnum.Readonly;
            }



        }
        #endregion

        private InitiatorCriteria CreateCriteria()
        {
            List<DocumentInitiatorLang> documentInitiatorList = new List<DocumentInitiatorLang>();

            InitiatorCriteria FilterCriteria = new InitiatorCriteria();
            FilterCriteria.UserIDFilter = documentInitiatorList;
            return FilterCriteria;
        }
        protected void btnAddInitiatorAutoItem_Click(object sender, ImageClickEventArgs e)
        {

            List<UserFavoriteInitiator> userFavoriteInitiatorList = DocumentInitiatorQuery.FindUserFavoriteInitiatorByUserID(CreateCriteria(), RequesterID).ToList<UserFavoriteInitiator>();

            List<DocumentInitiatorLang> documentInitiatorList = new List<DocumentInitiatorLang>();
            int countSeq = 0;
            foreach (UserFavoriteInitiator item in userFavoriteInitiatorList)
            {

                ++countSeq;
                DocumentInitiatorLang documentInitiatorItem = new DocumentInitiatorLang();
                documentInitiatorItem.DocumentID = DocumentID;
                documentInitiatorItem.Email = item.Email;
                documentInitiatorItem.EmployeeName = item.EmployeeName;
                documentInitiatorItem.Seq = UIHelper.ParseShort(countSeq.ToString());
                documentInitiatorItem.UserID = item.ActorUserID;
                documentInitiatorItem.InitiatorType = "1";
                documentInitiatorList.Add(documentInitiatorItem);
            }


            InitiatorReorderList.DataSource = ReorderInitiators(documentInitiatorList);
            InitiatorReorderList.DataBind();
            InitiatorUpdatePanel.Update();
        }


        public void BindControl()
        {
            this.BindReorderList();
        }

        protected int FindInitiatorTypeIndex(object value)
        {
            if (value == null) return -1;
            string s = value.ToString();
            if (s.Equals("1")) return 0;
            if (s.Equals("2")) return 1;
            return -1;

        }

        protected List<DocumentInitiatorLang> GetInitiators()
        {
            List<DocumentInitiatorLang> documentInitiatorList = new List<DocumentInitiatorLang>();
            foreach (AjaxControlToolkit.ReorderListItem item in InitiatorReorderList.Items)
            {
                if (item.IsAddItem) continue;
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Label seq = (Label)item.FindControl("Label4");
                    Label userId = (Label)item.FindControl("Label5");
                    Label empName = (Label)item.FindControl("Label1");
                    Label email = (Label)item.FindControl("Label2");
                    RadioButtonList initiatorType = (RadioButtonList)item.FindControl("RadioButtonList1");
                    DocumentInitiatorLang lang = new DocumentInitiatorLang();

                    lang.Seq = Int16.Parse(seq.Text);
                    lang.UserID = Int64.Parse(userId.Text);
                    lang.InitiatorType = initiatorType.SelectedValue;
                    lang.EmployeeName = empName.Text;
                    lang.Email = email.Text;
                    lang.DoApprove = false;
                    lang.DocumentID = DocumentID;

                    documentInitiatorList.Add(lang);
                }
            }
            return documentInitiatorList;
        }

        private long GetRequesterID()
        {
            long requesterId = 0;
            DataSet ds = TransactionService.GetDS(this.TransactionId);
            DataTable documentTable = ds.Tables["Document"];
            if (documentTable.Rows.Count > 0)
            {
                DataRow documentRow = documentTable.Rows[0];
                if (!string.IsNullOrEmpty(documentRow["RequesterID"].ToString()))
                    requesterId = UIHelper.ParseLong(documentRow["RequesterID"].ToString());
            }
            return requesterId;
        }

        List<DocumentInitiatorLang> ReorderInitiators(List<DocumentInitiatorLang> documentInitiatorList)
        {
            var x = from j in documentInitiatorList orderby j.InitiatorType ascending, j.Seq select j;
            List<DocumentInitiatorLang> tmp = x.ToList();
            for (short i = 0; i < tmp.Count; ++i)
                tmp[i].Seq = (short)(i + 1);
            return tmp;
        }
    }
}
