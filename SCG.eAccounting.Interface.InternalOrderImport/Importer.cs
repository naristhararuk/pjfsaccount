using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.IO;

using SCG.eAccounting.Interface.InternalOrderImport.DAL;

using SS.DB.Query;

using SCG.DB.BLL;
using SCG.DB.DTO;

namespace SCG.eAccounting.Interface.InternalOrderImport
{
    class Importer
    {
        private string path;
        private string eccflag;
        private string text = "";
        private long count_line = 1;
        private string programName = "ImportInternalOrder";
        private long createBy;
        private long updateBy;
        private string aliasName;
        private string subFolder;
        private string filename;

        public Importer(string path, string filename, string eccflag, string aliasName, string subFolder)
        {
            this.path = path;
            this.eccflag = eccflag;
            Factory.CreateTmpDbInternalOrderObject();
            Factory.CreateDbioImportLogObject();
            this.updateBy = this.createBy = ParameterServices.ImportSystemUserID;
            this.filename = filename;
            this.aliasName = aliasName;
            this.subFolder = subFolder;
        }

        public void startImport()
        {
            if (eccflag == "1")
                path += aliasName + "\\" + subFolder + "\\" +filename;
            else
                path += filename;
            Factory.TmpDbInternalOrderService.deleteAllInternalOrderTmp();
            using (StreamReader sr = new StreamReader(convertEncoding(path)))
            {
                while ((text = sr.ReadLine()) != null)
                {
                    if (text.Equals(""))
                    {
                        count_line++;
                        continue;
                    }
                    string[] data = (text).Split('^');

                    if (!addInternalOrderDataToTmp(data, eccflag == "1" ? true : false))
                    {
                        Console.WriteLine("cannot insert line : " + count_line);
                    }
                    count_line++;
                }

                // set company id and costcenter id 
                Factory.TmpDbInternalOrderService.setCompanyIDAndCostCenterIDToTmp();

                // add error to log
                Factory.TmpDbInternalOrderService.putMissingCompanyIDToLog();

                #region comment un-use
                // ไม่ต้อง Check CostCenter เพราะว่าเป็นแค่ Option
                //Factory.TmpDbInternalOrderService.putMissingCostCenterIDToLog();

                //Factory.TmpDbInternalOrderService.putMissingIONumberToLog();
                //Factory.TmpDbInternalOrderService.putMissingIOTypeToLog();
                //Factory.TmpDbInternalOrderService.putMissingIOTextToLog();
                //Factory.TmpDbInternalOrderService.putMissingCompanyCodeToLog();
                //Factory.TmpDbInternalOrderService.putMissingCostCenterCodeToLog();
                //Factory.TmpDbInternalOrderService.putMissingEffectiveDateToLog();
                //Factory.TmpDbInternalOrderService.putMissingExpireDateDateToLog();


                // set all active = false
                //Factory.TmpDbInternalOrderService.setInternalOrderActive(false, Convert.ToBoolean(eccflag == "1" ? true : false));
                //Factory.TmpDbInternalOrderService.addNewInternalOrderFromTmp();
                //Factory.TmpDbInternalOrderService.updateNewInternalOrder();
                #endregion

                // add new/update internal order to DbInternalOrder
                Factory.TmpDbInternalOrderService.ImportNewInternalOrder(eccflag == "1" ? true : false,aliasName);
            }
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
                //return null;
                // ถ้าแปลงไม่ได้ให้ Return '12.12.9999'

                DateTime USdateTime = Convert.ToDateTime("12.12.9999", new CultureInfo("en-US"));
                return Convert.ToDateTime(USdateTime, Thread.CurrentThread.CurrentCulture);
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

        private bool addInternalOrderDataToTmp(string[] data, bool eccflag)
        {

            long companyID = 0;
            long costCenterID = 0;
            string ioNumber = "";
            string ioType = "";
            string ioText = "";
            string companyCode = "";
            string costCenterCode = "";
            DateTime? effectiveDate = null;
            DateTime? expireDate = null;
            string businessArea = "";
            string profitCenter = "";

            bool canInsert = true;

            try
            {
                ioNumber = data[0].TrimEnd();
                ioType = data[1].TrimEnd();
                ioText = data[2].TrimEnd();
                companyCode = data[3].TrimEnd();
                costCenterCode = data[5].TrimEnd();
                effectiveDate = changeUsDateToCurrentCulture(data[6].TrimEnd());
                expireDate = changeUsDateToCurrentCulture(data[7].TrimEnd());

                if (eccflag)
                {
                    businessArea = data[8].TrimEnd();
                    profitCenter = data[9].TrimEnd();
                }
            }
            catch (IndexOutOfRangeException ie)
            {
                // do nothing 
                // use defalut value 
                Console.WriteLine(string.Format("IONumber : {0} => {1}", ioNumber, ie.ToString()));
                canInsert = false;
            }


            string errorMessage = "";
            int errorCode = 0;
            if (ioNumber.Equals(""))
            {
                errorCode = 4;
                errorMessage = "Column IONumber contain no data";
                canInsert = false;
            }
            if (ioType.Equals(""))
            {
                errorCode = 4;
                errorMessage = "Column IOType contain no data";
                canInsert = false;
            }
            if (ioText.Equals(""))
            {
                errorCode = 4;
                errorMessage = "Column IOText contain no data";
                canInsert = false;
            }

            if (companyCode.Equals(""))
            {
                errorCode = 4;
                errorMessage = "Column CompanyCode contain no data";
                canInsert = false;
            }
            // Cost Center เป็น Null ได้ครับ
            //if (costCenterCode.Equals(""))
            //{
            //    errorCode = 4;
            //    errorMessage = "Column CostCenterCode contain no data";
            //    canInsert = false;
            //}

            if (effectiveDate == null)
            {
                errorCode = 5;
                errorMessage = "Invalid data format of column EffectiveDate";
                canInsert = false;
            }

            if (expireDate == null)
            {
                expireDate = DateTime.MaxValue;
            }


            if (canInsert)
            {
                TmpDbInternalOrder tmpInternalOrder = new TmpDbInternalOrder();
                tmpInternalOrder.IONumber = ioNumber;
                tmpInternalOrder.IOType = ioType;
                tmpInternalOrder.IOText = ioText;

                tmpInternalOrder.CostCenterID = costCenterID;
                tmpInternalOrder.CostCenterCode = costCenterCode;

                tmpInternalOrder.CompanyID = companyID;
                tmpInternalOrder.CompanyCode = companyCode;


                tmpInternalOrder.EffectiveDate = effectiveDate;
                tmpInternalOrder.ExpireDate = expireDate;

                tmpInternalOrder.BusinessArea = businessArea;
                tmpInternalOrder.ProfitCenter = profitCenter;

                tmpInternalOrder.Active = true;
                tmpInternalOrder.CreBy = this.createBy;
                tmpInternalOrder.CreDate = DateTime.Now;
                tmpInternalOrder.UpdBy = this.updateBy;
                tmpInternalOrder.UpdDate = DateTime.Now;
                tmpInternalOrder.UpdPgm = programName;
                tmpInternalOrder.Line = count_line;
                Factory.TmpDbInternalOrderService.addTmpInternalOrder(tmpInternalOrder);
            }
            else
            {
                DbioImportLog log = new DbioImportLog();
                log.Active = true;
                if (!string.IsNullOrEmpty(companyCode) && companyCode.Length > 20)
                    log.CompanyCode = companyCode.Substring(0, 20);
                else
                    log.CompanyCode = companyCode;

                if (!string.IsNullOrEmpty(costCenterCode) && costCenterCode.Length > 20)
                    log.CostCenterCode = costCenterCode.Substring(0, 20);
                else
                    log.CostCenterCode = costCenterCode;
                log.EffectiveDate = effectiveDate;
                log.ExpireDate = expireDate;
                if (!string.IsNullOrEmpty(ioNumber) && ioNumber.Length > 20)
                    log.IONumber = ioNumber.Substring(0, 20);
                else
                    log.IONumber = ioNumber;

                if (!string.IsNullOrEmpty(ioType) && ioType.Length > 50)
                    log.IOType = ioType.Substring(0, 49);
                else
                    log.IOType = ioType;

                if (!string.IsNullOrEmpty(ioText) && ioText.Length > 100)
                    log.IOText = ioText.Substring(0, 100);
                else
                    log.IOText = ioText;

                log.ErrorCode = errorCode;

                if (!string.IsNullOrEmpty(errorMessage) && errorMessage.Length > 100)
                    log.Message = errorMessage.Substring(0, 100);
                else
                    log.Message = errorMessage;

                log.Line = count_line;
                Factory.DbioImportLogService.Save(log);

            }

            return canInsert;
        }

    }
}
