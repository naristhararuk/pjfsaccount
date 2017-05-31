using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using SS.Standard.Data.NHibernate.Query;
using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.Query
{
	public interface ISuUserLangQuery : IQuery<SuUserLang, long>
	{
		IList<UserLang> FindUserLangByUserId(long userId); 
		IList<SuUserLang> FindByUserAndLanguage(long userId, short languageId);
	}
}
