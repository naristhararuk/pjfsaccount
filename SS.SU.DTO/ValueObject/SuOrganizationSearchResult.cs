using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
	[Serializable]
	public partial class SuOrganizationSearchResult
	{
		#region Field
		private short organizationId;
		private string organizationName;
		private short languageId;
		private string languageName;
		private string comment;
		private bool active;
		//private SuLanguage language;
		//private SuOrganization organization;
		//private SuOrganizationLang organizationLang;
		#endregion
		
		#region Constructor
		public SuOrganizationSearchResult()
		{
		
		}
		#endregion
		
		#region Property
		//public virtual SuLanguage Language
		//{
		//    get { return this.language; }
		//    set { this.language = value; }
		//}
		//public virtual SuOrganization Organization
		//{
		//    get { return this.organization; }
		//    set { this.organization = value; }
		//}
		//public virtual SuOrganizationLang OrganizationLang
		//{
		//    get { return this.organizationLang; }
		//    set { this.organizationLang = value; }
		//}
		public virtual short OrganizationId
		{
			get { return this.organizationId; }
			set { this.organizationId = value; }
		}
		public virtual short LanguageId
		{
			get { return this.languageId; }
			set { this.languageId = value; }
		}
		public virtual string OrganizationName
		{
			get { return this.organizationName; }
			set { this.organizationName = value; }
		}
		public virtual string LanguageName
		{
			get { return this.languageName; }
			set { this.languageName = value; }
		}
		public virtual string Comment
		{
			get { return this.comment; }
			set { this.comment = value; }
		}
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		#endregion
	}
}
