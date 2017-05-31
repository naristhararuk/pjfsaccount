using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Configuration;
using SS.Standard.Data;
using SS.Standard.Security;
using AjaxControlToolkit;
using SS.Standard.Data.Mssql;
using SS.Standard.Data.Interfaces;

/// <summary>
/// Summary description for Language
/// </summary>
/// 

namespace SS.Standard.Language
{
    public class Translation 
    {
        static Hashtable TranslateMain;
        static Hashtable TranslatePGM;
        static string ProgramCode;
        public Translation()
        {
        }

        public static void TranslatePage(string PGM, ControlCollection Controls)
        {
            ProgramCode = PGM;
            if (ProgramCode != null && ProgramCode != "")
            {
                if (HttpContext.Current.Session["TranslateMain"] == null)
                {
                    TranslateMain = new Hashtable();
                    TranslatePGM = new Hashtable();
                    QueryLang(TranslateMain, TranslatePGM);
                }
                else
                {
                    TranslateMain = HttpContext.Current.Session["TranslateMain"] as Hashtable;
                    TranslatePGM = HttpContext.Current.Session["TranslatePGM"] as Hashtable;
                    string lang = UserAccount.CURRENT_LanguageID == null ? UserAccount.LanguageID.ToString() : UserAccount.CURRENT_LanguageID.ToString();
                    if (lang.Equals("0"))
                    {
                        lang = Provider.DbParameter.getDbParameter(1, 6); //System.Web.HttpContext.Current.Application["DefaultLanguageId"].ToString();
                    }
                    if (TranslatePGM["ProgramCode"].ToString() != ProgramCode)
                    {
                        QueryLang(TranslateMain, TranslatePGM);
                    }
                    else if (TranslatePGM["LanguageID"].ToString() != lang)
                    {
                        ChangeLang(TranslateMain, TranslatePGM);
                    }
                }
                ObjectIdentify(Controls);
            }
            else
            {
                throw new Exception("กรุณากำหนดค่าของ ProgramCode");
            }

        }

       

        private static void QueryLang(Hashtable TranslateMain, Hashtable TranslatePGM)
        {
            //DBManager dbm = new DBManager();
            Provider.TranslationDAL.OpenConnection();
            TranslateMain.Clear();
            TranslatePGM.Clear();
            TranslatePGM.Add("ProgramCode", ProgramCode);
            string lang = UserAccount.CURRENT_LanguageID == null ? UserAccount.LanguageID.ToString() : UserAccount.CURRENT_LanguageID.ToString();
            if (lang.Equals("0"))
            {

                lang = Provider.DbParameter.getDbParameter(1, 6); // System.Web.HttpContext.Current.Application["DefaultLanguageId"].ToString();
            }
            TranslatePGM.Add("LanguageID", lang);
            IDataReader dr = Provider.TranslationDAL.QueryLang(int.Parse(lang.Trim()),ProgramCode); // dbm.ExecuteReader("SELECT NAME,WORD FROM SU_TRANSLATE_PGM AS T JOIN SU_TRANSLATE_PGM_LANG AS L ON T.TRANS_ProgramID = L.TRANS_ProgramID JOIN SuProgram P ON T.ProgramID = P.ProgramID WHERE L.LanguageID = " + lang + " AND P.ProgramCode = '" + ProgramCode + "'", CommandType.Text);
            while (dr.Read())
            {
                TranslatePGM.Add(dr.GetString(0), dr.GetString(1));
            }
            dr.Close();
            dr = Provider.TranslationDAL.QueryLang(int.Parse(lang.Trim())); //dbm.ExecuteReader("SELECT NAME,WORD FROM SU_TRANSLATE AS T JOIN SU_TRANSLATE_LANG AS L ON T.TRANS_ID = L.TRANS_ID WHERE L.LanguageID = " + lang, CommandType.Text);
            while (dr.Read())
            {
                TranslateMain.Add(dr.GetString(0), dr.GetString(1));
            }
            dr.Close();
            HttpContext.Current.Session["TranslatePGM"] = TranslatePGM;
            HttpContext.Current.Session["TranslateMain"] = TranslateMain;
            Provider.TranslationDAL.CloseConnection();
        }

