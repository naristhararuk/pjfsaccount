using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    [Serializable]
    public class SuRTEContentSearchResult
    {
        #region Field
        private short nodeId;
        private short languageId;
        private string languageName;
        private short contentId;
        private string header;
        private string content;
        private string comment;
        private bool active;
        #endregion

        #region Constructor
        public SuRTEContentSearchResult()
		{
		
		}
		#endregion

        #region Property
        public virtual short NodeId
        {
            get { return nodeId; }
            set { this.nodeId = value; }
        }
        public virtual short LanguageId
        {
            get { return languageId; }
            set { this.languageId = value; }
        }
        public virtual string LanguageName
        {
            get { return languageName; }
            set { this.languageName = value; }
        }
        public virtual short ContentId
        {
            get { return contentId; }
            set { this.contentId = value; }
        }
        public virtual string Header
        {
            get { return header; }
            set { this.header = value; }
        }
        public virtual string Content
        {
            get { return content; }
            set { this.content = value; }
        }
        public virtual string Comment
        {
            get { return comment; }
            set { this.comment = value; }
        }
        public virtual bool Active
        {
            get { return active; }
            set { this.active = value; }
        }
        #endregion
    }
}
