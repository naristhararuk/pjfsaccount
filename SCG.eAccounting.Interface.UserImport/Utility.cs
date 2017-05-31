using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS.SU.DTO;
using System.Globalization;
using SS.Standard.Utilities;
using SCG.eAccounting.Interface.UserImport.DAL;
using SS.DB.Query;
using SCG.DB.DTO;
using SCG.DB.Query;
namespace SCG.eAccounting.Interface.UserImport
{
    class Utility
    {
        private CultureInfo provider = new CultureInfo("en-US");
        public void ReadFile(string filename)
        {
            string strMaxPasswordAge = ParameterServices.MaxPasswordAge.ToString();
            //modify by meaw change maximum password length to parameter and set unuse symbol for generate password
            PasswordGeneration passwordGeneration = new PasswordGeneration(ParameterServices.MinPasswordLength, ParameterServices.MaxPasswordLength, false, true, true, true, false);


            Encoding inputEnc = Encoding.GetEncoding("windows-874");


            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                string s;
                int lineNo = 0;
                using (StreamReader sr = new StreamReader(fs))
                {
                    s = sr.ReadLine();

                    while (s != null)
                    {
                        lineNo++;
                        if (lineNo != 1)
                        {
                            if (!String.IsNullOrEmpty(s))
                            {
                                try
                                {
                                    string[] ss = s.Split('|');
                                    if (ss.Length >= 21)
                                    {
                                        string password = passwordGeneration.Create();
                                        int errors = 0;
                                        TmpSuUser tmpSuUser = new TmpSuUser();
                                        if (IsRequireField(ss[3].TrimEnd(), "EmployeeCode", ss[0].TrimEnd(), ss[106].TrimEnd(), lineNo))
                                        {
                                            tmpSuUser.EmployeeCode = ss[3].TrimEnd();
                                        }
                                        else
                                        {
                                            errors++;
                                        }
                                        tmpSuUser.CompanyCode = ss[46].TrimEnd();
                                        tmpSuUser.CostCenterCode = ss[42].TrimEnd();
                                        tmpSuUser.PayrollCostCenter = ss[121].TrimEnd();
                                        tmpSuUser.LocationCode = ss[44].TrimEnd();
                                        if (IsRequireField(ss[106].TrimEnd(), "UserName", ss[0].TrimEnd(), ss[106].TrimEnd(), lineNo))
                                        {
                                            tmpSuUser.UserName = ss[106].TrimEnd();
                                        }
                                        else
                                        {
                                            errors++;
                                        }
                                        tmpSuUser.Password = Encryption.Md5Hash(password);
                                        tmpSuUser.PasswordExpiryDate = DateTime.Now.AddDays(int.Parse(strMaxPasswordAge));
                                        if (IsRequireField(ss[0].TrimEnd(), "PeopleID", ss[0].TrimEnd(), ss[106].TrimEnd(), lineNo))
                                        {
                                            tmpSuUser.PeopleID = ss[0].TrimEnd();
                                        }
                                        else
                                        {
                                            errors++;
                                        }
                                        if (IsRequireField(ss[5].TrimEnd() + " " + ss[6].TrimEnd(), "EmployeeName", ss[0].TrimEnd(), ss[106].TrimEnd(), lineNo))
                                        {
                                            tmpSuUser.EmployeeName = ss[5].TrimEnd() + " " + ss[6].TrimEnd();
                                        }
                                        else
                                        {
                                            errors++;
                                        }
                                        tmpSuUser.SectionName = ss[31].TrimEnd();
                                        tmpSuUser.PersonalLevel = ss[53].TrimEnd();
                                        tmpSuUser.PersonalDescription = ss[54].TrimEnd();
                                        tmpSuUser.PersonalGroup = ss[53].TrimEnd();
                                        tmpSuUser.PersonalLevelGroupDescription = ss[54].TrimEnd();
                                        tmpSuUser.PositionName = ss[16].TrimEnd();
                                        tmpSuUser.SupervisorName = ss[73].TrimEnd();
                                        //tmpSuUser.LocationCode = ss[12].TrimEnd();
                                        //validate phone no
                                        tmpSuUser.PhoneNo = ss[100].TrimEnd();
                                        if (tmpSuUser.PhoneNo.Length > 20)
                                        {
                                            tmpSuUser.PhoneNo = tmpSuUser.PhoneNo.Substring(0, 19);
                                        }
                                        if (!string.IsNullOrEmpty(ss[62]))
                                        {
                                            tmpSuUser.HireDate = IsDate(ss[62].TrimEnd(), "HireDate", ss[0].TrimEnd(), ss[106].TrimEnd(), lineNo);
                                        }
                                        if (!string.IsNullOrEmpty(ss[65]))
                                        {
                                            tmpSuUser.TerminateDate = IsDate(ss[65].TrimEnd(), "TerminateDate", ss[0].TrimEnd(), ss[106].TrimEnd(), lineNo);
                                        }
                                        tmpSuUser.ApprovalFlag = convertApprovalFlag(ss[52].TrimEnd());
                                        tmpSuUser.Email = ss[99].TrimEnd();
                                        tmpSuUser.FromEHr = true;
                                        tmpSuUser.Active = true;
                                        tmpSuUser.isNewUser = false;
                                        tmpSuUser.OldPassword = password;
                                        tmpSuUser.LineNumber = lineNo;
                                        if (!string.IsNullOrEmpty(ss[110].TrimEnd()))
                                            tmpSuUser.VendorCode = ss[110].TrimEnd();

                                        if (errors > 0)
                                        {
                                            s = sr.ReadLine();
                                            continue;
                                        }
                                        else
                                        {
                                            Factory.TmpSuUserService.AddUser(tmpSuUser);

                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid file format");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Line No:" + lineNo + " => Error : " + ex.ToString());
                                }
                            }
                        }
                        s = sr.ReadLine();
                    }
                }

            }
        }

