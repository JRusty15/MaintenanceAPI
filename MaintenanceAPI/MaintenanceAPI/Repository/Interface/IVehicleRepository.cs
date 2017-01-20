using MaintenanceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceAPI.Repository.Interface
{
    public interface IVehicleRepository
    {
        Vehicle GetVehicle(int vehicleID);
        IEnumerable<Vehicle> GetMyVehicles(int userID);
        bool AddVehicle(int vehicleTypeID, string color, int driveTypeID, int userID);
        IEnumerable<int> GetYears();
        IEnumerable<string> GetMakes(int year);
        IEnumerable<Vehicle> GetModels(int year, string make);
        IEnumerable<DriveTypes> GetDrives();
    }
}