        private static void ChangeLang(Hashtable TranslateMain, Hashtable TranslatePGM)
        {
            DBManager dbm = new DBManager();
            Provider.TranslationDAL.OpenConnection();
            string oldLang = (string)TranslatePGM["LanguageID"];
            TranslateMain.Clear();
            TranslatePGM.Clear();
            TranslatePGM.Add("ProgramCode", ProgramCode);
            string lang = UserAccount.CURRENT_LanguageID == null ? UserAccount.LanguageID.ToString() : UserAccount.CURRENT_LanguageID.ToString();
            if (lang.Equals("0"))
            {
                lang = System.Web.HttpContext.Current.Application["DefaultLanguageId"].ToString();
            }
            TranslatePGM.Add("LanguageID", lang);
            IDataReader dr = Provider.TranslationDAL.ChangeLang(int.Parse(oldLang.Trim()), int.Parse(lang.Trim()),ProgramCode); // dbm.ExecuteReader("SELECT O.WORD , N.WORD FROM SU_TRANSLATE_PGM_LANG AS O JOIN SU_TRANSLATE_PGM_LANG AS N ON O.TRANS_ProgramID = N.TRANS_ProgramID JOIN SU_TRANSLATE_PGM AS T ON T.TRANS_ProgramID = N.TRANS_ProgramID JOIN SuProgram AS P ON T.ProgramID = P.ProgramID WHERE O.LanguageID = " + oldLang + " AND N.LanguageID= " + lang + " AND P.ProgramCode = '" + ProgramCode + "'", CommandType.Text);
            while (dr.Read())
            {
                TranslatePGM.Add(dr.GetString(0), dr.GetString(1));
            }
            dr.Close();
            dr = Provider.TranslationDAL.ChangeLang(int.Parse(oldLang.Trim()), int.Parse(lang.Trim())); //dbm.ExecuteReader("SELECT O.WORD , N.WORD FROM SU_TRANSLATE_LANG AS O JOIN SU_TRANSLATE_LANG AS N ON O.TRANS_ID = N.TRANS_ID WHERE O.LanguageID = " + oldLang + " AND N.LanguageID= " + lang, CommandType.Text);
            while (dr.Read())
            {
                TranslateMain.Add(dr.GetString(0), dr.GetString(1));
            }
            dr.Close();
            HttpContext.Current.Session["TranslatePGM"] = TranslatePGM;
            HttpContext.Current.Session["TranslateMain"] = TranslateMain;
            Provider.TranslationDAL.CloseConnection();

        }
        private static void ObjectIdentify(ControlCollection obj)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].GetType().ToString() == "System.Web.UI.UpdatePanel") TranslateUpdatePanel(obj[i] as UpdatePanel);
                else if (obj[i].GetType().ToString() == "System.Web.UI.UpdateProgress") TranslateUpdateProgress(obj[i] as UpdateProgress);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.Label") TranslateLabel(obj[i] as Label);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.RequiredFieldValidator") TranslateRequiredFieldValidator(obj[i] as RequiredFieldValidator);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.Button") TranslateButton(obj[i] as Button);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.ImageButton") TranslateImageButton(obj[i] as ImageButton);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.CheckBox") TranslateCheckBox(obj[i] as CheckBox);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.RadioButton") TranslateCheckBox(obj[i] as RadioButton);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.LinkButton") TranslateLinkButton(obj[i] as LinkButton);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.HyperLink") TranslateHyperLink(obj[i] as HyperLink);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.Literal") TranslateLiteral(obj[i] as Literal);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.DropDownList") TranslateDropDownList(obj[i] as DropDownList);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.CheckBoxList") TranslateCheckBoxList(obj[i] as CheckBoxList);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.RadioButtonList") TranslateRadioButtonList(obj[i] as RadioButtonList);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.BulletedList") TranslateBulletedList(obj[i] as System.Web.UI.WebControls.BulletedList);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.GridView") TranslateGridView(obj[i] as GridView);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.DetailsView") TranslateDetailsView(obj[i] as DetailsView);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.Panel") TranslatePanel(obj[i] as Panel);
                else if (obj[i].GetType().ToString() == "System.Web.UI.WebControls.ContentPlaceHolder") TranslateContentPlaceHolder(obj[i] as ContentPlaceHolder);

                else if (obj[i].GetType().ToString() == "AjaxControlToolkit.TabContainer") TranslateTabContainer(obj[i] as TabContainer);
                else if (obj[i].GetType().ToString() == "AjaxControlToolkit.TabPanel") TranslateTabPanel(obj[i] as TabPanel);
                else if (obj[i].GetType().ToString() == "AjaxControlToolkit.Accordion") TranslateAccordion(obj[i] as Accordion);
                else if (obj[i].GetType().ToString() == "AjaxControlToolkit.AccordionPane") TranslateAccordionPane(obj[i] as AccordionPane);
                else if (obj[i].GetType().ToString() == "AjaxControlToolkit.ReorderList") TranslateReorderList(obj[i] as ReorderList);
                else if (obj[i].GetType().ToString() == "AjaxControlToolkit.TextBoxWatermarkExtender") TranslateTextBoxWatermarkExtender(obj[i] as TextBoxWatermarkExtender);

                else if (obj[i].GetType().ToString() == "System.Web.UI.HtmlControls.HtmlInputButton") TranslateHtmlInputButton(obj[i] as HtmlInputButton);
                else if (obj[i].GetType().ToString() == "System.Web.UI.HtmlControls.HtmlInputReset") TranslateHtmlInputReset(obj[i] as HtmlInputReset);
                else if (obj[i].GetType().ToString() == "System.Web.UI.HtmlControls.HtmlInputSubmit") TranslateHtmlInputSubmit(obj[i] as HtmlInputSubmit);

                else if (obj[i].Controls.Count > 0) ObjectIdentify(obj[i].Controls);

            }
        }



        private static string TagReplace(string tag)
        {
            if (tag != null && tag != "")
            {
                if (TranslatePGM.ContainsKey(tag.Trim()))
                {
                    return (string)TranslatePGM[tag.Trim()];
                }
                if (TranslateMain.ContainsKey(tag.Trim()))
                {
                    return (string)TranslateMain[tag.Trim()];
                }
            }
      
            return tag;
        }

        private static void TranslateHtmlInputSubmit(HtmlInputSubmit obj)
        {
            if (obj != null)
                obj.Value = TagReplace(obj.Value);
        }

        private static void TranslateHtmlInputReset(HtmlInputReset obj)
        {
            if (obj != null)
                obj.Value = TagReplace(obj.Value);
        }


        private static void TranslateRequiredFieldValidator(RequiredFieldValidator obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);
                obj.ErrorMessage = TagReplace(obj.ErrorMessage);
            }
        }

        private static void TranslateHtmlInputButton(HtmlInputButton obj)
        {
            if (obj != null)
                obj.Value = TagReplace(obj.Value);
        }

        private static void TranslateContentPlaceHolder(ContentPlaceHolder obj)
        {
            if (obj != null)
                ObjectIdentify(obj.Controls);
        }

        private static void TranslateTextBoxWatermarkExtender(TextBoxWatermarkExtender obj)
        {
            if (obj != null)
                obj.WatermarkText = TagReplace(obj.WatermarkText);
        }

        private static void TranslateReorderList(ReorderList obj)
        {

        }

        private static void TranslateAccordion(Accordion obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                ObjectIdentify(obj.Controls);
            }
        }

        private static void TranslateAccordionPane(AccordionPane obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                for (int i = 0; i < obj.Controls.Count; i++)
                {
                    ObjectIdentify(obj.Controls[i].Controls);

                }
            }
        }



        private static void TranslateTabPanel(TabPanel obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                if (obj.HeaderTemplate == null && obj.HeaderText != "")
                {
                    obj.HeaderText = TagReplace(obj.HeaderText);
                }
                for (int i = 0; i < obj.Controls.Count; i++)
                {
                    ObjectIdentify(obj.Controls[i].Controls);
                }
            }
            

        }

        private static void TranslateTabContainer(TabContainer obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);
                ObjectIdentify(obj.Controls);
            }
        }

        private static void TranslatePanel(Panel obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);
                ObjectIdentify(obj.Controls);
            }
        }


        private static void TranslateTableRow(TableRow row)
        {
            if (row != null)
            {
                row.ToolTip = TagReplace(row.ToolTip);

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Controls.Count > 0)
                    {
                        ObjectIdentify(row.Cells[i].Controls);
                    }
                    else if (row.Cells[i].Text != "")
                    {
                        row.Cells[i].Text = TagReplace(row.Cells[i].Text);
                    }
                }
            }
        }

        private static void TranslateDetailsView(DetailsView obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                TranslateTableRow(obj.HeaderRow);
                TranslateTableRow(obj.FooterRow);
                for (int i = 0; i < obj.Rows.Count; i++)
                {
                    TranslateTableRow(obj.Rows[i]);
                }
            }
        }

        private static void TranslateGridView(GridView obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                if (obj.Rows.Count > 0)
                {
                    if (obj.HeaderRow.Visible) TranslateTableRow(obj.HeaderRow);
                    if (obj.FooterRow.Visible) TranslateTableRow(obj.FooterRow);
                    for (int i = 0; i < obj.Rows.Count; i++)
                    {
                        TranslateTableRow(obj.Rows[i]);
                    }
                }
            }
        }

        private static void TranslateUpdatePanel(UpdatePanel obj)
        {
            if(obj != null)
                ObjectIdentify(obj.ContentTemplateContainer.Controls);

        }
        private static void TranslateUpdateProgress(UpdateProgress obj)
        {
           
            if (obj != null)
                ObjectIdentify(obj.Controls);


        }

        private static void TranslateBulletedList(System.Web.UI.WebControls.BulletedList obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                for (int i = 0; i < obj.Items.Count; i++)
                {
                    obj.Items[i].Text = TagReplace(obj.Items[i].Text);
                }
            }
        }

        private static void TranslateRadioButtonList(RadioButtonList obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                for (int i = 0; i < obj.Items.Count; i++)
                {
                    obj.Items[i].Text = TagReplace(obj.Items[i].Text);
                }
            }
        }

        private static void TranslateDropDownList(DropDownList obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                for (int i = 0; i < obj.Items.Count; i++)
                {
                    obj.Items[i].Text = TagReplace(obj.Items[i].Text);
                }
            }
        }

        private static void TranslateCheckBoxList(CheckBoxList obj)
        {
            if (obj != null)
            {
                obj.ToolTip = TagReplace(obj.ToolTip);

                for (int i = 0; i < obj.Items.Count; i++)
                {

                    obj.Items[i].Text = TagReplace(obj.Items[i].Text);

                }
            }
        }


        private static void TranslateLiteral(Literal obj)
        {
            if(obj !=null)
                obj.Text = TagReplace(obj.Text);

        }

       
        private static void TranslateLabel(Label obj)
        {
            if (obj != null)
            {
                obj.Text = TagReplace(obj.Text);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }
        }

        private static void TranslateButton(Button obj)
        {
            if (obj != null)
            {
                obj.Text = TagReplace(obj.Text);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }

        }
        private static void TranslateImageButton(ImageButton obj)
        {
            if (obj != null)
            {
                obj.AlternateText = TagReplace(obj.AlternateText);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }
        }

        private static void TranslateCheckBox(CheckBox obj)
        {
            if (obj != null)
            {
                obj.Text = TagReplace(obj.Text);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }

        }

        private static void TranslateRadioButton(RadioButton obj)
        {
            if (obj != null)
            {
                obj.Text = TagReplace(obj.Text);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }

        }

        private static void TranslateLinkButton(LinkButton obj)
        {
            if (obj != null)
            {
                obj.Text = TagReplace(obj.Text);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }

        }

        private static void TranslateHyperLink(HyperLink obj)
        {
            if (obj != null)
            {
                obj.Text = TagReplace(obj.Text);
                obj.ToolTip = TagReplace(obj.ToolTip);
            }

        }


       
    }
}
