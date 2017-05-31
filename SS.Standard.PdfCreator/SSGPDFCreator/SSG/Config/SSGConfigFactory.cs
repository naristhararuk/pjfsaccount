using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SSG.Config
{

    /// <summary>
    /// Configuration factory class for return a singleton instance config object.
    /// </summary>
    /// 
    /// <seealso cref="SSG.Config.SSGConfig"/>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    public static class SSGConfigFactory
    {
        #region Constant declaration
            
            // Declaration default value.
            /// <summary>
            /// The configuration file name
            /// </summary>
            public const string CONFIGURATION_FILE = "Config.xml";
            private const int CONVERT_TIMEOUT = 30;
            private const int MESSAGE_TIMEOUT = 1;
            private const int SERVICE_POLLING_INTERVAL = 1;
            private const int SERVICE_MAXIMUM_THREAD = 5;
            /// <summary>
            /// Directory separator symbol
            /// </summary>
            public const string DIRECTORY_SEPARATOR = "\\";

        #endregion

        #region Variable declaration
            
            private static SSGConfig m_oPiConfig = null;   // An onject holding the configulation value

        #endregion

        #region Constructor

            /// <summary>
            /// Default constructor
            /// </summary>
            static SSGConfigFactory()
            {
            }

        #endregion

        #region Methods

            /// <summary>
            /// Gets the singleton instance of configulation object.
            /// </summary>
            /// <returns>an instance object of SSGConfig has contained the configuration value</returns>
            public static SSGConfig GetConfig()
            {
                if (m_oPiConfig == null)
                {
                    // Get current directory for store configulation file.
                    string strConfigFile = Environment.CurrentDirectory + DIRECTORY_SEPARATOR + CONFIGURATION_FILE;
                    LoadConfig(strConfigFile);
                }
                return m_oPiConfig;
            }

            /// <summary>
            /// Gets the configuration object from specified configuration path.
            /// </summary>
            /// <param name="strFilename">Configuration file with full path</param>
            /// <returns>an instance object of SSGConfig has contained the configuration valuef</returns>
            public static SSGConfig GetConfig(string strFilename)
            {
                LoadConfig(strFilename);
                return m_oPiConfig;
            }

            /// <summary>
            /// Load configuration values from the config.xml file.
            /// If no exist config.xml file the constructor will be creating with default values.
            /// </summary>
            /// <param name="strFilename">Configuration file with full path</param>
            private static void LoadConfig(string strFilename)
            {
                m_oPiConfig = new SSGConfig();

                // Checking exists the configuration file and create default configuration if not found.
                if (!File.Exists(strFilename))
                {
                    // Set the default values.
                    m_oPiConfig.TexDirectory = Environment.CurrentDirectory + "\\tex";
                    m_oPiConfig.PdfDirectory = Environment.CurrentDirectory + "\\pdf";
                    m_oPiConfig.LogDirectory = Environment.CurrentDirectory + "\\log";
                    m_oPiConfig.Latex = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MiKTeX 2.7\\miktex\\bin\\latex.exe";
                    m_oPiConfig.PdfLatex = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MiKTeX 2.7\\miktex\\bin\\pdflatex.exe";
                    m_oPiConfig.ConvertTimeout = CONVERT_TIMEOUT;
                    m_oPiConfig.MsmqQueueName = Environment.MachineName + "\\private$\\PDFQueue";
                    m_oPiConfig.MessageTimeOut = MESSAGE_TIMEOUT;
                    m_oPiConfig.ServicePollingInterval = SERVICE_POLLING_INTERVAL;
                    m_oPiConfig.ServiceMaxThread = SERVICE_MAXIMUM_THREAD;
                    m_oPiConfig.DatabaseServer = Environment.MachineName;
                    m_oPiConfig.DatabaseName = "PDFRequest";
                    m_oPiConfig.IntegratedSecurity = true;
                    m_oPiConfig.DBUser = "user";
                    m_oPiConfig.DBPassword = "password";
                    m_oPiConfig.SystemLog = Environment.CurrentDirectory + DIRECTORY_SEPARATOR + AppDomain.CurrentDomain.FriendlyName + ".log";
                    
                    // Serialize the configuration object to file
                    SSGConfig.Serialize(strFilename, m_oPiConfig);
                }
                else
                {
                    // Read the configuration object from file
                    m_oPiConfig = SSGConfig.Deserialize(strFilename);
                }
            }

        #endregion
    }
}
