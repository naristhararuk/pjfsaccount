using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.BLL;

namespace SCG.eAccounting.Web.Forms.SCG.eAccounting.Programs
{
    public partial class ResponseTest : BasePage
    {
        public ISCGEmailService SCGEmailService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ctlOverRole.Initialize(1); // ID TEST
                HoldDetail1.Initialize(1); // ID TEST
                //RejectDetail1.Initialize(1);
                History1.Initialize(1);
            }
        }

        protected void ctlOverRoleSubmit_Click(object sender, EventArgs e)
        {
            ctlOverRole.SubmitResponse();
        }
        protected void ctlHoldDetail_Click(object sender, EventArgs e)
        {
            HoldDetail1.GetEventData(1);
        }

        protected void ctlSendEmail_Click(object sender, EventArgs e)
        {
            if (ctlEmailPetternDdl.SelectedValue.Equals("1"))
            {
                //userID 128 is kamon
                SCGEmailService.SendEmailEM01(18, 1, "c14b8e2c-0a2a-4ec2-9038-136d0b141c8e");
            }
            else if(ctlEmailPetternDdl.SelectedValue.Equals("2"))
            {
                SCGEmailService.SendEmailEM02(15, 1, null);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("3"))
            {
                SCGEmailService.SendEmailEM03(80, 1);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("4"))
            {
                SCGEmailService.SendEmailEM04(230, 1);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("5"))
            {
                SCGEmailService.SendEmailEM05(57, 1, new List<long> { 1 }, false);
                SCGEmailService.SendEmailEM05(57, 1, new List<long> { 1 }, true);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("6"))
            {
                SCGEmailService.SendEmailEM06(33, 1, new List<long> { 1 });
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("7"))
            {
                SCGEmailService.SendEmailEM07(1);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("8"))
            {
                SCGEmailService.SendEmailEM08(1,"admin");
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("9"))
            {
                SCGEmailService.SendEmailEM09(75, "", "testststs", string.Empty);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("10"))
            {
                SCGEmailService.SendEmailEM10(18, 1, "", "Remark Test", false);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("11"))
            {
                SCGEmailService.SendEmailEM11(2);
            }
            else if (ctlEmailPetternDdl.SelectedValue.Equals("12"))
            {
                SCGEmailService.SendEmailEM12(1, "admin");
            }

            
        }
    }
}
