using System.Collections.Generic;

namespace ProProperty.DAL
{
    interface IDataGateway<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T SelectById(int? id);
        void Insert(T obj);
    }
}
