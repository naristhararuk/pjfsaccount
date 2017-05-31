using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using SS.Standard.Security;


namespace SS.Standard.UI
{
    public class BaseUCTranslations : BaseUserControl
    {
        public IUserEngine UserEngine { get; set; }
        public IMenuEngine MenuEngine { get; set; }
        public IUserAccount UserAccount { get; set; }
        public IACLEngine ACLEngine { get; set; }
        public void SetCulture()
        {
            if (UserAccount.CurrentLanguageCode != null)
                this.UserCulture = new CultureInfo(UserAccount.CurrentLanguageCode);
        }
    }
}
