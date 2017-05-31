using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    [Serializable]
    public partial class DbSapInstance
    {
        private string code;
        public virtual string Code
        {
            get { return code; }
            set { code = value; }
        }

        private string aliasName;
        public virtual string AliasName
        {
            get { return aliasName; }
            set { aliasName = value; }
        }

        private string systemID;
        public virtual string SystemID
        {
            get { return systemID; }
            set { systemID = value; }
        }
        

        private string client;
        public virtual string Client
        {
            get { return client; }
            set { client = value; }
        }

        private string username;
        public virtual string UserName
        {
            get { return username; }
            set { username = value; }
        }

        private string password;
        public virtual string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string language;
        public virtual string Language
        {
            get { return language; }
            set { language = value; }
        }

        private string systemNumber;
        public virtual string SystemNumber
        {
            get { return systemNumber; }
            set { systemNumber = value; }
        }

        private string msgServerHost;
        public virtual string MsgServerHost
        {
            get { return msgServerHost; }
            set { msgServerHost = value; }
        }

        private string logonGroup;
        public virtual string LogonGroup
        {
            get { return logonGroup; }
            set { logonGroup = value; }
        }

        private string userCPIC;
        public virtual string UserCPIC
        {
            get { return userCPIC; }
            set { userCPIC = value; }
        }

        private string docTypeExpPostingDM;
        public virtual string DocTypeExpPostingDM
        {
            get { return docTypeExpPostingDM; }
            set { docTypeExpPostingDM = value; }
        }

        private string docTypeExpRmtPostingDM;
        public virtual string DocTypeExpRmtPostingDM
        {
            get { return docTypeExpRmtPostingDM; }
            set { docTypeExpRmtPostingDM = value; }
        }

        private string docTypeExpPostingFR;
        public virtual string DocTypeExpPostingFR
        {
            get { return docTypeExpPostingFR; }
            set { docTypeExpPostingFR = value; }
        }

        private string docTypeExpRmtPostingFR;
        public virtual string DocTypeExpRmtPostingFR
        {
            get { return docTypeExpRmtPostingFR; }
            set { docTypeExpRmtPostingFR = value; }
        }

        private string docTypeExpICPostingFR;
        public virtual string DocTypeExpICPostingFR
        {
            get { return docTypeExpICPostingFR; }
            set { docTypeExpICPostingFR = value; }
        }

        private string docTypeAdvancePostingDM;
        public virtual string DocTypeAdvancePostingDM
        {
            get { return docTypeAdvancePostingDM; }
            set { docTypeAdvancePostingDM = value; }
        }

        private string docTypeAdvancePostingFR;
        public virtual string DocTypeAdvancePostingFR
        {
            get { return docTypeAdvancePostingFR; }
            set { docTypeAdvancePostingFR = value; }
        }

        private string docTypeRmtPosting;
        public virtual string DocTypeRmtPosting
        {
            get { return docTypeRmtPosting; }
            set { docTypeRmtPosting = value; }
        }

        private string docTypeFixedAdvancePosting;
        public virtual string DocTypeFixedAdvancePosting
        {
            get { return docTypeFixedAdvancePosting; }
            set { docTypeFixedAdvancePosting = value; }
        }/*DocTypeFixedAdvance ตอน Query*/

        private string docTypeFixedAdvanceReturnPosting;
        public virtual string DocTypeFixedAdvanceReturnPosting 
        {
            get { return docTypeFixedAdvanceReturnPosting; }
            set { docTypeFixedAdvanceReturnPosting = value; }
        }/*DocTypeFixedAdvanceReturn ตอน Query*/

        private long updBy;
        public virtual long UpdBy
        {
            get { return this.updBy; }
            set { this.updBy = value; }
        }

        private DateTime updDate;
        public virtual DateTime UpdDate
        {
            get { return this.updDate; }
            set { this.updDate = value; }
        }
        
        private long creBy;
        public virtual long CreBy
        {
            get { return this.creBy; }
            set { this.creBy = value; }
        }

        private DateTime creDate;
        public virtual DateTime CreDate
        {
            get { return this.creDate; }
            set { this.creDate = value; }
        }
        
        private string updPgm;
        public virtual string UpdPgm
        {
            get { return this.updPgm; }
            set { this.updPgm = value; }
        }
        
        private byte[] rowVersion;
        public virtual byte[] RowVersion
        {
            get { return this.rowVersion; }
            set { this.rowVersion = value; }
        }
        public DbSapInstance()
		{
		}

        public DbSapInstance(string code)
		{
            this.code = code;
		}
    }
}
