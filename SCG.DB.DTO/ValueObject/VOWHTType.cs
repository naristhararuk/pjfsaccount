using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class VOWHTType
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Text
        {
            get { return String.Format("{0}-{1}", Code,Name); }
        }
        public bool IsPeople { get; set; }
    }
}
