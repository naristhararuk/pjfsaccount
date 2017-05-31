using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.CommunicationService.DTO;
using System.IO;
using System.Xml.Serialization;
using SS.Standard.CommunicationService.Implement;

namespace SS.Standard.CommunicationService
{
    public class SMSSerializer
    {
        public string
            SerializeObject(SMSContainer objectToSerialize)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(SMSContainer));
            xml.Serialize(stream, objectToSerialize);
            byte[] buffer = stream.GetBuffer();
            stream.Close();
            UTF8Encoding encoding = new UTF8Encoding();
            string slzString = encoding.GetString(buffer);
            return slzString;
        }

        public SMSContainer DeSerializeObject(string content)
        {
            SMSContainer objectToSerialize;
            Byte[] a = UnicodeEncoding.UTF8.GetBytes(content);
            MemoryStream stream = new MemoryStream(a);
            XmlSerializer xml = new XmlSerializer(typeof(SMSContainer));
            objectToSerialize = (SMSContainer)xml.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}
