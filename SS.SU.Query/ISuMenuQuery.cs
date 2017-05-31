using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;
using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.Standard.Data.Query.NHibernate;
using NHibernate;

namespace SS.SU.Query
{
    public interface ISuMenuQuery : IQuery<SuMenu, short> , ISimpleMasterQuery
    {
        List<UserMenu> FindAllMenu(long userID, short languageID);
        IList<SuMenuSearchResult> GetTranslatedList(short languageID, int firstResult, int maxResult, string sortExpression);
        IList<MenuLang> FindMenuLangByTranslateId(short menuId);
        ISQLQuery FindSuMenuSearchResult(short languageID,string sortExpression,bool isCount);
        int GetCountMenuList(short languageID);
        short GetMenuLevel(short? mainMenuId, short menuId);

        short FindMenuMainIDByProgramID(short programID);
        IList<MenuPath> FindAllByLanguage(short languageID);
    }
}
