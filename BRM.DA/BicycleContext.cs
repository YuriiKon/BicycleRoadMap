using BRM.DAL.Entities;
using System.Data.Entity;

namespace BRM.DAL
{
    public class BicycleContext : DbContext
    {
        static BicycleContext()
        {
            Database.SetInitializer(new BicycleContextInitializer());
        }

        public BicycleContext()
            : base("DbConnection") { }

        public DbSet<BicycleStation> Stations { get; set; }
        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Route>()
                .HasRequired(s => s.FinishPoint).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Route>()
                .HasRequired(s => s.StartPoint).WithMany().WillCascadeOnDelete(false);
            

        }
    }
}
