using System;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS.DB.DTO.ValueObject;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.DAL;
using SS.DB.Helper;

using SS.SU.BLL;

namespace SS.DB.BLL.Implement
{
    public partial class DbLanguageService : ServiceBase<DbLanguage, short>, IDbLanguageService
    {
        #region Constant
        private const int maxFileSize = 50000;
        private const int maxImageWidth = 50;
        private const int maxImageHeight = 50;
        #endregion

        public override IDao<DbLanguage, short> GetBaseDao()
        {
            return SsDbDaoProvider.DbLanguageDao;
        }

        #region IDbLanguageService Members

        public void AddLanguage(DbLanguage language, HttpPostedFile imageFile)
        {
            #region Validate DbLanguage
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(language.LanguageName))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            else
            {
                DbLanguage dblanguage = new DbLanguage();
                IList<Language> languageList = SsDbDaoProvider.DbLanguageDao.FindByDbLanguageCriteria(language);
                if (languageList.Count != 0)
                {
                    errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("LanguageNameIsAlready"));
                }
            }
            if (string.IsNullOrEmpty(language.LanguageCode))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            // Check file Type of imageFile.
            if ((!imageFile.ContentType.Equals("image/pjpeg"))&&(!imageFile.ContentType.Equals("image/gif")) && (!imageFile.ContentType.Equals("image/jpeg")) && (!imageFile.ContentType.Equals("image/jpg")))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
            }
            // Check file Size of imageFile.
            if (imageFile.ContentLength > maxFileSize)
            {
                // File size exceed 50KB.
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
            }
            // Check file Dimension of imageFile
            Image uploadImage = Image.FromStream(imageFile.InputStream);
            if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("DimensionError"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbLanguageDao.Save(language);
        }

        public void AddLanguage(DbLanguage language)
        {
            #region Validate DbLanguage
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(language.LanguageName))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            else
            {
                DbLanguage dblanguage = new DbLanguage();
                IList<Language> languageList = SsDbDaoProvider.DbLanguageDao.FindByDbLanguageCriteria(language);
                if (languageList.Count != 0)
                {
                    errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("LanguageNameIsAlready"));
                }
            }
            if (string.IsNullOrEmpty(language.LanguageCode))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbLanguageDao.Save(language);
        }

        public void UpdateLanguage(DbLanguage language)
        {
            #region Validate DbLanguage
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(language.LanguageName))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            //else
            //{
            //    DbLanguage sulanguage = new DbLanguage();
            //    IList<Language> languageList = DbDaoProvider.DbLanguageDao.FindBySuLanguageCriteria(language);
            //    if (languageList != null)
            //    {
            //        errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("LanguageNameIsAlready"));
            //    }
            //}
            if (string.IsNullOrEmpty(language.LanguageCode))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbLanguageDao.SaveOrUpdate(language);
        }

        public void UpdateLanguage(DbLanguage language, HttpPostedFile imageFile)
        {
            #region Validate DbLanguage
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (string.IsNullOrEmpty(language.LanguageName))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            //else
            //{
            //    DbLanguage sulanguage = new DbLanguage();
            //    IList<Language> languageList = DbDaoProvider.DbLanguageDao.FindBySuLanguageCriteria(language);
            //    if (languageList != null)
            //    {
            //        errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("LanguageNameIsAlready"));
            //    }
            //}
            if (string.IsNullOrEmpty(language.LanguageCode))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            }
            // Check file Type of imageFile.
            if ((!imageFile.ContentType.Equals("image/pjpeg")) && (!imageFile.ContentType.Equals("image/gif")) && (!imageFile.ContentType.Equals("image/jpeg")) && (!imageFile.ContentType.Equals("image/jpg")))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
            }
            // Check file Size of imageFile.
            if (imageFile.ContentLength > maxFileSize)
            {
                // File size exceed 50KB.
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
            }
            // Check file Dimension of imageFile
            Image uploadImage = Image.FromStream(imageFile.InputStream);
            if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
            {
                errors.AddError("Language.Error", new Spring.Validation.ErrorMessage("DimensionError"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            SsDbDaoProvider.DbLanguageDao.SaveOrUpdate(language);
        }

        #endregion

    }
}
