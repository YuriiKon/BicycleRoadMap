using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BicycleRoadMap.Controllers
{
    public class StationsController : ApiController
    {
        // GET api/stations/5
        public string Get(int startLatitude, int startLongitude, int finishLatitude, int finishLongitude)
        {
            return "value";
        }

    }
}