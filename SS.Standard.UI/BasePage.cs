using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Principal;

using AjaxControlToolkit;

using SS.Standard.Security;

using SS.DB.DTO;
using SS.DB.Query;

using SS.SU.DTO;
using SS.SU.BLL;
using Spring.Util;
using Spring.Context;
using Spring.Globalization;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using Spring.Expressions;

namespace SS.Standard.UI
{
    public class BasePage : Spring.Web.UI.Page
    {
        public MasterPage masterpage { get; set; }
        public IList<string> TranslateControlList { get; set; }

        public IUserEngine UserEngine { get; set; }
        public IMenuEngine MenuEngine { get; set; }
        public IUserAccount UserAccount { get; set; }
        public IACLEngine ACLEngine { get; set; }
        public ISuGlobalTranslateService SuGlobalTranslateService { get; set; }
        public ISuGlobalTranslateLangService SuGlobalTranslateLangService { get; set; }
        public event PopUpReturnEventHandler OnPopUpReturn;
        public delegate void PopUpReturnEventHandler(object sender, PopUpReturnArgs e);

        public bool CanEdit { get; set; }
        public bool CanAdd { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }

        public string ProgramCode { get; set; }
        public string ApplyTheme { get; set; }
        public string CompanyName { get; set; }
        public string PageTitle { get; set; }

        public bool EnableInsertControlCache { get; set; }
        public bool NotAuthenticationPage { get; set; }
        public string ApplicationMode { get { return GetApplicationMode(); } }

