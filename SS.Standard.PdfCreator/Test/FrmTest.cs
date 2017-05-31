using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Messaging;

using Test.SSG.WebService;

using SSG.PDF;
using SSG.Config;
using SSG.Logging;

namespace Test
{
    /// <summary>
    /// This class show example how to calling windows and web services
    /// for generate a pdf document from string content and latex file.
    /// How to checking a requested status. Where is a pdf document location.
    /// </summary>
    /// 
    /// <author>Phoonperm Suwannarattaphoom</author>
    /// <version>1.0</version>
    public partial class FrmTest : Form
    {
        #region Variable declaration

            private SSGConfig m_oPiConfig;
            private PDFCreator m_oPiLibrary;
            private PDFCreatorWebServiceSoapClient m_oPiWebService;
            private string m_strPiSampleFileName;

            List<Guid> oPiUncompleteID;

        #endregion

        #region Constructor

            /// <summary>
            /// Initialize and loading configuration.
            /// </summary>
            public FrmTest()
            {
                InitializeComponent();
                m_oPiLibrary = new PDFCreator();
                m_oPiWebService = new PDFCreatorWebServiceSoapClient();
                m_strPiSampleFileName = string.Empty;
                m_oPiConfig = SSGConfigFactory.GetConfig();
            }

        #endregion

        #region Methods

            /// <summary>
            /// Create a sample latex string content
            /// </summary>
            /// <returns>The latex string</returns>
            private string GetTestLatexString()
            {
                // Building latex string content
                StringBuilder oStringBuilder = new StringBuilder();
                oStringBuilder.AppendLine("%% LyX 1.6.1 created this file.  For more info, see http://www.lyx.org/.");
                oStringBuilder.AppendLine("%% Do not edit unless you really know what you are doing.");
                oStringBuilder.AppendLine("\\documentclass[thai]{article}");
                oStringBuilder.AppendLine();
                oStringBuilder.AppendLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% LyX specific LaTeX commands.");
                oStringBuilder.AppendLine("%% Because html converters don't know tabularnewline");
                oStringBuilder.AppendLine("\\providecommand{\\tabularnewline}{\\\\}");
                oStringBuilder.AppendLine();
                oStringBuilder.AppendLine("\\usepackage[thai]{babel}");
                oStringBuilder.AppendLine("\\usepackage{thswitch}");
                oStringBuilder.AppendLine();
                oStringBuilder.AppendLine("\\begin{document}");
                oStringBuilder.AppendLine("\\begin{tabular}{|c|c|c|}");
                oStringBuilder.AppendLine("\\hline ");
                oStringBuilder.AppendLine("ทดสอบ & อิอิ & อุอุ\\tabularnewline");
                oStringBuilder.AppendLine("\\hline");
                oStringBuilder.AppendLine("\\hline");
                oStringBuilder.AppendLine("ปู & ปู่ & ปู้\\tabularnewline");
                oStringBuilder.AppendLine("\\hline");
                oStringBuilder.AppendLine("อัน & อั่น & อั้น\\tabularnewline");
                oStringBuilder.AppendLine("\\hline");
                oStringBuilder.AppendLine("ดุกดิ๊ก & กะดูก & กาดิ๊ก\\tabularnewline");
                oStringBuilder.AppendLine("\\hline");
                oStringBuilder.AppendLine("\\end{tabular}");
                oStringBuilder.AppendLine("\\end{document}");

                return oStringBuilder.ToString();

            }

            /// <summary>
            /// Generate a pdf file from latex string
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void GeneratePDFButton_Click(object sender, EventArgs e)
            {
                // Select windows or web service
                if (LibraryRadio.Checked)
                    IDTextBox.Text = m_oPiLibrary.GeneratePDF(GetTestLatexString()).ToString();
                else
                    IDTextBox.Text = m_oPiWebService.GeneratePDFFromContent(GetTestLatexString()).ToString();
            }

