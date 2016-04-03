using ProProperty.Models;
using System.Collections.Generic;

namespace ProProperty.DAL
{
    interface IHdbPriceRangeGateway : IDataGateway<HdbPriceRange>
    {
        List<HdbPriceRange> hdbPriceRangeQuery(string district, string room);
        void DeleteAllHdbPriceRange();
    }
}
