using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace DeleteFilesService
{
    public class FilesManager
    {
        [DllImport("advapi32.DLL", SetLastError = true)]
        public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        public static void Delete()
        {
            try
            {
                FolderRootPathNames root = Settings.getFolderRootPathName;
                Folders targetFolder = Settings.getFolders;
                Files targetFile = Settings.getFiles;
                Authentication targetAuthentication = Settings.getAuthentication;
                OverDate targetDays = Settings.getOverDate;

                IntPtr admin_token;
                WindowsIdentity wid_admin = null;
                WindowsImpersonationContext wic = null;

                LogonUser(targetAuthentication.UserName, targetAuthentication.ServerName, targetAuthentication.Password, 9, 0, out
                    admin_token);
                wid_admin = new WindowsIdentity(admin_token);
                wic = wid_admin.Impersonate();

                if (root != null && root.FolderRootName != null && root.FolderRootName.Count > 0)
                {
                    string rootPath = string.Empty;
                    string[] allFiles = default(string[]);

                    foreach (FolderName itemFolder in targetFolder.FolderName)
                    {
                        foreach (FolderRootName itemRoot in root.FolderRootName)
                        {
                            if (itemRoot.Name == itemFolder.RootName)
                            {
                                rootPath = itemRoot.PathName + "\\" + itemFolder.Name;
                                foreach (FilesExtension itemFilesExtension in itemFolder.FilesExtension)
                                {
                                    allFiles = Directory.GetFiles(rootPath, string.Concat("*.", itemFilesExtension.Name));

                                    foreach (string itemFiles in allFiles)
                                    {
                                        doDeleteFile(itemFiles, targetDays.Days);
                                    }
                                }
                            }
                        }

                    }

                    foreach (FolderFilesName itemFolderFilesName in targetFile.FolderFilesName)
                    {
                        foreach (FolderRootName itemRoot in root.FolderRootName)
                        {
                            if (itemRoot.Name == itemFolderFilesName.RootName)
                            {
                                rootPath = itemRoot.PathName;
                                int xx = itemFolderFilesName.FileName.Count;
                                foreach (FileName item in itemFolderFilesName.FileName)
                                {
                                    doDeleteFile(rootPath + "\\" + item.Name, targetDays.Days);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }
          
         

        }
        private static bool CheckOldDay(DateTime expiryDate,int countDays)
        {
            if (DateTime.Compare(expiryDate, DateTime.Now.AddDays(-countDays)) < 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private static void doDeleteFile(string itemFiles,int countDays)
        {
            if (File.Exists(itemFiles))
            {
                FileInfo xFile = new FileInfo(itemFiles);
                if (CheckOldDay(xFile.LastWriteTime, countDays))
                {
                    xFile.Delete();
                    Log.ShowMessage(itemFiles + " was Deleted");
                }
            }
        }

        private string[] getFiles(string FolderPathName,string FilesFormat)
        {
            string[] result = default(string[]);
            try
            {
                result = Directory.GetFiles(FolderPathName, "*." + FilesFormat);
            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }
            return result;
        }

        private string[] getFolders(string PathName)
        {
            string[] result = default(string[]);
            try
            {
                result = Directory.GetDirectories(PathName);

            }
            catch (Exception ex)
            {

                Log.WriteLogs(ex);
            }

            return result;
        }
    }
}
