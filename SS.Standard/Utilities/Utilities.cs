using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using SS.Standard.Security;
using System.Security.Cryptography;
using System.Text;

namespace SS.Standard.Utilities
{
    public class Utilities
    {
        public Utilities()
        {

        }

        public static void EmptyValues(object[] ObjectArray)
        {

            #region Clear TextBox
            var resTextBox = from vo in ObjectArray
                             where vo is TextBox
                             select vo;

            foreach (var item in resTextBox)
                (item as TextBox).Text = string.Empty;
            #endregion Clear TextBox

            #region Clear Label
            var resLabel = from vo in ObjectArray
                           where vo is Label
                           select vo;

            foreach (var item in resLabel)
                (item as Label).Text = string.Empty;
            #endregion Clear TextBox

            #region Reset CheckBox
            var resCheckBox = from vo in ObjectArray
                              where vo is CheckBox
                              select vo;

            foreach (var item in resCheckBox)
            {
                CheckBox chk = (item as CheckBox);
                if (chk.Checked)
                    chk.Checked = false;
                else
                    chk.Checked = true;
            }
            #endregion Reset CheckBox

            #region Reset DropDownList
            var resDropDownList = from vo in ObjectArray
                                  where vo is DropDownList
                                  select vo;

            foreach (var item in resDropDownList)
                (item as DropDownList).ClearSelection();
            #endregion

            #region Reset RadioButton
            var resRadioButton = from vo in ObjectArray
                                 where vo is RadioButton
                                 select vo;

            foreach (var item in resRadioButton)
            {
                RadioButton rbtn = (item as RadioButton);
                if (rbtn.Checked)
                    rbtn.Checked = false;
                else
                    rbtn.Checked = true;
            }
            #endregion

            #region Reset RadioButtonList
            var resRadioButtonList = from vo in ObjectArray
                                     where vo is RadioButtonList
                                     select vo;

            foreach (var item in resRadioButton)
                (item as RadioButtonList).ClearSelection();

            #endregion

            #region Reset ListBox
            var resListBox = from vo in ObjectArray
                             where vo is ListBox
                             select vo;

            foreach (var item in resListBox)
                (item as ListBox).ClearSelection();

            #endregion
        }

        public static void setApplicationTitle(System.Web.UI.Page pagetitle,System.Web.HttpContext context)
        {
            pagetitle.Title = Convert.ToString(context.Application["ApplicationTitile"]);
        }

        public static DataSet SearchActiveStatus()
        {
            DataSet dst = new DataSet();
            DataTable dtb = new DataTable();
            dtb.Columns.Add("DisplayItems");
            dtb.Columns.Add("ValuesItems");
            dst.Tables.Add(dtb);

            DataRow dr1 = dst.Tables[0].NewRow();
            dr1["DisplayItems"] = "$All$";
            dr1["ValuesItems"] = "All";
            dst.Tables[0].Rows.Add(dr1);

            DataRow dr2 = dst.Tables[0].NewRow();
            dr2["DisplayItems"] = "$Active$";
            dr2["ValuesItems"] = "True";
            dst.Tables[0].Rows.Add(dr2);

            DataRow dr3 = dst.Tables[0].NewRow();
            dr3["DisplayItems"] = "$InActive$";
            dr3["ValuesItems"] = "False";
            dst.Tables[0].Rows.Add(dr3);

            dst.AcceptChanges();

            return dst;
        }

        public static DataSet ActiveStatus()
        {
            DataSet dst = new DataSet();
            DataTable dtb = new DataTable();
            dtb.Columns.Add("DisplayItems");
            dtb.Columns.Add("ValuesItems");
            dst.Tables.Add(dtb);

            DataRow dr2 = dst.Tables[0].NewRow();
            dr2["DisplayItems"] = "$Active$";
            dr2["ValuesItems"] = "True";
            dst.Tables[0].Rows.Add(dr2);

            DataRow dr3 = dst.Tables[0].NewRow();
            dr3["DisplayItems"] = "$InActive$";
            dr3["ValuesItems"] = "False";
            dst.Tables[0].Rows.Add(dr3);

            dst.AcceptChanges();

            return dst;
        }


    }
}
