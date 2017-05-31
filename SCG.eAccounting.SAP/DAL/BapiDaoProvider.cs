using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.DAL.Interface;

namespace SCG.eAccounting.SAP.DAL
{
    public class BapiDaoProvider
    {
        public static IBapiacap09Dao    Bapiacap09Dao   { get; set; }
        public static IBapiacar09Dao    Bapiacar09Dao   { get; set; }
        public static IBapiaccr09Dao    Bapiaccr09Dao   { get; set; }
        public static IBapiacextcDao    BapiacextcDao   { get; set; }
        public static IBapiacgl09Dao    Bapiacgl09Dao   { get; set; }
        public static IBapiache09Dao    Bapiache09Dao   { get; set; }
        public static IBapiacpa09Dao    Bapiacpa09Dao   { get; set; }
        public static IBapiactx09Dao    Bapiactx09Dao   { get; set; }
        public static IBapiret2Dao      Bapiret2Dao     { get; set; }
        public static IBapirunningDao   BapirunningDao  { get; set; }
        public static IBapireverseDao   BapireverseDao  { get; set; }
    }
}
