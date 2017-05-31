using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
    [Serializable]
    public partial class SuUserLoginToken
    {
        private Int64 userId;
        public virtual Int64 UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        private string userName;
        public virtual string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        private string token;
        public virtual string Token
        {
            get { return this.token; }
            set { this.token = value; }
        }
    }
}