            /// <summary>
            /// Get status from database
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void GetStatusButton_Click(object sender, EventArgs e)
            {
                // Select windows or web service
                if (LibraryRadio.Checked)
                    StatusLabel.Text = m_oPiLibrary.GetStatus(IDTextBox.Text).ToString();
                else
                    StatusLabel.Text = m_oPiWebService.GetStatus(IDTextBox.Text).ToString();  
            }

            /// <summary>
            /// Get location of pdf document
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void PdfLocationButton_Click(object sender, EventArgs e)
            {
                // Select windows or web service
                if (LibraryRadio.Checked)
                    PdfLocationLink.Text = m_oPiLibrary.GetPdfFileUri(IDTextBox.Text).ToString();
                else
                    PdfLocationLink.Text = m_oPiWebService.GetPdfUri(IDTextBox.Text).ToString();  
            }

            /// <summary>
            /// Clear text display on form
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void ClearButton_Click(object sender, EventArgs e)
            {
                try
                {
                    // Assign empty string
                    IDTextBox.Text = string.Empty;
                    StatusLabel.Text = string.Empty;
                    PdfLocationLink.Text = string.Empty;
                }
                catch (MessageQueueException tex)
                {
                    Console.WriteLine(tex.Message);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
           }

            /// <summary>
            /// Open a pdf document with internet explorer
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void PdfLocationLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
                System.Diagnostics.Process.Start("IExplore", PdfLocationLink.Text);
            }

            /// <summary>
            /// Generate a pdf file from a sample latex file 
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void GeneratePDFExistFileButton_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(m_strPiSampleFileName))
                    Sample1Radio_Click(this, null);
                // Copy a sample latex file into tex directory
                File.Copy(Environment.CurrentDirectory + "\\" + m_strPiSampleFileName, m_oPiConfig.TexDirectory + "\\" + IDTextBox.Text + ".tex");
                // Select windows or web service
                if (LibraryRadio.Checked)
                    IDTextBox.Text = m_oPiLibrary.GeneratePDF(new Guid(IDTextBox.Text)).ToString();
                else
                    IDTextBox.Text = m_oPiWebService.GeneratePDFFromFile(IDTextBox.Text).ToString();

            }

            /// <summary>
            /// Assign latex fileneme of example 1
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void Sample1Radio_Click(object sender, EventArgs e)
            {
                IDTextBox.Text = Guid.NewGuid().ToString();
                m_strPiSampleFileName = "Tex\\Test_Thaifonts1.tex";
            }

            /// <summary>
            /// Assign latex fileneme of example 2
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void Sample2Radio_Click(object sender, EventArgs e)
            {
                IDTextBox.Text = Guid.NewGuid().ToString();
                m_strPiSampleFileName = "Tex\\Test_Thaifonts2.tex";
            }

            /// <summary>
            /// Assign latex fileneme of example 3
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void Sample3Radio_Click(object sender, EventArgs e)
            {
                IDTextBox.Text = Guid.NewGuid().ToString();
                m_strPiSampleFileName = "Tex\\Test_Thaifonts3.tex";
            }

            /// <summary>
            /// Assign latex fileneme of example 4
            /// </summary>
            /// <param name="sender">sender object</param>
            /// <param name="e">event arguments</param>
            private void Sample4Radio_Click(object sender, EventArgs e)
            {
                IDTextBox.Text = Guid.NewGuid().ToString();
                m_strPiSampleFileName = "Tex\\Test_Thaifonts4.tex";
            }

            private void GenerateErrorButton_Click(object sender, EventArgs e)
            {
                // Select windows or web service
                if (LibraryRadio.Checked)
                    IDTextBox.Text = m_oPiLibrary.GeneratePDF("Error Content").ToString();
                else
                    IDTextBox.Text = m_oPiWebService.GeneratePDFFromContent("Error Content").ToString();
            }

        #endregion

          

    }
}
