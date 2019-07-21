using BRM.BLL.DTOs;
using BRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRM.BLL.Interfaces
{
    public interface IStationService
    {
        Task AddStation(BicycleStation model);
        List<BicycleStation> GetAllStations();
        Task<List<BicycleStation>> GetStations(int startLatitude, int startLongitude, int finishLatitude, int finishLongitude);
    }
}
