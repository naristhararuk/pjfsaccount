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

namespace SS.Standard.Base.UI
{
    /// <summary>
    /// BulkEditGridView allows users to edit multiple rows of a gridview at once, and have them
    /// all saved.
    /// </summary>
    [
    DefaultEvent("SelectedIndexChanged"),
    Designer("System.Web.UI.Design.WebControls.GridViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
    ControlValueProperty("SelectedValue"),
    ]
    public class BaseGridView : System.Web.UI.WebControls.GridView
    {
        //key for the RowInserting event handler list
        public static readonly object RowInsertingEvent = new object();

        private List<int> dirtyRows = new List<int>();
        private List<int> newRows = new List<int>();

        private TableItemStyle insertRowStyle;
        private bool isSaving;
        private bool continueUpdating = true;
        private bool buttonCausesValidation = true;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseGridView()
        {
        }

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

        /// <summary>
        /// We need to surpress data binding while we're uploading, or else values will not persist.
        /// </summary>
        protected override void PerformSelect()
        {
            if (!isSaving) base.PerformSelect();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            RowUpdating += new GridViewUpdateEventHandler(BulkEditGridView_RowUpdating);
            RowDataBound += new GridViewRowEventHandler(BulkEditGridView_RowDataBound);
        }

        protected override void OnRowUpdated(GridViewUpdatedEventArgs e)
        {
            base.OnRowUpdated(e);
            continueUpdating = null == e.Exception || (e.ExceptionHandled && ContinueOnError);
        }

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

        /// <summary>
        /// Enables inline inserting.  Off by default.
        /// </summary>
        [Category("Extended")]
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


        [Category("Extended")]
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

        /// <summary>
        /// Gets and sets whether the grid should display in edit or readonly mode.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("Behavior"),
        Browsable(true),
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


        //protected int RecordCount
        //{
        //    get { return (int)(ViewState["RecCount"] ?? 0); }
        //    set { ViewState["RecCount"] = value; }
        //}

        //protected Int32 CustomPageIndex
        //{
        //    get { return (Int32)(ViewState["CustomPageIndexKey"] ?? 0); }
        //    set { ViewState["CustomPageIndexKey"] = value; }
        //}

        //protected String CustomSortExpression
        //{
        //    get { return (ViewState["SortExpression"] != null ? ViewState["SortExpression"].ToString() : ""); }
        //    set { ViewState["SortExpression"] = value; }
        //}

        //protected SortDirection CustomSortDirection
        //{
        //    get { return (ViewState["SortDirection"] != null ? (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["SortDirection"].ToString(), true) : SortDirection.Ascending); }
        //    set { ViewState["SortDirection"] = value; }
        //}

        //protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        //{
        //    pagedDataSource.AllowPaging = true;
        //    pagedDataSource.AllowCustomPaging = true;
        //    pagedDataSource.VirtualCount = RecordCount;
        //    pagedDataSource.CurrentPageIndex = CustomPageIndex;
        //    base.InitializePager(row, columnSpan, pagedDataSource);
        //}

        //public delegate Object RequestDataHandler(int startRow, int pageSize, string sortExpression);
        //public event RequestDataHandler RequestData;
        //public delegate int RequestCountHandler();
        //public event RequestCountHandler RequestCount;

        //protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        //{
        //    CustomPageIndex = e.NewPageIndex;
        //    this.BindGrid();
        //}

        //protected override void OnSorting(GridViewSortEventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(CustomSortExpression) && e.SortExpression == CustomSortExpression)
        //    {
        //        if (CustomSortDirection == SortDirection.Ascending)
        //            CustomSortDirection = SortDirection.Descending;
        //        else
        //            CustomSortDirection = SortDirection.Ascending;
        //    }
        //    CustomSortExpression = e.SortExpression;

        //    this.BindGrid();
        //}

        //public void DataCountAndBind()
        //{
        //    if (RequestCount != null)
        //        RecordCount = RequestCount();
        //    else
        //        RecordCount = 0;
        //    CustomPageIndex = 0;
        //    this.BindGrid();
        //}



        //protected override void DataBind(bool raiseOnDataBinding)
        //{
        //    //base.DataBind(raiseOnDataBinding);
        //    DataCountAndBind();
        //}

        //protected void BindGrid()
        //{
        //    if (RequestData != null)
        //    {
        //        string sort = "";
        //        if (!string.IsNullOrEmpty(CustomSortExpression))
        //            sort = CustomSortExpression + " " + (CustomSortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        //        this.DataSource = RequestData(CustomPageIndex * this.PageSize, this.PageSize, sort);
        //    }
        //    this.DataBind();
        //}
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