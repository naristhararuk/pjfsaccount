using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SS.Standard.Messages
{
    public class ExceptionManager
    {
        public static string Manage(Exception ex)
        {
            if (ex is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException sqlex = ex as System.Data.SqlClient.SqlException;
                if (sqlex.Number == 542)
                {
                    return "Data in use";
                }

            }
            else if (ex is System.Data.Linq.ChangeConflictException)
            {
                return "Data Change";
            }

            //if(HttpContext.Current.Application
            return ex.Message;
        }

        public static void Loging()
        {

        }
        
    }
}
