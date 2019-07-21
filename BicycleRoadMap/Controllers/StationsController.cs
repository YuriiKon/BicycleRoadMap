using AutoMapper;
using BicycleRoadMap.Models;
using BRM.BLL;
using BRM.BLL.DTOs;
using BRM.BLL.Interfaces;
using BRM.BLL.Services;
using BRM.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BicycleRoadMap.Controllers
{
    [RoutePrefix("api/stations")]
    public class StationsController : ApiController
    {
        private readonly IStationService _service;
        //private readonly IMapper _mapper;

        public StationsController(IStationService service/*, IMapper mapper*/)
        {
            _service = service;
            //_mapper = mapper;
        }

        // GET api/stations
        [HttpGet]
        public async Task<AllWay> Get(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude)
        {
            return await _service.GetStations(startLatitude, startLongitude,  finishLatitude,  finishLongitude);
        }

        // GET api/stations/GetAll
        [HttpGet]
        [Route("GetAll")]
        public List<BicycleStation> GetAllStations()
        {
            return _service.GetAllStations();
        }

        [HttpGet]
        public List<Statistics> GetStatistics()
        {
            return _service.GetStatistics()
                .Take(10)
                .OrderBy(x => x.Count)
                .ToList();
        }

        // GET api/stations/5
        [HttpPost]
        public async Task<IHttpActionResult> AddStation(BicycleStation model)
        {
            
            await _service.AddStation(model);
            
            return Ok();
        }

        [HttpPost]
        [Route("AddListStations")]
        public async Task<IHttpActionResult> AddStations(List<BicycleStation> model)
        {

            await _service.AddListStations(model);

            return Ok();
        }

    }
}