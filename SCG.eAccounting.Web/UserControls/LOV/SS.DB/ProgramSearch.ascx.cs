using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


using SS.Standard.UI;
using SS.Standard.Security;

using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;
using SS.SU.DTO.ValueObject;
using SS.SU.BLL.Implement;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class ProgramSearch : BaseUserControl
    {
        #region Define Variable
        public ISuProgramLangService SuProgramLangService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            string popupURL = "~/UserControls/LOV/SS.DB/ProgramSearchPopup.aspx?RoleId={0}&LanguageId={1}";
            ctlProgramSearchLookupPopupCaller.URL = string.Format(popupURL, new object[] { RoleId, LanguageId });

        }
        #endregion   
     
        public void Show()
        {
            CallOnObjectLookUpCalling();
            string popupURL = "~/UserControls/LOV/SS.DB/ProgramSearchPopup.aspx?RoleId={0}&LanguageId={1}";
            ctlProgramSearchLookupPopupCaller.URL = string.Format(popupURL, new object[] { RoleId, LanguageId });
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), ctlProgramSearchLookupPopupCaller.ClientID + "_popup()", ctlProgramSearchLookupPopupCaller.ClientID + "_popup('" + ctlProgramSearchLookupPopupCaller.ProcessedURL + "')", true);
        }
        public string RoleId { get; set; }
        public string LanguageId { get; set; }

        protected void ctlProgramSearchLookupPopupCaller_NotifyPopupResult(object sender, string action, string value)
        {
            if (action != "ok") return;

            object returnValue = new object();

            string[] listID = value.Split('|');
            IList<ProgramLang> programList = new List<ProgramLang>();
            foreach (string id in listID)
            {
                SuProgramLang programlang = SuProgramLangService.FindByIdentity(UIHelper.ParseLong(id));
                if (programlang != null)
                {
                    ProgramLang pl = new ProgramLang();
                    pl.ProgramId = programlang.ProgramId;
                    pl.ProgramLangId = programlang.Id;
                    pl.ProgramName = programlang.ProgramsName;
                    pl.Comment = programlang.Comment;

                    programList.Add(pl);
                }
            }
            returnValue = programList;
            CallOnObjectLookUpReturn(returnValue);
        }
    }
}