using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Standard.UI
{
	/// <summary>
	/// ReadOnlyTextBox to Enable user to set IsReadOnly of this control.
	/// It will can keep state of text when change value from the script.
	/// </summary>
	public class ReadOnlyTextBox : System.Web.UI.WebControls.TextBox
	{
		public bool IsReadOnly { get; set; }

		protected override void OnPreRender(EventArgs e)
		{
			if (this.IsReadOnly)
			{
				this.Attributes.Add("readonly", "true");
			}
			else
			{
				this.Attributes.Remove("readonly");
			}

			base.OnPreRender(e);
		}
	}
}
