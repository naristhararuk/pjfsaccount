using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SCG.DB.DTO;
using SCG.DB.BLL;
using SCG.DB.DAL;
using SS.Standard.Utilities;
using SS.Standard.Security;

namespace SCG.DB.BLL.Implement
{
    public partial class RejectReasonLangService : ServiceBase<DbRejectReasonLang, long>, IRejectReasonLangService
    {
        public IUserAccount UserAccount { get; set; }
        public override IDao<DbRejectReasonLang, long> GetBaseDao()
        {
            return ScgDbDaoProvider.RejectReasonLangDao;
        }

        public void UpdateRejectReasonLang(IList<DbRejectReasonLang> reasonLangList,int reasonID)
        {
            if (!reasonID.Equals(0))
            {
                ScgDbDaoProvider.RejectReasonLangDao.DeleteRejectReasonLangByReasonId(reasonID);
            }
            foreach (DbRejectReasonLang rl in reasonLangList)
            {
                this.BuildDbRejectReasonLang(rl);
                ScgDbDaoProvider.RejectReasonLangDao.Save(rl);
            }
        }
        
        private void BuildDbRejectReasonLang(DbRejectReasonLang reasonlang)
        {
            reasonlang.CreBy = UserAccount.UserID;
            reasonlang.CreDate = DateTime.Now;
            reasonlang.UpdBy = UserAccount.UserID;
            reasonlang.UpdDate = DateTime.Now;
            reasonlang.UpdPgm = UserAccount.CurrentProgramCode;
        }
    }
}
