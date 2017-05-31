using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.Web.UserControls
{
	public interface IEditorUserControl
	{
		bool Display { set; }
		string Text { get; }
	}
}
