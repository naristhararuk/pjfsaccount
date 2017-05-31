using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using SCG.DB.DTO.ValueObject;
using NHibernate.Transform;
namespace SCG.eAccounting.DAL.Hibernate
{
    public class DbDocumentBoxIDPostingDao : NHibernateDaoBase<DbDocumentBoxidPosting,long>,IDbDocumentBoxIDPostingDao
    {
     
    }
}
