using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SCG.FN.DAL;
using SCG.FN.DTO;
using NHibernate;
using SCG.GL.DTO;
using NHibernate.Expression;
using SS.Standard.Data.NHibernate.QueryCreator;
using SCG.FN.DTO.ValueObject;
using NHibernate.Transform;

namespace SCG.FN.DAL.Hibernate
{
    public partial class FnReceiptTypeLangDao : NHibernateDaoBase<FnReceiptTypeLang, short>, IFnReceiptTypeLangDao
    {
        public FnReceiptTypeLangDao()
        {
        }
        public void DeleteAllReceiptTypeLang(short receiptTypeLangId)
        {
            GetCurrentSession()
            .Delete("from FnReceiptTypeLang rl where rl.ReceiptType.ReceiptTypeID = :ReceiptTypeID ", new object[] { receiptTypeLangId }, new NHibernate.Type.IType[] { NHibernateUtil.Int16 });
        }
      
    }
}
