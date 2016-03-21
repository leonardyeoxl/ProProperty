using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProProperty.Models
{
    [Table("Premises_type")]
    public class PremiseType
    {
        [Key]
        public int premises_type_id { get; set; }
        public string premises_type_name { get; set; }

        [NotMapped]
        public Boolean isChecked { get; set; } = false;
    }
}