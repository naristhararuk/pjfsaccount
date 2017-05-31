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
//using SS.Standard.Security;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Globalization;

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

        public static int ParseInt(string integer)
        {
            int returnInt;
            int.TryParse(integer, out returnInt);
            return returnInt;
        }

        public static short ParseShort(string int16)
        {
            short returnShort;
            short.TryParse(int16, out returnShort);
            return returnShort;
        }
        public static bool ParseBool(string strBoolean)
        {
            bool returnBoolean;
            bool.TryParse(strBoolean, out returnBoolean);
            return returnBoolean;
        }
        public static long ParseLong(string strLong)
        {
            long returnLong;
            long.TryParse(strLong, out returnLong);
            return returnLong;
        }
        public static double ParseDouble(string strDouble)
        {
            double returnDouble;
            double.TryParse(strDouble, out returnDouble);
            return returnDouble;
        }
        public static void WriteFiles(string TextFileBody,string fileName,string filePath,string fileFormat)
        {
            try {
                string path = "~/" + filePath +"/"+ fileName + "." + fileFormat;
            if ((!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))) {
            File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
            }
            using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path))) {
            //w.WriteLine(errorMessage);
            //w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
           // string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() + ". Error Message:" + errorMessage;
                w.WriteLine(TextFileBody);
           // w.WriteLine("__________________________");
            w.Flush();
            w.Close();
            }
            }
            catch (Exception ex) {
                string error = ex.Message;
             //   throw ex;
           // WriteError(ex.Message);
            }

        }

        public static void WriteLogs(string TextFileBody, string fileName, string filePath, string fileFormat)
        {
            try
            {
                string path = "~/" + filePath + "/" + fileName+"_"+DateTime.Now.Year.ToString()+DateTime.Now.Month.ToString().PadLeft(2,'0')+DateTime.Now.Day.ToString().PadLeft(2,'0')+ "." + fileFormat;
                if ((!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path))))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() + ". Error Message:" + TextFileBody;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                //   throw ex;
                // WriteError(ex.Message);
            }

        }
        public static string NumericToThai(double n)
        {

            n = Math.Abs(n);
            string s = n.ToString();
            String[] token = s.Split(new char[] { '.' });

            string output = "";

            string stmp = token[0].Substring(0, token[0].Length % 6);
            output += NumberToThai(stmp);
            token[0] = token[0].Substring(stmp.Length);

            while (token[0].Length > 0)
            {
                output += "ล้าน";
                stmp = token[0].Substring(0, 6);
                output += NumberToThai(stmp);
                token[0] = token[0].Substring(6);
            }

            output += "บาท";

            if (token.Length == 2)
            {
                output += NumberToThai(token[1]);
                output += "สตางค์";
            }
            return output;
        }

        public static string NumberToThai(String s)
        {
            string[] multiplier = { "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
            string[] number = { "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
            string output = "";
            for (int i = 0; i < s.Length; ++i)
            {

                int n = Int32.Parse(s.Substring(i, 1));
                if (n == 0) continue;
                output = output + number[n - 1];

                if (s.Length - i > 1)
                    output = output + multiplier[s.Length - i - 2];
            }

            output = output.Replace("สิบหนึ่ง", "สิบเอ็ด");
            output = output.Replace("สองสิบ", "ยี่สิบ");
            output = output.Replace("หนึ่งสิบ", "สิบ");

            return output;
        }
        
        /// <summary>
        /// Utility Function for delete all file in directory path.
        /// </summary>
		/// <param name="directoryPath">Directory Path of files to delete.</param>
        public static void FilesDelete(string directoryPath)
        {
			string[] files = Directory.GetFiles(directoryPath);
			foreach (string file in files)
			{
				try
				{
					File.Delete(file);
				}
				catch { }
			}
        }
    }
}
