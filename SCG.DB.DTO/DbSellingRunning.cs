using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    public class DbSellingRunning
    {
        public virtual long RunningID { get; set; }
        public virtual long CompanyID{ get; set;}
        public virtual int Year { get; set; }
        public virtual long RunningNo { get; set; }
        public virtual long CreBy { get; set; }
        public virtual DateTime CreDate { get; set; }
        public virtual long UpdBy { get; set; }
        public virtual DateTime UpdDate { get; set; }
        public virtual Byte RowVersion { get; set; }
    }
}
