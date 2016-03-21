using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace ProProperty.DAL
{
    public class DataGateway<T> : IDataGateway<T> where T : class
    {
        internal ProPropertyContext db = new ProPropertyContext();
        internal DbSet<T> data = null;

        public DataGateway()
        {
            this.data = db.Set<T>();
        }
        
        public IEnumerable<T> SelectAll()
        {
            return data;
        }

        public T SelectById(int? id)
        {
            T obj = data.Find(id);
            return obj;
        }

        public void Insert(T obj)
        {
            data.Add(obj);
            db.SaveChanges();

        }        
    }
}