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
        public int premises_id { get; set; }
        public String premises_name { get; set; }
        public String premises_address { get; set; }
        public String premises_description { get; set; }
        public decimal premises_lat { get; set; }
        public decimal premises_long { get; set; }
        public int premises_type_id { get; set; }
        public String premises_image { get; set; }
    }
}