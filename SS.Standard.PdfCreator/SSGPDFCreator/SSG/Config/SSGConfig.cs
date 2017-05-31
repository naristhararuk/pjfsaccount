using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSG.Config
{
    /// <summary>
    /// Configuration class for serialize and deserialize from config.xml file.
    /// </summary>
    /// 
    /// <seealso cref="SSG.Config.SSGConfigFactory"/>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    [XmlRoot("Config")]
    public class SSGConfig
    {

        #region Constant declaration

            // The integer null value used for assign initialize value.
            private const int INTEGER_NULL_VALUE = 0;
            
        #endregion

        #region Variable declaration

            // Properties members
            [XmlAttribute("TexDirectory")] 
            private string m_strPiTexDirectory;
            [XmlAttribute("PdfDirectory")]
            private string m_strPiPdfDirectory;
            [XmlAttribute("LogDirectory")]
            private string m_strPiLogDirectory;
            [XmlAttribute("Latex")]
            private string m_strPiLatex;
            [XmlAttribute("PdfLatex")]
            private string m_strPiPdfLatex;
            [XmlAttribute("ConvertTimeout")]
            private int m_nPiConvertTimeout;
            [XmlAttribute("MsmqQueueName")]
            private string m_strPiMsmqQueueName;
            [XmlAttribute("MessageTimeOut")]
            private int m_nPiMessageTimeOut;
            [XmlAttribute("ServicePollingInterval")]
            private int m_nPiServicePollingInterval;        
            [XmlAttribute("ServiceMaxThread")]
            private int m_nPiServiceMaxThread;
            [XmlAttribute("DatabaseServer")]
            private string m_strPiDatabaseServer;
            [XmlAttribute("DatabaseName")]
            private string m_strPiDatabaseName;
            [XmlAttribute("IntegratedSecurity")]
            private bool m_bPiIntegratedSecurity;
            [XmlAttribute("DBUser")]
            private string m_strPiDBUser;
            [XmlAttribute("DBPassword")]
            private string m_strPiDBPassword;
            [XmlAttribute("SystemLog")]
            private string m_strPiSystemLog;


        #endregion

        #region Constructor

            /// <summary>
            /// Initialize empty value when constructor has created.
            /// </summary>
           public SSGConfig()
            {
                m_strPiTexDirectory = string.Empty;
                m_strPiPdfDirectory = string.Empty;
                m_strPiLogDirectory = string.Empty;
                m_strPiLatex = string.Empty;
                m_strPiPdfLatex = string.Empty;
                m_nPiConvertTimeout = INTEGER_NULL_VALUE;
                m_strPiMsmqQueueName = string.Empty;
                m_nPiMessageTimeOut = INTEGER_NULL_VALUE;
                m_nPiServicePollingInterval = INTEGER_NULL_VALUE;
                m_nPiServiceMaxThread = INTEGER_NULL_VALUE;
                m_strPiDatabaseServer = string.Empty;
                m_strPiDatabaseName = string.Empty;
                m_bPiIntegratedSecurity = true;
                m_strPiDBUser = string.Empty;
                m_strPiDBPassword = string.Empty;
                m_strPiSystemLog = string.Empty;
            }

        #endregion

        #region Methods

            /// <summary>
            /// Serialize the values of properties member into configuration files.
            /// The configuration file has stored in the xml format.
            /// </summary>
            /// <param name="strFilename">a string of file name for writing the configuration values</param>
            /// <param name="oConfig">an object instance has holding the configulation values</param>
            /// <seealso cref="SSG.Config.SSGConfig.Deserialize"/>
            public static void Serialize(string strFilename, SSGConfig oConfig)
            {
                try
                {
                    // Serialize configuration values into a file.
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(SSGConfig));
                    StreamWriter oStmWriter = File.CreateText(strFilename);
                    oXmlSerializer.Serialize(oStmWriter, oConfig);
                    oStmWriter.Flush();
                    oStmWriter.Close();
                }
                catch (Exception)
                {
                    // Ignored exception occuring.
                }
            }

            /// <summary>
            /// Deserialize the values of properties member from configuration files.
            /// The configuration file has stored in the xml format.
            /// </summary>
            /// <param name="strFilename">a string of file name for reading the configuration values</param>
            /// <returns>an instance object of SSGConfig has contained the configuration value</returns>
            /// <seealso cref="SSG.Config.SSGConfig.Serialize"/>
            public static SSGConfig Deserialize(string strFilename)
            {
                try
                {
                    // Deserialize configuration values from a file.
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(SSGConfig));
                    StreamReader oStmReader = File.OpenText(strFilename);
                    SSGConfig c = (SSGConfig)oXmlSerializer.Deserialize(oStmReader);
                    oStmReader.Close();
                    return c;
                }
                catch (Exception)
                {
                    // Ignored exception occuring.
                    return null;
                }
            }

            /// <summary>
            /// Generate the database connection string form the configuration values using for access database server.
            /// </summary>
            /// <returns>a database connection string</returns>
            public string GetDatabaseConnectionString()
            {
                try
                {
                    string strConnection;

                    // Checking for using integrated security.
                    if (m_bPiIntegratedSecurity)
                    {
                        // Generate connection string with using the integrated security.
                        strConnection = "Data Source=" + m_strPiDatabaseServer + ";Initial Catalog=" + m_strPiDatabaseName + ";Integrated Security=SSPI;";
                    }
                    else
                    {
                        // Generate connection string with using the user and password.
                        strConnection = "Data Source=" + m_strPiDatabaseServer + ";Initial Catalog=" + m_strPiDatabaseName + ";User Id=" + m_strPiDBUser + ";Password=" + m_strPiDBPassword + ";";
                    }
                    return strConnection;
                }
                catch (Exception)
                {
                    // Ignored exception occuring.
                    return string.Empty;
                }
            }

        #endregion

        #region Properties

            /// <summary>
            /// Gets or sets the directory for storing latex file (.tex)
            /// </summary>
            /// <value>Directory of storing .tex file.</value>
            /// <remarks>The value must be full path directory</remarks>
            public string TexDirectory
            {
                get { return m_strPiTexDirectory; }
                set { m_strPiTexDirectory = value; }
            }

            /// <summary>
            /// Gets or sets the directory for storing pdf file (.pdf)
            /// </summary>
            /// <value>Directory of storing .pdf file.</value>
            /// <remarks>The value must be full path directory</remarks> 
            public string PdfDirectory
            {
                get { return m_strPiPdfDirectory; }
                set { m_strPiPdfDirectory = value; }
            }

            /// <summary>
            /// Gets or sets the directory for storing log file (.log)
            /// </summary>
            /// <value>Directory of storing .log file.</value>
            /// <remarks>The value must be full path directory</remarks>    
            public string LogDirectory
            {
                get { return m_strPiLogDirectory; }
                set { m_strPiLogDirectory = value; }
            }

            /// <summary>
            /// Gets or sets the location for latex.exe
            /// </summary>
            /// <example>This sample show where is the location of latex.exe
            /// <code>
            ///  SSGConfig oConfig = new SSGConfig();
            ///  oConfig.Latex = "C:\\Program Files\\MiKTeX 2.7\\miktex\\bin\\latex.exe";
            /// </code>
            /// </example>
            /// <value>Location of latex.exe</value>
            /// <remarks>The value must be full path with file name</remarks>
            public string Latex
            {
                get { return m_strPiLatex; }
                set { m_strPiLatex = value; }
            }

            /// <summary>
            /// Gets or sets the location for pdflatex.exe
            /// </summary>
            /// <example>This sample show where is the location of pdflatex.exe
            /// <code>
            ///  SSGConfig oConfig = new SSGConfig();
            ///  oConfig.PdfLatex = "C:\\Program Files\\MiKTeX 2.7\\miktex\\bin\\pdflatex.exe";
            /// </code>
            /// </example>
            /// <value>Location of pdflatex.exe</value>
            /// <remarks>The value must be full path with file name</remarks>
            public string PdfLatex
            {
                get { return m_strPiPdfLatex; }
                set { m_strPiPdfLatex = value; }
            }

            /// <summary>
            /// Gets or sets the time out value using for 
            /// waiting a process of converting .tex to .pdf file.
            /// </summary>
            /// <value>The time out value for killing process</value>
            /// <remarks>The value must be a numeric in second unit (0..nn). 
            /// If value is zero don't wait a process time out.</remarks>
            public int ConvertTimeout
            {
                get { return m_nPiConvertTimeout; }
                set { m_nPiConvertTimeout = value; }
            }

            /// <summary>
            /// Gets or sets the message queue name used for send and receive 
            /// a requests queue generating the pdf documents.
            /// </summary>
            /// <example>This sample show formating of message queue name
            /// <code>
            ///  SSGConfig oConfig = new SSGConfig();
            ///  oConfig.MsmqQueueName = "Develop\private$\PdfRequest";
            /// </code>
            /// </example>
            /// <value>The reference name of message queue</value>
            /// <remarks>The value must be a message queue format such as MachineName\private$\QueueName</remarks>
            public string MsmqQueueName
            {
                get { return m_strPiMsmqQueueName; }
                set { m_strPiMsmqQueueName = value; }
            }

            /// <summary>
            /// Gets or sets the time out value of receiving a messages queue 
            /// </summary>
            /// <value>The time out value for waiting a message in queue</value>
            /// <remarks>The value must be a numeric in second unit (1..60)</remarks>
            public int MessageTimeOut
            {
                get { return m_nPiMessageTimeOut; }
                set { m_nPiMessageTimeOut = value; }
            }

            /// <summary>
            /// Gets or sets the interval value for service polling in each threads
            /// </summary>
            /// <value>The interval value for service polling</value>
            /// <remarks>The value must be a numeric in second unit (1..60)</remarks>
            public int ServicePollingInterval
            {
                get { return m_nPiServicePollingInterval; }
                set { m_nPiServicePollingInterval = value; }
            }

            /// <summary>
            /// Gets or sets the maximum threads amount can growth in windows service
            /// for process converting .tex to .pdf
            /// </summary>
            /// <value>The maximum threads amount value</value>
            /// <remarks>The value must be a numeric (1..nn)</remarks>
            public int ServiceMaxThread
            {
                get { return m_nPiServiceMaxThread; }
                set { m_nPiServiceMaxThread = value; }
            }

            /// <summary>
            /// Gets or sets the name of database server with instance name
            /// </summary>
            /// <value>The instance database server name</value>
            /// <remarks>The value must be database server with instance name</remarks>            
            public string DatabaseServer
            {
                get { return m_strPiDatabaseServer; }
                set { m_strPiDatabaseServer = value; }
            }

            /// <summary>
            /// Gets or sets the database name
            /// </summary>
            /// <value>The name of database</value>
            /// <remarks>The database name has holding a tbPdfRequest table</remarks>
            public string DatabaseName
            {
                get { return m_strPiDatabaseName; }
                set { m_strPiDatabaseName = value; }
            }

            /// <summary>
            /// Gets or sets using integreated security for accessing the database
            /// </summary>
            /// <value>Identify using integreated security</value>
            /// <remarks>The integreated security must be a boolean value true or false. 
            /// If not use integrated security must be identify DBUser and DBPassword.</remarks>
            /// <seealso cref="SSG.Config.SSGConfig.DBUser"/>
            /// <seealso cref="SSG.Config.SSGConfig.DBPassword"/>
            public bool IntegratedSecurity
            {
                get { return m_bPiIntegratedSecurity; }
                set { m_bPiIntegratedSecurity = value; }
            }

            /// <summary>
            /// Gets or sets user name for authenticate accessing the database
            /// </summary>
            /// <value>The user name for access database</value>
            /// <remarks>The user name must have a permission for read and write the tbPdfRequest table in database</remarks>
            /// <seealso cref="SSG.Config.SSGConfig.DBPassword"/>
            public string DBUser
            {
                get { return m_strPiDBUser; }
                set { m_strPiDBUser = value; }
            }

            /// <summary>
            /// Gets or sets password for authenticate accessing the database
            /// </summary>
            /// <value>The password value for access database</value>
            /// <remarks>The password of user name has a permission for read and write the tbPdfRequest table in database</remarks>
            /// <seealso cref="SSG.Config.SSGConfig.DBUser"/>
            public string DBPassword
            {
                get { return m_strPiDBPassword; }
                set { m_strPiDBPassword = value; }
            }

            /// <summary>
            /// Gets or sets the file name for writing the system log
            /// </summary>
            /// <value>The location of file name for writing log</value>
            /// <remarks>The value must be full path with file name</remarks>
            public string SystemLog
            {
                get { return m_strPiSystemLog; }
                set { m_strPiSystemLog = value; }
            }

        #endregion

    }
}
