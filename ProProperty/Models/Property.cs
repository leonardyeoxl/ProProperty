using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.Models
{
    public class Property
    {
        int property_id;
        String address;
        String block;
        String unitFloor;
        String unitNo;
        String postalCode;
        decimal latitude;
        decimal longtitude;
        int hdbTown_id;
        int asking;
        String askingPrice;
        int valuation;
        decimal built_size;
        decimal land_size;
        String type;
        DateTime datePosted;
        DateTime exclusiveDate;
        DateTime expiryDate;
    }
}