using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;

using SSG.Config;
using SSG.PDF;
using SSG.PDF.Exceptions;
using SSG.Logging;

namespace SSG.PDF.Service
{

    #region Inner class

        /// <summary>
        /// The inner class Worker contains methods for calling SSG.PDFReceive.PDFCreator.RequestFromMSMQ 
        /// when thread is running state.
        /// </summary>
        /// 
        /// <author>Phoonperm Suwannarattaphoom</author>
        /// <version>1.0</version>
        class Worker
        {

            #region Constant declaration

                /// <summary>
                /// Default interval = 1 second
                /// </summary>
                private const int DEFAULT_INTERVAL_TIMER = 1;
                // Time constants
                /// <summary>
                /// Zero hour value
                /// </summary>
                public const int ZERO_HOUR = 0;
                /// <summary>
                /// Zero minute value
                /// </summary>
                public const int ZERO_MINUTE = 0;

            #endregion

            #region Variable declaration
                
                private static PDFCreator oPiPDFCreator;            // Object library
                private Guid oPiRequestedID;                        // Requested ID
                private int nPiID;                                  // Thread ID
                private int nPiInterval = DEFAULT_INTERVAL_TIMER;   // Thread sleep timer

                private bool bPiServiceStarted = false;             // This is a flag to indicate the service status
                private bool bPiServiceIdle = true;                 // This is a flag to indicate the idle status
                private bool bPiForceProcess = false;               // This is a flag to indicate force generate pdf

            #endregion

            #region Constructor

                /// <summary>
                /// Constructor a Worker with ID and interval sleeping value. 
                /// </summary>
                /// <param name="nID">The worker id.</param>
                /// <param name="nInterval">The interval value must be second unit. 
                /// It's define thread sleep.</param>
                public Worker(int nID, int nInterval)
                {
                    oPiRequestedID = Guid.Empty;
                    ServiceIdle = true;
                    nPiID = nID;
                    nPiInterval = nInterval;
                }

            #endregion

            #region Properties

                /// <summary>
                /// Gets or sets flag of start/stop service.
                /// </summary>
                /// <value>Status of start/stop service.</value>
                public bool ServiceStarted
                {
                    get { return bPiServiceStarted; }
                    set { bPiServiceStarted = value; }
                }

                /// <summary>
                /// Gets or sets flag of service idle status.
                /// </summary>
                /// <value>Status of idle service.</value>
                public bool ServiceIdle
                {
                    get { return bPiServiceIdle; }
                    set { bPiServiceIdle = value; }
                }

                /// <summary>
                /// Gets or sets flag of force perform generate pdf.
                /// </summary>
                /// <value>Status of force process.</value>
                public bool ForceProcess
                {
                    get { return bPiForceProcess; }
                    set { bPiForceProcess = value; }
                }

                /// <summary>
                /// Gets or sets the requested id.
                /// </summary>
                /// <value>Uniqeidentifier ID</value>
                public Guid RequestedID
                {
                    get { return oPiRequestedID; }
                    set { oPiRequestedID = value; }
                }

            #endregion

            #region Methods
                
                /// <summary>
                /// Execute task when thread started for doing process job.
                /// </summary>
                public void ExecuteTask()
                {
                    try
                    {

                        while (bPiServiceStarted)
                        {
                            // Check the current thread has assign the requested id
                            if (!oPiRequestedID.Equals(Guid.Empty))
                            {
                                ServiceIdle = false;
                                DoProcess(nPiID.ToString(), oPiRequestedID, bPiForceProcess);
                                ServiceIdle = true;
                                oPiRequestedID = Guid.Empty;
                            }
                            // Sleep for wait next executing
                            Thread.Sleep(new TimeSpan(ZERO_HOUR, ZERO_MINUTE, nPiInterval));
                        }
                        // Abort current thread
                        Thread.CurrentThread.Abort();
                        ServiceIdle = true;
                    }
                    catch (Exception)
                    {
                        // Reset idle status and requested id
                        ServiceIdle = true;
                        oPiRequestedID = Guid.Empty;
                    }
                }

                /// <summary>
                /// Do process for calling the SSG.PDFReceive.PDFCreator.PerfromGeneratePDF
                /// </summary>
                /// <param name="strWorker">The name of Worker</param>
                /// <param name="oID">The ID of requested pdf generate.</param>
                /// <param name="bForce">Force generate a pdf</param>
                /// <seealso cref="SSG.PDF.PDFCreator.PerfromGeneratePDF"/>
                public void DoProcess(string strWorker, Guid oID, bool bForce)
                {
                    try
                    {
                        // Calling a PerfromGeneratePDF with Woker name and requested id.
                        oPiPDFCreator = new PDFCreator();
                        oPiPDFCreator.PerfromGeneratePDF(strWorker, oID, bForce);
                    }
                    catch (Exception ex)
                    {
                        // Write error log.
                        Logger.WriteLog(ex);
                    }
                }

