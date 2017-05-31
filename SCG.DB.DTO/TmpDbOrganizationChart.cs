//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by NHibernate.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;

namespace SCG.DB.DTO
{
	/// <summary>
	/// POJO for TmpDbOrganizationchart. This class is autogenerated
	/// </summary>
	[Serializable]
	public partial class TmpDbOrganizationchart
	{
		#region Fields
		
		private long _tmp_DbOrgainzationchart;
		private string costCenterCode;
		private string departmentCode;
		private string departmentDescriptionEN;
		private string parentDepartmentCode;
		private bool active;
		private long updBy;
		private DateTime updDate;
		private long creBy;
		private DateTime creDate;
		private string updPgm;
        private string departmentDescriptionTH;
        private string departmentLevelCode;
        private string departmentLevelName;
        private string relationship;
        private string companyCode;
        private string companyNameTH;
        private string costCenterDescription;
        private string managerPersonID;
        private string managerName;
        private string managerPosition;
        private string managerPositionName;
        private string managerUserID;
        private string managerReportToUserID;
        private string sub1BusinessUnitEN;
        private string sub1BusinessUnitTH;
        private string sub1_1BusinessUnitEN;
        private string sub1_1BusinessUnitTH;
        private string companyEN;
        private string companyTH;
        private string sub1CompanyEN;
        private string sub1CompanyTH;
        private string divisionNameEN;
        private string divisionNameTH;
        private string sub1DivisionNameEN;
        private string sub1DivisionNameTH;
        private string departmentNameEN;
        private string departmentNameTH;
        private string subDepartmentNameEN;
        private string subDepartmentNameTH;
        private string sectionNameEN;
        private string sectionNameTH;
        private string sub1SectionNameEN;
        private string sub1SectionNameTH;
        private string shiftNameEN;
        private string shiftNameTH;
        private string sub1ShiftNameEN;
        private string sub1ShiftNameTH;
        private string orgIDOfSub1BusinessUnit;
        private string orgIDOfSub1_1BusinessUnit;
        private string orgIDOfCompany;
        private string orgIDOfSub1Company;
        private string orgIDOfDivision;
        private string orgIDOfSub1Division;
        private string orgIDOfDepartment;
        private string orgIDOfSub1Department;
        private string orgIDOfSection;
        private string orgIDOfSub1Section;
        private string orgIDOfShift;
        private string orgIDOfSub1Shift;
        private string startDate;
        private string delimitDate;
        private string systemDate;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the TmpDbOrganizationchart class
		/// </summary>
		public TmpDbOrganizationchart()
		{
		}

		public TmpDbOrganizationchart(long _tmp_DbOrgainzationchart)
		{
			this._tmp_DbOrgainzationchart = _tmp_DbOrgainzationchart;
		}
	
		/// <summary>
		/// Initializes a new instance of the TmpDbOrganizationchart class
		/// </summary>
		/// <param name="costCenterCode">Initial <see cref="TmpDbOrganizationchart.CostCenterCode" /> value</param>
		/// <param name="departmentCode">Initial <see cref="TmpDbOrganizationchart.DepartmentCode" /> value</param>
		/// <param name="departmentDescription">Initial <see cref="TmpDbOrganizationchart.DepartmentDescription" /> value</param>
		/// <param name="parentDepartmentCode">Initial <see cref="TmpDbOrganizationchart.ParentDepartmentCode" /> value</param>
		/// <param name="active">Initial <see cref="TmpDbOrganizationchart.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="TmpDbOrganizationchart.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="TmpDbOrganizationchart.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="TmpDbOrganizationchart.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="TmpDbOrganizationchart.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="TmpDbOrganizationchart.UpdPgm" /> value</param>
        public TmpDbOrganizationchart(string costCenterCode, string departmentCode, string departmentDescriptionEN, string parentDepartmentCode, bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm)
		{
			this.costCenterCode = costCenterCode;
			this.departmentCode = departmentCode;
            this.departmentDescriptionEN = departmentDescriptionEN;
			this.parentDepartmentCode = parentDepartmentCode;
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
		}
	
		/// <summary>
		/// Minimal constructor for class TmpDbOrganizationchart
		/// </summary>
		/// <param name="active">Initial <see cref="TmpDbOrganizationchart.Active" /> value</param>
		/// <param name="updBy">Initial <see cref="TmpDbOrganizationchart.UpdBy" /> value</param>
		/// <param name="updDate">Initial <see cref="TmpDbOrganizationchart.UpdDate" /> value</param>
		/// <param name="creBy">Initial <see cref="TmpDbOrganizationchart.CreBy" /> value</param>
		/// <param name="creDate">Initial <see cref="TmpDbOrganizationchart.CreDate" /> value</param>
		/// <param name="updPgm">Initial <see cref="TmpDbOrganizationchart.UpdPgm" /> value</param>
		public TmpDbOrganizationchart(bool active, long updBy, DateTime updDate, long creBy, DateTime creDate, string updPgm)
		{
			this.active = active;
			this.updBy = updBy;
			this.updDate = updDate;
			this.creBy = creBy;
			this.creDate = creDate;
			this.updPgm = updPgm;
		}
		#endregion
	
