using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Service;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

namespace SS.SU.BLL
{
	public interface ISuAnnouncementGroupService : IService<SuAnnouncementGroup, short>
	{
		IList<SuAnnouncementGroup> FindBySuAnnouncementGroupCriteria(SuAnnouncementGroup announcementGroupCriteria, int firstResult, int maxResults, string sortExpression);
		IList<AnnouncementGroup> GetTranslatedList(short languageId);
		int CountBySuAnnouncementGroupCriteria(SuAnnouncementGroup announcementGroupCriteria);
		short AddAnnouncementGroup(SuAnnouncementGroup announcementGroup, SuAnnouncementGroupLang announcementGroupLang);
		void UpdateAnnouncementGroup(SuAnnouncementGroup announcementGroup);
		
		short AddAnnouncementGroup(SuAnnouncementGroup announcementGroup, SuAnnouncementGroupLang announcementGroupLang, HttpPostedFile imageFileStream);
		void UpdateAnnouncementGroup(SuAnnouncementGroup announcementGroup, HttpPostedFile imageFileStream);
	}
}
