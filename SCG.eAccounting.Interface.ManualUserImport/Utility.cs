using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS.SU.DTO;
using System.Globalization;
using SS.Standard.Utilities;
using SCG.eAccounting.Interface.ManualUserImport.DAL;
using SS.DB.Query;
namespace SCG.eAccounting.Interface.ManualUserImport
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
                
                using (StreamReader sr = new StreamReader(fs, inputEnc))
                {
                    s = sr.ReadLine();

                    while (s != null)
                    {
                        lineNo++;
                        if (!String.IsNullOrEmpty(s))
                        {
                            //Console.WriteLine(s);
                            string[] ss = s.Split('|');
                            if (ss.Length >= 23)
                            {
                                string password = passwordGeneration.Create();
                                int errors = 0;
                                TmpSuUser tmpSuUser = new TmpSuUser();
                                if (IsRequireField(ss[2].TrimEnd(), "EmployeeCode", ss[0].TrimEnd(), ss[1].TrimEnd(), lineNo))
                                {
                                    tmpSuUser.EmployeeCode = ss[2].TrimEnd();
                                }
                                else
                                {
                                    errors++;
                                }
                                tmpSuUser.CompanyCode = ss[4].TrimEnd();
                                tmpSuUser.CostCenterCode = ss[12].TrimEnd();
                                tmpSuUser.LocationCode = ss[13].TrimEnd();
                                if (IsRequireField(ss[1].TrimEnd(), "UserName", ss[0].TrimEnd(), ss[1].TrimEnd(), lineNo))
                                {
                                    tmpSuUser.UserName = ss[1].TrimEnd();
                                }
                                else
                                {
                                    errors++;
                                }
                                tmpSuUser.Password = Encryption.Md5Hash(password);
                                tmpSuUser.PasswordExpiryDate = DateTime.Now.AddDays(int.Parse(strMaxPasswordAge));
                                if (IsRequireField(ss[0].TrimEnd(), "PeopleID", ss[0].TrimEnd(), ss[1].TrimEnd(), lineNo))
                                {
                                    tmpSuUser.PeopleID = ss[0].TrimEnd();
                                }
                                else
                                {
                                    errors++;
                                }
                                if (IsRequireField(ss[2].TrimEnd(), "EmployeeName", ss[0].TrimEnd(), ss[1].TrimEnd(), lineNo))
                                {
                                    tmpSuUser.EmployeeName = ss[3].TrimEnd();
                                }
                                else
                                {
                                    errors++;
                                }
                                tmpSuUser.SectionName = ss[5].TrimEnd();
                                tmpSuUser.PersonalLevel = ss[6].TrimEnd();
                                tmpSuUser.PersonalDescription = ss[7].TrimEnd();
                                tmpSuUser.PersonalGroup = ss[8].TrimEnd();
                                tmpSuUser.PersonalLevelGroupDescription = ss[9].TrimEnd();
                                tmpSuUser.PositionName = ss[10].TrimEnd();
                                tmpSuUser.SupervisorName = ss[11].TrimEnd();
                                //tmpSuUser.LocationCode = ss[12].TrimEnd();
                                //validate phone no
                                tmpSuUser.PhoneNo = ss[16].TrimEnd();
                                if (tmpSuUser.PhoneNo.Length > 20)
                                {
                                    tmpSuUser.PhoneNo = tmpSuUser.PhoneNo.Substring(0, 19);
                                }


                                if (ss[17] != " ")
                                {
                                    tmpSuUser.HireDate = IsDate(ss[17].TrimEnd(), "HireDate", ss[0].TrimEnd(), ss[1].TrimEnd(), lineNo);
                                }
                                if (ss[18] != " ")
                                {
                                    tmpSuUser.TerminateDate = IsDate(ss[18].TrimEnd(), "TerminateDate", ss[0].TrimEnd(), ss[1].TrimEnd(), lineNo);
                                }
                                tmpSuUser.ApprovalFlag = convertApprovalFlag(ss[19].TrimEnd());
                                tmpSuUser.Email = ss[15].TrimEnd();
                                if (!string.IsNullOrEmpty(ss[20]))
                                {
                                    if (ss[20].Trim().Equals("Y"))
                                    {
                                        tmpSuUser.FromEHr = true;
                                    }
                                    else
                                    {
                                        tmpSuUser.FromEHr = false;
                                    }
                                }
                                tmpSuUser.Active = true;
                                tmpSuUser.isNewUser = false;
                                tmpSuUser.OldPassword = password;
                                tmpSuUser.LineNumber = lineNo;

                                if (!string.IsNullOrEmpty(ss[21]))
                                {
                                    if (ss[21].Trim().Equals("Y"))
                                    {
                                        tmpSuUser.isAdUser = true;
                                    }
                                    else
                                    {
                                        tmpSuUser.isAdUser = false;
                                    }
                                }
                                tmpSuUser.VendorCode = ss[22].TrimEnd();
                                if (errors > 0)
                                {
                                    s = sr.ReadLine();
                                    continue;
                                }
                                else
                                    Factory.TmpSuUserService.AddUser(tmpSuUser);
                            }
                            else
                            {
                                Console.WriteLine("Invalid file format");
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
            string OutputFileName = "tmp_" + filename.Substring(filename.LastIndexOf("\\")+1) ;
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
                result = DateTime.ParseExact(date, "yyyymmdd", provider);
            }
            catch(FormatException e)
            {
                SueHrProfileLog eHrLog = new SueHrProfileLog();
                eHrLog.PeopleID = PeopleID;
                eHrLog.UserName = UserName;
                eHrLog.Message = "Invalid data in column '"+column+"' at line "+LineNumber+".";
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
