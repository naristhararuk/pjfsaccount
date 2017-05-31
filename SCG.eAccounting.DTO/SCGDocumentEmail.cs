using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.DTO
{
    public class SCGDocumentEmail
    {
        private long documentID;

        public long DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }

        private DateTime documentDate;

        public DateTime DocumentDate
        {
            get { return documentDate; }
            set { documentDate = value; }
        }

        private string employeeName;

        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        private long userId;

        public long UserID
        {
            get { return userId; }
            set { userId = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private long cacheWorkflowID;

        public long CacheWorkflowID
        {
            get { return cacheWorkflowID; }
            set { cacheWorkflowID = value; }
        }

        private string approverEmail;

        public string ApproverEmail
        {
            get { return approverEmail; }
            set { approverEmail = value; }
        }

    }
}
