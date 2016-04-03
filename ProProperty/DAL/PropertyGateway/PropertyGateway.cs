using ProProperty.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProProperty.DAL
{
    public class PropertyGateway : DataGateway<Property>, IPropertyGateway
    {
        public List<string> GetPropertyTypes()
        {
            List<string> types = new List<string>();
            List<Property> property = SelectAll().ToList();

            foreach(Property p in property)
            {
                if(!types.Contains(p.propertyType.ToUpper()))
                {
                    types.Add(p.propertyType.ToUpper());
                }
            }

            return types;
        }

        public List<Property> GetProperties(int townId, int minPrice, int maxPrice, int minBuiltSize, int maxBuiltSize, string propertyType)
        {
            List<Property> properties;

            properties = SelectAll().Where(
                property => property.HDBTown == townId &&
                (property.valuation >= minPrice && property.valuation <= maxPrice) &&
                (property.built_size_in_sqft >= minBuiltSize &&
                property.built_size_in_sqft <= maxBuiltSize) &&
                property.propertyType == propertyType).ToList();

            return properties;
        }
    }
}