using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    [Table("Property")]
    public class Property
    {
        [Key]
        public int propertyID { get; set; }
        public String address { get; set; }
        public String block { get; set; }
        public int unitFloor { get; set; }
        public int unitNo { get; set; }
        public int postalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int HDBTown { get; set; }
        public int asking { get; set; }
        public String agreedPrice { get; set; }
        public int valuation { get; set; }
        public decimal built_size_in_sqft { get; set; }
        public decimal land_size_in_sqft { get; set; }
        public String propertyType { get; set; }
        public DateTime datePosted { get; set; }
        public DateTime exclusiveDate { get; set; }
        public DateTime expiryDate { get; set; }
    }
}