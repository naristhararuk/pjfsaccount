using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web;

using System.Threading;

using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.VisualBasic;

using SS.SU.BLL;
using SS.SU.DTO;
using SS.Standard.Security;
using SS.Standard.CommunicationService.DTO;


namespace SS.Standard.CommunicationService.Implement
{
    /// <summary>
    /// SMS API for send short message through internet
    /// </summary>
    
    public class SMS : ISMS
    {
        //public ISuSmsLogService SuSmsLogService { get; set; }
        //public IUserAccount UserAccount { get; set; }

        /// <summary>
        /// SMS Gateway Server must be IP Address Only
        /// </summary>
        public string SMSGateWayServer { get; set; }

        /// <summary>
        /// SMS Gateway Server Port must be integer only
        /// </summary>
        public int SMSGateWayPort { get; set; }

        /// <summary>
        /// User Name for access through the proxy server
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// Password for access through the proxy server
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// Proxy Server Name or IP Address
        /// </summary>
        public string ProxyServer { get; set; }
        /// <summary>
        /// Proxy Server Port must be integer only
        /// </summary>
        public int ProxyPort { get; set; }

       

        public bool SendStatus { get; set; }

        public string TRANSID { get; set; }
        public string FROM { get; set; }
        public string TO { get; set; }
        public string CMD { get; set; }
        public string REPORT { get; set; }
        public string CHARGE { get; set; }
        public string CODE { get; set; }
        public string CTYPE { get; set; }
        public string CONTENT { get; set; }

        public bool useProxy { get; set; }


        private DateTime SendDate;
       

        public SMS()
        {
           
        }

        public string Send()
        {

           string result = ClientGetAsync.Main(
               SMSGateWayServer,
               SMSGateWayPort,
               useProxy,
               ProxyServer,
               ProxyPort,
               ProxyUserName,
               ProxyPassword,
               TRANSID,
               CMD,
               FROM,
               TO,
               REPORT,
               CHARGE,
               CODE,
               CTYPE,
               CONTENT
                );

            SendDate = new DateTime();
       
        
            SendStatus = true;
            return result;
          
            

          
        }

       
    }

  

    public class RequestState
    {
        const int BufferSize = 1024;
        public StringBuilder RequestData;
        public byte[] BufferRead;
        public WebRequest Request;
        public Stream ResponseStream;
        public Decoder StreamDecode = Encoding.UTF8.GetDecoder();

        public RequestState()
        {
            BufferRead = new byte[BufferSize];
            RequestData = new StringBuilder(String.Empty);
            Request = null;
            ResponseStream = null;
        }
    }
    public class ClientGetAsync
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        const int BUFFER_SIZE = 1024;

