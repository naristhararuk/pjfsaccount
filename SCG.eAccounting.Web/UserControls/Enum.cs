using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCG.eAccounting.Web.UserControls
{
	public class Enum
	{
		
	}
	
	public struct ModeEnum
	{
		public const string Readonly = "Readonly";
		public const string ReadWrite = "ReadWrite";
	}

	public struct AdvanceTypeEnum
	{
		public const string International = "International";
		public const string Domestic = "Domestic";
	}

    public struct FlagEnum
    {
        // Use set document status.
        public const string NewFlag = "New";
        public const string EditFlag = "Edit";
        public const string ViewFlag = "View";
    }
    public struct ViewStateName
    {
        //User in Initialize Document
        public const string TransactionID = "TransactionID";
        public const string DocumentID = "DocumentID";
        public const string ParentTxID = "ParentTxID";
        public const string InitialFlag = "InitialFlag";
        public const string EditableFields = "EditableFields";
        public const string VisibleFields = "VisibleFields";
    }
    public struct LabelExtenderViewStateName
    {
        public const string LinkControlVisible = "LinkControlVisible";
        public const string LinkControlEdiable = "LinkControlEditable";
        public const string InitialFlag = "InitialFlag";
    }

    public struct ProgramCodeEnum
    {
        public const string Search = "Search";
        public const string TASearch = "TASearch";
    }
}
