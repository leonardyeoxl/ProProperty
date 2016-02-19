using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        

        IEnumerable<T> IDataGateway<T>.getPropertyBasedOnOptions()
        {
            //get the options
            //do algo based db
            //return the list of properties
            throw new NotImplementedException();
        }

        IEnumerable<T> IDataGateway<T>.getPremisesFromProperty(T obj)
        {
            //get the property object
            //do algo to get the list of premises that are near the property
            //return the list of premises
            throw new NotImplementedException();
        }
    }
}