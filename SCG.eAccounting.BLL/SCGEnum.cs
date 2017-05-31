using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;

using Spring.Globalization.Resolvers;

namespace SCG.eAccounting.BLL
{
    public class ZoneType 
    {
        public const string Domestic = "DM";//For identify Domestic type of each item
        public const string Foreign = "FR";//For identify Foreign type of each item
    }

    public class PostingStaus
    {
        public const string New = "N";
        public const string Posted = "P";
        public const string PartialPosted = "PP";
        public const string Complete = "C";
    }

    public enum ActorType
    { 
        Creator,
        Requester,
        Initiator,
        Approver,
        Verifier,
        ApproveVerifier,
        Cashier,
        Receiver,
        SAP
    }
    public enum EmailType
    {
        EM01,
        EM02,
        EM03,
        EM04,
        EM05,
        EM06,
        EM07,
        EM08,
        EM09,
        EM10,
        EM11,
        EM12, 
        EM13,
        EM14, 
        EM15, 
        EM16
    }

  
    #region SMS ENUM
    public enum SMSStatus
    {
        Send,
        Receive
    }
    public enum SMSContenFormat
    {
        SMS01,
        SMS02,
        SMS03
    }
    public enum SMSBusinessCase
    {
        AdvanceDomesticDocument,
        AdvanceForeignDocument,
        ExpenseDomesticDocument,
        ExpenseForeignDocument,
        TADocumentDomestic,
        TADocumentForeign,
        Cash,
        Cheque,
        None
    }

    #endregion 

    public struct TravellBy
    {
        //Use in TADocument  
        public const string Domestic = "D";
        public const string Foreign = "F";
    }

    public struct Ticketing
    {
        public const string TravellingServiceSection = "T";
        public const string EmployeeSection = "E";
    }

    public struct InvoiceType
    {
        //Modifier by thum  at  19/03/2009
        //Use in ExpenseInvoiceItem  
        public const string General = "G";
        public const string Perdiem = "P";
        public const string Mileage = "M";
    }

    public struct GroupStatus
    {
        // Use in table DBStatus.
        public const string CancelType = "CancelType";
        public const string ChequeStatus = "ChequeStatus";
        public const string FaxSend = "FaxSend";
        public const string Mapped = "Mapped";
        public const string ReceiptType = "ReceiptType";
        public const string PaymentTypeDMT = "PaymentTypeDMT";
        public const string PaymentTypeFRN = "PaymentTypeFRN";
        public const string TravelBy = "TravelBy";
    }
    public struct DocumentTypeID
    {
        public const int AdvanceDomesticDocument = 1;
        public const int AdvanceForeignDocument = 5;
        public const int TADocumentDomestic = 2;
        public const int ExpenseDomesticDocument = 3;
        public const int ExpenseForeignDocument = 7;
        public const int RemittanceDocument = 4;
        public const int TADocumentForeign = 8;
        public const int MPADocument = 10;
        public const int CADocument = 11;
        public const int FixedAdvanceDocument = 12;

    }

    public struct PaymentType
    {
        public const string CA = "CA";//เงินสด
        public const string CQ = "CQ";//เช็ค
        public const string TR = "TR";//เงินโอน
        public const string BN = "BN";//Bank note
        public const string TQ = "TQ";//Traveller cheque
        public const string DF = "DF";//Draft
    }

    public struct WorkFlowTypeID
    {
        public const int AdvanceWorkFlowType = 1;
        //public const int AdvanceForeignWorkFlowType = 6;
        public const int TAWorkFlowType = 4;
        public const int RemittanceWorkFlow = 5;
        public const int ExpenseWorkFlow = 7;
        public const int MPAWorkFlow = 8;
        public const int CAWorkFlow = 9;
        public const int FixedAdvanceWorkFlow = 10;
    }

    public struct AdvanceStateID
    {
        public const int Draft = 1;
        public const int WaitAR = 2;
        public const int WaitInitial = 3;
        public const int WaitApprove = 4;
        public const int WaitVerify = 5;
        public const int WaitApproveVerify = 6;
        public const int Hold = 7;
        public const int WaitPayment = 8;
        public const int Complete = 9;
        public const int Cancel = 10;
        public const int WaitApproveRejection = 11;
        public const int WaitReverse = 12;
        public const int WaitDocument = 13;
        public const int OutStanding = 25;
        public const int WaitTA = 40;
        public const int WaitTAInitial = 42;
        public const int WaitTAApprove = 43;
    }

