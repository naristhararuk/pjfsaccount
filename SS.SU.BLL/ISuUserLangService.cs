using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
	public interface ISuUserLangService : IService<SuUserLang, long>
	{
		IList<SuUserLang> FindByUserIdAndLanguageId(long userID, short languageID);
		
		long AddNewUserLang(SuUserLang userLang);
		void UpdateUserLang(SuUserLang userLang);
	}
}
