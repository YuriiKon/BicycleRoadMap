using BRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BRM.BLL
{
    public class Algorithm
    {
        private readonly int maxBicycleDistance = 10000;


        private readonly static string api_key = "5b3ce3597851110001cf6248bc42568680c942a190d03a022ffb8878";
        
        public static List<BicycleStation> BicycleStations { get; set; }

        public static List<Route> WayStations { get; set; } 

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

            List<Point> allParking = GetPossibleParking(); // GET ALL BICYCLE STATIONS
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
            List<SharingParking> st = new List<SharingParking>();
            foreach(Route item in WayStations)
            {
                SharingParking sharingParking = new SharingParking();
                var sharing = BicycleStations.FirstOrDefault(x => x.Id == item.StartPointId);
                var parking = BicycleStations.FirstOrDefault(x => x.Id == item.FinishPointId);
                sharingParking.Sharing = new Point()
                {
                    Latitude = sharing.Latitude,
                    Longitude = sharing.Longitude
                };
                sharingParking.Parking = new Point()
                {
                    Latitude = parking.Latitude,
                    Longitude = parking.Longitude
                };
                sharingParking.Time = item.Duration;
                sharingParking.Distance = item.Distance;
                st.Add(sharingParking);
            }
            //st = WayStations.Select(x => new SharingParking()
            //{
            //    Sharing = new Point()
            //    {
            //        Latitude = x.StartPoint.Latitude,
            //        Longitude = x.FinishPoint.Longitude
            //    },
            //    Parking = new Point()
            //    {
            //        Latitude = x.StartPoint.Latitude,
            //        Longitude = x.FinishPoint.Longitude
            //    },
            //    Distance = x.Distance,
            //    Time = x.Duration
            //}).ToList();
            
            return st;
            
        }

        // Get all bicycle stations
        private List<Point> GetPossibleParking()
        {
            return BicycleStations.Select(x => new Point
            {
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList();
        }

        //TODO API: B to PARKING. If less than 1.5km true, otherwise false
        private bool RequestAPI(Point B, Point Parking)
        {
            var baseAddress = new Uri("https://api.openrouteservice.org");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                bool c = httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Authorization", api_key);
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                httpClient.DefaultRequestHeaders.Host = "api.openrouteservice.org";
                var client = new RestClient("https://api.openrouteservice.org/v2/directions/cycling-regular");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("content-length", "61");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("Host", "api.openrouteservice.org");
                request.AddHeader("Postman-Token", "56194758-9eba-443a-b1bd-4acb440421f2,1418ba03-d208-4e07-85ff-a0030e059712");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
                request.AddHeader("Authorization", "5b3ce3597851110001cf6248bc42568680c942a190d03a022ffb8878");
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("undefined", "{\"coordinates\":[[" + B.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                        + B.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "],["
                        + Parking.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                        + Parking.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]]}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var temp = (JObject)JsonConvert.DeserializeObject(response.Content);

                var distance = temp["routes"][0]["summary"]["distance"].Value<double>();
                var duration = temp["routes"][0]["summary"]["duration"].Value<double>();
                return distance < 1500 ? true : false;
            }
        }

        public static List<Route> AddRoutes(BicycleStation model, List<BicycleStation> allStations)
        {
            List<Route> newRoutes = new List<Route>();
            var baseAddress = new Uri("https://api.openrouteservice.org");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                bool c = httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Authorization", api_key);


                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                httpClient.DefaultRequestHeaders.Host = "api.openrouteservice.org";

                var client = new RestClient("https://api.openrouteservice.org/v2/directions/cycling-regular");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("content-length", "61");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("Host", "api.openrouteservice.org");
                request.AddHeader("Postman-Token", "56194758-9eba-443a-b1bd-4acb440421f2,1418ba03-d208-4e07-85ff-a0030e059712");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "PostmanRuntime/7.15.0");
                request.AddHeader("Authorization", "5b3ce3597851110001cf6248bc42568680c942a190d03a022ffb8878");
                request.AddHeader("Content-Type", "application/json");

                foreach (var station in allStations)
                {
                    if (station.Latitude == model.Latitude && station.Longitude == model.Longitude) continue;
                    request.AddParameter("undefined", "{\"coordinates\":[[" + model.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                        + model.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "],["
                        + station.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                        + station.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]]}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var temp = (JObject)JsonConvert.DeserializeObject(response.Content);

                    var distance = temp["routes"][0]["summary"]["distance"].Value<double>();
                    var duration = temp["routes"][0]["summary"]["duration"].Value<double>();
                    
                    newRoutes.Add(new Route() { StartPoint = model, FinishPoint = station, Distance = distance, Duration = duration });

                    request.AddParameter("undefined", "{\"coordinates\":[[" + station.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                        + station.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "],["
                        + model.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                        + model.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]]}", ParameterType.RequestBody);
                    response = client.Execute(request);
                    temp = (JObject)JsonConvert.DeserializeObject(response.Content);

                    distance = temp["routes"][0]["summary"]["distance"].Value<double>();
                    duration = temp["routes"][0]["summary"]["duration"].Value<double>();

                    newRoutes.Add(new Route() { StartPoint = station, FinishPoint = model, Distance = distance, Duration = duration });
                }
            }
            return newRoutes;
        }
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

            public double Time { get; set; }

            public double Distance { get; set; }
        }

        public class Point
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
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