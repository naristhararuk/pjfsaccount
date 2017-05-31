using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.DTO;
using System.Data;


namespace SCG.DB.DTO
{
    public partial class DbMileageRateRevisionDetail
    {

        public DbMileageRateRevisionDetail()
        {
        }

        public DbMileageRateRevisionDetail(Guid Id)
        {
            this.id = Id;      
        }
        private Guid? id;
        public virtual Guid? Id 
        { 
            get { return id;}
            set { id = value;}
        }

        private string personalLevelGroupCode;
        public virtual string PersonalLevelGroupCode
        {
            get { return personalLevelGroupCode; }
            set { personalLevelGroupCode = value; }
        }

        private Guid mileageRateRevisionId;
        public virtual Guid MileageRateRevisionId
        {
            get { return mileageRateRevisionId; }
            set { mileageRateRevisionId = value; }
        }
        private Guid mileageProfileId;
        public virtual Guid MileageProfileId
        {
            get { return mileageProfileId; }
            set { mileageProfileId = value; }
        }

        private string profileName;
        public virtual string ProfileName
        {
            get { return profileName; }
            set {  profileName = value ;}
        }


        private Double carRate;
        public virtual Double CarRate
        {
            get { return carRate; }
            set { carRate = value; }
        }

        private Double carRate2;
        public virtual Double CarRate2
        {
            get { return carRate2; }
            set { carRate2 = value; }
        }

        private Double pickUpRate;
        public virtual Double PickUpRate
        {
            get { return pickUpRate; }
            set { pickUpRate = value; }
        }

        private Double pickUpRate2;
        public virtual Double PickUpRate2
        {
            get { return pickUpRate2; }
            set { pickUpRate2 = value; }
        }


        private Double motocycleRate;
        public virtual Double MotocycleRate
        {
            get { return motocycleRate; }
            set { motocycleRate = value; }
        }

        private Double motocycleRate2;
        public virtual Double MotocycleRate2
        {
            get { return motocycleRate2; }
            set { motocycleRate2 = value; }
        }

        private Int64 creBy;

        public virtual Int64 CreBy
        {
            get { return creBy; }
            set { creBy = value; }
        }

        private DateTime creDate;

        public virtual DateTime CreDate
        {
            get { return creDate; }
            set { creDate = value; }
        }

        private Int64 updBy;

        public virtual Int64 UpdBy
        {
            get { return updBy; }
            set { updBy = value; }
        }

        private DateTime updDate;

        public virtual DateTime UpdDate
        {
            get { return updDate; }
            set { updDate = value; }
        }

        private string updPgm;

        public virtual string UpdPgm
        {
            get { return updPgm; }
            set { updPgm = value; }
        }

        private Byte[] rowVersion;

        public virtual Byte[] RowVersion
        {
            get { return rowVersion; }
            set { rowVersion = value; }
        }

        private bool active;

        public virtual bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private DateTime effectiveFromDate;
        public virtual DateTime EffectiveFromDate
        {
            get { return effectiveFromDate; }
            set { effectiveFromDate = value; }
        }

        private DateTime effectiveToDate;
        public virtual DateTime EffectiveToDate
        {
            get { return effectiveToDate; }
            set { effectiveToDate = value; }
        }

        private DateTime approvedDate;
        public virtual DateTime ApprovedDate
        {
            get { return approvedDate; }
            set { approvedDate = value; }
        }

 
    }
}
