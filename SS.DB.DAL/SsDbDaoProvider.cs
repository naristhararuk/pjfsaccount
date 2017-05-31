using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DAL
{
    public class SsDbDaoProvider
    {
        public SsDbDaoProvider() { }
        
        public static IDbCurrencyLangDao DbCurrencyLangDao { get; set; }
        public static IDbCurrencyDao DbCurrencyDao { get; set; }
        public static IDbExchangeRateDao DbExchangeRateDao { get; set; }
        public static IDbLanguageDao DbLanguageDao { get; set; }
        public static IDbParameterDao DbParameterDao { get; set; }
        public static IDbStatusDao DbStatusDao { get; set; }
        public static IDbStatusLangDao DbStatusLangDao { get; set; }
        public static IDbParameterGroupDao DbParameterGroupDao { get; set; }
        public static IDbProvinceDao DbProvinceDao { get; set; }
        public static IDbProvinceLangDao DbProvinceLangDao { get; set; }
        public static IDbRegionDao DbRegionDao { get; set; }
	public static IDbZoneDao DbZoneDao { get; set; }
        public static IDbZoneLangDao DbZoneLangDao { get; set; }
    }
}

