using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;

namespace SS.Standard.UI
{
    public class ServiceValidationSummary : Spring.Web.UI.Controls.ValidationSummary
    {
        static string script = " function SbErrorAnchor() {  " +
                " if(window.document.body.scrollHeight)" +
                "  { " +
                "    window.scrollTo(0,window.document.body.scrollHeight); " +
                "  } else if(screen.height) { " +
                "  window.scrollTo(0, window.screen.height); " +
                "  } " +
                " } ";
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.ValidationContainer.ValidationErrors.IsEmpty)
            {
                System.Web.HttpContext.Current.ClearError();
                ////sb.Append("window.location.href='#ctlOnErrorFocus';");
                //sb.Append("function SbErrorAnchor() {  window.location.href=\"#" + this.ClientID + "ErrorAnchor\";  }  ");
                //sb.AppendLine("document.getElementById('" + this.ClientID +  "ErrorAnchor').focus();");
                string funcCallErrorAnchor = "window.setTimeout('SbErrorAnchor();', 500);"; 
                ScriptManager.RegisterClientScriptBlock(this, typeof(ServiceValidationSummary), UniqueID, script, true);
                ScriptManager.RegisterStartupScript(this, typeof(ServiceValidationSummary), UniqueID, funcCallErrorAnchor, true);
            }

        }
   
        protected override void Render(HtmlTextWriter writer)
        {
                base.Render(writer);
                //if (System.Web.HttpContext.Current.AllErrors != null && System.Web.HttpContext.Current.AllErrors.Length > 0)
                //{
                //    System.Web.HttpContext.Current.ClearError();
                //    writer.WriteLine("<a name=\"" + this.ClientID + "ErrorAnchor\"></a>");
                //}
        }
    }
}
