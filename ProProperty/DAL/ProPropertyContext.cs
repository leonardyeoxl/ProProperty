using System.Data.Entity;
using ProProperty.Models;

namespace ProProperty.DAL
{
    public class ProPropertyContext : DbContext
    {
        //constuctor calls the base class constructor
        //base class constructor takes a database connection string
        //its going to config that connection dynamically for us and 
        //create a database called "ExploreCaliforniaDB" when we run the app
        public ProPropertyContext() : base("ict2106_t12_testDB")
        {

        }

        //holds the tours
        //when we run app and create a new tour, it will be added to the Tours DbSet
        //and create a new database which contains a Tours table(based on Tours class)
        public DbSet<Property> Properties { get; set; }
        public DbSet<Premise> Premises { get; set; }
        public DbSet<Town> Town { get; set; }
        public DbSet<HdbPriceRange> Hdb_price_range { get; set; }
        public DbSet<PremiseType> Premises_type { get; set; }
        public DbSet<Agent> Agent { get; set; }
    }
}