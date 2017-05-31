using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;

using Spring.Globalization.Resolvers;

namespace SCG.eAccounting.Web.Helper
{
    public class Constant
    {
        public static string DateFormat;

        public string dateFormat
        {
            set 
			{
				Constant.DateFormat = value; 
			}
        }

        public static string DateTimeFormat;

        public string dateTimeFormat
        {
            set
            {
                Constant.DateTimeFormat = value;
            }
        }

        public static string CalendarDateFormat;

        public string calendarDateFormat
        {
            set
            {
                Constant.CalendarDateFormat = value;
            }
        }

       
    }
    
    public class UIHelper
    {
        public static readonly CultureInfo THAI_CULTURE_INFO = new CultureInfo("th-TH");
        public static readonly CultureInfo ENGLISH_CULTURE_INFO = new CultureInfo("en-US");

        public enum ParseCultureInfo
        {
            ThaiCulturInfo = 1,
            EnglishCulturInfo = 2
        }

        private UIHelper()
        {
        }

        public static SessionCultureResolver UserCulture { get; set; }
        
        public static DateTime? ParseDate(string date, string format)
        {
            DateTime? result = null;
            if (!string.IsNullOrEmpty(date))
            {
                result = DateTime.ParseExact(date, format, UserCulture.DefaultCulture);
            }

            return result;
        }

        [Obsolete]
        public static DateTime? ParseDate(string date, CultureInfo cultureInfo)
        {
			string format = Constant.DateFormat;
            DateTime? result = null;
            
            if (!string.IsNullOrEmpty(date))
            {
				//result = DateTime.Parse(DateTime.Parse(date).ToString(SCG.eAccounting.Web.Helper.Constant.DateFormat), cultureInfo.Name.Equals("th-TH")?THAI_CULTURE_INFO:ENGLISH_CULTURE_INFO);
				//result = DateTime.Parse(DateTime.Parse(date).ToString(SCG.eAccounting.Web.Helper.Constant.DateFormat), DateTimeFormatInfo.InvariantInfo);
				result = DateTime.ParseExact(date, format, cultureInfo);
            }

            return result;
        }
        public static DateTime? ParseDate(string date)
        {
            return ParseDate(date, Constant.DateFormat);
        }
        public static string ToDateString(DateTime? date, string format)
        {
            string result = null;

            if (date.HasValue)
            {
                result = ((DateTime)date.Value).ToString(format, UserCulture.DefaultCulture);
            }

            return result;
        }

        public static string ToDateString(DateTime? date)
        {
            return ToDateString(date, Constant.DateFormat);
        }

        public static string ToDateTimeString(DateTime? date, string format)
        {
            string result = null;

            if (date.HasValue)
            {
                result = ((DateTime)date.Value).ToString(format, UserCulture.DefaultCulture);
            }

            return result;
        }

        public static string ToDateTimeString(DateTime? date)
        {
            return ToDateTimeString(date, Constant.DateTimeFormat);
        }

        public static int ParseInt(string integer)
        {
            int returnInt;
            int.TryParse(integer, out returnInt);
            return returnInt;
        }
        public static decimal ParseDecimal(string strDecimal)
        {
            //change int type to Decimal Type by Thum 19-03-2009
            decimal returnDecimal;
            decimal.TryParse(strDecimal, out returnDecimal);
            return returnDecimal;
        }
		public static double ParseDouble(string strDouble)
		{
            //change int type to Double Type by oum 17-02-2009
            //int returnInt;
            //int.TryParse(integer, out returnInt);
            double returnDouble;
			double.TryParse(strDouble, out returnDouble);
			return returnDouble;
		}
        public static short ParseShort(string int16)
        {
            short returnShort;
            short.TryParse(int16, out returnShort);
            return returnShort;
        }
        public static long ParseLong(string strLong)
        {
            long returnLong;
            long.TryParse(strLong, out returnLong);
            return returnLong;
        }
        public static System.IO.MemoryStream Serialization<T>(T Obj) where T : class
        {
            System.Runtime.Serialization.DataContractSerializer ds = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            ds.WriteObject(stream, Obj);
            stream.Position = 0;
            return stream;
        }

        public static T DeSerialization<T>(object obj) where T : class
        {
            System.Runtime.Serialization.DataContractSerializer ds = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            T result = ds.ReadObject(obj as System.IO.MemoryStream) as T;
            return result;
        }


        public static string BindDate(object dateValue)
        {
            if (dateValue == null)
                return null;
            else
            {
                DateTime date = DateTime.MinValue;
                bool b = DateTime.TryParse(dateValue.ToString(), out date);;
                if (date.Equals(DateTime.MinValue))
                {
                    return null;
                }
                return ToDateString(date);
            }
        }

        public static string BindDateTime(object dateTimeValue)
        {
            if (dateTimeValue == null)
                return null;
            else
            {
                DateTime date = DateTime.MinValue;
                bool b = DateTime.TryParse(dateTimeValue.ToString(), out date); ;
                if (date.Equals(DateTime.MinValue))
                {
                    return null;
                }
                return ToDateTimeString(date);
            }
        }

		public static string BindDecimal(string strNumber)
		{
			decimal tempNo;
			decimal.TryParse(strNumber, out tempNo);
			return String.Format("{0:#,##0.00}", tempNo);
		}

		public static string BindExchangeRate(string strNumber)
		{
			decimal tempNo;
			decimal.TryParse(strNumber, out tempNo);
			return String.Format("{0:#,##0.00000}", tempNo);
		}

        public static string BindInvoiceNo(object invoiceNo)
        {
            string result;

            if (invoiceNo == null)
            {
                result = "N/A";
            }                
            else if (invoiceNo == string.Empty)
            {
                result = "N/A";
            }
            else
            {
                result = invoiceNo.ToString();
            }
            
            return result;
        }

        internal static DateTime ParseDate(SCG.eAccounting.Web.UserControls.Calendar ctlDate)
        {
            throw new NotImplementedException();
        }

        public static string BindDecimalNumberAccountFormat(string strNumber)
        {
            decimal tempNo;
            decimal.TryParse(strNumber, out tempNo);
            return String.Format("{0:#,##0.00;(#,##0.00);}", tempNo);
        }
    }
}
