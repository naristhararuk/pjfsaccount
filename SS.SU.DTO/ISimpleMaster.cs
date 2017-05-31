using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.SU.DTO
{
    public interface ISimpleMaster
    {
        //short Divisionid { get; set; }
        short Menuid { get; set; }
        string MenuCode { get; set; }       //Code
        short MenuLevel { get; set; }      //Title
        string Comment { get; set; }
        long UpdBy { get; set; }
        DateTime UpdDate { get; set; }
        long CreBy { get; set; }
        DateTime CreDate { get; set; }
        string UpdPgm { get; set; }
        Byte[] RowVersion { get; set; }
        bool Active { get; set; }
    }
}
