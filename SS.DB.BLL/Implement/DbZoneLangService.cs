using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.DB.DTO;
using SS.DB.DAL;
using SS.SU.BLL;
using SS.Standard.Utilities;
using SS.DB.DTO.ValueObject;

namespace SS.DB.BLL.Implement
{
    public partial class DbZoneLangService : ServiceBase<DbZoneLang, long>, IDbZoneLangService
    {
        public override IDao<DbZoneLang, long> GetBaseDao()
        {
            return SsDbDaoProvider.DbZoneLangDao;
        }
        public IList<DbZoneResult> FindZoneLangByZoneID(short zoneId)
        {
            return SsDbDaoProvider.DbZoneLangDao.FindZoneLangByZoneId(zoneId);
        }
        public void UpdateZoneLang(IList<DbZoneLang> zoneLangList)
        {
            if (zoneLangList.Count > 0)
            {
                SsDbDaoProvider.DbZoneLangDao.DeleteZoneLangByID(zoneLangList[0].Zone.Zoneid);
            }
            foreach (DbZoneLang zoneLang in zoneLangList)
            {
                SsDbDaoProvider.DbZoneLangDao.Save(zoneLang);
            }

        }

        public long AddZoneLang(DbZone zone,DbZoneLang zoneLang)
        {

            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(zoneLang.ZoneName))
            {
                errors.AddError("Zone.Error", new Spring.Validation.ErrorMessage("RequiredZoneName"));
            }
            //else
            //{
            //    IList<DbStatus> statusList = SsDbDaoProvider.DbStatusDao.FindByStatusCriteria(status);
            //    if (statusList.Count != 0)
            //    {
            //        errors.AddError("Status.Error", new Spring.Validation.ErrorMessage("LanguageNameIsAlready"));
            //    }
            //}
            //if (string.IsNullOrEmpty(status.Status))
            //{
            //    errors.AddError("Status.Error", new Spring.Validation.ErrorMessage("RequiredSymbol"));
            //}
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            
            //DbStatus dbstatus = new DbStatus();
            //dbstatus.GroupStatus = status.GroupStatus;
            //dbstatus.Status = status.Status;
            //dbstatus.Comment = status.Comment;
            //dbstatus.Active = status.Active;
            //dbstatus.CreBy = status.CreBy;
            //dbstatus.CreDate = DateTime.Now.Date;
            //dbstatus.UpdBy = status.UpdBy;
            //dbstatus.UpdDate = DateTime.Now.Date;
            //dbstatus.UpdPgm = status.UpdPgm;
            SsDbDaoProvider.DbZoneDao.Save(zone);
            return SsDbDaoProvider.DbZoneLangDao.Save(zoneLang);
        }

        public void UpdateZoneLang(DbZone zone)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();


            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            //DbZone dbZone = SsDbDaoProvider.DbZoneDao.FindByIdentity(zone.Zoneid);
            //dbZone.GroupStatus = zone.GroupStatus;
            //dbZone.Status = zone.Status;
            //dbZone.Comment = zone.Comment;
            //dbZone.Active = zone.Active;
            //dbZone.UpdBy = zone.UpdBy;
            //dbZone.UpdDate = DateTime.Now.Date;
            //dbZone.UpdPgm = zone.UpdPgm;

            SsDbDaoProvider.DbZoneDao.SaveOrUpdate(zone);
        }

    }
}
