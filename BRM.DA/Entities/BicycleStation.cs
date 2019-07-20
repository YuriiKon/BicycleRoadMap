using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace BRM.DAL.Entities
{
    public class BicycleStation
    {
        public int Id { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        public string Name { get; set; }

        public ICollection<Route> Routes { get; set; }

        public BicycleStation()
        {
            Routes = new List<Route>();
        }
    } 
}
