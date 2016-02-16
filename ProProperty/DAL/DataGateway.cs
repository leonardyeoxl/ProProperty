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

        //returns property when user selects the options
        IEnumerable<T> getPropertyBasedOnOptions()
        {
            //get the options
            //do algorithm based db
            //return the array of 
            throw new NotImplementedException();
        }

        //returns premise when user clicks on the property
        IEnumerable<T> IDataGateway<T>.getPremisesFromProperty(T obj)
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IDataGateway<T>.getPropertyBasedOnOptions()
        {
            throw new NotImplementedException();
        }
    }
}