using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ProProperty.Models
{
    [Table("Hdb_price_range")]
    public class Hdb_price_range
    {
        [Key]
        public int hdb_id { get; set; }
        public string financial_year { get; set; }
        public string town { get; set; }
        public string room_type { get; set; }
        public string min_selling_price {get; set;}
        public string max_selling_price { get; set; }
        public string min_selling_price_less_ahg_shg { get; set; }
        public string max_selling_price_less_ahg_shg { get; set; }
    }
}