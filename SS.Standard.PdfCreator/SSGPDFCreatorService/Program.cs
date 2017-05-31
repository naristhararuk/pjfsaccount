﻿using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace SSG.PDF.Service
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
//#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new PDFCreatorService() 
			};
            ServiceBase.Run(ServicesToRun);
//#else
//            PDFCreatorService oService = new PDFCreatorService();
//            oService.DoProcess(null);
//#endif
        }
    }
}
