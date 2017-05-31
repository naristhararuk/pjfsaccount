using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.SAP.BAPI.Service.Const
{
    public enum DocumentKind
    {
        Advance,
        Remittance,        
        Expense,
        ExpenseRemittance,
        FixedAdvance,
        FixedAdvanceReturn
    }
}
