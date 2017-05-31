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
using SCG.DB.DTO.ValueObject;

namespace SCG.DB.BLL.Implement
{
    public partial class TmpDbOrganizationChartService : ServiceBase<TmpDbOrganizationchart, long>, ITmpDbOrganizationChartService
    {
        public override IDao<TmpDbOrganizationchart, long> GetBaseDao()
        {
            return ScgDbDaoProvider.TmpDbOrganizationChartDao;
        }

        public void AddTmpOrganizationChart(TmpDbOrganizationchart tmpOrgChart)
        {
            ScgDbDaoProvider.TmpDbOrganizationChartDao.Save(tmpOrgChart);
        }

        public void AddTmpOrganizationChartList(List<TmpDbOrganizationchart> tmpOrgChartList)
        {
            foreach(TmpDbOrganizationchart tmpOrgChart in tmpOrgChartList)
            {
                ScgDbDaoProvider.TmpDbOrganizationChartDao.Save(tmpOrgChart);
            }
        }

        public void DeleteAll()
        {
            ScgDbDaoProvider.TmpDbOrganizationChartDao.DeleteAll();
        }

        public void CommitImport()
        {
            ScgDbDaoProvider.TmpDbOrganizationChartDao.CommitImport();
        }
    }
}
