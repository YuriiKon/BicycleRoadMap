using AutoMapper;
using BRM.BLL.DTOs;
using BRM.BLL.Interfaces;
using BRM.DAL;
using BRM.DAL.Entities;
using System.Threading.Tasks;

namespace BRM.BLL.Services
{
    public class StationService : IStationService
    {
        private readonly BicycleContext _db;
        //private readonly IMapper _mapper;

        public StationService(BicycleContext db/*, IMapper mapper*/)
        {
            _db = db;
            //_mapper = mapper;
        }

        public async Task AddStation(BicycleStation model)
        {
            _db.Stations.Add(model);
            await _db.SaveChangesAsync();
        }
    }
}