    public struct WorkFlowStateName
    {
        public const string Cancel = "Cancel";
        public const string Cancelled = "Cancelled";
        public const string Complete = "Complete";
        public const string Draft = "Draft";
        public const string Hold = "Hold";
        public const string WaitApprove = "Wait for Approve";
        public const string WaitApproveRejection = "Wait for Approve Rejection";
        public const string WaitApproveVerify = "Wait for Approve Verify";
        public const string WaitAR = "Wait for A/R";
        public const string WaitDocument = "Wait for Document";
        public const string WaitInitial = "Wait for Initial";
        public const string WaitPayment = "Wait for Payment";
        public const string WaitPaymentSAP = "Wait for Payment SAP";
        public const string WaitRemittance = "Wait for Remittance";
        public const string WaitReverse = "Wait for Reverse";
        public const string WaitVerify = "Wait for Verify";
    }

    public struct WorkFlowStateFlag
    {
        public const string Draft = "Draft";
        public const string WaitApprove = "WaitApprove";
        public const string WaitAgree = "WaitAR";
        public const string WaitInitial = "WaitInitial";
        public const string Hold = "Hold";
        public const string WaitVerify = "WaitVerify";
        public const string WaitApproveVerify = "WaitApproveVerify";
        public const string WaitPayment = "WaitPayment";
        public const string WaitReceive = "WaitDocument";
        public const string Outstanding = "Outstanding";
        public const string OutstandingOverdue = "OutstandingOverdue";
        public const string WaitRemittance = "WaitRemittance";
        public const string WaitApproveRemittance = "WaitApproveRemittance";
        public const string Complete = "Complete";
    }

    public struct WorkFlowEventID
    {
        public const int AdvanceApproveWaitApproveVerify = 19;
        public const int ExpenseApproveWaitApproveVerify = 74;
        public const int RemittanceApproveWaitApproveRemittance = 107;
    }

    public struct EventName
    {
        public const string Send = "Send";
        public const string Approve = "Approve";
    }

    public struct OwnerMileage
    {
        public const string Company = "COM";//บริษัท
        public const string Employee = "EMP";//พนักงาน  
    }

    public struct TypeOfCar
    {
        public const string PrivateCar = "PRI";//รถส่วนตัว
        public const string Pickup = "PIC";//รถบิ๊กอัพ
        public const string MotorCycle = "MOT";//รถจักรยานยนต์
    }

    public struct ExpenseGroup
    {
        public const string Office = "0";//Office
        public const string Factory = "1";//Factory
        public const string Both = "2";//Both
    }

    public enum ViewPostDocumentType
    {
        AdvanceDomestic,
        AdvanceForeign,
        Remittance,
        Expense,
        FixedAdvance
    }

    public struct PayrollType
    {
        public const string Perdiem = "PERDIEM";
        public const string Mileage = "MILEAGE";
    }

    public enum CurrencySymbol
    { 
        THB,
        USD
    }

    public class PersonalGroupType
    {
        public const string InternationalStaff = "A"; // for international staff
    }

    public enum CountryZonePerdiem
    { 
        NormalZone = 1,
        HighZone = 2,
        ThaiZone = 7,
        DomesticZone = 8
    }

    public enum TaxCodeOption
    { 
        None, Require
    }

    public enum CostCenterOption
    {
        None, Require, Optional
    }

    public enum InternalOrderOption
    {
        None, Require, Optional
    }

    public enum SaleOrderOption
    {
        None, Require, Optional
    }
    public enum FixedAdvanceTypeOption
    { 
        New = 1,
        Adjust = 2,
        Return = 3
    }

    public struct FixedAdvanceStateID
    {
        public const int Draft = 1;
        //public const int WaitAR = 2;
        //public const int WaitInitial = 3;
        //public const int WaitApprove = 4;
        //public const int WaitVerify = 5;
        //public const int WaitApproveVerify = 6;
        //public const int Hold = 7;
        //public const int WaitPayment = 8;
        public const int Complete = 9;
        public const int Cancel = 10;
        //public const int WaitApproveRejection = 11;
        //public const int WaitReverse = 12;
        //public const int WaitDocument = 13;
        //public const int OutStanding = 25;
        //public const int WaitTA = 40;
        //public const int WaitTAInitial = 42;
        //public const int WaitTAApprove = 43;
    }

}
