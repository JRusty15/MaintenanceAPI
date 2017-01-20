using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaintenanceAPI.Models.InputModels
{
    public class UserRegistrationInput
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}