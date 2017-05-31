using System;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using SS.SU.BLL;

namespace SS.SU.BLL.Implement
{
	public class SuAnnouncementGroupService : ServiceBase<SuAnnouncementGroup, short>, ISuAnnouncementGroupService
	{
		#region Constant
		private const int maxFileSize = 51200;
		private const int maxImageWidth = 50;
		private const int maxImageHeight = 50;
		#endregion
		
		#region Override Method
		public override IDao<SuAnnouncementGroup, short> GetBaseDao()
		{
			return DaoProvider.SuAnnouncementGroupDao;
		}
		#endregion
	
		#region ISuAnnouncementGroupService Members
		public IList<SuAnnouncementGroup> FindBySuAnnouncementGroupCriteria(SuAnnouncementGroup announcementGroupCriteria, int firstResult, int maxResults, string sortExpression)
		{
			return NHibernateDaoHelper.FindPagingByCriteria<SuAnnouncementGroup>(DaoProvider.SuAnnouncementGroupDao, "FindBySuAnnouncementGroupCriteria", new object[] { announcementGroupCriteria }, firstResult, maxResults, sortExpression);
		}
		public IList<AnnouncementGroup> GetTranslatedList(short languageId)
		{
			return DaoProvider.SuAnnouncementGroupDao.GetTranslatedList(languageId);
		}
		public int CountBySuAnnouncementGroupCriteria(SuAnnouncementGroup announcementGroupCriteria)
		{
			return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuAnnouncementGroupDao, "FindBySuAnnouncementGroupCriteria", new object[] { announcementGroupCriteria });
		}
		public short AddAnnouncementGroup(SuAnnouncementGroup announcementGroup, SuAnnouncementGroupLang announcementGroupLang)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementGroup.DisplayOrder == null)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderTypeMissMatch"));
			}
			else if (announcementGroup.DisplayOrder.GetValueOrDefault(0) == 0)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderRequired"));
			}
			if (string.IsNullOrEmpty(announcementGroupLang.AnnouncementGroupName))
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("AnnouncementGroupNameRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			short announcementGroupId = DaoProvider.SuAnnouncementGroupDao.Save(announcementGroup);
			DaoProvider.SuAnnouncementGroupLangDao.Save(announcementGroupLang);
			
			return announcementGroupId;
		}
		public short AddAnnouncementGroup(SuAnnouncementGroup announcementGroup, SuAnnouncementGroupLang announcementGroupLang, HttpPostedFile imageFile)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementGroup.DisplayOrder == null)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderTypeMissMatch"));
			}
			else if (announcementGroup.DisplayOrder.GetValueOrDefault(0) == 0)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderRequired"));
			}
			if (string.IsNullOrEmpty(announcementGroupLang.AnnouncementGroupName))
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("AnnouncementGroupNameRequired"));
			}
			// Check file Type of imageFile.
			if ((!imageFile.ContentType.Equals("image/gif")) && (!imageFile.ContentType.Equals("image/jpeg")) && (!imageFile.ContentType.Equals("image/jpg")) && (!imageFile.ContentType.Equals("image/pjpeg")))
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
			}
			// Check file Size of imageFile.
			if (imageFile.ContentLength > maxFileSize) 
			{
				// File size exceed 50KB.
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
			}
			// Check file Dimension of imageFile
			Image uploadImage = Image.FromStream(imageFile.InputStream);
			if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DimensionError"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			short announcementGroupId = DaoProvider.SuAnnouncementGroupDao.Save(announcementGroup);
			DaoProvider.SuAnnouncementGroupLangDao.Save(announcementGroupLang);
			
			return announcementGroupId;
		}
		public void UpdateAnnouncementGroup(SuAnnouncementGroup announcementGroup)
		{
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementGroup.DisplayOrder == null)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderTypeMissMatch"));
			}
			else if (announcementGroup.DisplayOrder.GetValueOrDefault(0) == 0)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderRequired"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion
			
			DaoProvider.SuAnnouncementGroupDao.SaveOrUpdate(announcementGroup);
		}
		public void UpdateAnnouncementGroup(SuAnnouncementGroup announcementGroup, HttpPostedFile imageFile)
		{	
			#region Validate SuAnnouncementGroup
			Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
			if (announcementGroup.DisplayOrder == null)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderTypeMissMatch"));
			}
			else if (announcementGroup.DisplayOrder.GetValueOrDefault(0) == 0)
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("DisplayOrderRequired"));
			}
			// Check file Type of imageFile.
			if ((!imageFile.ContentType.Equals("image/gif")) && (!imageFile.ContentType.Equals("image/jpeg")) &&
				(!imageFile.ContentType.Equals("image/jpg")) && (!imageFile.ContentType.Equals("image/pjpeg")))
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
			}
			// Check file Size of imageFile.
			if (imageFile.ContentLength > maxFileSize)
			{
				// File size exceed 50KB.
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
			}
			// Check file Dimension of imageFile
			Image uploadImage = Image.FromStream(imageFile.InputStream);
			if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
			{
				errors.AddError("AnnouncementGroup.Error", new Spring.Validation.ErrorMessage("555"));
			}
			if (!errors.IsEmpty) throw new ServiceValidationException(errors);
			#endregion

			DaoProvider.SuAnnouncementGroupDao.SaveOrUpdate(announcementGroup);
		}
		#endregion
	}
}
