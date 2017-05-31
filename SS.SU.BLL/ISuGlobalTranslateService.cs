using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;

namespace SS.SU.BLL
{
    public interface ISuGlobalTranslateService : IService<SuGlobalTranslate, long>
    {
        //IList<SuGlobalTranslate> FindAll();
        //void Delete(SuGlobalTranslate domain);
        //SuGlobalTranslate FindByIdentity(long id);
        //long Save(SuGlobalTranslate domain);
        //void SaveOrUpdate(SuGlobalTranslate domain);
        //void Update(SuGlobalTranslate domain);
        IList<SuGlobalTranslate> FindBySuGolbalTranslateCriteria(SuGlobalTranslate criteria, int firstResult, int maxResults, string sortExpression);
        int CountBySuGolbalTranslateCriteria(SuGlobalTranslate criteria);
        long AddGlobalTranslate(SuGlobalTranslate globalTranslate);
        void UpdateGlobalTranslate(SuGlobalTranslate globalTranslate);

		long AddProgramTranslateControl_Symbol(SuGlobalTranslate globalTranslate);

		void DeleteByProgramCodeAndControl(string programCode, string translateControl);
    }
}
