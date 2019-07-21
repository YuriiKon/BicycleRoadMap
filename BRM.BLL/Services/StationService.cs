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
            Algorithm.WayStations = GetAllStations();

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

        public async Task<List<BicycleStation>> GetStations(int startLongitude, int startLatitude, int finishLatitude, int finishLongitude)
        {
            return _db.Stations.Select(s => s).ToList();
        }
    }
}
