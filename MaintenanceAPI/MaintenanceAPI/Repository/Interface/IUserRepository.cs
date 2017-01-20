using MaintenanceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MaintenanceAPI.Repository.Interface
{
    public interface IUserRepository
    {
        MaintenanceUser Login(string username, string password);
        SystemReply UserRegistration(string username, string password, string email);
        bool UserValidation(Guid registrationID);
    }
}