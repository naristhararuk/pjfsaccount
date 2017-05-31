using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace SCG.eAccounting.AppService
{
    [ServiceContract]
    public interface ISmsWcfService
    {
        [OperationContract]
        void ReceiveSMS(string from, string to, string content, string transID);
    }
}
