using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Reflection;

using SS.Standard.UI;
using SS.Standard.WorkFlow.DTO;

using SCG.eAccounting.Web.UserControls;
using SCG.eAccounting.Web.UserControls.DocumentEditor;
using SCG.eAccounting.Web.Helper;
using System.ComponentModel;

namespace SCG.eAccounting.Web.CustomControls
{

    [
    Browsable(true),
    Designer("System.Web.UI.Design.WebControls.LabelDesigner,System.Drawing.Design,System.Windows.Forms.Design, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
    ControlValuePropertyAttribute("Text")
    ]
	public class LabelExtender : System.Web.UI.WebControls.Label
	{
        
		#region Property
		public bool LinkControlVisible
		{
			get { return (bool)ViewState[LabelExtenderViewStateName.LinkControlVisible]; }
			set { ViewState[LabelExtenderViewStateName.LinkControlVisible] = value; }
		}
		public bool LinkControlEditable
		{
			get { return (bool)ViewState[LabelExtenderViewStateName.LinkControlEdiable]; }
			set { ViewState[LabelExtenderViewStateName.LinkControlEdiable] = value; }
		}
		public object LinkControlGroupID { get; set; }
		public string InitialFlag 
		{ 
			get { return ViewState[LabelExtenderViewStateName.InitialFlag].ToString(); } 
			set { ViewState[LabelExtenderViewStateName.InitialFlag] = value; }
		}
		public string LinkControlID { get; set; }
		public string EmptyDataNominee { get; set; }
		#endregion

		#region Override Method (OnPreRender)
		protected override void OnPreRender(EventArgs e)
		{
            base.OnPreRender(e);
            this.DataBind();

            #region Find IDocumentEditor to check IsContainVisibleFields , IsContainEditableFields
            if (ViewState[LabelExtenderViewStateName.LinkControlVisible] == null || ViewState[LabelExtenderViewStateName.LinkControlEdiable] == null)
            {

                Control currentControl = this;
                while (currentControl != null)
                {
                    if (currentControl is IDocumentEditor)
                    {
                        this.LinkControlVisible = ((IDocumentEditor)currentControl).IsContainVisibleFields(LinkControlGroupID);
                        this.LinkControlEditable = ((IDocumentEditor)currentControl).IsContainEditableFields(LinkControlGroupID);

                        break;
                    }
                    else
                    {
                        currentControl = currentControl.Parent;
                    }
                }
            }
            #endregion
            
            Control control = this.Parent.FindControl(LinkControlID);
			
			// Check Whether the LinkControlVisible is true and Mode not View mode.
			if (this.LinkControlVisible)
			{
				string linkControlValue = string.Empty;

				PropertyInfo property = control.GetType().GetProperty("Text");
				if (property != null)
				{

                    linkControlValue = HttpUtility.HtmlEncode(property.GetValue(control, null).ToString());
					if (!string.IsNullOrEmpty(linkControlValue) && !string.IsNullOrEmpty(this.EmptyDataNominee))
					{
						linkControlValue = this.EmptyDataNominee;
					}
				}

				// Check whether LinkControl is DropdownList.
				if (control.GetType().Equals(typeof(DropDownList))) 
				{
                    if (((DropDownList)control).SelectedItem != null)
                    {
                        linkControlValue = ((DropDownList)control).SelectedItem.Text;
                        if (!string.IsNullOrEmpty(linkControlValue) && !string.IsNullOrEmpty(this.EmptyDataNominee))
                        {
                            linkControlValue = this.EmptyDataNominee;
                        }
                    }
				}

				// Check whether LinkControl is RadionButton.
				if (control.GetType().Equals(typeof(RadioButton)))
				{
					linkControlValue = string.Empty;
					
					RadioButton rdo = ((RadioButton)control);
					if (rdo.Checked)
					{
						linkControlValue = "<input type=\"radio\" disabled=\"disabled\" checked=\"checked\" />";
					}
					else
					{
						linkControlValue = "<input type=\"radio\" disabled=\"disabled\" />";
					}
					
					linkControlValue += rdo.Text;
				}

				// Check whether LinkControl is RadionButton.
				if (control.GetType().Equals(typeof(CheckBox)))
				{
					linkControlValue = string.Empty;
				
					CheckBox chk = ((CheckBox)control);
					if (chk.Checked)
					{
						linkControlValue = "<input type=\"checkbox\" disabled=\"disabled\" checked=\"checked\" />";
					}
					else
					{
						linkControlValue = "<input type=\"checkbox\" disabled=\"disabled\" />";
					}
					
					linkControlValue += chk.Text;
				}
				
				// Set text of this LabelExtender.
				this.Text = linkControlValue;

				if (string.IsNullOrEmpty(this.InitialFlag))
				{
					this.HideAll(control);
				}
				else
				{
					if (this.InitialFlag.Equals(FlagEnum.ViewFlag)) // Check whether InitialFlag is View Flag.
					{
						this.DisplayReadonlyControl(control);
					}
					else // InitialFlag is Edit/New Flag.
					{
						if (this.LinkControlEditable) // Check this field is Can Edit.
						{
							this.DisplayEditableControl(control);
						}
						else // Check this field is read only.
						{
							this.DisplayReadonlyControl(control);
						}
					}
				}
			}
			else
			{
				this.HideAll(control);
			}
		}
		#endregion
		
		#region Private Method
		private void DisplayEditableControl(Control control)
		{
			if (control is IEditorUserControl)
			{
				((IEditorUserControl)control).Display = true;
			}
			else if (control is RadioButton)
			{
				((RadioButton)control).Style["display"] = "inline-block";
			}
			else if (control is CheckBox)
			{
				((CheckBox)control).Style["display"] = "inline-block";
			}
			else
			{
				control.Visible = true;
			}
			
			this.Style["display"] = "none";
		}
		private void DisplayReadonlyControl(Control control)
		{
			if (control is IEditorUserControl)
			{
				((IEditorUserControl)control).Display = false;
			}
			else if (control is RadioButton)
			{
				((RadioButton)control).Style["display"] = "none";
			}
			else if (control is CheckBox)
			{
				((CheckBox)control).Style["display"] = "none";
			}
			else
			{
				control.Visible = false;
			}
			this.Style["display"] = "inline-block";
		}
		private void HideAll(Control control)
		{
			if (control is IEditorUserControl)
			{
				((IEditorUserControl)control).Display = false;
			}
			else if (control is RadioButton)
			{
				((RadioButton)control).Style["display"] = "none";
			}
			else if (control is CheckBox)
			{
				((CheckBox)control).Style["display"] = "none";
			}
			else
			{
				control.Visible = false;
			}
			this.Style["display"] = "none";
		}
		#endregion
	}
}
