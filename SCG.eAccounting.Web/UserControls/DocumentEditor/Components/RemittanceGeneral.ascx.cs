using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SCG.eAccounting.DTO.ValueObject;

using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor.Components
{
    public partial class RemittanceGeneral : BaseUserControl
    {
		#region Property
		public string Mode
		{
			get { return ctlMode.Text; }
			set { ctlMode.Text = value; }
		}
		#endregion

		#region Page_Load Event
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                IList<SCG.eAccounting.DTO.ValueObject.Remittance> remittanceList = new List<SCG.eAccounting.DTO.ValueObject.Remittance>();

                SCG.eAccounting.DTO.ValueObject.Remittance remit = new SCG.eAccounting.DTO.ValueObject.Remittance();

                remit.ForeignAdvance = "25,000.00";
                remit.ExchangeRate = "35.5000";
                remit.ForeignRemit = "10.00";

                remittanceList.Add(remit);
                SCG.eAccounting.DTO.ValueObject.Remittance remit1 = new SCG.eAccounting.DTO.ValueObject.Remittance();

                remit1.ForeignAdvance = "0.00";
                remit1.ExchangeRate = "23.0000";
                remit1.ForeignRemit = "20.00";

                remittanceList.Add(remit1);

                RemitGridView.DataSource = remittanceList;
                RemitGridView.DataBind();
            }
		}
		#endregion

		#region Public Method
        public void ChangeMode()
        {
            if ((!string.IsNullOrEmpty(this.Mode)) && (this.Mode.Equals(ModeEnum.ReadWrite)))
            {
                RemitGridView.Enabled = true;
                RemitGridView.Columns[5].Visible = true;
            }
            else
            {
                RemitGridView.Enabled = false;
                RemitGridView.Columns[5].Visible = false;
            }
        }
		#endregion
    }
}