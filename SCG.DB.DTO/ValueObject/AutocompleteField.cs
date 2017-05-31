using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    [Serializable]
    public class AutocompleteField
    {        
        public long   ID          { get; set; }
        public string   Code          { get; set; }
        public string   Description        { get; set; }


        public AutocompleteField()
        {
        }
    }
}
