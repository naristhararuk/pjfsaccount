using System;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using SS.SU.BLL;
using SS.Standard.Utilities;

namespace SS.SU.BLL.Implement
{
    public class SuRTEContentService : ServiceBase<SuRTEContent, short>, ISuRTEContentService
    {
        #region Override Method
        public override IDao<SuRTEContent, short> GetBaseDao()
        {
            return DaoProvider.SuRTEContentDao;
        }
        #endregion
        #region ISuRTEContentService Members

        public short AddContent(SuRTEContent content)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(content.Header))
            {
                errors.AddError("Content.Error", new Spring.Validation.ErrorMessage("RequeiredHeader"));
            }
            if (string.IsNullOrEmpty(content.Content))
            {
                errors.AddError("Content.Error", new Spring.Validation.ErrorMessage("RequeiredContent"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            return DaoProvider.SuRTEContentDao.Save(content);
        }
        public void UpdateContent(SuRTEContent content)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(content.Header))
            {
                errors.AddError("Content.Error", new Spring.Validation.ErrorMessage("RequeiredHeader"));
            }
            if (string.IsNullOrEmpty(content.Content))
            {
                errors.AddError("Content.Error", new Spring.Validation.ErrorMessage("RequeiredContent"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);


            DaoProvider.SuRTEContentDao.SaveOrUpdate(content);
        }

        #endregion
    }
}
