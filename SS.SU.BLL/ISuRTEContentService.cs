using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
    public interface ISuRTEContentService : IService<SuRTEContent, short>
    {
        short AddContent(SuRTEContent content);
        void UpdateContent(SuRTEContent content);
    }
}