        public static string Main(
            string SMSGateWayServer,
            int SMSGateWayPort,
            bool useProxy,
            string ProxyServer,
            int ProxyPort,
            string ProxyUserName,
            string ProxyPassword,
            string TRANSID,
            string CMD,
            string FROM,
            string TO,
            string REPORT,
            string CHARGE,
            string CODE,
            string CTYPE,
            string CONTENT
            )
        {

            // Get the URI from the command line.
            Uri httpSite = new Uri("http://" + SMSGateWayServer + ":" + SMSGateWayPort+"/");

            // Create the request object.
            WebRequest wreq = WebRequest.Create(httpSite);


            if (useProxy)
            {
                WebProxy webproxy = new WebProxy(ProxyServer + ":" + ProxyPort);
                webproxy.Credentials = new NetworkCredential(ProxyUserName, ProxyPassword);
                webproxy.BypassProxyOnLocal = true;
                wreq.Proxy = webproxy;
            }


            wreq.Timeout = 100000;  // 100 Second

            wreq.Method = "POST";
            // Create POST data and convert it to a byte array.
            StringBuilder smsBuilder = new StringBuilder(string.Empty);
            smsBuilder.AppendFormat("TRANSID={0}", TRANSID == null ? "BULK" : TRANSID);
            smsBuilder.AppendFormat("&CMD={0}", CMD == null ? "SENDMSG" : CMD);
            smsBuilder.AppendFormat("&FROM={0}", FROM == null ? "66898113444" : FROM);
            smsBuilder.AppendFormat("&TO={0}", TO);
            smsBuilder.AppendFormat("&REPORT={0}", REPORT == null ? "N" : REPORT.Trim());
            smsBuilder.AppendFormat("&CHARGE={0}", CHARGE == null ? "N" : CHARGE.Trim());
            smsBuilder.AppendFormat("&CODE={0}", CODE == null ? "SCG_Accounting_Bulk_SMS" : CODE);
            smsBuilder.AppendFormat("&CTYPE={0}", CTYPE == null ? "TEXT" : CTYPE);
            smsBuilder.AppendFormat("&CONTENT={0}", CONTENT);


            string postData = smsBuilder.ToString();

            // string postData = "CMD=SENDMSG&TRANSID=BULK&FROM=66898113444&TO=66865709511&CHARGE=N&REPORT=Y&CODE=SCG_Accounting_Bulk_SMS&CTYPE=TEXT&CONTENT=testPushMessageNajaเดชนะครับ";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            wreq.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            wreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = wreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();

            // Create the state object.
            RequestState rs = new RequestState();

            // Put the request into the state object so it can be passed around.
            rs.Request = wreq;

            // Issue the async request.
            IAsyncResult r = (IAsyncResult)wreq.BeginGetResponse(
               new AsyncCallback(RespCallback), rs);

            // Wait until the ManualResetEvent is set so that the application 
            // does not exit until after the callback is called.

            Thread.Sleep(300);

            string returnValues = string.Empty;
            //while (!allDone.WaitOne())
            //{
            //    if (allDone.WaitOne())
            //    {
            //        break;
            //    }
            //}
            if (allDone.WaitOne())
            {
                returnValues = rs.RequestData.ToString();
            }



            return returnValues;
           // Console.WriteLine(rs.RequestData.ToString());

        }



        private static void RespCallback(IAsyncResult ar)
        {
            // Get the RequestState object from the async result.
            RequestState rs = (RequestState)ar.AsyncState;

            // Get the WebRequest from RequestState.
            WebRequest req = rs.Request;

            // Call EndGetResponse, which produces the WebResponse object
            //  that came from the request issued above.
            WebResponse resp = req.EndGetResponse(ar);

            //  Start reading data from the response stream.
            Stream ResponseStream = resp.GetResponseStream();

            // Store the response stream in RequestState to read 
            // the stream asynchronously.
            rs.ResponseStream = ResponseStream;

            //  Pass rs.BufferRead to BeginRead. Read data into rs.BufferRead
            IAsyncResult iarRead = ResponseStream.BeginRead(rs.BufferRead, 0,
               BUFFER_SIZE, new AsyncCallback(ReadCallBack), rs);
        }


        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            // Get the RequestState object from AsyncResult.
            RequestState rs = (RequestState)asyncResult.AsyncState;

            // Retrieve the ResponseStream that was set in RespCallback. 
            Stream responseStream = rs.ResponseStream;

            // Read rs.BufferRead to verify that it contains data. 
            int read = responseStream.EndRead(asyncResult);
            if (read > 0)
            {
                // Prepare a Char array buffer for converting to Unicode.
                Char[] charBuffer = new Char[BUFFER_SIZE];

                // Convert byte stream to Char array and then to String.
                // len contains the number of characters converted to Unicode.
                int len =
                   rs.StreamDecode.GetChars(rs.BufferRead, 0, read, charBuffer, 0);

                String str = new String(charBuffer, 0, len);

                // Append the recently read data to the RequestData stringbuilder
                // object contained in RequestState.
                rs.RequestData.Append(
                   Encoding.ASCII.GetString(rs.BufferRead, 0, read));

                // Continue reading data until 
                // responseStream.EndRead returns –1.
                IAsyncResult ar = responseStream.BeginRead(
                   rs.BufferRead, 0, BUFFER_SIZE,
                   new AsyncCallback(ReadCallBack), rs);
            }
            else
            {
                if (rs.RequestData.Length > 0)
                {
                    //  Display data to the console.
                    string strContent;
                    strContent = rs.RequestData.ToString();
                }
                // Close down the response stream.
                responseStream.Close();
                // Set the ManualResetEvent so the main thread can exit.
                allDone.Set();
            }
            return;
        }
    }  
}
