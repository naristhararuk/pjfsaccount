using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ProgressBar : System.Web.UI.UserControl
    {
        
        public string AssociatedUpdatePanelID
        {
            get { return UpdatePanelGridViewProgress.AssociatedUpdatePanelID; }
            set { UpdatePanelGridViewProgress.AssociatedUpdatePanelID = value; }
        }

    }
}