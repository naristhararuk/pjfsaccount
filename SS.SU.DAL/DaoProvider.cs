using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DAL
{
    public class DaoProvider
    {
        public DaoProvider() { }
        public static ISuUserDao SuUserDao { get; set; }
        public static IUserEngineDao UserEngineDao { get; set; }
        public static ISuRoleDao SuRoleDao { get; set; }
        public static ISuRoleLangDao SuRoleLangDao { get; set; }
        public static ISuProgramRoleDao SuProgramRoleDao { get; set; }
        public static ISuUserRoleDao SuUserRoleDao { get; set; }
        public static ISuUserLangDao SuUserLangDao { get; set; }
        //public static ISuLanguageDao SuLanguageDao { get; set; }
        public static ISuGlobalTranslateDao SuGlobalTranslateDao { get; set; }
        public static ISuSessionDao SuSessionDao { get; set; }
        public static IMenuEngineDao MenuEngineDao { get; set; }
        public static ISuGlobalTranslateLangDao SuGlobalTranslateLangDao { get; set; }
        public static ISuProgramDao SuProgramDao { get; set; }
        public static ISuProgramLangDao SuProgramLangDao { get; set; }
        public static ISuAnnouncementGroupDao SuAnnouncementGroupDao { get; set; }
        public static ISuAnnouncementGroupLangDao SuAnnouncementGroupLangDao { get; set; }
        public static ISuAnnouncementDao SuAnnouncementDao { get; set; }
        public static ISuAnnouncementLangDao SuAnnouncementLangDao { get; set; }
        public static ISuOrganizationDao SuOrganizationDao { get; set; }
        public static ISuOrganizationLangDao SuOrganizationLangDao { get; set; }
        public static ISuDivisionDao SuDivisionDao { get; set; }
        public static ISuDivisionLangDao SuDivisionLangDao { get; set; }
        public static ISuMenuDao SuMenuDao { get; set; }
        public static ISuMenuLangDao SuMenuLangDao { get; set; }
        //public static IDbCurrencyDao DbCurrencyDao { get; set; }
        //public static IDbExchangeRateDao DbExchangeRateDao { get; set; }
        public static ISuRTENodeDao SuRTENodeDao { get; set; }
        public static ISuPasswordHistoryDao SuPasswordHistoryDao { get; set; }
        public static ISuRTEContentDao SuRTEContentDao { get; set; }
        public static ISuUserLogDao SuUserLogDao { get; set; }

        public static ISuUserFavoriteActorDao SuUserFavoriteActorDao { get; set; }

        public static ISuRolepbDao SuRolepbDao { get; set; }
        public static ISuRoleServiceDao SuRoleServiceDao { get; set; }
        public static ISuSmsLogDao SuSmsLogDao { get; set; }
        public static ITmpSuUserDao TmpSuUserDao { get; set; }
        public static ISuImageToSAPLogDao SuImageToSAPLogDao { get; set; }

        public static ISuEHrProfileLogDao SuEHRProfileLogDao { get; set; }
        public static ISuEHRExpenseLogDao SuEHRExpenseLogDao { get; set; }

        public static ISuStatisticDao SuStatisticDao { get; set; }

        public static ISuEmailResendingDao SuEmailResendingDao { get; set; }

        public static ISuUserLoginTokenDao SuUserLoginTokenDao { get; set; }
        public static ISuUserPersonalLevelDao SuUserPersonalLevelDao { get; set; }
    }
}

