using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO.ValueObject
{
    [Serializable]
    public class SuRTENodeSearchResult
    {
        #region Field
        private short nodeId;
        private short nodeHeaderId;
        private short nodeOrderNo;
        private short languageId;
        private string nodeType;
        private string imagePath;
        private string header;
        private string content;
        private string comment;
        private bool active;
        #endregion

        #region Constructor
        public SuRTENodeSearchResult()
		{
		
		}
		#endregion

        #region Property
        public virtual short NodeId
        {
            get { return this.nodeId; }
            set { this.nodeId = value; }
        }
        public virtual short NodeHeaderId
        {
            get { return this.nodeHeaderId; }
            set { this.nodeHeaderId = value; }
        }
        public virtual short LanguageId
        {
            get { return this.languageId; }
            set { this.languageId = value; }
        }
        public virtual short NodeOrderNo
        {
            get { return this.nodeOrderNo; }
            set { this.nodeOrderNo = value; }
        }
        public virtual string NodeType
        {
            get { return this.nodeType; }
            set { this.nodeType = value; }
        }
        public virtual string ImagePath
        {
            get { return this.imagePath; }
            set { this.imagePath = value; }
        }
        public virtual string Header
        {
            get { return this.header; }
            set { this.header = value; }
        }
        public virtual string Content
        {
            get { return this.content; }
            set { this.content = value; }
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
