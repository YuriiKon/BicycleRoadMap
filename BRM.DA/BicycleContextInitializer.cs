using BRM.DA.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRM.DA
{
    public class BicycleContextInitializer : DropCreateDatabaseAlways<BicycleContext>
    {
        protected override void Seed(BicycleContext db)
        {
            BicycleStation bs1 = new BicycleStation() { Latitude = 51.539569, Longitude = 46.040847, Name = "Эконом" };
            BicycleStation bs2 = new BicycleStation() { Latitude = 51.534801, Longitude = 46.037543, Name = "KFC на Радищева" };

            db.Stations.Add(bs1);
            db.Stations.Add(bs2);
            db.SaveChanges();

            Route r = new Route() { StartPoint = bs1, FinishPoint = bs2, Duration = 129.7, Distance = 606.1 };

            db.Routes.Add(r);
            db.SaveChanges();

        }
    }
}
