using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;

namespace SS.SU.BLL.Implement
{
    public partial class SuDivisionService : ServiceBase<SuDivision, short>, ISuDivisionService
    {
        public override IDao<SuDivision, short> GetBaseDao()
        {
            return DaoProvider.SuDivisionDao;
        }
        public IList<SuDivision> FindDivisionByOrganization(SuOrganization criteria, int firstResult, int maxResults, string sortExpression)
        {
            return NHibernateDaoHelper.FindPagingByCriteria<SuDivision>(DaoProvider.SuDivisionDao, "FindByOrganizationCriteria", new object[] { criteria, false }, firstResult, maxResults, sortExpression);
        }
        public int CountByOrganizationCriteria(SuOrganization criteria)
        {
            return NHibernateDaoHelper.CountByCriteria(DaoProvider.SuDivisionDao, "FindByOrganizationCriteria", new object[] { criteria, true });
        }
		public short AddDivision(SuDivision div, SuDivisionLang divLang)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (div.Organization == null)
            {
                errors.AddError("Division.Error", new Spring.Validation.ErrorMessage("RequiredOrganization"));
            }
			if (string.IsNullOrEmpty(divLang.DivisionName))
			{
                errors.AddError("Division.Error", new Spring.Validation.ErrorMessage("RequiredDivisionName"));
			}

            //Check ProgramCode and symbol is unique.
            //if (DaoProvider.SuGlobalTranslateDao.IsDuplicateProgramCodeSymbol(globalTranslate))
            //{
            //    errors.AddError("GlobalTranslate.Error", new Spring.Validation.ErrorMessage("UniqueProgramCodeSymbol"));
            //}
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

			//SuDivision division = new SuDivision();
			//division.Comment = div.Comment;
			//division.Active = div.Active;
			//division.CreBy = div.CreBy;
			//division.CreDate = DateTime.Now.Date;
			//division.UpdBy = div.UpdBy;
			//division.UpdDate = DateTime.Now.Date;
			//division.UpdPgm = div.UpdPgm;

			short divId = DaoProvider.SuDivisionDao.Save(div);
			DaoProvider.SuDivisionLangDao.Save(divLang);
			
            return divId;
        }
        public void UpdateDivision(SuDivision div)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (div.Organization == null)
            {
                errors.AddError("Division.Error", new Spring.Validation.ErrorMessage("RequiredOrganization"));
            }

            //Check ProgramCode and symbol is unique.
            //if (DaoProvider.SuGlobalTranslateDao.IsDuplicateProgramCodeSymbol(globalTranslate))
            //{
            //    errors.AddError("GlobalTranslate.Error", new Spring.Validation.ErrorMessage("UniqueProgramCodeSymbol"));
            //}
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

			//SuDivision division = new SuDivision();
			//division.Comment = div.Comment;
			//division.Active = div.Active;
			//division.UpdBy = div.UpdBy;
			//division.UpdDate = DateTime.Now.Date;
			//division.UpdPgm = div.UpdPgm;            

			//if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.SuDivisionDao.SaveOrUpdate(div);
        }

        //#region TestMasterGrid
        //public void DeleteMasterRecord(string id)
        //{
        //    SuDivision division = DaoProvider.SuDivisionDao.FindByIdentity(Convert.ToInt16(id));
        //    DaoProvider.SuDivisionDao.Delete(division);
        //}
        //public void Add(ISimpleMaster obj)
        //{
        //    //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

        //    SuDivision division = new SuDivision();
        //    division.Comment = obj.Comment;
        //    division.Active = obj.Active;
        //    division.CreBy = obj.CreBy;
        //    division.CreDate = obj.CreDate;
        //    division.UpdBy = obj.UpdBy;
        //    division.UpdDate = obj.UpdDate;
        //    division.UpdPgm = obj.UpdPgm;
        //    division.Organization = new SuOrganization(Convert.ToInt16("1"));
        //    short divId = DaoProvider.SuDivisionDao.Save(division);
        //}
        //public void Updated(ISimpleMaster obj)
        //{
        //    //Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

        //    SuDivision division = DaoProvider.SuDivisionDao.FindByIdentity(obj.Divisionid);
        //    division.Comment = obj.Comment;
        //    division.Active = obj.Active;
        //    division.UpdBy = obj.UpdBy;
        //    division.UpdDate = obj.UpdDate;
        //    division.UpdPgm = obj.UpdPgm;
        //    DaoProvider.SuDivisionDao.SaveOrUpdate(division);
        //}
        //#endregion
    }
}
