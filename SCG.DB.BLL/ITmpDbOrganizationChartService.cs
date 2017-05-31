using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SCG.DB.DTO;

namespace SCG.DB.BLL
{
    public interface ITmpDbOrganizationChartService : IService<TmpDbOrganizationchart, long>
    {
        void AddTmpOrganizationChart(TmpDbOrganizationchart tmpOrgChart);
        void AddTmpOrganizationChartList(List<TmpDbOrganizationchart> tmpOrgChartList);
        void DeleteAll();
        void CommitImport();
    }
}
