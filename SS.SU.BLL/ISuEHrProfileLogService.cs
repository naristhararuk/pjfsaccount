using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using System.Web.UI.WebControls;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
namespace SS.SU.BLL
{
    public interface ISuEHrProfileLogService : IService<SueHrProfileLog, long>
    {
        void DeleteAll();
        void AddLog(SueHrProfileLog eHrLog);
    }
}