            #endregion
        }

    #endregion

    /// <summary>
    /// The class PDFCreatorService contains methods for performing 
    /// start/stop windows service and control threads polling.
    /// </summary>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    public partial class PDFCreatorService : ServiceBase
    {

        #region Constant declaration

            // Time constants
            /// <summary>
            /// Zero hour value
            /// </summary>
            private const int ZERO_HOUR = 0;
            /// <summary>
            /// Zero minute value
            /// </summary>
            private const int ZERO_MINUTE = 0; 
            /// <summary>
            /// Pending time to stop process
            /// </summary>
            private const int PENDING_TIME_TO_STOP = 10;
            /// <summary>
            /// First index of thread
            /// </summary>
            private const int FIRST_INDEX_THREAD = 0;
            /// <summary>
            /// Starting index of path file
            /// </summary>
            private const int START_PATH_INDEX = 6;
            /// <summary>
            /// Base index of uncompleted list
            /// </summary>
            private const int UNCOMPLETED_BASE_INDEX = 0;

            private enum ProcessState
            {
                IDLE_STATE = 0,     // Idle state nothing to do
                CLEAR_STATE = 1,    // Clear existing message queue
                REDO_STATE = 2,     // Redo generate process for new and inprocess status of requested job.
                READ_STATE = 3      // Read new request from message queue
            };

        #endregion

        #region Variable declaration

            static private Thread[] oAPiWorkerThreads;     // array of worker threads
            static private Worker[] oAPiWorkers;           // the objects that do the actual work
            static private int iPiCurrentThread;           // index of current thread
            static private bool bPiServiceStarted = false; // This is a flag to indicate the service status
            static private ProcessState ePiState;          // Process state 
            static private List<Guid> oPiUncompleteID;     // Uncomplete requested job 
            static private SSGConfig oPiConfig;            // object configuration
            static private PDFCreator oPiPDFCreator;       // object library

        #endregion

        #region Contructor

            /// <summary>
            /// Initialize and loading configuration 
            /// </summary>
            public PDFCreatorService()
            {
                InitializeComponent();
                // Load configuration file from execution path
                oPiConfig = SSGConfigFactory.GetConfig(GetConfigFile());
                oPiPDFCreator = new PDFCreator();
                ePiState = ProcessState.IDLE_STATE;
                Logger.WriteLog("PDF Generator Service has initialized...");
            }

        #endregion

        #region Methods

            /// <summary>
            /// Get configuration file from execution path
            /// </summary>
            /// <returns>Configuration file name</returns>
            protected string GetConfigFile()
            {
                string strFilename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) +
                                                           SSGConfigFactory.DIRECTORY_SEPARATOR + 
                                                           SSGConfigFactory.CONFIGURATION_FILE;
                strFilename = strFilename.Substring(START_PATH_INDEX);
                return strFilename;
            }

            /// <summary>
            /// Perfrom create threads with maximum thread polling.
            /// </summary>
            /// <param name="args">Argument parameter</param>
            protected override void OnStart(string[] args)
            {
                try
                {
                    // Create process with maximum thread polling
                    oAPiWorkers = new Worker[oPiConfig.ServiceMaxThread];
                    oAPiWorkerThreads = new Thread[oPiConfig.ServiceMaxThread];
                    iPiCurrentThread = FIRST_INDEX_THREAD;
                    
                    for (int iThreadCounter = 0; iThreadCounter < oPiConfig.ServiceMaxThread; iThreadCounter++)
                    {
                        // create an object
                        oAPiWorkers[iThreadCounter] = new Worker(iThreadCounter + 1, oPiConfig.ServicePollingInterval);

                        // set properties on the object
                        oAPiWorkers[iThreadCounter].ServiceStarted = true;

                        // create a thread and attach to the object
                        ThreadStart st = new ThreadStart(oAPiWorkers[iThreadCounter].ExecuteTask);
                        oAPiWorkerThreads[iThreadCounter] = new Thread(st);
                    }

                    // start the threads
                    for (int iThreadCounter = 0; iThreadCounter < oPiConfig.ServiceMaxThread; iThreadCounter++)
                    {
                        oAPiWorkerThreads[iThreadCounter].Start();
                    }

                    // load uncomplete requested job
                    oPiUncompleteID = oPiPDFCreator.LoadUncompleted();
                    bPiServiceStarted = true;
                    Logger.WriteLog("PDF Generator Service has started.");
                    ePiState = ProcessState.CLEAR_STATE;
                    // start the main thread
                    new Thread(new ThreadStart(DoProcess)).Start();

                }
                catch (Exception ex)
                {
                    // Write error log.
                    Logger.WriteLog(ex);
                }

            }
            
