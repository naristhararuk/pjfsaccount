using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Query;

using SS.DB.DTO;
using NHibernate;
using SS.DB.DTO.ValueObject;

namespace SS.DB.Query
{
    public interface IDbParameterQuery : IQuery<DbParameter, short> 
    {
        string getParameterByGroupNo_SeqNo(string groupNo, string seqNo);
        IList<DbParameter> GetParameterByGroupNo(string groupNo);

        IList<Parameter> GetParameterList(short groupNo, int firstResult, int maxResult, string sortExpression);
        ISQLQuery FindByParameterCriteria(short groupNo, bool isCount, string sortExpression);
        int CountByParameterCriteria(short groupNo);

        string GetParameterByName(string name);
        IList<HashBox> GetAllNameAndValueParameter();
    }
}
