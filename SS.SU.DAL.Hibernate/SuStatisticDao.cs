using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SS.SU.DTO;
namespace SS.SU.DAL.Hibernate
{
    public partial class SuStatisticDao : NHibernateDaoBase<SuStatistic, int>, ISuStatisticDao
    {
    }
}
