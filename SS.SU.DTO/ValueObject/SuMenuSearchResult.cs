using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    [Serializable]
    public class SuMenuSearchResult
    {
        #region Constructor
        public SuMenuSearchResult()
        {

        }
        #endregion

        #region Field
        private string menuMainCode;
        private short languageId;
        private string menuName;

        private short menuId;
        private string menuCode;
        private short programid;
        private short menuMainid;
        private short menuLevel;
        private short menuSeq;
        private string comment;
        private bool active;

        #endregion

        #region Property
        public virtual string MenuMainCode
        {
            get { return this.menuMainCode; }
            set { this.menuMainCode = value; }
        }
        public virtual short LanguageId
        {
            get { return this.languageId; }
            set { this.languageId = value; }
        }
        public virtual string MenuName
        {
            get { return this.menuName; }
            set { this.menuName = value; }
        }
        public virtual short MenuId
        {
            get { return this.menuId; }
            set { this.menuId = value; }
        }
        public virtual string MenuCode
        {
            get { return this.menuCode; }
            set { this.menuCode = value; }
        }
        public virtual short ProgramId
        {
            get { return this.programid; }
            set { this.programid = value; }
        }
        public virtual short MenuMainId
        {
            get { return this.menuMainid; }
            set { this.menuMainid = value; }
        }
        public virtual short MenuLevel
        {
            get { return this.menuLevel; }
            set { this.menuLevel = value; }
        }
        public virtual short MenuSeq
        {
            get { return this.menuSeq; }
            set { this.menuSeq = value; }
        }
        public virtual string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }
        #endregion
    }
}