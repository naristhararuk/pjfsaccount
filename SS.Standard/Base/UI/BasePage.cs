using System;
using System.Data;
using System.Configuration;
using System.Linq;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using SS.Standard.Security;
using SS.Standard.Utilities;
using System.IO;
using System.Text;
using AjaxControlToolkit;

namespace SS.Standard.Base.UI
{
    public class BasePage : System.Web.UI.Page
    {
        public MasterPage masterpage { get; set; }
        protected string ProgramCode;
        protected string CompanyName;
        protected bool Authentication = true;
        private ObjectStateFormatter _formatter = new ObjectStateFormatter();
        protected override void OnPreInit(EventArgs e)
        {
            //this.Theme  = ConfigurationManager.AppSettings["DefaultThemes"];
            //CompanyName = ConfigurationManager.AppSettings["CompanyName"];
            base.OnPreInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {

            if (Authentication && ProgramCode != "" && ProgramCode != null && ProgramCode != "SignIn")
            {
                UserEngine.Permission(UserEngine.ACL.View, ProgramCode, true);
                if (UserEngine.Permission(UserEngine.ACL.Add, ProgramCode, false))
                {
                    CanAdd = true;
                }
                if (UserEngine.Permission(UserEngine.ACL.Delete, ProgramCode, false))
                {
                    CanDelete = true;
                }

                if (UserEngine.Permission(UserEngine.ACL.Edit, ProgramCode, false))
                {
                    CanEdit = true;
                }

            }
          //  UserAccount.CURRENT_LanguageID = 1;
            base.OnLoad(e);
        }
        public void permission()
        {
              if (Authentication && ProgramCode != "" && ProgramCode != null) 
                UserEngine.Permission(UserEngine.ACL.View, ProgramCode, true);

        }
        public void RegisterScriptForGridView(BaseGridView ctlUserGrid)
        {
            //StringBuilder script = new StringBuilder();
            //script.Append(" function validateCheckBox(objChk, objFlag) ");
            //script.Append("{ ");
            //script.Append("CheckboxesCheckUncheck(objChk, objFlag, ");
            //script.Append("'" + ctlUserGrid.ClientID + "', '" + ctlUserGrid.HeaderRow.FindControl("ctlHeader").ClientID + "'); ");
            //script.Append("} ");

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "validateChkBox", script.ToString(), true);
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            //this.Page.Title = Convert.ToString(HttpContext.Current.Application["ApplicationTitle"]);

            base.OnPreRenderComplete(e);
            //if (ProgramCode != "" && ProgramCode != null) 
            //    Language.Translation.TranslatePage(ProgramCode, Page.Form.Controls);
        }

        [System.Web.Services.WebMethod]
        public static string GetDialog(string id)
        {
            SS.Standard.AlertMsg.Dialog dlg = new SS.Standard.AlertMsg.Dialog();
            return dlg.MsgID(id) + "!@#$%";


        }

        [System.Web.Services.WebMethod]
        public static void GetMessageDialog(string id)
        {
            Provider.MessageDAL.GetShowMessage(id);
        }
     

        [System.Web.Services.WebMethod]        
        public static string GetDialogConfirm(string id,object a)
        {
            string bb = a.ToString();
            
            SS.Standard.AlertMsg.Dialog dlg = new SS.Standard.AlertMsg.Dialog();
            return dlg.MsgID(id) + "!@#$%";
            //dlg.Msg(id);

        }
        [System.Web.Services.WebMethod]
        public static string GetDialogError(string id)
        {
           // string bb = a.ToString();

            SS.Standard.AlertMsg.Dialog dlg = new SS.Standard.AlertMsg.Dialog(id);
            return dlg.MsgID(id) + "!@#$%";
            //dlg.Msg(id);

        }

        public bool CanEdit { get; set; }
        public bool CanAdd   {get;set;}
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }


        #region " for II6 Only or IIS7 if not open auto compression feature "
        /*
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            MemoryStream ms = new MemoryStream();
            _formatter.Serialize(ms, viewState);
            byte[] viewStateArray = ms.ToArray();
            ScriptManager.RegisterHiddenField(this.Page, "__COMPRESSEDVIEWSTATE", Convert.ToBase64String(CompressViewState.Compress(viewStateArray)));
        }
        protected override object LoadPageStateFromPersistenceMedium()
        {
            string vsString = Request.Form["__COMPRESSEDVIEWSTATE"];
            byte[] bytes = Convert.FromBase64String(vsString);
            bytes = CompressViewState.Decompress(bytes);
            return _formatter.Deserialize(Convert.ToBase64String(bytes));
        }
          */
        #endregion 


     
    }
}
