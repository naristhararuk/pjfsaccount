using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS.DB.BLL;
using SS.DB.DTO;

using SCG.DB.BLL;
using SCG.DB.DTO;
using SS.DB.Query;

using SCG.eAccounting.Interface.OrganizationImport.DAL;

namespace SCG.eAccounting.Interface.OrganizationImport
{
    class Utility
    {
        public void ReadFile(string filename)
        {
            Encoding inputEnc = Encoding.GetEncoding("windows-874");


            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string s;
                    int count = 1;
                    s = sr.ReadLine();
                    while (s != null)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            if (count != 1)
                            {
                                TmpDbOrganizationchart tmpOrgChart = new TmpDbOrganizationchart();
                                //Console.WriteLine();
                                //Console.WriteLine("Row:" + count);
                                string[] ss = s.Split('|');
                                if (ss.Length != 7)
                                {
                                    tmpOrgChart.CostCenterCode = ss[9].TrimEnd();
                                    tmpOrgChart.DepartmentCode = ss[0].TrimEnd();
                                    tmpOrgChart.DepartmentDescriptionEN = ss[2].TrimEnd();
                                    tmpOrgChart.ParentDepartmentCode = ss[3].TrimEnd();
                                    tmpOrgChart.Active = true;
                                    tmpOrgChart.UpdBy = ParameterServices.SystemUserID;
                                    tmpOrgChart.UpdDate = DateTime.Now;
                                    tmpOrgChart.CreBy = ParameterServices.SystemUserID;
                                    tmpOrgChart.CreDate = DateTime.Now;
                                    tmpOrgChart.UpdPgm = "OrganizationImport";
                                    tmpOrgChart.DepartmentDescriptionTH = ss[1].TrimEnd();
                                    tmpOrgChart.DepartmentLevelCode = ss[4].TrimEnd();
                                    tmpOrgChart.DepartmentLevelName = ss[5].TrimEnd();
                                    tmpOrgChart.Relationship = ss[6].TrimEnd();
                                    tmpOrgChart.CompanyCode = ss[7].TrimEnd();
                                    tmpOrgChart.CompanyNameTH = ss[8].TrimEnd();
                                    tmpOrgChart.CostCenterDescription = ss[10].TrimEnd();
                                    tmpOrgChart.ManagerPersonID = ss[11].TrimEnd();
                                    tmpOrgChart.ManagerName = ss[12].TrimEnd();
                                    tmpOrgChart.ManagerPosition = ss[13].TrimEnd();
                                    tmpOrgChart.ManagerPositionName = ss[14].TrimEnd();
                                    tmpOrgChart.ManagerUserID = ss[15].TrimEnd();
                                    tmpOrgChart.ManagerReportToUserID = ss[16].TrimEnd();
                                    tmpOrgChart.Sub1BusinessUnitEN = ss[17].TrimEnd();
                                    tmpOrgChart.Sub1BusinessUnitTH = ss[18].TrimEnd();
                                    tmpOrgChart.Sub1_1BusinessUnitEN = ss[19].TrimEnd();
                                    tmpOrgChart.Sub1_1BusinessUnitTH = ss[20].TrimEnd();
                                    tmpOrgChart.CompanyEN = ss[21].TrimEnd();
                                    tmpOrgChart.CompanyTH = ss[22].TrimEnd();
                                    tmpOrgChart.Sub1CompanyEN = ss[23].TrimEnd();
                                    tmpOrgChart.Sub1CompanyTH = ss[24].TrimEnd();
                                    tmpOrgChart.DivisionNameEN = ss[25].TrimEnd();
                                    tmpOrgChart.DivisionNameTH = ss[26].TrimEnd();
                                    tmpOrgChart.Sub1DivisionNameEN = ss[27].TrimEnd();
                                    tmpOrgChart.Sub1DivisionNameTH = ss[28].TrimEnd();
                                    tmpOrgChart.DepartmentNameEN = ss[29].TrimEnd();
                                    tmpOrgChart.DepartmentNameTH = ss[30].TrimEnd();
                                    tmpOrgChart.SubDepartmentNameEN = ss[31].TrimEnd();
                                    tmpOrgChart.SubDepartmentNameTH = ss[32].TrimEnd();
                                    tmpOrgChart.SectionNameEN = ss[33].TrimEnd();
                                    tmpOrgChart.SectionNameTH = ss[34].TrimEnd();
                                    tmpOrgChart.Sub1SectionNameEN = ss[35].TrimEnd();
                                    tmpOrgChart.Sub1SectionNameTH = ss[36].TrimEnd();
                                    tmpOrgChart.ShiftNameEN = ss[37].TrimEnd();
                                    tmpOrgChart.ShiftNameTH = ss[38].TrimEnd();
                                    tmpOrgChart.Sub1ShiftNameEN = ss[39].TrimEnd();
                                    tmpOrgChart.Sub1ShiftNameTH = ss[40].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1BusinessUnit = ss[41].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1_1BusinessUnit = ss[42].TrimEnd();
                                    tmpOrgChart.OrgIDOfCompany = ss[43].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1Company = ss[44].TrimEnd();
                                    tmpOrgChart.OrgIDOfDivision = ss[45].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1Division = ss[46].TrimEnd();
                                    tmpOrgChart.OrgIDOfDepartment = ss[47].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1Department = ss[48].TrimEnd();
                                    tmpOrgChart.OrgIDOfSection = ss[49].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1Section = ss[50].TrimEnd();
                                    tmpOrgChart.OrgIDOfShift = ss[51].TrimEnd();
                                    tmpOrgChart.OrgIDOfSub1Shift = ss[52].TrimEnd();
                                    tmpOrgChart.StartDate = ss[53].TrimEnd();
                                    tmpOrgChart.DelimitDate = ss[54].TrimEnd();
                                    tmpOrgChart.SystemDate = ss[55].TrimEnd();
                                    try
                                    {
                                        Factory.TmpDbOrganizationChartService.AddTmpOrganizationChart(tmpOrgChart);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("DepartmentCode : " + tmpOrgChart.DepartmentCode);
                                        Console.WriteLine(ex.ToString());
                                        //continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid file format");
                                }
                            }
                        }
                        count++;
                        s = sr.ReadLine();

                    }
                }
            }
        }

        private string convertEncoding(string filename)
        {
            string OutputFileName = "tmp_" + filename;
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

    }
}
