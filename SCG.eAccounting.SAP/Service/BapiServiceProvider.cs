using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.Service.Interface;

namespace SCG.eAccounting.SAP.Service
{
    public class BapiServiceProvider
    {
        public static IBapiacap09Service    Bapiacap09Service   { get; set; }
        public static IBapiacar09Service    Bapiacar09Service   { get; set; }
        public static IBapiaccr09Service    Bapiaccr09Service   { get; set; }
        public static IBapiacextcService    BapiacextcService   { get; set; }
        public static IBapiacgl09Service    Bapiacgl09Service   { get; set; }
        public static IBapiache09Service    Bapiache09Service   { get; set; }
        public static IBapiacpa09Service    Bapiacpa09Service   { get; set; }
        public static IBapiactx09Service    Bapiactx09Service   { get; set; }
        public static IBapiret2Service      Bapiret2Service     { get; set; }
        public static IBapirunningService   BapirunningService  { get; set; }
        public static IBapireverseService   BapireverseService  { get; set; }
    }
}
