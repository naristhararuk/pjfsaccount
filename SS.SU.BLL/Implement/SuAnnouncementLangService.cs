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
	public class SuAnnouncementLangService : ServiceBase<SuAnnouncementLang, long>, ISuAnnouncementLangService
	{
		#region Override Method
		public override IDao<SuAnnouncementLang, long> GetBaseDao()
		{
			return DaoProvider.SuAnnouncementLangDao;
		}
		#endregion
		
		#region ISuAnnouncementLangService Members
		public IList<SuAnnouncementLang> FindByAnnouncementAndLanguage(short announcementId, short languageId)
		{
			return DaoProvider.SuAnnouncementLangDao.FindByAnnouncementAndLanguage(announcementId, languageId);
		}
		public IList<SuAnnouncementLang> FindByAnnouncementId(short announcementId)
		{
			return DaoProvider.SuAnnouncementLangDao.FindByAnnouncementId(announcementId);
		}
		public void UpdateAnnouncementLang(IList<SuAnnouncementLang> announcementLangList)
		{
			if (announcementLangList.Count > 0)
			{
				DaoProvider.SuAnnouncementLangDao.DeleteByCriteria(announcementLangList[0].Announcement.AnnouncementGroupid);
				
				// Loop for insert new announcementLang of each
				foreach (SuAnnouncementLang announcementLang in announcementLangList)
				{
					DaoProvider.SuAnnouncementLangDao.Save(announcementLang);
				}
			}
		}
		public void SaveAnnouncementLang(SuAnnouncementLang announcementLang)
		{
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementLang.Announcement == null)
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementRequired"));
			}
			if (announcementLang.Language == null)
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("LanguageRequired"));
			}
			if (string.IsNullOrEmpty(announcementLang.AnnouncementHeader))
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
			}
			if (string.IsNullOrEmpty(announcementLang.AnnouncementBody))
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);

			DaoProvider.SuAnnouncementLangDao.DeleteByCriteria(announcementLang.Announcement.Announcementid, announcementLang.Language.Languageid);
			DaoProvider.SuAnnouncementLangDao.Save(announcementLang);
		}
		public void InsertAnnouncementLang(SuAnnouncementLang announcementLang)
		{
			#region Validate AnnouncementLang
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementLang.Announcement == null)
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementRequired"));
			}
			if (announcementLang.Language == null)
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("LanguageRequired"));
			}
			if (string.IsNullOrEmpty(announcementLang.AnnouncementHeader))
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementHeaderRequired"));
			}
			if (string.IsNullOrEmpty(announcementLang.AnnouncementBody))
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementBodyRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			DaoProvider.SuAnnouncementLangDao.Save(announcementLang);
		}
		public void UpdateAnnouncementLang(SuAnnouncementLang announcementLang)
		{
			#region Validate AnnouncementLang
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementLang.Announcement == null)
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementRequired"));
			}
			if (announcementLang.Language == null)
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("LanguageRequired"));
			}
			if (string.IsNullOrEmpty(announcementLang.AnnouncementHeader))
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementHeaderRequired"));
			}
			if (string.IsNullOrEmpty(announcementLang.AnnouncementBody))
			{
				errors.AddError("AnnouncementLang.Error", new Spring.Validation.ErrorMessage("AnnouncementBodyRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			DaoProvider.SuAnnouncementLangDao.SaveOrUpdate(announcementLang);
		}
		#endregion
	}
}
