using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRM.DA.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public double Duration { get; set; }
        public double Distance { get; set; }

        public int StartPointId { get; set; }
        public int FinishPointId { get; set; }

        public BicycleStation StartPoint { get; set; }
        public BicycleStation FinishPoint { get; set; }
    }
}
