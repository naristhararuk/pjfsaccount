using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.QueryDao;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Query.Interface;

namespace SCG.eAccounting.SAP.Query.Hibernate
{
    public class Bapirtax1u15Query : NHibernateQueryBase<Bapirtax1u15, long>, IBapirtax1u15Query
    {
        public IList<Bapirtax1u15> FindByDocNo(string DocNo)
        {
            return GetCurrentSession().CreateQuery("FROM Bapirtax1u15 A WHERE  A.DocNo LIKE :DOC_NO").SetString("DOC_NO", DocNo + "%").List<Bapirtax1u15>();
        }

        public void DeleteByDocNo(string DocNo)
        {
            GetCurrentSession()
            .Delete(" FROM Bapirtax1u15 A WHERE A.DocNo LIKE :DOC_NO ",
            new object[] { DocNo + "%" },
            new NHibernate.Type.IType[] { NHibernateUtil.String });
        }
    }
}
