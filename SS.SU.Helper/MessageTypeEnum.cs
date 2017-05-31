using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.Helper
{

    [Serializable]
    public partial class MessageTypeEnum
    {
        public enum AlertMessage
        {
            Information,
            Confirmation,
            Error,
            Alert
        }
    }
}
