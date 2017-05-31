using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.AlertMsg.DAL.Interface
{
    public interface IMessageDAL
    {
        string GetSqlExceptionNumber(Exception ex);
        AlertMessage GetMessage(Exception ex);
        AlertMessage GetMessage(string MsgID);
        string GetShowMessage(string MsgID);
    }
}
