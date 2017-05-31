using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

using SSG.PDF;
using SSG.Config;
using SSG.Logging;

namespace SSG.PDF.WebService
{

    /// <summary>
    /// The class PDFCreatorWebService contains methods for performing creating tex file from string content, 
    /// generate pdf document from tex file, checking status state of generating the pdf documents and 
    /// get location of generated a pdf file.
    /// </summary>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    [WebService(Namespace = "http://www.softsquaregroup.com/",
                Name="PDF Creator Web Service", 
                Description="Web service for generate pdf documents.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PDFCreatorWebService : System.Web.Services.WebService
    {

        /// <summary>
        /// Starting index of path file
        /// </summary>
        private const int START_PATH_INDEX = 6;

        /// <summary>
        /// Generate a pdf document file from the string content.
        /// </summary>
        /// <param name="strContent">The latex string content.</param>
        /// <returns>Return the uniqueidentified of tex and pdf files. 
        /// Empty uniqueidentified if creating failure.</returns>
        /// <seealso cref="SSG.PDF.PDFCreator.GeneratePDF(Guid oID)"/>
        [WebMethod(Description = "Generate the PDF document form string content.")]
        public Guid GeneratePDFFromContent(string strContent)
        {
            // Calling GeneratePDF from object library with configuration file from execution path
            PDFCreator oPDF = new PDFCreator(GetConfigFile());
            return oPDF.GeneratePDF(strContent);
        }

        /// <summary>
        /// Generate a pdf document file from the latex file.
        /// </summary>
        /// <param name="strID">The uniqueidentified string of tex file name.</param>
        /// <returns>Return the uniqueidentified of pdf files. 
        /// Empty uniqueidentified if creating failure.</returns>
        [WebMethod(Description = "Generate the PDF document form exist latex file.")]
        public Guid GeneratePDFFromFile(string strID)
        {
            // Calling GeneratePDF from object library with configuration file from execution path
            PDFCreator oPDF = new PDFCreator(GetConfigFile());
            return oPDF.GeneratePDF(new Guid(strID));
        }

        /// <summary>
        /// Get a pdf requested status from database.
        /// </summary>
        /// <param name="strID">The uniqueidentified string</param>
        /// <returns>The status of a pdf requested.</returns>
        [WebMethod(Description = "Get the status of generating a pdf document.")]
        public PDFCreator.PdfRequestStatus GetStatus(string strID)
        {
            // Calling GetStatus from object library with configuration file from execution path
            PDFCreator oPDF = new PDFCreator(GetConfigFile());
            return oPDF.GetStatus(strID);
        }

        /// <summary>
        /// Getting pdf file location from specified id.
        /// </summary>
        /// <param name="strID">The uniqueidentified string</param>
        /// <returns>The location of pdf file name</returns>
        [WebMethod(Description = "Get the location of generated a pdf document.")]
        public string GetPdfUri(string strID)
        {
            // Calling GetPdfFileUri from object library with configuration file from execution path
            PDFCreator oPDF = new PDFCreator(GetConfigFile());
            string strPath = this.Context.Request.ApplicationPath.ToString();
            //string strAppName = this.Context.Request.Url.AbsoluteUri.ToString();
           // string xxx = this.Context.Request.Url.AbsolutePath.ToString();
            string strDomain = this.Context.Request.Url.Host.ToString();
            string url = "http://" + strDomain + strPath + "/Bin/Pdf/" + oPDF.GetPdfFileName(strID);
            return url;
        }



        /// <summary>
        /// The dummy initialize component
        /// </summary>
        private void InitializeComponent()
        {   
        }

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
    }
}
