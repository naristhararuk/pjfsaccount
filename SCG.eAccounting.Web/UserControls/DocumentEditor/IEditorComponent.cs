using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
	public interface IEditorComponent
	{
        void Initialize(Guid txID, long documentID, string initFlag);
        //void BindControl();
        //void ResetControlValue();
	}
}
