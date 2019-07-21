using BRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRM.DAL
{
    public class BicycleContextInitializer : DropCreateDatabaseAlways<BicycleContext>
    {
        protected override void Seed(BicycleContext db)
        {
            BicycleStation bs1 = new BicycleStation() { Latitude = 51.548678, Longitude = 46.007026, Name = "Эконом" };
            BicycleStation bs2 = new BicycleStation() { Latitude = 51.540841, Longitude = 46.013070, Name = "KFC на Радищева" };
            //BicycleStation bs3 = new BicycleStation() { Latitude = 51.527946, Longitude = 46.000740, Name = "3" };
            //BicycleStation bs4 = new BicycleStation() { Latitude = 51.520398, Longitude = 46.002032, Name = "4" };
            //BicycleStation bs5 = new BicycleStation() { Latitude = 51.516085, Longitude = 46.006580, Name = "5" };
            //BicycleStation bs6 = new BicycleStation() { Latitude = 51.520278, Longitude = 46.013876, Name = "6" };
            //BicycleStation bs7 = new BicycleStation() { Latitude = 51.528631, Longitude = 46.018895, Name = "7" };
            //BicycleStation bs8 = new BicycleStation() { Latitude = 51.525592, Longitude = 46.042263, Name = "8" };
            //BicycleStation bs9 = new BicycleStation() { Latitude = 51.529663, Longitude = 46.061768, Name = "9" };
            //BicycleStation bs10 = new BicycleStation() { Latitude = 51.534801, Longitude = 46.037543, Name = "10" };


            db.Stations.Add(bs1);
            db.Stations.Add(bs2);
            //db.Stations.Add(bs3);
            //db.Stations.Add(bs4);
            //db.Stations.Add(bs5);
            //db.Stations.Add(bs6);
            //db.Stations.Add(bs7);
            //db.Stations.Add(bs8);
            //db.Stations.Add(bs9);
            //db.Stations.Add(bs10);
            db.SaveChanges();

            Route r1 = new Route() { StartPoint = bs1, FinishPoint = bs2, Duration = 141.3, Distance = 652.7 };
            Route r2 = new Route() { StartPoint = bs2, FinishPoint = bs1, Duration = 129.7, Distance = 606.1 };

            db.Routes.Add(r1);
            db.Routes.Add(r2);
            db.SaveChanges();

        }
    }
}
