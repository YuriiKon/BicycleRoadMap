using BRM.DA.Entities;
using System.Data.Entity;

namespace BRM.DA
{
    public class BicycleContext : DbContext
    {
        static BicycleContext()
        {
            Database.SetInitializer<BicycleContext>(new BicycleContextInitializer());
        }

        public BicycleContext()
            :base("DbConnetion")
        {}

        public DbSet<BicycleStation> Stations { get; set; }
        public DbSet<Route> Routes { get; set; }
    }
}
