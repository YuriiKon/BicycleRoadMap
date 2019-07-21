using AutoMapper;
using BRM.BLL.DTOs;
using BRM.BLL.Interfaces;
using BRM.DAL;
using BRM.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BRM.BLL.Services
{
    public class StationService : IStationService
    {
        private readonly BicycleContext _db;
        //private readonly IMapper _mapper;

        public StationService(BicycleContext db/*, IMapper mapper*/)
        {
            _db = db;
            Algorithm.BicycleStations = GetAllStations();
            Algorithm.WayStations = GetAllRoutes();

            //_mapper = mapper;
        }

        public async Task AddStation(BicycleStation model)
        {
            var entity = _db.Stations.Add(model);
            await _db.SaveChangesAsync();

            var routes =  Algorithm.AddRoutes(entity, GetAllStations());
            _db.Routes.AddRange(routes);

            await _db.SaveChangesAsync();
        }

        public async Task AddListStations(List<BicycleStation> models)
        {
            foreach (var model in models)
            {
                await AddStation(model);
            }
        }

        public List<BicycleStation> GetAllStations()
        {
            return _db.Stations.Select(s => s).ToList(); 
        }

        public BicycleStation GetStation(int id)
        {
            return _db.Stations.FirstOrDefault(x => x.Id == id);
        }

        public List<Route> GetAllRoutes()
        {
            return _db.Routes.Select(s => s).ToList();
        }

        public async Task<AllWay> GetStations(double startLongitude, double startLatitude, double finishLatitude, double finishLongitude)
        {
            Algorithm algorithm = new Algorithm(new Way() {
                A = new Point() { Latitude = startLatitude, Longitude = startLongitude },
                B = new Point() { Latitude = finishLatitude, Longitude = finishLongitude }
            });
            AllWay way = algorithm.Print();
            if(way.Sharing != null && way.Parking != null)
            {
                var route = _db.Routes.FirstOrDefault(x => x.StartPoint.Latitude == way.Sharing.Latitude && x.StartPoint.Longitude == way.Sharing.Longitude
                    && x.FinishPoint.Latitude == way.Parking.Latitude && x.FinishPoint.Longitude == way.Parking.Longitude);
                route.TripCount++;
                _db.Entry(route);
                await _db.SaveChangesAsync();
            }
            return way;
        }

        public List<Statistics> GetStatistics()
        {
            return GetAllRoutes().Select(x => new Statistics()
            {
                A = new Point()
                {
                    Latitude = x.StartPoint.Latitude,
                    Longitude = x.StartPoint.Longitude
                },
                B = new Point()
                {
                    Latitude = x.FinishPoint.Latitude,
                    Longitude = x.FinishPoint.Longitude
                },
                Count = x.TripCount
            }).ToList();
        }
    }
}
