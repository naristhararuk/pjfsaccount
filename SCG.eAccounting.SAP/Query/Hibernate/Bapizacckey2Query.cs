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
    public class Bapizacckey2Query : NHibernateQueryBase<Bapizacckey2, long>, IBapizacckey2Query
    {
        public IList<Bapizacckey2> FindByDocNo(string DocNo)
        {
            return GetCurrentSession().CreateQuery("FROM Bapizacckey2 A WHERE  A.DocNo LIKE :DOC_NO").SetString("DOC_NO", DocNo + "%").List<Bapizacckey2>();
        }

        public void DeleteByDocNo(string DocNo)
        {
            GetCurrentSession()
            .Delete(" FROM Bapizacckey2 A WHERE A.DocNo LIKE :DOC_NO ",
            new object[] { DocNo + "%" },
            new NHibernate.Type.IType[] { NHibernateUtil.String });
        }
    }
}
