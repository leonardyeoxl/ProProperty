using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProProperty.DAL
{
    public interface IDataGateway<T> where T : class
    {
        IEnumerable<T> getPropertyBasedOnOptions(); //returns property when user selects the options
        IEnumerable<T> getPremisesFromProperty(T obj);   //returns premise when user clicks on the property
        
    }
}
