using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.Query
{
    public interface ISimpleMasterQuery
    {
        Object GetList(int startRow, int pageSize, string sortExpression);
        int GetCount();
    }
}
