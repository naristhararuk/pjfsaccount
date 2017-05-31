using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCG.eAccounting.SAP.DTO;
using SCG.eAccounting.SAP.Service;
using SCG.eAccounting.SAP.Query;

namespace SCG.eAccounting.SAP.BAPI.Service.SIMULATOR
{
    public class GenerateRunning
    {
        #region public static string GetFIDoc(string Year, string Period)
        public static  string GetFIDoc(string Year, string Period)
        {
            IList<Bapirunning> listRunning = BapiQueryProvider.BapirunningQuery.FindByYearPeriod(Year, Period);
            if (listRunning.Count > 0)
            {
                listRunning[0].Runing = listRunning[0].Runing + 1;
                BapiServiceProvider.BapirunningService.SaveOrUpdate(listRunning[0]);

                return listRunning[0].Year.Substring(2, 2) + listRunning[0].Period + listRunning[0].Runing.ToString("000000");
            }
            else
            {
                Bapirunning running = new Bapirunning();
                running.Year = Year;
                running.Period = Period;
                running.Runing = 1;
                running.Active = true;
                running.CreBy = 1;
                running.CreDate = DateTime.Now;
                running.UpdBy = 1;
                running.UpdDate = DateTime.Now;
                running.UpdPgm = "Simulate";
                BapiServiceProvider.BapirunningService.SaveOrUpdate(running);

                return running.Year.Substring(2, 2) + running.Period + running.Runing.ToString("000000");
            }
        }
        #endregion public static string GetFIDoc(string Year, string Period)
    }
}
