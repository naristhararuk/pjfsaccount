using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SCG.eAccounting.Interface.Utilities
{
    public class FileManager
    {
        /// <summary>
        /// use this method for copy file via absolute path.
        /// </summary>
        /// <param name="pathFileIn">Accept string both of Share folder or Local machine folder.</param>
        /// <param name="pathFileOut">Accept string both of Share folder or Local machine folder.</param>
        public static void CopyFile(string pathFileIn, string pathFileOut)
        {
            FileStream fileIn;
            FileStream fileOut;
            try
            {
                fileIn = new FileStream(pathFileIn, FileMode.Open);
                fileOut = new FileStream(pathFileOut, FileMode.Create);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File input not found.");
                return;
            }
            int i;

            // Copy File byte to byte style.
            try
            {
                do
                {
                    i = fileIn.ReadByte();
                    if (i != -1) fileOut.WriteByte((byte)i);
                } while (i != -1);
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message + "File Error");
            }

            fileIn.Close();
            fileOut.Close();
        }

        public static void DeleteFile(string pathFileName)
        {
            File.Delete(pathFileName);
            
        }
    }
}
