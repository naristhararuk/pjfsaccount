using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.DB.DTO.ValueObject
{
    public class ServiceTeamLocationResult
    {
        public long     ServiceTeamLocationID   { get;set;}
        public long     ServiceTeamID           {get;set;}
        public long     LocationID              {get;set;}
        public bool     Active                  {get;set;}

        public string   CompanyCode { get; set; }
        public string   CompanyName             { get; set; }
        public string   LocationCode { get; set; }
        public string   LocationName            { get; set; }
        public string   Description { get; set; }
    }
}
