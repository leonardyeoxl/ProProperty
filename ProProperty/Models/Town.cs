using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProProperty.Models
{
    [Table("Town")]
    public class Town
    {
        [Key]
        public int town_id { get; set; }
        public string town_name { get; set; }
    }
}