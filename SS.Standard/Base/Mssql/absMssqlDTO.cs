using System;
using System.Collections.Generic;
using System.Text;

namespace SS.Standard.Base.Mssql
{
    public abstract class absMssqlDTO
    {
        private int _up_by = 0;
        public int UPD_BY 
        {
            get { return this._up_by;}
            set {
                if (value.GetType().Equals("System.String"))
                    this._up_by = Convert.ToInt32(value);
                else
                    this._up_by = value;
            }
        }
        private DateTime _upd_date;
        public System.DateTime UPD_DATE
        {
            get { return this._upd_date; }
            set {
                if (value.GetType().Equals("System.String"))
                    this._upd_date = Convert.ToDateTime(value);
                else
                    this._upd_date = value;
            }
        }

        public string UPD_PGM { get; set; }

        private int _cre_by = 0;
        public int CRE_BY {

            get { return this._cre_by; }
            set
            {
                if (value.GetType().Equals("System.String"))
                    this._cre_by = Convert.ToInt32(value);
                else
                    this._cre_by = value;
            }
        }
        private DateTime _cre_date;
        public System.DateTime CRE_DATE {

            get { return this._cre_date; }
            set
            {
                if (value.GetType().Equals("System.String"))
                    this._cre_date = Convert.ToDateTime(value);
                else
                    this._cre_date = value;
            }
        }

        public byte[] ROWVERSION { get; set; }

        public bool Active { get; set; }
    }
}
