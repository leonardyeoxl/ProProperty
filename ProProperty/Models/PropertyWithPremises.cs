using System.Collections.Generic;

namespace ProProperty.Models
{
    public class PropertyWithPremises
    {
        public Property property { get; set; }
        public Agent agent { get; set; }
        public List<Premise> listOfPremise { get; set; }
    }
}