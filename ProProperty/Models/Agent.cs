using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    public class Agent
    {
        public int agent_id { get; set; }
        public String name { get; set; }
        public int contactNumber { get; set; }
        public String email { get; set; }
    }
}