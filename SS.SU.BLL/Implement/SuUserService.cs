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

using SS.SU.BLL;
using SS.SU.DAL;
using SCG.DB.DTO;
using SS.DB.Query;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using System.Text.RegularExpressions;




namespace SS.SU.BLL.Implement
{
    public partial class SuUserService : ServiceBase<SuUser, long>, ISuUserService
    {
        #region Constant
        //public ParameterServices ParameterServices { get; set; }
        public ISuUserRoleService SuUserRoleService { get; set; }
        public ISuRoleService SuRoleService { get; set; }
        private const int maxFileSize = 102400;
        private const int maxImageWidth = 800;
        private const int maxImageHeight = 600;
        #endregion

        #region Overrid Method
        public override IDao<SuUser, long> GetBaseDao()
        {
            return DaoProvider.SuUserDao;
        }
        #endregion





        public void DeleteUser(SuUser user)
        {
            DaoProvider.SuUserDao.Delete(user);

            //sync delete user data to eXpense 4.7
            DaoProvider.SuUserDao.SyncDeleteUser(user.UserName);
        }

        #region ISuUserService Members
        public IList<SuUser> FindByUserName(string userName)
        {
            return DaoProvider.SuUserDao.FindByUserName(userName);
        }
        public long AddNewUser(SuUser user, short languageId, HttpPostedFile mapFile)
        {
            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // Validate UserName.
            if (string.IsNullOrEmpty(user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameRequired"));
            }
            else
            {
                if (DaoProvider.SuUserDao.FindByUserName(user.UserName).Count > 0)
                {
                    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameAlreadyExist"));
                }
            }

            // Validate Password.
            if (string.IsNullOrEmpty(user.Password))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordRequired"));
            }
            //if (string.IsNullOrEmpty(user.ConfirmPassword))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("ConfirmPasswordRequired"));
            //}
            int strMinPasswordLength = ParameterServices.MinPasswordAge; // modify by tom 03/03/2009 ParameterServices.minPasswordAge;
            string strPasswordNotAllow = ParameterServices.NotAllowPassword;

            if (user.Password.Length < strMinPasswordLength)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordLengthError"));
            }
            //if (!user.Password.Equals(user.ConfirmPassword))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordNotMatch"));
            //}
            if ((user.Password.Equals(user.UserName)) || (user.Password.Equals(strPasswordNotAllow)))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordNotAllow"));
            }

            // Validate IdCardNo.
            //if (string.IsNullOrEmpty(user.IdCardNo))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoRequired"));
            //}
            //else
            //{
            //    try
            //    {
            //        long idCardNo = long.Parse(user.IdCardNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoTypeMismatch"));
            //    }
            //}

            // Validate Organization.
            //if (user.Organization == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            //}
            //else
            //{
            //    // Validate DivisionName.
            //    if (string.IsNullOrEmpty(user.TempDivisionName))
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            //    }
            //    else
            //    {
            //        IList<SuDivisionLang> divLangList = DaoProvider.SuDivisionLangDao.FindByDivisionName(user.Organization.Organizationid, languageId, user.TempDivisionName);
            //        if (divLangList.Count == 0)
            //        {
            //            errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionNotExist"));
            //        }
            //        else
            //        {
            //            user.Division = divLangList[0].Division;
            //        }
            //    }
            //}

            // Validate DefaultLanguage.
            if (user.Language == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DefaultLanguageRequired"));
            }

            // Validate HomeTelephone.
            //if (!string.IsNullOrEmpty(user.HomePhoneNo))
            //{
            //    try
            //    {
            //        long homePhoneNo = long.Parse(user.HomePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate HomeTelephone Ext.
            //if (!string.IsNullOrEmpty(user.HomePhoneNoExt))
            //{
            //    try
            //    {
            //        int homePhoneNoExt = int.Parse(user.HomePhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate WorkingPhoneNo.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNo))
            //{
            //    try
            //    {
            //        long workPhoneNo = long.Parse(user.WorkingPhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate WorkingPhoneNo Ext.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNoExt))
            //{
            //    try
            //    {
            //        int workPhoneNoExt = int.Parse(user.WorkingPhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate FaxNo.
            //if (!string.IsNullOrEmpty(user.FaxNo))
            //{
            //    try
            //    {
            //        long faxNo = long.Parse(user.FaxNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoTypeMisMatch"));
            //    }
            //}
            // Validate FaxNo Ext.
            //if (!string.IsNullOrEmpty(user.FaxNoExt))
            //{
            //    try
            //    {
            //        int faxNoExt = int.Parse(user.FaxNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoExtTypeMisMatch"));
            //    }
            //}