		#region Properties
		
		/// <summary>
		/// Gets or sets the tmp_DbOrgainzationchart for the current TmpDbOrganizationchart
		/// </summary>
		public virtual long tmp_DbOrgainzationchart
		{
			get { return this._tmp_DbOrgainzationchart; }
			set { this._tmp_DbOrgainzationchart = value; }
		}
		
		/// <summary>
		/// Gets or sets the CostCenterCode for the current TmpDbOrganizationchart
		/// </summary>
		public virtual string CostCenterCode
		{
			get { return this.costCenterCode; }
			set { this.costCenterCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the DepartmentCode for the current TmpDbOrganizationchart
		/// </summary>
		public virtual string DepartmentCode
		{
			get { return this.departmentCode; }
			set { this.departmentCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the DepartmentDescription for the current TmpDbOrganizationchart
		/// </summary>
		public virtual string DepartmentDescriptionEN
		{
            get { return this.departmentDescriptionEN; }
            set { this.departmentDescriptionEN = value; }
		}
		
		/// <summary>
		/// Gets or sets the ParentDepartmentCode for the current TmpDbOrganizationchart
		/// </summary>
		public virtual string ParentDepartmentCode
		{
			get { return this.parentDepartmentCode; }
			set { this.parentDepartmentCode = value; }
		}
		
		/// <summary>
		/// Gets or sets the Active for the current TmpDbOrganizationchart
		/// </summary>
		public virtual bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdBy for the current TmpDbOrganizationchart
		/// </summary>
		public virtual long UpdBy
		{
			get { return this.updBy; }
			set { this.updBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdDate for the current TmpDbOrganizationchart
		/// </summary>
		public virtual DateTime UpdDate
		{
			get { return this.updDate; }
			set { this.updDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreBy for the current TmpDbOrganizationchart
		/// </summary>
		public virtual long CreBy
		{
			get { return this.creBy; }
			set { this.creBy = value; }
		}
		
		/// <summary>
		/// Gets or sets the CreDate for the current TmpDbOrganizationchart
		/// </summary>
		public virtual DateTime CreDate
		{
			get { return this.creDate; }
			set { this.creDate = value; }
		}
		
		/// <summary>
		/// Gets or sets the UpdPgm for the current TmpDbOrganizationchart
		/// </summary>
		public virtual string UpdPgm
		{
			get { return this.updPgm; }
			set { this.updPgm = value; }
		}

        public virtual string DepartmentDescriptionTH
        {
            get { return this.departmentDescriptionTH; }
            set { this.departmentDescriptionTH = value; }
        }

        public virtual string DepartmentLevelCode
        {
            get { return this.departmentLevelCode; }
            set { this.departmentLevelCode = value; }
        }

        public virtual string DepartmentLevelName
        {
            get { return this.departmentLevelName; }
            set { this.departmentLevelName = value; }
        }

        public virtual string Relationship
        {
            get { return this.relationship; }
            set { this.relationship = value; }
        }

        public virtual string CompanyCode
        {
            get { return this.companyCode; }
            set { this.companyCode = value; }
        }

        public virtual string CompanyNameTH
        {
            get { return this.companyNameTH; }
            set { this.companyNameTH = value; }
        }

        public virtual string CostCenterDescription
        {
            get { return this.costCenterDescription; }
            set { this.costCenterDescription = value; }
        }

        public virtual string ManagerPersonID
        {
            get { return this.managerPersonID; }
            set { this.managerPersonID = value; }
        }

        public virtual string ManagerName
        {
            get { return this.managerName; }
            set { this.managerName = value; }
        }

        public virtual string ManagerPosition
        {
            get { return this.managerPosition; }
            set { this.managerPosition = value; }
        }

        public virtual string ManagerPositionName
        {
            get { return this.managerPositionName; }
            set { this.managerPositionName = value; }
        }

        public virtual string ManagerUserID
        {
            get { return this.managerUserID; }
            set { this.managerUserID = value; }
        }

        public virtual string ManagerReportToUserID
        {
            get { return this.managerReportToUserID; }
            set { this.managerReportToUserID = value; }
        }

        public virtual string Sub1BusinessUnitEN
        {
            get { return this.sub1BusinessUnitEN; }
            set { this.sub1BusinessUnitEN = value; }
        }

        public virtual string Sub1BusinessUnitTH
        {
            get { return this.sub1BusinessUnitTH; }
            set { this.sub1BusinessUnitTH = value; }
        }

        public virtual string Sub1_1BusinessUnitEN
        {
            get { return this.sub1_1BusinessUnitEN; }
            set { this.sub1_1BusinessUnitEN = value; }
        }

        public virtual string Sub1_1BusinessUnitTH
        {
            get { return this.sub1_1BusinessUnitTH; }
            set { this.sub1_1BusinessUnitTH = value; }
        }

        public virtual string CompanyEN
        {
            get { return this.companyEN; }
            set { this.companyEN = value; }
        }

        public virtual string CompanyTH
        {
            get { return this.companyTH; }
            set { this.companyTH = value; }
        }

        public virtual string Sub1CompanyEN
        {
            get { return this.sub1CompanyEN; }
            set { this.sub1CompanyEN = value; }
        }

        public virtual string Sub1CompanyTH
        {
            get { return this.sub1CompanyTH; }
            set { this.sub1CompanyTH = value; }
        }

        public virtual string DivisionNameEN
        {
            get { return this.divisionNameEN; }
            set { this.divisionNameEN = value; }
        }

        public virtual string DivisionNameTH
        {
            get { return this.divisionNameTH; }
            set { this.divisionNameTH = value; }
        }

        public virtual string Sub1DivisionNameEN
        {
            get { return this.sub1DivisionNameEN; }
            set { this.sub1DivisionNameEN = value; }
        }

        public virtual string Sub1DivisionNameTH
        {
            get { return this.sub1DivisionNameTH; }
            set { this.sub1DivisionNameTH = value; }
        }
        public virtual string DepartmentNameEN
        {
            get { return this.departmentNameEN; }
            set { this.departmentNameEN = value; }
        }

        public virtual string DepartmentNameTH
        {
            get { return this.departmentNameTH; }
            set { this.departmentNameTH = value; }
        }

        public virtual string SubDepartmentNameEN
        {
            get { return this.subDepartmentNameEN; }
            set { this.subDepartmentNameEN = value; }
        }

        public virtual string SubDepartmentNameTH
        {
            get { return this.subDepartmentNameTH; }
            set { this.subDepartmentNameTH = value; }
        }

        public virtual string SectionNameEN
        {
            get { return this.sectionNameEN; }
            set { this.sectionNameEN = value; }
        }

        public virtual string SectionNameTH
        {
            get { return this.sectionNameTH; }
            set { this.sectionNameTH = value; }
        }

        public virtual string Sub1SectionNameEN
        {
            get { return this.sub1SectionNameEN; }
            set { this.sub1SectionNameEN = value; }
        }

        public virtual string Sub1SectionNameTH
        {
            get { return this.sub1SectionNameTH; }
            set { this.sub1SectionNameTH = value; }
        }

        public virtual string ShiftNameEN
        {
            get { return this.shiftNameEN; }
            set { this.shiftNameEN = value; }
        }

        public virtual string ShiftNameTH
        {
            get { return this.shiftNameTH; }
            set { this.shiftNameTH = value; }
        }

        public virtual string Sub1ShiftNameEN
        {
            get { return this.sub1ShiftNameEN; }
            set { this.sub1ShiftNameEN = value; }
        }

        public virtual string Sub1ShiftNameTH
        {
            get { return this.sub1ShiftNameTH; }
            set { this.sub1ShiftNameTH = value; }
        }

        public virtual string OrgIDOfSub1BusinessUnit
        {
            get { return this.orgIDOfSub1BusinessUnit; }
            set { this.orgIDOfSub1BusinessUnit = value; }
        }

        public virtual string OrgIDOfSub1_1BusinessUnit
        {
            get { return this.orgIDOfSub1_1BusinessUnit; }
            set { this.orgIDOfSub1_1BusinessUnit = value; }
        }

        public virtual string OrgIDOfCompany
        {
            get { return this.orgIDOfCompany; }
            set { this.orgIDOfCompany = value; }
        }

        public virtual string OrgIDOfSub1Company
        {
            get { return this.orgIDOfSub1Company; }
            set { this.orgIDOfSub1Company = value; }
        }

        public virtual string OrgIDOfDivision
        {
            get { return this.orgIDOfDivision; }
            set { this.orgIDOfDivision = value; }
        }

        public virtual string OrgIDOfSub1Division
        {
            get { return this.orgIDOfSub1Division; }
            set { this.orgIDOfSub1Division = value; }
        }

        public virtual string OrgIDOfDepartment
        {
            get { return this.orgIDOfDepartment; }
            set { this.orgIDOfDepartment = value; }
        }

        public virtual string OrgIDOfSub1Department
        {
            get { return this.orgIDOfSub1Department; }
            set { this.orgIDOfSub1Department = value; }
        }

        public virtual string OrgIDOfSection
        {
            get { return this.orgIDOfSection; }
            set { this.orgIDOfSection = value; }
        }

        public virtual string OrgIDOfSub1Section
        {
            get { return this.orgIDOfSub1Section; }
            set { this.orgIDOfSub1Section = value; }
        }

        public virtual string OrgIDOfShift
        {
            get { return this.orgIDOfShift; }
            set { this.orgIDOfShift = value; }
        }

        public virtual string OrgIDOfSub1Shift
        {
            get { return this.orgIDOfSub1Shift; }
            set { this.orgIDOfSub1Shift = value; }
        }

        public virtual string StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public virtual string DelimitDate
        {
            get { return this.delimitDate; }
            set { this.delimitDate = value; }
        }

        public virtual string SystemDate
        {
            get { return this.systemDate; }
            set { this.systemDate = value; }
        }
		#endregion
	}
}
