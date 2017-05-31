using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using SS.Standard.Security;

namespace SS.Standard.Utilities
{
    // Declare a clock class
    [ToolboxData("<{0}:Clock runat=server></{0}:Clock>")]
    public class Clock : WebControl
    {
        
        //Create one TimerControl   
        Timer timer = new Timer();
        // Create one label control for click value
        Label clockLabel = new Label();
        // Declare one Updatepanel
        UpdatePanel updatePanel = new UpdatePanel();
        
        // Now override the Load event of Current Web Control
        protected override void OnLoad(EventArgs e)
        {
            //updatePanel.SkinID = "SkClock";
            // Create Ids for Control
            timer.ID = ID + "_tiker";
            clockLabel.ID = ID + "_l";
            // get the contentTemplate Control Instance
            Control controlContainer = updatePanel.ContentTemplateContainer;
            // add Label and timer control in Update Panel
            controlContainer.Controls.Add(clockLabel);
            controlContainer.Controls.Add(timer);
            // Add control Trigger in update panel on Tick Event
            updatePanel.Triggers.Add(new AsyncPostBackTrigger() { ControlID = timer.ID, EventName = "Tick" });
            updatePanel.ChildrenAsTriggers = true;
            // Set default clock time in label
            clockLabel.Text = DateTime.Now.ToString();
            // Set Interval
            timer.Interval = 1000;
            // Add handler to timer
            timer.Tick += new EventHandler<EventArgs>(timer_Tick);

            updatePanel.RenderMode = UpdatePanelRenderMode.Block;
            //Add update panel to the base control collection.
            base.Controls.Add(updatePanel);
        }
        protected override void Render(HtmlTextWriter output)
        {
            base.Render(output);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // Set current date time in label to move current at each Tick Event
            //if (UserAccount.CURRENT_LanguageID.Equals(1))
            //    clockLabel.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt",new System.Globalization.CultureInfo("th-TH"));
            //else
            //    clockLabel.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt", new System.Globalization.CultureInfo("en-US"));
        }
    }
}
