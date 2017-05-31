using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
namespace DeleteFilesService
{
    public sealed class Settings
    {
        private static readonly string _CurrentDirectory;
        private static readonly Assembly _Assembly;
        private static string _SettingsFilePath;
        private static readonly Regex GlobalsRegex = new Regex(@"(?<g>\${(?<k>[^}]+)})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static FolderRootPathNames getFolderRootPathName { get; set; }
        public static Folders getFolders { get; set; }
        public static Files getFiles { get; set; }
        public static Authentication getAuthentication { get; set; }
        public static OverDate getOverDate { get; set; }
        /// <summary>
        /// Initializes the <see cref="Settings"/> class.
        /// </summary>
        static Settings()
        {
            _CurrentDirectory = Environment.CurrentDirectory;
            _Assembly = Assembly.GetEntryAssembly();
            if (_Assembly == null)
            {
                _Assembly = Assembly.GetCallingAssembly();
            }
            _SettingsFilePath = string.Format("{0}{1}.config", CurrentDirectory, _Assembly.GetName().Name);
        }
        /// <summary>
        /// Gets the current diretory.
        /// </summary>
        /// <value>The current diretory.</value>
        /// 
        /// <summary>
        /// Inits this instance.
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            try
            {
                XmlDocument d = LoadSettings();
                ReadSettings(d);
                return true;
            }
            catch (Exception e)
            {
                Log.WriteLogs(e);
                return false;
            }
        }
        public static string CurrentDirectory
        {
            get
            {
                return _CurrentDirectory.EndsWith("\\") ? _CurrentDirectory : string.Format("{0}\\", _CurrentDirectory);
            }
        }
   
        private static XmlSchema LoadSettingsSchema()
        {
            try
            {
                string xmlResouce = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".DeleteFilesServiceConfigs.xsd";
                using (Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(xmlResouce))
                {
                    return XmlSchema.Read(s, null);
                }

            }
            catch (Exception ex)
            {
                Log.WriteLogs(ex);
            }
            return null;

        }
        private static XmlDocument LoadSettings()
        {
            XmlSchemaSet xmlSchemas = new XmlSchemaSet { };
            xmlSchemas.Add(LoadSettingsSchema());

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
            {
                CloseInput = true,
                ConformanceLevel = ConformanceLevel.Document,
                ValidationType = ValidationType.Schema,
                ValidationFlags =
                    XmlSchemaValidationFlags.ReportValidationWarnings
                    | XmlSchemaValidationFlags.ProcessIdentityConstraints,
                Schemas = xmlSchemas
            };

            int errorCount = 0;
            xmlReaderSettings.ValidationEventHandler += (sender, e) =>
            {
                if (e.Severity == XmlSeverityType.Error) errorCount++;
            };

            using (FileStream fileStream = File.OpenRead(SettingsFilePath))
            using (XmlReader xmlReader = XmlReader.Create(fileStream, xmlReaderSettings))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);

                if (errorCount > 0)
                {
                    throw new XmlException(string.Format(
                        "{0} XML schema validation errors found in DeleteFileService settings.",
                        errorCount));
                }

