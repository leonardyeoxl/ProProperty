using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    public class Premise
    {
        public int premise_id { get; set; }
        public String name { get; set; }
        public String address { get; set; }
        public String description { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public int type_id { get; set; }
        public String image { get; set; }
    }
}