using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaintenanceAPI.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string Color { get; set; }
        public string DriveType { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}