            /// <summary>
            /// Stop running of exist threads
            /// </summary>
            protected override void OnStop()
            {
                bPiServiceStarted = false;

                for (int iThreadCounter = 0; iThreadCounter < oPiConfig.ServiceMaxThread; iThreadCounter++)
                {
                    // set flag to stop worker thread
                    oAPiWorkers[iThreadCounter].ServiceStarted = false;
                    // give it a little time to finish any pending work
                    oAPiWorkerThreads[iThreadCounter].Join(new TimeSpan(ZERO_HOUR, ZERO_MINUTE, PENDING_TIME_TO_STOP));
                }

                Logger.WriteLog("PDF Generator Service has stopped.");
            }

            /// <summary>
            /// Maintain process state uncompleted task and read request from message queue 
            /// then assign job to the worker thread.
            /// Waiting for worker thread is idle then assign the requested id to generate a pdf document.
            /// </summary>
            protected static void DoProcess()
            {
                bool bJobAssigned;              // flag job assign to thread
                Guid oRequestedID=Guid.Empty;   // requested id
                bool bDoProcessAssignJob;       // flag for do process assign job
                bool bForceProcess = false;     // Force generate a pdf

                try
                {
                    bDoProcessAssignJob = false;
                    // Loop when service has started.
                    while (bPiServiceStarted)
                    {

                        switch (ePiState)
                        {
                            case ProcessState.IDLE_STATE:
                                bDoProcessAssignJob = false;
                                // Change to next state.
                                ePiState = ProcessState.CLEAR_STATE;
                                break;
                            case ProcessState.CLEAR_STATE:
                                bDoProcessAssignJob = false;
                                // Clear existing message queue before waiting new request incoming.
                                oPiPDFCreator.ClearExistQueue();
                                Logger.WriteLog("PDF Generator Service purged existing message queue.");
                                ePiState = ProcessState.REDO_STATE;
                                break;
                            case ProcessState.REDO_STATE:
                                // Get requested id for do process assign job 
                                bDoProcessAssignJob = true;
                                oRequestedID = ReadUncompltedID();
                                bForceProcess = true;
                                // Redo until empty list change to next state
                                if (oPiUncompleteID.Count <= 0)
                                {
                                    Logger.WriteLog("PDF Generator Service redo uncomplete requested jobs.");
                                    ePiState = ProcessState.READ_STATE;
                                }
                                break;
                            case ProcessState.READ_STATE:
                                // Get requested id from message queue for do process assign job
                                bDoProcessAssignJob = true;
                                oRequestedID = oPiPDFCreator.ReadRequestFromMSMQ();
                                bForceProcess = false;
                                break;
                        }

                        // Process assign job
                        if (bDoProcessAssignJob)
                        {
                            bJobAssigned = false;

                            if (!oRequestedID.Equals(Guid.Empty))
                            {
                                // Loop until job has assign to a thread
                                while (!bJobAssigned)
                                {
                                    if (oAPiWorkers[iPiCurrentThread].ServiceIdle)
                                    {
                                        // if thread is idle assign job to this
                                        oAPiWorkers[iPiCurrentThread].ForceProcess = bForceProcess;
                                        oAPiWorkers[iPiCurrentThread].RequestedID = oRequestedID;
                                        bJobAssigned = true;
                                    }
                                    // Increment index to next thread sequence.
                                    iPiCurrentThread++;

                                    if (iPiCurrentThread >= oPiConfig.ServiceMaxThread)
                                    {
                                        // Reset if index overflow
                                        iPiCurrentThread = FIRST_INDEX_THREAD;
                                    }
                                }
                            }
                        }
                        // Sleep for wait incoming message queue
                        Thread.Sleep(new TimeSpan(ZERO_HOUR, ZERO_MINUTE, oPiConfig.ServicePollingInterval));
                    }
                }
                catch (Exception ex)
                {
                    // Write error log.
                    Logger.WriteLog(ex);
                }
            }

            /// <summary>
            /// Get uncomplete requested id from list queue.
            /// </summary>
            /// <returns>Uncomplete requested id</returns>
            protected static Guid ReadUncompltedID()
            {
                Guid oID = Guid.Empty;
                try
                {
                    // Read from head queue list
                    if (oPiUncompleteID.Count > UNCOMPLETED_BASE_INDEX)
                    {
                        oID = oPiUncompleteID[UNCOMPLETED_BASE_INDEX];
                        oPiUncompleteID.RemoveAt(UNCOMPLETED_BASE_INDEX);
                    }
                        
                }
                catch (Exception ex)
                {
                    // Write error log.
                    Logger.WriteLog(ex);
                }
                return oID;
            }

    #endregion

    }
}
