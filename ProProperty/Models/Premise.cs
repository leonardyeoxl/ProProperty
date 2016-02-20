using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    [Table("Premises")]
    public class Premise
    {
        [Key]
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