using MaintenanceAPI.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using MaintenanceAPI.Models;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace MaintenanceAPI.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private static string m_ConnectionString;
        public VehicleRepository()
        {
            m_ConnectionString = ConfigurationManager.ConnectionStrings["MaintenanceDB"].ConnectionString;
        }

        public bool AddVehicle(int vehicleTypeID, string color, int driveTypeID, int userID)
        {
            var response = false;
            using (var connection = new SqlConnection(m_ConnectionString))
            {
                int vehicleID = connection.Query<int>(@"INSERT INTO [Maintenance].[dbo].[Vehicles]
                                       ([VehicleTypeID]
                                       ,[Color]
                                       ,[DriveTypeID])
                                 VALUES
                                       (@VehicleTypeID
                                       ,@Color
                                       ,@DriveTypeID);
                                 SELECT CAST(SCOPE_IDENTITY() as int)", 
                                       new { VehicleTypeID = vehicleTypeID, Color = color, DriveTypeID = driveTypeID }).Single();

                if(vehicleID > 0)
                {
                    connection.Execute(@"INSERT INTO [Maintenance].[dbo].[UserVehicles]
                                        ([UserID]
                                        ,[VehicleID])
                                        VALUES
                                        (@UserID
                                        ,@VehicleID)",
                                        new { UserID = userID, VehicleID = vehicleID });

                    response = true;
                }
            }

            return response;
        }

        public IEnumerable<DriveTypes> GetDrives()
        {
            List<DriveTypes> driveTypes = null;
            using (var connection = new SqlConnection(m_ConnectionString))
            {
                driveTypes = connection.Query<DriveTypes>(@"SELECT [DriveTypeID]
                                                                  ,[DriveType]
                                                              FROM [Maintenance].[dbo].[DriveTypes]").ToList();
            }
            return driveTypes;
        }

        public IEnumerable<string> GetMakes(int year)
        {
            List<string> makes = null;
            using (var connection = new SqlConnection(m_ConnectionString))
            {
                makes = connection.Query<string>(@"SELECT DISTINCT [Make]
                                                   FROM [Maintenance].[dbo].[VehicleTypes]
                                                   WHERE Year = @Year", new { Year = year }).ToList();
            }
            return makes;
        }

        public IEnumerable<Vehicle> GetModels(int year, string make)
        {
            List<Vehicle> models = null;
            using (var connection = new SqlConnection(m_ConnectionString))
            {
                var results = connection.Query<Vehicle>(@"SELECT *
                                                   FROM [Maintenance].[dbo].[VehicleTypes]
                                                   WHERE Year = @Year AND Make = @Make", 
                                                   new { Year = year, Make = make });
                if(results != null && results.Any())
                {
                    models = results.ToList();
                }
            }
            return models;
        }

        public IEnumerable<Vehicle> GetMyVehicles(int userID)
        {
            List<Vehicle> myVehicles = null;

            using (var connection = new SqlConnection(m_ConnectionString))
            {
                var vehicles = connection.Query<Vehicle>(@"SELECT v.*
                                                          FROM [Maintenance].[dbo].[vwVehicle] v
                                                          INNER JOIN UserVehicles u
                                                          ON v.VehicleID = u.VehicleID
                                                          WHERE u.UserID = UserID", 
                                                          new { UserID = userID });

                if (vehicles.Any())
                {
                    myVehicles = vehicles.ToList();
                }
            }
            return myVehicles;
        }

        public Vehicle GetVehicle(int vehicleID)
        {
            Vehicle vehicle = null;
            using(var connection = new SqlConnection(m_ConnectionString))
            {
                var vehicles = connection.Query<Vehicle>(
                    "SELECT * FROM [dbo].[vwVehicle] WHERE VehicleID = @VehicleID", 
                    new { VehicleID = vehicleID });

                if(vehicles != null && vehicles.Count() > 0)
                {
                    vehicle = vehicles.First();
                }
            }

            return vehicle;
        }

        public IEnumerable<int> GetYears()
        {
            List<int> years = null;
            using (var connection = new SqlConnection(m_ConnectionString))
            {
                years = connection.Query<int>(@"SELECT DISTINCT Year
                                                FROM [Maintenance].[dbo].[VehicleTypes]
                                                ORDER BY Year DESC").ToList();
            }

            return years;
        }
    }
}