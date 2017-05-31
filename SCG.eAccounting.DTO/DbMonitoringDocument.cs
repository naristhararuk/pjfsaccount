using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO
{
    public class DbMonitoringDocument
    {
        private string documentNo;

        public string DocumentNo
        {
            get { return documentNo; }
            set { documentNo = value; }
        }

        private string referenceNo;

        public string ReferenceNo
        {
            get { return referenceNo; }
            set { referenceNo = value; }
        }

        private DateTime? documentDate;

        public DateTime? DocumentDate
        {
            get { return documentDate; }
            set { documentDate = value; }
        }

        private string cacheCurrentStateName;

        public string CacheCurrentStateName
        {
            get { return cacheCurrentStateName; }
            set { cacheCurrentStateName = value; }
        }

        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string cacheCreatorName;

        public string CacheCreatorName
        {
            get { return cacheCreatorName; }
            set { cacheCreatorName = value; }
        }

        private string cacheRequesterName;

        public string CacheRequesterName
        {
            get { return cacheRequesterName; }
            set { cacheRequesterName = value; }
        }

        private Double? cacheAmountLocalCurrency;

        public Double? CacheAmountLocalCurrency
        {
            get { return cacheAmountLocalCurrency; }
            set { cacheAmountLocalCurrency = value; }
        }

        private Double? cacheAmountMainCurrency;

        public Double? CacheAmountMainCurrency
        {
            get { return cacheAmountMainCurrency; }
            set { cacheAmountMainCurrency = value; }
        }

        private Double? cacheAmountTHB;

        public Double? CacheAmountTHB
        {
            get { return cacheAmountTHB; }
            set { cacheAmountTHB = value; }
        }

        private DateTime? approveDate;

        public DateTime? ApproveDate
        {
            get { return approveDate; }
            set { approveDate = value; }
        }

        private long? attachmentID;

        public long? AttachmentID
        {
            get { return attachmentID; }
            set { attachmentID = value; }
        }

        private long workflowID;

        public long WorkflowID
        {
            get { return workflowID; }
            set { workflowID = value; }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private Boolean cacheAttachment;

        public Boolean CacheAttachment
        {
            get { return cacheAttachment; }
            set { cacheAttachment = value; }
        }
        private string cacheBoxID;

        public string CacheBoxID
        {
            get { return cacheBoxID; }
            set { cacheBoxID = value; }
        }

        private DateTime? receiveDocumentDate;

        public DateTime? ReceiveDocumentDate
        {
            get { return receiveDocumentDate; }
            set { receiveDocumentDate = value; }
        }

        private Boolean isVerifyWithImage;

        public Boolean IsVerifyWithImage
        {
            get { return isVerifyWithImage; }
            set { isVerifyWithImage = value; }
        }
    }
}
