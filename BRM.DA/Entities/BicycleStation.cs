using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace BRM.DA.Entities
{
    public class BicycleStation
    {
        public int Id { get; set; }
        public SqlGeography Location { get; set; }
        public string Name { get; set; }
    }
}
