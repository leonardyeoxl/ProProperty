using ProProperty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProProperty.DAL
{
    public class HdbPriceRangeGateway : DataGateway<Hdb_price_range>
    {
        public List<Hdb_price_range> hdbPriceRangeQuery(string district, string room)
        {
            return data.Where(p => p.town == district && p.room_type == room).ToList();
        }
    }
}