        //private ObjectStateFormatter _formatter = new ObjectStateFormatter();
        #region Override Method
        protected override void OnPreInit(EventArgs e)
        {
            this.Theme = ApplyTheme;
            this.CompanyName = CompanyName;
            this.Page.Title = PageTitle;
            base.OnPreInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                UserAccount.CurrentProgramCode = this.ProgramCode;
                if (!String.IsNullOrEmpty(ProgramCode) && !NotAuthenticationPage)
                {
                    ACLEngine.Permission(ProgramCode, true);
                }
            }
            catch { }
            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {

            if (ParameterServices.EnableInsertControlCache)
            {
                this.Recursive(this.Controls);
            }


            base.OnPreRender(e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Is this required for FireFox? Would be good to do this without magic strings.
            // Won't it overwrite the previous setting
            //Response.Headers.Add("Cache-Control", "no-cache, no-store");

            // Why is it necessary to explicitly call SetExpires. Presume it is still better than calling
            // Response.Headers.Add( directly
            //Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));

        }
        #endregion

        #region Public Method
        public void ChangeLanguage(short languageID)
        {
            UserAccount.SetLanguage(languageID);
        }
        public void SignOut(long userID)
        {
            UserEngine.SignOut(userID);
        }
        #endregion

        #region Private Method
        private void RedirectSignInPage()
        {
            string url = HttpContext.Current.Request.Url.ToString();
            if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttp))
            {
                url = url.Replace(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
            }
            System.Web.HttpContext.Current.Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Login.aspx")));
        }
        public void RedirectMenuPage()
        {
            string url = HttpContext.Current.Request.Url.ToString();
            if (ParameterServices.EnableSSLOnLoginPage && HttpContext.Current.Request.Url.Scheme.Equals(Uri.UriSchemeHttps))
            {
                url = url.Replace(Uri.UriSchemeHttps, Uri.UriSchemeHttp);
            }
            System.Web.HttpContext.Current.Response.Redirect(url.Replace(HttpContext.Current.Request.RawUrl, ResolveUrl("~/Menu.aspx")));
        }
        private bool IsTranslateControl(Control control, out string translateWord, out string translateControl)
        {
            bool returnValue = false;
            translateWord = string.Empty;
            translateControl = string.Empty;

            PropertyInfo info = control.GetType().GetProperty("Text");
            if (info == null)
                info = control.GetType().GetProperty("HeaderText");

            if (info == null)
                info = control.GetType().GetProperty("ToolTip");

            if ((info != null) && (info.GetValue(control, null).ToString().StartsWith("$")) && (info.GetValue(control, null).ToString().EndsWith("$")))
            {
                translateControl = control.ID + "." + info.Name;
                translateWord = info.GetValue(control, null).ToString().TrimStart('$').TrimEnd('$');// +info.Name;
                returnValue = true;
            }

            return returnValue;
        }
        private void CheckTranslateGridViewHeader(BaseGridView control)
        {
            for (int index = 0; index < control.Columns.Count; index++)
            {
                if (control.Columns[index].HeaderText.StartsWith("$") && control.Columns[index].HeaderText.EndsWith("$"))
                {
                    if (!string.IsNullOrEmpty(this.ProgramCode))
                    {
                        try
                        {
                            string translateControl = string.Concat(control.ID, ".Columns[", index.ToString(), "]", ".HeaderText");
                            string translateWord = control.Columns[index].HeaderText.TrimStart('$').TrimEnd('$');

                            SuGlobalTranslate translate = new SuGlobalTranslate();
                            translate.ProgramCode = this.ProgramCode;
                            translate.TranslateSymbol = string.Empty;
                            translate.TranslateControl = translateControl;
                            translate.Active = true;
                            translate.CreBy = UserAccount.UserID;
                            translate.UpdBy = UserAccount.UserID;
                            translate.UpdPgm = UserAccount.CurrentProgramCode;

                            long translateID = SuGlobalTranslateService.AddProgramTranslateControl_Symbol(translate);

                            if (translateID > 0)
                            {
                                IList<SuGlobalTranslateLang> translateLangList = new List<SuGlobalTranslateLang>();
                                SuGlobalTranslateLang translateTH = this.BuildSuGlobalTranslateLang(translateID, ParameterServices.DefaultLanguage, translateWord);
                                translateLangList.Add(translateTH);

                                SuGlobalTranslateLang translateEN = this.BuildSuGlobalTranslateLang(translateID, ParameterServices.EnglishLanguageID, translateWord);
                                translateLangList.Add(translateEN);

                                SuGlobalTranslateLangService.UpdateGlobalTranslateLang(translateLangList);
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
        private void CheckTranslateControlAndSaveToDatabase(Control control)
        {
            string translateControl = string.Empty;
            string translateWord = string.Empty;

            if (this.IsTranslateControl(control, out translateWord, out translateControl) && (!string.IsNullOrEmpty(this.ProgramCode)))
            {
                try
                {
                    SuGlobalTranslate translate = new SuGlobalTranslate();
                    translate.ProgramCode = this.ProgramCode;
                    translate.TranslateControl = translateControl;
                    translate.TranslateSymbol = string.Empty;
                    translate.Active = true;
                    translate.CreBy = UserAccount.UserID;
                    translate.UpdBy = UserAccount.UserID;
                    translate.UpdPgm = UserAccount.CurrentProgramCode;

                    long translateID = SuGlobalTranslateService.AddProgramTranslateControl_Symbol(translate);

                    if (translateID > 0)
                    {
                        IList<SuGlobalTranslateLang> translateLangList = new List<SuGlobalTranslateLang>();
                        SuGlobalTranslateLang translateTH = this.BuildSuGlobalTranslateLang(translateID, ParameterServices.DefaultLanguage, translateWord);
                        translateLangList.Add(translateTH);

                        SuGlobalTranslateLang translateEN = this.BuildSuGlobalTranslateLang(translateID, ParameterServices.EnglishLanguageID, translateWord);
                        translateLangList.Add(translateEN);

                        SuGlobalTranslateLangService.UpdateGlobalTranslateLang(translateLangList);
                    }
                }
                catch (Exception) { }
            }
        }
        private void CheckTranslateSymbolAndSaveToDatabase(string translateWord)
        {
            if (!string.IsNullOrEmpty(this.ProgramCode))
            {
                try
                {
                    SuGlobalTranslate translate = new SuGlobalTranslate();
                    translate.ProgramCode = this.ProgramCode;
                    translate.TranslateControl = string.Empty;
                    translate.TranslateSymbol = translateWord;
                    translate.Active = true;
                    translate.CreBy = UserAccount.UserID;
                    translate.UpdBy = UserAccount.UserID;
                    translate.UpdPgm = UserAccount.CurrentProgramCode;

                    long translateID = SuGlobalTranslateService.AddProgramTranslateControl_Symbol(translate);

                    if (translateID > 0)
                    {
                        IList<SuGlobalTranslateLang> translateLangList = new List<SuGlobalTranslateLang>();
                        SuGlobalTranslateLang translateTH = this.BuildSuGlobalTranslateLang(translateID, ParameterServices.DefaultLanguage, translateWord);
                        translateLangList.Add(translateTH);

                        SuGlobalTranslateLang translateEN = this.BuildSuGlobalTranslateLang(translateID, ParameterServices.EnglishLanguageID, translateWord);
                        translateLangList.Add(translateEN);

                        SuGlobalTranslateLangService.UpdateGlobalTranslateLang(translateLangList);
                    }
                }
                catch (Exception) { }
            }
        }
        private SuGlobalTranslateLang BuildSuGlobalTranslateLang(long translateID, short languageID, string translateWord)
        {
            SuGlobalTranslateLang translateLang = new SuGlobalTranslateLang();
            translateLang.Language = new DbLanguage(languageID);
            translateLang.Translate = new SuGlobalTranslate(translateID);
            translateLang.TranslateWord = translateWord;
            translateLang.Active = true;
            translateLang.CreBy = UserAccount.UserID;
            translateLang.UpdBy = UserAccount.UserID;
            translateLang.UpdPgm = UserAccount.CurrentProgramCode;

            return translateLang;
        }
        private void Recursive(ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is UpdateProgress) continue;
                if (control is BaseUserControl) continue;
                if (control is FormView) continue;
                if (control is BaseGridView)
                {
                    this.CheckTranslateGridViewHeader(control as BaseGridView);
                    continue;
                }

                if (control.HasControls())
                    this.Recursive(control.Controls);
                else
                    this.CheckTranslateControlAndSaveToDatabase(control);
            }
        }

        #endregion

        public void Permission()
        {
            //if (!NotAuthenticationPage)
            //{
            //    ACLEngine.Permission(ProgramCode, true);
            //}
        }
        public void RegisterScriptForGridView(BaseGridView ctlUserGrid)
        {

        }

        [System.Web.Services.WebMethod]
        public static string GetDialog(string id)
        {
            //SS.Standard.AlertMsg.Dialog dlg = new SS.Standard.AlertMsg.Dialog();
            //return dlg.MsgID(id) + "!@#$%";
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethod]
        public static void GetMessageDialog(string id)
        {
            //Provider.MessageDAL.GetShowMessage(id);
            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethod]
        public static string GetDialogConfirm(string id, object a)
        {
            //string bb = a.ToString();

            //SS.Standard.AlertMsg.Dialog dlg = new SS.Standard.AlertMsg.Dialog();
            //return dlg.MsgID(id) + "!@#$%";
            //dlg.Msg(id);

            throw new NotImplementedException();
        }

        [System.Web.Services.WebMethod]
        public static string GetDialogError(string id)
        {
            // string bb = a.ToString();

            //SS.Standard.AlertMsg.Dialog dlg = new SS.Standard.AlertMsg.Dialog(id);
            //return dlg.MsgID(id) + "!@#$%";
            ////dlg.Msg(id);

            throw new NotImplementedException();
        }

        #region " for II6 Only or IIS7 if not open auto compression feature "
        PageStatePersister pageStatePersister = null;
        protected override PageStatePersister PageStatePersister
        {

            get
            {
                if (pageStatePersister == null)
                {
                    if (ParameterServices.EnableSessionViewState)
                        pageStatePersister = new SessionPageStatePersister(this);
                    else
                        pageStatePersister = base.PageStatePersister;
                }
                return pageStatePersister;

            }
          

        }





        #endregion

        #region Override Spring Page methods
        /// <summary>
        /// Returns message for the specified resource name.
        /// </summary>
        /// <param name="name">Resource name.</param>
        /// <returns>Message text.</returns>
        public string GetProgramMessage(string name)
        {
            if (ParameterServices.EnableInsertControlCache)
                this.CheckTranslateSymbolAndSaveToDatabase(name);

            name = string.Format("{0}|;;|{1}", this.ProgramCode, name);
            return MessageSource.GetMessage(name, UserCulture);
        }
        #endregion

        protected BaseUserControl GetCurrentPopUpControl()
        {
            BaseUserControl ctl = null;
            if (PopUpUpdatePanel != null)
            {
                return (BaseUserControl)PopUpUpdatePanel.FindControl("Popup");
            }
            else
            {
                return null;
            }
        }
        protected T LoadPopup<T>(string path, UpdatePanel parent) where T : BaseUserControl
        {
            ViewState["Loading Page"] = path;
            BaseUserControl ctl = (BaseUserControl)Page.LoadControl(path);
            ctl.ID = "Popup";
            ctl.OnPopUpReturn += BasePopUpReturnHandler;
            ViewState["ParentID"] = parent.ID;
            parent.ContentTemplateContainer.Controls.Clear();
            parent.ContentTemplateContainer.Controls.Add(ctl);
            parent.Update();
            return (T)ctl;
        }
        public virtual void BasePopUpReturnHandler(object sender, PopUpReturnArgs e)
        {
            PopUpUpdatePanel.ContentTemplateContainer.Controls.Clear();
            ViewState["Loading Page"] = null;
            PopUpUpdatePanel.Update();
        }

        protected UpdatePanel PopUpUpdatePanel;

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (ViewState["Loading Page"] != null)
            {
                BaseUserControl ctl = (BaseUserControl)Page.LoadControl(ViewState["Loading Page"].ToString());
                ctl.ID = "Popup";
                ctl.OnPopUpReturn += BasePopUpReturnHandler;
                PopUpUpdatePanel.ContentTemplateContainer.Controls.Clear();
                PopUpUpdatePanel.ContentTemplateContainer.Controls.Add(ctl);
            }
        }
        protected void NotifyPopUpReturn(PopUpReturnArgs e)
        {
            if (OnPopUpReturn != null)
                OnPopUpReturn(this, e);
        }

        private string GetApplicationMode()
        {
            string applicationMode = string.Empty;
            System.Collections.Specialized.NameValueCollection obj = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("appSettings");
            applicationMode = obj.GetValues("ApplicationMode")[0].ToString();

            return applicationMode;
        }
    }
}
