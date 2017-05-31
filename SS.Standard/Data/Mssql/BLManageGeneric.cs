using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using SS.Standard.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Text;
using System.Collections.Generic;



namespace SS.Standard.Data.Mssql
{

	public abstract class BLManageGeneric
	{
		
		#region Local Vairable
        private static int pgmPathIndex = HttpContext.Current.Request.FilePath.LastIndexOf("/");
        private static string pgm = HttpContext.Current.Request.FilePath;
        public static string strProgramCode = pgm.Substring((pgmPathIndex + 1), (pgm.Length - (pgmPathIndex + 6)));
		#endregion
        public int bitConvert(bool bACT)
        {
            
            if (bACT)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public string HexConvert(byte[] bytes)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' }; 
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
                
            }
            string hexString = new string(chars);
            return "0x" + hexString;
        }
	}
}
