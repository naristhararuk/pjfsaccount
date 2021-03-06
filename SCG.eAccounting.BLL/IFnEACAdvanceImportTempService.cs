﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SCG.eAccounting.DTO;

namespace SCG.eAccounting.BLL
{
    public interface IFnEACAdvanceImportTempService : IService<FneacAdvanceImportTemp, long>
    {
        void ClearAll();
        void ImportIntoDatabase();
    }
}
