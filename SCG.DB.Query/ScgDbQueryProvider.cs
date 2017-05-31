using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SCG.DB.Query;

namespace SCG.DB.Query
{
    public class ScgDbQueryProvider
    {
        public ScgDbQueryProvider() { }
        public static IDbAccountCompanyQuery DbAccountCompanyQuery { get; set; }
        public static IDbLocationLangQuery DbLocationLangQuery { get; set; }
        public static IDbBankQuery DbBankQuery { get; set; }
        public static IDbCountryLangQuery DbCountryLangQuery { get; set; }
        public static IDbCountryQuery DbCountryQuery { get; set; }
        public static IDbReasonLangQuery DbReasonLangQuery { get; set; }
        public static IDbReasonQuery DbReasonQuery { get; set; }
        public static IDbTaxQuery DbTaxQuery { get; set; }
        public static IDbAccountQuery DbAccountQuery { get; set; }
        public static IDbAccountLangQuery DbAccountLangQuery { get; set; }
        public static IDbExpenseGroupQuery DbExpenseGroupQuery { get; set; }
        public static IDbExpenseGroupLangQuery DbExpenseGroupLangQuery { get; set; }
        public static IDbVendorQuery DbVendorQuery { get; set; }
        public static IDbIOQuery DbIOQuery { get; set; }
        public static IDbCompanyQuery DbCompanyQuery { get; set; }
        public static IDbCompanyTaxQuery DbCompanyTaxQuery { get; set; }
        //public static ISuUserProfileQuery SuUserProfileQuery { get; set; }
        public static IDbLocationQuery DbLocationQuery { get; set; }
        public static IDbCostCenterQuery DbCostCenterQuery { get; set; }
        public static IDbWithHoldingTaxTypeQuery DbWithHoldingTaxTypeQuery { get; set; }
        public static IDbPaymentMethodQuery DbPaymentMethodQuery { get; set; }
        public static IDbWithHoldingTaxQuery DbWithHoldingTaxQuery { get; set; }
        public static IDbCompanyPaymentMethodQuery DbCompanyPaymentMethodQuery { get; set; }
        public static IDbStatusLangQuery SCGDbStatusLangQuery { get; set; }
        public static IDbServiceTeamQuery DbServiceTeamQuery { get; set; }
        public static IDbServiceTeamLocationQuery DbServiceTeamLocationQuery { get; set; }
        public static IDbPbLangQuery DbPbLangQuery { get; set; }
        //public static ISCGDbStatusLangQuery SCGDbStatusLangQuery { get; set; }
        public static IDbPBQuery DbPBQuery { get; set; }
        public static IDbPbRateQuery DbPbRateQuery { get; set; }
        public static IDbDocumentRunningQuery DbDocumentRunningQuery { get; set; }

        public static ITmpDbCostCenterQuery TmpCostCenterQuery { get; set; }
        public static IDbCostCenterImportLogQuery DbCostCenterImportLogQuery { get; set; }
        public static ITmpDbInternalOrderQuery TmpDbInternalOrderQuery { get; set; }
        public static IDbioImportLogQuery DbioImportLogQuery { get; set; }

        public static IDbOrganizationChartQuery DbOrganizationChartQuery { get; set; }
        public static IRejectReasonQuery RejectReasonQuery { get; set; }
        public static IRejectReasonLangQuery RejectReasonLangQuery { get; set; }

        public static IDbRejectReasonQuery DbRejectReasonQuery { get; set; }
        public static IDbRejectReasonLangQuery DbRejectReasonLangQuery { get; set; }
        public static IDbDictionaryQuery DbDictionaryQuery { get; set; }

        public static IDbMoneyRequestQuery DbMoneyRequestQuery { get; set; }
        public static IDbBuyingRunningQuery DbBuyingRunningQuery { get; set; }
        public static IDbBuyingLetterDetailQuery DbBuyingLetterDetailQuery { get; set; }
        public static IDbBuyingLetterQuery DbBuyingLetterQuery { get; set; }

        public static IDbSellingRunningQuery DbSellingRunningQuery { get; set; }
        public static IDbSellingLetterDetailQuery DbSellingLetterDetailQuery { get; set; }
        public static IDbSellingLetterQuery DbSellingLetterQuery { get; set; }

        public static IDbPBCurrencyQuery DbPBCurrencyQuery { get; set; }
        public static IDbSapInstanceQuery DbSapInstanceQuery { get; set; }
        public static IDbBuQuery DbBuQuery { get; set; }

        public static IDbHolidayProfileQuery DbHolidayProfileQuery { get; set; }
        public static IDbHolidayQuery DbHolidayQuery { get; set; }
        public static IDbProfileListQuery DbProfileListQuery { get; set; }
        public static IDbMileageRateRevisionQuery DbMileageRateRevisionQuery { get; set; }
        public static IDbMileageRateRevisionDetailQuery DbMileageRateRevisionDetailQuery { get; set; }
    }
}
