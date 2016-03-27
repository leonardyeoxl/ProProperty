using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProProperty.Models
{
    [Table("Agent")]
    public class Agent
    {
        [Key]
        public int agent_id { get; set; }
        public String agent_name { get; set; }
        public int agent_contact_number { get; set; }
        public String agent_email { get; set; }
        public String agent_image { get; set; }
    }
}