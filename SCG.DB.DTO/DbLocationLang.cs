﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SCG.DB.DTO
{
    /// <summary>
    /// POJO for DbLocationLang. This class is autogenerated
    /// </summary>
    [Serializable]
    public partial class DbLocationLang
    {
        #region Fields

        private long locationLangID;
        private string locationName;
        private string comment;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private Byte[] rowVersion;
        private bool active;
        private SS.DB.DTO.DbLanguage languageID;
        private SCG.DB.DTO.DbLocation locationID;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DbLocationLang class
        /// </summary>
        public DbLocationLang()
        {
        }

        public DbLocationLang(long locationLangID)
        {
            this.locationLangID = locationLangID;
        }

        /// <summary>
        /// Initializes a new instance of the DbLocationLang class
        /// </summary>
        /// <param name="locationName">Initial <see cref="DbLocationLang.LocationName" /> value</param>
        /// <param name="comment">Initial <see cref="DbLocationLang.Comment" /> value</param>
        /// <param name="updBy">Initial <see cref="DbLocationLang.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="DbLocationLang.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="DbLocationLang.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="DbLocationLang.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="DbLocationLang.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="DbLocationLang.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="DbLocationLang.Active" /> value</param>
        /// <param name="languageID">Initial <see cref="DbLanguage.LanguageID" /> value</param>
        /// <param name="loactionID">Initial <see cref="DbLocation.LoactionID" /> value</param>
        public DbLocationLang(string locationName, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, Byte[] rowVersion, bool active, SS.DB.DTO.DbLanguage languageID, SCG.DB.DTO.DbLocation locationID)
        {
            this.locationName = locationName;
            this.comment = comment;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.active = active;
            this.languageID = languageID;
            this.locationID = locationID;
        }

        /// <summary>
        /// Minimal constructor for class DbLocationLang
        /// </summary>
        /// <param name="locationName">Initial <see cref="DbLocationLang.LocationName" /> value</param>
        /// <param name="updBy">Initial <see cref="DbLocationLang.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="DbLocationLang.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="DbLocationLang.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="DbLocationLang.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="DbLocationLang.UpdPgm" /> value</param>
        /// <param name="active">Initial <see cref="DbLocationLang.Active" /> value</param>
        public DbLocationLang(string locationName, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
        {
            this.locationName = locationName;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.active = active;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the LocationLangID for the current DbLocationLang
        /// </summary>
        public virtual long LocationLangID
        {
            get { return this.locationLangID; }
            set { this.locationLangID = value; }
        }

        /// <summary>
        /// Gets or sets the LocationName for the current DbLocationLang
        /// </summary>
        public virtual string LocationName
        {
            get { return this.locationName; }
            set { this.locationName = value; }
        }

        /// <summary>
        /// Gets or sets the Comment for the current DbLocationLang
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current DbLocationLang
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current DbLocationLang
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current DbLocationLang
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current DbLocationLang
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current DbLocationLang
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current DbLocationLang
        /// </summary>
        public virtual Byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current DbLocationLang
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the LanguageID for the current DbLanguage
        /// </summary>
        public virtual SS.DB.DTO.DbLanguage LanguageID
        {
            get { return this.languageID; }
            set { this.languageID = value; }
        }

        /// <summary>
        /// Gets or sets the LocationID for the current DbLocation
        /// </summary>
        public virtual SCG.DB.DTO.DbLocation LocationID
        {
            get { return this.locationID; }
            set { this.locationID = value; }
        }

        #endregion
    }
}
