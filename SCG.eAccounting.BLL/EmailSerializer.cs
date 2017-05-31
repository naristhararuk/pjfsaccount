using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using SS.Standard.CommunicationService.DTO;

namespace SCG.eAccounting.BLL
{
    public class EmailSerializer
    {
        public EmailSerializer()
        {
        }

        public string
            SerializeObject(EmailDTO objectToSerialize)
        {

            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(EmailDTO));
            xml.Serialize(stream, objectToSerialize);
            byte[] buffer = stream.GetBuffer();
            stream.Close();
            UTF8Encoding encoding = new UTF8Encoding();
            string slzString = encoding.GetString(buffer);
            return slzString;
        }

        public EmailDTO DeSerializeObject(string content)
        {
            EmailDTO objectToSerialize;
            Byte[] a = UnicodeEncoding.UTF8.GetBytes(content);
            MemoryStream stream = new MemoryStream(a);
            //Stream stream = File.Open("D:/EXP/test.xml", FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(EmailDTO));
            objectToSerialize = (EmailDTO)xml.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}
