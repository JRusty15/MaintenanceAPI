using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaintenanceAPI.Models
{
    public class UserRegistration
    {
        public Guid RegistrationID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Validated { get; set; }
    }
}