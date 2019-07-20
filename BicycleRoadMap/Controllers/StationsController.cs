using AutoMapper;
using BicycleRoadMap.Models;
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
    public class StationsController : ApiController
    {
        private readonly IStationService _service;
        //private readonly IMapper _mapper;

        public StationsController(IStationService service/*, IMapper mapper*/)
        {
            _service = service;
            //_mapper = mapper;
        }

        // GET api/stations/5
        [HttpGet]
        public string Get(int startLatitude, int startLongitude, int finishLatitude, int finishLongitude)
        {
            return "value";
        }

        // GET api/stations/5
        [HttpPost]
        public async Task<IHttpActionResult> AddStation(BicycleStation model)
        {
            
            await _service.AddStation(model);
            
            return Ok();
        }


    }
}