                return xmlDocument;
            }
        }

        private static void ReadSettings(XmlDocument d)
        {
            #region FolderRootPathName
            // FolderRootPathName
            XmlNodeList listFolderRootPathName = d.SelectNodes("//Settings/FolderRootPathNames/FolderRootName");
             int k = 0;
             if (listFolderRootPathName != null)
            {
                if (getFolderRootPathName == null)
                 {
                getFolderRootPathName = new FolderRootPathNames();
                getFolderRootPathName.FolderRootName = new List<FolderRootName>();
                 }
                foreach (XmlNode node in listFolderRootPathName)
                {
                    FolderRootName rootName = new FolderRootName();

                    XmlNode n1 = node.Attributes["Name"];
                    if (n1 != null)
                    {
                        if (n1 != null)
                        {
                            rootName.Name = n1.Value;
                        }
                        XmlNodeList list2 = node.SelectNodes("PathName");
                        if (list2 != null)
                        {
                            rootName.PathName = list2[0].InnerXml;
                        }

                        if (getFolderRootPathName.FolderRootName != null && !getFolderRootPathName.FolderRootName.Contains(rootName))
                        {
                            getFolderRootPathName.FolderRootName.Add(rootName);
                        }
                    }
                     k++;
                }
            }
            #endregion 

             #region Folders
             // Folders
             XmlNodeList listFolders = d.SelectNodes("//Settings/Folders/FolderName");
             int j = 0;
             if (listFolders != null)
             {
                 if(getFolders==null)
                 {
                     getFolders = new Folders();
                     getFolders.FolderName = new List<FolderName>();
                    
                 }
                 foreach (XmlNode node in listFolders)
                 {
                     FolderName folderName = new FolderName();
                     if (folderName.FilesExtension == null)
                     {
                         folderName.FilesExtension = new List<FilesExtension>();
                     }
                     XmlNode n1 = node.Attributes["Name"];
                     XmlNode n2 = node.Attributes["RootName"];
                     if (n1 != null)
                     {
                         folderName.Name = n1.Value;
                     }
                     if (n2 != null)
                     {
                         folderName.RootName = n2.Value;
                     }
                     XmlNodeList listFilesExtension = node.SelectNodes("Extension");
                     if (listFilesExtension != null)
                     {
                         int ik = 0;
                         foreach (XmlNode nodeExtension in listFilesExtension)
                         {
                             folderName.FilesExtension.Add(new FilesExtension(){ Name=nodeExtension.InnerXml});
                             ik++;
                         }
                     }

                     getFolders.FolderName.Add(folderName);
                     j++;
                 }
             }
             #endregion

             #region Files
             // Files
             XmlNodeList listFiles = d.SelectNodes("//Settings/Files/FolderFilesName");
             int jk = 0;
             if (listFolders != null)
             {
                 if (getFiles == null)
                 {
                     getFiles = new Files();
                     getFiles.FolderFilesName = new List<FolderFilesName>();

                 }
                 foreach (XmlNode node in listFiles)
                 {
                     FolderFilesName folderFilesName = new FolderFilesName();
                     if (folderFilesName.FileName == null)
                     {
                         folderFilesName.FileName = new List<FileName>();
                     }
                     XmlNode n1 = node.Attributes["Name"];
                     XmlNode n2 = node.Attributes["RootName"];
                     if (n1 != null)
                     {
                         folderFilesName.Name = n1.Value;
                     }
                     if (n2 != null)
                     {
                         folderFilesName.RootName = n2.Value;
                     }
                     XmlNodeList listFilesName = node.SelectNodes("FileName");
                     if (listFilesName != null)
                     {
                         int ik = 0;
                         foreach (XmlNode nodeFileName in listFilesName)
                         {
                             folderFilesName.FileName.Add(new FileName() { Name = nodeFileName.InnerXml });
                             ik++;
                         }
                     }

                     getFiles.FolderFilesName.Add(folderFilesName);
                     jk++;
                 }
             }
             #endregion

             #region Authentication
             // Authentication
             XmlNodeList listServerName = d.SelectNodes("//Settings/Authentication/ServerName");
             XmlNodeList listUserName = d.SelectNodes("//Settings/Authentication/UserName");
             XmlNodeList listPassword = d.SelectNodes("//Settings/Authentication/Password");
             if (getAuthentication == null)
             {
                 getAuthentication = new Authentication();
             }
            if(listServerName != null)
            {
             getAuthentication.ServerName = ((XmlNode)listServerName[0]).InnerXml;
            }
            if (listUserName != null)
            {
                getAuthentication.UserName = ((XmlNode)listUserName[0]).InnerXml;
            }
            if (listPassword != null)
            {
                getAuthentication.Password = ((XmlNode)listPassword[0]).InnerXml;
            }
             
             #endregion

            #region OverDate
            // OverDate
            XmlNodeList listOverDate = d.SelectNodes("//Settings/OverDate/Days");
            if (getOverDate == null)
            {
                getOverDate = new OverDate();
            }
            if (listOverDate != null)
            {
                getOverDate.Days = int.Parse(((XmlNode)listOverDate[0]).InnerXml);
            }

            #endregion
        }
        /// <summary>
        /// Gets or sets the DeleteFileService settings file path.
        /// </summary>
        /// <value>The config file path.</value>
        public static string SettingsFilePath
        {
            get
            {
                return _SettingsFilePath;
            }
            set
            {
                _SettingsFilePath = string.Format("{0}{1}", CurrentDirectory, value);
            }
        }
    }
}
