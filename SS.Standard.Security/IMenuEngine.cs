using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SS.Standard.Security
{
    public interface IMenuEngine
    {
        void UpdateDataMenu();
        void CreateMenu(Menu menu);
        void CreateMenu(TreeView menu);
        void SetApplicationMode(string applicationMode);
    }
}
