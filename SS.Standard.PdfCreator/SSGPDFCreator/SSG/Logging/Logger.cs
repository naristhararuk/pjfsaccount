using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml;

using SSG.Config;

namespace SSG.Logging
{

    /// <summary>
    /// Looger class for logging messages into the log file.
    /// This class can specific a kind of message type into the log.
    /// </summary>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    public class Logger
    {
        #region Constant declaration

            /// <summary>
            /// Logging message type
            /// </summary>
            public enum LogType
            {
                /// <summary>
                /// Not specific message type
                /// </summary>
                NONE = 0,

                /// <summary>
                /// Information message type
                /// </summary>
                INFO = 1,   
    
                /// <summary>
                /// Warning message type
                /// </summary>
                WARNING = 2,   
 
                /// <summary>
                /// Error message type
                /// </summary>
                ERROR = 3,     
 
                /// <summary>
                /// Debug message type
                /// </summary>
                DEBUG = 4       
            }

        #endregion

        #region Variable declaration

            // Straem writer for writting message into log file.
            private static StreamWriter m_oPiStreamWriter = null;
            // Keep the configuration values.
            private static SSGConfig m_oPiConfig;

        #endregion

        #region constructor

            /// <summary>
            /// Gettering the configuration values when constructor has created. 
            /// </summary>
            public Logger()
            {
                m_oPiConfig = SSGConfigFactory.GetConfig();
            }

        #endregion

        #region Methods

            /// <summary>
            /// Gettering location of the system log filename from the configuration object.
            /// </summary>
            /// <returns>The string of system log file name.</returns>
            private static string GetLogFilename() {
                m_oPiConfig = SSGConfigFactory.GetConfig();
                return m_oPiConfig.SystemLog;
            }

            /// <summary>
            /// Write the exception information into system log file
            /// with the ERROR message type.
            /// </summary>
            /// <param name="objException">The exception information object.</param>
            public static void WriteLog(Exception objException)
            {
                // Get system log filename
                string strLogFile = GetLogFilename();

                //If the log file not found it will create it.
                if (!File.Exists(strLogFile))
                {
                    FileStream oFileStream = new FileStream(strLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    oFileStream.Close();
                }
                
                // write the error log to that text file
                WriteException(strLogFile, objException);
            }

            /// <summary>
            /// Write the message into system log file
            /// with the INFO message type.
            /// </summary>
            /// <param name="strMessage">The string of message detail.</param>
            public static void WriteLog(string strMessage)
            {
                // Get system log filename
                string strLogFile = GetLogFilename();

                //If the log file not found it will create it.
                if (!File.Exists(strLogFile))
                {
                    FileStream oFileStream = new FileStream(strLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    oFileStream.Close();
                }

                // write the log message to that text file
                WriteMessage(strLogFile, strMessage, LogType.INFO);
            }

            /// <summary>
            /// Write the message into system log file
            /// with manual specific message type.
            /// </summary>
            /// <param name="strMessage">The string of message detail.</param>
            /// <param name="eType">Type of message.</param>
            /// <seealso cref="SSG.Logging.LogType"/>
            public static void WriteLog(string strMessage, LogType eType)
            {
                // Get system log filename
                string strLogFile = GetLogFilename();

                //If the log file not found it will create it.
                if (!File.Exists(strLogFile))
                {
                    FileStream oFileStream = new FileStream(strLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    oFileStream.Close();
                }

                // write the log message to that text file
                WriteMessage(strLogFile, strMessage, eType);
            }

            /// <summary>
            /// Write the separator breaking line into system log file.
            /// </summary>
            /// <param name="strFilename">The string of system log file location.</param>
            /// <returns>True if successful writed. Otherwise false.</returns>
            private static bool WriteSeparator(string strFilename)
            {
                bool bSuccess = false;
                try
                {
                    // Write separator breaking line.
                    m_oPiStreamWriter = new StreamWriter(strFilename, true);
                    m_oPiStreamWriter.WriteLine("--------------------------------------------------------------------------------");
                    m_oPiStreamWriter.WriteLine();
                    m_oPiStreamWriter.Flush();
                    m_oPiStreamWriter.Close();
                    // Writed successful.
                    bSuccess = true;
                }
                catch (Exception)
                {
                    // Writing failure
                    bSuccess = false;
                }

                // Return the status of writing
                return bSuccess;
            }

            /// <summary>
            /// Write the exception information into system log file.
            /// </summary>
            /// <param name="strFilename">The string of system log file location.</param>
            /// <param name="objException">The exception information object.</param>
            /// <returns>True if successful writed. Otherwise false.</returns>
            private static bool WriteException(string strFilename, Exception objException)
            {
                bool bSuccess = false;
                try
                {
                    // Write exception information.
                    WriteMessage(strFilename, "Exception", LogType.ERROR);   
                    m_oPiStreamWriter = new StreamWriter(strFilename, true);
                    m_oPiStreamWriter.WriteLine(" Source : " + objException.Source.ToString().Trim());
                    m_oPiStreamWriter.WriteLine(" Method : " + objException.TargetSite.Name.ToString());
                    m_oPiStreamWriter.WriteLine(" Computer : " + Dns.GetHostName().ToString());
                    m_oPiStreamWriter.WriteLine(" Error : " + objException.Message.ToString().Trim());
                    m_oPiStreamWriter.WriteLine(" Stack Trace : " + objException.StackTrace.ToString().Trim());
                    m_oPiStreamWriter.Flush();
                    m_oPiStreamWriter.Close();
                    // Writed successful.
                    bSuccess = true;
                }
                catch (Exception)
                {
                    // Writing failure
                    bSuccess = false;
                }

                // Return the status of writing
                return bSuccess;
            }

            /// <summary>
            /// Write the message into system log file
            /// with manual specific message type.
            /// </summary>
            /// <param name="strFilename">The string of system log file location.</param>
            /// <param name="strMessage">The string of message detail.</param>
            /// <param name="eType">Type of message.</param>
            /// <returns>True if successful writed. Otherwise false.</returns>
            /// <seealso cref="SSG.Logging.LogType"/>
            private static bool WriteMessage(string strFilename, string strMessage, LogType eType)
            {
                bool bSuccess = false;
                try
                {
                    string strMsgType = string.Empty;

                    // Write exception information.
                    m_oPiStreamWriter = new StreamWriter(strFilename, true);
                    switch (eType)
                    {
                        case LogType.NONE :
                            strMsgType = "[NONE]   ";
                            break;
                        case LogType.INFO :
                            strMsgType = "[INFO]   ";
                            break;
                        case LogType.WARNING:
                            strMsgType = "[WARNING]";
                            break;
                        case LogType.ERROR:
                            strMsgType = "[ERROR]  ";
                            break;
                        case LogType.DEBUG:
                            strMsgType = "[DEBUG]  ";
                            break;
                    }

                    m_oPiStreamWriter.WriteLine(strMsgType + " : " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + strMessage.Trim());
                    m_oPiStreamWriter.Flush();
                    m_oPiStreamWriter.Close();

                    // Writed successful.
                    bSuccess = true;
                }
                catch (Exception)
                {
                    // Writing failure
                    bSuccess = false;
                }

                // Return the status of writing
                return bSuccess;
            }
        #endregion
    }
}
