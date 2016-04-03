using ProProperty.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProProperty.DAL
{
    public class HdbPriceRangeGateway : DataGateway<HdbPriceRange>, IHdbPriceRangeGateway
    {
        public List<HdbPriceRange> hdbPriceRangeQuery(string district, string room)
        {
            return data.Where(p => p.town == district && p.room_type == room).ToList();
        }

        public void DeleteAllHdbPriceRange()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Hdb_price_range]");
        }
    }
}