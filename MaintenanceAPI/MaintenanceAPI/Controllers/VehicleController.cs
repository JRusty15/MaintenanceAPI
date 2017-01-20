using MaintenanceAPI.Models;
using MaintenanceAPI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MaintenanceAPI.Controllers
{
    [RoutePrefix("Vehicle")]
    public class VehicleController : ApiController
    {
        private IVehicleRepository m_VehicleRepo { get; set; }
        public VehicleController(IVehicleRepository vehicleRepo)
        {
            m_VehicleRepo = vehicleRepo;
        }

        [HttpGet]
        [Route("Vehicle/{vehicleID}")]
        public Vehicle GetVehicle(int vehicleID)
        {
            return m_VehicleRepo.GetVehicle(vehicleID);
        }

        [HttpGet]
        [Route("MyVehicles")]
        public IEnumerable<Vehicle> MyVehicles()
        {
            return m_VehicleRepo.GetMyVehicles((User as MaintenanceUser).UserID);
        }

        [HttpPost]
        [Route("AddVehicle/{vehicleTypeID}/{color}/{driveTypeID}")]
        public bool AddVehicle(int vehicleTypeID, string color, int driveTypeID)
        {
            return m_VehicleRepo.AddVehicle(vehicleTypeID, color, driveTypeID, (User as MaintenanceUser).UserID);
        }

        [HttpGet]
        [Route("Makes/{year}")]
        public IEnumerable<string> GetMakes(int year)
        {
            return m_VehicleRepo.GetMakes(year);
        }

        [HttpGet]
        [Route("Models/{year}/{make}")]
        public IEnumerable<Vehicle> GetModels(int year, string make)
        {
            return m_VehicleRepo.GetModels(year, make);
        }

        [HttpGet]
        [Route("Years")]
        public IEnumerable<int> GetYears()
        {
            return m_VehicleRepo.GetYears();
        }

        [HttpGet]
        [Route("Drives")]
        public IEnumerable<DriveTypes> GetDrives()
        {
            return m_VehicleRepo.GetDrives();
        }
    }
}
