using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Messaging;
using System.Data;
using System.Data.SqlClient;

using SSG.PDF.Exceptions;
using SSG.Config;
using SSG.Logging;

namespace SSG.PDF
{

    /// <summary>
    /// The class PDFCreator contains methods for performing creating tex file from string content, 
    /// generate pdf document from tex file, send and receive message in the message queue, 
    /// updating and checking status state of generating the pdf documents.
    /// </summary>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    public class PDFCreator
    {

        #region Constant declaration
            
            // The file name extensions.

            /// <summary>
            /// Tex file extension
            /// </summary>
            public const string TEX_FILE_EXTENSION = ".tex";
            /// <summary>
            /// Pdf file extension
            /// </summary>
            public const string PDF_FILE_EXTENSION = ".pdf";
            /// <summary>
            /// Log file extension
            /// </summary>
            public const string LOG_FILE_EXTENSION = ".log";
            /// <summary>
            /// Wild card file extension
            /// </summary>
            public const string WILDCARD_FILE_EXTENSION = ".*";
            /// <summary>
            /// Directory separator
            /// </summary>
            public const string DIRECTORY_SEPARATOR = "\\";
            /// <summary>
            /// Double quote
            /// </summary>
            public const string DOUBLE_QUOTE = "\"";
            /// <summary>
            /// Set the pdflatex interaction mode to non stop mode.
            /// </summary>
            public const string NON_STOP_MODE = " -interaction=nonstopmode ";
            /// <summary>
            /// Encoding support thai language
            /// </summary>
            public const string THAI_ENCODING = "TIS-620";
            
            // Time constants

            /// <summary>
            /// Zero timeout value
            /// </summary>
            public const int ZERO_TIMEOUT = 0;
            /// <summary>
            /// Zero hour value
            /// </summary>
            public const int ZERO_HOUR = 0;
            /// <summary>
            /// Zero minute value
            /// </summary>
            public const int ZERO_MINUTE = 0;
            /// <summary>
            /// Thousand milliseconds value
            /// </summary>
            public const int THOUSAND_MILLISECONDS = 1000;

            // Label for writing into log file.

            /// <summary>
            /// Worker label 
            /// </summary>
            public const string WORKER_LABEL = ": Worker #";
            /// <summary>
            /// Begining process label
            /// </summary>
            public const string BEGINING_PROCESS_LABEL = " - Begin excuting ";
            /// <summary>
            /// Ending process label
            /// </summary>
            public const string ENDING_PROCESS_LABEL = " - Success.";
            /// <summary>
            /// Failure process label
            /// </summary>
            public const string FAILURE_PROCESS_LABEL = " - Fail.";

            /// <summary>
            /// Database table name
            /// </summary>
            public const string PDF_REQUEST_TABLE = "tbPDFRequest";

            /// <summary>
            /// The status type of generating pdf document.
            /// </summary>
            public enum PdfRequestStatus {
                /// <summary>
                /// Not specific status
                /// </summary>
                None = 0,  
                /// <summary>
                /// Created new request status
                /// </summary>
                New = 1, 
                /// <summary>
                /// In processing status
                /// </summary>
                InProcess = 2, 
                /// <summary>
                /// Created successful
                /// </summary>
                Success = 3,    
                /// <summary>
                /// Creating failure
                /// </summary>
                Fail = 4        
            };

            private SSGConfig m_oPiConfig; // Configuration objects

        #endregion

        #region Contructor

            /// <summary>
            /// Gettering the configuration values when constructor has created. 
            /// </summary>
            public PDFCreator()
            {
                // Get the configuration
                m_oPiConfig = SSGConfigFactory.GetConfig();
            }

            /// <summary>
            /// Gettering the specified configuration values when constructor has created. 
            /// </summary>
            /// <param name="strFilename"></param>
            public PDFCreator(string strFilename)
            {
                // Get the configuration
                m_oPiConfig = SSGConfigFactory.GetConfig(strFilename);
            }

        #endregion

        #region Methods

            /// <summary>
            /// Generate a pdf document file from the string content.
            /// </summary>
            /// <param name="strTexContent">The latex string content.</param>
            /// <returns>Return the uniqueidentified of tex and pdf files. 
            /// Empty uniqueidentified if creating failure.</returns>
            /// <seealso cref="SSG.PDF.GeneratePDF(Guid oID)"/>
            public Guid GeneratePDF(string strTexContent)
            {
                try
                {
                    Guid oID = Guid.Empty;

                    // Generate a pdf file if not empty string content. 
                    if (!string.IsNullOrEmpty(strTexContent))
                    {
                        // 1. Generate GUID
                        oID = Guid.NewGuid();
                        // 2. Save texContent to <Guid>.tex
                        CreateTexFile(oID.ToString(), strTexContent);
                        // 3. Generate a pdf document from tex file.
                        oID = GeneratePDF(oID);
                    }
                    // 4.  Return GUID
                    return oID;
                }
                catch (CreateTexException ex)
                {
                    // Write error log and return empty uniqueidentified.
                    Logger.WriteLog(ex);
                    return Guid.Empty;
                }
            }

            /// <summary>
            /// Generate a pdf document file from the latex file.
            /// </summary>
            /// <param name="oID">The uniqueidentified of tex file name.</param>
            /// <returns>Return the uniqueidentified of pdf files. 
            /// Empty uniqueidentified if creating failure.</returns>
            public Guid GeneratePDF(Guid oID)
            {
                try
                {
                    if (File.Exists(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + oID.ToString() + TEX_FILE_EXTENSION))
                    {
                        // 1. Add record in PDFRequest
                        UpdateDatabaseStatus(oID.ToString(), PdfRequestStatus.New);
                        // 2. Add Token to Microsoft message queue to trigger service
                        SendGeneratePDFRequestToMSMQ(oID.ToString());
                    }
                    // 3.  Return GUID
                    return oID;
                }
                catch (CreateTexException ex)
                {
                    // Write error log and return empty uniqueidentified.
                    Logger.WriteLog(ex);
                    return Guid.Empty;
                }
            }

            /// <summary>
            /// Getting tex file location from specified id.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <returns>The location of tex file name</returns>
            public string GetTexFileUri(string strID)
            {
                try
                {
                    // Generate the location of tex file.
                    return m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + TEX_FILE_EXTENSION;
                }
                catch (Exception ex)
                {
                    // Write error log and return empty string.
                    Logger.WriteLog(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// Getting pdf file location from specified id.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <returns>The location of pdf file name</returns>
            public string GetPdfFileUri(string strID)
            {
                try
                {
                    // Generate the location of pdf file.
                    return m_oPiConfig.PdfDirectory + DIRECTORY_SEPARATOR + strID + PDF_FILE_EXTENSION;
                }
                catch (Exception ex)
                {
                    // Write error log and return empty string.
                    Logger.WriteLog(ex);
                    return string.Empty;
                }
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
            public string GetPdfFileName(string strID)
            {
                try
                {
                    // Generate the location of pdf file.
                    //return m_oPiConfig.PdfDirectory + DIRECTORY_SEPARATOR + strID + PDF_FILE_EXTENSION;
                    return strID + PDF_FILE_EXTENSION;
                }
                catch (Exception ex)
                {
                    // Write error log and return empty string.
                    Logger.WriteLog(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// Getting log file location from specified id.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <returns>The location of log file name</returns>
            public string GetLogFileUri(string strID)
            {
                try
                {
                    // Generate the location of log file.
                    return m_oPiConfig.LogDirectory + DIRECTORY_SEPARATOR + strID + LOG_FILE_EXTENSION;
                }
                catch (Exception ex)
                {
                    // Write error log and return empty string.
                    Logger.WriteLog(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            /// Create a tex file from string latex content into tex directory from configuration value.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <param name="strTexContent">The latex string content.</param>
            /// <exception cref="SSG.PDF.Exceptions.CreateTexException">Thrown when creating tex file has error.</exception>
            private void CreateTexFile(string strID, string strTexContent)
            {
                try
                {
                    // Checking for the store directory of Tex files.
                    if (!Directory.Exists(m_oPiConfig.TexDirectory)) 
                    {   
                        // Create directory for storing Tex files.
                        Directory.CreateDirectory(m_oPiConfig.TexDirectory);
                    }

                    // Create Tex file
                    string strFilename = GetTexFileUri(strID);
                    // Delete before processing a new file.
                    if (File.Exists(strFilename))
                        File.Delete(strFilename);
                    // Force to thai encoding.
                    Encoding oEncoding = Encoding.GetEncoding(THAI_ENCODING);
                    // Create latex file.
                    TextWriter oTextWriter = new StreamWriter(strFilename, false, oEncoding);
                    oTextWriter.Write(strTexContent);
                    oTextWriter.Close();

                    // Thrown a CreateTexException when file not found.
                    if (!File.Exists(strFilename))
                        throw new CreateTexException();
                }
                catch (Exception ex)
                {
                    // Write error log and thrown a CreateTexException.
                    Logger.WriteLog(ex);
                    throw new CreateTexException(ex.ToString());
                }
            }

            /// <summary>
            /// Create a pdf file from exist tex file into pdf directory from configuration value.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <exception cref="SSG.PDF.Exceptions.CreateTexException">Thrown when creating pdf file has error.</exception>
            public void CreatePdfFile(string strID)
            {
                try
                {
                    // Checking for the store directory of Pdf files.
                    string strFilename = GetTexFileUri(strID);
                    string strPdfFile = GetPdfFileUri(strID);
                    string strLogFile = GetLogFileUri(strID);

                    if (!Directory.Exists(m_oPiConfig.PdfDirectory))
                    {
                        // Create directory for storing Pdf files.
                        Directory.CreateDirectory(m_oPiConfig.PdfDirectory);
                    }
                    if (!Directory.Exists(m_oPiConfig.LogDirectory))
                    {
                        // Create directory for storing Log files.
                        Directory.CreateDirectory(m_oPiConfig.LogDirectory);
                    }

                    // Delete before processing a new file.
                    if (File.Exists(strPdfFile))
                        File.Delete(strPdfFile);
                    // Delete before processing a new file.
                    if (File.Exists(strLogFile))
                        File.Delete(strLogFile);

                    // Process creating temporary auxilary files.
                    System.Diagnostics.Process oProcess = new System.Diagnostics.Process();

                    // Commment block for disable process latex.exe
                    /*
                    oProcess.EnableRaisingEvents = false;
                    oProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;  // Set to background process.
                    oProcess.StartInfo.FileName = DOUBLE_QUOTE + m_oPiConfig.Latex + DOUBLE_QUOTE;
                    oProcess.StartInfo.Arguments = DOUBLE_QUOTE + strFilename + DOUBLE_QUOTE;
                    oProcess.StartInfo.WorkingDirectory = m_oPiConfig.TexDirectory; // Set location has exist tex files.
                    oProcess.Start();
                    // Wait for a process successful or timeout
                    if (m_oPiConfig.ConvertTimeout == ZERO_TIMEOUT)
                        oProcess.WaitForExit();
                    else
                        oProcess.WaitForExit(m_oPiConfig.ConvertTimeout * THOUSAND_MILLISECONDS);

                    // Timeout to kill the precess
                    if (!oProcess.HasExited)
                        oProcess.Kill();
                    */

                    // Process creating pdf file.
                    oProcess.EnableRaisingEvents = false;
                    oProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;  // Set to background process.
                    oProcess.StartInfo.FileName = DOUBLE_QUOTE + m_oPiConfig.PdfLatex + DOUBLE_QUOTE;
                    oProcess.StartInfo.Arguments = NON_STOP_MODE + DOUBLE_QUOTE + m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + DOUBLE_QUOTE;
                    oProcess.StartInfo.WorkingDirectory = m_oPiConfig.TexDirectory; // Set location has exist tex files.
                    oProcess.Start();
                    // Wait for a process successful or timeout
                    if (m_oPiConfig.ConvertTimeout == ZERO_TIMEOUT)
                        oProcess.WaitForExit();
                    else
                        oProcess.WaitForExit(m_oPiConfig.ConvertTimeout * THOUSAND_MILLISECONDS);

                    // Timeout to kill the precess
                    if (!oProcess.HasExited)
                        oProcess.Kill();

                    // Move pdf file to storage folder
                    if (File.Exists(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + PDF_FILE_EXTENSION))
                        if (!strPdfFile.Equals(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + PDF_FILE_EXTENSION))
                            File.Move(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + PDF_FILE_EXTENSION, strPdfFile);

                    // Move log file to storage folder
                    if (File.Exists(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + LOG_FILE_EXTENSION))
                        if (!strLogFile.Equals(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + LOG_FILE_EXTENSION))
                            File.Move(m_oPiConfig.TexDirectory + DIRECTORY_SEPARATOR + strID + LOG_FILE_EXTENSION, strLogFile);

                    // Delete temporary files.
                    string[] strFiles = Directory.GetFiles(m_oPiConfig.TexDirectory, strID + WILDCARD_FILE_EXTENSION);
                    foreach (string strTempFile in strFiles)
                    {
                        // Skip delete if files extension is .tex, .pdf or .log
                        if (!(strTempFile.EndsWith(TEX_FILE_EXTENSION, true, null) ||
                            strTempFile.EndsWith(PDF_FILE_EXTENSION, true, null) ||
                            strTempFile.EndsWith(LOG_FILE_EXTENSION, true, null)))
                        {
                            // Perform delete temporary files.
                            File.Delete(strTempFile);
                        }
                    }

                    // Thrown a CreatePdfException when file not found.
                    if (!File.Exists(strPdfFile))
                        throw new CreatePdfException();

                    // Update succesful status into datatbase. 
                    UpdateDatabaseStatus(strID, PdfRequestStatus.Success);
                }
                catch (Exception ex)
                {
                    // Write error log, update failure status into database and thrown a CreatePdfException.
                    Logger.WriteLog(ex);
                    UpdateDatabaseStatus(strID, PdfRequestStatus.Fail);
                    throw new CreatePdfException(ex.ToString());
                }
            }

            /// <summary>
            /// Get message queue instance object from message queue name
            /// and create message queue object if not exist.
            /// </summary>
            /// <returns>An object instance of message queue</returns>
            public MessageQueue GetMSMQ()
            {
                try
                {
                    MessageQueue oMsgQ;
                    // Checking for exists Message Queue.
                    if (MessageQueue.Exists(m_oPiConfig.MsmqQueueName))
                        oMsgQ = new MessageQueue(m_oPiConfig.MsmqQueueName);
                    else
                        oMsgQ = MessageQueue.Create(m_oPiConfig.MsmqQueueName);
                    oMsgQ.SetPermissions(Environment.UserName, 
                                         MessageQueueAccessRights.FullControl, 
                                         AccessControlEntryType.Set);

                    // Return an object instance.
                    return oMsgQ;
                }
                catch (Exception ex)
                {
                    // Write error log and return null instance.
                    Logger.WriteLog(ex);
                    return null;
                }
            }

            /// <summary>
            /// Clear existing messgae queue to empty.
            /// </summary>
            public void ClearExistQueue()
            {
                try
                {
                    MessageQueue oMsgQ = GetMSMQ();
                    // Purge all message queue
                    oMsgQ.Purge();
                }
                catch (Exception ex)
                {
                    // Write error log.
                    Logger.WriteLog(ex);
                }
            }

            /// <summary>
            /// Send message require generate a pdf document to windows message queue.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <exception cref="SSG.PDF.Exceptions.SendMessageQueueException">Thrown when sending message has error.</exception>
            private void SendGeneratePDFRequestToMSMQ(string strID)
            {
                try
                {
                    // Get instance message queue.
                    MessageQueue oMsgQ =  GetMSMQ();
                    
                    // Create a new message object
                    Message oMsg = new Message();
                    oMsg.UseDeadLetterQueue = true;
                    oMsg.Formatter = new BinaryMessageFormatter();  // Force to binary format sending
                    // Set body message with uniqueidentified ID
                    oMsg.Body = strID;
                    oMsg.Label = strID;
                    // Post the request message.
                    oMsgQ.Send(oMsg);

                    oMsgQ.Close();
                }
                catch (Exception ex)
                {
                    // Write error log, update failure status into database and thrown a SendMessageQueueException.
                    Logger.WriteLog(ex);
                    UpdateDatabaseStatus(strID, PdfRequestStatus.Fail);
                    throw new SendMessageQueueException(ex.Message);
                }
            }

            /// <summary>
            /// Read ID of required generate a pdf document message from windows message queue.
            /// </summary>
            /// <returns>An object instance of requested id in message queue</returns>
            /// <exception cref="SSG.PDF.Exceptions.ReadMessageQueueException">Thrown when receiving message has error.</exception>
            public Guid ReadRequestFromMSMQ()
            {

                Guid oID = Guid.Empty;
                try
                {
                    // Get instance message queue.
                    MessageQueue oMsgQ = GetMSMQ();

                    // Receive required generate a pdf document message 
                    Message oMsg = oMsgQ.Receive(new TimeSpan(ZERO_HOUR, ZERO_MINUTE, m_oPiConfig.MessageTimeOut));
                    oMsg.Formatter = new BinaryMessageFormatter();
                    // Get a uniqeidentified tex id
                    oID = new Guid(oMsg.Body.ToString());
                    
                    oMsgQ.Close();
                }
                catch (MessageQueueException)
                {
                    // Ignored message queue time out exception.
                    return oID;
                }
                catch (Exception ex)
                {
                    // Write error log
                    Logger.WriteLog(ex);
                    throw new ReadMessageQueueException(ex.Message);
                }
                return oID;
            }

            /// <summary>
            /// Perform generate a pdf document from each thread.
            /// </summary>
            /// <param name="strWorker">Name of worker from the owner thread.</param>
            /// <param name="oID">The ID of requested pdf generate.</param>
            /// <param name="bForce">Flag to force perform generate pdf with out checking status</param>
            /// <exception cref="SSG.PDF.Exceptions.CreatePdfException">Thrown when generating pdf has error.</exception>
            public void PerfromGeneratePDF(string strWorker, Guid oID, bool bForce)
            {
                try
                {
                    bool bDoProcess = false;

                    if (bForce)
                    {
                        // Ignore status and force to process generating
                        bDoProcess = true;
                    }
                    else
                    {
                        // Make sure a requested not stay in process generating
                        if (!IsInProcess(oID.ToString()))
                            bDoProcess = true;
                    }

                    
                    if (bDoProcess)
                    {
                        // Perform generate a pdf document.
                        Logger.WriteLog(oID.ToString() + WORKER_LABEL + strWorker + BEGINING_PROCESS_LABEL + oID.ToString() + TEX_FILE_EXTENSION);
                        UpdateDatabaseStatus(oID.ToString(), PDFCreator.PdfRequestStatus.InProcess);
                        CreatePdfFile(oID.ToString());
                        Logger.WriteLog(oID.ToString() + WORKER_LABEL + strWorker + ENDING_PROCESS_LABEL);
                    }
                }
                catch (Exception ex)
                {
                    // Write error log, update failure status into database and thrown a ReceiveMessageQueueException.
                    Logger.WriteLog(oID.ToString() + WORKER_LABEL + strWorker + FAILURE_PROCESS_LABEL);
                    Logger.WriteLog(ex);
                    throw new CreatePdfException(ex.Message);
                }
            }

            /// <summary>
            /// Update a status into pdf request table.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <param name="eStatus">The value must be a PdfRequestStatus</param>
            /// <exception cref="SSG.PDF.Exceptions.UpdateStatusException">Thrown when update database has error.</exception>
            public void UpdateDatabaseStatus(string strID, PdfRequestStatus eStatus)
            {
                try
                {
                    // Create connection
                    SqlConnection oConn = new SqlConnection(m_oPiConfig.GetDatabaseConnectionString());
                    SqlCommand oCmd;
                    string strSQL;

                    // Open database connection
                    oConn.Open();

                    // Create sql statment for update a status
                    if (eStatus == PdfRequestStatus.New)
                        strSQL = "INSERT INTO " + PDF_REQUEST_TABLE + " (ID, Status, CreatedDate, LastUpdate) VALUES(@ID, @Status, @CreatedDate, @LastUpdate);";
                    else
                        strSQL = "UPDATE " + PDF_REQUEST_TABLE + " SET Status=@Status, LastUpdate=@LastUpdate WHERE ID=@ID;";

                    // Assign a parameters for sql statement
                    oCmd = new SqlCommand(strSQL, oConn);
                    SqlParameter oParamID = new SqlParameter("@ID", SqlDbType.UniqueIdentifier);
                    Guid oID = new Guid(strID);
                    oParamID.Value = oID;
                    oCmd.Parameters.Add(oParamID);
                    SqlParameter oParamStatus = new SqlParameter("@Status", SqlDbType.Int);
                    oParamStatus.Value = eStatus;
                    oCmd.Parameters.Add(oParamStatus);
                    if (eStatus == PdfRequestStatus.New)
                    {
                        SqlParameter oParamCreatedDate = new SqlParameter("@CreatedDate", SqlDbType.DateTime);
                        oParamCreatedDate.Value = DateTime.Now;
                        oCmd.Parameters.Add(oParamCreatedDate);
                    }
                    SqlParameter oParamLastUpdate = new SqlParameter("@LastUpdate", SqlDbType.DateTime);
                    oParamLastUpdate.Value = DateTime.Now;
                    oCmd.Parameters.Add(oParamLastUpdate);

                    // Execute sql statement
                    oCmd.ExecuteNonQuery();

                    // Close database connection
                    oConn.Close();
                }
                catch (Exception ex)
                {
                    // Write error log and thrown a UpdateStatusException.
                    Logger.WriteLog(ex);
                    throw new UpdateStatusException(ex.Message);
                }
            }

            /// <summary>
            /// Get a pdf requested status from database.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <returns>The status of a pdf requested.</returns>
            public PdfRequestStatus GetStatus(string strID)
            {
                try
                {
                    // Create connection.
                    SqlConnection oConn = new SqlConnection(m_oPiConfig.GetDatabaseConnectionString());
                    SqlCommand oCmd;

                    PdfRequestStatus eStatus = PdfRequestStatus.None;

                    string strSQL;

                    // Open database connection
                    oConn.Open();

                    strSQL = "SELECT Status FROM " + PDF_REQUEST_TABLE + " WHERE (ID=@ID);";

                    // Assign a parameters for sql statement
                    oCmd = new SqlCommand(strSQL, oConn);
                    SqlParameter oParamID = new SqlParameter("@ID", SqlDbType.UniqueIdentifier);
                    Guid oID = new Guid(strID);
                    oParamID.Value = oID;
                    oCmd.Parameters.Add(oParamID);
                    // Execute sql statement
                    eStatus = (PdfRequestStatus)oCmd.ExecuteScalar();

                    // Close database connection
                    oConn.Close();

                    // Return the status of a pdf requested.
                    return eStatus;
                }
                catch (Exception ex)
                {
                    // Write error log and return PdfRequestStatus.None.
                    Logger.WriteLog(ex);
                    return PdfRequestStatus.None;
                }
            }

            /// <summary>
            /// Get a pdf requested still in process status from the database.
            /// </summary>
            /// <param name="strID">The uniqueidentified string</param>
            /// <returns>Return true is in processing. Otherwise return false.</returns>
            public bool IsInProcess(string strID)
            {
                try
                {
                    // Retrieve a pdf requested status from the database 
                    // and checking status for PdfRequestStatus.InProcess
                    if (GetStatus(strID) == PdfRequestStatus.InProcess)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    // Write error log and return false.
                    Logger.WriteLog(ex);
                    return false;
                }
            }

            /// <summary>
            /// Get list of requested id from specific status
            /// </summary>
            /// <param name="eStatus">The status want to get list</param>
            /// <returns>List of guid</returns>
            public List<Guid> GetListFromStatus(PdfRequestStatus eStatus)
            {
                try
                {
                    // Create connection.
                    SqlConnection oConn = new SqlConnection(m_oPiConfig.GetDatabaseConnectionString());
                    SqlCommand oCmd;
                    SqlDataReader oReader;
                    List<Guid> oGuidList = new List<Guid>();

                    string strSQL;

                    // Open database connection
                    oConn.Open();

                    strSQL = "SELECT ID FROM " + PDF_REQUEST_TABLE + " WHERE (Status=@Status);";

                    // Assign a parameters for sql statement
                    oCmd = new SqlCommand(strSQL, oConn);
                    SqlParameter oParamStatus = new SqlParameter("@Status", SqlDbType.Int);
                    oParamStatus.Value = eStatus;
                    oCmd.Parameters.Add(oParamStatus);
                    // Execute sql statement
                    oReader = oCmd.ExecuteReader();
                    if (oReader.HasRows)
                        // Save requested id into list
                        while (oReader.Read())
                        {
                            oGuidList.Add(oReader.GetGuid(0));
                        }

                    oReader.Close();

                    // Close database connection
                    oConn.Close();

                    // Return list of requested id.
                    return oGuidList;
                }
                catch (Exception ex)
                {
                    // Write error log and return empty list
                    Logger.WriteLog(ex);
                    return new List<Guid>();
                }
            }

            /// <summary>
            /// Get uncompleted list of requested id from new and inprocess status
            /// </summary>S
            /// <returns>List of guid</returns>
            public List<Guid> LoadUncompleted()
            {
                List<Guid> oUncompleteID= new List<Guid>();
                try
                {
                    List<Guid> oNewID = GetListFromStatus(PdfRequestStatus.New);

                    foreach (Guid oID in oNewID.ToArray())
                    {
                        oUncompleteID.Add(oID);
                    }

                    List<Guid> oInProcessID = GetListFromStatus(PdfRequestStatus.InProcess);

                    foreach (Guid oID in oInProcessID.ToArray())
                    {
                        oUncompleteID.Add(oID);
                    }
                }
                catch (Exception ex)
                {
                    // Write error log.
                    Logger.WriteLog(ex);
                }
                return oUncompleteID;
            }


        #endregion
    }
}
