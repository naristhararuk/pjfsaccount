using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.GL.DTO.ValueObject
{
    [Serializable]
    public partial class DbLanguageSearchResult
    {
        #region Fields

        private short languageid;
        private string languageName;
        private string languageCode;
        private string imagePath;
        private string comment;
        private bool active;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Languageid for the current SuLanguage
        /// </summary>
        public virtual short Languageid
        {
            get { return this.languageid; }
            set { this.languageid = value; }
        }

        /// <summary>
        /// Gets or sets the LanguageName for the current SuLanguage
        /// </summary>
        public virtual string LanguageName
        {
            get { return this.languageName; }
            set { this.languageName = value; }
        }

        /// <summary>
        /// Gets or sets the LanguageCode for the current SuLanguage
        /// </summary>
        public virtual string LanguageCode
        {
            get { return this.languageCode; }
            set { this.languageCode = value; }
        }

        /// <summary>
        /// Gets or sets the ImagePath for the current SuLanguage
        /// </summary>
        public virtual string ImagePath
        {
            get { return this.imagePath; }
            set { this.imagePath = value; }
        }

        /// <summary>
        /// Gets or sets the Comment for the current SuLanguage
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current SuLanguage
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        #endregion
    }
}
