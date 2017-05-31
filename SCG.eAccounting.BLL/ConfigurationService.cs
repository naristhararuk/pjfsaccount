using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.BLL
{
    public class ConfigurationService
    {
    
    }

    public class AvAdvanceConstant
    {
        public static int AdvanceHoldStateEventID;
        public int advanceHoldStateEventID
        {
            set { AvAdvanceConstant.AdvanceHoldStateEventID = value; }
        }
    }

    public class FnExpenseConstant
    {
        public static int ExpenseHoldStateEventID;
        public int expenseHoldStateEventID
        {
            set { FnExpenseConstant.ExpenseHoldStateEventID = value; }
        }
    }

    public class TaConstant
    {
        public static int TaCompleteStateID;
        public int taCompleteStateID
        {
            set { TaConstant.TaCompleteStateID = value; }
        }
    }
}
