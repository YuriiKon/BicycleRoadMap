using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRM.BLL
{
    public class Algorithm
    {
        private readonly int maxBicycleDistance = 10;

        /// <summary>
        /// A is start
        /// B is End
        /// </summary>
        public Way Location { get; set; }

        /// <summary>
        /// A is Sharing
        /// B is Parking
        /// </summary>
        public Way Sharing { get; private set; }

        /// <summary>
        /// Create object
        /// </summary>
        /// <param name="location">A and B coords</param>
        public Algorithm(Way location)
        {
            Location = location;
        }

        public AllWay Print()
        {
            Sharing = GetBicycleWay();
            if (Sharing != null)
            {
                return new AllWay(Location, Sharing);
            }
            else
            {
                return new AllWay(Location, new Way { A = null, B = null }); //best route is not available
            }
        }

        /// <summary>
        /// Get the shortest A-S-P-B way. Less than 15km
        /// Minimum P-B way and minimum P-S way
        /// Return best S-P way
        /// </summary>
        public Way GetBicycleWay()
        {
            List<SharingParking> allPoints = GetAllSP(); //TODO: GET ALL SHARING-PARKING POINTS TABLE
            List<Point> allParking = GetPossibleParking(); //TODO: GET ALL BICYCLE STATIONS

            List<Point> possibleParking = new List<Point>(); //POSSIBLE PARKING POINTS (< 1.5km). HIGH PRORITY 
            List<SharingParking> possiblePoints = new List<SharingParking>(); //GET ALL POSSIBLE SHARING-PARKING POINTS
            foreach (var item in allParking)
            {
                if (RequestAPI(Location.B, item)) // Less than 1.5km
                {
                    possibleParking.Add(item);
                }
            }
            SharingParking best = null;
            foreach (var item in possibleParking)
            {
                var points = allPoints.Where(x => x.Parking.Latitude == item.Latitude
                    && x.Parking.Longitude == item.Longitude && x.Distance < maxBicycleDistance).ToList();
                if (points.Count > 0)
                {
                    foreach (var way in points)
                    {
                        if (best != null)
                        {
                            if (best.Distance > way.Distance)
                            {
                                best = way;
                            }
                        }
                        else
                        {
                            best = way;
                        }
                    }
                }
            }
            return best != null ?
                new Way()
                {
                    A = best.Sharing,
                    B = best.Parking
                } :
                null;
        }

        //TODO Get all sharing points. Directed graph (n*(n-1))-nodes
        private List<SharingParking> GetAllSP()
        {
            return new List<SharingParking>
            {
                new SharingParking()
                {
                    Sharing = new Point()
                    {
                        Latitude = "51.548678",
                        Longitude = "46.007026"
                    },
                    Parking = new Point()
                    {
                        Latitude = "51.540841",
                        Longitude = "46.013070"
                    },
                    Distance = 10,
                    Time = new TimeSpan(0, 10, 0)
                },
                new SharingParking()
                {
                    Sharing = new Point()
                    {
                        Latitude = "51.548678",
                        Longitude = "46.007026"
                    },
                    Parking = new Point()
                    {
                        Latitude = "51.527946",
                        Longitude = "46.000740"
                    },
                    Distance = 15,
                    Time = new TimeSpan(0, 25, 0)
                },
                new SharingParking()
                {
                    Sharing = new Point()
                    {
                        Latitude = "51.527946",
                        Longitude = "46.000740"
                    },
                    Parking = new Point()
                    {
                        Latitude = "51.527946",
                        Longitude = "46.000740"
                    },
                    Distance = 5,
                    Time = new TimeSpan(0, 10, 0)
                },

                new SharingParking()
                {
                    Sharing = new Point()
                    {
                        Latitude = "51.540841",
                        Longitude = "46.013070"
                    },
                    Parking = new Point()
                    {
                        Latitude = "51.548678",
                        Longitude = "46.007026"
                    },
                    Distance = 10,
                    Time = new TimeSpan(0, 10, 0)
                },
                new SharingParking()
                {
                    Sharing = new Point()
                    {
                        Latitude = "51.527946",
                        Longitude = "46.000740"
                    },
                    Parking = new Point()
                    {
                        Latitude = "51.548678",
                        Longitude = "46.007026"
                    },
                    Distance = 17,
                    Time = new TimeSpan(0, 28, 0)
                },
                new SharingParking()
                {
                    Sharing = new Point()
                    {
                        Latitude = "51.527946",
                        Longitude = "46.000740"
                    },
                    Parking = new Point()
                    {
                        Latitude = "51.527946",
                        Longitude = "46.000740"
                    },
                    Distance = 5,
                    Time = new TimeSpan(0, 10, 0)
                }
            };
        }

        //TODO Get all bicycle stations
        private List<Point> GetPossibleParking()
        {
            return new List<Point>()
            {
                new Point()
                {
                    Latitude = "51.540841",
                    Longitude = "46.013070"
                },
                new Point()
                {
                    Latitude = "51.548678",
                    Longitude = "46.007026"
                },
                new Point()
                {
                    Latitude = "51.527946",
                    Longitude = "46.000740"
                }
            };
        }

        //TODO API: B to PARKING. If less than 1.5km true, otherwise false
        private bool RequestAPI(Point B, Point Parking) { return true; }

    }

    public class Way
    {
        public Point A { get; set; }

        public Point B { get; set; }
    }

    public class SharingParking
    {
        public Point Sharing { get; set; }

        public Point Parking { get; set; }

        public TimeSpan Time { get; set; }

        public double Distance { get; set; }
    }

    public class Point
    {
        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }

    public class AllWay : Way
    {
        public Point Sharing { get; set; }

        public Point Parking { get; set; }

        public AllWay(Way Location, Way BicycleStations)
        {
            A = Location.A;
            B = Location.B;
            Sharing = BicycleStations.A;
            Parking = BicycleStations.B;
        }
    }
}
//new Point()
//{
//    Latitude = "51.548678",
//    Longitude = "46.007026"
//},
//new Point()
//{
//    Latitude = "51.540841",
//    Longitude = "46.013070"
//},
//new Point()
//{
//    Latitude = "51.527946",
//    Longitude = "46.000740"
//},
//new Point()
//{
//    Latitude = "51.520398",
//    Longitude = "46.002032"
//},
//new Point()
//{
//    Latitude = "51.516085",
//    Longitude = "46.006580"
//},
//new Point()
//{
//    Latitude = "51.520278",
//    Longitude = "46.013876"
//},
//new Point()
//{
//    Latitude = "51.528631",
//    Longitude = "46.018895"
//},
//new Point()
//{
//    Latitude = "51.525592",
//    Longitude = "46.042263"
//},
//new Point()
//{
//    Latitude = "51.529663",
//    Longitude = "46.061768"
//}