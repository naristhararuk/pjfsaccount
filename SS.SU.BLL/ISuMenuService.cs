using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuMenuService : IService<SuMenu, short>, ISimpleMasterService
    {
        //IList<SuGlobalTranslate> FindAll();
        //void Delete(SuGlobalTranslate domain);
        //SuGlobalTranslate FindByIdentity(long id);
        //long Save(SuGlobalTranslate domain);
        //void SaveOrUpdate(SuGlobalTranslate domain);
        //void Update(SuGlobalTranslate domain);
        IList<SuMenu> FindBySuMenuCriteria(SuMenu criteria, int firstResult, int maxResults, string sortExpression);
        int CountBySuMenuCriteria(SuMenu criteria);
        long AddMenu(SuMenu menu);
        void UpdateMenu(SuMenu menu);
        

    }
}
