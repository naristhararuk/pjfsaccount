using System;
using System.Collections.Generic;
using System.Linq;
using Spring.Context.Support;
using Spring.Objects.Factory;
using System.Text;
using System.Runtime.CompilerServices;

namespace SCG.eAccounting.Interface.Utilities
{
    public class ObjectManager
    {
        static IObjectFactory factory;


        private static void  ObjectManager2()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string Dao = appPath + @"Configurations\Dao.xml";
            string Engine = appPath + @"Configurations\Engine.xml";
            string Query = appPath + @"Configurations\Query.xml";
            //string SAPBapi = appPath + @"Configurations\SAPBapi.xml";
            string Services = appPath + @"Configurations\Services.xml";
            string WorkFlow = appPath + @"Configurations\WorkFlow.xml";
            string DatabaseConfig = appPath + "Db.xml";
            //string Log = appPath + @"Configurations\Log4Net.xml";
            string[] config = new string[] { Dao, Engine, Query, Services, WorkFlow, DatabaseConfig };
            XmlApplicationContext context = new XmlApplicationContext(config);
           
            Spring.Context.Support.ContextRegistry.RegisterContext(context);
            factory = context;

    
        }

        public static object GetObject(string objectName)
        {
            if (factory == null)
	        {
                ObjectManager2();
            }
            return factory.GetObject(objectName);


        }
    }
}
