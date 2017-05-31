using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;
using SCG.eAccounting.Query;
using System.Text;
using System.Data;
using SCG.eAccounting.BLL.Implement;
using SCG.eAccounting.DTO;
using SS.DB.Query;
using System.IO;
using SS.Standard.Utilities;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SCG.DB.DTO;
using SCG.DB.Query;
using SCG.DB.DAL;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.eAccounting
{
    public partial class MileageRateRevisionLookUp : BasePage
    {
        #region Properties
        public string addError { get; set; }
        public string addError2 { get; set; }
        public string addError3 { get; set; }

        private Guid mRRevisionId;

        public Guid MRRevisionId
        {
            get { return mRRevisionId; }
            set { mRRevisionId = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            MRRevisionId = new Guid(Request["MRRevitionId"].ToString());
        }

        private void CallOnObjectLookUpReturn(string id)
        {
            //Hide();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "notifyPopupResult()", "notifyPopupResult('ok' , '" + id + "')", true);
        }

        protected void ctlAttach_OnClick(object sender, ImageClickEventArgs e)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            string filePath = ParameterServices.LocalAccessUploadFilePath;

            if (ctlFileUpload.HasFile)
            {
                FileInfo info = new FileInfo(ctlFileUpload.PostedFile.FileName);
                string fileName = info.Name;

                if (this.ValidationErrors.IsEmpty)
                {
                    try
                    {
                        // Save new file to stored directory.
                        string storePath = string.Empty;
                        storePath = AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\") + "\\" + this.MRRevisionId.ToString() + "\\";
                        if (Directory.Exists(storePath))
                        {
                            ctlFileUpload.SaveAs(storePath + fileName);
                            ImPortExcelFile(storePath, fileName);

                        }
                        else
                        {
                            if (!Directory.Exists(storePath))
                            {
                                Directory.CreateDirectory(storePath);
                            }
                            ctlFileUpload.SaveAs(storePath + fileName);
                            ImPortExcelFile(storePath, fileName);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), this.ClientID + "uploadfile", "parent.uploadfile();", true);
                    string duplicate = string.Empty;
                    string found = string.Empty;
                    if (!String.IsNullOrEmpty(addError2))
                    {
                        duplicate = "Duplicate data column : " + addError2;
                    }
                    if (!String.IsNullOrEmpty(addError3))
                    {
                        found = "Find not found data column : " + addError3;
                    }

                    if (!String.IsNullOrEmpty(addError3) || !String.IsNullOrEmpty(addError2))
                        this.ValidationErrors.AddError("ImPortExcel.Error", new Spring.Validation.ErrorMessage(duplicate + found));
                }
            }
        }

        protected void ctlCancel_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.ClientID + "Cancel", "notifyPopupResult('cancel');", true);
        }

        private void ImPortExcelFile(string storePath, string fileName)
        {
            addError2 = String.Empty;
            IList<DbMileageRateRevisionDetail> rows = new List<DbMileageRateRevisionDetail>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(storePath + fileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
                {
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().Where(t => t.HasChildren).FirstOrDefault();

                    if (sheetData == null)
                        continue;

                    foreach (Row r in sheetData.Elements<Row>())
                    {
                        DbMileageRateRevisionDetail row = new DbMileageRateRevisionDetail();
                        if (r.RowIndex == 1)
                        {
                            IList<DbMileageRateRevisionDetail> mRiTemId = ScgDbQueryProvider.DbMileageRateRevisionDetailQuery.FindForRemoveMileageRateRevisionItem(MRRevisionId);
                            foreach (DbMileageRateRevisionDetail mileageId in mRiTemId)
                            {
                                ScgDbDaoProvider.DbMileageRateRevisionDetailDao.Delete(mileageId);
                            }
                            continue;
                        }/*check if empty all column break*/
                        List<Cell> cells = r.Elements<Cell>().ToList();
                        row.MileageRateRevisionId = MRRevisionId;
                        Guid? result = ScgDbQueryProvider.DbProfileListQuery.GetProfileListIdByName(ExcelUtility.GetValue(workbookPart, r, 0));
                        row.MileageProfileId = new Guid(result.ToString());
                        row.PersonalLevelGroupCode = ExcelUtility.GetValue(workbookPart, r, 1);
                        row.CarRate = !String.IsNullOrEmpty(ExcelUtility.GetValue(workbookPart, r, 2)) ? UIHelper.ParseDouble(ExcelUtility.GetValue(workbookPart, r, 2)) : 0;
                        row.CarRate2 = !String.IsNullOrEmpty(ExcelUtility.GetValue(workbookPart, r, 3)) ? UIHelper.ParseDouble(ExcelUtility.GetValue(workbookPart, r, 3)) : 0;
                        row.MotocycleRate = !String.IsNullOrEmpty(ExcelUtility.GetValue(workbookPart, r, 4)) ? UIHelper.ParseDouble(ExcelUtility.GetValue(workbookPart, r, 4)) : 0;
                        row.MotocycleRate2 = !String.IsNullOrEmpty(ExcelUtility.GetValue(workbookPart, r, 5)) ? UIHelper.ParseDouble(ExcelUtility.GetValue(workbookPart, r, 5)) : 0;
                        row.PickUpRate = !String.IsNullOrEmpty(ExcelUtility.GetValue(workbookPart, r, 6)) ? UIHelper.ParseDouble(ExcelUtility.GetValue(workbookPart, r, 6)) : 0;
                        row.PickUpRate2 = !String.IsNullOrEmpty(ExcelUtility.GetValue(workbookPart, r, 7)) ? UIHelper.ParseDouble(ExcelUtility.GetValue(workbookPart, r, 7)) : 0;
                        row.Active = true;
                        row.CreBy = 1;
                        row.CreDate = DateTime.Now;
                        row.UpdBy = 1;
                        row.UpdDate = DateTime.Now;
                        row.UpdPgm = "1";
                        rows.Add(row);
                    }

                    break;
                }
            }
            //int count = 1;
            //DbMileageRateRevisionDetail temp = null ;
            for (int index = 0; index < rows.Count; index++)
            {
                addError = string.Empty;
                for (int index2 = 0; index2 < rows.Count; index2++)
                {
                    if (index != index2)
                    {
                        if (rows[index].MileageProfileId == new Guid())
                        {
                            addError = "found"; break;
                        }
                        if (rows[index].MileageProfileId == rows[index2].MileageProfileId && rows[index].PersonalLevelGroupCode == rows[index2].PersonalLevelGroupCode)
                        {
                            addError = "duplicate";
                        }

                    }
                }

                if (String.IsNullOrEmpty(addError))
                {
                    ScgDbDaoProvider.DbMileageRateRevisionDetailDao.SaveOrUpdate(rows[index]);
                }
                else if (addError == "duplicate")
                {
                    //if (!String.IsNullOrEmpty(addError2))
                    //{
                    //    addError2 += ",";
                    //}
                    addError2 += (index + 2).ToString() + " ";
                }
                else if (addError == "found")
                {
                    //if (!String.IsNullOrEmpty(addError3))
                    //{
                    //    addError3 += ",";
                    //}
                    addError3 += (index + 2).ToString() + " ";
                }
            }
        }
    }
}