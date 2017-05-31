using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Threading;

using AjaxControlToolkit;

using SS.Standard.Security;

using SS.DB.DTO;
using SS.DB.Query;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.Query;
using SS.SU.BLL;

using Spring.Globalization;
using Spring.Context;
using Spring.Expressions;

namespace SS.Standard.UI
{
	[Serializable]
	public class BaseUserControl : Spring.Web.UI.UserControl
	{
        public string ApplicationMode { get { return GetApplicationMode(); } }
		public IUserAccount UserAccount { get; set; }
		public IUserEngine UserEngine { get; set; }
		public ISuGlobalTranslateService SuGlobalTranslateService { get; set; }
		public ISuGlobalTranslateLangService SuGlobalTranslateLangService { get; set; }
		public bool EnableInsertControlCache { get; set; }
		public string ProgramCode { get; set; }
		public IList<string> TranslateControlList { get; set; }

		public event ObjectLookUpReturnEventHandler OnObjectLookUpReturn;
		public delegate void ObjectLookUpReturnEventHandler(object sender, ObjectLookUpReturnArgs e);

		public event ObjectNotifyEventHandler OnObjectNotifyEvent;
		public delegate void ObjectNotifyEventHandler(object sender, ObjectNotifyArgs e);

		public event ObjectLookUpCallingEventHandler OnObjectLookUpCalling;
		public delegate void ObjectLookUpCallingEventHandler(object sender, ObjectLookUpCallingEventArgs e);

		public event ObjectIsHideReturnEventHandler OnObjectIsHideReturn;
		public delegate void ObjectIsHideReturnEventHandler(object sender, ObjectIsHideReturnArgs e);

		public event PopUpReturnEventHandler OnPopUpReturn;
		public delegate void PopUpReturnEventHandler(object sender, PopUpReturnArgs e);

		public BaseUserControl()
		{

		}

		#region Override Method
		protected override void OnPreRender(EventArgs e)
		{

                if (ParameterServices.EnableInsertControlCache)
				{
					this.Recursive(this.Controls);
                }


			
			base.OnPreRender(e);
		}
		#endregion

        private string GetApplicationMode()
        {
            string applicationMode = string.Empty;
            System.Collections.Specialized.NameValueCollection obj = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("appSettings");
            applicationMode = obj.GetValues("ApplicationMode")[0].ToString();

            return applicationMode;
        }

		#region Private Method
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

		#region Override Spring Page methods
		/// <summary>
		/// Returns message for the specified resource name.
		/// </summary>
		/// <param name="name">Resource name.</param>
		/// <returns>Message text.</returns>
		public string GetProgramMessage(string name)
		{
			if (this.EnableInsertControlCache)
				this.CheckTranslateSymbolAndSaveToDatabase(name);

			name = string.Format("{0}|;;|{1}", this.ProgramCode, name);
			return MessageSource.GetMessage(name, UserCulture);
		}
		#endregion

		protected void OnObjectIsHide(bool IsHide)
		{
			ObjectIsHideReturnArgs HideArgs = new ObjectIsHideReturnArgs();
			HideArgs.ObjectReturn = IsHide;
			if (OnObjectIsHideReturn != null)
				OnObjectIsHideReturn(this, HideArgs);
		}
		protected void CallOnObjectLookUpCalling()
		{
			ObjectLookUpCallingEventArgs callingArgs = new ObjectLookUpCallingEventArgs();
			if (OnObjectLookUpCalling != null)
				OnObjectLookUpCalling(this, callingArgs);
		}
		protected void CallOnObjectLookUpReturn(object returnedObject)
		{
			ObjectLookUpReturnArgs args = new ObjectLookUpReturnArgs();
			args.ObjectReturn = returnedObject;
			if (OnObjectLookUpReturn != null)
				OnObjectLookUpReturn(this, args);
		}
		protected void CallOnObjectNotifyEvent(object eventReturn)
		{
			ObjectNotifyArgs args = new ObjectNotifyArgs();
			args.EventReturn = eventReturn;
			if (OnObjectNotifyEvent != null)
				OnObjectNotifyEvent(this, args);
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
		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);

			if (ViewState["Loading Page"] != null)
			{
				BaseUserControl ctl = (BaseUserControl)Page.LoadControl(ViewState["Loading Page"].ToString());
				ctl.ID = "Popup";
				ctl.OnPopUpReturn += BasePopUpReturnHandler;
				UpdatePanel parent = (UpdatePanel)this.FindControl(ViewState["ParentID"].ToString());
				parent.ContentTemplateContainer.Controls.Clear();
				parent.ContentTemplateContainer.Controls.Add(ctl);
			}
		}
		protected BaseUserControl GetCurrentPopUpControl()
		{
			BaseUserControl ctl = null;
			return (BaseUserControl)this.FindControl("Popup");
		}
		public virtual void BasePopUpReturnHandler(object sender, PopUpReturnArgs e)
		{
			UpdatePanel parent = (UpdatePanel)this.FindControl(ViewState["ParentID"].ToString());
			parent.ContentTemplateContainer.Controls.Clear();
			ViewState["Loading Page"] = null;
			parent.Update();
		}
		protected void NotifyPopUpReturn(PopUpReturnArgs e)
		{
			if (OnPopUpReturn != null)
				OnPopUpReturn(this, e);
		}
	}
	public class PopUpReturnArgs : EventArgs
	{
		public PopUpReturnType Type { get; set; }
		public object Data { get; set; }
		public PopUpReturnArgs(PopUpReturnType type, object data)
		{
			Type = type;
			Data = data;
		}
	}
	public enum PopUpReturnType
	{
		OK, Cancel, Add
	}
	public class ObjectIsHideReturnArgs : EventArgs
	{
		public object ObjectReturn { get; set; }
	}
	public class ObjectNotifyArgs : EventArgs
	{
		public object EventReturn { get; set; }
	}
	public class ObjectLookUpCallingEventArgs : EventArgs
	{
		public object ObjectReturn { get; set; }
	}
	public class ObjectLookUpReturnArgs : EventArgs
	{
		public object ObjectReturn { get; set; }
	}

    public enum RequireDocumentAttachmentFlag
    { 
        NotRequired, Required
    }
}
