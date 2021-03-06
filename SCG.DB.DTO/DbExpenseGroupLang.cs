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
    /// POJO for DbExpenseGroupLang. This class is autogenerated
    /// </summary>
    [Serializable]
    public partial class DbExpenseGroupLang
    {
        #region Fields
        private long expenseGroupLangID;
        private string description;
        private string comment;
        private bool active;
        private long updBy;
        private DateTime updDate;
        private long creBy;
        private DateTime creDate;
        private string updPgm;
        private byte[] rowVersion;
        private SCG.DB.DTO.DbExpenseGroup expenseGroupID;
        private SS.DB.DTO.DbLanguage languageID;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DbExpenseGroupLang class
        /// </summary>
        public DbExpenseGroupLang()
        {
        }

        public DbExpenseGroupLang(long expenseGroupLangID)
        {
            this.expenseGroupLangID = expenseGroupLangID;
        }

        /// <summary>
        /// Initializes a new instance of the DbExpenseGroupLang class
        /// </summary>
        /// <param name="description">Initial <see cref="DbExpenseGroupLang.Description" /> value</param>
        /// <param name="comment">Initial <see cref="DbExpenseGroupLang.Comment" /> value</param>
        /// <param name="updBy">Initial <see cref="DbExpenseGroupLang.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="DbExpenseGroupLang.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="DbExpenseGroupLang.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="DbExpenseGroupLang.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="DbExpenseGroupLang.UpdPgm" /> value</param>
        /// <param name="rowVersion">Initial <see cref="DbExpenseGroupLang.RowVersion" /> value</param>
        /// <param name="active">Initial <see cref="DbExpenseGroupLang.Active" /> value</param>
        /// <param name="expenseGroupID">Initial <see cref="DbExpenseGroup.expenseGroupID" /> value</param>
        /// <param name="languageID">Initial <see cref="DbLanguage.LanguageID" /> value</param>
        public DbExpenseGroupLang(string description, string comment, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, byte[] rowVersion, bool active, SCG.DB.DTO.DbExpenseGroup expenseGroupID, SS.DB.DTO.DbLanguage languageID)
        {
            this.description = description;
            this.comment = comment;
            this.updBy = updBy;
            this.updDate = updDate;
            this.creBy = creBy;
            this.creDate = creDate;
            this.updPgm = updPgm;
            this.rowVersion = rowVersion;
            this.active = active;
            this.expenseGroupID = expenseGroupID;
            this.languageID = languageID;
        }

        /// <summary>
        /// Minimal constructor for class DbExpenseGroupLang
        /// </summary>
        /// <param name="updBy">Initial <see cref="DbExpenseGroupLang.UpdBy" /> value</param>
        /// <param name="updDate">Initial <see cref="DbExpenseGroupLang.UpdDate" /> value</param>
        /// <param name="creBy">Initial <see cref="DbExpenseGroupLang.CreBy" /> value</param>
        /// <param name="creDate">Initial <see cref="DbExpenseGroupLang.CreDate" /> value</param>
        /// <param name="updPgm">Initial <see cref="DbExpenseGroupLang.UpdPgm" /> value</param>
        /// <param name="active">Initial <see cref="DbExpenseGroupLang.Active" /> value</param>
        public DbExpenseGroupLang(long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm, bool active)
        {
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
        /// Gets or sets the expenseGroupLangID for the current DbExpenseGroupLang
        /// </summary>
        public virtual long ExpenseGroupLangID
        {
            get { return this.expenseGroupLangID; }
            set { this.expenseGroupLangID = value; }
        }

        /// <summary>
        /// Gets or sets the Description for the current DbExpenseGroupLang
        /// </summary>
        public virtual string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// Gets or sets the Comment for the current DbExpenseGroupLang
        /// </summary>
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /// <summary>
        /// Gets or sets the UpdBy for the current DbExpenseGroupLang
        /// </summary>
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        /// <summary>
        /// Gets or sets the UpdDate for the current DbExpenseGroupLang
        /// </summary>
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }

        /// <summary>
        /// Gets or sets the CreBy for the current DbExpenseGroupLang
        /// </summary>
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        /// <summary>
        /// Gets or sets the CreDate for the current DbExpenseGroupLang
        /// </summary>
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }

        /// <summary>
        /// Gets or sets the UpdPgm for the current DbExpenseGroupLang
        /// </summary>
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }

        /// <summary>
        /// Gets or sets the RowVersion for the current DbExpenseGroupLang
        /// </summary>
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Active for the current DbExpenseGroupLang
        /// </summary>
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Gets or sets the ExpenseGroupID for the current DbExpenseGroup
        /// </summary>
        public virtual SCG.DB.DTO.DbExpenseGroup ExpenseGroupID
        {
            get { return this.expenseGroupID; }
            set { this.expenseGroupID = value; }
        }

        /// <summary>
        /// Gets or sets the LanguageID for the current DbLanguage
        /// </summary>
        public virtual SS.DB.DTO.DbLanguage LanguageID
        {
            get { return this.languageID; }
            set { this.languageID = value; }
        }

        #endregion
    }
}
