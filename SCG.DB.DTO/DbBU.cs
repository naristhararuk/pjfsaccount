using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO
{
    public partial class DbBU
    {
        private string buCode;
        public virtual string BuCode
        {
            get { return this.buCode; }
            set { this.buCode = value; }
        }
        private string buName;
        public virtual string BuName
        {
            get { return this.buName; }
            set { this.buName = value; }
        }
        //public DbBU()
        //{ }
        //public DbBU(string buCode)
        //{
        //    this.buCode = buCode;
        //}
        //public DbBU(string buCode, string buName)
        //{
        //    this.buCode = buCode;
        //    this.buName = buName;
        //}
    }
}
