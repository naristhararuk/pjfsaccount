using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SS.Standard.UI
{
    [Serializable]
   public class SSReportViewer : ReportViewer
   {
       public bool EnableSendEmail { get; set; }
       public SS.Standard.UI.BaseReportViewers.GenerateType GenerateFileType { get; set; }

       public event ObjectOnClickButtonReturnEventHandler OnClickButtonReturn;
       public delegate void ObjectOnClickButtonReturnEventHandler(object sender, ObjectOnClickButtonReturnArgs e);

        public event ObjectOnClickButtonCallingEventHandler OnObjectOnClickButtonCalling;
        public delegate void ObjectOnClickButtonCallingEventHandler(object sender, ObjectOnClickButtonCallingEventArgs e);
        
       
       protected override void AddedControl(Control control, int index)
       {
          
               if ("Microsoft.Reporting.WebForms.ToolbarControl".Equals(control.GetType().FullName))
               {
                   if (EnableSendEmail)
                   {
                   foreach (Control cc in control.Controls)
                   {
                       if (cc.ToString() == "Microsoft.Reporting.WebForms.PrintGroup")
                       {
                           ImageButton imgButton = new ImageButton();
                           imgButton.ID = "ctlSendEmailButton";
                           imgButton.ImageUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority.ToString() + "/ImageFiles/Icon/sendemail.png";
                           imgButton.Click += new ImageClickEventHandler(ImageButton1_Click);
                           imgButton.ToolTip = "Send Email";
                           cc.Controls.Add(imgButton);
                           break;
                       }
                   }

               }
           }
           base.AddedControl(control, index);
       }
       protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
       {
           CallOnObjectOnClickButtonReturn(true);
       }
       protected void CallOnObjectOnClickButtonCalling()
       {
           ObjectOnClickButtonCallingEventArgs callingArgs = new ObjectOnClickButtonCallingEventArgs();
           if (OnObjectOnClickButtonCalling != null)
               OnObjectOnClickButtonCalling(this, callingArgs);
       }
       protected void CallOnObjectOnClickButtonReturn(object returnedObject)
       {
           ObjectOnClickButtonReturnArgs args = new ObjectOnClickButtonReturnArgs();
           args.ObjectOnClickButtonReturn = returnedObject;
           if (OnClickButtonReturn != null)
               OnClickButtonReturn(this, args);
       }
       public class ObjectOnClickButtonReturnArgs : EventArgs
       {
           public object ObjectOnClickButtonReturn { get; set; }
       }
       public class ObjectOnClickButtonCallingEventArgs : EventArgs
       {
           public object OnObjectOnClickButtonCalling { get; set; }
       }

    }
}
