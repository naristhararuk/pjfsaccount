using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;
using SCG.FN.DTO;
using SCG.FN.DTO.ValueObject;

namespace SCG.FN.BLL
{
    public interface IFnCashierLangService : IService<FnCashierLang, long>
    {
        void UpdateCashierLang(IList<FnCashierLang> cashierLangList);
    }
}
