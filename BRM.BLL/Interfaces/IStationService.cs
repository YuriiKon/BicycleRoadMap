using BRM.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BRM.BLL.Interfaces
{
    public interface IStationService
    {
        Task AddStation(BicycleStation model);
        List<BicycleStation> GetAllStations();
        Task<AllWay> GetStations(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude);
        Task AddListStations(List<BicycleStation> models);
        BicycleStation GetStation(int id);
        List<Route> GetAllRoutes();
        List<Statistics> GetStatistics();
    }
}
