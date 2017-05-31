using System;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using SS.Standard.Security;
using SS.Standard.Data.Oracle;
using SS.Standard.Data.Interfaces;

namespace SS.Standard.Base.Oracle
{
    public class baseOracleDAL : OracleHelper ,IDBManager
    {
        public static string Now
        {
            get
            {
                DateTime dt = DateTime.Now;
                DateTimeFormatInfo df = DateTimeFormatInfo.InvariantInfo;
                return dt.ToString("g", df);
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

        public string PgmName
        {
            get { return HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf("/") + 1).Split('.')[0]; }
        }

        public Int64 Timestamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now - origin;
            return (Int64)Math.Floor(diff.TotalSeconds);
        }

        public int UserID
        {
            get { return UserAccount.UserID; }
        }

        public string UserName
        {
            get { return UserAccount.UserName; }
        }


        #region IDBManager Members

        public void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

        }
       
        public void CloseConnection()
        {
            if(Connection.State == ConnectionState.Open)
                Connection.Close();
        }
        
        
        
        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                Transaction = Connection.BeginTransaction();
            }
            else
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
           
                Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            Transaction.Rollback();
        }

        #endregion


      
    }
}
