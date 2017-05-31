using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.SU.Query;

namespace SS.SU.Query
{
    public class QueryProvider
    {
        public QueryProvider() { }
       // public static ParameterServices ParameterServices { get; set; }
        public static ISuUserQuery SuUserQuery { get; set; }
		//public static ISuUserLangQuery SuUserLangQuery { get; set; }
		public static ISuPasswordHistoryQuery SuPasswordHistoryQuery { get; set; }
        public static ISuOrganizationQuery SuOrganizationQuery { get; set; }
        //public static ISuLanguageQuery SuLanguageQuery { get; set; }
        public static ISuRoleQuery SuRoleQuery { get; set; }
		public static ISuRoleLangQuery SuRoleLangQuery { get; set; }
        public static ISuGlobalTranslateQuery SuGlobalTranslateQuery { get; set; }
        public static ISuGlobalTranslateLangQuery SuGlobalTranslateLangQuery { get; set; }
        public static ISuDivisionQuery SuDivisionQuery { get; set; }
        public static ISuOrganizationLangQuery SuOrganizationLangQuery { get; set; }
        public static ISuUserRoleQuery SuUserRoleQuery { get; set; }
        public static ISuProgramQuery SuProgramQuery { get; set; }
        //public static IDbParameterQuery DbParameterQuery { get; set; }
        public static ISuMenuQuery SuMenuQuery { get; set; }
        public static ISuSessionQuery SuSessionQuery { get; set; }
        public static ISuProgramRoleQuery SuProgramRoleQuery { get; set; }
		public static ISuAnnouncementGroupQuery SuAnnouncementGroupQuery { get; set; }
		public static ISuAnnouncementGroupLangQuery SuAnnouncementGroupLangQuery { get; set; }
		public static ISuAnnouncementQuery SuAnnouncementQuery { get; set; }
		public static ISuAnnouncementLangQuery SuAnnouncementLangQuery { get; set; }
        public static ISuDivisionLangQuery SuDivisionLangQuery { get; set; }
        public static ISuUserLogQuery SuUserLogQuery { get; set; }
        //public static IDbCurrencyQuery DbCurrencyQuery { get; set; }
        //public static IDbExchangeRateQuery DbExchangeRateQuery { get; set; }
        public static ISuRTENodeQuery SuRTENodeQuery { get; set; }
        public static ISuRTEContentQuery SuRTEContentQuery { get; set; }

        public static ISuUserFavoriteActorQuery SuUserFavoriteActorQuery { get; set; }

        public static ISuRolePBQuery SuRolePBQuery { get; set; }
        public static ISuRoleServiceQuery SuRoleServiceQuery { get; set; }

        public static ISuPostSAPLogQuery SuPostSAPLogQuery { get; set; }
        public static ISuImageToSAPLogQuery SuImageToSAPLogQuery { get; set; }
        public static ISuSmsLogQuery SuSmsLogQuery { get; set; }
        public static ISuEHRExpenseLogQuery SuEHRExpenseLogQuery { get; set; }
        public static ISuEHRProfileLogQuery SuEHRProfileLogQuery { get; set; }
        public static ISuEmailResendingQuery SuEmailResendingQuery { get; set; }
        public static ISuUserPersonalLevelQuery SuUserPersonalLevelQuery { get; set; }
    }
}
