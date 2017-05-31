using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Data;
using SS.SU.DTO;
using SS.SU.Helper;
using SS.DB.Query;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SS.Standard.UI
{
    /// <summary>
    /// BulkEditGridView allows users to edit multiple rows of a gridview at once, and have them
    /// all saved.
    /// </summary>
    [
    Browsable(true),
    DefaultEvent("SelectedIndexChanged"),
    Designer("System.Web.UI.Design.WebControls.GridViewDesigner,System.Drawing.Design,System.Windows.Forms.Design, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
    ControlValueProperty("SelectedValue")
    ]
    public class BaseGridView : System.Web.UI.WebControls.GridView
    {
        private GridViewRow _headerRow;
        private GridViewRow _footerRow;

        public new GridViewRow HeaderRow
        {
            get { return base.HeaderRow ?? _headerRow; }
        }

        public new GridViewRow FooterRow
        {
            get { return base.FooterRow ?? _footerRow; }
        }



        //key for the RowInserting event handler list
        public static readonly object RowInsertingEvent = new object();
        protected DropDownList ctlRecordsPerPage = new DropDownList();

        private List<int> dirtyRows = new List<int>();
        private List<int> newRows = new List<int>();

        private TableItemStyle insertRowStyle;
        private bool isSaving;
        private bool continueUpdating = true;
        private bool buttonCausesValidation = true;

        public delegate Object RequestDataHandler(int startRow, int pageSize, string sortExpression);
        public event RequestDataHandler RequestData;
        public delegate int RequestCountHandler();
        public event RequestCountHandler RequestCount;

        public bool ClearSortExpression { get; set; }



        #region public BaseGridView()
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseGridView()
        {

        }
        #endregion public BaseGridView()

        #region <== OVERRIDE METHOD ==>

        #region protected override void OnInit(EventArgs e)
        protected override void OnInit(EventArgs e)
        {
            if (!DesignMode)
            {
                ctlRecordsPerPage.AutoPostBack = true;
                ctlRecordsPerPage.ID = "ctlRecordsPerPage";
                ctlRecordsPerPage.DataSource = RecordsPerPageCollection();
                ctlRecordsPerPage.DataTextField = "TEXT";
                ctlRecordsPerPage.DataValueField = "VALUE";

                base.OnInit(e);

                ctlRecordsPerPage.SelectedIndexChanged += new System.EventHandler(this.ctlRecordsPerPage_SelectedIndexChanged);
                RowUpdating += new GridViewUpdateEventHandler(BulkEditGridView_RowUpdating);
                RowDataBound += new GridViewRowEventHandler(BulkEditGridView_RowDataBound);
            }
        }
        #endregion protected override void OnInit(EventArgs e)

        #region protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
        /// <summary>
        /// Modifies the creation of the row to set all rows as editable.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="dataSourceIndex"></param>
        /// <param name="rowType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            if (ReadOnly)
            {
                return base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
            }
            else
            {
                return base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState | DataControlRowState.Edit);
            }

        }
        #endregion protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)

        #region protected override void OnRowCreated(GridViewRowEventArgs e)
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (!DesignMode)
            {
                base.OnRowCreated(e);
            }
        }
        #endregion protected override void OnRowCreated(GridViewRowEventArgs e)

        #region protected override void InitializeRow(GridViewRow row, DataControlField[] fields)
        /// <summary>
        /// Adds event handlers to controls in all the editable cells.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fields"></param>
        protected override void InitializeRow(GridViewRow row, DataControlField[] fields)
        {
            base.InitializeRow(row, fields);
            foreach (TableCell cell in row.Cells)
            {
                if (cell.Controls.Count > 0)
                {
                    AddChangedHandlers(cell.Controls);
                }
            }
        }
        #endregion protected override void InitializeRow(GridViewRow row, DataControlField[] fields)

        #region protected override void PerformSelect()
        /// <summary>
        /// We need to surpress data binding while we're uploading, or else values will not persist.
        /// </summary>
        protected override void PerformSelect()
        {
            if (!isSaving) base.PerformSelect();
        }
        #endregion protected override void PerformSelect()

        #region protected override void OnRowUpdated(GridViewUpdatedEventArgs e)
        protected override void OnRowUpdated(GridViewUpdatedEventArgs e)
        {
            base.OnRowUpdated(e);
            continueUpdating = null == e.Exception || (e.ExceptionHandled && ContinueOnError);
        }
        #endregion protected override void OnRowUpdated(GridViewUpdatedEventArgs e)

        #region protected override void OnLoad(EventArgs e)
        /// <summary>
        /// Attaches an eventhandler to the onclick method of the save button.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Attach an event handler to the save button.
            if (false == string.IsNullOrEmpty(this.SaveButtonID))
            {
                Control btn = RecursiveFindControl(this.NamingContainer, this.SaveButtonID);
                if (null != btn)
                {
                    IButtonControl buttonControl = btn as IButtonControl;

                    if (buttonControl != null)
                    {
                        buttonControl.Click += new EventHandler(SaveClicked);
                        buttonCausesValidation = buttonControl.CausesValidation;
                    }
                }
            }
        }
        #endregion protected override void OnLoad(EventArgs e)

        #region protected override void OnPagePreLoad(object sender, EventArgs e)
        /// <summary>
        /// We have to recreate our insert row so we can load the postback info from it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPagePreLoad(object sender, EventArgs e)
        {
            base.OnPagePreLoad(sender, e);

            if (this.EnableInsert && this.Page.IsPostBack)
            {
                for (int i = 0; i < this.InsertRowCount; i++)
                    this.CreateInsertRow();
            }
        }
        #endregion protected override void OnPagePreLoad(object sender, EventArgs e)

        #region protected override void OnDataBound(EventArgs e)
        /// <summary>
        /// After the controls are databound, add a row to the end.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataBound(EventArgs e)
        {
            if (this.EnableInsert)
            {
                for (int i = 0; i < this.InsertRowCount; i++)
                    this.CreateInsertRow();
            }

            base.OnDataBound(e);
        }
        #endregion protected override void OnDataBound(EventArgs e)


        #region protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            pagedDataSource.AllowPaging = true;
            pagedDataSource.AllowCustomPaging = true;
            pagedDataSource.VirtualCount = RecordCount;
            pagedDataSource.CurrentPageIndex = CustomPageIndex;
            base.InitializePager(row, columnSpan, pagedDataSource);

            Table pagerTable = (Table)row.Cells[0].Controls[0];
            pagerTable.Style["float"] = "left";

            if (ShowPageSizeDropDownList)
                CreateRecordsPerPage(row);
        }
        #endregion protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)

        #region public override int PageCount
        public override int PageCount
        {
            get
            {
                int pageCount = base.PageCount;
                if (pageCount == 1 && ShowPageSizeDropDownList)
                {
                    System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
                    if (sf.GetMethod().Name == "CreateChildControls" && sf.GetMethod().ReflectedType == typeof(GridView))
                    {
                        pageCount++;
                    }
                }
                return pageCount;
            }
        }
        #endregion public override int PageCount

        #region protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            CustomPageIndex = e.NewPageIndex;
            this.BindGrid();
        }
        #endregion protected override void OnPageIndexChanging(GridViewPageEventArgs e)

        #region protected override void OnSorting(GridViewSortEventArgs e)
        protected override void OnSorting(GridViewSortEventArgs e)
        {
            if (!String.IsNullOrEmpty(CustomSortExpression) && e.SortExpression == CustomSortExpression)
            {
                if (CustomSortDirection == SortDirection.Ascending)
                    CustomSortDirection = SortDirection.Descending;
                else
                    CustomSortDirection = SortDirection.Ascending;
            }
            CustomSortExpression = e.SortExpression;

            this.BindGrid();
        }
        #endregion protected override void OnSorting(GridViewSortEventArgs e)

        #region protected override void DataBind(bool raiseOnDataBinding)
        protected override void DataBind(bool raiseOnDataBinding)
        {
            //base.DataBind(raiseOnDataBinding);
            DataCountAndBind();
        }
        #endregion protected override void DataBind(bool raiseOnDataBinding)

        #region protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            base.CreateChildControls(dataSource, dataBinding);
            PagedDataSource data = new PagedDataSource();
            data.DataSource = dataSource;
            int rows = data.DataSourceCount;
            //  no data rows created, create empty table if enabled
            if (rows == 0 && (this.ShowFooterWhenEmpty || this.ShowHeaderWhenEmpty))
            {
                //  create the table
                Table table = this.CreateChildTable();

                DataControlField[] fields;
                if (this.AutoGenerateColumns)
                {
                    PagedDataSource source = new PagedDataSource();
                    source.DataSource = dataSource;

                    System.Collections.ICollection autoGeneratedColumns = this.CreateColumns(source, true);
                    fields = new DataControlField[autoGeneratedColumns.Count];
                    autoGeneratedColumns.CopyTo(fields, 0);
                }
                else
                {
                    fields = new DataControlField[this.Columns.Count];
                    this.Columns.CopyTo(fields, 0);
                }

                if (this.ShowHeaderWhenEmpty)
                {
                    //  create a new header row
                    _headerRow = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    this.InitializeRow(_headerRow, fields);

                    //  add the header row to the table
                    table.Rows.Add(_headerRow);
                }

                //  create the empty row
                GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                TableCell cell = new TableCell();
                cell.ColumnSpan = fields.Length;
                cell.Width = Unit.Percentage(100);
                cell.HorizontalAlign = HorizontalAlign.Center;
                //  respect the precedence order if both EmptyDataTemplate
                //  and EmptyDataText are both supplied ...
                //if (!string.IsNullOrEmpty(this.EmptyDataText))
                if (!MsgDataNotFound().Equals(""))
                {
                    //cell.Controls.Add(new LiteralControl(EmptyDataText));

                    Label lblNodata = new Label();
                    //lblNodata.SkinID    = "SkCtlLabelNodataNew";
                    lblNodata.Font.Bold = true;
                    lblNodata.Font.Name = "Tahoma";
                    lblNodata.Font.Size = 10;
                    lblNodata.ForeColor = System.Drawing.Color.Red;
                    lblNodata.Text = MsgDataNotFound();
                    cell.Controls.Add(lblNodata);
                }
                else if (this.EmptyDataTemplate != null)
                {
                    this.EmptyDataTemplate.InstantiateIn(cell);
                }

                emptyRow.Cells.Add(cell);
                table.Rows.Add(emptyRow);

                if (this.ShowFooterWhenEmpty)
                {
                    //  create footer row
                    _footerRow = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);
                    this.InitializeRow(_footerRow, fields);

                    //  add the footer to the table
                    table.Rows.Add(_footerRow);
                }

                this.Controls.Clear();
                this.Controls.Add(table);
            }

            return rows;
        }
        #endregion protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)

        #endregion <== OVERRIDE METHOD ==>

        #region <== VIRTUAL METHOD ==>

        #region protected virtual void OnRowInserting(GridViewInsertEventArgs args)
        /// <summary>
        /// Fires the <see cref="RowInserting"/> event.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnRowInserting(GridViewInsertEventArgs args)
        {
            Delegate handler = this.Events[RowInsertingEvent];
            if (null != handler)
                handler.DynamicInvoke(this, args);
        }
        #endregion protected virtual void OnRowInserting(GridViewInsertEventArgs args)

        #region protected virtual void CreateInsertRow()
        /// <summary>
        /// Creates the insert row and adds it to the inner table.
        /// </summary>
        protected virtual void CreateInsertRow()
        {
            GridViewRow row = this.CreateRow(this.Rows.Count, -1, DataControlRowType.DataRow, DataControlRowState.Insert);

            DataControlField[] fields = new DataControlField[this.Columns.Count];
            this.Columns.CopyTo(fields, 0);

            row.ApplyStyle(this.insertRowStyle);

            this.InitializeRow(row, fields);

            //Creates header row for empty data.
            if (this.Rows.Count == 0)
            {
                this.Controls.Add(new Table());
                GridViewRow header = this.CreateRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                this.InitializeRow(header, fields);
                //this.CreateChildTable();
                this.InnerTable.Rows.Add(header);

            }

            int index = this.InnerTable.Rows.Count - (this.ShowFooter ? 1 : 0);
            this.InnerTable.Rows.AddAt(index, row);
        }
        #endregion protected virtual void CreateInsertRow()


        #region protected virtual void CreateRecordsPerPage(GridViewRow row)
        protected virtual void CreateRecordsPerPage(GridViewRow row)
        {
            if (!DesignMode)
            {
                Panel panel = new Panel();
                panel.Style["float"] = "right";
                panel.Controls.Add(ctlRecordsPerPage);

                row.Cells[0].Controls.Add(panel);
            }
        }
        #endregion protected virtual void CreateRecordsPerPage(GridViewRow row)

        #endregion <== VIRTUAL METHOD ==>

        #region <== METHOD ==>

        #region public List<GridViewRow> DirtyRows
        /// <summary>
        /// Gets a list of modified rows.
        /// </summary>
        public List<GridViewRow> DirtyRows
        {
            get
            {
                List<GridViewRow> drs = new List<GridViewRow>();
                foreach (int rowIndex in dirtyRows)
                {
                    drs.Add(this.Rows[rowIndex]);
                }

                return drs;
            }
        }
        #endregion public List<GridViewRow> DirtyRows

        #region void BulkEditGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        /// <summary>
        /// Handle the RowDataBound event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This method will save the current values for use later in populating the
        /// OldValues collection of the RowUpdating event.</remarks>
        void BulkEditGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow || false == SaveOldValues || ReadOnly)
            {
                return;
            }

            OrderedDictionary valList = new OrderedDictionary();
            ExtractRowValues(valList, e.Row, false, false);

            Dictionary<object, OrderedDictionary> oldValues = ViewState["oValues"] as Dictionary<object, OrderedDictionary>;
            if (oldValues == null)
            {
                oldValues = new Dictionary<object, OrderedDictionary>();
                ViewState["oValues"] = oldValues;
            }

            if (oldValues.ContainsKey(e.Row.RowIndex)) oldValues[e.Row.RowIndex] = valList;
            else oldValues.Add(e.Row.RowIndex, valList);
        }
        #endregion void BulkEditGridView_RowDataBound(object sender, GridViewRowEventArgs e)

        #region void BulkEditGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        /// <summary>
        /// Handle the row updating event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>This method will be responsible for populating the OldValues collection
        /// of the EventArgs.</remarks>
        void BulkEditGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (SaveOldValues || ReadOnly)
            {
                Dictionary<object, OrderedDictionary> oldValues = ViewState["oValues"] as Dictionary<object, OrderedDictionary>;
                if (oldValues != null && oldValues.ContainsKey(e.RowIndex))
                {
                    foreach (DictionaryEntry entry in oldValues[e.RowIndex])
                    {
                        if (e.OldValues.Contains(entry.Key)) e.OldValues[entry.Key] = entry.Value;
                        else e.OldValues.Add(entry.Key, entry.Value);
                    }
                }
            }
        }
        #endregion void BulkEditGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)

        #region private void AddChangedHandlers(ControlCollection controls)
        /// <summary>
        /// Adds an event handler to editable controls.
        /// </summary>
        /// <param name="controls"></param>
        private void AddChangedHandlers(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                //Changed the control checks to use interfaces, so that it's more robust.
                if (ctrl is IEditableTextControl)
                {
                    // Drop-down lists, and any list control, implement the IEditableTextControl interface
                    ((IEditableTextControl)ctrl).TextChanged += new EventHandler(this.HandleRowChanged);
                }
                else if (ctrl is ICheckBoxControl)
                {
                    ((ICheckBoxControl)ctrl).CheckedChanged += new EventHandler(this.HandleRowChanged);
                }
                else if (ctrl is HtmlInputText)
                {
                    //Added for BUG#69
                    ((HtmlInputText)ctrl).ServerChange += new EventHandler(this.HandleRowChanged);
                }
                ////could add recursion if we are missing some controls.
                //else if (ctrl.Controls.Count > 0 && !(ctrl is INamingContainer) )
                //{
                //    AddChangedHandlers(ctrl.Controls);
                //}
            }
        }
        #endregion private void AddChangedHandlers(ControlCollection controls)

        #region void HandleRowChanged(object sender, EventArgs args)
        /// <summary>
        /// This gets called when a row is changed.  Store the id of the row and wait to update
        /// until save is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void HandleRowChanged(object sender, EventArgs args)
        {
            GridViewRow row = ((Control)sender).NamingContainer as GridViewRow;
            if (null != row)
            {
                if (0 != (row.RowState & DataControlRowState.Insert))
                {
                    int altRowIndex = this.InnerTable.Rows.GetRowIndex(row);
                    if (false == newRows.Contains(altRowIndex))
                        newRows.Add(altRowIndex);
                }
                else
                {
                    if (false == dirtyRows.Contains(row.RowIndex))
                        dirtyRows.Add(row.RowIndex);
                }
            }

        }
        #endregion void HandleRowChanged(object sender, EventArgs args)

        #region private Control RecursiveFindControl(Control namingcontainer, string controlName)
        /// <summary>
        /// Looks for a control recursively up the control tree.  We need this because Page.FindControl
        /// does not find the control if we are inside a masterpage content section.
        /// </summary>
        /// <param name="namingcontainer"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        private Control RecursiveFindControl(Control namingcontainer, string controlName)
        {
            Control c = namingcontainer.FindControl(controlName);

            if (c != null)
                return c;

            if (namingcontainer.NamingContainer != null)
                return RecursiveFindControl(namingcontainer.NamingContainer, controlName);

            return null;
        }
        #endregion private Control RecursiveFindControl(Control namingcontainer, string controlName)

        #region private void SaveClicked(object sender, EventArgs e)
        /// <summary>
        /// Handles the save event, and calls the save method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveClicked(object sender, EventArgs e)
        {
            this.Save();
            this.DataBind();
        }
        #endregion private void SaveClicked(object sender, EventArgs e)

        #region public void Save()
        /// <summary>
        /// Saves any modified rows.  This is called automatically if the SaveButtonId is set.
        /// </summary>
        public void Save()
        {
            isSaving = true;	// used to surpress databinding while updating

            try
            {
                foreach (int row in dirtyRows)
                {
                    // Save the current row
                    this.UpdateRow(row, buttonCausesValidation);
                    if (!continueUpdating) break;
                }

                // We should not even try to insert rows if continueUpdating is false
                if (continueUpdating)
                {
                    foreach (int row in newRows)
                    {
                        //Make the datasource save a new row.
                        this.InsertRow(row, buttonCausesValidation);
                    }
                }
            }
            finally
            {
                dirtyRows.Clear();
                newRows.Clear();
                isSaving = false;
                continueUpdating = true;
            }
        }
        #endregion public void Save()

        #region private void InsertRow(int rowIndex, bool causesValidation)
        /// <summary>
        /// Prepares the <see cref="RowInserting"/> event and calls insert on the DataSource.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="causesValidation"></param>
        private void InsertRow(int rowIndex, bool causesValidation)
        {
            GridViewRow row = null;

            if ((!causesValidation || (this.Page == null)) || this.Page.IsValid)
            {
                DataSourceView dsv = null;
                bool useDataSource = base.IsBoundUsingDataSourceID;
                if (useDataSource)
                {
                    dsv = this.GetData();
                    if (dsv == null)
                    {
                        throw new HttpException("DataSource Returned Null View");
                    }
                }
                GridViewInsertEventArgs args = new GridViewInsertEventArgs(rowIndex);
                if (useDataSource)
                {
                    if ((row == null) && (this.InnerTable.Rows.Count > rowIndex))
                    {
                        row = this.InnerTable.Rows[rowIndex] as GridViewRow;
                    }
                    if (row != null)
                    {
                        this.ExtractRowValues(args.NewValues, row, true, true);
                    }
                }

                this.OnRowInserting(args);

                if (!args.Cancel && useDataSource)
                {
                    dsv.Insert(args.NewValues, new DataSourceViewOperationCallback(DataSourceViewInsertCallback));
                }
            }
        }
        #endregion private void InsertRow(int rowIndex, bool causesValidation)

        #region private bool DataSourceViewInsertCallback(int i, Exception ex)
        /// <summary>
        /// Callback for the datasource's insert command.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool DataSourceViewInsertCallback(int i, Exception ex)
        {
            if (null != ex)
            {
                throw ex;
            }

            return true;
        }
        #endregion private bool DataSourceViewInsertCallback(int i, Exception ex)

        #region public event GridViewInsertEventHandler RowInserting
        /// <summary>
        /// Event fires when new row has been edited, and save is clicked.
        /// </summary>
        public event GridViewInsertEventHandler RowInserting
        {
            add
            {
                this.Events.AddHandler(RowInsertingEvent, value);
            }
            remove
            {
                this.Events.RemoveHandler(RowInsertingEvent, value);
            }
        }
        #endregion public event GridViewInsertEventHandler RowInserting

        #region protected Table InnerTable
        /// <summary>
        /// Access to the GridView's inner table.
        /// </summary>
        protected Table InnerTable
        {
            get
            {
                if (false == this.HasControls())
                    return null;

                return (Table)this.Controls[0];
            }
        }
        #endregion protected Table InnerTable

        #region public void DataCountAndBind()
        public void DataCountAndBind()
        {
            if (RequestCount != null)
                RecordCount = RequestCount();
            else
                RecordCount = 0;

            int pageCount = 0;
            if (PageSize != 0)
            {
                PageSize = (PageSize >= 20 ? PageSize : 20);
                pageCount = RecordCount / PageSize;
            }

            if (CustomPageIndex != pageCount - 1 && CustomPageIndex > pageCount)
                CustomPageIndex = 0;

            this.BindGrid();
        }
        //public void DataCountAndBind(bool isReBindGrid)
        //{
        //    CustomPageIndex = 0;
        //    if (RequestCount != null)
        //        RecordCount = RequestCount();
        //    else
        //        RecordCount = 0;
        //    this.BindGrid();

        //}
        #endregion public void DataCountAndBind()

        #region protected void BindGrid()
        protected void BindGrid()
        {
            this.PageSize = RecordsPerPage;

            if (ClearSortExpression)
            {
                CustomSortExpression = string.Empty;
            }

            if (RequestData != null)
            {
                string sort = "";
                if (!string.IsNullOrEmpty(CustomSortExpression))
                    sort = CustomSortExpression + " " + (CustomSortDirection == SortDirection.Ascending ? "ASC" : "DESC");
                this.DataSource = RequestData(CustomPageIndex * this.PageSize, this.PageSize, sort);
            }

            this.DataBind();

            if (ShowPageSizeDropDownList)
            {
                try
                {
                    ctlRecordsPerPage.SelectedValue = Convert.ToString(RecordsPerPage);
                }
                catch
                {
                    ctlRecordsPerPage.SelectedIndex = 0;
                }
            }
        }
        #endregion protected void BindGrid()


        #region private void SetCookieRecordPerPage(int RecordPage)
        private void SetCookieRecordPerPage(int RecordPage)
        {
            HttpCookie cookieOld = System.Web.HttpContext.Current.Request.Cookies[UserAccountID() + "_" + CookieName + "_PageSize"];
            if (cookieOld != null)
                System.Web.HttpContext.Current.Request.Cookies.Remove(UserAccountID() + "_" + CookieName + "_PageSize");

            HttpCookie cookie = new HttpCookie(UserAccountID() + "_" + CookieName + "_PageSize");
            cookie.Value = RecordPage.ToString();
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion private void SetCookieRecordPerPage(int RecordPage)

        #region private int GetCookieRecordPerPage()
        private int GetCookieRecordPerPage()
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[UserAccountID() + "_" + CookieName + "_PageSize"];
            if (null == cookie)
            {
                SetCookieRecordPerPage(PageSize);   //return 20;
                cookie = System.Web.HttpContext.Current.Request.Cookies[UserAccountID() + "_" + CookieName + "_PageSize"];
            }
            //else
            return int.Parse(cookie.Value.ToString()) >= 20 ? int.Parse(cookie.Value.ToString()) : 20;
        }
        #endregion private int GetCookieRecordPerPage()

        #region private string UserAccountID()
        private string UserAccountID()
        {
            try
            {
                UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                return user.UserID.ToString();
            }
            catch
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                System.Web.HttpContext.Current.Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
                
                return "0";
            }
        }
        #endregion private string UserAccountID()

        #region private string CurrentUserLanguageID()
        private string CurrentUserLanguageID()
        {
            try
            {
                UserSession user = (UserSession)HttpContext.Current.Session[SessionEnum.WebSession.UserProfiles.ToString()];
                return user.CurrentUserLanguageID.ToString();
            }
            catch
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
                {
                    url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
                }
                System.Web.HttpContext.Current.Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
                
                return "1";
            }
        }
        #endregion private string CurrentUserLanguageID()

        #region private DataTable RecordsPerPageCollection()
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets RecordsPerPageCollection.")
        ]
        private DataTable RecordsPerPageCollection()
        {
            #region Not Found In Session
            if (HttpContext.Current.Session["RecordsPerPageCollection"] == null)
            {

                DataTable dtbDropDown = new DataTable();
                dtbDropDown.Columns.Add("TEXT");
                dtbDropDown.Columns.Add("VALUE");

                //comment by lerm+ string strValueDropDownList = SsDbQueryProvider.DbParameterQuery.getParameterByGroupNo_SeqNo("6", "1");
                string strValueDropDownList = ParameterServices.DropDownListCount;
                if (!strValueDropDownList.Equals(""))
                {
                    #region Case Found In Database
                    string[] strValue = strValueDropDownList.Split('/');
                    for (int i = 0; i < strValue.Length; i++)
                    {
                        DataRow dr = dtbDropDown.NewRow();
                        dr["TEXT"] = strValue[i].ToString();
                        dr["VALUE"] = strValue[i].ToString();
                        dtbDropDown.Rows.Add(dr);
                    }
                    #endregion Case Found In Database
                }
                else
                {
                    #region Case Not Found In Database
                    for (int i = 10; i <= 100; i = i + 10)
                    {
                        DataRow dr = dtbDropDown.NewRow();
                        dr["TEXT"] = i.ToString();
                        dr["VALUE"] = i.ToString();
                        dtbDropDown.Rows.Add(dr);
                    }
                    #endregion Case Not Found In Database
                }

                HttpContext.Current.Session["RecordsPerPageCollection"] = dtbDropDown;
            }
            #endregion Not Found In Session

            return (DataTable)HttpContext.Current.Session["RecordsPerPageCollection"];
        }
        #endregion private DataTable RecordsPerPageCollection()

        #region private string MsgDataNotFound()
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets GetMsgDataNotFound.")
        ]
        private string MsgDataNotFound()
        {
            ArrayList alLang = new ArrayList();
            if (this.ShowMsgDataNotFound)
            {
                #region Not Found In Session
                if (HttpContext.Current.Session["MsgDataNotFound"] == null)
                {
                    string strValueLang = ParameterServices.MsgDataNotFound;
                    if (!strValueLang.Equals(""))
                    {
                        #region Case Found In Database
                        string[] strValue = strValueLang.Split('/');
                        for (int i = 0; i < strValue.Length; i++)
                            alLang.Add(strValue[i].ToString());
                        #endregion Case Found In Database
                    }
                    else
                    {
                        #region Case Not Found In Database
                        alLang.Add("ไม่พบข้อมูล");
                        alLang.Add("No data found.");
                        #endregion Case Not Found In Database
                    }

                    HttpContext.Current.Session["MsgDataNotFound"] = alLang;
                }
                #endregion Not Found In Session

                alLang = (ArrayList)HttpContext.Current.Session["MsgDataNotFound"];
                if (CurrentUserLanguageID().Equals("1"))
                    return alLang[0].ToString();
                else
                    return alLang[1].ToString();
            }
            else
            {
                return this.Message;
            }
        }
        #endregion private string MsgDataNotFound()

        #endregion <== METHOD ==>

        #region <== PROPERTY ==>

        #region public string SaveButtonID
        /// <summary>
        /// Setting this property will cause the grid to update all modified records when 
        /// this button is clicked.  It currently supports Button, ImageButton, and LinkButton.
        /// If you set this property, you do not need to call save programatically.
        /// </summary>
        [IDReferenceProperty(typeof(Control))]
        public string SaveButtonID
        {
            get
            {
                return (string)(this.ViewState["SaveButtonID"] ?? String.Empty);
            }
            set
            {
                this.ViewState["SaveButtonID"] = value;
            }
        }
        #endregion public string SaveButtonID

        #region public bool EnableInsert
        /// <summary>
        /// Enables inline inserting.  Off by default.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Browsable(true),
        Category("Extended"),
        Description("Get and Set whether the properties should be EnableInsert.")
        ]
        public bool EnableInsert
        {

            get
            {

                return (bool)(this.ViewState["EnableInsert"] ?? false);

            }
            set
            {
                this.ViewState["EnableInsert"] = value;
            }

        }
        #endregion public bool EnableInsert

        #region public int InsertRowCount
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Browsable(true),
        Category("Extended"),
        Description("Get and Set whether the properties should be InsertRowCount.")
        ]
        public int InsertRowCount
        {
            get
            {
                return (int)(this.ViewState["InsertRowCount"] ?? 1);
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value must be >= 1");

                this.ViewState["InsertRowCount"] = value;
            }
        }
        #endregion public int InsertRowCount

        #region public TableItemStyle InsertRowStyle
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Styles"),
        PersistenceMode(PersistenceMode.InnerProperty),
        NotifyParentProperty(true),
        Description("GridView_InsertRowStyle")
        ]
        public TableItemStyle InsertRowStyle
        {
            get
            {
                if (this.insertRowStyle == null)
                {
                    this.insertRowStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)this.insertRowStyle).TrackViewState();
                    }
                }
                return this.insertRowStyle;
            }
        }
        #endregion public TableItemStyle InsertRowStyle

        #region public bool SaveOldValues
        /// <summary>
        /// Gets and sets whether the old values should be saved.
        /// </summary>
        /// <value><para>This value must be true if you will be using 'CompareAllValues' in the
        /// ObjectDataSource that the grid is connected to.  Setting this to true will cause
        /// the size of the ViewState to grow substantially, so only use it when necessary.</para>
        /// <para>If you are not using that parameter, or if ReadOnly is true, then the values
        /// will not be saved.</para></value>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Behavior"),
        Browsable(true),
        Description("Gets and sets whether the old values should be saved.")
        ]
        public bool SaveOldValues
        {
            get
            {
                return (bool)(this.ViewState["SaveOldValues"] ?? false);
            }
            set
            {
                this.ViewState["SaveOldValues"] = value;
            }
        }
        #endregion public bool SaveOldValues

        #region public bool ReadOnly
        /// <summary>
        /// Gets and sets whether the grid should display in edit or readonly mode.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Browsable(true),
        Category("Behavior"),
        Description("Gets and sets whether the grid should display in edit or readonly mode.")
        ]
        public bool ReadOnly
        {
            get
            {
                return (bool)(this.ViewState["ReadOnly"] ?? false);
            }
            set
            {
                this.ViewState["ReadOnly"] = value;
            }
        }
        #endregion public bool ReadOnly

        #region public bool ContinueOnError
        /// <summary>
        /// Gets and sets whether the grid should continue attempting to update rows after a handled exception.
        /// </summary>
        /// <value><para>Setting this to true will cause the grid to essentially ignore any
        /// handled exceptions during the row update process.  Any unhandled exceptions will cause
        /// the update process to stop.</para></value>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Behavior"),
        Browsable(true),
        Description("Gets and sets whether the grid should continue attempting to update rows after a handled exception.")
        ]
        public bool ContinueOnError
        {
            get
            {
                return (bool)(this.ViewState["ContinueOnError"] ?? false);
            }
            set
            {
                this.ViewState["ContinueOnError"] = value;
            }
        }
        #endregion public bool ContinueOnError


        #region protected int RecordCount
        protected int RecordCount
        {
            get { return (int)(ViewState["RecCount"] ?? 0); }
            set { ViewState["RecCount"] = value; }
        }
        #endregion protected int RecordCount

        #region protected Int32 CustomPageIndex
        public Int32 CustomPageIndex
        {
            get { return (Int32)(ViewState["CustomPageIndexKey"] ?? 0); }
            set { ViewState["CustomPageIndexKey"] = value; }
        }
        #endregion protected Int32 CustomPageIndex

        #region protected String CustomSortExpression
        protected String CustomSortExpression
        {
            get { return (ViewState["SortExpression"] != null ? ViewState["SortExpression"].ToString() : ""); }
            set { ViewState["SortExpression"] = value; }
        }
        #endregion protected String CustomSortExpression

        #region protected SortDirection CustomSortDirection
        protected SortDirection CustomSortDirection
        {
            get { return (ViewState["SortDirection"] != null ? (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["SortDirection"].ToString(), true) : SortDirection.Ascending); }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion protected SortDirection CustomSortDirection

        #region protected int RecordsPerPage
        protected int RecordsPerPage
        {
            get
            {
                return GetCookieRecordPerPage();
            }
            set
            {
                SetCookieRecordPerPage(value);
            }
        }
        #endregion protected int RecordsPerPage


        #region public bool ShowPageSizeDropDownList
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets IsShowPageSizeDropDownList.")
        ]
        public bool ShowPageSizeDropDownList
        {
            get
            {
                return (bool)(this.ViewState["ShowPageSizeDropDownList"] ?? true);
            }
            set
            {
                this.ViewState["ShowPageSizeDropDownList"] = value;
            }
        }
        #endregion public bool ShowPageSizeDropDownList

        #region public string CookieName
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets CookieName.")
        ]
        public string CookieName
        {
            get
            {
                return (string)(this.ViewState["CookieName"] ?? this.ID);
            }
            set
            {
                this.ViewState["CookieName"] = value;
            }
        }
        #endregion public string CookieName

        #region public bool ShowHeaderWhenEmpty
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets ShowHeaderWhenEmpty.")
        ]
        public bool ShowHeaderWhenEmpty
        {
            get
            {
                if (this.ViewState["ShowHeaderWhenEmpty"] == null)
                {
                    this.ViewState["ShowHeaderWhenEmpty"] = true;
                }

                return (bool)this.ViewState["ShowHeaderWhenEmpty"];
            }
            set
            {
                this.ViewState["ShowHeaderWhenEmpty"] = value;
            }
        }
        #endregion public bool ShowHeaderWhenEmpty

        #region public bool ShowFooterWhenEmpty
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets ShowFooterWhenEmpty.")
        ]
        public bool ShowFooterWhenEmpty
        {
            get
            {
                if (this.ViewState["ShowFooterWhenEmpty"] == null)
                {
                    this.ViewState["ShowFooterWhenEmpty"] = false;
                }

                return (bool)this.ViewState["ShowFooterWhenEmpty"];
            }
            set
            {
                this.ViewState["ShowFooterWhenEmpty"] = value;
            }
        }
        #endregion public bool ShowFooterWhenEmpty

        #region public bool ShowMsgDataNotFound
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets ShowMsgDataNotFound.")
        ]
        public bool ShowMsgDataNotFound
        {
            get
            {
                if (this.ViewState["ShowMsgDataNotFound"] == null)
                {
                    this.ViewState["ShowMsgDataNotFound"] = true;

                }

                return (bool)this.ViewState["ShowMsgDataNotFound"];
            }
            set
            {
                this.ViewState["ShowMsgDataNotFound"] = value;
            }
        }
        #endregion public bool ShowMsgDataNotFound

        #region public string Message
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Standard"),
        Browsable(true),
        Description("Gets and Sets Message.")
        ]
        public string Message
        {
            get
            {
                if (this.ViewState["Message"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.ViewState["Message"].ToString();
                }
            }
            set
            {
                this.ViewState["Message"] = value;
            }
        }
        #endregion public string Message

        #endregion <== PROPERTY ==>

        #region protected void ctlRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        protected void ctlRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomPageIndex = 0;
            RecordsPerPage = Convert.ToInt32(ctlRecordsPerPage.SelectedValue);
            this.DataCountAndBind();
        }
        #endregion protected void ctlRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void GridViewInsertEventHandler(object sender, GridViewInsertEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class GridViewInsertEventArgs : CancelEventArgs
    {
        private int _rowIndex;
        private IOrderedDictionary _values;

        public GridViewInsertEventArgs(int rowIndex)
            : base(false)
        {
            this._rowIndex = rowIndex;
        }

        /// <summary>
        /// Gets a dictionary containing the revised values of the non-key field name/value
        /// pairs in the row to update.
        /// </summary>
        public IOrderedDictionary NewValues
        {
            get
            {
                if (this._values == null)
                {
                    this._values = new OrderedDictionary();
                }
                return this._values;
            }
        }

        /// <summary>
        /// Gets the index of the row being updated.
        /// </summary>
        public int RowIndex { get { return this._rowIndex; } }
    }

}