using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;

using SS.DB.BLL;
using SS.DB.DTO;

using SCG.DB.BLL;
using SCG.DB.DTO;
using SS.DB.Query;

using SCG.eAccounting.Interface.CostcenterImport.DAL;

namespace SCG.eAccounting.Interface.CostcenterImport
{
    class Importer
    {
        private string path;
        private string eccFlag;
        private string text;
        private TmpDbCostCenter tmpDbCostCenter;
        private long createBy;
        private long updateBy;
        private string programName = "ImportCostCenter";
        private long count_line = 1;
        private string aliasName;
        private string subFolder;

        public Importer(string path, string eccFlag,string aliasName,string subFolder)
        {
            this.path = path;
            this.eccFlag = eccFlag;
            Factory.CreateTmpDbCostCenterObject();
            Factory.CreateDbCostCenterImportLogObject();
            this.updateBy = this.createBy = ParameterServices.ImportSystemUserID;
            this.aliasName = aliasName;
            this.subFolder = subFolder;
        }

        public void startImport()
        {
            if (eccFlag == "1")
                path += aliasName + "\\" + subFolder;
            using (StreamReader sr = new StreamReader(convertEncoding(path)))
            {
                // delete all tmp

                Factory.TmpDbCostCenterService.deleteAllTmpDbCostCenter();

                while ((text = sr.ReadLine()) != null)
                {

                    if (text.Equals(""))
                    {
                        count_line++;
                        continue;
                    }

                    string[] data = text.Split('^');
                    if (!addTmpCostCenterData(data, eccFlag == "1" ? true : false))
                    {
                        Console.WriteLine("cannot insert line : " + count_line);
                    }
                    count_line++;
                }

                Factory.TmpDbCostCenterService.setCompanyIDToTmpCostCenter();

                // checking invalid data and insert to log

                Factory.TmpDbCostCenterService.checkMissingCostCenterCode();
                Factory.TmpDbCostCenterService.checkMissingCompany();

                #region comment un-use
                // Factory.TmpDbCostCenterService.checkMissingDescription();
                // Factory.TmpDbCostCenterService.checkValidDateFormat();
                // Factory.TmpDbCostCenterService.checkExpireDateFormat();


                //set all record in  DbCostCenter active = false
                //Factory.TmpDbCostCenterService.setActiveDbCosCenter(false);
                // add new costcenter from tmp
                //Factory.TmpDbCostCenterService.addNewCostCenterFromTmp();
                // update new costcenter from tmp
                //Factory.TmpDbCostCenterService.updateCostCenterFromTmp();
                #endregion

                // add new costcenter to DbCostCenter
                Factory.TmpDbCostCenterService.ImportNewCostCenter(eccFlag == "1" ? true : false, aliasName);
            }
        }
        private string convertEncoding(string path)
        {
            string OutputFilePath = Path.Combine(Path.GetDirectoryName(path), "tmp_" + Path.GetFileName(path));
            FileStream fs = File.OpenRead(path);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();
            Encoding inputEnc = Encoding.GetEncoding("windows-874");
            Encoding outputEnc = Encoding.GetEncoding("utf-8");
            byte[] decoded = Encoding.Convert(inputEnc, outputEnc, bytes, 0, bytes.Length);
            FileStream fw = File.OpenWrite(OutputFilePath);
            fw.Write(decoded, 0, (int)decoded.Length);
            fw.Close();
            return OutputFilePath;
        }
        private DateTime? changeUsDateToCurrentCulture(string date)
        {
            try
            {
                string replaceString = date.Replace("/", ".");
                string[] str = replaceString.Split('.');
                string day = str[0];
                string month = str[1];
                string year = str[2];

                if (int.Parse(year) < 1753 || int.Parse(year) > 9999)
                    return null;

                string usFormat = month + "." + day + "." + year;
                DateTime USdateTime = Convert.ToDateTime(usFormat, new CultureInfo("en-US"));

                return Convert.ToDateTime(USdateTime, Thread.CurrentThread.CurrentCulture);
            }
            catch (Exception fe)
            {
                return null;
            }

        }
        private bool addTmpCostCenterData(string[] data, bool eccflag)
        {
            string costCenterCode = "";
            DateTime? valid = null;
            DateTime? expire = null;
            string description = "";
            string companyCode = "";
            string businessArea = "";
            string profitCenter = "";

            bool canInsert = true;
            try
            {
                costCenterCode = data[0].TrimEnd();
                valid = changeUsDateToCurrentCulture(data[1].TrimEnd());
                expire = changeUsDateToCurrentCulture(data[2].TrimEnd());
                description = data[3].TrimEnd();
                companyCode = data[4].TrimEnd();

                if (eccflag)
                {
                    businessArea = data[7].TrimEnd();
                    profitCenter = data[8].TrimEnd();
                }
            }
            catch (IndexOutOfRangeException ie)
            {
                // do nothing 
                // use defalut value 
                Console.WriteLine(string.Format("CostCenter : {0} => {1}", costCenterCode, ie.ToString()));
                canInsert = false;
            }
            string errorMessage = "";
            int errorCode = 0;

            if (companyCode.TrimEnd().Equals(""))
            {
                errorCode = 2;
                errorMessage = "Column CompanyCode contain no data";
                canInsert = false;
            }
            if (costCenterCode.TrimEnd().Equals(""))
            {
                errorCode = 2;
                errorMessage = "Column CostCenterCode contain no data";
                canInsert = false;
            }
            if (description.TrimEnd().Equals(""))
            {
                errorCode = 2;
                errorMessage = "Column Description contain no data";
                canInsert = false;
            }
            if (valid == null)
            {
                errorCode = 3;
                errorMessage = "Invalid data format of column Valid";
                canInsert = false;
            }
            if (expire == null)
            {
                errorCode = 3;
                errorMessage = "Invalid data format of column Expire";
                canInsert = false;
            }

            long companyID = 0;


            if (canInsert)
            {
                tmpDbCostCenter = new TmpDbCostCenter();
                tmpDbCostCenter.Active = true;
                tmpDbCostCenter.CostCenterCode = costCenterCode;
                tmpDbCostCenter.Valid = valid;
                tmpDbCostCenter.Expire = expire;
                tmpDbCostCenter.Description = description;
                tmpDbCostCenter.CompanyCode = companyCode;
                tmpDbCostCenter.ActualPrimaryCosts = true;
                tmpDbCostCenter.CompanyID = companyID;
                tmpDbCostCenter.BusinessArea = businessArea;
                tmpDbCostCenter.ProfitCenter = profitCenter;
                // fix code


                tmpDbCostCenter.CreBy = this.createBy;
                tmpDbCostCenter.UpdBy = this.updateBy;
                tmpDbCostCenter.CreDate = DateTime.Now;
                tmpDbCostCenter.UpdDate = DateTime.Now;
                tmpDbCostCenter.UpdPgm = this.programName;
                tmpDbCostCenter.RowVersion = null;
                tmpDbCostCenter.Line = count_line;
                //Console.WriteLine(data[0]);
                if (!Factory.TmpDbCostCenterService.IsDuplicateCostCenterCode(costCenterCode))
                {
                    Factory.TmpDbCostCenterService.addTmpDbCostCenter(tmpDbCostCenter);
                }
            }
            else
            {
                DbCostCenterImportLog log = new DbCostCenterImportLog();
                log.CompanyCode = companyCode;
                log.CostCenterCode = costCenterCode;
                log.ValidFrom = valid;
                log.ExpireDate = expire;
                log.Description = description;
                log.ErrorCode = errorCode;
                log.Message = errorMessage;
                log.Active = true;
                log.Line = count_line;

                Factory.DbCostCenterImportLogService.Save(log);
            }

            return canInsert;
        }
    }
}
