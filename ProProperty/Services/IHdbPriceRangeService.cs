using ProProperty.Models;
using System.Collections.Generic;

namespace ProProperty.Services
{
    interface IHdbPriceRangeService
    {
        List<HdbPriceRange> GetHdbPriceRange();
    }
}
