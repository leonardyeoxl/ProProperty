using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;


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

        public IEnumerable<T> getPremisesFromProperty(T obj)
        {
            //get the property object
            //do algo to get the list of premises that are near the property
            //return the list of premises
            throw new NotImplementedException();
        }

        public T SelectById(string id)
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