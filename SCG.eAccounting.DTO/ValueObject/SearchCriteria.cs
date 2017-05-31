using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCG.eAccounting.DTO.ValueObject
{
    [Serializable]
    public class SearchCriteria
    {
        #region
        private string budgetYear;
        private string docTypeFrom;
        private string docTypeTo;
        private string costCenterFrom;
        private string costCenterTo;
        private string creDateFrom;
        private string creDateTo;
        private string docNoFrom;
        private string docNoTo;
        private double? amountFrom;
        private double? amountTo;
        private string docStatus;
        private bool multipleApprove;
        private bool selectedHrFrom;
        private List<ImageOptionCriteria> imageOption;

        #endregion

        #region Contructor
        public SearchCriteria()
        {
            imageOption = new List<ImageOptionCriteria>();
        }
        #endregion

        #region Property
        public string BudgetYear
        {
            get { return budgetYear; }
            set { this.budgetYear = value; }
        }
        public string DocTypeFrom
        {
            get { return docTypeFrom; }
            set { this.docTypeFrom = value; }
        }
        public string DocTypeTo
        {
            get { return docTypeTo; }
            set { this.docTypeTo = value; }
        }
        public string CostCenterFrom
        {
            get { return costCenterFrom; }
            set { this.costCenterFrom = value; }
        }
        public string CostCenterTo
        {
            get { return costCenterTo; }
            set { this.costCenterTo = value; }
        }
        public string CreDateFrom
        {
            get { return creDateFrom; }
            set { this.creDateFrom = value; }
        }
        public string CreDateTo
        {
            get { return creDateTo; }
            set { this.creDateTo = value; }
        }
        public string DocNoFrom
        {
            get { return docNoFrom; }
            set { this.docNoFrom = value; }
        }
        public string DocNoTo
        {
            get { return docNoTo; }
            set { this.docNoTo = value; }
        }
        public double? AmountFrom
        {
            get { return amountFrom; }
            set { this.amountFrom = value; }
        }
        public double? AmountTo
        {
            get { return amountTo; }
            set { this.amountTo = value; }
        }
        public string DocStatus
        {
            get { return docStatus; }
            set { this.docStatus = value; }
        }

        public bool MutipleApprove
        {
            get { return multipleApprove; }
            set { this.multipleApprove = value; }
        }

        public bool SelcetseHrFrom
        {
            get { return selectedHrFrom; }
            set { this.selectedHrFrom = value; }
        }

        public List<ImageOptionCriteria> ImageOption
        {
            get { return this.imageOption; }
            set { this.imageOption = value; }
        }

        #endregion

        #region Inbox
        public string RequestNo { get; set; }
        public string RequestDateFrom { get; set; }
        public string RequestDateTo { get; set; }
        public long CreatorID { get; set; }
        public long RequesterID { get; set; }
        public long ApproverID { get; set; }
        public long InitiatorID { get; set; }
        public long Receiver { get; set; }
        public long DocumentTypeID { get; set; }
        public string DocumentStatus { get; set; }
        public long UserID { get; set; }
        public string FlagQuery { get; set; }
        public string Subject { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public long CompanyID{ get; set; }
        public string Status { get; set; }
        public string FlagSearch { get; set; }
        public string FlagOutstanding { get; set; }
        public string FlagJoin { get; set; }
        public short LanguageID { get; set; }
        public string ReferneceNo { get; set; }
        public long ServiceTeamID { get; set; }
        public long PBID { get; set; }
        public string SearchType { get; set; }
        #endregion Inbox       
    }

    public enum ImageOptionCriteria
    {
        Image,
        HardCopy,
        ImageHardCopy
    }
}
