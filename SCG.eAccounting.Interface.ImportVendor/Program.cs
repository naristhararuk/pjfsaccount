using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.Interface.ImportVendor.DAL;
using System.IO;
using SCG.DB.BLL;
using SCG.DB.DTO;

namespace SCG.eAccounting.Interface.ImportVendor
{
    class Program
    {
        public static string ImportVendorPath
        {
            get;
            set;
            //get
            //{
            //    return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "2");
            //}
        }
        public static string ImportVendorFile
        {
            get
            {
                //return Factory.DbParameterQuery.getParameterByGroupNo_SeqNo("13", "11");
                return "tne_vendormaster_dom";
            }
        }

        static void Main(string[] args)
        {
            Factory.CreateObject();
            
            ImportVendorPath = args[0];
            string filePath = ImportVendorPath+ ImportVendorFile+".txt";
            StreamReader reader;
            Console.WriteLine("Opening file Vendor data file.");
            try
            {
                Encoding inputEnc = Encoding.GetEncoding("windows-874");
                reader = new StreamReader(filePath,inputEnc);
            }
            catch (Exception)
            {
                Console.WriteLine("!!Error!! : Could not find Vendor-Import file path." + filePath);
                Console.WriteLine("Program was terminated.");
                return;
            }
            Console.WriteLine("Getting DbVendorService Object.");
            IDbVendorTempService vendorTemp = Factory.DbVendorTempService;
            Console.WriteLine("Clearing DbVendortemp data...");
            vendorTemp.DeleteAll();
            string textLine;

            //Read file with connectionful/connectionoriented methodlogy.
            Console.WriteLine("Importing vendor Data from file to temporary.");
            bool firstLine = true;
            Int64 count = 0;
            while ((textLine = reader.ReadLine()) != null)
            {
                count++;
                if (!firstLine)
                {
                    string[] textFeild = textLine.Split('|');
                    //Add one by one row to database.
                    try
                    {

                    #region Map data to DTO
                    DbVendorTemp vendor = new DbVendorTemp();
                    vendor.Active = true;
                    vendor.BlockDelete = false;
                    vendor.BlockPost = false;
                    vendor.City = textFeild[12].Trim();
                    vendor.Country = textFeild[14].Trim();
                    vendor.CreBy = 1;
                    vendor.CreDate = DateTime.Now.Date;
                    vendor.PostalCode = textFeild[13].Trim();
                    vendor.Street = textFeild[6].Trim();
                    vendor.TaxNo1 = textFeild[28].Trim();
                    vendor.TaxNo2 = textFeild[29].Trim();
                    vendor.UpdBy = 1;
                    vendor.UpdDate = DateTime.Now.Date;
                    vendor.UpdPgm = "Interface";
                    vendor.VendorCode = textFeild[0].Trim();
                    vendor.VendorName1 = textFeild[1].Trim();
                    vendor.VendorName2 = textFeild[3].Trim();
                    vendor.VendorTitle = textFeild[2].Trim();
                    vendor.TaxNo3 = textFeild[30].Trim();
                    vendor.TaxNo4 = textFeild[31].Trim();
                    #endregion

                    //to do Add DTO to database.
                    
                        //save each row to temp table.
                    if (vendor.TaxNo3.Length == 13)
                        vendorTemp.Save(vendor);
                    }
                    catch (Exception ex)
                    {
                        //don;t display text for codeoptimiZation.
                        Console.WriteLine("!!Error!! while fetching data line " + count.ToString() +" : " + ex.ToString());
                        continue;
                    }
                }
                firstLine = false;
            }
            try
            {
                vendorTemp.CommitTempToVendor();
            }
            catch (Exception ex)
            {
                //don't display text for codeoptimiZation.
                //Console.WriteLine("!!Error!! while importing : " + ex.StackTrace);
                
            }

            Console.WriteLine("Finished import");
        }
    }
}
