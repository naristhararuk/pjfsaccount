﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.DTO;
using SS.SU.Helper;
using SS.SU.Query;
using SCG.eAccounting.Interface.ManualUserImport.DAL;
using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.BLL;
using SCG.eAccounting.BLL.Implement;
using SS.SU.DTO.ValueObject;
using SCG.eAccounting.BLL;
using SCG.eAccounting.Interface.Utilities;
using SS.Standard.Security;

namespace SCG.eAccounting.Interface.ManualUserImport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!String.IsNullOrEmpty(args[0]))
            {
                
                //IUserEngineService userEngine = (IUserEngineService)ObjectManager.GetObject("UserEngineService");
                //SuUser user = SS.SU.Query.QueryProvider.SuUserQuery.FindByIdentity(ParameterServices.ImportSystemUserID);
                //userEngine.SignIn(user.UserName);

                //IUserAccount userAccount = (IUserAccount)ObjectManager.GetObject("UserAccount");
                //userAccount.CurrentProgramCode = "UserImport";


                string fileName = args[0];
                FileStream fs = new FileStream(fileName, FileMode.Open);
                if (fs.Length > 0)
                {
                    fs.Close();
                Factory.CreateObject();
                int ById = ParameterServices.SystemUserID;
                int RoleId = ParameterServices.DefaultUserRoleID;
                Utility utility = new Utility();
                Console.WriteLine("Deleting temp database...");
                Factory.TmpSuUserService.DeleteAll();
                Console.WriteLine("Deleting log database...");
                Factory.SuEHrProfileLogService.DeleteAll();
                Console.WriteLine("Reading file...");
                try
                {
                    utility.ReadFile(fileName);
                }
                catch (Exception)
                {
                    Console.WriteLine("couldn't find " + fileName);
                    return;
                }
              
                Console.WriteLine("Preparing before import...");
                Factory.TmpSuUserService.BeforeCommit();
                Console.WriteLine("Importing...");
                Factory.TmpSuUserService.CommitManualImport(ById, RoleId);
                Console.WriteLine("Writing errors log...");
                Factory.TmpSuUserService.AfterCommit(ById);
                Console.WriteLine("Sending Email to new users...");
                IList<NewUserEmail> newUsers = Factory.TmpSuUserService.FindNewUser();

                ISCGEmailService emailService = (ISCGEmailService)ObjectManager.GetObject("SCGEmailService");

                foreach (NewUserEmail newUser in newUsers)
                {
                    try
                    {
                        emailService.SendEmailEM08(newUser.UserID, newUser.Password);
                    }
                    catch
                    {
                        Console.WriteLine("Can't send email");
                    }
                }
                Console.WriteLine("Finished...");
            }
            else
                {
                    fs.Close();
                    Console.WriteLine("Connot import user.File is emtry.");
                    Console.WriteLine("Finished...");
                }
            }
            else
                Console.WriteLine("Please select input file");

        }
        
    }
}
