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
        [DisplayFormat(DataFormatString = "{0:0.00000000000}")]
        public decimal Latitude { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00000000000}")]
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

        public static double GetMinBuiltSize(string roomTypeForm)
        {
            double squareFoot = 10.7639;
            switch(roomTypeForm)
            {
                case "2":
                    return Math.Round(squareFoot * 45);
                case "3":
                    return Math.Round(squareFoot * 60);
                case "4":
                    return Math.Round(squareFoot * 80);
                case "5":
                    return Math.Round(squareFoot * 110);
                default:
                    return 0;
            }
        }

        public static double GetMaxBuiltSize(string roomTypeForm)
        {
            double squareFoot = 10.7639;
            switch (roomTypeForm)
            {
                case "2":
                    return Math.Round(squareFoot * 45);
                case "3":
                    return Math.Round(squareFoot * 65);
                case "4":
                    return Math.Round(squareFoot * 100);
                case "5":
                    return Math.Round(squareFoot * 120);
                default:
                    return 0;
            }
        }

        //return roomtype
        public static int GetRoomType(decimal MinBuildSize, decimal MaxBuildSize)
        {
            return 0;
        }
    }
}