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
	public class SuAnnouncementService : ServiceBase<SuAnnouncement, short>, ISuAnnouncementService
	{
		#region Override Method
		public override IDao<SuAnnouncement, short> GetBaseDao()
		{
			return DaoProvider.SuAnnouncementDao;
		}
		#endregion
	
		#region ISuAnnouncementService Members
		public IList<SuAnnouncement> FindBySuAnnouncementCriteria(SuAnnouncement announcementCriteria, int firstResult, int maxResults, string sortExpression)
		{
			return NHibernateDaoHelper.FindPagingByCriteria<SuAnnouncement>(DaoProvider.SuAnnouncementDao, "FindBySuAnnouncementCriteria", new object[] { announcementCriteria }, firstResult, maxResults, sortExpression);
		}
		public int CountBySuAnnouncementCriteria(SuAnnouncement announcementCriteria)
		{
			return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuAnnouncementDao, "FindBySuAnnouncementCriteria", new object[] { announcementCriteria });
		}
		public short AddAnnouncement(SuAnnouncement announcement, SuAnnouncementLang announcementLang)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (string.IsNullOrEmpty(announcementLang.AnnouncementHeader))
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("AnnouncementHeaderRequired"));
			}

			if (announcement.EffectiveDate == null)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateFormatError"));
			}
			else if (announcement.EffectiveDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateRequired"));
			}
			else if (announcement.EffectiveDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateNotBeBackDate"));
			}

			if (announcement.LastDisplayDate == null)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("LastDisplayDateFormatError"));
			}
			else if (announcement.LastDisplayDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("LastDisplayDateRequired"));
			}
			else if (announcement.LastDisplayDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("LastDisplayDateNotBeBackDate"));
			}
			
			if ((!announcement.EffectiveDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && (!announcement.LastDisplayDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
				 && (announcement.EffectiveDate != null) && (announcement.LastDisplayDate != null))
			{
				if (announcement.EffectiveDate.Value > announcement.LastDisplayDate.Value)
				{
					errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTLastDisplayDate"));
				}
			}
			
			if (announcement.AnnouncementGroup == null)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("AnnouncementGroupRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			short announcementId = DaoProvider.SuAnnouncementDao.Save(announcement);
			DaoProvider.SuAnnouncementLangDao.Save(announcementLang);
			
			return announcementId;
		}
		public void UpdateAnnouncement(SuAnnouncement announcement)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcement.EffectiveDate == null)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateFormatError"));
			}
			else if (announcement.EffectiveDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateRequired"));
			}
			else if (announcement.EffectiveDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateNotBeBackDate"));
			}

			if (announcement.EffectiveDate == null)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("LastDisplayDateFormatError"));
			}
			else if (announcement.LastDisplayDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("LastDisplayDateRequired"));
			}
			else if (announcement.LastDisplayDate.Value < DateTime.Today)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("LastDisplayDateNotBeBackDate"));
			}
			
			if ((!announcement.EffectiveDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && (!announcement.LastDisplayDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
				&& (announcement.EffectiveDate != null) && (announcement.LastDisplayDate != null))
			{
				if (announcement.EffectiveDate.Value > announcement.LastDisplayDate.Value)
				{
					errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTLastDisplayDate"));
				}
			}
			
			if (announcement.AnnouncementGroup == null)
			{
				errors.AddError("Announcement.Error", new Spring.Validation.ErrorMessage("AnnouncementGroupRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			DaoProvider.SuAnnouncementDao.SaveOrUpdate(announcement);
		}
		#endregion
	}
}
