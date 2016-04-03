using ProProperty.Models;
using System.Collections.Generic;

namespace ProProperty.DAL
{
    interface IPropertyGateway : IDataGateway<Property>
    {
        List<string> GetPropertyTypes();
        List<Property> GetProperties(int townId, int minPrice, int maxPrice, int minBuiltSize, int maxBuiltSize, string propertyType);
    }
}