            // Validate MobilePhoneNo.
            //if (!string.IsNullOrEmpty(user.MobilePhoneNo))
            //{
            //    try
            //    {
            //        long mobileNo = long.Parse(user.MobilePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("MobilePhoneNoTypeMisMatch"));
            //    }
            //}

            // Validate Email.
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EmailRequired"));
            }

            // Validate EffectiveDate and EndDate.
            //if (user.EffDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateTypeMisMatch"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateRequired"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateNotBeBackDate"));
            //}

            //if (user.EndDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateTypeMisMatch"));
            //}
            //else if (user.EndDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateRequired"));
            //}
            //else if (user.EndDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateNotBeBackDate"));
            //}

            //if ((user.EffDate != null) && (user.EndDate != null) &&
            //    (!user.EffDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && 
            //    (!user.EndDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)))
            //{
            //    if (user.EffDate.Value > user.EndDate.Value)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTEndDate"));
            //    }
            //}

            // Validate SetFailTime.
            if (user.SetFailTime == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("SetFailTimeTypeMisMatch"));
            }

            // Check file type of mapFile
            if ((!mapFile.ContentType.Equals("image/gif")) && (!mapFile.ContentType.Equals("image/jpeg")) && (!mapFile.ContentType.Equals("image/jpg")) && (!mapFile.ContentType.Equals("image/pjpeg")))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
            }
            // Check file Size of imageFile.
            if (mapFile.ContentLength > maxFileSize)
            {
                // File size exceed 100KB.
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
            }
            // Check file Dimension of imageFile
            Image uploadImage = Image.FromStream(mapFile.InputStream);
            if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DimensionError"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            // Set user.Password value by encrypt input with MD5 algorithm.
            user.Password = Encryption.Md5Hash(user.Password);
            return DaoProvider.SuUserDao.Save(user);
        }
        public long AddNewUser(SuUser user, short languageId)
        {
            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // Validate UserName.
            if (string.IsNullOrEmpty(user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameRequired"));
            }
            else
            {
                if (DaoProvider.SuUserDao.FindByUserName(user.UserName).Count > 0)
                {
                    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameAlreadyExist"));
                }
            }

            // Validate Password.
            if (string.IsNullOrEmpty(user.Password))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordRequired"));
            }
            //if (string.IsNullOrEmpty(user.ConfirmPassword))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("ConfirmPasswordRequired"));
            //}
            int strMinPasswordLength = ParameterServices.MinPasswordAge; // modify by tom 03/03/2009 ParameterServices.minPasswordAge;
            string strPasswordNotAllow = ParameterServices.NotAllowPassword;

            if (user.Password.Length < strMinPasswordLength)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordLengthError"));
            }
            //if (!user.Password.Equals(user.ConfirmPassword))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordNotMatch"));
            //}
            if ((user.Password.Equals(user.UserName)) || (user.Password.Equals(strPasswordNotAllow)))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordNotAllow"));
            }

            // Validate IdCardNo.
            //if (string.IsNullOrEmpty(user.IdCardNo))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoRequired"));
            //}
            //else
            //{
            //    try
            //    {
            //        long idCardNo = long.Parse(user.IdCardNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoTypeMisMatch"));
            //    }
            //}

            // Validate Organization.
            //if (user.Organization == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            //}
            //else
            //{
            //    // Validate DivisionName.
            //    if (string.IsNullOrEmpty(user.TempDivisionName))
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            //    }
            //    else
            //    {
            //        IList<SuDivisionLang> divLangList = DaoProvider.SuDivisionLangDao.FindByDivisionName(user.Organization.Organizationid, languageId, user.TempDivisionName);
            //        if (divLangList.Count == 0)
            //        {
            //            errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionNotExist"));
            //        }
            //        else
            //        {
            //            user.Division = divLangList[0].Division;
            //        }
            //    }
            //}

            // Validate DefaultLanguage.
            if (user.Language == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DefaultLanguageRequired"));
            }

            // Validate HomeTelephone.
            //if (!string.IsNullOrEmpty(user.HomePhoneNo))
            //{
            //    try
            //    {
            //        long homePhoneNo = long.Parse(user.HomePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate HomeTelephone Ext.
            //if (!string.IsNullOrEmpty(user.HomePhoneNoExt))
            //{
            //    try
            //    {
            //        int homePhoneNoExt = int.Parse(user.HomePhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate WorkingPhoneNo.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNo))
            //{
            //    try
            //    {
            //        long workPhoneNo = long.Parse(user.WorkingPhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate WorkingPhoneNo Ext.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNoExt))
            //{
            //    try
            //    {
            //        int workPhoneNoExt = int.Parse(user.WorkingPhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate FaxNo.
            //if (!string.IsNullOrEmpty(user.FaxNo))
            //{
            //    try
            //    {
            //        long faxNo = long.Parse(user.FaxNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoTypeMisMatch"));
            //    }
            //}
            // Validate FaxNo Ext.
            //if (!string.IsNullOrEmpty(user.FaxNoExt))
            //{
            //    try
            //    {
            //        int faxNoExt = int.Parse(user.FaxNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoExtTypeMisMatch"));
            //    }
            //}

            // Validate MobilePhoneNo.
            //if (!string.IsNullOrEmpty(user.MobilePhoneNo))
            //{
            //    try
            //    {
            //        long mobileNo = long.Parse(user.MobilePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("MobilePhoneNoTypeMisMatch"));
            //    }
            //}

            // Validate Email.
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EmailRequired"));
            }

            // Validate EffectiveDate and EndDate.
            //if (user.EffDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateTypeMisMatch"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateRequired"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateNotBeBackDate"));
            //}

            //if (user.EndDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateTypeMisMatch"));
            //}
            //else if (user.EndDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateRequired"));
            //}
            //else if (user.EndDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateNotBeBackDate"));
            //}

            //if ((user.EffDate != null) && (user.EndDate != null) &&
            //    (!user.EffDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && 
            //    (!user.EndDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)))
            //{
            //    if (user.EffDate.Value > user.EndDate.Value)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTEndDate"));
            //    }
            //}

            // Validate SetFailTime.
            if (user.SetFailTime == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("SetFailTimeTypeMisMatch"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            // Set user.Password value by encrypt input with MD5 algorithm.
            user.Password = Encryption.Md5Hash(user.Password);
            return DaoProvider.SuUserDao.Save(user);
        }
        public void UpdateUser(SuUser user, short languageId, HttpPostedFile mapFile)
        {
            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // Validate IdCardNo.
            //if (string.IsNullOrEmpty(user.IdCardNo))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoRequired"));
            //}
            //else
            //{
            //    //try
            //    //{
            //    //    long idCardNo = long.Parse(user.IdCardNo);
            //    //}
            //    //catch (Exception)
            //    //{
            //    //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoTypeMisMatch"));
            //    //}
            //}

            // Validate Organization.
            //if (user.Organization == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            //}
            //else
            //{
            //    // Validate Division.
            //    if (string.IsNullOrEmpty(user.TempDivisionName))
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            //    }
            //    else
            //    {
            //        //IList<SuDivisionLang> divLangList = DaoProvider.SuDivisionLangDao.FindByDivisionName(user.Organization.Organizationid, languageId, user.TempDivisionName);
            //        //if (divLangList.Count == 0)
            //        //{
            //        //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionNotExist"));
            //        //}
            //        //else
            //        //{
            //        //    user.Division = divLangList[0].Division;
            //        //}
            //    }
            //}

            // Validate DefaultLanguage.
            if (user.Language == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DefaultLanguageRequired"));
            }

            // Validate HomeTelephone.
            //if (!string.IsNullOrEmpty(user.HomePhoneNo))
            //{
            //    try
            //    {
            //        long homePhoneNo = long.Parse(user.HomePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate HomeTelephone Ext.
            //if (!string.IsNullOrEmpty(user.HomePhoneNoExt))
            //{
            //    try
            //    {
            //        int homePhoneNoExt = int.Parse(user.HomePhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate WorkingPhoneNo.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNo))
            //{
            //    try
            //    {
            //        long workPhoneNo = long.Parse(user.WorkingPhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate WorkingPhoneNo Ext.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNoExt))
            //{
            //    try
            //    {
            //        int workPhoneNoExt = int.Parse(user.WorkingPhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate FaxNo.
            //if (!string.IsNullOrEmpty(user.FaxNo))
            //{
            //    try
            //    {
            //        long faxNo = long.Parse(user.FaxNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoTypeMisMatch"));
            //    }
            //}
            // Validate FaxNo Ext.
            //if (!string.IsNullOrEmpty(user.FaxNoExt))
            //{
            //    try
            //    {
            //        int faxNoExt = int.Parse(user.FaxNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoExtTypeMisMatch"));
            //    }
            //}

            // Validate MobilePhoneNo.
            //if (!string.IsNullOrEmpty(user.MobilePhoneNo))
            //{
            //    try
            //    {
            //        long mobileNo = long.Parse(user.MobilePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("MobilePhoneNoTypeMisMatch"));
            //    }
            //}

            // Validate Email.
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EmailRequired"));
            }

            // Validate EffectiveDate and EndDate.
            //if (user.EffDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateTypeMisMatch"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateRequired"));
            //}
            //else if (user.EffDate < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateNotBeBackDate"));
            //}

            //if (user.EndDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateTypeMisMatch"));
            //}
            //else if (user.EndDate == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateRequired"));
            //}
            //else if (user.EndDate < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateNotBeBackDate"));
            //}

            //if ((user.EffDate != null) && (user.EndDate != null) &&
            //    (!user.EffDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && 
            //    (!user.EndDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)))
            //{
            //    if (user.EffDate.Value > user.EndDate.Value)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTEndDate"));
            //    }
            //}

            // Validate SetFailTime.
            if (user.SetFailTime == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("SetFailTimeTypeMisMatch"));
            }

            // Validate mapFile.
            // Check file type of mapFile
            if ((!mapFile.ContentType.Equals("image/gif")) && (!mapFile.ContentType.Equals("image/jpeg")) && (!mapFile.ContentType.Equals("image/jpg")) && (!mapFile.ContentType.Equals("image/pjpeg")))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
            }
            // Check file Size of imageFile.
            if (mapFile.ContentLength > maxFileSize)
            {
                // File size exceed 100KB.
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
            }
            // Check file Dimension of imageFile
            Image uploadImage = Image.FromStream(mapFile.InputStream);
            if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DimensionError"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            DaoProvider.SuUserDao.SaveOrUpdate(user);
        }
        public void UpdateUser(SuUser user, short languageId)
        {
            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // Validate IdCardNo.
            //if (string.IsNullOrEmpty(user.IdCardNo))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoRequired"));
            //}
            //else
            //{
            //    //try
            //    //{
            //    //    long idCardNo = long.Parse(user.IdCardNo);
            //    //}
            //    //catch (Exception)
            //    //{
            //    //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoTypeMisMatch"));
            //    //}
            //}

            // Validate Organization.
            //if (user.Organization == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            //}
            //else
            //{
            //    // Validate Division.
            //    if (string.IsNullOrEmpty(user.TempDivisionName))
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            //    }
            //    else
            //    {
            //        IList<SuDivisionLang> divLangList = DaoProvider.SuDivisionLangDao.FindByDivisionName(user.Organization.Organizationid, languageId, user.TempDivisionName);
            //        if (divLangList.Count == 0)
            //        {
            //            errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionNotExist"));
            //        }
            //        else
            //        {
            //            user.Division = divLangList[0].Division;
            //        }
            //    }
            //}

            // Validate DefaultLanguage.
            if (user.Language == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DefaultLanguageRequired"));
            }

            // Validate HomeTelephone.
            //if (!string.IsNullOrEmpty(user.HomePhoneNo))
            //{
            //    try
            //    {
            //        long homePhoneNo = long.Parse(user.HomePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate HomeTelephone Ext.
            //if (!string.IsNullOrEmpty(user.HomePhoneNoExt))
            //{
            //    try
            //    {
            //        int homePhoneNoExt = int.Parse(user.HomePhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate WorkingPhoneNo.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNo))
            //{
            //    try
            //    {
            //        long workPhoneNo = long.Parse(user.WorkingPhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate WorkingPhoneNo Ext.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNoExt))
            //{
            //    try
            //    {
            //        int workPhoneNoExt = int.Parse(user.WorkingPhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate FaxNo.
            //if (!string.IsNullOrEmpty(user.FaxNo))
            //{
            //    try
            //    {
            //        long faxNo = long.Parse(user.FaxNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoTypeMisMatch"));
            //    }
            //}
            // Validate FaxNo Ext.
            //if (!string.IsNullOrEmpty(user.FaxNoExt))
            //{
            //    try
            //    {
            //        int faxNoExt = int.Parse(user.FaxNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoExtTypeMisMatch"));
            //    }
            //}

            // Validate MobilePhoneNo.
            //if (!string.IsNullOrEmpty(user.MobilePhoneNo))
            //{
            //    try
            //    {
            //        long mobileNo = long.Parse(user.MobilePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("MobilePhoneNoTypeMisMatch"));
            //    }
            //}

            // Validate Email.
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EmailRequired"));
            }

            // Validate EffectiveDate and EndDate.
            //if (user.EffDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateTypeMisMatch"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateRequired"));
            //}
            //else if (user.EffDate < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateNotBeBackDate"));
            //}

            //if (user.EndDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateTypeMisMatch"));
            //}
            //else if (user.EndDate == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateRequired"));
            //}
            //else if (user.EndDate < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateNotBeBackDate"));
            //}

            //if ((user.EffDate != null) && (user.EndDate != null) &&
            //    (!user.EffDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && 
            //    (!user.EndDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)))
            //{
            //    if (user.EffDate.Value > user.EndDate.Value)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTEndDate"));
            //    }
            //}

            // Validate SetFailTime.
            if (user.SetFailTime == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("SetFailTimeTypeMisMatch"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            DaoProvider.SuUserDao.SaveOrUpdate(user);
        }
        #endregion

        public IList<VOUserProfile> FindUserProfileByUserName(string userName)
        {
            return DaoProvider.SuUserDao.FindUserProfileByUserName(userName);
        }
        public long AddUserProfile(SuUser user, short languageId, HttpPostedFile mapFile)
        {

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // Validate VOUserProfile
            if (string.IsNullOrEmpty(user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameRequired"));
            }
            else
            {
                if (DaoProvider.SuUserDao.FindUserProfileByUserName(user.UserName).Count > 0)
                {
                    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameAlreadyExist"));
                }
            }

            // Validate Email.
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EmailRequired"));
            }


            return DaoProvider.SuUserDao.Save(user);
        }
        public long AddUserProfile(SuUser user, short languageId)
        {
            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            // Validate UserName.
            if (string.IsNullOrEmpty(user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameRequired"));
            }
            else
            {
                if (DaoProvider.SuUserDao.FindByUserName(user.UserName).Count > 0)
                {
                    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("UserNameAlreadyExist"));
                }
            }

            // Validate Password.
            if (string.IsNullOrEmpty(user.Password))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordRequired"));
            }
            //if (string.IsNullOrEmpty(user.ConfirmPassword))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("ConfirmPasswordRequired"));
            //}
            int strMinPasswordLength = ParameterServices.MinPasswordAge; // modify by tom 03/03/2009 ParameterServices.minPasswordAge;
            string strPasswordNotAllow = ParameterServices.NotAllowPassword;

            if (user.Password.Length < strMinPasswordLength)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordLengthError"));
            }
            //if (!user.Password.Equals(user.ConfirmPassword))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordNotMatch"));
            //}
            if ((user.Password.Equals(user.UserName)) || (user.Password.Equals(strPasswordNotAllow)))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("PasswordNotAllow"));
            }

            // Validate IdCardNo.
            //if (string.IsNullOrEmpty(user.IdCardNo))
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoRequired"));
            //}
            //else
            //{
            //    try
            //    {
            //        long idCardNo = long.Parse(user.IdCardNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("IDCardNoTypeMisMatch"));
            //    }
            //}

            // Validate Organization.
            //if (user.Organization == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("OrganizationRequired"));
            //}
            //else
            //{
            //    // Validate DivisionName.
            //    if (string.IsNullOrEmpty(user.TempDivisionName))
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionRequired"));
            //    }
            //    else
            //    {
            //        IList<SuDivisionLang> divLangList = DaoProvider.SuDivisionLangDao.FindByDivisionName(user.Organization.Organizationid, languageId, user.TempDivisionName);
            //        if (divLangList.Count == 0)
            //        {
            //            errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DivisionNotExist"));
            //        }
            //        else
            //        {
            //            user.Division = divLangList[0].Division;
            //        }
            //    }
            //}

            // Validate DefaultLanguage.
            if (user.Language == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("DefaultLanguageRequired"));
            }

            // Validate HomeTelephone.
            //if (!string.IsNullOrEmpty(user.HomePhoneNo))
            //{
            //    try
            //    {
            //        long homePhoneNo = long.Parse(user.HomePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate HomeTelephone Ext.
            //if (!string.IsNullOrEmpty(user.HomePhoneNoExt))
            //{
            //    try
            //    {
            //        int homePhoneNoExt = int.Parse(user.HomePhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("HomePhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate WorkingPhoneNo.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNo))
            //{
            //    try
            //    {
            //        long workPhoneNo = long.Parse(user.WorkingPhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoTypeMisMatch"));
            //    }
            //}
            // Validate WorkingPhoneNo Ext.
            //if (!string.IsNullOrEmpty(user.WorkingPhoneNoExt))
            //{
            //    try
            //    {
            //        int workPhoneNoExt = int.Parse(user.WorkingPhoneNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("WorkingPhoneNoExtTypeMisMatch"));
            //    }
            //}

            // Validate FaxNo.
            //if (!string.IsNullOrEmpty(user.FaxNo))
            //{
            //    try
            //    {
            //        long faxNo = long.Parse(user.FaxNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoTypeMisMatch"));
            //    }
            //}
            // Validate FaxNo Ext.
            //if (!string.IsNullOrEmpty(user.FaxNoExt))
            //{
            //    try
            //    {
            //        int faxNoExt = int.Parse(user.FaxNoExt);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("FaxNoExtTypeMisMatch"));
            //    }
            //}

            // Validate MobilePhoneNo.
            //if (!string.IsNullOrEmpty(user.MobilePhoneNo))
            //{
            //    try
            //    {
            //        long mobileNo = long.Parse(user.MobilePhoneNo);
            //    }
            //    catch (Exception)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("MobilePhoneNoTypeMisMatch"));
            //    }
            //}

            // Validate Email.
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EmailRequired"));
            }

            // Validate EffectiveDate and EndDate.
            //if (user.EffDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateTypeMisMatch"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateRequired"));
            //}
            //else if (user.EffDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffDateNotBeBackDate"));
            //}

            //if (user.EndDate == null)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateTypeMisMatch"));
            //}
            //else if (user.EndDate.GetValueOrDefault(DateTime.MinValue) == DateTime.MinValue)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateRequired"));
            //}
            //else if (user.EndDate.GetValueOrDefault(DateTime.MinValue) < DateTime.Today)
            //{
            //    errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EndDateNotBeBackDate"));
            //}

            //if ((user.EffDate != null) && (user.EndDate != null) &&
            //    (!user.EffDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)) && 
            //    (!user.EndDate.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue)))
            //{
            //    if (user.EffDate.Value > user.EndDate.Value)
            //    {
            //        errors.AddError("User.Error", new Spring.Validation.ErrorMessage("EffectiveDateGTEndDate"));
            //    }
            //}

            // Validate SetFailTime.
            if (user.SetFailTime == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("SetFailTimeTypeMisMatch"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            #endregion

            // Set user.Password value by encrypt input with MD5 algorithm.
            user.Password = Encryption.Md5Hash(user.Password);
            return DaoProvider.SuUserDao.Save(user);
        }

        // save UserProfile in UserProfileEditor.ascx
        public long SaveUserProfile(SuUser user)
        {

            //comment by oum 

            //user.SetFailTime = 0;
            //user.FailTime = 0;
            //user.ChangePassword = true;

            user.UpdDate = DateTime.Now;
            user.CreDate = DateTime.Now;

            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("User Id Required"));
            }
            if (DaoProvider.SuUserDao.FindUserName(user.Userid, user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Username already exist"));
            }
            if (!user.IsAdUser && string.IsNullOrEmpty(user.Password))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Password Required"));
            }
            if (string.IsNullOrEmpty(user.PeopleID))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("People Id Required"));
            }
            if (string.IsNullOrEmpty(user.EmployeeCode))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Employee Code Required"));
            }
            if (string.IsNullOrEmpty(user.EmployeeName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Employee Name Required"));
            }
            if (user.Company == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Company Required"));
            }
            if (user.CostCenter == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Cost Center Code Required"));
            }
            if (user.Location == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Location Required"));
            }
            if (user.VendorCode.Length == 8)
            {
                user.VendorCode = "00" + user.VendorCode;
            }
            Regex regExName = new Regex(@"^([ ]?)*(([ ]?[;][ ]?)*([_.-]?[0-9a-zA-Z])*@([_.-]?[0-9,a-z,A-Z,-])*\.[a-zA-Z]{2,4})*([ ]?)+$"); //(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            Match match = regExName.Match(user.Email);
            if (!match.Success)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Email is not corrected format"));
            }

            #endregion
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            if (user.IsAdUser)
            {
                user.Password = string.Empty;
            }

            long result = DaoProvider.SuUserDao.Save(user);
            // Add Default Role for this user
            AddDefaultRole(user);

            //sync new user data to eXpense 4.7
            DaoProvider.SuUserDao.SyncNewUser();

            return result;

        }
        // update UserProfile in UserProfileEditor.ascx
        public void UpdateUserProfile(SuUser user)
        {
            #region Validate SuUser
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("User Id Required"));
            }
            if (DaoProvider.SuUserDao.FindUserName(user.Userid, user.UserName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Username already exist"));
            }
            if (!user.IsAdUser && string.IsNullOrEmpty(user.Password))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Password Required"));
            }
            if (string.IsNullOrEmpty(user.PeopleID))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("People Id Required"));
            }
            if (string.IsNullOrEmpty(user.EmployeeCode))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Employee Code Required"));
            }
            if (string.IsNullOrEmpty(user.EmployeeName))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Employee Name Required"));
            }
            if (user.Company == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Company Required"));
            }
            if (user.CostCenter == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Cost Center Code Required"));
            }
            if (user.Location == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Location Required"));
            }
            if (user.VendorCode.Length == 8)
            {
                user.VendorCode = "00" + user.VendorCode;
            }
            Regex regExName = new Regex(@"^([ ]?)*(([ ]?[;][ ]?)*([_.-]?[0-9a-zA-Z])*@([_.-]?[0-9,a-z,A-Z,-])*\.[a-zA-Z]{2,4})*([ ]?)+$"); //(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            Match match = regExName.Match(user.Email);
            if (!match.Success)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("Email is not corrected format"));
            }
            #endregion

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            if (user.IsAdUser)
            {
                user.Password = string.Empty;
            }
            DaoProvider.SuUserDao.SaveOrUpdate(user);

            //sync update user data to eXpense 4.7
            DaoProvider.SuUserDao.SyncUpdateUser(user.UserName);
        }

        public string Forgetpassword(string UserName)
        {
            SuUser User = QueryProvider.SuUserQuery.FindSuUserByUserName(UserName);

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (User == null)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("CAN NOT FIND USERNAME"));
            }
            else if (User.IsAdUser)
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("CannotResetPassword"));
            }
            else if (string.IsNullOrEmpty(User.Email))
            {
                errors.AddError("User.Error", new Spring.Validation.ErrorMessage("CAN NOT FIND EMAIL"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            string UserPassword = GeneratePassword();
            User.Password = Encryption.Md5Hash(UserPassword);
            User.ChangePassword = true;
            Update(User);

            //sync update user data to eXpense 4.7
            DaoProvider.SuUserDao.SyncUpdateUser(User.UserName);

            return UserPassword;
        }

        private string GeneratePassword()
        {
            //modify by meaw change maximum password length to parameter and set unuse symbol for generate password
            PasswordGeneration passwordGeneration = new PasswordGeneration();
            passwordGeneration.IncludeThaiLetters = false;
            passwordGeneration.IncludeSpecial = false;
            passwordGeneration.MaximumLength = ParameterServices.MaxPasswordLength;
            passwordGeneration.MinimumLength = ParameterServices.MinPasswordLength;

            return passwordGeneration.Create();
        }

        private void AddDefaultRole(SuUser user)
        {
            //สร้าง default userrole ให้กับ useraccoun ที่มาใหม่ หรือไม่มี role จ๊ะ
            //By lerm +
            SuUserRole userRole = new SuUserRole();
            userRole.Active = true;
            userRole.Comment = "Automatic default userrole adder.";
            userRole.CreBy = 1;
            userRole.CreDate = DateTime.Now.Date;
            userRole.Role = SuRoleService.FindByIdentity(
                (short)ParameterServices.DefaultUserRoleID);
            userRole.User = FindByIdentity(user.Userid);
            userRole.UpdBy = 1;
            userRole.UpdDate = DateTime.Now.Date;
            userRole.UpdPgm = user.UpdPgm;
            long result = SuUserRoleService.Save(userRole);
            if (result < 0)
            {
                throw new Exception("Can't assign default role.");
            }
        }

        public bool IsPrivilege(SuUser user)
        {
            return DaoProvider.SuUserDao.IsPrivilege(user);
        }
    }
}
