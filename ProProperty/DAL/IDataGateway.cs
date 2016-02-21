using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProProperty.Models;

namespace ProProperty.DAL
{
    interface IDataGateway<T> where T : class
    {
        IEnumerable<T> SelectAll(); //returns property when user selects the options
        IEnumerable<T> getPremisesFromProperty(T obj);   //returns premise when user clicks on the property
        T SelectById(int? id);

    }
}
