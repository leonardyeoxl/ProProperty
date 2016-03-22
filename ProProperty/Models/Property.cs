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

        [NotMapped]
        private static double squareFoot = 10.7639;

        public static double GetMinBuiltSize(string roomTypeForm)
        {
            switch(roomTypeForm)
            {
                case "2":
                    return squareFoot * 45;
                case "3":
                    return squareFoot * 60;
                case "4":
                    return squareFoot * 80;
                case "5":
                    return squareFoot * 110;
                default:
                    return 0;
            }
        }

        public static double GetMaxBuiltSize(string roomTypeForm)
        {
            switch (roomTypeForm)
            {
                case "2":
                    return squareFoot * 45;
                case "3":
                    return squareFoot * 65;
                case "4":
                    return squareFoot * 100;
                case "5":
                    return squareFoot * 120;
                default:
                    return 0;
            }
        }

        //return roomtype
        public int GetRoomType()
        {
            int floorArea = (int)((double)built_size_in_sqft / squareFoot);
            if (floorArea >= 40 && floorArea <= 50)
                return 2;
            else if (floorArea >= 55 && floorArea <= 70)
                return 3;
            else if (floorArea >= 80 && floorArea <= 100)
                return 4;
            else if (floorArea >= 110 && floorArea <= 120)
                return 5;
            return 0;
        }
    }
}