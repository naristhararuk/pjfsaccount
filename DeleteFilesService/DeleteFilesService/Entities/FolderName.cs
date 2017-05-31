using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeleteFilesService
{
    public class FolderName
    {
        public string Name { get; set; }
        public string RootName { get; set; }
        public List<FilesExtension> FilesExtension { get; set; }
    }
}
