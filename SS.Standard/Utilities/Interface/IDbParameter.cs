using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SS.Standard.Utilities.Interface
{
    public interface IDbParameter
    {
        string getDbParameter(string group_no, string seq_no);
        string getDbParameter(int group_no, int seq_no);
        void OpenConnection();
        void CloseConnection();

    }
}
