using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class VOWHTRate
    {
        public long ID { get; set; }
        public double Rate { get; set; }
        public string Description { get; set; }
        public string Text
        {
            //modify by tom 01/12/2009, show description only.
            //get { return String.Format("{0}-{1}", Rate, Description); }
            get { return String.Format("{0}", Description); }
        }
    }
}
