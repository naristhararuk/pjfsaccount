using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class SearchResultData
    {
        public SearchResultData() 
        { 
        }
        
        public int Seq { get; set; }
        public long DocumentID { get; set; }
        public long WorkflowID { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentTypeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Amount { get; set; }
        public string DocumentStatus { get; set; }
        public DateTime ReceiveDocumentDate { get; set; }
        public string Creator
        {
            get { return String.Format("{0} {1}", FirstName,LastName); }
        }

        #region Inbox
        public string RequestNo { get; set; }
        public string ReferenceNo { get; set; }
        public int WorkFlowStateID { get; set; }
        public int WorkFlowTypeID { get; set; }
        public long DocumentTypeID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Subject { get; set; }
        public long CreatorID { get; set; }
        public string CreatorName { get; set; }
        public long RequesterID { get; set; }
        public string RequesterName { get; set; }
        public long ApproverID { get; set; }
        public string ApproverName { get; set; }
        public long InitiatorID { get; set; }
        public string InitiatorName { get; set; }
        public long UserID { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int? TaskGroup { get; set; }
        public int ItemCount { get; set; }
        public string StateName { get; set; }
        public double AmountMainCurrency { get; set; }
        public double AmountLocalCurrency { get; set; }
        public DateTime? ApproveDate { get; set; }
        #endregion Inbox
    }
}
