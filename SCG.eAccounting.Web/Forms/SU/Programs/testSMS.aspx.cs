using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.CommunicationService;
using SS.Standard.CommunicationService.DTO;
using SS.Standard.UI;

using SCG.eAccounting.Report;
using System.IO;
using SS.Standard.CommunicationService.Implement;
using SS.SU.BLL;
using SS.SU.Query;
using SS.DB.Query;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class testSMS : BasePage
    {
        public ISMSService SMSService { get; set; }
        public IEmailService EmailService { get; set; }
        public ISuSmsLogService SuSmsLogService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Send_Click(object sender, EventArgs e)
        {
          

      
            SMSDTO smsDTO = new SMSDTO();

            smsDTO.To = TO.Text;
            smsDTO.Content = CONTENT.Text;

            smsDTO.UseProxy = true;
            bool result = SMSService.Send(smsDTO);
            string xx = result.ToString();

        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
                EmailDTO email = new EmailDTO();

                email.MailSendTo.Add(new AddMailSendTo() { Name = "น้องเดช", Email = "apidesh_w@softsquaregroup.com" });

                email.Sender.Email = "apidesh@hotmail.com";
                email.Sender.Name = "Desh";
                email.MailSubject = string.Format("Employee Request : {0} Approve/Reject", "22222222222");
                email.MailSendingType = Email.MailType.HTML;
            
                email.MailBody = "test send email from SCG Server";

                EmailService.SendEmail(email);
        }

     

        protected void btnReporting_Click(object sender, EventArgs e)
        {
            string outputFolder = "C:\\ReportOutPutWeb";
            string reportpath = "eAccountingReports";
            string reportname = "TAReport";
            string filesname = reportname + new Guid().ToString() + new Random(1).Next(100000).ToString();
            List<ReportParameter> param = new List<ReportParameter>();
            ReportParameter param1 = new ReportParameter();
            param1.ParamterName = "TADocID";
            param1.ParamterValue = "47";
            param.Add(param1);
            FilesGenerator rp = new FilesGenerator();

            string reportServer = ParameterServices.ReportingURL;
            string ReportUserName = ParameterServices.ReportUserName;
            string ReportPassword = ParameterServices.ReportPassword;
            string ReportDomainName = ParameterServices.ReportDomainName;



            byte[] results = rp.GetByte(reportServer, ReportUserName, ReportPassword, ReportDomainName, reportpath, reportname, param, FilesGenerator.ExportType.PDF);
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            FileStream stream = File.Create(outputFolder + "\\" + filesname + ".pdf", results.Length);
            stream.Write(results, 0, results.Length);
            stream.Close();
        }
    }
}
