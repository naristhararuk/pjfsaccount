using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.DB.DTO.ValueObject
{
    public class Parameter
    {
        public short Id { get; set; }
        public short GroupNo { get; set; }
        public short? SeqNo { get; set; }
        public string ConfigurationName { get; set; }
        public string ParameterValue { get; set; }
        public string Comment { get; set; }
        public string ParameterType { get; set; }
        public bool Active { get; set; }
    }
}

