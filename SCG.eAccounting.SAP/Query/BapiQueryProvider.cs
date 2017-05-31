using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.Query.Interface;

namespace SCG.eAccounting.SAP.Query
{
    public class BapiQueryProvider
    {
        public static IBapiacap09Query      Bapiacap09Query     { get; set; }
        public static IBapiacar09Query      Bapiacar09Query     { get; set; }
        public static IBapiaccr09Query      Bapiaccr09Query     { get; set; }
        public static IBapiacextcQuery      BapiacextcQuery     { get; set; }
        public static IBapiacgl09Query      Bapiacgl09Query     { get; set; }
        public static IBapiache09Query      Bapiache09Query     { get; set; }
        public static IBapiacpa09Query      Bapiacpa09Query     { get; set; }
        public static IBapiactx09Query      Bapiactx09Query     { get; set; }
        public static IBapiret2Query        Bapiret2Query       { get; set; }
        public static IBapirunningQuery     BapirunningQuery    { get; set; }
        public static IBapireverseQuery     BapireverseQuery    { get; set; }
    }
}
