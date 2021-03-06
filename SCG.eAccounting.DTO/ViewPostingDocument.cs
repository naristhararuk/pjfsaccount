﻿using System;
using SCG.DB.DTO;
using SS.SU.DTO;
using SS.Standard.WorkFlow.DTO;
using SCG.eAccounting.DTO.DataSet;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;

namespace SCG.eAccounting.DTO
{
    /// <summary>
    /// POJO for SCGDocument. This class is autogenerated
    /// </summary>
    [Serializable]
    public partial class ViewPostingDocument
    {
        #region Fields
        private long    documentID;
        private string  documentNo;
        private string  postingStatus;
        #endregion Fields

        #region Properties
        public virtual long DocumentID
        {
            get { return this.documentID; }
            set { this.documentID = value; }
        }
        public virtual string DocumentNo
        {
            get { return this.documentNo; }
            set { this.documentNo = value; }
        }
        public virtual string PostingStatus
        {
            get { return this.postingStatus; }
            set { this.postingStatus = value; }
        }
        #endregion Properties
    }
}
