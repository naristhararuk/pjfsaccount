using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DAL
{
    public class ScgDbDaoProvider
    {
        public static IDbAccountCompanyDao DbAccountCompanyDao { get; set; }
        public static IDbPBLangDao DbPBLangDao { get; set; }
        public static IDbPBDao DbPBDao { get; set; }
        public static IDbIODao DbIODao { get; set; }
        public static IDbBankDao DbBankDao { get; set; }
        public static IDbBankLangDao DbBankLangDao { get; set; }
        public static IDbCountryDao DbCountryDao { get; set; }
        public static IDbCountryLangDao DbCountryLangDao { get; set; }
        public static IDbReasonDao DbReasonDao { get; set; }
        public static IDbReasonLangDao DbReasonLangDao { get; set; }
        public static IDbCompanyTaxDao DbCompanyTaxDao { get; set; }
        public static IDbTaxDao DbTaxDao { get; set; }
        public static IDbAccountDao DbAccountDao { get; set; }
        public static IDbAccountLangDao DbAccountLangDao { get; set; }
        public static IDbExpenseGroupDao DbExpenseGroupDao { get; set; }
        public static IDbExpenseGroupLangDao DbExpenseGroupLangDao { get; set; }
        public static IDbWithHoldingTaxTypeDao DbWithHoldingTaxTypeDao { get; set; }
        public static IDbCompanyDao DbCompanyDao { get; set; }
        public static IDbWithHoldingTaxDao DbWithHoldingTaxDao { get; set; }
        public static IDbVendorDao DbVendorDao { get; set; }
        public static IDbCompanyPaymentMethodDao DbCompanyPaymentMethodDao { get; set; }
        public static IDbLocationDao DbLocationDao { get; set; }
        public static IDbLocationLangDao DbLocationLangDao { get; set; }
        public static IDbPaymentMethodDao DbPaymentMethodDao { get; set; }
        public static IDbServiceTeamDao DbServiceTeamDao { get; set; }
        public static IDbServiceTeamLocationDao DbServiceTeamLocationDao { get; set; }
        public static IDbDocumentRunningDao DbDocumentRunningDao { get; set; }
        public static IDbVendorTempDao DbVendorTempDao { get; set; }
        public static IDbCostCenterDao DbCostCenterDao { get; set; }

        public static ITmpDbCostCenterDao TmpDbCostCenterDao { get; set; }
        public static IDbCostCenterImportLogDao DbCostCenterImportLogDao { get; set; }
        public static ITmpDbInternalOrderDao TmpDbInternalOrderDao { get; set; }
        public static IDbioImportLogDao DbioImportLogDao { get; set; }

        public static ITmpDbOrganizationChartDao TmpDbOrganizationChartDao { get; set; }
        public static IDbOrganizationChartDao DbOrganizationChartDao { get; set; }
        public static IRejectReasonDao RejectReasonDao { get; set; }
        public static IRejectReasonLangDao RejectReasonLangDao { get; set; }

        public static IDbBuyingRunningDao DbBuyingRunningDao { get; set; }
        public static IDbBuyingLetterDetailDao DbBuyingLetterDetailDao { get; set; }
        public static IDbBuyingLetterDao DbBuyingLetterDao { get; set; }

        public static IDbSellingRunningDao DbSellingRunningDao { get; set; }
        public static IDbSellingLetterDetailDao DbSellingLetterDetailDao { get; set; }
        public static IDbSellingLetterDao DbSellingLetterDao { get; set; }
        public static IDbPbRateDao DbPbRateDao { get; set; }

        public static IDbPBCurrencyDao DbPBCurrencyDao { get; set; }
        public static IDbSapInstanceDao DbSapInstanceDao { get; set; }

        public static IDbHolidayProfileDao DbHolidayProfileDao { get; set; }
        public static IDbHolidayDao DbHolidayDao { get; set; }
        public static IDbProfileListDao DbProfileListDao { get; set; }
        public static IDbMileageRateRevisionDao DbMileageRateRevisionDao { get; set; }
        public static IDbMileageRateRevisionDetailDao DbMileageRateRevisionDetailDao { get; set; }
        
    }
}
