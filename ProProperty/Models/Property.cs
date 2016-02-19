using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    public class Property
    {
        public int property_id { get; set; }
        public String address { get; set; }
        public String block { get; set; }
        public String unitFloor { get; set; }
        public String unitNo { get; set; }
        public String postalCode { get; set; }
        public decimal latitude { get; set; }
        public decimal longtitude { get; set; }
        public int hdbTown_id { get; set; }
        public int asking { get; set; }
        public String askingPrice { get; set; }
        public int valuation { get; set; }
        public decimal built_size { get; set; }
        public decimal land_size { get; set; }
        public String type { get; set; }
        public DateTime datePosted { get; set; }
        public DateTime exclusiveDate { get; set; }
        public DateTime expiryDate { get; set; }
    }
}