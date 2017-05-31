using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using SS.Standard.UI;
using SS.DB.Query;
namespace SCG.eAccounting.Web
{
    public partial class testApproveSMS : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       
        protected void btnSend_Click(object sender, EventArgs e)
        {
            Label7.Visible = false;
          string flag = apprvoe.Checked == true ? "Y" : "N";
          string result = ClientGetAsync.Main(TRANSID.Text, FROM.Text, TO.Text, TOKENID.Text, flag);
            // Create the state object.

            // Put the request into the state object so it can be passed around.
            //rs.Request = wreq;

            // Issue the async request.
            Label7.Visible = true;
            Label7.Text = "The Request for approval via by sms with tokenID : " + TOKENID.Text + " was successful";

            //Response.ClearHeaders();
            //Response.Clear();
            //Response.Write(result);
            //Response.End();
            TextBox1.Text = result;

          //  Response.Write("<script>showRespone('" + result + "');</script>");
            //this.ClientScript.RegisterStartupScript(GetType(), "onload", "<script>alert('" + result + "');</script>");
            //UpdatePanel1.Update();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label14.Visible = false;
            string strSMID = txtSMID.Text;
            string strFROM = txtFROM.Text;
            string strDetail = txtDetail.Text;
            string strStatus = (RadioButton1.Checked == true ? "OK" : "ERR");

            string result = ClientGetAsync2.Main(strSMID, strFROM, strDetail, strStatus);
            // Create the state object.

            // Put the request into the state object so it can be passed around.
            //rs.Request = wreq;

            // Issue the async request.
            Label14.Visible = true;
            Label14.Text = "The Request was successful";
          //  Response.ClearHeaders();
          //  Response.Clear();
            TextBox2.Text = result;
            //Response.Write("<script>showRespone('" + result + "');</script>");
            // this.ClientScript.RegisterStartupScript(GetType(), "onload", "<script>alert('" + result + "');</script>");

            //UpdatePanel2.Update();
          //  Response.Write(result);
          //  Response.End();
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

            public static string Main(string TRANSID, string FROM, string TO, string TOKENID, string apprvoe)
            {
                string strTRANSID = TRANSID;
                string strFROM = FROM;
                string strTO = TO;
                string strCONTENT = TOKENID + apprvoe;


                string smsConten = "TRANSID=" + strTRANSID + "&FROM=" + strFROM + "&TO=" + strTO + "&CONTENT=" + strCONTENT;


                // Get the URI from the command line.
                // Uri httpSite = new Uri("http://smsservicedev.scg.co.th/receive.aspx/");
//                Uri httpSite = new Uri("http://" + HttpContext.Current.Server.MachineName + HttpContext.Current.Request.ApplicationPath + "/receive.aspx/");
                
                Uri httpSite = new Uri("http://scgaccountdev.scg.co.th:82/receive.aspx/");
               // Uri httpSite = new Uri("http://scgaccountqas.scg.co.th/receive.aspx/");
                //Uri httpSite = new Uri("http://SCG-EACCOUNTING:84/receive.aspx/");


                // Create the request object.
                WebRequest wreq = WebRequest.Create(httpSite);

                //WebProxy proxyAuthen = new WebProxy("", 8080);
                WebProxy proxyAuthen = new WebProxy(ParameterServices.ProxyServer + ":" + ParameterServices.ProxyPort);
                proxyAuthen.BypassProxyOnLocal = true;
                proxyAuthen.Credentials = new NetworkCredential(ParameterServices.ProxyUserName, ParameterServices.ProxyPassword);
                wreq.Proxy = proxyAuthen;
                wreq.Timeout = 100000;

                wreq.Method = "POST";
                // Create POST data and convert it to a byte array.
                string postData = smsConten;
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
                allDone.WaitOne();


                return rs.RequestData.ToString();
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



        public class ClientGetAsync2
        {
            public static ManualResetEvent allDone = new ManualResetEvent(false);

            const int BUFFER_SIZE = 1024;

            public static string Main(string SMID, string FROM, string Detail, string apprvoe)
            {
                string strSMID = SMID;
                string strFROM = FROM;
                string strDetail = Detail;
                string strStatus = apprvoe;


                string smsConten = "SMID=" + strSMID + "&FROM=" + strFROM + "&DETAIL=" + strDetail + "&STATUS=" + strStatus;
                

                // Get the URI from the command line.
                // Uri httpSite = new Uri("http://smsservicedev.scg.co.th/receive.aspx/");
                //Uri httpSite = new Uri("http://" + HttpContext.Current.Server.MachineName + HttpContext.Current.Request.ApplicationPath+ "/DLVReceive.aspx/");
                Uri httpSite = new Uri("http://scgaccountdev.scg.co.th:82/DLVReceive.aspx/");
                //Uri httpSite = new Uri("http://SCG-EACCOUNTING:84/DLVReceive.aspx/");
                //Uri httpSite = new Uri("http://scgaccountqas.scg.co.th/DLVReceive.aspx/");

                // Create the request object.
                WebRequest wreq = WebRequest.Create(httpSite);

                //WebProxy proxyAuthen = new WebProxy("", 8080);
                WebProxy proxyAuthen = new WebProxy(ParameterServices.ProxyServer + ":" + ParameterServices.ProxyPort);
                proxyAuthen.BypassProxyOnLocal = true;
                proxyAuthen.Credentials = new NetworkCredential(ParameterServices.ProxyUserName, ParameterServices.ProxyPassword);
                wreq.Proxy = proxyAuthen;
                wreq.Timeout = 100000;

                wreq.Method = "POST";
                // Create POST data and convert it to a byte array.
                string postData = smsConten;
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
                allDone.WaitOne();


                return rs.RequestData.ToString();
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
}
