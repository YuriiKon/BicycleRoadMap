using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRM.DAL.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public double Duration { get; set; }
        public double Distance { get; set; }
        public int TripCount { get; set; } = 0;

        public int StartPointId { get; set; }
        public int FinishPointId { get; set; }

        [ForeignKey("StartPointId")]
        public BicycleStation StartPoint { get; set; }
        [ForeignKey("FinishPointId")]
        public BicycleStation FinishPoint { get; set; }
    }
}
