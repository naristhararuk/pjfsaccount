using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.DAL;
using SS.SU.BLL;

namespace SS.SU.BLL.Implement
{
	public class SuAnnouncementGroupLangService : ServiceBase<SuAnnouncementGroupLang, short>, ISuAnnouncementGroupLangService
	{
		public override IDao<SuAnnouncementGroupLang, short> GetBaseDao()
		{
			return DaoProvider.SuAnnouncementGroupLangDao;
		}

		#region ISuAnnouncementGroupLangService Members
		public IList<SuAnnouncementGroupLang> FindByAnnouncementGroupId(short announcementGroupId)
		{
			return DaoProvider.SuAnnouncementGroupLangDao.FindByAnnouncementGroupId(announcementGroupId);
		}
		public void UpdateAnnouncementGroupLang(IList<SuAnnouncementGroupLang> announcementGroupLangList)
		{
			if (announcementGroupLangList.Count > 0)
			{
				// Delete all of AnnouncementGroupLang row of AnnouncementGroupId
				DaoProvider.SuAnnouncementGroupLangDao.DeleteByCriteria(announcementGroupLangList[0].AnnouncementGroup.AnnouncementGroupid);
				
				foreach (SuAnnouncementGroupLang announcementGroupLang in announcementGroupLangList)
				{
					#region Validate AnnouncementLang
					Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
					if (announcementGroupLang.AnnouncementGroup == null)
					{
						errors.AddError("AnnouncementGroupLang.Error", new Spring.Validation.ErrorMessage("AnnouncementGroupRequired"));
					}
					if (announcementGroupLang.Language == null)
					{
						errors.AddError("AnnouncementGroupLang.Error", new Spring.Validation.ErrorMessage("LanguageRequired"));
					}
					if (string.IsNullOrEmpty(announcementGroupLang.AnnouncementGroupName))
					{
						errors.AddError("AnnouncementGroupLang.Error", new Spring.Validation.ErrorMessage("AnnouncementGroupNameRequired"));
					}
					if (!errors.IsEmpty) throw new ServiceValidationException(errors);
					#endregion
					
					DaoProvider.SuAnnouncementGroupLangDao.Save(announcementGroupLang);
				}
			}
		}
		#endregion
	}
}
