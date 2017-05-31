using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace SS.Standard.Utilities
{
    public class ScheduleManager
    {

        public ScheduleManager()
        {

        }

        /// <summary>
        /// Windows Schedule service, for troubleshoote contact to lerm+.
        /// </summary>
        /// <param name="adminUsername">username remote computer</param>
        /// <param name="adminPassword">pasword</param>
        /// <param name="programFilePath">full file path with extension</param>
        /// <param name="dateTime">date time to execute</param>
        public static void CreateOnceSchedule(string adminUsername, string adminPassword, string programFilePath, DateTime dateTime, string programPath)
        {
            //StreamWriter sw = new StreamWriter("SSShedule.bat");
            string command;
            string commandFormat;

            //Delete already task.
            command = "/Delete /TN SSS /F";

            System.Diagnostics.Process.Start(programPath, command);

            //Duration for machanism waiting.
            Thread.Sleep(3000);

            //Add new schedule.
            commandFormat = "/Create /RU {0} /RP {1} /SC ONCE /TN {2} /TR {3} /ST {4}";
            command = string.Format(commandFormat, adminUsername, adminPassword, "SSS", programFilePath, dateTime.ToString("HH:mm:ss"));

            System.Diagnostics.Process.Start(programPath, command);
        }

    }
}
