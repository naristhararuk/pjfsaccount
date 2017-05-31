using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.Query
{
    public class SsDbQueryProvider
    {
         public SsDbQueryProvider() { }
         public static IDbCurrencyLangQuery DbCurrencyLangQuery { get; set; }
         public static IDbParameterQuery DbParameterQuery { get; set; }
         public static IDbCurrencyQuery DbCurrencyQuery { get; set; }
         public static IDbExchangeRateQuery DbExchangeRateQuery { get; set; }
         public static IDbLanguageQuery DbLanguageQuery { get; set; }
         public static IDbStatusQuery DbStatusQuery { get; set; }
         public static IDbStatusLangQuery DbStatusLangQuery { get; set; }
         public static IDbParameterGroupQuery DbParameterGroupQuery { get; set; }
         public static IDbProvinceQuery DbProvinceQuery { get; set; }
         public static IDbProvinceLangQuery DbProvinceLangQuery { get; set; }
         public static IDbRegionQuery DbRegionQuery { get; set; }
         public static IDbRegionLangQuery DbRegionLangQuery { get; set; }
	 public static IDbZoneQuery DbZoneQuery { get; set; }
         public static IDbZoneLangQuery DbZoneLangQuery { get; set; }
    }
}


