using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;


namespace SCG.eAccounting.Web
{
    public partial class GetType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listFile.Items.Count; i++)
            {
                string pathWriteFile = @"C:\EXPORT_DATA\" + listFile.Items[i].ToString() + ".txt";

                string line = "";
                StringBuilder strControl = new StringBuilder(string.Empty);

                StreamReader strRead = new StreamReader(txtPath.Text + listFile.Items[i].ToString() + ".designer.cs");
                while ( (line = strRead.ReadLine()) != null)
                {
                    if (line.Trim().ToString().StartsWith("protected global::"))
                    {
                        string[] strComplete = line.Trim().Replace("protected global::", "").Replace(";", "").Split(' ');
                        if (strComplete.Length > 0)
                        {
                            //ListBox1.Items.Add(strComplete[0].ToString());
                            //ListBox2.Items.Add(strComplete[1].ToString());
                            strControl.Append(strComplete[0].ToString()).Append("\t\t\t").Append(strComplete[1].ToString()).Append("\r\n");
                        }
                    }
                }

                
                StreamWriter sw = new StreamWriter(pathWriteFile);
                sw.Write(strControl.ToString());
                sw.Flush();
                sw.Close();

                strRead.Close();
                strRead.Dispose();
            }
        }

        protected void btnGetFile_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(txtPath.Text);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.FullName.EndsWith(".aspx.designer.cs"))
                    listFile.Items.Add(fi.Name.Replace(".designer.cs", ""));
            }
        }

        protected void btnGetFile_Click1(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            SCG.eAccounting.Web.Forms.SU.Programs.UserProfile a = new SCG.eAccounting.Web.Forms.SU.Programs.UserProfile();
            Page page = a.Page;

            //for (int i = 0; i < a.Controls.Count; i++)
            //    ListBox1.Items.Add(a.Controls[i].ID.ToString());

            //for (int i = 0; i < page.Controls.Count; i++)
            //    ListBox1.Items.Add(page.Controls[i].ID.ToString());

            //object p = Activator.CreateInstance(typeof(SCG.eAccounting.Web.Forms.SU.Programs.Announcement));
            //if (p is Page)
            //{
                
            //    Page b = (Page)p;
                
                

            //    for (int i = 0; i < b.Controls.Count; i++)
            //        ListBox1.Items.Add(b.Controls[i].ID.ToString());
            //}
        }
    }
}
