using ProProperty.Models;
using System.Collections.Generic;

namespace ProProperty.Controllers
{
    interface IPropertyController
    {
        void addProperty(PropertyWithPremises property);
        void clearListProperty();
        IEnumerable<PropertyWithPremises> getAllProperties();
    }
}
