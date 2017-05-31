using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
     [Serializable]
    public partial class UserRoles
    {
         private long userID;
         private short roleID;
         private string roleName;
         private bool receiveDocument;
         private bool verifyDocument;
         private bool approveVerifyDocument;
         private bool verifyPayment;
         private bool approveVerifyPayment;
         private bool counterCashier;
         private bool allowMultipleApprovePayment;
         private bool allowMultipleApproveAccountant;



         public virtual long UserID
         {
             get { return this.userID; }
             set { this.userID = value; }
         }
         public virtual short RoleID
         {
             get { return this.roleID; }
             set { this.roleID = value; }
         }


         public virtual string RoleName
         {
             get { return this.roleName; }
             set { this.roleName = value; }
         }

         public virtual bool ReceiveDocument
         {
             get { return this.receiveDocument; }
             set { this.receiveDocument = value; }
         }
         public virtual bool VerifyDocument
         {
             get { return this.verifyDocument; }
             set { this.verifyDocument = value; }
         }
         public virtual bool ApproveVerifyDocument
         {
             get { return this.approveVerifyDocument; }
             set { this.approveVerifyDocument = value; }
         }
         public virtual bool VerifyPayment
         {
             get { return this.verifyPayment; }
             set { this.verifyPayment = value; }
         }
         public virtual bool ApproveVerifyPayment
         {
             get { return this.approveVerifyPayment; }
             set { this.approveVerifyPayment = value; }
         }
         public virtual bool CounterCashier
         {
             get { return this.counterCashier; }
             set { this.counterCashier = value; }
         }

         public virtual bool AllowMultipleApprovePayment
         {
             get { return this.allowMultipleApprovePayment; }
             set { this.allowMultipleApprovePayment = value; }
         }

         public virtual bool AllowMultipleApproveAccountant
         {
             get { return this.allowMultipleApproveAccountant; }
             set { this.allowMultipleApproveAccountant = value; }
         }
        
    }
}
