using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    public class PropertyWithPremises
    {
        public Property property { get; set; }
        public List<Premise> listOfPremise { get; set; }
    }
}