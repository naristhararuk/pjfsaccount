using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using SCG.eAccounting.Interface.RefreshWorkFlowPermission.DAL;

namespace SCG.eAccounting.Interface.RefreshWorkFlowPermission
{
    public class ListenerThread
    {
        private TimeSpan _MarkingQueue = default(TimeSpan);
        private MarkerProcessTime MarkerProcessTimeInstance = default(MarkerProcessTime);
        private TcpListener listener        = default(TcpListener);
        private TcpClient client            = default(TcpClient);
        private Thread tMarkerProcessTime   = default(Thread);
        public int ProcessTimeLength { get; set; }
        public int listenerPort { get; set; }
       
        public ListenerThread()
        {
            try
            {
                listenerPort = Factory.RefreshWorkFlowPermissionListernerPort;
                ProcessTimeLength = Factory.RefreshWorkFlowPermissionProcessTime;
                MarkerProcessTimeInstance = new MarkerProcessTime();
                MarkerProcessTimeInstance.FirstServiceStart = true;
               // _MarkingQueue = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0); // new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0, 0);
                _MarkingQueue = new TimeSpan(0,0, 0, 0);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void ListenerProcess()
        {
            try
            {
                MarkerProcessTimeInstance._MarkingQueue = _MarkingQueue;
                tMarkerProcessTime = new Thread(MarkerProcessTimeInstance.NotifyRefreshProcess);
                tMarkerProcessTime.Start();

                while (true)
                {
                    try
                    {
                        listener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), listenerPort);
                        listener.Start();
                        Log.WriteMsgLogs("Begin TcpListener and Wait for signal from requester.");

                        NetworkStream stream = listener.AcceptTcpClient().GetStream();

                        Log.WriteMsgLogs("Received data from the signal from requester");

                        if (readStreamMessage(stream).ToLower().Equals("scgrefreshpermission"))
                        {
                            _MarkingQueue = new TimeSpan(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).Add(new TimeSpan(0,Factory.RefreshWorkFlowPermissionProcessTime,0)); 
                            //DateTime.Now.AddMinutes(ProcessTimeLength);
                           // _MarkingQueue = new TimeSpan(dt.Day, dt.Hour, dt.Minute, 0, 0);

                            MarkerProcessTimeInstance._MarkingQueue = _MarkingQueue;

                            MarkerProcessTimeInstance.FirstServiceStart = false;
                            Log.WriteMsgLogs("Set marking queue to " + _MarkingQueue.ToString());
                            //Force Thread worker abort when have a new signal
                            MarkerProcessTimeInstance.ForceThreadAbort();

                        }
                        listener.Stop();

                        Log.WriteMsgLogs("End Tcplistener stop &  Client Closed.");

                    }
                    catch (Exception exd)
                    {

                        Log.WriteLogs(exd);
                        Thread.Sleep(new TimeSpan(0,1,0));
                    }
                  
                }
            
            }
            catch (Exception ex)
            {
                Log.WriteLogs(ex);
            }
        }

        private string readStreamMessage(NetworkStream stream)
        {
            StringBuilder myCompleteMessage = new StringBuilder();

            try
            {
                string responeMsgSuccess = " Received => Next Processtime : ";
                string responeMsgFailed = " Can not read message ";
                if (stream.CanRead)
                {
                    byte[] myReadBuffer = new byte[1024];
                    int numberOfBytesRead = 0;

                    // Incoming message may be larger than the buffer size.
                    do
                    {
                        numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                    }
                    while (stream.DataAvailable);

                    //Send respone message to client
                    DateTime dt = DateTime.Now.AddMinutes(ProcessTimeLength);
                    TimeSpan newDTProcessRespone = new TimeSpan(dt.Day, dt.Hour, dt.Minute, 0, 0);

                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(responeMsgSuccess + newDTProcessRespone);
                    stream.Write(data, 0, data.Length);
                    // Print out the received message to the console.
                   // Log.ShowMessage(" Received the following message : " + myCompleteMessage.ToString() );
                    //Log.ShowMessage(responeMsgSuccess + _MarkingQueueRespone);

                }
                else
                {
                    //Send respone message to client
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(responeMsgFailed);
                    stream.Write(data, 0, data.Length);

                   //Log.ShowMessage("Sorry.  You cannot read from this NetworkStream.");
                }

            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }
            
            return myCompleteMessage.ToString();
        }

        public void ForceThreadAbort()
        {
            try
            {
                Log.WriteMsgLogs("ListenerThread : ForceThreadAbort by user or program");
                MarkerProcessTimeInstance.ForceThreadAbort();
                if (tMarkerProcessTime != null && tMarkerProcessTime.IsAlive)
                {
                    tMarkerProcessTime.Abort();
                    tMarkerProcessTime.Join(new TimeSpan(0, 1, 0));
                }

            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }
        }

    }
}