        private bool? convertApprovalFlag(string input)
        {
            if (input.ToLower() == "y")
                return true;
            else if (input.ToLower() == "n")
                return false;
            else
                return null;
        }

        private string convertEncoding(string filename)
        {
            string OutputFileName = "tmp_" + filename.Substring(filename.LastIndexOf("\\") + 1);
            FileStream fs = File.OpenRead(filename);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();
            Encoding inputEnc = Encoding.GetEncoding("windows-874");
            Encoding outputEnc = Encoding.GetEncoding("utf-8");
            byte[] decoded = Encoding.Convert(inputEnc, outputEnc, bytes, 0, bytes.Length);
            FileStream fw = File.OpenWrite(OutputFileName);
            fw.Write(decoded, 0, (int)decoded.Length);
            fw.Close();
            return OutputFileName;
        }

        private DateTime? IsDate(string date, string column, string PeopleID, string UserName, long LineNumber)
        {
            DateTime? result;
            try
            {
                result = DateTime.Parse(date, provider);
            }
            catch (FormatException e)
            {
                SueHrProfileLog eHrLog = new SueHrProfileLog();
                eHrLog.PeopleID = PeopleID;
                eHrLog.UserName = UserName;
                eHrLog.Message = "Invalid data in column '" + column + "' at line " + LineNumber + ".";
                eHrLog.Active = true;
                eHrLog.UpdBy = ParameterServices.SystemUserID;
                eHrLog.UpdDate = DateTime.Now;
                eHrLog.CreDate = DateTime.Now;
                eHrLog.CreBy = ParameterServices.SystemUserID;
                eHrLog.UpdPgm = "UserImport";
                Factory.SuEHrProfileLogService.AddLog(eHrLog);
                result = null;
            }
            return result;
        }

        private bool IsRequireField(string data, string column, string PeopleID, string UserName, long LineNumber)
        {
            bool result;

            if (data == " " || data == "")
            {
                SueHrProfileLog eHrLog = new SueHrProfileLog();
                eHrLog.PeopleID = PeopleID;
                eHrLog.UserName = UserName;
                eHrLog.Message = "Column '" + column + "' is empty at line " + LineNumber + ".";
                eHrLog.Active = true;
                eHrLog.UpdBy = ParameterServices.SystemUserID;
                eHrLog.UpdDate = DateTime.Now;
                eHrLog.CreDate = DateTime.Now;
                eHrLog.CreBy = ParameterServices.SystemUserID;
                eHrLog.UpdPgm = "UserImport";
                Factory.SuEHrProfileLogService.AddLog(eHrLog);
                result = false;
            }
            else
                result = true;
            return result;

        }
    }
}
